using api.DTOs.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly ITokenService tokenService;
        private readonly SignInManager<AppUser> signInManager;
        #endregion

        #region CTOR
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.signInManager = signInManager;
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
                        return Ok(new NewUserDTO { Email = appUser.Email, Token= tokenService.CreateToken(appUser), UserName= appUser.UserName});
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


        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            try
            {
                if(!ModelState.IsValid)
                    return BadRequest(ModelState);

                AppUser user = await userManager.Users.FirstOrDefaultAsync(u => u.UserName == loginDTO.UserName.ToLower().Trim());

                if (user is null)
                    return Unauthorized("Invalid username or password");

                var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

                if (!result.Succeeded)
                {
                    return Unauthorized("Invalid username or password");
                }

                return Ok(new NewUserDTO { Email = user.Email, Token = tokenService.CreateToken(user), UserName = user.UserName });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
