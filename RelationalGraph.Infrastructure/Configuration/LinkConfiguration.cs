using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RelationalGraph.Domain.Entities;

namespace RelationalGraph.Infrastructure.Configurations;

public class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Weight)
               .IsRequired();

        builder.HasOne(l => l.Source)
               .WithMany(n => n.OutputLinks)
               .HasForeignKey(l => l.SourceId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(l => l.Target)
               .WithMany(n => n.InputLinks)
               .HasForeignKey(l => l.TargetId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
