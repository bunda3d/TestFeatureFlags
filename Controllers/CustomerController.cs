using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.Mvc;

using TestFeatureFlags.Data;
using TestFeatureFlags.ViewModels;

namespace TestFeatureFlags.Controllers
{
	[Route("[controller]")]
	public class CustomerController : Controller
	{
		private readonly ApplicationDbContext context;

		public CustomerController(ApplicationDbContext context)
		{
			this.context = context;
		}

		//private readonly ILogger<CustomerController> _logger;

		//public CustomerController(ILogger<CustomerController> logger)
		//{
		//	_logger = logger;
		//}

		//private readonly IFeatureManager _featureManager;

		//public CustomerController(IFeatureManagerSnapshot featureManager) =>
		//_featureManager = featureManager;

		//[FeatureGate(MyFeatureFlags.LateStops)]
		[HttpGet("Index")]
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult GetCustomers()
		{
			try
			{
				//number of times api is called for current table
				var draw = Request.Form["draw"].FirstOrDefault();
				//count of records to skip, used for paging in EF Core
				var start = Request.Form["start"].FirstOrDefault();
				//page size, from DDL 'showing n entries'
				var length = Request.Form["length"].FirstOrDefault();
				//current column set for sorting
				var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
				//asc or desc order
				var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
				//value from search box
				var searchValue = Request.Form["search[value]"].FirstOrDefault();
				int pageSize = length != null ? Convert.ToInt32(length) : 0;
				int skip = start != null ? Convert.ToInt32(start) : 0;
				int recordsTotal = 0;
				//gets IQueryable of DataSource
				var customerData = (from tempcustomer in context.Customers select tempcustomer);
				//sorting behavior
				if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
				{
					customerData = customerData.OrderBy(sortColumn + " " + sortColumnDirection);
				}
				//searching behavior (search each column specified)
				if (!string.IsNullOrEmpty(searchValue))
				{
					customerData = customerData.Where(m => m.FirstName.Contains(searchValue)
					|| m.LastName.Contains(searchValue)
					|| m.Contact.Contains(searchValue)
					|| m.Email.Contains(searchValue));
				}
				//gets count of returned records
				//NOTE: possibly most expensive query in controller. Could avoid by other scheme, like storing total records in another table, maybe?
				recordsTotal = customerData.Count();
				//performs paging using EF Core
				var data = customerData.Skip(skip).Take(pageSize).ToList();
				//sets data in required format, returns it
				var jsonData = new
				{
					draw = draw,
					recordsFiltered = recordsTotal,
					recordsTotal = recordsTotal,
					data = data
				};
				return Ok(jsonData);
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		//[HttpDelete("delete/{id:int}")]
		//public void Delete(int id)
		//{
		//	try
		//	{
		//		var company =
		//			 DataRepository.GetCompanies().FirstOrDefault(c => c.ID == id);
		//		if (company == null)
		//			return "Company cannot be found";
		//		DataRepository.GetCompanies().Remove(company);
		//		return "ok";
		//	}
		//	catch (Exception ex)
		//	{
		//		return ex.Message;
		//	}
		//}

		//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		//public IActionResult Error()
		//{
		//	return View(new ErrorVM { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		//}
	}
}