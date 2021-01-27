using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class RegisterDto
    {
        /*
        to add a vildation we use something refered to data annotations
        so we add atribute called[required] to username and password
        */
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}