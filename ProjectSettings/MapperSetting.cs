using DAL.Entities;
using BLL.DTO;

namespace ProjectSettings
{
    public class MapperSettings : AutoMapper.Profile
    {
        public MapperSettings()
        {
            CreateMap<UserDTO, User>().ReverseMap();
            CreateMap<TourDTO, Tour>().ReverseMap();
            CreateMap<TicketDTO, Ticket>().ReverseMap();
        }
    }
}
