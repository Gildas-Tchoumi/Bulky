using Bulky.DataAccess.Data;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bulky.Controllers
{
	public class CategoryController : Controller
	{
		//proprieter de type applicationDbcontext creer et en lecture seul
		private readonly ApplicationDbContext _context;

		//constructeur
		public CategoryController(ApplicationDbContext db) 
		{ 
			_context = db;
		}
		public IActionResult Index()
		{
			//recuperation de toutes les categories dans une liste
			List<Category> categorieslist = _context.Categories.ToList();

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
				_context.Categories.Add(cat);
				_context.SaveChanges();
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
			Category? catedit = _context.Categories.Find(id);
			//La methode FirstOrDefault fonctionne avec autre propriete que id
			Category? catedit1 = _context.Categories.FirstOrDefault(u=>u.categoryId == id);
			if (catedit == null)
			{
				return NotFound();
			}
			return View(catedit); 
		
		}

		[HttpPost]
		public IActionResult Edit(Category cat)
		{
			if (ModelState.IsValid)
			{
				_context.Categories.Update(cat);
				_context.SaveChanges();
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
			Category? catedit = _context.Categories.Find(id);
			//La methode FirstOrDefault fonctionne avec autre propriete que id
			//Category? catedit1 = _context.Categories.FirstOrDefault(u => u.categoryId == id);
			if (catedit == null)
			{
				return NotFound();
			}
			return View(catedit);

		}

		[HttpPost, ActionName("Delete")]
		public IActionResult DeletePost(int? id)
		{
			Category? cat = _context.Categories.Find(id);
			if (cat == null)
			{
				return NotFound();
			}
			_context.Categories.Remove(cat);
				_context.SaveChanges();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");
	
		}
	}
	
}
