using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BirFikrimVar.Entity.Models;




namespace BirFikrimVar.DataAccess.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Like { get; set; }
        public DbSet<CommentLike> CommentLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Like>()
            .HasOne(l => l.Idea)
            .WithMany(i => i.Likes)
            .HasForeignKey(l => l.IdeaId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();


            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Idea)
                .WithMany(i => i.Comments)
                .HasForeignKey(c => c.IdeaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Like>()
                .HasIndex(l => new { l.IdeaId, l.UserId })
                .IsUnique();

            // Aynı yorumu aynı kullanıcı yalnızca 1 kez beğenebilsin
            modelBuilder.Entity<CommentLike>()
                .HasIndex(cl => new { cl.CommentId, cl.UserId })
                .IsUnique();

            // CommentLike -> Comment (Cascade)
            modelBuilder.Entity<CommentLike>()
                .HasOne<Comment>(x => x.Comment)
                .WithMany(c => c.Likes)// Comment’e Likes koleksiyonu ekle
                .HasForeignKey(x => x.CommentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CommentLike>()
                .HasOne<User>(x => x.User)
                .WithMany(u => u.CommentLikes)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            
        }



    }
}
