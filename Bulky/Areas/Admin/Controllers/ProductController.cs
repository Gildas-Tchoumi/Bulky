using Bulky.DataAccess.Repositorie.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Bulky.web.Areas.Admin.Controllers
{
	[Area("Admin")]

	//controller pour la gestion des produits
	[Authorize(Roles = SD.Role_Admin)]
	public class ProductController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		//
		private readonly IWebHostEnvironment _webHostEnvironment;
		public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
		{
			this._unitOfWork = unitOfWork;
			_webHostEnvironment = webHostEnvironment;
		}

		// List of product
		public IActionResult Index()
		{
			// recuperer tous les produits et retourner la liste 

			//avec les categories et covertype
			//List<Product> Products = _unitOfWork.Product.GetAll(includeProperties: "Category, CoverType").ToList();

			//avec seulement les categories
			List<Product> Products = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return View(Products);
		}

		//vue de creation
		public IActionResult Upsert(int id)
		{
			//IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
			//{
			//	Text = c.Name,
			//	Value = c.categoryId.ToString()
			//});
			//using ViewBag to pass the category list to the view
			//ViewBag.CategoryList = categoryList;
			//using ViewData to pass the category list to the view
			//ViewData["CategoryList"] = categoryList;

			//using ProductVM to pass the category list to the view
			ProductVM productVM = new ProductVM()
			{
				Product = new Product(),
				CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.categoryId.ToString()
				})
			};
			if(id == 0 || id == null)
			{
				//create product
				return View(productVM);

			} else
			{
				//update product
				productVM.Product = _unitOfWork.Product.Get(p => p.Id == id);
					return View(productVM);
			}
		}

		//create product
		[HttpPost]
		public IActionResult Upsert(ProductVM productVM, IFormFile? file)
		{
			if (ModelState.IsValid)
			{
				//utiliser pour definir le chemin vers le repertoire racine wwwroot
				string wwwRootPath = _webHostEnvironment.WebRootPath;

				// si le fichier n'est pas vide on va le stocker dans le repertoire products de images
				if(file != null)
				{
					//renommer dabord le nom du fichier(ceci nous donne un nom aleatoire) + recuperation de l'extension du fichier telecharger
					String fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

					//definissons maintenant un chemin vers le repertoire product a traver une combinaison
					string productPath = Path.Combine(wwwRootPath, @"Images\Products");

					// verifier si le champ n'est pas vide,dans le cas de la mise a jour
					if(!string.IsNullOrEmpty(productVM.Product.ImageUrl))
					{
						//delete old image
						//recuperation de l'ancienne image en supprimant la bare oblique de depart
						var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

						//verifier si une image existe dans l'oldimagepath et la supprimer

						if(System.IO.File.Exists(oldImagePath))
						{
							//delete
							System.IO.File.Delete(oldImagePath);
						}

					}

					// enregistrement de l'image(filemode.create pour specifier la creation lorsqu'on utilise la classe FileStream)
					using (var fileStream = new FileStream(Path.Combine(productPath, fileName),FileMode.Create))
					{
						//copier l'image enregistree precedement dans filestream
						file.CopyTo(fileStream);
					}

					//Enregistre le chemin ou l'url de l'image dans la table produit
					productVM.Product.ImageUrl = @"\Images\Products\" + fileName;

				}
				// verification pour create ou update
				if(productVM.Product.Id == 0)
				{
				_unitOfWork.Product.Add(productVM.Product);
				} else
				{
					_unitOfWork.Product.Update(productVM.Product);
				}
					_unitOfWork.Save();
				TempData["success"] = "Product created successfully";
				return RedirectToAction("Index");
			} else
			{
				// repopulate the category list if the model state is invalid
				productVM.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.categoryId.ToString()
				});
			}
			return View(productVM);
		}
			

		// vue de modification
		//public IActionResult Edit(int? id)
		//{
		//	if(id == null || id == 0)
		//	{
		//		return NotFound();
		//	}

		//	Product? prod = _unitOfWork.Product.Get(p => p.Id == id);
		//	if (prod == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(prod);
		//}

		//[HttpPost]
		//public IActionResult Edit(Product product)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_unitOfWork.Product.Update(product);
		//		_unitOfWork.Save();
		//		TempData["success"] = "Product updated successfully";
		//		return RedirectToAction("Index");
		//	}
		//	return View();

		//}

		//delet product
		//public IActionResult Delete(int? id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}

		//	Product? prod = _unitOfWork.Product.Get(p => p.Id == id);
		//	if (prod == null)
		//	{
		//		return NotFound();
		//	}

		//	return View(prod);
		//}

		//[HttpPost, ActionName("Delete")]
		//public IActionResult Delete(int? id,Product prod)
		//{
		//	prod = _unitOfWork.Product.Get(u => u.Id == id);
		//	//var t = prod;
			
		//		_unitOfWork.Product.Remove(prod);
		//		_unitOfWork.Save();
		//		TempData["success"] = "Product deleted successfully";
		//		return RedirectToAction("Index");

		//}
		// API CALLS pour le datatable dans la vue index pour recuperer les produits en format json et les afficher dans le datatable 
		#region API CALLS
		[HttpGet]
		public IActionResult GetAll()
		{
			var productList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
			return Json(new { data = productList });
		}

		[HttpDelete]
		public IActionResult Delete(int? id)
		{
			var prod = _unitOfWork.Product.Get(u => u.Id == id);

			if (prod == null) { 
				return Json(new { success = false, message = "Error while deleting" });
			}
			//delete image from wwwroot
			var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, prod.ImageUrl.TrimStart('\\'));

			if (System.IO.File.Exists(oldImagePath))
			{
				//delete
				System.IO.File.Delete(oldImagePath);
			}
			_unitOfWork.Product.Remove(prod);
			_unitOfWork.Save();
			
			return Json(new { success = true, message = "Product deleted successfully" });
		}
		#endregion
	}
}
