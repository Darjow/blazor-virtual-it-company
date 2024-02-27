using Ardalis.GuardClauses;
using Domain.Common;
using Domain.Server;
using Domain.VirtualMachines.VirtualMachine;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Data.Configurations;

namespace Persistence.Data.Configuration
{
    internal class FysiekeServerEntityTypeConfiguration : EntityTypeConfiguration<FysiekeServer>
    {
        public override void Configure(EntityTypeBuilder<FysiekeServer> builder)
        {
            base.Configure(builder);
            builder.Property(p => p.Naam).IsRequired();
            builder.Property(p => p.ServerAddress).IsRequired();
            builder.OwnsOne(f => f.Hardware, hardware =>
            {
                hardware.Property(h => h.Memory).IsRequired();
                hardware.Property(h => h.Storage).IsRequired();
                hardware.Property(h => h.Amount_vCPU).IsRequired();

            });
            //https://github.com/dotnet/efcore/issues/24614  <- cant have 2 same owned types  so i made 2 base types of hardware to fix this problem. (ugly)

            builder.OwnsOne(f => f.HardwareAvailable, hardware =>
            {
                hardware.Property(h => h.Memory).IsRequired();
                hardware.Property(h => h.Storage).IsRequired();
                hardware.Property(h => h.Amount_vCPU).IsRequired();

            });




           // builder.Ignore(f => f.HardwareAvailable);
        }
    }
}
