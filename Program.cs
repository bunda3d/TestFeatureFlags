using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TestFeatureFlags
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		// public static IHostBuilder CreateHostBuilder(string[] args) =>
		//     Host.CreateDefaultBuilder(args)
		//         .ConfigureWebHostDefaults(webBuilder =>
		//         {
		//             webBuilder.UseStartup<Startup>();
		//         });

		/** code block for azure configuration explorer keys that don't auto update with 'sentinel' */
		//public static IHostBuilder CreateHostBuilder(string[] args) =>
		//	Host.CreateDefaultBuilder(args)
		//		.ConfigureWebHostDefaults(webBuilder =>
		//			webBuilder.ConfigureAppConfiguration(config =>
		//			{
		//				var settings = config.Build();
		//				var connection = settings.GetConnectionString("AppConfig");
		//				config.AddAzureAppConfiguration(options =>
		//					options.Connect(connection).UseFeatureFlags());
		//			}).UseStartup<Startup>());

		/** AzureAppConfig for 'configuration explorer' keys that use auto refresh (of optionsConfig) via 'sentinel' */

		// may want to lower SetCacheExpiration time for testing refresh (h, m, s)
		public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
		webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
		{
			var settings = config.Build();
			config.AddAzureAppConfiguration(options =>
			{
				options.Connect(settings["ConnectionStrings:AppConfig"])
				.ConfigureRefresh(refresh =>
				{
					refresh.Register("TestApp:Settings:Sentinel", refreshAll: true).SetCacheExpiration(new TimeSpan(0, 1, 0));
				});
			});
		}).UseStartup<Startup>());
	}
}