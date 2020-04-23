using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CafeApps.Web.Models;
using static CafeApps.Web.Models.Table;
using static CafeApps.Web.Models.User;

namespace CafeApps.Web.Controllers
{
    public class CustomerController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Customer
        public ActionResult Index()
        {
            if (Session["UserId"] != null)
            {
                return View(db.Menus.ToList());
                
            }
            return RedirectToAction("CustomerLogin");
        }

        public ActionResult CustomerLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CustomerLogin(User user)
        {
            var CheckUser = db.Users.Where(u => u.Username == user.Username && u.Role == Roles.Customer).SingleOrDefault();

            if (CheckUser != null)
            {
                if (CheckUser.Password != user.Password)
                {
                    ViewBag.Error = "Invalid user";
                    return View();
                }
                Session["UserId"] = CheckUser.UserId;
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Invalid user";
            return View();
        }

        [HttpPost]
        public ActionResult AddMenuToCart(int id)
        {
            Cart cart = new Cart();
            cart.UsersId = Convert.ToInt32(Session["UserId"]);
            var getFoodId = db.Menus.SingleOrDefault(m => m.MenuId == id);
            var getCart = db.Carts.SingleOrDefault(c => c.MenusId == id);

            if (getCart != null)
            {

                getCart.Quantity += 1;
                getCart.TotalAmount = getCart.Quantity * getFoodId.Price;

                db.Entry(getCart).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Message = true, Msg = getFoodId.FoodName + " already added into cart" }, JsonRequestBehavior.AllowGet);
            }

            else
            {
                cart.MenusId = id;
                cart.Price = getFoodId.Price;
                cart.Quantity = 1;
                cart.TotalAmount = getFoodId.Price * cart.Quantity;
                cart.FoodName = getFoodId.FoodName;
                db.Carts.Add(cart);
                db.SaveChanges();
                return Json(new { Message = false, Msg = getFoodId.FoodName + " added into cart" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewCart()
        {
            if (Session["UserId"] != null)
            {
                int id = Convert.ToInt32(Session["UserId"]);
                return View(db.Carts.Where(c => c.UsersId == id).ToList());
            }
            return RedirectToAction("CustomerLogin");
        }

        [HttpPost]
        public ActionResult AddQuantity(int id)
        {
            var getCart = db.Carts.SingleOrDefault(c => c.CartId == id);
            var getFoodId = db.Menus.SingleOrDefault(m => m.MenuId == getCart.MenusId);
            if (getCart != null)
            {
                getCart.Quantity += 1;
                getCart.TotalAmount = getCart.Quantity * getFoodId.Price;
                db.Entry(getCart).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Message = true, Msg = getFoodId.FoodName + " already added into cart" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = false, Msg = "Invalid Item" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeductQuantity(int id)
        {
            var getCart = db.Carts.SingleOrDefault(c => c.CartId == id);
            var getFoodId = db.Menus.SingleOrDefault(m => m.MenuId == getCart.MenusId);
            if (getCart != null)
            {
                getCart.Quantity -= 1;
                getCart.TotalAmount = getCart.Quantity * getFoodId.Price;
                if (getCart.Quantity == 0)
                {
                    db.Carts.Remove(getCart);
                    db.SaveChanges();
                }
                db.Entry(getCart).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { Message = true, Msg = getFoodId.FoodName + " already deduct from cart" }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Message = false, Msg = "Invalid Item" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteFood(int id)
        {
            var getCart = db.Carts.SingleOrDefault(c => c.CartId == id);
            var getFoodId = db.Menus.SingleOrDefault(m => m.MenuId == getCart.MenusId);

            db.Carts.Remove(getCart);
            db.SaveChanges();
            return Json(new { Message = true, Msg = getFoodId.FoodName + " remove from cart" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmOrder()
        {
            Table table = new Table();
            if (Session["UserId"] != null)
            {
                int TableId = 1;
                //table.TableId = TableId;
                var GetTable = db.Tables.SingleOrDefault(t => t.TableId == TableId);
                int Id = Convert.ToInt32(Session["UserId"]);
                var User = db.Users.SingleOrDefault(u => u.UserId == Id);
                if (User != null)
                {
                   
                    if (table.TableId == TableId && table.Status == TableStatus.Occupied)
                    {
                        TableId++;

                        if (table.TableId == TableId && table.Status != TableStatus.Occupied)
                        {
                            GetTable.UserId = User.UserId;
                            GetTable.Status = TableStatus.Occupied;
                            db.Entry(GetTable).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                    GetTable.UserId = User.UserId;
                    GetTable.Status = TableStatus.Occupied;
                    db.Entry(GetTable).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return View(GetTable);
            }
            return RedirectToAction("CustomerLogin");
        }

        public ActionResult Logout()
        {
            Session["UserId"] = null;
            return RedirectToAction("Index", "Home");
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
