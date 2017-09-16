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
    public class UserInfoesController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: UserInfoes
        public ActionResult Index()
        {
            return View();
        }

        // POST: UserInfoes
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ID,UserID,UserName,StudentID,Name,Email" +
            "Phone,DepartmentName")] UserInfo userInfo)
        {
            var name = User.Identity.Name;
            var user = unitOfWork.UserRepository.Get()
                .Where(item => item.UserName.Equals(name) == true)
                .ToList();
            //var user = unitOfWork.UserRepository.GetByID(Convert.ToInt32(User.Identity.Name));
            var userInfoObj = unitOfWork.UserInfoRepository.Get()
                .Where(item => item.UserID == user[0].ID)
                .ToList();
            if(userInfoObj == null)
            {
                var userInfoInput = new UserInfo
                {
                    UserID = user[0].ID,
                    UserName = user[0].UserName,
                    StudentID = userInfo.StudentID,
                    Name = userInfo.Name,
                    Email = userInfo.Email,
                    Phone = userInfo.Phone,
                    DepartmentName = userInfo.DepartmentName
                };
                unitOfWork.UserInfoRepository.Insert(userInfoInput);
                unitOfWork.Save();
                return View(userInfoInput);
            }
            else
            {
                userInfoObj[0].UserName = user[0].UserName;
                userInfoObj[0].StudentID = userInfo.StudentID;
                userInfoObj[0].Name = userInfo.Name;
                userInfoObj[0].Email = userInfo.Email;
                userInfoObj[0].Phone = userInfo.Phone;
                userInfoObj[0].DepartmentName = userInfo.DepartmentName;
                unitOfWork.Save();
                return View(userInfoObj[0]);
            }
            
        }

    }
}
