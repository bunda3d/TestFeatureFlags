using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.FeatureManagement;
using TestFeatureFlags.Identity.Models;

using TestFeatureFlags.Identity.Policy;
using TestFeatureFlags.Controllers;
using TestFeatureFlags.Data;

namespace TestFeatureFlags
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
				b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

			services.AddTransient<IPasswordValidator<AppUser>, CustomPasswordPolicy>();
			services.AddTransient<IUserValidator<AppUser>, CustomUsernameEmailPolicy>();
			services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
			services.Configure<IdentityOptions>(opts =>
			{
				opts.User.RequireUniqueEmail = true;
				opts.Password.RequiredLength = 8;
				opts.Password.RequireNonAlphanumeric = true;
				opts.Password.RequireLowercase = false;
				opts.Password.RequireUppercase = true;
				opts.Password.RequireDigit = true;
			});
			services.ConfigureApplicationCookie(opts => 
			{
				opts.LoginPath = "/Account/Login";
				opts.Cookie.Name = ".AspNetCore.Identity.Application";
				opts.ExpireTimeSpan = TimeSpan.FromMinutes(20);
				opts.SlidingExpiration = true;
			});

			services.AddControllers();
			services.AddRazorPages();
			services.AddFeatureManagement();
			services.Configure<Settings>(Configuration.GetSection("TestApp:Settings"));
			services.AddControllersWithViews();
			services.AddAzureAppConfiguration();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				//app.UseHsts();
			}
			app.UseAzureAppConfiguration();

			app.UseHttpsRedirection();

			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
				endpoints.MapRazorPages();
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}