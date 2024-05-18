﻿using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Contexts;
using POS.Infraestructure.Persistences.Interfaces;
using System.Data;

namespace POS.Infraestructure.Persistences.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly POSContext _context;

        public IUserRepository _user = null!;

        public IWarehouseRepository _warehouse = null!;

        public IProductStockRepository _productStock = null!;

        public IPurchaseDetailRepository _purchaseDetail = null!;

        public IGenericRepository<Category> _category = null!;

        public IGenericRepository<Provider> _provider = null!;

        public IGenericRepository<DocumentType> _documentType = null!;

        public IGenericRepository<Product> _product = null!;

        public IGenericRepository<Purcharse> _purcharse = null!;

        public IGenericRepository<Client> _client = null!;

        public IGenericRepository<Sale> _sale = null!;

        public ISaleDetailRepository _saleDetail = null!;

        public UnitOfWork(POSContext context, IConfiguration configuration)
        {
            _context = context;
            
        }

        public IGenericRepository<Category> Category => _category ?? new GenericRepository<Category>(_context);
        public IGenericRepository<Provider> Provider => _provider ?? new GenericRepository<Provider>(_context);
        public IGenericRepository<DocumentType> DocumentType => _documentType ?? new GenericRepository<DocumentType>(_context);
        public IUserRepository User  => _user ?? new UserRepository(_context);
        public IWarehouseRepository Warehouse => _warehouse ?? new WarehouseRepository(_context);
        public IGenericRepository<Product> Product => _product ?? new GenericRepository<Product>(_context);

        public IProductStockRepository ProductStock => _productStock ?? new ProdutStockRepository(_context);

        public IGenericRepository<Purcharse> Purcharse => _purcharse ?? new GenericRepository<Purcharse>(_context);

        public IPurchaseDetailRepository PurchaseDetail => _purchaseDetail ?? new PurchaseDetailRepository(_context);

        public IGenericRepository<Client> Client => _client ?? new GenericRepository<Client>(_context);

        public IGenericRepository<Sale> Sale => _sale ?? new GenericRepository<Sale>(_context);

        public ISaleDetailRepository SaleDetail => _saleDetail ?? new SaleDetailRepository(_context);

        public IDbTransaction BeginTransaction()
        {
            var transaction = _context.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
