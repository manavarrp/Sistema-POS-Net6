﻿using AutoMapper;
using POS.Application.Dtos.Purchase.Request;
using POS.Application.Dtos.Purchase.Response;
using POS.Domain.Entities;

namespace POS.Application.Mappers
{
    public class PurchaseMappingsProfile : Profile
    {
        public PurchaseMappingsProfile()
        {
            CreateMap<Purcharse, PurchaseResponseDto>()
               .ForMember(x => x.PurchaseId, x => x.MapFrom(y => y.Id))
               .ForMember(x => x.Warehouse, x => x.MapFrom(y => y.Warehouse.Name))
               .ForMember(x => x.Provider, x => x.MapFrom(y => y.Provider.Name))
               .ForMember(x => x.DateOfPurchase, x => x.MapFrom(y => y.AuditCreateDate))
               .ReverseMap();

            CreateMap<Purcharse, PurchaseByIdResponseDto>()
               .ForMember(x => x.PurchaseId, x => x.MapFrom(y => y.Id))
               .ReverseMap();

            CreateMap<PurcharseDetail, PurcharseDetailByIdResponseDto>()
               .ForMember(x => x.ProductId, x => x.MapFrom(y => y.ProductId))
               .ForMember(x => x.Image, x => x.MapFrom(y => y.Product.Image))
               .ForMember(x => x.Code, x => x.MapFrom(y => y.Product.Code))
               .ForMember(x => x.Name, x => x.MapFrom(y => y.Product.Name))
               .ForMember(x => x.TotalAmount, x => x.MapFrom(y => y.Total))
               .ReverseMap();

            CreateMap<PurchaseRequestDto, Purcharse>();

            CreateMap<PurcharseDetailRequestDto, PurcharseDetail>();
        }
    }
}
