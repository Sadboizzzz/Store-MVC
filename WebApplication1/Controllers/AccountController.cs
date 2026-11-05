using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        private readonly DBSportStoreEntities1 db = new DBSportStoreEntities1();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignUp(SignUpViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool userNameExists = db.UserAccounts.Any(u => u.UserName == model.UserName);
            if (userNameExists)
            {
                ModelState.AddModelError("UserName", "Tên đăng nhập đã tồn tại.");
                return View(model);
            }

            var customerRole = db.Roles.FirstOrDefault(r => r.RoleName == "Customer");
            if (customerRole == null)
            {
                ModelState.AddModelError("", "Không tìm thấy Role Customer.");
                return View(model);
            }

            string salt = PasswordHelper.CreateSalt();
            string passwordHash = PasswordHelper.Hash(model.Password, salt);

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    var account = new UserAccount
                    {
                        UserName = model.UserName,
                        PasswordHash = passwordHash,
                        PasswordSalt = salt,
                        RoleID = customerRole.RoleID,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    db.UserAccounts.Add(account);
                    db.SaveChanges();

                    var customer = new Customer
                    {
                        UserID = account.UserID,
                        FullName = model.FullName,
                        Phone = model.Phone,
                        Email = model.Email,
                        Address = model.Address
                    };

                    db.Customers.Add(customer);
                    db.SaveChanges();
                    transaction.Commit();

                    TempData["SignUpSuccess"] = "Đăng ký thành công. Vui lòng đăng nhập.";
                    return RedirectToAction("Login");
                }
                catch
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Có lỗi khi tạo tài khoản. Vui lòng thử lại.");
                    return View(model);
                }
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var account = db.UserAccounts
                .Include(u => u.Role)
                .FirstOrDefault(u => u.UserName == model.UserName && u.IsActive);

            if (account == null || !PasswordHelper.Verify(model.Password, account.PasswordSalt, account.PasswordHash))
            {
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không đúng.");
                return View(model);
            }

            Session["UserID"] = account.UserID;
            Session["UserName"] = account.UserName;
            Session["Role"] = account.Role.RoleName;

            var customer = db.Customers.FirstOrDefault(c => c.UserID == account.UserID);
            if (customer != null)
            {
                Session["CustomerID"] = customer.CustomerID;
                Session["FullName"] = customer.FullName;
            }
            else
            {
                Session.Remove("CustomerID");
                Session.Remove("FullName");
            }

            return RedirectToAction("Index", "Store");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            Session.Remove("UserID");
            Session.Remove("UserName");
            Session.Remove("Role");
            Session.Remove("CustomerID");
            Session.Remove("FullName");

            return RedirectToAction("Index", "Store");
        }

        public ActionResult AdminTest()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login");
            }

            string role = Session["Role"] == null ? "" : Session["Role"].ToString();
            if (role != "Admin")
            {
                return Content("Bạn không có quyền truy cập trang Admin.");
            }

            return Content("Đăng nhập Admin thành công. Bạn đang có quyền Admin.");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
