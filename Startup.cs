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
using Microsoft.AspNetCore.Authorization;
using TestFeatureFlags.Identity.CustomPolicy;

using TestFeatureFlags.TagHelpers;
using TestFeatureFlags.Identity.Models;
using TestFeatureFlags.Models;
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
				opts.AccessDeniedPath = "/Account/AccessDenied";
				opts.Cookie.Name = ".AspNetCore.Identity.Application";
				opts.ExpireTimeSpan = TimeSpan.FromMinutes(20);
				opts.SlidingExpiration = true;
			});

			services.AddAuthentication()
	.AddGoogle(opts =>
	{
		opts.ClientId = "460161640272-dskc56sn2co51pjr740ehe21uqnue88u.apps.googleusercontent.com";
		opts.ClientSecret = "8V0m3z5QnpqRV1e_47sW6UYo";
		opts.SignInScheme = IdentityConstants.ExternalScheme;
	});

			// claims permission policy https://www.yogihosting.com/aspnet-core-identity-policies/#create

			services.AddAuthorization(opts =>
			{
				opts.AddPolicy("AspManager", policy =>
				{
					policy.RequireRole("Manager");
					policy.RequireClaim("permission", "late-stops.no-view");
				});
			});
			services.AddTransient<IAuthorizationHandler, AllowUsersHandler>();
			services.AddAuthorization(opts =>
			{
				opts.AddPolicy("AllowTom", policy =>
				{
					policy.AddRequirements(new AllowUserPolicy("tom"));
				});
			});
			services.AddTransient<IAuthorizationHandler, AllowPrivateHandler>();
			services.AddAuthorization(opts =>
			{
				opts.AddPolicy("PrivateAccess", policy =>
				{
					policy.AddRequirements(new AllowPrivatePolicy());
				});
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
				app.UseHsts();
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