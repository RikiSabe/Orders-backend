using App.Orders;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace App.Controllers
{
    [RemoteService(Name = "Default")]
    [Area("app")]
    [ControllerName("Order")]
    [Route("api/app/orders")]
    public class OrderController : AbpControllerBase, IOrderAppService
    {
        private readonly IOrderAppService _orderAppService;

        public OrderController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }

        [HttpGet]
        public Task<PagedResultDto<OrderDto>> GetListAsync(GetOrderListDto input)
        {
            return _orderAppService.GetListAsync(input);
        }

        [HttpGet("{id}")]
        public Task<OrderDto> GetAsync(Guid id)
        {
            return _orderAppService.GetAsync(id);
        }

        [HttpPost]
        public Task<OrderDto> CreateAsync(CreateUpdateOrderDto input)
        {
            return _orderAppService.CreateAsync(input);
        }

        [HttpPut("{id}")]
        public Task<OrderDto> UpdateAsync(Guid id, CreateUpdateOrderDto input)
        {
            return _orderAppService.UpdateAsync(id, input);
        }

        [HttpDelete("{id}")]
        public Task DeleteAsync(Guid id)
        {
            return _orderAppService.DeleteAsync(id);
        }
    }
}
