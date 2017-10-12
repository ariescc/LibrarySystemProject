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
    public class ArticlesController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Articles
        public ActionResult Index()
        {
            return View(unitOfWork.ArticleRepository.Get());
        }

        // GET: LatestNews
        public ActionResult LatestNews()
        {
            var articles = unitOfWork.ArticleRepository.Get();
            return View(articles);
        }

        // GET: Articles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Article article = unitOfWork.ArticleRepository.GetByID(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // GET: Articles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Articles/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AuthorName,Title,Content")] Article article)
        {
            if (ModelState.IsValid)
            {
                article.CreateTime = DateTime.Now;
                unitOfWork.ArticleRepository.Insert(article);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(article);
        }

        // GET: Articles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Article article = db.Articles.Find(id);
            Article article = unitOfWork.ArticleRepository.GetByID(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ArticleID,BookTypeID,Title,AuthorName,CreateTime")] Article article)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.context.Entry(article).State = EntityState.Modified;
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(article);
        }

        // GET: Articles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Article article = db.Articles.Find(id);
            Article article = unitOfWork.ArticleRepository.GetByID(id);
            if (article == null)
            {
                return HttpNotFound();
            }
            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //Article article = db.Articles.Find(id);
            Article article = unitOfWork.ArticleRepository.GetByID(id);
            unitOfWork.ArticleRepository.Delete(id);
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
