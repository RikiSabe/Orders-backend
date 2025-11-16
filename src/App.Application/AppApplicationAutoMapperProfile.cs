using App.Orders;
using AutoMapper;

namespace App;

public class AppApplicationAutoMapperProfile : Profile
{
    public AppApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Order, OrderDto>();
        CreateMap<CreateUpdateOrderDto, Order>()
            .ForMember(dest => dest.OrderNumber, opt => opt.Ignore());
    }
}
