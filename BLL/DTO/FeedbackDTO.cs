using System;

namespace BLL.DTO
{
    public class FeedbackDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }
        public int TourId { get; set; }
        public string UserName { get; set; }

    }
}
