using MindNose.Domain.Entities;
using MindNose.Infrastructure.Persistence;

public class ConversationGuideRepository
{
    private readonly ApplicationDbContext _context;

    public ConversationGuideRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public ConversationGuide? GetById(int id)
    {
        return _context.ConversationGuides.Find(id);
    }

    public List<ConversationGuide> GetAll()
    {
        return _context.ConversationGuides.ToList();
    }

    public void Add(ConversationGuide conversationGuide)
    {
        _context.ConversationGuides.Add(conversationGuide);
        _context.SaveChanges();
    }

    public void Update(ConversationGuide conversationGuide)
    {
        _context.ConversationGuides.Update(conversationGuide);
        _context.SaveChanges();
    }

    public void Remove(ConversationGuide conversationGuide)
    {
        _context.ConversationGuides.Remove(conversationGuide);
        _context.SaveChanges();
    }
}
