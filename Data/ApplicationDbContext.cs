using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TestFeatureFlags.Identity.Models;
using TestFeatureFlags.Models;

namespace TestFeatureFlags.Data
{
	public class ApplicationDbContext : IdentityDbContext<AppUser> //DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{
		}

		public DbSet<Customer> Customers { get; set; }
	}
}