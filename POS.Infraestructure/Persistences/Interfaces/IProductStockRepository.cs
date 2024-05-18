using POS.Domain.Entities;

namespace POS.Infraestructure.Persistences.Interfaces
{
    public interface IProductStockRepository
    {
        Task<bool> RegisterProductSock(ProductStock productStock);
        Task<IEnumerable<ProductStock>> GetProductStocksByWarehouse(int productId);
        Task<ProductStock> GetProductStockByProductId(int productId, int warehouseId);
        Task<bool> UpdateCurrentStockbyProducts(ProductStock productStock);
    }
}
