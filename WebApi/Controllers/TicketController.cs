using BLL.DTO;
using BLL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using WebApi.Models;
using AutoMapper;
using DAL.Entities;

namespace WebApi.Controllers
{
   // [Authorize(Roles = "user", AuthenticationSchemes = "Bearer")]
    [Route("api/ticket")]
    [ApiController]
    public class TicketController:ControllerBase
    {
        private ITicketService _ticketService;
        private ITourService _tourService;
        private IUserService _userService;

        public TicketController(ITicketService ticketService, ITourService tourService)
        {
            _ticketService = ticketService;
            _tourService = tourService;
        }

        [HttpPost]
        public async Task<ActionResult> BuyTicket([FromBody] TicketModel model)
        {
            TicketDTO ticket = Mapper.Map<TicketModel, TicketDTO>(model);
            var purchase = await _ticketService.BuyTicket(ticket);
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> GetNumberOfFreeTickets(TourDTO tourDTO)
        {
            int freeTickets =  _ticketService.ShowNumberOfFreeTickets(tourDTO);
            Log.Information($"Here are {freeTickets} free tickets");
            return Ok(freeTickets);
        }

        [HttpGet]
        [Route("tickets/soldTickets/{tourId}")]
        public async Task<ActionResult> ShowNumberOfSoldTickets(DateTime date)
        {
            var tickets = await _ticketService.ShowAllSoldTickets(date);
            Log.Information($"Here are all sold tickets for {date}");
            return Ok(tickets);
        }

        [HttpDelete]
        [Route("{userId}/{tourId}")]
        public async Task<ActionResult> CancelPurchase(string userId, int tourId)
        {
            await _ticketService.DeleteTicket(userId, tourId);
            Log.Information($"You canceled this trip {tourId}");
            return Ok();
        }

    }
}
