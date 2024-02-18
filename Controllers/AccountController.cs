﻿using api.DTOs.Account;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]

    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public sealed class AccountController : ControllerBase
    {
        #region FIELDS
        private readonly UserManager<AppUser> userManager;
        #endregion

        #region CTOR
        public AccountController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }
        #endregion

        [HttpPost(Name = "Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDTO.UserName,
                    Email = registerDTO.Email
                };

                var createdUser = await userManager.CreateAsync(appUser, registerDTO.Password);

                if (createdUser.Succeeded)
                {
                    var roleResult = await userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok("User created");
                    }
                    else
                    {
                        return StatusCode(StatusCodes.Status500InternalServerError, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, createdUser.Errors);
                }

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }





    }
}