using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Model.DTOs
{
    public class ProductDto : EntityDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string ProductCondition { get; set; }
        public string Image { get; set; }
        public string UserName { get; set; }
        public int Price { get; set; }
        public bool IsOfferable { get; set; } = true;
        public bool IsSold { get; set; } = false;
        public virtual int? CategoryId { get; set; }
    }
    public class CreateProductInput
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        [Required]
        public string ProductCondition { get; set; }
        [JsonIgnore]
        public string Image { get; set; } = "image";
        [Required]
        public string UserName { get; set; }
        [Required]
        public int Price { get; set; }
        [JsonIgnore]
        public bool IsOfferable { get; set; } = true;
        [JsonIgnore]
        public bool IsSold { get; set; } = false;
        [Required]
        public virtual int? CategoryId { get; set; }
        //public string CreatedById { get; set; }
        //public string CreatedBy { get; set; }
    }

    public class UpdateProductInput : CreateProductInput
    {
        public int Id { get; set; }
        //public string ModifiedById { get; set; }
        //public string ModifiedBy { get; set; }
    }
    public class ProductImage
    {
        [Required] public IFormFile Image { get; set; }
    }
}
