using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CafeApps.Web.Models;
using static CafeApps.Web.Models.User;

namespace CafeApps.Web.Controllers
{
    public class EmployeeController : Controller
    {
        private AppDbContext db = new AppDbContext();

        // GET: Employee
        public ActionResult Index()
        {
            if (Session["EmployeeId"] != null)
            {
                return View(db.Tables.ToList());
            }
            return RedirectToAction("EmployeeLogin");
        }

        public ActionResult EmployeeLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmployeeLogin(User user)
        {
            var CheckUser = db.Users.Where(u => u.Username == user.Username && u.Role == Roles.Cashier).SingleOrDefault();

            if (CheckUser != null)
            {
                if (CheckUser.Password != user.Password)
                {
                    ViewBag.Error = "Invalid user";
                    return View();
                }
                Session["EmployeeId"] = CheckUser.UserId;
                return RedirectToAction("Index");
            }
            ViewBag.Error = "Invalid user";
            return View();
        }

        // GET: Employee/Edit/5
        public ActionResult EditTableStatus(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Table table = db.Tables.Find(id);
            if (table == null)
            {
                return HttpNotFound();
            }
            return View(table);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTableStatus(Table table)
        {
            if (ModelState.IsValid)
            {
                if (table.Status == Table.TableStatus.Empty)
                {
                    table.UserId = null;
                    db.Entry(table).State = EntityState.Modified;
                    db.SaveChanges();
                }
                db.Entry(table).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(table);
        }
        public ActionResult Logout()
        {
            Session["EmployeeId"] = null;
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
