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

namespace WebApi.Controllers
{
    //[ Authorize(Roles ="user" , AuthenticationSchemes ="Bearer")]
    [Route("api/tour")]
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
        //[Authorize(Roles ="admin")]
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

        [HttpPut]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult> CreateTour([FromBody] TourModel tourModel)
        {
            TourDTO tourDTO = new TourDTO
            {
                Name = tourModel.Name,
                City = tourModel.City,
                Location = tourModel.Location,
                Price = tourModel.Price,
                PlacesCount = tourModel.PlacesCount,
                Description = tourModel.Description,
                Date = tourModel.Date
            };
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

        [HttpPut]
        [Route("{tourId}/uploadImage")]
        public async Task<ActionResult> UploadPhoto( int tourId)
        {
            var tour = _tourService.GetTour(tourId);
            var postedFile = Request.Form.Files["Image"];
            if (postedFile == null)
            {
                Log.Warning($"tour {tourId} did not attach the file. 404 returned");
                return BadRequest("File has not been attached");
            }
            if ((postedFile.ContentType.Contains("jpg") || postedFile.ContentType.Contains("png") || postedFile.ContentType.Contains("jpeg")) == false)
            {
                Log.Warning($"Tour {tourId} attached file with uncorrect format");
                return BadRequest("The file format is wrong");
            }
            var imageUrl = $@"C:\Users\gorel\source\repos\TDA-Frontend\src\assets\{postedFile.FileName}";
            using (var stream = new FileStream(imageUrl, FileMode.Create))
            {
                postedFile.CopyTo(stream);
            }
            OperationDetails operationDetails = await _tourService.UploadImage(imageUrl, tour.Id);
            Log.Information($"User {tour.Id} changed image");
            return Ok(operationDetails);
        }

        [HttpGet]
        [Route("guide/{tourId}")]
        public async Task<ActionResult> FindGuideForTour(int tourid)
        {
            var tour = _tourService.GetTour(tourid);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", "Incorrect Id");
            UserDTO userDTO = await _userService.GetGuideByTourId(tourid);
            Log.Information($"Here is guide {userDTO.Id} for this tour");
            return Ok(userDTO);
        }
        
        [HttpPut]
        [Route("guide/{userId}")]
        public async Task<ActionResult> MakeGuideForTour(int tourId, string userId)
        {
            var tour = _tourService.GetTour(tourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", $"{tourId}");
            var user = await _userService.FindUserByIdAsync(userId);
            if (user == null)
                throw new ValidationException("There is no information about this user", userId);
            var operationDetails = await _tourService.MakeGuide(tourId, userId);
            Log.Information("Guide ", userId);
            return Ok(operationDetails);
        }

        [HttpPut]
        [Route("{tourId}")]
        public async Task<ActionResult> ChangeTour([FromBody] TourModel tourModel)
        {
            TourDTO tourDTO = new TourDTO
            {
                Name = tourModel.Name,
                Location = tourModel.Location,
                City = tourModel.City,
                Description = tourModel.Description,
                Date = tourModel.Date,
                ImageUrl = tourModel.ImageUrl,
                PlacesCount = tourModel.PlacesCount,
                UserId = tourModel.UserId,
                Id = tourModel.Id,
                Price = tourModel.Price
            };
            var operationDetails = await _tourService.ChangeTourInformation(tourDTO);
            return Ok(operationDetails);
        }
    }
}
