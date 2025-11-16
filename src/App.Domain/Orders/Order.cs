using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace App.Orders
{
    public class Order : FullAuditedAggregateRoot<Guid>
    {
        [Required]
        public string OrderNumber { get; set; }

        [Required]
        [MaxLength(200)]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? ShipDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        [Range(0.00, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; }

        [Required]
        [MaxLength(500)]
        public string ShippingAddress { get; set; }

        [MaxLength(1000)]
        public string? Notes { get; set; }

        public Order() { }

        public Order(
            Guid id,
            string orderNumber,
            string customerName,
            string customerEmail,
            DateTime orderDate,
            OrderStatus status,
            decimal totalAmount,
            string shippingAddress,
            string notes = null,
            DateTime? shipDate = null
        ) : base(id)
        {
            SetOrderNumber(orderNumber);
            SetCustomerName(customerName);
            SetCustomerEmail(customerEmail);
            OrderDate = orderDate;
            Status = status;
            SetTotalAmount(totalAmount);
            SetShippingAddress(shippingAddress);
            Notes = notes;
            ShipDate = shipDate;
        }

        public Order SetOrderNumber(string orderNumber)
        {
            OrderNumber = Check.NotNullOrWhiteSpace(
                orderNumber,
                nameof(orderNumber)
            );
            return this;
        }

        public Order SetCustomerName(string customerName)
        {
            CustomerName = Check.NotNullOrWhiteSpace(
                customerName,
                nameof(customerName)
            );
            return this;
        }

        public Order SetCustomerEmail(string customerEmail)
        {
            CustomerEmail = Check.NotNullOrWhiteSpace(
                customerEmail,
                nameof(customerEmail),
                maxLength: 256
            );
            return this;
        }

        public Order SetTotalAmount(decimal totalAmount)
        {
            if (totalAmount <= 0)
            {
                throw new ArgumentException("El monto total debe ser mayor a 0", nameof(totalAmount));
            }
            TotalAmount = totalAmount;
            return this;
        }

        public Order SetShippingAddress(string shippingAddress)
        {
            ShippingAddress = Check.NotNullOrWhiteSpace(
                shippingAddress,
                nameof(shippingAddress),
                maxLength: 500
            );
            return this;
        }

        public Order UpdateStatus(OrderStatus newStatus)
        {
            Status = newStatus;
            if (newStatus == OrderStatus.Shipped && !ShipDate.HasValue)
            {
                ShipDate = DateTime.UtcNow;
            }
            return this;
        }
        public Order SetShipDate(DateTime? shipDate)
        {
            if (shipDate.HasValue && shipDate.Value < OrderDate)
            {
                throw new ArgumentException(
                    "La fecha de envío no puede ser anterior a la fecha de pedido",
                    nameof(shipDate)
                );
            }
            ShipDate = shipDate;
            return this;
        }

    }
}
