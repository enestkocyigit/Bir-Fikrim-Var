using BirFikrimVar.Business.Managers;
using BirFikrimVar.Business.Services;
using BirFikrimVar.DataAccess.Context;
using BirFikrimVar.DataAccess.Repositories;
using BirFikrimVar.Business.Mapping;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using BirFikrimVar.Business.Validation;
using AutoMapper;




var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IIdeaService, IdeaManager>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddScoped<ILikeService, LikeManager>();
builder.Services.AddScoped<ICommentService, CommentManager>();
builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
    
    options.UseSqlServer(
        "Server=DESKTOP-6ALKE60;Database=BirFikrimVarDb;Trusted_Connection=True;TrustServerCertificate=True;",
        b => b.MigrationsAssembly("BirFikrimVar.DataAccess")  
    ));


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, config =>
    {
        config.LoginPath = "/Account/Login";
        config.LogoutPath = "/Account/Logout";
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var users = ctx.Users.ToList();

    foreach (var u in users)
    {
        var p = u.Password ?? "";
        // BCrypt hash ise $2 ile baþlar. Deðilse hash'le.
        if (!p.StartsWith("$2a$") && !p.StartsWith("$2b$"))
        {
            u.Password = BCrypt.Net.BCrypt.HashPassword(p);
        }
    }

    await ctx.SaveChangesAsync();
}

app.Run();
