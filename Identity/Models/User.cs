﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

//good tutorial + explainer of UserManager class https://www.yogihosting.com/aspnet-core-identity-create-read-update-delete-users/#usermanager

namespace TestFeatureFlags.Identity.Models
{
	public class User
	{
		[Required]
		public string Name { get; set; }

		[Required]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
		public string Email { get; set; }

		[Required]
		public string Password { get; set; }

		public Country Country { get; set; }

		public int Age { get; set; }

		[Required]
		public string Salary { get; set; }
	}
}