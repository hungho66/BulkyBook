using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //UnitOfWork thay cho DbContext (Repository Pattern)
        private readonly IUnitOfWork _unitOfWork;
        //DI
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            Category category = new Category();
            if(id == null)
            {
                //This is for create
                return View(category);
            }

            //This is for Edit
            category = _unitOfWork.Category.Get(id.GetValueOrDefault());
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category category)
        {
            if (ModelState.IsValid) //Kiểm tra các Atribute có hợp lệ ở model Category hay ko, giống Validation
            {
                if (category.Id == 0)
                {
                    _unitOfWork.Category.Add(category);//Nếu không có Id thì Create
                }
                else
                {
                    _unitOfWork.Category.Update(category);
                }
                _unitOfWork.Save(); //Save này nằm ở IUnitOfWork
                return RedirectToAction("Index");
            }
            return View(category);
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {

            var allObj = _unitOfWork.Category.GetAll();
            return Json(new { data = allObj });
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {

            var objFromDb = _unitOfWork.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
