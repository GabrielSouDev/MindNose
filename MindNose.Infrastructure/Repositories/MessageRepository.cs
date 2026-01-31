using MindNose.Domain.Entities;
using MindNose.Infrastructure.Persistence;

public class MessageRepository
{
    private readonly ApplicationDbContext _context;

    public MessageRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Message? GetById(int id)
    {
        return _context.Messages.Find(id);
    }

    public List<Message> GetAll()
    {
        return _context.Messages.ToList();
    }

    public void Add(Message message)
    {
        _context.Messages.Add(message);
        _context.SaveChanges();
    }

    public void Update(Message message)
    {
        _context.Messages.Update(message);
        _context.SaveChanges();
    }

    public void Remove(Message message)
    {
        _context.Messages.Remove(message);
        _context.SaveChanges();
    }
}