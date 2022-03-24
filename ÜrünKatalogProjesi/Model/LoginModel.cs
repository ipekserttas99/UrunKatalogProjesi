using System.ComponentModel.DataAnnotations;

namespace MezuniyetProjesi.Model
{
    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }  
    }
}
