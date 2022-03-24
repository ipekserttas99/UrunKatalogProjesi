using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MezuniyetProjesi.Model.DTOs
{
    public class MailDto
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }

    }

    public class CreateMailInput
    {
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
    }

    public class UpdateMailInput : CreateMailInput
    {
        public int Id { get; set; }
    }
}
