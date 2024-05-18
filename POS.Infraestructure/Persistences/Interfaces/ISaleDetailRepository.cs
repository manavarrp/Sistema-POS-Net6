using POS.Domain.Entities;

namespace POS.Infraestructure.Persistences.Interfaces
{
    public interface ISaleDetailRepository
    {
        Task<IEnumerable<SaleDetail>> GetSaleDetailBySale(int saleId);
    }
}
