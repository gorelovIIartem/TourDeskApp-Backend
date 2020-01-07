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
        /// <summary>
        /// Add new ticket.
        /// </summary>
        /// <param name="ticketDTO"> Ticket wich will be added.</param>
        Task<TicketDTO> BuyTicket(TicketDTO ticketDTO);
        /// <summary>
        /// Delete ticket.
        /// </summary>
        /// <param name="userID"> Part of composite key. Ticket with this id of user will be deleted.</param>
        /// <param name="tourId"> Part of composite key. Ticket with this id of tour will be deleted.</param>
        Task<OperationDetails> DeleteTicket(string userID, int tourId);
    }
}
