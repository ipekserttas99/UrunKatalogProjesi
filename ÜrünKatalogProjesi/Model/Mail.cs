using System;

namespace MezuniyetProjesi.Model
{
    public class Mail
    {
        public int Id { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; }
    }
}
