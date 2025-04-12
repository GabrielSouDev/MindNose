using Microsoft.EntityFrameworkCore;
using RelationalGraph.Domain.Entities;
using RelationalGraph.Infrastructure.Configurations;

namespace RelationalGraph.Infrastructure.Context;

public class RelationalGraphContext : DbContext
{
    public RelationalGraphContext(DbContextOptions<RelationalGraphContext> options)
        : base(options) { }
    public DbSet<Node> Nodes { get; set; }
    public DbSet<Link> Links { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<NodeCategory> NodeCategories { get; set; }
    public DbSet<NodeAccessLog> NodeAccessLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new NodeConfiguration());
        modelBuilder.ApplyConfiguration(new LinkConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        modelBuilder.ApplyConfiguration(new NodeCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new NodeAccessLogConfiguration());

        base.OnModelCreating(modelBuilder);
    }
}
