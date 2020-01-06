using DAL.Entities;
using BLL.DTO;

namespace ProjectSettings
{
    public class MapperSettings : AutoMapper.Profile
    {
        public MapperSettings()
        {
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<Tour, TourDTO>().ReverseMap();
            CreateMap<TicketDTO, Ticket>().ReverseMap();
            CreateMap<FeedbackDTO, Feedback>().ReverseMap();
            CreateMap<UserDTO, UserProfile>().ReverseMap();
        }
    }
}
