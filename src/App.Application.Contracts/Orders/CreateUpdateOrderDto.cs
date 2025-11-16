using System;
using System.ComponentModel.DataAnnotations;

namespace App.Orders
{
    public class CreateUpdateOrderDto
    {
        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(256)]
        public string CustomerEmail { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        public DateTime? ShipDate { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Total amount must be greater than 0")]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(500)]
        public string ShippingAddress { get; set; }

        [StringLength(1000)]
        public string Notes { get; set; }
    }
}
