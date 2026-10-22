using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Models.Buoi04;

namespace DBSportStore.Models
{
    public class StoreHomeViewModel
    {
        // Danh sách danh mục lấy trực tiếp từ bảng Category.
        public List<Category> Categories { get; set; }

        // Danh sách sản phẩm đang bán để Trang Chủ tiếp tục hiển thị.
        public List<ProductCardViewModel> Products { get; set; }

        // Từ khóa người dùng đang tìm kiếm.
        public string SearchTerm { get; set; }

        // Danh mục đang được chọn để lọc sản phẩm.
        public int? SelectedCategoryId { get; set; }

        // Trang hiện tại.
        public int CurrentPage { get; set; }

        // Tổng số trang.
        public int TotalPages { get; set; }

        // Giá tối thiểu người dùng nhập.
        public decimal? MinPrice { get; set; }

        // Giá tối đa người dùng nhập.
        public decimal? MaxPrice { get; set; }

        // Tổng số sản phẩm sau khi áp dụng các bộ lọc.
        public int TotalItems { get; set; }

        // Kiểu sắp xếp hiện tại.
        public string SortOrder { get; set; }


    }
}
