using AutoMapper;
using BLL.DTO;
using BLL.Infrastructure;
using BLL.Interfaces;
using DAL.Entities;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TicketService : ITicketService, IDisposable
    {
        private readonly IUnitOfWork DataBase;
        public TicketService(IUnitOfWork uow)
        {
            DataBase = uow;
        }
        public int ShowNumberOfFreeTickets(TourDTO tourDTO)
        {
            if (tourDTO == null)
                throw new ValidationException("There is no information about this tour", "");
            int numberOfFreeTickets = tourDTO.PlacesCount;
            return numberOfFreeTickets;
        }


        public async Task<TicketDTO> BuyTicket(TourDTO tourDTO)
        {
            TicketDTO ticket = null;
            if (tourDTO == null)
                throw new ValidationException("There is no information about this tour", "");
            if (tourDTO.PlacesCount == 0)
                throw new ValidationException("There is no free places in this tour", "");
            DataBase.TicketManager.Create(Mapper.Map<TicketDTO, Ticket>(ticket));
            tourDTO.PlacesCount--;
            await DataBase.SaveAsync();
            return ticket;

        }

        public async Task<OperationDetails> ShowAllSoldTickets(DateTime date)
        {
            ICollection<TicketDTO> tickets = null;
            await Task.Run(() =>
            {
                var soldtickets = DataBase.TicketManager.GetAll().Where(p => p.PurchaseDate == date);
                foreach (var p in soldtickets)
                {
                    tickets.Add(Mapper.Map<Ticket, TicketDTO>(p));
                }
                return tickets;
            });
            return new OperationDetails(true, "Tickets are got succesfully", "ticket");

        }




        public void Dispose()
        {
            DataBase.Dispose();
        }

        public TicketDTO GetTicket(int TicketId)
        {
            TicketDTO ticket = null;
            Ticket ticketBoof = DataBase.TicketManager.Get(TicketId);
            if (ticketBoof != null)
                ticket = Mapper.Map<Ticket, TicketDTO>(ticketBoof);
            return ticket;
        }

        public async Task<OperationDetails> DeleteTicket(int TicketId)
        {
            Ticket ticket = DataBase.TicketManager.Get(TicketId);
            if (ticket == null)
                throw new ValidationException("There is no information about this ticket", "");
            DataBase.TicketManager.Delete(TicketId);
            await DataBase.SaveAsync();
            return new OperationDetails(true, "Ticket deleted successfully", "ticket");
        }
    }
}
