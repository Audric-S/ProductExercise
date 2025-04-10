using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class OrderDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int ClientId { get; set; }

        public List<OrderDetailsDto> OrderDetails { get; set; } = new List<OrderDetailsDto>();
    }
}
