﻿using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Contexts;
using POS.Infraestructure.Persistences.Interfaces;

namespace POS.Infraestructure.Persistences.Repositories
{
    internal class SaleDetailRepository : ISaleDetailRepository
    {
        private readonly POSContext _context;

        public SaleDetailRepository(POSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SaleDetail>> GetSaleDetailBySale(int saleId)
        {
            var response = await _context.Products
                .AsNoTracking()
                .Join(_context.SaleDetails, p => p.Id, sd => sd.ProductId, (p, sd)
                => new { Product = p, SaleDetail = sd })
                .Where(x => x.SaleDetail.SaleId == saleId)
                .Select(x => new SaleDetail
                {
                    ProductId = x.Product.Id,
                    Product = new Product
                    {
                        Image = x.Product.Image,
                        Code = x.Product.Code,
                        Name = x.Product.Name
                    },
                    Quantity = x.SaleDetail.Quantity,
                    UnitSalePrice = x.SaleDetail.UnitSalePrice,
                    Total = x.SaleDetail.Total
                })
                .ToListAsync();

            return response;
        }
    }
}
