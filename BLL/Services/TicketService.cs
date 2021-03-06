﻿using AutoMapper;
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


        public async Task<TicketDTO> BuyTicket(TicketDTO ticketDTO)
        {
            
            Tour tour = DataBase.TourManager.Get(ticketDTO.TourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", tour.Name);
            if (ticketDTO == null)
                throw new ValidationException("There is no information",$"{ticketDTO.Id}");
            ApplicationUser user = await DataBase.UserManager.FindByIdAsync(ticketDTO.UserId);
            if (user == null)
                throw new ValidationException("There is no information about this user", user.UserName);
            if (tour.PlacesCount < 1)
                throw new ValidationException("There is no free places", tour.Name);
            Ticket ticket = new Ticket
            {
                UserId = ticketDTO.UserId,
                TourId = ticketDTO.TourId,
                PurchaseDate = DateTime.Now,
                IsSold = true
            };
            DataBase.TicketManager.Create(ticket);
            tour.PlacesCount--;
            await DataBase.SaveAsync();
            return Mapper.Map<TicketDTO>(ticket);

        }
        public void Dispose()
        {
            DataBase.Dispose();
        }

        public async Task<OperationDetails> DeleteTicket(string userId, int tourId)
        {
            var user = DataBase.QUserManager.Get(userId);
            if (user == null)
                throw new ValidationException("There is no information about this user", userId);
            var tour = DataBase.TourManager.Get(tourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", "");
            Ticket ticket = DataBase.TicketManager.GetAll().Where(p => p.UserId == userId && p.TourId == tourId).FirstOrDefault();
            DataBase.TicketManager.Delete(ticket);
            tour.PlacesCount++;
            await DataBase.SaveAsync();
            return new OperationDetails(true, "Ticket deleted successfully", "ticket");
        }
    }
}
