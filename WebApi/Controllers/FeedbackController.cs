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
    [Authorize(Roles = "user, admin", AuthenticationSchemes = "Bearer")]
    [Route("api/feedbacks")]
    [ApiController]
    public class FeedbackController:ControllerBase
    {
        private IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet]
        [Route("user/{UserId}")]
        public async Task<ActionResult> GetFeedbacksByUserId(string userId)
        {
            ICollection<FeedbackDTO> feedbacks = await _feedbackService.GetAllFeedbacksByUserId(userId);
            Log.Information($"Here are all feedbacks by this user {userId}");
            return Ok(feedbacks);
        }

        [HttpGet]
        [Route("tour/{tourId}")]
        public IActionResult GetFeedbacksByTourId(int tourId)
        {
            IEnumerable<FeedbackDTO> feedbacks = _feedbackService.GetAllFeedbacksByTourId(tourId);
            Log.Information($"Here are all feedbacks by this tour {tourId}");
            return Ok(feedbacks);
        }

        [HttpDelete]
        [Route("{feedbackId}")]
       public async Task<ActionResult> DeleteFeedback(int feedbackId)
        {
            var operationdetails = await _feedbackService.DeleteFeedback(feedbackId);
            Log.Information($"Feedback {feedbackId} is deleted");
            return Ok(operationdetails);
        }

        [HttpPut]
        public async Task<ActionResult> CreateFeedback([FromBody] FeedbackModel feedbackModel)
        {
            FeedbackDTO feedbackDTO = new FeedbackDTO
            {
                Id = feedbackModel.Id,
                Content = feedbackModel.Content,
                CreationDate = DateTime.Now,
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
