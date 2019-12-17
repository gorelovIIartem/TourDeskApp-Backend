using System;

namespace BLL.DTO
{
    public class TicketDTO
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string UserId { get; set; }
        public int TourId { get; set; }
        public bool IsSold { get; set; }
    }
}
