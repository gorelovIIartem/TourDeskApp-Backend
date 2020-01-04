using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using BLL.Interfaces;
using WebApi.Configuration;
using WebApi.Models;
using Serilog;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController:ControllerBase
    {
        private IUserService _userService { get; set; }

        public AuthorizationController(IUserService userService)
        {
            _userService = userService;
        }

        private async Task<ClaimsIdentity> GetIdentity(string userName, string password)
        {
            var user = await _userService.FindUserAsync(userName, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id),
                };
                var userRole = await _userService.GetRolesByUserId(user.Id);
                foreach (string roleName in userRole)
                {
                    claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName));
                }
                var claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }
            return null;
        }

        [HttpPost]
        [Route("token")]
        public async Task<ActionResult> Token(LoginModel person)
        {
            var identity = await GetIdentity(person.UserName, person.Password);
            if(identity==null)
            {
                Log.Warning($"User{person.UserName} doesn't exist in database");
                return BadRequest("User doesn't exist");
            }
            var result = GenerateToken(identity);
            string userId = string.Empty;
            var roles = new List<string>();
            foreach(var claim in identity.Claims)
            {
                if (claim.Type == ClaimsIdentity.DefaultNameClaimType) userId = claim.Value;
                if (claim.Type == ClaimsIdentity.DefaultRoleClaimType) roles.Add(claim.Value);
            }
            if(result==null)
            {
                Log.Warning($"User{person.UserName} doesn't exist in database or doesn't have claims");
                return NotFound();
            }
            else
            {
                return Ok(new { result, userId, roles });
            }
        }

        private string GenerateToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSummetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(jwt);

        }
    }
}
