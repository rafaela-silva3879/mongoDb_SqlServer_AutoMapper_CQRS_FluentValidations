using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces.Cache;
using DeveloperStore.Domain.Interfaces.Repositories;
using DeveloperStore.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Domain.Services
{
    public class SaleDomainService : ISaleDomainService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ISaleItemDomainService _saleItemDomainService;
        private readonly ISaleCache _saleCache;
        private readonly ISaleItemCache _saleItemCache;
        private readonly IProductCache _productCache;
        public SaleDomainService(IUnitOfWork unitOfWork,
            ISaleItemDomainService saleItemDomainService,
            ISaleCache saleCache,
            ISaleItemCache saleItemCache,
            IProductCache productCache)
        {
            _unitOfWork = unitOfWork;
            _saleItemDomainService = saleItemDomainService;
            _saleCache = saleCache;
            _saleItemCache = saleItemCache;
            _productCache = productCache;
        }

        public async Task AddAsync(Sale entity)
        {
            try
            {
                entity.Id = Guid.NewGuid().ToString().ToLower();
                await _unitOfWork.SaleRepository.AddAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task DeleteAsync(Sale entity)
        {
            try
            {
                await _unitOfWork.SaleRepository.DeleteAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<Sale>> GetAllAsync()
        {
            try
            {
                var lista = await _unitOfWork.SaleRepository.GetAllAsync();
                return lista;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<Sale> GetByIdAsync(string id)
        {
            try
            {
                var s = await _unitOfWork.SaleRepository.GetByIdAsync(id);
                return s;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task UpdateAsync(Sale entity)
        {
            try
            {
                await _unitOfWork.SaleRepository.UpdateAsync(entity);
            }
            catch (Exception)
            {

                throw;
            }
        }

       public async Task<Sale> MakeSale(List<SaleItem> saleItems, string userId)
        {
            if (saleItems == null || !saleItems.Any())
                throw new ArgumentException("The sale must contain at least one item.");

            _unitOfWork.BeginTransaction();
            try
            {
                var sale = new Sale
                {
                    Id=Guid.NewGuid().ToString().ToLower(),
                    SaleDate = DateTime.UtcNow,
                    UserId = userId,
                    IsCancelled = false,
                    TotalSaleAmount = 0
                };
                var newSaleItemsList = new List<SaleItem>();
                foreach (var item in saleItems)
                {
                    var saleItem = new SaleItem
                    {
                        Quantity = item.Quantity,
                        ProductId = item.ProductId
                    };

                    var saleItemAttributes = await _saleItemDomainService.CalculateTotalItemAmountCommand(item);
                    saleItem.TotalItemAmount = saleItemAttributes.TotalItemAmount;
                    saleItem.Discount = saleItemAttributes.Discount;

                    var productsInStock = await _unitOfWork.ProductRepository.GetAsync(p => p.Id.Equals(item.ProductId));
                    if (item.Quantity > productsInStock.Quantity)
                        throw new Exception($"There is no sufficient stock for {productsInStock.Name}.");

                    productsInStock.Quantity = productsInStock.Quantity - item.Quantity;
                    await _unitOfWork.ProductRepository.UpdateAsync(productsInStock);
                    await _productCache.UpdateAsync(productsInStock);

                    sale.TotalSaleAmount += saleItemAttributes.TotalItemAmount;
                    newSaleItemsList.Add(saleItem);
                    
                }

                await _unitOfWork.SaleRepository.AddAsync(sale);
                await _saleCache.AddAsync(sale);

                foreach (var saleItem in newSaleItemsList)
                {
                    saleItem.SaleId = sale.Id;
                    saleItem.Id=Guid.NewGuid().ToString().ToLower();
                    await _unitOfWork.SaleItemRepository.AddAsync(saleItem); // Add saleItem instead of item
                    await _saleItemCache.AddAsync(saleItem);
                }

                _unitOfWork.Commit();
                sale.SaleItems = new List<SaleItem>();
                sale.User= await _unitOfWork.UserRepository.GetByIdAsync(sale.UserId);
                sale.SaleItems = await _unitOfWork.SaleItemRepository.GetAllAsync(si => si.SaleId==sale.Id);
               
                foreach(var item in sale.SaleItems)
                {
                    item.Product=new Product();
                    item.Product =await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                }
                
                return sale;
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}