using AutoMapper;
using POS.Application.Dtos.Warehouse.Request;
using POS.Application.Dtos.Warehouse.Response;
using POS.Domain.Entities;
using POS.Utilities.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Application.Mappers
{
    internal class WarehouseMappingsProfile : Profile
    {
        public WarehouseMappingsProfile()
        {
            CreateMap<Warehouse, WarehouseResponseDto>()
                .ForMember(x => x.WarehouseId, x => x.MapFrom(y => y.Id))
                .ForMember(x=> x.StateWarehouse, x => x.MapFrom(y => y.State.Equals((int)StateTypes.Active) ? "Activo" : "Inactivo" ))
                .ReverseMap();

            CreateMap<Warehouse, WarehouseResponseByIdDto>()
                .ForMember(x => x.WarehouseId, x => x.MapFrom(y => y.Id))
                .ReverseMap();

            CreateMap<WarehouseRequestDto, Warehouse>();
        }
    }
}
