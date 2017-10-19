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
using System.IO;
using System.Drawing;
using System.Web.Script.Serialization;
using LibraryProject.ViewModels;

namespace LibraryProject.Controllers
{
    public class BooksController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        // GET: BooksManage
        [Auth(Code = "libraryadmin")]
        public ActionResult BooksManage()
        {
            var books = unitOfWork.BookRepository.Get()
                .GroupBy(item => item.Isbn13)
                .Select(item => item.First());
            return View(books);
        }

        // GET: Books
        public ActionResult Index()
        {
            var books = unitOfWork.BookRepository.Get()
                .GroupBy(item => item.Isbn13)
                .Select(item => item.First());
            return View(books);
        }

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Book book = db.Books.Find(id);
            var book = unitOfWork.BookRepository.GetByID(id);
            book.BarCode = book.ID.ToString();
            unitOfWork.Save();
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/SearchResult/:isbn
        public ActionResult SearchResult(string isbn)
        {
            var resList = unitOfWork.BookRepository.Get()
                .Where(item => item.Isbn13.Equals(isbn) == true)
                .ToList();

            foreach(var item in resList)
            {
                item.BarCode = item.ID.ToString();
            }

            return View(resList);
        }

        // GET: Books/Create
        [Auth(Code = "libraryadmin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Books/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Code = "libraryadmin")]
        public ActionResult Create([Bind(Include = "ID,Isbn,Title,Publisher,Image,Isbn10,Isbn13,Summary,Pages,Price,msg,code,Location,IsAvaliable")] Book book)
        {
            if (ModelState.IsValid)
            {
                string isbn = book.Isbn;
                Book bookInfo;
                string json;

                // 获取豆瓣api信息
                GetInfo(isbn, out bookInfo, out json);

                if (bookInfo != null)
                {
                    if (bookInfo.msg != null)
                    {
                        //throw new Exception("获取失败" + bookInfo.msg);
                        ModelState.AddModelError("Isbn", "No data are gotten from Douban!");
                    }
                    if (bookInfo.Title != null)
                    {
                        book.Title = bookInfo.Title;
                    }
                    if (bookInfo.Author != null)
                    {
                        book.Author = bookInfo.Author;
                    }
                    if (bookInfo.Publisher != null)
                    {
                        book.Publisher = bookInfo.Publisher;
                    }
                    if (bookInfo.Isbn10!=null){
                        book.Isbn10 = bookInfo.Isbn10;
                    }
                    if (bookInfo.Image != null)
                    {
                        book.Image = bookInfo.Image;
                    }
                    if (bookInfo.Isbn13 != null)
                    {
                        book.Isbn13 = bookInfo.Isbn13;
                    }
                    if (bookInfo.Summary != null)
                    {
                        book.Summary = bookInfo.Summary;
                    }
                    if (bookInfo.Pages != null)
                    {
                        book.Pages = bookInfo.Pages;
                    }
                    if (bookInfo.Price != null)
                    {
                        book.Price = bookInfo.Price;
                    }
                    string jsonOutput = json;
                    unitOfWork.BookRepository.Insert(book);
                    unitOfWork.Save();
                    return RedirectToAction("BooksManage");
                }
                else
                {
                    //throw new Exception("获取失败");
                    ModelState.AddModelError("Isbn", "No data can be gotten, please Manual input！");
                }
                
            }

            return View(book);
        }

        // GET: Books/Edit/5
        [Auth(Code = "libraryadmin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Book book = db.Books.Find(id);
            var book = unitOfWork.BookRepository.GetByID(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Auth(Code = "libraryadmin")]
        public ActionResult Edit([Bind(Include = "ID,Title,Publisher,Image,Isbn10,Isbn13,Summary,Pages,Price,msg,code,Location,IsAvaliable")] Book book)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.context.Entry(book).State = EntityState.Modified;
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        // GET: Books/Delete/5
        [Auth(Code = "libraryadmin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Book book = db.Books.Find(id);
            var book = unitOfWork.BookRepository.GetByID(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Auth(Code = "libraryadmin")]
        public ActionResult DeleteConfirmed(int id)
        {
            //Book book = db.Books.Find(id);
            //db.Books.Remove(book);
            //db.SaveChanges();
            unitOfWork.BookRepository.Delete(id);
            unitOfWork.Save();
            return RedirectToAction("BooksManage");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
               unitOfWork.context.Dispose();
            }
            base.Dispose(disposing);
        }

        // 调用 豆瓣图书 api 
        // 根据 ISBN 从豆瓣api获取图书的详细信息
        //根据ISBN码从豆瓣API获取书籍详细信息
        public static bool GetInfo(string isbn, out Book bookInfo, out string json)
        {
            try
            {
                //豆瓣API
                string uri = "https://api.douban.com/v2/book/isbn/" + isbn;
                //获取书籍详细信息，Json格式
                json = DoGet(uri, "utf-8");
                //将获取到的Json格式的文件转换为定义的类
                bookInfo = ToMap(json);
                return true;
            }
            catch
            {
                //信息获取失败
                bookInfo = null;
                json = "";
                return false;
            }
        }

        // Json 实例化为 book 实体
        //Json解析
        private static Book ToMap(string jsonString)
        {
            //实例化JavaScriptSerializer类的新实例
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Book>(jsonString);
            }
            catch
            {
                return null;
            }
        }

        //HTTP的GET请求，获取书籍详细信息
        private static string DoGet(string url, string charset)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            if (response != null)
            {
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, System.Text.Encoding.GetEncoding(charset));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            throw new Exception();
        }

        // Http 获取图书封面
        //HTTP获取图片，获取书籍封面
        public static Image DoGetImage(string url)
        {
            WebRequest request = WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            if (response != null)
            {
                Stream stream = response.GetResponseStream();
                return Image.FromStream(stream);
            }
            return DrawCover();
        }

        // 绘制封面
        private static Bitmap DrawCover()
        {
            Bitmap image = new Bitmap(102, 145);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.Silver);
            StringFormat format = new StringFormat { Alignment = StringAlignment.Center };
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            //封面显示内容：暂无封面
            g.DrawString("暂无封面", new Font("黑体", 12f, FontStyle.Regular), Brushes.Black, 51, 50, format);
            return image;
        }
    }
}
