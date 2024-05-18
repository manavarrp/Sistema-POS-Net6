using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Sale.Request;
using POS.Application.Dtos.Sale.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class SaleApplication : ISaleApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IOrderingQuery _orderingQuery;

        public SaleApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<SaleResponseDto>>> ListSales(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<SaleResponseDto>>();
            try
            {
                var sales = _unitOfWork.Sale.GetAllQueryable()
                    .AsNoTracking()
                    .Include(x => x.VoucherDocumentType)
                    .Include(x => x.Client)
                    .Include(x => x.Warehouse)
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            sales = sales.Where(x => x.VoucherNumber!.Contains(filters.TextFilter));
                            break;
                    }
                }

                if (filters.StateFilter is not null)
                {
                    sales = sales.Where(x => x.State.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    sales = sales.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }
                if (filters.Sort is null) filters.Sort = "Id";

                var items = await _orderingQuery.Ordering(filters, sales, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await sales.CountAsync();
                response.Data = _mapper.Map<IEnumerable<SaleResponseDto>>(items);
                response.Message = ReplyMessage.MESSAGE_QUERY;


            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse<SaleByIdResponseDto>> GetSaleById(int saleId)
        {
            var response = new BaseResponse<SaleByIdResponseDto>();

            try
            {
                var sale = await _unitOfWork.Sale.GetByIdAsync(saleId);

                if (sale is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var saleDetail = await _unitOfWork.SaleDetail.GetSaleDetailBySale(sale.Id);

                sale.SaleDetails = saleDetail.ToList();

                response.IsSuccess = true;
                response.Data = _mapper.Map<SaleByIdResponseDto>(sale);
                response.Message = ReplyMessage.MESSAGE_QUERY;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
            return response;
        }

        public async Task<BaseResponse<bool>> RegisterSale(SaleRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var sale = _mapper.Map<Sale>(requestDto);
                sale.State = (int)StateTypes.Active;
                await _unitOfWork.Sale.RegisterAsync(sale);

                foreach (var item in sale.SaleDetails)
                {
                    var productStock = await _unitOfWork.ProductStock.GetProductStockByProductId(item.ProductId, requestDto.WarehouseId);
                    productStock.CurrentStock -= item.Quantity;
                    await _unitOfWork.ProductStock.UpdateCurrentStockbyProducts(productStock);
                }

                transaction.Commit();

                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;

            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);

            }
            return response;
        }
    }
}
