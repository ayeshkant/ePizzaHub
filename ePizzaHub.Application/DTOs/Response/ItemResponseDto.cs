using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Application.DTOs.Response
{
    public class ItemResponseDto
    {
        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public decimal UnitPrice { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }
}
