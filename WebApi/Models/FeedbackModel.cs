using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Models
{
    public class FeedbackModel
    {
        public int Id { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public int TourId { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
