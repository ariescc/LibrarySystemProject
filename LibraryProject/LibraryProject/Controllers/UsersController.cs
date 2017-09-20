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
using LibraryProject.Repositories;
using System.Web.UI;
using System.Web.Security;
using Newtonsoft.Json;

namespace LibraryProject.Controllers
{
    public class UsersController : Controller
    {
        // private UserDbContext db = new UserDbContext();

        public readonly static UsersController usersController = new UsersController();

        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login([Bind(Include ="UserName,Password")] User user)
        {
            var userObj = unitOfWork.UserRepository.Get()
                .Where(item => item.UserName.Equals(user.UserName) == true)
                .ToList();
            if(userObj[0] != null)
            {
                if (userObj[0].Password.Equals(user.Password) == true)
                {
                    CheckLogin.Instance.IsLogin = true;
                    // Session 实现

                    // 序列化用户实体
                    string UserData = JsonConvert.SerializeObject(userObj[0]);

                    // 保存身份信息
                    FormsAuthenticationTicket Ticket = new FormsAuthenticationTicket(1, user.UserName,
                        DateTime.Now, DateTime.Now.AddHours(12), false, UserData);

                    HttpCookie Cookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                        FormsAuthentication.Encrypt(Ticket));

                    Response.Cookies.Add(Cookie);

                    //return RedirectToAction("Index","Books","Index");
                    return Redirect("/Home/Index/");
                }
            }
            else
            {
                ModelState.AddModelError("UserName", "No User");
            }
            return View(user);
        }

        // 注销登录
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Home/Index/");
        }

        // GET: Users
        public ActionResult Index()
        {
            //return View(db.Users.ToList());
            return View(unitOfWork.UserRepository.Get());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //User user = db.Users.Find(id);
            User user = unitOfWork.UserRepository.GetByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,UserName,Password,PasswordComfirm,PhoneNum,Email")] User user)
        {
            if(IsDistinctUserName(user.UserName))
            {
                ModelState.AddModelError("UserName", "用户名称不唯一");
            }
            if (ModelState.IsValid)
            {
                //db.Users.Add(user);
                //db.SaveChanges();
                unitOfWork.UserRepository.Insert(user);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //User user = db.Users.Find(id);
            User user = unitOfWork.UserRepository.GetByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,UserName,Password,PhoneNum,Email")] User user)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(user).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //User user = db.Users.Find(id);
            User user = unitOfWork.UserRepository.GetByID(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //User user = db.Users.Find(id);
            //db.Users.Remove(user);
            //db.SaveChanges();
            unitOfWork.UserRepository.Delete(id);
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
        private bool IsDistinctUserName(string username)
        {
            if (unitOfWork.UserRepository.Get().Where(r => r.UserName == username).ToList().Count > 0)
                return true;
            else
                return false;
        }

        [OutputCache(Location = OutputCacheLocation.None,NoStore = true)]//清楚缓存
        public JsonResult CheckUserName(string username)
        {
            if (IsDistinctUserName(username))
            {
                return Json("用户名不唯一", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

    }
}
