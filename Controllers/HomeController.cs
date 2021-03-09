using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TestFeatureFlags.Identity.Models;
using TestFeatureFlags.Models;
using TestFeatureFlags.ViewModels;

namespace TestFeatureFlags.Controllers
{
	public class HomeController : Controller
	{
		// sentinel updates
		private readonly Settings _settings;
		private readonly ILogger<HomeController> _logger;
		private UserManager<AppUser> userManager;

		public HomeController(ILogger<HomeController> logger, IOptionsSnapshot<Settings> settings, UserManager<AppUser> userMgr)
		{
			_logger = logger;
			_settings = settings.Value;
			userManager = userMgr;
		}


		[Authorize]
		public async Task<IActionResult> IndexAsync()
		{
			ViewData["BackgroundColor"] = _settings.BackgroundColor;
			ViewData["FontSize"] = _settings.FontSize;
			ViewData["FontColor"] = _settings.FontColor;
			ViewData["Message"] = _settings.Message;

			AppUser user = await userManager.GetUserAsync(HttpContext.User);
			string message = "Hello little doggies named " + user.UserName;

			return View((object)message);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}