using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace App.Orders
{
    public class OrderAppService 
        : CrudAppService<
            Order,
            OrderDto,
            Guid,
            GetOrderListDto,
            CreateUpdateOrderDto,
            CreateUpdateOrderDto>,
          IOrderAppService
    {
        public OrderAppService(IRepository<Order, Guid> repository)
            : base(repository)
        {
        }

        protected override async Task<IQueryable<Order>> CreateFilteredQueryAsync(GetOrderListDto input)
        {
            var query = await base.CreateFilteredQueryAsync(input);
                
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                query = query.Where(o =>
                    o.OrderNumber.Contains(input.Filter) ||
                    o.CustomerName.Contains(input.Filter) ||
                    o.CustomerEmail.Contains(input.Filter)
                );
            }

            if (input.Status != null && input.Status.Any())
            {
                query = query.Where(o => input.Status.Contains(o.Status));
            }

            if (input.StartDate.HasValue)
            {
                query = query.Where(o => o.OrderDate >= input.StartDate.Value);
            }

            if (input.EndDate.HasValue)
            {
                var endOfDay = input.EndDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(o => o.OrderDate <= endOfDay);
            }

            return query;
        }

        public override async Task<OrderDto> CreateAsync(CreateUpdateOrderDto input)
        {
            var orderNumber = await GenerateOrderNumberAsync();

            var order = new Order(
                GuidGenerator.Create(),
                orderNumber,
                input.CustomerName,
                input.CustomerEmail,
                input.OrderDate,
                input.Status,
                input.TotalAmount,
                input.ShippingAddress,
                input.Notes,
                input.ShipDate
            );

            await Repository.InsertAsync(order, autoSave: true);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        public override async Task<OrderDto> UpdateAsync(Guid id, CreateUpdateOrderDto input)
        {
            var order = await Repository.GetAsync(id);

            order
                .SetCustomerName(input.CustomerName)
                .SetCustomerEmail(input.CustomerEmail)
                .SetTotalAmount(input.TotalAmount)
                .SetShippingAddress(input.ShippingAddress);

            order.OrderDate = input.OrderDate;
            order.Notes = input.Notes;

            if (order.Status != input.Status)
            {
                order.UpdateStatus(input.Status);
            }

            if (input.ShipDate.HasValue && input.ShipDate != order.ShipDate)
            {
                order.SetShipDate(input.ShipDate);
            }

            await Repository.UpdateAsync(order, autoSave: true);

            return ObjectMapper.Map<Order, OrderDto>(order);
        }

        private async Task<string> GenerateOrderNumberAsync()
        {
            var date = DateTime.Now.ToString("yyyyMMdd");
            var count = await Repository.CountAsync();
            var sequence = (count + 1).ToString("D5");
            return $"ORD-{date}-{sequence}";
        }
    }
}
