using System;
using Volo.Abp.Application.Dtos;

namespace App.Orders
{
    public class OrderDto : FullAuditedEntityDto<Guid>
    {
        public string OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ShipDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
    }
}
