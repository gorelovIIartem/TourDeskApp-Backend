using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class UserProfileModel
    {
        public string UserId { get; set; }
        [Required]
        public string FullName { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public DateTime Birthday { get; set; }
        public string ImageUrl { get; set; }
    }
}
