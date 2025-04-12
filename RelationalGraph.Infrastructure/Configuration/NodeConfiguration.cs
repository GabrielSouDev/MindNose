using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RelationalGraph.Domain.Entities;

namespace RelationalGraph.Infrastructure.Configurations;

public class NodeConfiguration : IEntityTypeConfiguration<Node>
{
    public void Configure(EntityTypeBuilder<Node> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Term)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(n => n.Summary)
               .HasMaxLength(500);

        builder.HasIndex(n => n.Term).IsUnique();

        builder.HasMany(n => n.InputLinks)
               .WithOne(l => l.Target)
               .HasForeignKey(l => l.TargetId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(n => n.OutputLinks)
               .WithOne(l => l.Source)
               .HasForeignKey(l => l.SourceId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
