using System;

namespace BLL.DTO
{
    public class TourDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Location { get; set; }
        public double Price { get; set; }
        public int PlacesCount { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
