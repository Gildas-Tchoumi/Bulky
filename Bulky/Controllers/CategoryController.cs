using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositorie.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Controllers
{
	public class CategoryController : Controller
	{
		//proprieter de type applicationDbcontext creer et en lecture seul
		private readonly ICategoriesRepository _caterepos;

		//constructeur
		public CategoryController(ICategoriesRepository caterepos) 
		{
			_caterepos = caterepos;
		}
		public IActionResult Index()
		{
			//recuperation de toutes les categories dans une liste
			List<Category> categorieslist = _caterepos.GetAll().ToList();

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
				_caterepos.Add(cat);
				_caterepos.Save();
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
			Category? catedit1 = _caterepos.Get(u => u.categoryId == id);
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
				_caterepos.Update(cat);
				_caterepos.Save();
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
			Category? catedit1 = _caterepos.Get(u => u.categoryId == id);
			if (catedit1 == null)
			{
				return NotFound();
			}
			return View(catedit1);

		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Category? cat = _caterepos.Get(u => u.categoryId == id);
			if (cat == null)
			{
				return NotFound();
			}
			_caterepos.Remove(cat);
			_caterepos.Save();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");
	
		}
	}
	
}
