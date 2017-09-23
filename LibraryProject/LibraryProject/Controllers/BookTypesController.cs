using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryProject.DAL;
using LibraryProject.Models;

namespace LibraryProject.Controllers
{
    public class BookTypesController : Controller
    {
        private LibraryContext db = new LibraryContext();

        // GET: BookTypes
        public ActionResult Index()
        {
            return View(db.BookTypes.ToList());
        }

        // GET: BookTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookType bookType = db.BookTypes.Find(id);
            if (bookType == null)
            {
                return HttpNotFound();
            }
            return View(bookType);
        }

        // GET: BookTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BookTypes/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,BookName,BookAmount")] BookType bookType)
        {
            if (ModelState.IsValid)
            {
                db.BookTypes.Add(bookType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bookType);
        }

        // GET: BookTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookType bookType = db.BookTypes.Find(id);
            if (bookType == null)
            {
                return HttpNotFound();
            }
            return View(bookType);
        }

        // POST: BookTypes/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,BookName,BookAmount")] BookType bookType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bookType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bookType);
        }

        // GET: BookTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BookType bookType = db.BookTypes.Find(id);
            if (bookType == null)
            {
                return HttpNotFound();
            }
            return View(bookType);
        }

        // POST: BookTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BookType bookType = db.BookTypes.Find(id);
            db.BookTypes.Remove(bookType);
            db.SaveChanges();
            return RedirectToAction("Index");
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
