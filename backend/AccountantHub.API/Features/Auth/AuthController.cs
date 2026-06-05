using AccountantHub.Infrastructure.Identity;
using AccountantHub.Infrastructure.Persistence.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AccountantHub.API.Features.Auth;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IJwtTokenService _jwtTokenService;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenService = jwtTokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ValidationErrorResponse());
        }

        var existing = await _userManager.FindByEmailAsync(request.Email);
        if (existing is not null)
        {
            return Conflict(new
            {
                success = false,
                message = "Email is already registered.",
                data = (object?)null,
                meta = (object?)null
            });
        }

        var user = new ApplicationUser
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName.Trim()
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var message = string.Join(" ", result.Errors.Select(e => e.Description));
            if (result.Errors.Any(e => e.Code == "DuplicateUserName" || e.Code == "DuplicateEmail"))
            {
                return Conflict(new
                {
                    success = false,
                    message = "Email is already registered.",
                    data = (object?)null,
                    meta = (object?)null
                });
            }

            return BadRequest(new
            {
                success = false,
                message,
                data = (object?)null,
                meta = (object?)null
            });
        }

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new
        {
            success = true,
            message = "OK",
            data = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                FullName = user.FullName
            },
            meta = (object?)null
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ValidationErrorResponse());
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid email or password.",
                data = (object?)null,
                meta = (object?)null
            });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            return Unauthorized(new
            {
                success = false,
                message = "Invalid email or password.",
                data = (object?)null,
                meta = (object?)null
            });
        }

        var token = _jwtTokenService.GenerateToken(user);
        return Ok(new
        {
            success = true,
            message = "OK",
            data = new AuthResponseDto
            {
                Token = token,
                Email = user.Email!,
                FullName = user.FullName
            },
            meta = (object?)null
        });
    }

    private object ValidationErrorResponse()
    {
        var message = string.Join(
            " ",
            ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage));

        return new
        {
            success = false,
            message,
            data = (object?)null,
            meta = (object?)null
        };
    }
}
