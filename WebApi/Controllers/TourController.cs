using BLL.DTO;
using BLL.Interfaces;
using BLL.Infrastructure;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using WebApi.Models;
using AutoMapper;
using System.IO;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [ Authorize(Roles ="user, admin" , AuthenticationSchemes ="Bearer")]
    [Route("api/tour")]
    [ApiController]
    public class TourController:ControllerBase
    {
        private ITourService _tourService;
        public TourController(ITourService tourService)
        {
            _tourService = tourService;
        }

        [HttpDelete]
        [Route("{tourId}")]
        public async Task<ActionResult> DeleteTour(int tourId)
        {
            var operationDetails = await _tourService.DeleteTour(tourId);
            Log.Information($"Tour{tourId} is deleted");
            return Ok(operationDetails);
        }

        [HttpGet]
        [Route("{tourId}")]
        public IActionResult GetTour(int tourId)
        {
            TourDTO tour = _tourService.GetTour(tourId);
            Log.Information($"Your tour with id: {tourId}");
            return Ok(tour);
        }

        [HttpGet]
        public IActionResult GetAllTours()
        {
            IEnumerable<TourDTO> allTours = _tourService.GetAllTours();
            Log.Information("All visited tours");
            return Ok(allTours);
        }
        [HttpGet]
        [Route("visitedtours/{userId}")]
        public IActionResult GetAllToursUserVisited(string userId)
        {
            IEnumerable<TourDTO> visitedTours = _tourService.GetToursUserVisited(userId);
            Log.Information($"Visited tours by this user{userId} are here");
            return Ok(visitedTours);
        }

        
       

       

      

       

      

       
    }
}
