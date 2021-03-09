using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace TestFeatureFlags.Identity.Models
{
	public class AppUser : IdentityUser
	{
		public Country Country { get; set; }
		public int Age { get; set; }

		[Required]
		public string Salary { get; set; }

		public virtual string CreatedBy { get; set; }
		public virtual DateTime CreatedDate { get; set; }
		public virtual string ModifiedBy { get; set; }
		public virtual DateTime ModifiedDate { get; set; }
	}
}