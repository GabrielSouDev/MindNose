using Microsoft.EntityFrameworkCore;
using MindNose.Domain.Entities.Chat;
using MindNose.Infrastructure.Persistence;

public class ConversationGuideRepository
{
    private readonly ApplicationDbContext _context;

    public ConversationGuideRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ConversationGuide?> GetByIdAsync(Guid id)
    {
        return await _context.ConversationGuides.Include(c => c.Messages)
                                                .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<ConversationGuideDisplay>?> GetGuideDisplayListByUserId(Guid id)
    {
        return await _context.ConversationGuides.Where(c => c.UserProfileId == id)
                                                .Select(c => new ConversationGuideDisplay() 
                                                    { 
                                                        Id = c.Id, 
                                                        ActualModel = c.ActualModel, 
                                                        CreatedAt = c.CreatedAt, 
                                                        LastModified = c.LastModified}
                                                    )
                                                .ToListAsync();
    }

    public async Task<List<ConversationGuide>> GetAllAsync()
    {
        return await _context.ConversationGuides.ToListAsync();
    }

    public async Task AddAsync(ConversationGuide conversationGuide)
    {
        await _context.ConversationGuides.AddAsync(conversationGuide);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ConversationGuide conversationGuide)
    {
        _context.ConversationGuides.Update(conversationGuide);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(ConversationGuide conversationGuide)
    {
        _context.ConversationGuides.Remove(conversationGuide);
        await _context.SaveChangesAsync();
    }
}
