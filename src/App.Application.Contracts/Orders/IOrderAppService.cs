using System;
using Volo.Abp.Application.Services;

namespace App.Orders
{
    public interface IOrderAppService: ICrudAppService
        <OrderDto,
        Guid,
        GetOrderListDto,
        CreateUpdateOrderDto,
        CreateUpdateOrderDto> {}
}
