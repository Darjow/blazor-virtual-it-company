using Domain.VirtualMachines.VirtualMachine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Data.Configurations;

namespace Persistence.Data.Configuration
{
    internal class VirtualMachineEntityTypeConfiguration : EntityTypeConfiguration<VirtualMachine>
    {
        public override void Configure(EntityTypeBuilder<VirtualMachine> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.OperatingSystem).IsRequired();
            builder.Property(p => p.Name).IsRequired();
            builder.Property(p => p.Mode).IsRequired();
            builder.OwnsOne(p => p.Hardware, hardware =>
            {
                hardware.Property(h => h.Memory).HasColumnName("Memory").IsRequired();
                hardware.Property(h => h.Storage).HasColumnName("Storage").IsRequired();
                hardware.Property(h => h.Amount_vCPU).HasColumnName("Amount_vCPU").IsRequired();
            });

            builder.Ignore(e => e.Statistics);
         
        /*    builder.HasOne<VMContract>()
                .WithOne()
                .HasForeignKey<VMContract>(c => c.Id)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<VMConnection>()
                .WithOne()
                .HasForeignKey<VMConnection>(e => e.Id)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Backup>()
                .WithOne()
                .HasForeignKey<Backup>(e => e.Id)
                .OnDelete(DeleteBehavior.NoAction);*/



        }
    }
}