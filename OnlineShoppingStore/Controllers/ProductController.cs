using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OnlineShopEdmx.Model;
using OnlineShopping.Models;
using OnlineShopping.Repository;
using OnlineShopping.ViewModel;

namespace OnlineShopping.Controllers
{
    public class ProductController : Controller
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        private Populator populator = new Populator();

        private readonly IHostingEnvironment hostingEnvo;

        public ProductController(IHostingEnvironment hostingEnvo)
        {

            this.hostingEnvo = hostingEnvo;
        }
        private List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<CategoryDetail>().GetAllRecords();
            foreach (var item in cat)
            {
                list.Add(new SelectListItem
                {
                    Value = item.CategoryId.ToString(),
                    Text = item.CategoryName
                });
            }
            return list;
        }
        public async Task<IActionResult> Index()
        {
            return await Task.Run(() => View(new ProductViewModelLst
            {
                dbModelLst = _unitOfWork.GetRepositoryInstance<ProductDetail>().GetAllRecordsIQueryable().Where(i => i.IsDelete == false).ToList()
            }));
        }
        public ActionResult Create()
        {
            ViewBag.CategoryList = GetCategory();
            return PartialView(new ProductViewModel
            {
                ddlIsActive = populator.GetPairModel("IsActive"),
                dbModel = new ProductDetail
                {
                    IsDelete = false,
                    CreatedDate = DateTime.Now
                }

            });
        }
        [HttpPost]
        public ActionResult Create(ProductViewModel tbl, IFormFile file)
        {
            string uniqueFileName = null;
            if (tbl.image != null)
            {
                if (imageValidation(tbl.image.FileName) == true)
                {
                    string uploadsFolder = Path.Combine(hostingEnvo.WebRootPath, "productImages");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + tbl.image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    tbl.image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
            }
            tbl.dbModel.ProductImage = uniqueFileName;
            tbl.dbModel.CreatedDate = tbl.dbModel.ModifiedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<ProductDetail>().Add(tbl.dbModel);
            return RedirectToAction("Create");
        }
        public ActionResult Edit(int id)
        {
            ViewBag.CategoryList = GetCategory();

            return PartialView(new ProductViewModel
            {
                dbModel = _unitOfWork.GetRepositoryInstance<ProductDetail>().GetFirstorDefault(id),
                ddlIsActive = populator.GetPairModel("IsActive"),
            });
        }
        [HttpPost]
        public ActionResult Edit(ProductViewModel tbl)
        {
            string uniqueFileName = null;
            if (tbl.image != null)
            {
                if (imageValidation(tbl.image.FileName) == true)
                {
                    string uploadsFolder = Path.Combine(hostingEnvo.WebRootPath, "productImages");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + tbl.image.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    tbl.image.CopyTo(new FileStream(filePath, FileMode.Create));
                }
            }
            tbl.dbModel.ProductImage = uniqueFileName;
            tbl.dbModel.ModifiedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<ProductDetail>().Update(tbl.dbModel);
            return RedirectToAction("Index");
        }


        public bool imageValidation(String imageName = "null.doc")
        {
            string ext = System.IO.Path.GetExtension(imageName);
            switch (ext)
            {
                case (".jpg"):
                    return true;
                    break;
                case ".png":
                    return true;
                    break;
                case ".gif":
                    return true;
                    break;
                case ".jpeg":
                    return true;
                    break;
                case null:
                    return true;
                    break;
                default:
                    return false;
            }
        }


    }
}