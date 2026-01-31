using MindNose.Domain.Entities;
using MindNose.Infrastructure.Persistence;

public class UserProfileRepository
{
    private readonly ApplicationDbContext _context;

    public UserProfileRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public UserProfile? GetById(int id)
    {
        return _context.UserProfiles.Find(id);
    }

    public List<UserProfile> GetAll()
    {
        return _context.UserProfiles.ToList();
    }

    public void Add(UserProfile userProfile)
    {
        _context.UserProfiles.Add(userProfile);
        _context.SaveChanges();
    }

    public void Update(UserProfile userProfile)
    {
        _context.UserProfiles.Update(userProfile);
        _context.SaveChanges();
    }

    public void Remove(UserProfile userProfile)
    {
        _context.UserProfiles.Remove(userProfile);
        _context.SaveChanges();
    }
}
