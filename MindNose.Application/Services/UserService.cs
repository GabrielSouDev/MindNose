using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MindNose.Domain.Configurations;
using MindNose.Domain.Consts;
using MindNose.Domain.Request.User;
using MindNose.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MindNose.Application.Services;

public class UserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IOptionsMonitor<JWTSettings> _jwtSettings;

    public UserService(UserManager<ApplicationUser> userManager, IOptionsMonitor<JWTSettings> jwtSettings)
    {
        _userManager = userManager;
        _jwtSettings = jwtSettings;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserRequest userRequest)
    {
        var user = new ApplicationUser
        {
            UserName = userRequest.UserName,
            Email = userRequest.Email,
        };

        var result = await _userManager.CreateAsync(user, userRequest.Password);
        await _userManager.AddToRoleAsync(user, Role.User);

        return result;
    }

    public async Task<string> LoginAsync(LoginRequest loginRequest)
    {
        var user = await _userManager.FindByEmailAsync(loginRequest.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            return string.Empty;

        var token = await GenarateJwtAsync(user);

        return token;
    }

    private async Task<string> GenarateJwtAsync(ApplicationUser user)
    {
        var jwtSettings = _jwtSettings.CurrentValue;

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(ClaimTypes.Name, user.UserName!),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: jwtSettings.Issuer,
            audience: jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(
                int.Parse(jwtSettings.ExpireMinutes)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}