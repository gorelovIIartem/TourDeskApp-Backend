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

namespace WebApi.Controllers
{
    [Authorize(Roles = "user", AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
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
                Roles = new string[] { "user" }
            };
            OperationDetails operationDetails = await _userService.CreateUserAsync(userDTO);
            Log.Information($"User{userDTO.UserName} has been registered");
            return Ok(operationDetails);
        }
        [HttpDelete]
        [CheckCurrentUserFilter]
        [Route("delete/{userId}")]
        public async Task<ActionResult> DeleteUser(string userId)
        {
            await _userService.DeleteUser(userId);
            Log.Information($"User{userId} has been deleted");
            return Ok();
        }
        [HttpGet]
        [Route("{userId}")]
        [CheckCurrentUserFilter]
        public async Task<ActionResult> GetUser(string userId)
        {
            var user =  await _userService.FindUserByIdAsync(userId);
            return Ok(user);
        }

        [ModelValidationFilter]
        [HttpPut]
        [Route("{userId}")]
        public async Task<ActionResult> ChageUser([FromBody] UserProfileModel userModel)
        {
            UserDTO user = new UserDTO
            {
                Age = userModel.Age,
                Phone = userModel.Phone,
                Birthday = userModel.Birthday,
                Address = userModel.Address,
                FullName = userModel.FullName,
                Id = userModel.UserId,
                Email=userModel.Email,
                ImageUrl=userModel.ImageUrl
            };
            var operationDetails = await _userService.ChangeProfileInformation(user);
            return Ok(operationDetails);
        }
       
        [HttpPut]
        [CheckCurrentUserFilter]
        [Route("{userId}/uploadImage")]
        public async Task<ActionResult> UploadImage(string userId)
        {
            var user = await _userService.FindUserByIdAsync(userId);
            var postedFile = Request.Form.Files["Image"];
            if (postedFile == null)
            {
                Log.Warning($"User {userId} did not attach the file. 404 returned");
                return BadRequest("File has not been attached");
            }
            if ((postedFile.ContentType.Contains("jpg") || postedFile.ContentType.Contains("png") || postedFile.ContentType.Contains("jpeg")) == false)
            {
                Log.Warning($"User {userId} attached file with uncorrect format");
                return BadRequest("The file format is wrong");
            }
            var imageUrl = $@"C:\Users\gorel\source\repos\TDA-Frontend\src\assets\{postedFile.FileName}";
            using (var stream = new FileStream(imageUrl, FileMode.Create))
            {
                postedFile.CopyTo(stream);
            }
            OperationDetails operationDetails = await _userService.UploadImage(imageUrl, user.Id);
            Log.Information($"User {user.Id} changed image");
            return Ok(operationDetails);
        }

        [HttpGet]
        public async Task<ActionResult> GetUsers()
        {
            IEnumerable<UserDTO> users = await _userService.GetAllUsers();
            Log.Information("All users");
            return Ok(users);
        }

    }
}
