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

            var ImageList = unitOfWork.UploadImageRepository.Get()
                .Where(item => item.UserID == user.ID)
                .ToList();

            string ImageInput;
            if(ImageList.Count() == 0)
            {
                ImageInput = null;
            }
            else
            {
                ImageInput = "UploadFiles/temp/" + ImageList.LastOrDefault().HeadFileName;
            }

            var borrowAmount = unitOfWork.BorrowAndReturnRepository.Get()
                .Where(ctx => ctx.UserID == user.ID && ctx.IsReturn == true)
                .ToList()
                .Count();

            var currentAmount = unitOfWork.BorrowAndReturnRepository.Get()
                .Where(ctx => ctx.UserID == user.ID && ctx.IsReturn == false)
                .ToList()
                .Count();
            var userObj = new UserInfo();
            if(userInfo.Count() == 0)
            {
                userObj = new UserInfo
                {
                    UserName = user.UserName,
                    StudentID = "",
                    Email = "",
                    Phone = "",
                    DepartmentName = "",
                    Name = "",
                    BorrowAmount = borrowAmount,
                    CurrentBorrowAmount = currentAmount,
                    Image = ImageInput
                };
            }
            else
            {
                userObj = new UserInfo
                {
                    UserName = user.UserName,
                    StudentID = userInfo[0].StudentID,
                    Email = userInfo[0].Email,
                    Phone = userInfo[0].Phone,
                    DepartmentName = userInfo[0].DepartmentName,
                    Name = userInfo[0].Name,
                    BorrowAmount = borrowAmount,
                    CurrentBorrowAmount = currentAmount,
                    Image = ImageInput
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
            var imageAddress = unitOfWork.UploadImageRepository.Get()
                .Where(item => item.UserID == user.ID)
                .ToList();

            string ImageInput;
            if(imageAddress.Count() == 0)
            {
                ImageInput = null;
            }
            else
            {
                ImageInput = "~/UploadFiles/temp/" + imageAddress.LastOrDefault().HeadFileName;
            }

            if(userDatabase.Count() == 0)
            {
                userInfoInput = new UserInfo {
                    UserID = user.ID,
                    UserName = userInfo.UserName,
                    StudentID = userInfo.StudentID,
                    Name = userInfo.Name,
                    Email = userInfo.Email,
                    Phone = userInfo.Phone,
                    DepartmentName = userInfo.DepartmentName,
                    Image = ImageInput
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
                userDatabase[0].Image = ImageInput;
                userInfoInput = userDatabase[0];

                unitOfWork.Save();

                
            }
            return RedirectToAction("Index", "Home");
        }

        // GET: /UserInfoes/BorrowHistory
        public ActionResult BorrowHistory()
        {
            var user = CheckLogin.Instance.GetUser();
            var borrowBooks = unitOfWork.BorrowAndReturnRepository.Get()
                .Where(item => item.UserID == user.ID && item.IsReturn == true)
                .ToList();
            var outputList = new List<BorrowHistoryOutput>();
            foreach(var item in borrowBooks)
            {
                var book = unitOfWork.BookRepository.GetByID(item.BookID);
                var cur = new BorrowHistoryOutput
                {
                    BookName = book.Title,
                    BorrowTime = item.BorrowTime,
                    ReturnTime = item.ReturnTime,
                    IsReturn = item.IsReturn
                };
                outputList.Add(cur);
            }
            return View(outputList);
        }

        // GET: /UserInfoes/CurrentBorrow
        public ActionResult CurrentBorrow()
        {
            var user = CheckLogin.Instance.GetUser();
            var borrowBooks = unitOfWork.BorrowAndReturnRepository.Get()
                .Where(item => item.UserID == user.ID && item.IsReturn == false)
                .ToList();

            var outputList = new List<BorrowHistoryOutput>();
            foreach(var item in borrowBooks)
            {
                var book = unitOfWork.BookRepository.GetByID(item.BookID);
                var tmp = DateTime.Now - item.BorrowTime;
                var days = tmp.Days;
                if (days <= 30)
                {
                    days = 0;
                }
                var cur = new BorrowHistoryOutput()
                {
                    BookName = book.Title,
                    BorrowTime = item.BorrowTime,
                    ReturnTime = item.ReturnTime,
                    IsReturn = item.IsReturn,
                    ExpiredDays = days
                };
                outputList.Add(cur);
            }
            return View(outputList);
        }
    }
}
