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


        // GET: PersonInfo
        public ActionResult PersonInfo()
        {
            var user = CheckLogin.Instance.GetUser();
            var userInfo = unitOfWork.UserInfoRepository.Get()
                .Where(ctx => ctx.UserID == user.ID)
                .ToList();
            var userObj = new UserInfo();
            if(userInfo.Count() == 0)
            {
                userObj = new UserInfo
                {
                    UserName = user.UserName,
                    StudentID = "",
                    Email = user.Email,
                    Phone = user.PhoneNum,
                    DepartmentName = "",
                    Name = ""
                };
            }
            else
            {
                userObj = new UserInfo
                {
                    UserName=user.UserName,
                    StudentID=userInfo[0].StudentID,
                    Email=user.Email,
                    Phone=user.PhoneNum,
                    DepartmentName=userInfo[0].DepartmentName,
                    Name=userInfo[0].Name
                };
            }
            return View(userObj);
        }

        [HttpPost]
        public ActionResult PersonInfo([Bind(Include ="UserName,StudentID,Name,Email,Phone,DepartmentName")] UserInfo userInfo)
        {
            var user = CheckLogin.Instance.GetUser();
            var userDatabase = unitOfWork.UserInfoRepository.Get()
                .Where(ctx => ctx.UserName.Equals(userInfo.UserName) == true)
                .ToList();
            var userInfoInput = new UserInfo();
            if(userDatabase.Count() == 0)
            {
                userInfoInput = new UserInfo {
                    UserID=user.ID,
                    UserName = userInfo.UserName,
                    StudentID = userInfo.StudentID,
                    Name = userInfo.Name,
                    Email = userInfo.Email,
                    Phone = userInfo.Phone,
                    DepartmentName = userInfo.DepartmentName
                };
                unitOfWork.UserInfoRepository.Insert(userInfoInput);
                unitOfWork.Save();
            }
            else
            {
                userDatabase[0].UserID = user.ID;
                userDatabase[0].UserName = userInfo.UserName;
                userDatabase[0].StudentID = userInfo.StudentID;
                userDatabase[0].Name = userInfo.Name;
                userDatabase[0].Email = userInfo.Email;
                userDatabase[0].Phone = userInfo.Phone;
                userDatabase[0].DepartmentName = userInfo.DepartmentName;
                userInfoInput = userDatabase[0];
                unitOfWork.Save();
            }
            return View(userInfoInput);
        }
    }
}
