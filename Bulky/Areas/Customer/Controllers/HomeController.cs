using Bulky.DataAccess.Repositorie.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bulky.web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork _unitOfWork;

		public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

		}

        public IActionResult Index()
        {
			//recuperer tous les produits et retourner la liste avec les categories
            IEnumerable<Product> Products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return View(Products);
        }

		public IActionResult Details(int? id)
		{
			//recuperer tous les produits et retourner la liste avec les categories
			Product Product = _unitOfWork.Product.Get(u => u.Id == id,includeProperties: "Category");
			return View(Product);
		}

		public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
