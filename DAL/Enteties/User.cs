using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class User
    {
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual UserProfile UserProfile { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Tour> Tours { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }


        public User()
        {
            Feedbacks = new List<Feedback>();
            Tours = new List<Tour>();
            Tickets = new List<Ticket>();
        }
    }
}
