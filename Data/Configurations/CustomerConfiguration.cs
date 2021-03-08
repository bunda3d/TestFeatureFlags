using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using TestFeatureFlags.Models;

namespace TestFeatureFlags.Data.Configurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		public virtual void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.ToTable("Customer");
			builder.HasKey(k => k.Id);
			builder.Property(p => p.FirstName);
			builder.Property(p => p.LastName);
			builder.Property(p => p.Contact);
			builder.Property(p => p.Email);
			builder.Property(p => p.DateOfBirth);
		}
	}
}