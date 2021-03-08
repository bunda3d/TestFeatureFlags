using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestFeatureFlags.Controllers
{
	public class Settings
	{
		public string BackgroundColor { get; set; }
		public float FontSize { get; set; }
		public string FontColor { get; set; }
		public string Message { get; set; }
	}
}