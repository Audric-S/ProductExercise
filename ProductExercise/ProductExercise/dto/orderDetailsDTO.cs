
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class OrderDetailsDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
