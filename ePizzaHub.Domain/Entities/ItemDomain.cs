using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizzaHub.Domain.Entities
{
    public class ItemDomain
    {
        public int Id { get; set; }

        public string Name { get; set; }=default!;

        public string Description { get; set; } = default!;

        public decimal UnitPrice { get; set; } = default!;

        public string ImageUrl { get; set; } = default!;
    }
}
