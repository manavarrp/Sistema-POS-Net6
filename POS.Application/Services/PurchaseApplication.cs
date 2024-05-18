using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Purchase.Request;
using POS.Application.Dtos.Purchase.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class PurchaseApplication : IPurchaseApplication
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper   _mapper;
        private readonly IOrderingQuery _orderingQuery;

        public PurchaseApplication(IUnitOfWork unitOfWork, IMapper mapper, IOrderingQuery orderingQuery)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<PurchaseResponseDto>>> ListPurchases(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<PurchaseResponseDto>>();
            try
            {
                var purchases = _unitOfWork.Purcharse.GetAllQueryable()
                    .AsNoTracking()
                    .Include(x => x.Provider)
                    .Include(x => x.Warehouse)
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            purchases = purchases.Where(x => x.Provider.Name!.Contains(filters.TextFilter));
                            break;
                       
                    }
                }

                if (filters.StateFilter is not null)
                {
                    purchases = purchases.Where(x => x.State.Equals(filters.StateFilter));
                }

                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    purchases = purchases.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }
                 filters.Sort ??= "Id";

                var items = await _orderingQuery.Ordering(filters, purchases, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await purchases.CountAsync();
                response.Data = _mapper.Map<IEnumerable<PurchaseResponseDto>>(items);
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

        public async Task<BaseResponse<PurchaseByIdResponseDto>> GetPurchaseById(int purchaseId)
        {
            var response = new BaseResponse<PurchaseByIdResponseDto>();

            try
            {
                var purchase = await _unitOfWork.Purcharse.GetByIdAsync(purchaseId);

                if (purchase is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                
                var purchaseDetail = await _unitOfWork.PurchaseDetail.GetPurcharseDetailByPurchase(purchase.Id);

                purchase.PurcharseDetails = purchaseDetail.ToList();

                response.IsSuccess = true;
                response.Data = _mapper.Map<PurchaseByIdResponseDto>(purchase);
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

        public async Task<BaseResponse<bool>> RegisterPurchase(PurchaseRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var purchase = _mapper.Map<Purcharse>(requestDto);
                purchase.State = (int)StateTypes.Active;
                await _unitOfWork.Purcharse.RegisterAsync(purchase);

                foreach (var item in purchase.PurcharseDetails)
                {
                    var productStock = await _unitOfWork.ProductStock.GetProductStockByProductId(item.ProductId, requestDto.WarehouseId);
                    productStock.CurrentStock += item.Quantity;
                    productStock.PurchasePrice = item.UnitPurchasePrice;
                    await _unitOfWork.ProductStock.UpdateCurrentStockbyProducts(productStock);
                }

                transaction.Commit();

                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_SAVE;

            }catch (Exception ex)
            {
                transaction.Rollback();
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);

            }
            return response;
        }

        public async Task<BaseResponse<bool>> CancelPurcharse(int purchaseId)
        {
            var response = new BaseResponse<bool>();
            using var transaction = _unitOfWork.BeginTransaction();

            try
            {
                var purcharse = await GetPurchaseById(purchaseId);
                response.Data = await _unitOfWork.Purcharse.RemoveAsync(purchaseId);

                foreach (var item in purcharse.Data!.PurcharseDetails)
                {
                    var productStock = await _unitOfWork.ProductStock.GetProductStockByProductId(item.ProductId, purcharse.Data.WarehouseId);
                    productStock.CurrentStock -= item.Quantity;

                    await _unitOfWork.ProductStock.UpdateCurrentStockbyProducts(productStock); 
                }
                transaction.Commit();

                response.IsSuccess = true;
                response.Message = ReplyMessage.MESSAGE_DELETE;

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
