using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;
using WebApplication1.Models.Buoi04;

namespace WebApplication1.Controllers
{
    public class StoreController : Controller
    {
        // Sửa DBSportStoreModel thành DBSportStoreEntities
        private DBSportStoreEntities1 db = new DBSportStoreEntities1();

        // GET: Store
        // Action Index xử lý URL /Store/Index
        public ActionResult Index()
        {
            // Lấy các sản phẩm từ database và chuyển sang ProductCardViewModel
            var products = db.Products
                .Where(p => p.IsActive)
                .Select(p => new ProductCardViewModel
                {
                    ProductID = p.ProductID,
                    NamePro = p.ProductName,
                    CategoryName = p.Category.CategoryName,
                    Price = p.Price,
                    ImagePro = p.ImageUrl
                })
                .ToList();

            // Truyền products sang View Index
            return View(products);
        }
    }
}