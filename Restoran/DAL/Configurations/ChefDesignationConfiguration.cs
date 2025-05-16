using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restoran.Models;

namespace Restoran.DAL.Configurations
{
    public class ChefDesignationConfiguration : IEntityTypeConfiguration<ChefDesignation>
    {
        public void Configure(EntityTypeBuilder<ChefDesignation> builder)
        {
            builder.HasKey(md => md.Id);

            builder.Property(md => md.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.HasIndex(md => md.Name)
                   .IsUnique();
        }
    }
}
