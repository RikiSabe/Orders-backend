using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace App.Orders
{
    public class GetOrderListDto : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }
        public List<OrderStatus> Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
