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
    [Authorize(Roles = "user", AuthenticationSchemes = "Bearer")]
    [Route("api/ticket")]
    [ApiController]
    public class TicketController:ControllerBase
    {
        private ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<ActionResult> BuyTicket([FromBody] TicketModel model)
        {
            TicketDTO ticket = Mapper.Map<TicketModel, TicketDTO>(model);
            var purchase = await _ticketService.BuyTicket(ticket);
            return Ok(purchase);
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
