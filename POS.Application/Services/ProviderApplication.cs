using AutoMapper;
using Microsoft.EntityFrameworkCore;
using POS.Application.Commons.Bases.Request;
using POS.Application.Commons.Bases.Response;
using POS.Application.Commons.Ordering;
using POS.Application.Dtos.Provider.Request;
using POS.Application.Dtos.Provider.Response;
using POS.Application.Interfaces;
using POS.Domain.Entities;
using POS.Infraestructure.Persistences.Interfaces;
using POS.Utilities.Static;
using WatchDog;

namespace POS.Application.Services
{
    public class ProviderApplication : IProviderApplication
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrderingQuery _orderingQuery;

        public ProviderApplication(IMapper mapper, IUnitOfWork unitOfWork, IOrderingQuery orderingQuery)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _orderingQuery = orderingQuery;
        }

        public async Task<BaseResponse<IEnumerable<ProviderResponseDto>>> ListProviders(BaseFiltersRequest filters)
        {
            var response = new BaseResponse<IEnumerable<ProviderResponseDto>>();

            try
            {
                var providers = _unitOfWork.Provider
                    .GetAllQueryable()
                    .Include(x => x.DocumentType)
                    .AsQueryable();

                if (filters.NumFilter is not null && !string.IsNullOrEmpty(filters.TextFilter))
                {
                    switch (filters.NumFilter)
                    {
                        case 1:
                            providers = providers.Where(x => x.Name.Contains(filters.TextFilter));
                            break;
                        case 2:
                            providers = providers.Where(x => x.Email.Contains(filters.TextFilter));
                            break;
                        case 3:
                            providers = providers.Where(x => x.DocumentNumber.Contains(filters.TextFilter));
                            break;
                    }
                }
                if (filters.StateFilter is not null)
                {
                    providers = providers.Where(x => x.State.Equals(filters.StateFilter));
                }
                if (!string.IsNullOrEmpty(filters.StartDate) && !string.IsNullOrEmpty(filters.EndDate))
                {
                    providers = providers.Where(x => x.AuditCreateDate >= Convert.ToDateTime(filters.StartDate) && x.AuditCreateDate <= Convert.ToDateTime(filters.EndDate).AddDays(1));
                }
                if (filters.Sort is null) filters.Sort = "Id";

                var items = await _orderingQuery.Ordering(filters, providers, !(bool)filters.Download!).ToListAsync();

                response.IsSuccess = true;
                response.TotalRecords = await providers.CountAsync();
                response.Data = _mapper.Map<IEnumerable<ProviderResponseDto>>(items);
                response.Message = ReplyMessage.MESSAGE_QUERY;

            }
            catch (Exception ex)
            {
                response.IsSuccess= false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
         

            return response;

        }

        public async Task<BaseResponse<ProviderResponseDto>> ProvidersById(int providerId)
        {
            var response = new BaseResponse<ProviderResponseDto>();

            try
            {
                var provider = await _unitOfWork.Provider.GetByIdAsync(providerId);

                if (provider is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }
                response.IsSuccess = true;
                response.Data = _mapper.Map<ProviderResponseDto>(provider);
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

        public async Task<BaseResponse<bool>> RegisterProvider(ProviderRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var provider = _mapper.Map<Provider>(requestDto);

                response.Data = await _unitOfWork.Provider.RegisterAsync(provider);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_SAVE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                }

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
           
            return response;
        }

        public async Task<BaseResponse<bool>> EditProvider(int providerId, ProviderRequestDto requestDto)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var providerEdit = await ProvidersById(providerId);

                if (providerEdit.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                var provider = _mapper.Map<Provider>(requestDto);
                provider.Id = providerId;
                response.Data = await _unitOfWork.Provider.EditAsync(provider);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_UPDATE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_FAILED;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
           
            return response;
        }

        public async Task<BaseResponse<bool>> RemoveProvider(int providerId)
        {
            var response = new BaseResponse<bool>();
            try
            {
                var providerRemove = await ProvidersById(providerId);
                if (providerRemove.Data is null)
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                    return response;
                }

                response.Data = await _unitOfWork.Provider.RemoveAsync(providerId);

                if (response.Data)
                {
                    response.IsSuccess = true;
                    response.Message = ReplyMessage.MESSAGE_DELETE;
                }
                else
                {
                    response.IsSuccess = false;
                    response.Message = ReplyMessage.MESSAGE_QUERY_EMPTY;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ReplyMessage.MESSAGE_EXCEPTION;
                WatchLogger.Log(ex.Message);
            }
           
            return response;
        }
    }
}
