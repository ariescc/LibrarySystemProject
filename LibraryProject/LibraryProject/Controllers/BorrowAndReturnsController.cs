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
    public class BorrowAndReturnsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: BorrowAndReturns
        public ActionResult Index()
        {
            return View(unitOfWork.BorrowAndReturnRepository.Get());
        }

        // GET: BorrowAndReturns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //BorrowAndReturn borrowAndReturn = db.BorrowAndReturns.Find(id);
            BorrowAndReturn borrowAndReturn = unitOfWork.BorrowAndReturnRepository.GetByID(id);
            if (borrowAndReturn == null)
            {
                return HttpNotFound();
            }
            return View(borrowAndReturn);
        }

        // GET: BorrowAndReturns/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BorrowAndReturns/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserID,BookID,BorrowTime,ReturnTime,ExpiredDays,IsReturn")] BorrowAndReturn borrowAndReturn)
        {
            if (ModelState.IsValid)
            {
                //db.BorrowAndReturns.Add(borrowAndReturn);
                //db.SaveChanges();
                unitOfWork.BorrowAndReturnRepository.Insert(borrowAndReturn);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(borrowAndReturn);
        }

        // GET: BorrowAndReturns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //BorrowAndReturn borrowAndReturn = db.BorrowAndReturns.Find(id);
            BorrowAndReturn borrowAndReturn = unitOfWork.BorrowAndReturnRepository.GetByID(id);
            if (borrowAndReturn == null)
            {
                return HttpNotFound();
            }
            return View(borrowAndReturn);
        }

        // POST: BorrowAndReturns/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserID,BookID,BorrowTime,ReturnTime,ExpiredDays,IsReturn")] BorrowAndReturn borrowAndReturn)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(borrowAndReturn).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(borrowAndReturn);
        }

        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
