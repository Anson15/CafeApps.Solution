using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CafeApps.Web.Models;
using static CafeApps.Web.Models.User;

namespace CafeApps.Web.Controllers
{
    public class AdminController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult DetailsOfUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(User user)
        {
            var CheckUser = db.Users.Where(u => u.Username == user.Username && u.Role ==Roles.Admin).SingleOrDefault();

            if (CheckUser != null)
            {
                if (CheckUser.Password != user.Password)
                {
                    ViewBag.Error = "Invalid user";
                    return View();
                }
                Session["Id"] = CheckUser.UserId;
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Invalid user";
            return View();
        }

        public ActionResult CreateUser()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            db.Users.Add(user);
            return RedirectToAction("Index");
        }

        // GET: Users/Edit/5
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(User user)
        {
            //var updateUser = db.Users.Where(u => u.UserId == user.UserId).SingleOrDefault();
            db.Entry(user).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        // GET: Users/Delete/5
        public ActionResult DeleteUser(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ViewAllMenu()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            return View(db.Menus.ToList());
        }

        public ActionResult AddMenu()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddMenu(Menu menu)
        {
            string filename = Path.GetFileNameWithoutExtension(menu.ImageFile.FileName);
            string extension=Path.GetExtension(menu.ImageFile.FileName);
            filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
            menu.Image = "~/Image/" + filename;
            filename = Path.Combine(Server.MapPath("~/Image/"), filename);
            menu.ImageFile.SaveAs(filename);
            db.Menus.Add(menu);
            db.SaveChanges();
            return RedirectToAction("ViewAllMenu");
        }

        public ActionResult EditMenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        [HttpPost]
        public ActionResult EditMenu(Menu menu)
        {
            if (ModelState.IsValid)
            { 
                string filename = Path.GetFileNameWithoutExtension(menu.ImageFile.FileName);
                string extension = Path.GetExtension(menu.ImageFile.FileName);
                filename = filename + DateTime.Now.ToString("yymmssfff") + extension;
                menu.Image = "~/Image/" + filename;
                filename = Path.Combine(Server.MapPath("~/Image/"), filename);
                menu.ImageFile.SaveAs(filename);
                db.Entry(menu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewAllMenu");
            }
            return View(menu);
        }

        public ActionResult DeleteMenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("DeleteMenu")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMenuConfirmed(int id)
        {
            Menu menu = db.Menus.Find(id);
            db.Menus.Remove(menu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult DetailsOfMenu(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Menu menu = db.Menus.Find(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }
        public ActionResult AddTable()
        {
            if (Session["Id"] == null)
            {
                return RedirectToAction("AdminLogin");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddTable(Table table)
        {
            db.Tables.Add(table);
            db.SaveChanges();
            return RedirectToAction("ViewTableStatus");
        }

        public ActionResult ViewTableStatus()
        {
            if (Session["Id"] != null)
            {
                return View(db.Tables.ToList());
            }
            return RedirectToAction("AdminLogin");
        }

        public ActionResult Logout()
        {
            Session["Id"] = null;
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
