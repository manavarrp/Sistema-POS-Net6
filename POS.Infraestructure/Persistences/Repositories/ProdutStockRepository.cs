using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Contexts;
using POS.Infraestructure.Persistences.Interfaces;

namespace POS.Infraestructure.Persistences.Repositories
{
    public class ProdutStockRepository : IProductStockRepository
    {
        private readonly POSContext _context;

        public ProdutStockRepository(POSContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterProductSock(ProductStock productStock)
        {
            await _context.AddAsync(productStock);
            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }

        public async Task<IEnumerable<ProductStock>> GetProductStocksByWarehouse(int productId)
        {
            return await _context.ProductStocks
                .AsNoTracking()
                .Join(_context.Warehouses, ps => ps.WarehouseId, w => w.Id, (ps, w)
                 => new { ProductStock = ps, Warehouse = w })
                .Where(x => x.ProductStock.ProductId == productId)
                .OrderBy(x => x.Warehouse.Id)
                .Select(x => new ProductStock
                {
                   Warehouse = new  Warehouse { Name = x.Warehouse.Name },
                   CurrentStock = x.ProductStock.CurrentStock,
                   PurchasePrice = x.ProductStock.PurchasePrice

                })
                .ToListAsync();
        }

        public async Task<ProductStock> GetProductStockByProductId(int productId, int warehouseId)
        {
            var productStock = await _context.ProductStocks
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ProductId == productId && x.WarehouseId == warehouseId);

            return productStock!;
        }

        public async Task<bool> UpdateCurrentStockbyProducts(ProductStock productStock)
        {
            _context.Update(productStock);
            var recordsAffected = await _context.SaveChangesAsync();
            return recordsAffected > 0;
        }
    }
}
