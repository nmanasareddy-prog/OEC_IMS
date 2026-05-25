using AutoMapper;
using OEC.IMS.Application.Features.Parts.Dtos;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Application.Features.Parts.Mappings;

public sealed class PartProfile : Profile
{
    public PartProfile()
    {
        CreateMap<Category, CategoryDto>();
        CreateMap<Part, PartDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.QuantityOnHand, o => o.MapFrom(s => s.InventoryStock != null ? s.InventoryStock.QuantityOnHand : 0))
            .ForMember(d => d.IsLowStock, o => o.MapFrom(s => s.InventoryStock != null && s.InventoryStock.QuantityOnHand <= s.ReorderLevel));

        CreateMap<Part, PartListItemDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.QuantityOnHand, o => o.MapFrom(s => s.InventoryStock != null ? s.InventoryStock.QuantityOnHand : 0))
            .ForMember(d => d.IsLowStock, o => o.MapFrom(s => s.InventoryStock != null && s.InventoryStock.QuantityOnHand <= s.ReorderLevel));
    }
}
