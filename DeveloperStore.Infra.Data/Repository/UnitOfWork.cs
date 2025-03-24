using DeveloperStore.Domain.Interfaces.Repositories;
using DeveloperStore.Infra.Data.Contexts;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeveloperStore.Infra.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext? _dataContext;
        private IDbContextTransaction? _transaction;


        private IUserRepository? _userRepository;
        private IProductRepository? _productRepository;
        private ISaleRepository? _saleRepository;
        private ISaleItemRepository? _saleItemRepository;

        public UnitOfWork(DataContext? dataContext)
        {
            _dataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext)); // Garantir que o DataContext não seja nulo
        }

        public void BeginTransaction()
        {
            if (_transaction != null)
                throw new InvalidOperationException("A transação já foi iniciada.");

            _transaction = _dataContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction == null)
                throw new InvalidOperationException("A transação não foi iniciada.");

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public void Rollback()
        {
            if (_transaction == null)
                throw new InvalidOperationException("A transação não foi iniciada.");

            _transaction.Rollback();
            _transaction.Dispose();
            _transaction = null;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                    _userRepository = new UserRepository(_dataContext);

                return _userRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                    _productRepository = new ProductRepository(_dataContext);

                return _productRepository;
            }
        }

        public ISaleRepository SaleRepository
        {
            get
            {
                if (_saleRepository == null)
                    _saleRepository = new SaleRepository(_dataContext);

                return _saleRepository;
            }
        }

        public ISaleItemRepository SaleItemRepository
        {
            get
            {
                if (_saleItemRepository == null)
                    _saleItemRepository = new SaleItemRepository(_dataContext);

                return _saleItemRepository;
            }
        }

        public void Dispose()
        {
            _dataContext?.Dispose();
            _transaction?.Dispose();
        }
    }
}
