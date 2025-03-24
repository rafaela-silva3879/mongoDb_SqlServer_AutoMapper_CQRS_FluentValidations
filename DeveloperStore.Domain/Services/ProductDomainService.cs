using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Repositories;
using DeveloperStore.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Services
{
    public class ProductDomainService : IProductDomainService
    {
        private readonly IUnitOfWork? _unitOfWork;
        public ProductDomainService(IUnitOfWork? unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(Product entity)
        {
            try
            {
                entity.Id = Guid.NewGuid().ToString().ToLower();
               await _unitOfWork.ProductRepository.AddAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(Product entity)
        {
            try
            {
                await _unitOfWork.ProductRepository.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Product>> GetAllAsync()
        {
            try
            {
                var lista = await _unitOfWork.ProductRepository.GetAllAsync();
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            try
            {
                var p =await _unitOfWork.ProductRepository.GetByIdAsync(id);
                return p;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateAsync(Product entity)
        {
            try
            {
               await _unitOfWork.ProductRepository.UpdateAsync(entity);               
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
