using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Model.DTOs
{
    public class OfferDto : EntityDto
    {

        public virtual int ProductId { get; set; }
        public bool IsOfferPercentage { get; set; }
        public int OfferedPrice { get; set; }
    }
    public class CreateOfferInput
    {
        [Required]
        public virtual int ProductId { get; set; }
        [Required]
        public bool IsOfferPercentage { get; set; }
        [Required]
        public int OfferedPrice { get; set; }

    }

    public class UpdateOfferInput : CreateOfferInput
    {
        [Required]
        public int Id { get; set; }
    }
}
