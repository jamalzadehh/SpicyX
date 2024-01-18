using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SpicyXPractice.Models;

namespace SpicyXPractice.Configurations
{
    public class ChecConfigration:IEntityTypeConfiguration<Chef>
    {
        public void Configure(EntityTypeBuilder<Chef> builder)
        {
            builder.Property(x=>x.FullName).IsRequired().HasMaxLength(256);
            builder.Property(x=>x.Position).IsRequired().HasMaxLength(256);
            
        }

    }
}
