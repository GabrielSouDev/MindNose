using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RelationalGraph.Domain.Entities;

namespace RelationalGraph.Infrastructure.Configurations;

public class NodeAccessLogConfiguration : IEntityTypeConfiguration<NodeAccessLog>
{
    public void Configure(EntityTypeBuilder<NodeAccessLog> builder)
    {
        builder.HasKey(log => log.Id);

        builder.Property(log => log.AccessedAt)
               .IsRequired();

        builder.Property(log => log.AccessContext)
               .HasConversion<string>()
               .IsRequired();

        builder.HasIndex(log => new { log.NodeId, log.AccessedAt });

        builder.HasOne(log => log.Node)
               .WithMany()
               .HasForeignKey(log => log.NodeId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
