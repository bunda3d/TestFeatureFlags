using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TestFeatureFlags.Identity.Models;

namespace TestFeatureFlags.Data.Configurations
{
	public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
	{
		public virtual void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.ToTable("AppUser");

			builder.Property(p => p.CreatedBy).HasMaxLength(128).IsRequired();
			builder.Property(p => p.CreatedDate).HasColumnType("DateTime2").IsRequired();
			builder.Property(p => p.ModifiedBy).HasMaxLength(128).IsRequired();
			builder.Property(p => p.ModifiedDate).HasColumnType("DateTime2").IsRequired();
		}
	}
}