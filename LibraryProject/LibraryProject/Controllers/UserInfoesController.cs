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
            var borrowAmount = unitOfWork.BorrowAndReturnRepository.Get()
                .Where(ctx => ctx.UserID == user.ID)
                .ToList()
                .Count();
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
                    Name = "",
                    BorrowAmount = borrowAmount,
                    CurrentBorrowAmount = 0
                };
            }
            else
            {
                userObj = new UserInfo
                {
                    UserName = user.UserName,
                    StudentID = userInfo[0].StudentID,
                    Email = user.Email,
                    Phone = user.PhoneNum,
                    DepartmentName = userInfo[0].DepartmentName,
                    Name = userInfo[0].Name,
                    BorrowAmount = borrowAmount,
                    CurrentBorrowAmount = 0
                };
            }
            return View(userObj);
        }

        [HttpPost]
        public ActionResult PersonInfo([Bind(Include ="UserName,StudentID,Name,Email,Phone,DepartmentName")] UserInfo userInfo, HttpPostedFileBase image)
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

                if(image != null)
                {
                    userInfo.ImageType = image.ContentType;
                    // 新建一个长度等于图片大小的二进制地址
                    userInfo.ImageData = new byte[image.ContentLength];
                    // 将image读取到ImageData中
                    image.InputStream.Read(userInfo.ImageData, 0, image.ContentLength);
                }

                TempData["message"] = string.Format("{0} has been saved", userInfo.Name);

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

                if (image != null)
                {
                    userInfo.ImageType = image.ContentType;
                    // 新建一个长度等于图片大小的二进制地址
                    userInfo.ImageData = new byte[image.ContentLength];
                    // 将image读取到ImageData中
                    image.InputStream.Read(userInfo.ImageData, 0, image.ContentLength);
                }

                TempData["message"] = string.Format("{0} has been saved", userInfo.Name);
                unitOfWork.Save();

                
            }
            return RedirectToAction("Index", "Home");
        }

        // 通过ID， 将二进制文件转换为图片
        public FileContentResult GetImage(int userId)
        {
            var userinfo = unitOfWork.UserInfoRepository.Get()
                .Where(item => item.UserID == userId)
                .ToList();

            if (userinfo[0] != null)
            {
                return File(userinfo[0].ImageData, userinfo[0].ImageType);
            }
            else
            {
                return null;
            }
        }


        // GET: /UserInfoes/BorrowHistory
        public ActionResult BorrowHistory()
        {
            var user = CheckLogin.Instance.GetUser();
            var borrowBooks = unitOfWork.BorrowAndReturnRepository.Get()
                .Where(item => item.UserID == user.ID)
                .ToList();
            var outputList = new List<BorrowHistoryOutput>();
            foreach(var item in borrowBooks)
            {
                var book = unitOfWork.BookRepository.GetByID(item.BookID);
                var cur = new BorrowHistoryOutput
                {
                    //BookName = book.Name,
                    BorrowTime = item.BorrowTime,
                    ReturnTime = item.ReturnTime,
                    IsReturn = item.IsReturn
                };
                outputList.Add(cur);
            }
            return View(outputList);
        }
    }
}
