using AutoMapper;
using OEC.IMS.Application.Features.Vehicles.Dtos;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Application.Features.Vehicles.Mappings;

public sealed class VehicleProfile : Profile
{
    public VehicleProfile()
    {
        CreateMap<Manufacturer, ManufacturerDto>();
        CreateMap<VehicleModel, VehicleModelDto>();
    }
}
