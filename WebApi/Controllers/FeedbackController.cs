using BLL.DTO;
using BLL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;

namespace WebApi.Controllers
{
    [Authorize(Roles = "user", AuthenticationSchemes = "Bearer")]
    [Route("api/feedbacks")]
    [ApiController]
    public class FeedbackController:ControllerBase
    {
        private IUserService _userService;
        private ITicketService _ticketService;
        private IFeedbackService _feedbackService;

        public FeedbackController(IUserService userService, ITicketService ticketService, IFeedbackService feedbackService)
        {
            _userService = userService;
            _ticketService = ticketService;
            _feedbackService = feedbackService;
        }

        [HttpGet]
        [Route("feedbacks/userId/{UserId}")]
        public async Task<ActionResult> GetFeedbacksByUserId(string  userId)
        {
            ICollection<FeedbackDTO> feedbacks = await _feedbackService.GetAllFeedbacksByUserId(userId);
            Log.Information($"Here are all feedbacks by this user {userId}");
            return Ok(feedbacks);
        }

        [HttpGet]
        [Route("feedbacks/tourId/{tourId}")]
        public async Task<ActionResult> GetFeedbacksByTourId(int tourId)
        {
            ICollection<FeedbackDTO> feedbacks = await _feedbackService.GetAllFeedbacksByTourId(tourId);
            Log.Information($"Here are all feedbacks by this tour {tourId}");
            return Ok(feedbacks);
        }

        public async Task<ActionResult> GetFeedbacksSortedByDate(DateTime date)
        {
            ICollection<FeedbackDTO> feedbacks = await _feedbackService.GetAllFeedbacksSortedByDate(date);
            Log.Information($"Here are feedbacks sorted by date{date}");
            return Ok(feedbacks);
        }

    }
}
