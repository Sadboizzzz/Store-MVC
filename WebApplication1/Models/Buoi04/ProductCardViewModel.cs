using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Buoi04
{
    public class ProductCardViewModel

    {

        public int ProductID { get; set; } // mã sản phẩm

        public int CategoryID { get; set; } // mã danh mục

        public string NamePro { get; set; } // tên sản phẩm

        public string CategoryName { get; set; } // tên danh mục

        public decimal Price { get; set; } // đơn giá

        public string ImagePro { get; set; } // tên file ảnh trong Content/images



    }
}