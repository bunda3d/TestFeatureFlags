using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;

using TestFeatureFlags.Identity.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TestFeatureFlags.Controllers
{
	[Authorize]
	public class ClaimsController : Controller
	{
		private UserManager<AppUser> userManager;
		private IAuthorizationService authService;

		public ClaimsController(UserManager<AppUser> userMgr, IAuthorizationService auth)
		{
			userManager = userMgr;
			authService = auth;
		}

		public ViewResult Index() => View(User?.Claims);

		public ViewResult Create() => View();

		[HttpPost]
		[ActionName("Create")]
		public async Task<IActionResult> Create_Post(string claimType, string claimValue)
		{
			AppUser user = await userManager.GetUserAsync(HttpContext.User);
			Claim claim = new Claim(claimType, claimValue, ClaimValueTypes.String);
			IdentityResult result = await userManager.AddClaimAsync(user, claim);

			if (result.Succeeded)
				return RedirectToAction("Index");
			else
				Errors(result);
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Delete(string claimValues)
		{
			AppUser user = await userManager.GetUserAsync(HttpContext.User);

			string[] claimValuesArray = claimValues.Split(";");
			string claimType = claimValuesArray[0],
				claimValue = claimValuesArray[1],
				claimIssuer = claimValuesArray[2];

			Claim claim = User.Claims.Where(
				x => x.Type == claimType &&
				x.Value == claimValue &&
				x.Issuer == claimIssuer).FirstOrDefault();

			IdentityResult result = await userManager.RemoveClaimAsync(user, claim);

			if (result.Succeeded)
				//return View("Index", User?.Claims);
				return RedirectToAction("Index");
			else
				Errors(result);

			return View("Index");
		}

		// action with claims permission policy (init in Startup)
		[Authorize(Policy = "AspManager")]
		public ViewResult Project() => View("Index", User?.Claims);

		[Authorize(Policy = "AllowTom")]
		public ViewResult TomFiles() => View("Index", User?.Claims);

		public async Task<IActionResult> PrivateAccess(string title)
		{ 
			//validate request against a policy: https://www.yogihosting.com/aspnet-core-identity-policies/#download 

			string[] allowedUsers = { "tom", "alice" };
			AuthorizationResult authorized = await authService.AuthorizeAsync(User, allowedUsers, "PrivateAccess");

			if (authorized.Succeeded)
				return View("Index", User?.Claims);
			else
				return new ChallengeResult();
		}

		private void Errors(IdentityResult result)
		{
			foreach (IdentityError error in result.Errors)
				ModelState.AddModelError("", error.Description);
		}
	}
}