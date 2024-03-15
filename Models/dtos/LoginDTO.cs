using System.ComponentModel.DataAnnotations;

namespace TodoListAPP.Models.dtos
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}
