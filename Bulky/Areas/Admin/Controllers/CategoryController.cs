using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositorie.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.web.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CategoryController : Controller
	{
		//proprieter de type applicationDbcontext creer et en lecture seul
		private readonly IUnitOfWork _unitOfWork;

		//constructeur
		public CategoryController(IUnitOfWork unitOfWork) 
		{
			_unitOfWork = unitOfWork;
		}
		public IActionResult Index()
		{
			//recuperation de toutes les categories dans une liste
			List<Category> categorieslist = _unitOfWork.Category.GetAll().ToList();

			return View(categorieslist);
		}
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Create(Category cat)
		{
			if (ModelState.IsValid) {
				_unitOfWork.Category.Add(cat);
				_unitOfWork.Save();
				TempData["success"] = "Category created successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Edit(int? id) 
		{ 
			if(id == null || id == 0)
			{
				return NotFound();
			}
			//Recuperation de la category
			//La methode find ne fonctionne que sur la cle primaire
			//Category? catedit = _context.Categories.Find(id);
			//La methode FirstOrDefault fonctionne avec autre propriete que id
			Category? catedit1 = _unitOfWork.Category.Get(u => u.categoryId == id);
			if (catedit1 == null)
			{
				return NotFound();
			}
			return View(catedit1); 
		
		}

		[HttpPost]
		public IActionResult Edit(Category cat)
		{
			if (ModelState.IsValid)
			{
				_unitOfWork.Category.Update(cat);
				_unitOfWork.Save();
				TempData["success"] = "Category updated successfully";
				return RedirectToAction("Index");
			}
			return View();
		}

		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}
			//Recuperation de la category
			//La methode find ne fonctionne que sur la cle primaire
			//Category? catedit = _caterepos.Remove(id);
			//La methode FirstOrDefault fonctionne avec autre propriete que id
			Category? catedit1 = _unitOfWork.Category.Get(u => u.categoryId == id);
			if (catedit1 == null)
			{
				return NotFound();
			}
			return View(catedit1);

		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Category? cat = _unitOfWork.Category.Get(u => u.categoryId == id);
			if (cat == null)
			{
				return NotFound();
			}
			_unitOfWork.Category.Remove(cat);
			_unitOfWork.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");
	
		}
	}
	
}
