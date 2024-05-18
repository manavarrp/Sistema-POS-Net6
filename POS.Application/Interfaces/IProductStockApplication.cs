using POS.Application.Commons.Bases.Response;
using POS.Application.Dtos.ProductStock.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Interfaces
{
    public interface IProductStockApplication
    {
        Task<BaseResponse<IEnumerable<ProductStockByWarehouseResponseDto>>> GetProductStockByWarehouse(int productId);
    }
}
