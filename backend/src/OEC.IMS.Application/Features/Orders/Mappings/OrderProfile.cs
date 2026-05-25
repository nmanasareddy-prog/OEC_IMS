using AutoMapper;
using OEC.IMS.Application.Features.Orders.Dtos;
using OEC.IMS.Domain.Entities;

namespace OEC.IMS.Application.Features.Orders.Mappings;

public sealed class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderLine, OrderLineDto>()
            .ForMember(d => d.PartSku, o => o.MapFrom(s => s.Part.Sku))
            .ForMember(d => d.PartName, o => o.MapFrom(s => s.Part.Name));

        CreateMap<Order, OrderDto>();
        CreateMap<Order, OrderListItemDto>()
            .ForMember(d => d.LineCount, o => o.MapFrom(s => s.OrderLines.Count));
    }
}
