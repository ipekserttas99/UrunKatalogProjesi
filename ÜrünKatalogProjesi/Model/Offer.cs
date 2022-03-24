using MezuniyetProjesi.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Model
{
    public class Offer : Entity
    {
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public virtual int ProductId { get; set; }
        public bool IsOfferPercentage { get; set; }
        public int OfferedPrice { get; set; }
    }
}
