using POS.Domain.Entities;
using System.Data;

namespace POS.Infraestructure.Persistences.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        //Declaracion o matricula de nuestra interface  a nivel repository
        IGenericRepository<Category> Category {  get; }
        IGenericRepository<Provider> Provider {  get; }
        IGenericRepository<DocumentType> DocumentType {  get; }
        IUserRepository User { get; }
        IWarehouseRepository Warehouse { get; }
        IProductStockRepository ProductStock { get; }
        IPurchaseDetailRepository PurchaseDetail { get; }
        IGenericRepository<Product> Product { get; }
        IGenericRepository<Purcharse> Purcharse { get; }
        IGenericRepository<Client> Client { get; }
        IGenericRepository<Sale> Sale { get; }
        ISaleDetailRepository SaleDetail { get; }
        void SaveChanges();
        Task SaveChangesAsync();
        IDbTransaction BeginTransaction();
    }
}
