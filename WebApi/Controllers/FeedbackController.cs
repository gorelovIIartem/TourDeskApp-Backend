using BLL.DTO;
using BLL.Interfaces;
using WebApi.Models;
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

        //work
        [HttpGet]
        [Route("user/{UserId}")]
        public async Task<ActionResult> GetFeedbacksByUserId(string userId)
        {
            ICollection<FeedbackDTO> feedbacks = await _feedbackService.GetAllFeedbacksByUserId(userId);
            Log.Information($"Here are all feedbacks by this user {userId}");
            return Ok(feedbacks);
        }

        //work
        [HttpGet]
        [Route("tour/{tourId}")]
        public async Task<ActionResult> GetFeedbacksByTourId(int tourId)
        {
            IEnumerable<FeedbackDTO> feedbacks = await _feedbackService.GetAllFeedbacksByTourId(tourId);
            Log.Information($"Here are all feedbacks by this tour {tourId}");
            return Ok(feedbacks);
        }

        //work
        [HttpGet]
        [Route("sorted")]
        public async Task<ActionResult> GetFeedbacksSortedByDate(DateTime date)
        {
            ICollection<FeedbackDTO> feedbacks = await _feedbackService.GetAllFeedbacksSortedByDate(date);
            Log.Information($"Here are feedbacks sorted by date{date}");
            return Ok(feedbacks);
        }

        //work
        [HttpDelete]
        [Route("{feedbackId}")]
        public async Task<ActionResult> DeleteFeedback(int feedbackId)
        {
            var operationdetails = await _feedbackService.DeleteFeedback(feedbackId);
            Log.Information($"Feedback {feedbackId} is deleted");
            return Ok(operationdetails);
        }

        //work
        [HttpPut]
        public async Task<ActionResult> CreateFeedback([FromBody] FeedbackModel feedbackModel)
        {
            FeedbackDTO feedbackDTO = new FeedbackDTO
            {
                Id = feedbackModel.Id,
                Content = feedbackModel.Content,
                CreationDate = feedbackModel.CreationDate,
                UserId = feedbackModel.UserId,
                UserName = feedbackModel.UserName,
                TourId = feedbackModel.TourId
            };
            var operationDetails = await _feedbackService.AddFeedback(feedbackDTO);
            Log.Information($"Feedback {feedbackDTO.Id} is added succesfully");
            return Ok(operationDetails);
        }

    }
}
