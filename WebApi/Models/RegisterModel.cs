using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        public string FullName { get; set; }

    }
}
