using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using TestFeatureFlags.Models;
using TestFeatureFlags.ViewModels;

namespace TestFeatureFlags.Controllers
{
	public class HomeController : Controller
	{
		// sentinel updates
		private readonly Settings _settings;

		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger, IOptionsSnapshot<Settings> settings)
		{
			_logger = logger;
			_settings = settings.Value;
		}

		public IActionResult Index()
		{
			ViewData["BackgroundColor"] = _settings.BackgroundColor;
			ViewData["FontSize"] = _settings.FontSize;
			ViewData["FontColor"] = _settings.FontColor;
			ViewData["Message"] = _settings.Message;

			return View();
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