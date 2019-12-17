using System;

namespace DAL.Entities
{
    public class Feedback
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }
        public virtual User User { get; set; }

    }
}
