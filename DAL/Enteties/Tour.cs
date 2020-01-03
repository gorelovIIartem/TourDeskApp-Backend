using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DAL.Entities
{
    public class Tour
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public int PlacesCount { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public virtual ICollection<Feedback> FeedBacks { get; set; }
        public string ImageUrl { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public DateTime Date { get; set; }
    }
}
