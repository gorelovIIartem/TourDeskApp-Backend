using System;
using BLL.DTO;
using BLL.Infrastructure;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ITicketService
    {
        int ShowNumberOfFreeTickets(TourDTO tourDTO);
        Task<TicketDTO> BuyTicket(TourDTO tourDTO);
        Task<OperationDetails> ShowAllSoldTickets(DateTime date);
        TicketDTO GetTicket(int TicketId);
        Task<OperationDetails> DeleteTicket(int TicketId);
    }
}
