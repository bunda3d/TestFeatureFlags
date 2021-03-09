using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace TestFeatureFlags.Identity.Models
{
	public class Login
	{
		[Required]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
	}
}