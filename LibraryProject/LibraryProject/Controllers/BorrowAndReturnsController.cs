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
                    BarCode = item.BookID,
                    UserName = unitOfWork.UserRepository.GetByID(item.UserID).UserName,
                    BookName = unitOfWork.BookRepository.GetByID(item.BookID).Title,
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
        public ActionResult Create([Bind(Include = "ID,StudentID,BookID,IsReturn")] BorrowAndReturnInput borrowAndReturnInput)
        {
            var borrowAndReturn = new BorrowAndReturn();
            if (ModelState.IsValid)
            {
                //db.BorrowAndReturns.Add(borrowAndReturn);
                //db.SaveChanges();
                var userInfoObj = unitOfWork.UserInfoRepository.Get()
                    .Where(ctx => ctx.StudentID.Equals(borrowAndReturnInput.StudentID) == true)
                    .ToList();

                if(userInfoObj.Count() == 0)
                {
                    ModelState.AddModelError("StudentID", "No student with this ID");
                }

                var book = unitOfWork.BookRepository.Get()
                    .Where(item =>item.ID == borrowAndReturnInput.BookID)
                    .ToList();

                if(book.Count() == 0)
                {
                    ModelState.AddModelError("BookID", "No book with this ID");
                }
                else
                {
                    book[0].IsAvaliable = true;

                    borrowAndReturn = new BorrowAndReturn
                    {
                        BookID = borrowAndReturnInput.BookID,
                        UserID = userInfoObj[0].UserID,
                        BorrowTime = DateTime.Now,
                        IsReturn = false
                    };
                    unitOfWork.BorrowAndReturnRepository.Insert(borrowAndReturn);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            return View(borrowAndReturnInput);
        }

        // GET: BorrowAndReturns/Return
        [Auth(Code = "libraryadmin")]
        public ActionResult Return()
        {
            return View();
        }

        // POST: BorrowAndReturns/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Code = "libraryadmin")]
        public ActionResult Return([Bind(Include = "BookID")]BorrowAndReturnInput borrowAndReturnInput)
        {
            var bookObj = unitOfWork.BookRepository.Get()
                .Where(item => item.ID == borrowAndReturnInput.BookID)
                .ToList();

            if(bookObj.Count() == 0)
            {
                throw new Exception("请核查图书库内是否有该本图书");
            }

            var borrow = unitOfWork.BorrowAndReturnRepository.Get()
                .Where(item=>item.BookID == bookObj[0].ID)
                .ToList();

            if(borrow == null)
            {
                throw new Exception("请核查该本图书是否已经借阅");
            }

            var tmp = DateTime.Now - borrow[0].BorrowTime;
            var days = tmp.Days;

            /*var borrowAndReturn = new BorrowAndReturnInput()
            {
                BookID = borrow[0].BookID,
                StudentID = borrowAndReturnInput.StudentID,
                ReturnTime = DateTime.Now,
                IsReturn = true,
                ExpiredDays = days
            };*/

            borrow.LastOrDefault().IsReturn = true;
            borrow.LastOrDefault().ReturnTime = DateTime.Now;
            borrow.LastOrDefault().ExpiredDays = days;

            var book = unitOfWork.BookRepository.GetByID(borrowAndReturnInput.BookID);
            book.IsAvaliable = false;

            unitOfWork.Save();

            return RedirectToAction("Index");
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

        // Delete
        [Auth(Code = "libraryadmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            unitOfWork.BorrowAndReturnRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("Index");
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
