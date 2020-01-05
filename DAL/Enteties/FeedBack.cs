using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Entities
{
    public class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public int TourId { get; set; }
        public string UserName { get; set; }
        public virtual Tour Tour { get; set; }
        public virtual User User { get; set; }

    }
}
