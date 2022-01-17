﻿using Manage.Core.Entities;
using Manage.Infrastructure.Data;
using Manage.WebApi.Interface;
using Manage.WebApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Manage.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ManageContext _manageConetxt;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IAdministrationPageService _administrationPageService;

        public AccountController( ManageContext manageConetxt ,UserManager<ApplicationUser> userManager 
            ,SignInManager<ApplicationUser> signInManager
            ,RoleManager<ApplicationRole> roleManager
            ,IConfiguration configuration , 
            IAdministrationPageService administrationPageService) 
        {
            _manageConetxt = manageConetxt;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _administrationPageService = administrationPageService;
        }


        [HttpPost("Auth")]
        public async Task<IActionResult> Auth([FromBody] TokenRequestViewModel model)
        {
            // return a generic HTTP Status 500 (Server Error)
            // if the client payload is invalid.
            if (model == null) return new StatusCodeResult(500);

            switch (model.grant_type)
            {
                case "password":
                    return await GetToken(model);
                case "refresh_token":
                    return await RefreshToken(model);
                default:
                    // not supported - return a HTTP 401 (Unauthorized)
                    return new UnauthorizedResult();
            }
        }

        private async Task<IActionResult> GetToken(TokenRequestViewModel model)
        {
            try
            {
                // check if there's an user with the given username
                var user = await _userManager.FindByNameAsync(model.username);
                // fallback to support e-mail address instead of username
                if (user == null && model.username.Contains("@"))
                    user = await _userManager.FindByEmailAsync(model.username);

                if (user == null
                    || !await _userManager.CheckPasswordAsync(user, model.password))
                {
                    // user does not exists or password mismatch
                    return new UnauthorizedResult();
                }

                // username & password matches: create the refresh token
                var rt = CreateRefreshToken(model.client_id, user.Id);

                // add the new refresh token to the DB
                _manageConetxt.Tokens.Add(rt);
                _manageConetxt.SaveChanges();

                // create & return the access token
                var t = await CreateAccessToken(user.Id, rt.Value , model.role_name);
                //return Json(t);
                return Ok(t);
            }
            catch (Exception ex)
            {
                return new UnauthorizedResult();
            }
        }

        private Token CreateRefreshToken(string clientId, string userId)
        {
            return new Token()
            {
                ClientId = clientId,
                UserId = userId,
                Type = 0,
                Value = Guid.NewGuid().ToString("N"),
                CreatedDate = DateTime.UtcNow
            };
        }

        private async Task<TokenResponseViewModel> CreateAccessToken(string userId, string refreshToken , string roleName )
        {
            DateTime now = DateTime.UtcNow;

            var user = await _userManager.FindByIdAsync(userId);
     
            var role = await _userManager.GetRolesAsync(user);
            var rName = roleName;
        
            if (role.Contains(roleName))
            {
                // add the registered claims for JWT (RFC7519).
                // For more info, see https://tools.ietf.org/html/rfc7519#section-4.1
                var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(now).ToUnixTimeSeconds().ToString()),
                //new Claim(ClaimTypes.Role,role[3])
                new Claim(ClaimTypes.Role,rName)
              
                // TODO: add additional claims here
            };

                var tokenExpirationMins =
                    _configuration.GetValue<int>("Auth:Jwt:TokenExpirationInMinutes");
                var issuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_configuration["Auth:Jwt:Key"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["Auth:Jwt:Issuer"],
                    audience: _configuration["Auth:Jwt:Audience"],
                    claims: claims,
                    notBefore: now,
                    expires: now.Add(TimeSpan.FromMinutes(tokenExpirationMins)),
                    signingCredentials: new SigningCredentials(
                        issuerSigningKey, SecurityAlgorithms.HmacSha256)
                );
                var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);
                string profilePicturePath = "";
                var userPersonalData = _manageConetxt.EmployeePersonalDetails.Where(x => x.Id == userId).FirstOrDefault();

                if (userPersonalData == null)
                {
                    profilePicturePath = "";
                }
                else
                {

                    UpdateEmployeePersonalDetailsPhoto(userPersonalData);

                    profilePicturePath = userPersonalData.PhotoPath;

                }
                return new TokenResponseViewModel()
                {
                    token = encodedToken,
                    expiration = tokenExpirationMins,
                    refresh_token = refreshToken,
                    role_name = rName,
                    userId = userId,
                    profile_picture_path = profilePicturePath,
                    user_name = user.FullName

                };
            }
            else
            {
                return new TokenResponseViewModel();
            }
          
        }

        private async Task<IActionResult> RefreshToken(TokenRequestViewModel model)
        {
            try
            {
                // check if the received refreshToken exists for the given clientId
                var rt = _manageConetxt.Tokens
                    .FirstOrDefault(t =>
                    t.ClientId == model.client_id
                    && t.Value == model.refresh_token);

                if (rt == null)
                {
                    // refresh token not found or invalid (or invalid clientId)
                    return new UnauthorizedResult();
                }

                // check if there's an user with the refresh token's userId
                var user = await _userManager.FindByIdAsync(rt.UserId);

                if (user == null)
                {
                    // UserId not found or invalid
                    return new UnauthorizedResult();
                }

                // generate a new refresh token
                var rtNew = CreateRefreshToken(rt.ClientId, rt.UserId);

                // invalidate the old refresh token (by deleting it)
                _manageConetxt.Tokens.Remove(rt);

                // add the new refresh token
                _manageConetxt.Tokens.Add(rtNew);

                // persist changes in the DB
                _manageConetxt.SaveChanges();

                // create a new access token...
                var response = await CreateAccessToken(rtNew.UserId, rtNew.Value , model.role_name);

                // ... and send it to the client
                return Ok(response);
            }
            catch (Exception ex)
            {
                return new UnauthorizedResult();
            }
        }

        private void UpdateEmployeePersonalDetailsPhoto(EmployeePersonalDetails model)
        {
            if (model == null ||
                   string.IsNullOrEmpty(model.PhotoPath))
            {
                return;
            }

            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads/img",
             model.PhotoPath);
            if (!System.IO.File.Exists(photoPath))
            {
                model.PhotoPath = null;
                return;
            }

            var photoBytes = System.IO.File.ReadAllBytes(photoPath);

            var fileExtension = model.PhotoPath.Split('.')[1];

            model.PhotoPath =
                $"data:image/{fileExtension};base64,{Convert.ToBase64String(photoBytes)}";

        }



    }
}
