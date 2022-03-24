using MezuniyetProjesi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Model
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string ProductCondition { get; set; }
        public string Image { get; set; }
        public int Price { get; set; }
        public string UserName { get; set; }
        public bool IsOfferable { get; set; } = false;
        public bool IsSold { get; set; } = false;
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        public virtual int? CategoryId { get; set; }
    }
}
