using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.Purchase.Request;
using POS.Application.Dtos.Purchase.Response;

namespace POS.Application.Interfaces
{
    public interface IPurchaseApplication
    {
        Task<BaseResponse<IEnumerable<PurchaseResponseDto>>> ListPurchases(BaseFiltersRequest filters);
        Task<BaseResponse<PurchaseByIdResponseDto>> GetPurchaseById(int purchaseId);
        Task<BaseResponse<bool>> RegisterPurchase(PurchaseRequestDto requestDto);
        Task<BaseResponse<bool>> CancelPurcharse(int purchaseId);
    }
}
