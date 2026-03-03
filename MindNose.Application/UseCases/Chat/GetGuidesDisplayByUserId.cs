using Microsoft.AspNetCore.Http;
using MindNose.Domain.Entities.Chat;
using MindNose.Domain.Interfaces.UseCases.Chat;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MindNose.Application.UseCases.Chat;

public class GetGuidesDisplayByUserId : IGetGuidesDisplayByUserId
{
    private readonly ConversationGuideRepository _guideRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public GetGuidesDisplayByUserId(ConversationGuideRepository guideRepository, IHttpContextAccessor httpContextAccessor)
    {
        _guideRepository = guideRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<ConversationGuideDisplay>?> ExecuteAsync()
    {
        var userId = GetUserId();
        var guides = await _guideRepository.GetGuideDisplayListByUserId(userId);
        
        return guides;
    }

    private Guid GetUserId()
    {
        var subValue = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        Guid userId;
        if (Guid.TryParse(subValue, out userId))
        {
            return userId;
        }
        else
        {
            throw new Exception("Claim 'sub' não é um GUID válido.");
        }
    }
}
