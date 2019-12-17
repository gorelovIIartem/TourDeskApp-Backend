using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class TicketModel
    {
        public int Id { get; set; }
        [Required]
        public DateTime PurchaseDate { get; set; }
        public string UserId { get; set; }
        public int TourId { get; set; }
        [Required]
        public bool IsSold { get; set; }
    }
}
