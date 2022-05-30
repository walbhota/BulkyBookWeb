using Microsoft.AspNetCore.Mvc;
using BulkyBookWeb.Data;
using BulkyBookWeb.Models;

namespace BulkyBookWeb.Controllers;

public class CategoryController : Controller
{
	private readonly ApplicationDbContext _db;

	public CategoryController (ApplicationDbContext db)
	{
		_db = db;

	}

	public IActionResult Index(string SearchText = "")
	{
		List<Category> categories;
		if (SearchText != "" && SearchText != null)
		{
			categories = _db.Categories.Where(p => p.Name.Contains(SearchText)).ToList();
			if(categories.Count == 0)
            {
				TempData["error"] = "No result for searched key";
				return RedirectToAction("Index");

			}
		}
		else
			
		categories = _db.Categories.ToList();

		
		return View(categories);

	}

	//GET ACTION METHOD
	public IActionResult Create()
	{
		return View();
	}

    //POST ACTION METHOD
    [HttpPost]
    [ValidateAntiForgeryToken]
	public IActionResult Create(Category obj)
	{
		if(obj.Name == obj.DisplayOrder.ToString())
        {
			ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
        }
		if(ModelState.IsValid){
		_db.Categories.Add(obj);
		_db.SaveChanges();
			TempData["success"] = "Category created successfully";
		return RedirectToAction("Index");
			}
		return View(obj);
	}

	//GET ACTION METHOD
	public IActionResult Edit(int? id)
	{
		if (id == null || id ==0)
        {
			return NotFound();
        }
		var categoryFromDb = _db.Categories.Find(id);
		//var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
		//var categoryFromDbSingle = _db.Categories.SingleOrDefault(u=>u.Id==id);

		if(categoryFromDb == null)
        {
			return NotFound();
        }
		return View(categoryFromDb);
	}

	//POST ACTION METHOD
	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult Edit(Category obj)
	{
		if (obj.Name == obj.DisplayOrder.ToString())
		{
			ModelState.AddModelError("name", "The Display Order cannot exactly match the Name.");
		}
		if (ModelState.IsValid)
		{
			_db.Categories.Update(obj);
			_db.SaveChanges();
			TempData["success"] = "Category updated successfully";
			return RedirectToAction("Index");
		}
		return View(obj);
	}

	//GET ACTION METHOD
	public IActionResult Delete(int? id)
	{
		if (id == null || id == 0)
		{
			return NotFound();
		}
		var categoryFromDb = _db.Categories.Find(id);
		//var categoryFromDbFirst = _db.Categories.FirstOrDefault(u=>u.Id==id);
		//var categoryFromDbSingle = _db.Categories.SingleOrDefault(u=>u.Id==id);

		if (categoryFromDb == null)
		{
			return NotFound();
		}
		return View(categoryFromDb);
	}

	//POST ACTION METHOD
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public IActionResult DeleteConfirmed(int? id)
	{
		var obj = _db.Categories.Find(id);
		if (obj == null)
        {
			return NotFound();
        }
			_db.Categories.Remove(obj);
			_db.SaveChanges();
		TempData["success"] = "Category deleted successfully";
		return RedirectToAction("Index");
	}

	//GET ACTION METHOD


	
}
