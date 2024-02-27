using Domain.Projecten;
using Domain.Users;
using Domain.VirtualMachines.VirtualMachine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Data.Configurations;

namespace Persistence.Data.Configuration
{
    internal class ProjectEntityTypeConfiguration : EntityTypeConfiguration<Project>
    {
        public override void Configure(EntityTypeBuilder<Project> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Name).IsRequired();


            /*builder.HasMany<VirtualMachine>()
                .WithOne()
                .HasForeignKey(e => e.Id);

  /        builder.HasOne<Klant>()
                .WithMany()
                .IsRequired();*/

        }
    }
}