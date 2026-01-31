using MindNose.Domain.Entities;
using MindNose.Infrastructure.Persistence;

public class ChunkRepository
{
    private readonly ApplicationDbContext _context;

    public ChunkRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Chunk? GetById(int id)
    {
        return _context.Chunks.Find(id);
    }

    public List<Chunk> GetAll()
    {
        return _context.Chunks.ToList();
    }

    public void Add(Chunk chunk)
    {
        _context.Chunks.Add(chunk);
        _context.SaveChanges();
    }

    public void Update(Chunk chunk)
    {
        _context.Chunks.Update(chunk);
        _context.SaveChanges();
    }

    public void Remove(Chunk chunk)
    {
        _context.Chunks.Remove(chunk);
        _context.SaveChanges();
    }
}
