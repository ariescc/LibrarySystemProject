using LibraryProject.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryProject.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        [AllowAnonymous]
        public ActionResult Index()
        {
            var articleList = unitOfWork.ArticleRepository.Get();

            return View(articleList);
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}