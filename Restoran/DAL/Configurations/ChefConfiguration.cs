using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Restoran.Models;

namespace Restoran.DAL.Configurations
{
    public class ChefConfiguration : IEntityTypeConfiguration<Chef>
    {
        public void Configure(EntityTypeBuilder<Chef> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(m => m.ImgUrl)
                .HasMaxLength(500);

            builder.Property(m => m.ChefDesignationId)
                .IsRequired();

            builder.Property(m => m.IsDeactive)
                .HasDefaultValue(false);

            builder.Property(m => m.TwitterUrl)
                .HasMaxLength(500);

            builder.Property(m => m.FacebookUrl)
                .HasMaxLength(500);

            builder.Property(m => m.InstagramUrl)
                .HasMaxLength(500);

        }
    }
}
