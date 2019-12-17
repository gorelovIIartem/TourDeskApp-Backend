using System;

namespace DAL.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string UserId { get; set; }
        public int TourId { get; set; }
        public bool IsSold { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
