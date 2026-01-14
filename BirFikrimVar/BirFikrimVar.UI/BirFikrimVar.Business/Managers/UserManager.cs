using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BirFikrimVar.Business.Services;
using BirFikrimVar.DataAccess.Repositories;
using BirFikrimVar.Entity.Models;

namespace BirFikrimVar.Business.Managers
{
    public class UserManager : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;

        public UserManager(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync() { 
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id) { 
            return await _userRepository.GetByIdAsync(id);
        }
        public async Task<User?> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            var users = await _userRepository.FindAsync(u => u.Email == email && u.Password == password);
            return users.FirstOrDefault();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            var list = await _userRepository.FindAsync(u => u.Email == email);
            return list.FirstOrDefault();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var exists = await _userRepository.FindAsync(u => u.Email == email);
            return exists.Any();
        }

        public async Task AddUserAsync(User user) {

            var exists = await EmailExistsAsync(user.Email);
            if (exists)
                throw new InvalidOperationException("Bu e-posta ile zaten bir hesap var.");

            await _userRepository.AddAsync(user);
        }

        public void UpdateUser(User user) {
            _userRepository.Update(user);
        }

        public void DeleteUser(User user) {
            _userRepository.Delete(user);
        }
    }
}
