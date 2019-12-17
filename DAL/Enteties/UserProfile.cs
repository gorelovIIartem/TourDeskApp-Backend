using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    [Table("UserProfile")]
    public class UserProfile
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public virtual User User { get; set; }
        public DateTime Birthday { get; set; }
    }
}
