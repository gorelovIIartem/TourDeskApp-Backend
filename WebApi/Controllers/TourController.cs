using BLL.DTO;
using BLL.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Models;
using AutoMapper;

namespace WebApi.Controllers
{
    [Authorize(Roles ="guide", AuthenticationSchemes ="Bearer")]
    [Route("api/guide")]
    [ApiController]
    public class TourController:ControllerBase
    {
        private ITourService _tourService;
        private IUserService _userService;
        public TourController(ITourService tourService, IUserService userService)
        {
            _tourService = tourService;
            _userService = userService;
        }

        [HttpDelete]
        [Route("delete/{TourId}")]
        public async Task<ActionResult> DeleteTour(int tourId)
        {
            var operationDetails = await _tourService.DeleteTour(tourId);
            Log.Information($"Tour{tourId} is deleted");
            return Ok(operationDetails);
        }
        //[HttpGet]
        //[Route("/getall")]
        //[AllowAnonymous]
        //public IActionResult GetAllTours()
        //{
        //    return Ok(_tourService.)
        //}

        [HttpGet]
        public async Task<ActionResult> GetAllToursUserVisited(string userId)
        {
            ICollection<TourDTO> visitedTours =await _tourService.GetToursUserVisited(userId);
            Log.Information($"Visited tours by this user{userId} are here");
            return Ok(visitedTours);
        }

        [HttpPut]
        public async Task<ActionResult> CreateTour([FromBody] TourModel tourModel)
        { 
            var tourDTO = Mapper.Map<TourDTO>(tourModel);
            var operationDetails = await _tourService.AddTour(tourDTO);
            Log.Information($"Tour{tourDTO.Id} is added succesfully");
            return Ok(operationDetails);
        }
        public async Task<ActionResult> MakeSale(string userId, int tourId)
        {
            var user = await _userService.FindUserByIdAsync(userId);
            var tour = _tourService.GetTour(tourId);
            if (user.Birthday == tour.Date)
            {
                await _tourService.MakeSale(userId, tourId);
            }
            Log.Information($"Sale for{userId} made succesfully");
            return Ok();
        }
        
    }
}
