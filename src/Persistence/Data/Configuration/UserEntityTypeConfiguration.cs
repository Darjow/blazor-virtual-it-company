using Domain;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Data.Configurations;

namespace Persistence.Data.Configuration
{
    internal class UserEntityTypeConfiguration : EntityTypeConfiguration<Gebruiker>
    {
        public override void Configure(EntityTypeBuilder<Gebruiker> builder)
        {
            
            base.Configure(builder);

            builder
                .HasDiscriminator<string>("GebruikerType")
                .HasValue<InterneKlant>("InterneKlant")
                .HasValue<ExterneKlant>("ExterneKlant")
                .HasValue<Administrator>("Administrator");
        }

    
    }
}