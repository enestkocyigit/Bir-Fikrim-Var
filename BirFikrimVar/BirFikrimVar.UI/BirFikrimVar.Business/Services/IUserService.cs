using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Entity.Models;

namespace BirFikrimVar.Business.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAndPasswordAsync(string email, string password);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);

        Task AddUserAsync(User user);
        void UpdateUser(User user);
        void DeleteUser(User user);
    }
}
