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
    public class SaleItemDomainService : ISaleItemDomainService
    {
        private readonly IUnitOfWork? _unitOfWork;
        public SaleItemDomainService(IUnitOfWork? unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddAsync(SaleItem entity)
        {
            try
            {
                entity.Id = Guid.NewGuid().ToString().ToLower();
                await _unitOfWork.SaleItemRepository.AddAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(SaleItem entity)
        {
            try
            {
               await _unitOfWork.SaleItemRepository.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<SaleItem>> GetAllAsync()
        {
            try
            {
                var lista =await _unitOfWork.SaleItemRepository.GetAllAsync();
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<SaleItem> GetByIdAsync(string id)
        {
            try
            {
                var s =await _unitOfWork.SaleItemRepository.GetByIdAsync(id);
                return s;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateAsync(SaleItem entity)
        {
            try
            {
               await _unitOfWork.SaleItemRepository.UpdateAsync(entity);
               
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<SaleItem> CalculateTotalItemAmountCommand(SaleItem? entity)
        {
            try
            {
                var produto = await _unitOfWork.ProductRepository.GetByIdAsync(entity.ProductId);

                // Verify the quantity of items
                decimal discount = 0;

                if (entity.Quantity > 20)
                {
                    // It is not possible sell more than 20 items per sale
                    throw new InvalidOperationException("Não é possível vender mais de 20 itens iguais.");
                }
                else if (entity.Quantity > 4)
                {
                    // 10% discout for shopping with more than 4 items
                    discount = 0.10m;
                }
                else if (entity.Quantity >= 10)
                {
                    // 20% descount between 10 e 20 items
                    discount = 0.20m;
                }
   

                // Calculate total item with no discount
                var totalItemAmount = entity.Quantity * produto.UnitPrice;

                // Apply discount
                entity.Discount = totalItemAmount * discount;

                // Calcula o total do item com desconto aplicado
                entity.TotalItemAmount = totalItemAmount - entity.Discount;
               
                return entity;
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
