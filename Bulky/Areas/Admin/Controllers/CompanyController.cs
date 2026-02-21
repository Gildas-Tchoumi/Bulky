using Bulky.DataAccess.Repositorie.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky.web.Areas.Admin.Controllers
{

	[Area("Admin")]

	[Authorize(Roles = SD.Role_Admin)]
	public class CompanyController : Controller
	{
		private readonly IUnitOfWork _unitOfWork;

		public CompanyController(IUnitOfWork unitOfWork) 
		{
			_unitOfWork = unitOfWork;
		}

		public IActionResult Index()
		{
			//recuperation de toutes les categories dans une liste
			List<Company> companieslist = _unitOfWork.Company.GetAll().ToList();

			return View(companieslist);
		}

		//vue de creation
		public IActionResult Upsert(int id)
		{
			CompanyVM companyVM = new CompanyVM()
			{
				Company = new Company()
			};
			if (id == 0 || id == null)
			{
				//create company
				return View(companyVM);
			}
			else
			{
				//update company
				Company? company = _unitOfWork.Company.Get(c => c.Id == id);
				companyVM.Company = company;
				return View(companyVM);
			}
		}

		//create Company
		[HttpPost]
		public IActionResult Upsert(CompanyVM companyVM)
		{
			if (ModelState.IsValid)
			{
				// verification pour create ou update
				if (companyVM.Company.Id == 0)
				{
					_unitOfWork.Company.Add(companyVM.Company);
				}
				else
				{
					_unitOfWork.Company.Update(companyVM.Company);
				}
				_unitOfWork.Save();
				TempData["success"] = companyVM.Company.Id == 0 ? "Company created successfully" : "Company updated successfully";
				return RedirectToAction("Index");
			}
			
			return View(companyVM);
		}


		#region API CALLS

		[HttpGet]
			public IActionResult GetAll()
			{
				List<Company> companieslist = _unitOfWork.Company.GetAll().ToList();
				return Json(new { data = companieslist });
			}
			[HttpDelete]
			public IActionResult Delete(int? id)
			{
				Company? company = _unitOfWork.Company.Get(c => c.Id == id);
				if (company == null)
				{
					return Json(new { success = false, message = "Error while deleting" });
				}
				_unitOfWork.Company.Remove(company);
				_unitOfWork.Save();
				return Json(new { success = true, message = "Company deleted successfully" });
		}

		#endregion

		//[HttpPost]
		//public IActionResult Create(Category cat)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_unitOfWork.Category.Add(cat);
		//		_unitOfWork.Save();
		//		TempData["success"] = "Category created successfully";
		//		return RedirectToAction("Index");
		//	}
		//	return View();
		//}

		//public IActionResult Edit(int? id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}
		//	//Recuperation de la category
		//	//La methode find ne fonctionne que sur la cle primaire
		//	//Category? catedit = _context.Categories.Find(id);
		//	//La methode FirstOrDefault fonctionne avec autre propriete que id
		//	Category? catedit1 = _unitOfWork.Category.Get(u => u.categoryId == id);
		//	if (catedit1 == null)
		//	{
		//		return NotFound();
		//	}
		//	return View(catedit1);

		//}

		//[HttpPost]
		//public IActionResult Edit(Category cat)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_unitOfWork.Category.Update(cat);
		//		_unitOfWork.Save();
		//		TempData["success"] = "Category updated successfully";
		//		return RedirectToAction("Index");
		//	}
		//	return View();
		//}

		//public IActionResult Delete(int? id)
		//{
		//	if (id == null || id == 0)
		//	{
		//		return NotFound();
		//	}
		//	//Recuperation de la category
		//	//La methode find ne fonctionne que sur la cle primaire
		//	//Category? catedit = _caterepos.Remove(id);
		//	//La methode FirstOrDefault fonctionne avec autre propriete que id
		//	Category? catedit1 = _unitOfWork.Category.Get(u => u.categoryId == id);
		//	if (catedit1 == null)
		//	{
		//		return NotFound();
		//	}
		//	return View(catedit1);

		//}

		//[HttpPost, ActionName("Delete")]
		//public IActionResult DeletePost(int? id)
		//{
		//	Category? cat = _unitOfWork.Category.Get(u => u.categoryId == id);
		//	if (cat == null)
		//	{
		//		return NotFound();
		//	}
		//	_unitOfWork.Category.Remove(cat);
		//	_unitOfWork.Save();
		//	TempData["success"] = "Category deleted successfully";
		//	return RedirectToAction("Index");

		//}
	}


}
