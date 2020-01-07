using System.Threading.Tasks;
using WebApi.Models;
using BLL.Interfaces;
using BLL.DTO;
using BLL.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using BLL.Services;
using Serilog;
using System.IO;
using System.Collections.Generic;
using AutoMapper;

namespace WebApi.Controllers
{
    [Authorize(Roles = "admin", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        public IUserService _userService;
        public ITourService _tourService;

        public AdminController(IUserService userService, ITourService tourService)
        {
            _userService = userService;
            _tourService = tourService;
        }


        [ModelValidationFilter]
        [AllowAnonymous]
        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody]RegisterModel model)
        {

            UserDTO userDTO = new UserDTO
            {
                Password = model.Password,
                UserName = model.UserName,
                FullName = model.FullName,
                Role = "admin"
            };
            OperationDetails operationDetails = await _userService.CreateUserAsync(userDTO);
            Log.Information($"admin {userDTO.UserName} has been registered");
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

        [HttpPut]
        [Route("{tourId}/{userId}")]
        public async Task<ActionResult> MakeGuideForTour(int tourId, string userId)
        {
            TourDTO tour = await _tourService.GetTour(tourId);
            if (tour == null)
                throw new ValidationException("There is no information about this tour", $"{tourId}");
            if (tour.UserId != null)
                throw new ValidationException("This tour has already guide!", tour.UserId);
            var user = await _userService.FindUserByIdAsync(userId);
            if (user == null)
                throw new ValidationException("There is no information about this user", userId);
            var operationDetails = await _tourService.MakeGuide(tourId, userId);
            Log.Information("Guide ", userId);
            return Ok(operationDetails);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<ActionResult> GetAllToursForGuide(string userId)
        {
            UserDTO user = await _userService.FindUserByIdAsync(userId);
            if (user == null)
                throw new ValidationException("There is no information about this user", userId);
            IEnumerable<TourDTO> tours = await _tourService.GetGuideTours(userId);
            Log.Information("Tours are got succesfully");
            return Ok(tours);
        }

        [HttpPut]
        [Route("{tourId}/uploadImage")]
        public async Task<ActionResult> UploadPhoto(int tourId)
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

        [HttpPut]
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

        [HttpDelete]
        [Route("{tourId}")]
        public async Task<ActionResult> DeleteTour(int tourId)
        {
            var operationDetails = await _tourService.DeleteTour(tourId);
            Log.Information($"Tour{tourId} is deleted");
            return Ok(operationDetails);
        }
    }
}
