namespace MindNose.Domain.Entities;

public abstract class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastModified { get; set; }

    public void Touch()
    {
        LastModified = DateTime.UtcNow;
    }
}