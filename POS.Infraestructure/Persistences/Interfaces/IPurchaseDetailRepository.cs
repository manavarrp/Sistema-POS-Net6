using POS.Domain.Entities;

namespace POS.Infraestructure.Persistences.Interfaces
{
    public interface IPurchaseDetailRepository
    {
        Task<IEnumerable<PurcharseDetail>> GetPurcharseDetailByPurchase(int purchaseId);
    }
}
