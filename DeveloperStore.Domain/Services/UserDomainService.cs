using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Repositories;
using DeveloperStore.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Services
{
    public class UserDomainService : IUserDomainService
    {
        private readonly IUnitOfWork? _unitOfWork;
        public UserDomainService(IUnitOfWork? unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(User entity)
        {
            try
            {
                entity.Id = Guid.NewGuid().ToString().ToLower();
                await _unitOfWork.UserRepository.AddAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(User entity)
        {
            try
            {
                await _unitOfWork.UserRepository.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<User>> GetAllAsync()
        {
            try
            {
                var lista = await _unitOfWork.UserRepository.GetAllAsync();
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> GetByIdAsync(string id)
        {
            try
            {
                var c = await _unitOfWork.UserRepository.GetByIdAsync(id);
                return c;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateAsync(User entity)
        {
            try
            {    
               await _unitOfWork.UserRepository.UpdateAsync(entity);          
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> ValidateCredentialsAsync(string email, string password)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(u => u.Email == email && u.Password == password);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(u => u.Email == email);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<User> GetUserByTokenAsync(string token)
        {
            try
            {
                var user = await _unitOfWork.UserRepository.GetAsync(u => u.PasswordResetToken == token);
                return user;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void Dispose()
        {
            _unitOfWork?.Dispose();
        }

    


    }
}
