using AutoMapper;
using POS.Application.Dtos.ProductStock.Response;
using POS.Domain.Entities;

namespace POS.Application.Mappers
{
    public class ProductStockMappingsProfile : Profile
    {
        public ProductStockMappingsProfile()
        {
            CreateMap<ProductStock, ProductStockByWarehouseResponseDto>()
                .ForMember(x => x.Warehouse, x => x.MapFrom(y => y.Warehouse.Name))
                .ReverseMap();
        }

    }
}
