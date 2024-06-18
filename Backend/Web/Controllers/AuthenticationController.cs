﻿using Application.Services;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Filters;
using Web.ViewModels;

namespace Web.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IJwtService _jwtService;

        public AuthenticationController(
            UserManager<IdentityUser> userManager,
            IJwtService jwtService,
            ILogger<AuthenticationController> logger)
        {
            _userManager = userManager;
            _logger = logger;
            _jwtService = jwtService;
        }

        [AnonymousOnly]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid data");
                return BadRequest(ModelState);
            }

            IdentityUser? user = await _userManager.FindByEmailAsync(registerViewModel.Email);

            if (user is not null)
            {
                _logger.LogError($"User with name \"{registerViewModel.Email}\" is already exist!");
                return BadRequest($"User with email \"{registerViewModel.Email}\" already exists");
            }

            IdentityUser newUser = new() { Email = registerViewModel.Email, UserName = registerViewModel.UserName };

            IdentityResult isSucessfullyCreated = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (isSucessfullyCreated.Succeeded)
            {
                AuthTokens tokens = await _jwtService.GenerateJwtTokens(newUser);

                _logger.LogInformation("User was sucessfully registered!");

                return Ok(tokens);
            }

            return BadRequest(isSucessfullyCreated.Errors.FirstOrDefault());
        }

        [AnonymousOnly]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid data");
                return BadRequest(ModelState);
            }

            IdentityUser? user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user is null)
            {
                _logger.LogInformation($"User with email \"{loginViewModel.Email} doe not exist\"");
                return NotFound($"User with email \"{loginViewModel.Email} doe not exist\"");
            }

            bool isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (!isPasswordCorrect)
            {
                _logger.LogError("Invalid password");
                return BadRequest("Invalid password");
            }

            AuthTokens tokens = await _jwtService.GenerateJwtTokens(user);

            return Ok(tokens);
        }

        [Authorize]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid data");
                return BadRequest(ModelState);
            }

            AuthTokens? tokens = await _jwtService.VerifyAndGenerateToken(new() {
                MainToken = tokenRequest.MainToken,
                RefreshToken = tokenRequest.RefreshToken
            }, _userManager);

            if(tokens is null)
            {
                _logger.LogError("Tokens are invalid");
                return BadRequest("Tokens are invalid");
            }

            return Ok(tokens);
        }
    }
}