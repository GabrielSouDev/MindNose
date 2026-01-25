using Microsoft.AspNetCore.Mvc;
using MindNose.Application.Services;
using MindNose.Domain.Request.User;

namespace MindNose.Apresentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;

    public AuthController(UserService userService)
    {
        _userService = userService;
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRequest userRequest)
    {
        var result = await _userService.Register(userRequest);

        if(!result.Succeeded)
            return BadRequest(result.Errors);

        return Created();
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        var token = await _userService.Login(loginRequest);

        if(string.IsNullOrEmpty(token))
            return Unauthorized();

        return Ok(new { token });
    }
}
