using System;
using System.ComponentModel.DataAnnotations;


namespace WebApi.Models
{
    public class TourModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int PlacesCount { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public string ImageUrl { get; set; }
    }
}
