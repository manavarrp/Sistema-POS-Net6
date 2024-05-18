using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.Warehouse.Request;
using POS.Application.Dtos.Warehouse.Response;

namespace POS.Application.Interfaces
{
    public interface IWarehouseApplication
    {
        Task<BaseResponse<IEnumerable<WarehouseResponseDto>>> ListWarehouse(BaseFiltersRequest filters);
        Task<BaseResponse<WarehouseResponseByIdDto>> WarehouseById(int warehouseId);
        Task<BaseResponse<bool>> RegisterWarehouse(WarehouseRequestDto requestDto);
        Task<BaseResponse<bool>> EditWarehouse(int warehouseId, WarehouseRequestDto requestDto);
        Task<BaseResponse<bool>> RemoveWarehouse(int warehouseId);

    }
}
