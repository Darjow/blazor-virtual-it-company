using Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Data.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Configuration
{
    internal class ContactPersonEntityTypeConfiguation: EntityTypeConfiguration<ContactDetails>
    {
        public override void Configure(EntityTypeBuilder<ContactDetails> builder)
        {
            base.Configure(builder);
            builder.Property(c => c.FirstName).IsRequired();
            builder.Property(c => c.LastName).IsRequired();
            builder.Property(c => c.PhoneNumber).IsRequired();
            builder.Property(c => c.Email).IsRequired();


        }

    }
}
