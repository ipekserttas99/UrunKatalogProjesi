using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Model.DTOs
{
    public class CategoryDto: EntityDto
    {
        public string Name { get; set; }
    }
    public class CreateCategoryInput
    {
        [Required]
        public string Name { get; set; }
        

    }
    public class UpdateCategoryInput : CreateCategoryInput
    {
        [Required]
        public int Id { get; set; }
        //public string ModifiedById { get; set; }
        //public string ModifiedBy { get; set; }
    }
}
