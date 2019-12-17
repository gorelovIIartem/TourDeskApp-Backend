using System.Threading.Tasks;
using WebApi.Models;
using BLL.Interfaces;
using BLL.DTO;
using BLL.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;
using Serilog;

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
        [Route("/delete/{userId}")]
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
                Email=userModel.Email
            };
            var operationDetails = await _userService.ChangeProfileInformation(user);
            return Ok(operationDetails);
        }
       


    }
}
