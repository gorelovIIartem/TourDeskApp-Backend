using BLL.DTO;
using BLL.Interfaces;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace WebApi.Controllers
{
    [Authorize(Roles = "user", AuthenticationSchemes = "Bearer")]
    [Route("api/ticket")]
    [ApiController]
    public class TicketController:ControllerBase
    {
        private ITicketService _ticketService;
        private ITourService _tourService;

        public TicketController(ITicketService ticketService, ITourService tourService)
        {
            _ticketService = ticketService;
            _tourService = tourService;
        }

        [HttpPost]
        public async Task<ActionResult> BuyTicket(TourDTO tourDTO)
        {
            var ticket = await _ticketService.BuyTicket(tourDTO);
            Log.Information("Here is your ticket. Have a nice trip");
            return Ok(ticket);
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
         
    }
}
