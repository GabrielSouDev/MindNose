using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RelationalGraph.Domain.Entities;

namespace RelationalGraph.Infrastructure.Configurations;

public class NodeCategoryConfiguration : IEntityTypeConfiguration<NodeCategory>
{
    public void Configure(EntityTypeBuilder<NodeCategory> builder)
    {
        builder.HasKey(nc => new { nc.NodeId, nc.CategoryId }); // Chave composta

        builder
            .HasOne(nc => nc.Node)
            .WithMany(n => n.NodeCategories)
            .HasForeignKey(nc => nc.NodeId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(nc => nc.Category)
            .WithMany(c => c.NodeCategories)
            .HasForeignKey(nc => nc.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .Property(nc => nc.SourceType)
            .HasConversion<string>()
            .IsRequired();

        builder
            .Property(nc => nc.CreatedAt)
            .IsRequired();

        builder
            .Property(nc => nc.UpdatedAt)
            .IsRequired(false);
    }
}
