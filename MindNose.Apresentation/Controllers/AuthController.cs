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
        var result = await _userService.RegisterUserAsync(userRequest);

        if(!result.Succeeded)
            return BadRequest(result.Errors);

        return CreatedAtAction(nameof(Register), new { email = userRequest.Email });
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = await _userService.LoginAsync(loginRequest);

        if(string.IsNullOrEmpty(token))
            return Unauthorized("E-mail ou senha inválidos.");

        return Ok(new { token });
    }
}
