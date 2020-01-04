using System;
using BLL.DTO;
using BLL.Infrastructure;
using System.Threading.Tasks;
using System.Collections;
using DAL.Entities;
using System.Collections.Generic;

namespace BLL.Interfaces
{
    public interface ITicketService
    {
        int ShowNumberOfFreeTickets(TourDTO tourDTO);
        Task<TicketDTO> BuyTicket(TicketDTO ticketDTO, int tourId);
        Task<OperationDetails> ShowAllSoldTickets(DateTime date);
        TicketDTO GetTicket(int TicketId);
        Task<OperationDetails> DeleteTicket(string userID, int tourId);
    }
}
