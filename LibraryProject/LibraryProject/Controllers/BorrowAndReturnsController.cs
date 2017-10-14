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
using LibraryProject.ViewModels;

namespace LibraryProject.Controllers
{
    public class BorrowAndReturnsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: BorrowAndReturns
        [Auth(Code ="libraryadmin")]
        public ActionResult Index()
        {
            IEnumerable<BorrowAndReturn> objList = unitOfWork.BorrowAndReturnRepository.Get();
            List<BorrowAndReturnOuput> BorrowAndReturnOutputList = new List<BorrowAndReturnOuput>();
            foreach(var item in objList)
            {
                var input = new BorrowAndReturnOuput
                {
                    ID=item.ID,
                    UserName = unitOfWork.UserRepository.GetByID(item.UserID).UserName,
                    BookName = unitOfWork.BookRepository.GetByID(item.BookID).Name,
                    IsReturn= item.IsReturn,
                    BorrowTime=item.BorrowTime,
                    ReturnTime=item.ReturnTime,
                    ExpiredDays=item.ExpiredDays
                };
                BorrowAndReturnOutputList.Add(input);
            }
            return View(BorrowAndReturnOutputList);
        }

        // GET: BorrowAndReturns/Details/5
        [Auth(Code ="libraryadmin")]
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
        [Auth(Code ="libraryadmin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: BorrowAndReturns/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Code ="libraryadmin")]
        public ActionResult Create([Bind(Include = "ID,StudentID,BookID,BorrowTime,ReturnTime,ExpiredDays,IsReturn")] BorrowAndReturnInput borrowAndReturnInput)
        {
            var borrowAndReturn = new BorrowAndReturn();
            if (ModelState.IsValid)
            {
                //db.BorrowAndReturns.Add(borrowAndReturn);
                //db.SaveChanges();
                var userInfoObj = unitOfWork.UserInfoRepository.Get()
                    .Where(ctx => ctx.StudentID.Equals(borrowAndReturnInput.StudentID) == true)
                    .ToList();
                borrowAndReturn = new BorrowAndReturn
                {
                    ID=borrowAndReturnInput.ID,
                    BookID=borrowAndReturnInput.BookID,
                    UserID=userInfoObj[0].UserID,
                    BorrowTime=borrowAndReturnInput.BorrowTime,
                    ReturnTime=borrowAndReturnInput.ReturnTime,
                    ExpiredDays=borrowAndReturnInput.ExpiredDays,
                    IsReturn=borrowAndReturnInput.IsReturn
                };
                unitOfWork.BorrowAndReturnRepository.Insert(borrowAndReturn);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(borrowAndReturn);
        }

        // GET: BorrowAndReturns/Edit/5
        [Auth(Code ="libraryadmin")]
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
        [Auth(Code ="libraryadmin")]
        public ActionResult Edit([Bind(Include = "ID,UserID,BookID,BorrowTime,ReturnTime,ExpiredDays,IsReturn")] BorrowAndReturn borrowAndReturn)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.context.Entry(borrowAndReturn).State = EntityState.Modified;
                unitOfWork.Save();
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
