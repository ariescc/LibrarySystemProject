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
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace LibraryProject.Controllers
{
    public class UploadImagesController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        // GET: UploadImages
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult uploadHead(HttpPostedFileBase head)//命名和上传控件name 一样
        {
            try
            {
                if ((head == null))
                {
                    return Json(new { msg = 0 });
                }
                else
                {
                    var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "bmp" };
                    var fileExt = System.IO.Path.GetExtension(head.FileName).Substring(1);
                    if (!supportedTypes.Contains(fileExt))
                    {
                        return Json(new { msg = -1 });
                    }

                    if (head.ContentLength > 1024 * 1000 * 10)
                    {
                        return Json(new { msg = -2 });
                    }

                    Random r = new Random();
                    var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(10000) + "." + fileExt;
                    var filepath = Path.Combine(Server.MapPath("~/UploadFiles/temp"), filename);
                    head.SaveAs(filepath);
                    return Json(new { msg = filename });
                }
            }
            catch (Exception)
            {
                return Json(new { msg = -3 });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult saveHead()
        {
            var user = CheckLogin.Instance.GetUser();
            UploadImage model = new UploadImage();
            model.HeadFileName = Request.Form["HeadFileName"].ToString();
            model.X = Convert.ToInt32(Request.Form["X"]);
            model.X = Convert.ToInt32(Request.Form["Y"]);
            model.Width = Convert.ToInt32(Request.Form["Width"]);
            model.Height = Convert.ToInt32(Request.Form["Height"]);
            model.UserID = user.ID;
            if ((model == null))
            {
                return Json(new { msg = 0 });
            }
            else
            {
                var filepath = Path.Combine(Server.MapPath("~/UploadFiles/temp"), model.HeadFileName);
                string fileExt = Path.GetExtension(filepath);
                Random r = new Random();
                var filename = DateTime.Now.ToString("yyyyMMddHHmmss") + r.Next(10000) + fileExt;
                var path180 = Path.Combine(Server.MapPath("~/UploadFiles/180"), filename);
                var path75 = Path.Combine(Server.MapPath("~/UploadFiles/75"), filename);
                var path50 = Path.Combine(Server.MapPath("~/UploadFiles/50"), filename);
                var path25 = Path.Combine(Server.MapPath("~/UploadFiles/25"), filename);
                cutAvatar(filepath, model.X, model.Y, model.Width, model.Height, 75L, path180, 180);
                cutAvatar(filepath, model.X, model.Y, model.Width, model.Height, 75L, path75, 75);
                cutAvatar(filepath, model.X, model.Y, model.Width, model.Height, 75L, path50, 50);
                cutAvatar(filepath, model.X, model.Y, model.Width, model.Height, 75L, path25, 25);
                unitOfWork.UploadImageRepository.Insert(model);
                unitOfWork.Save();
                return Json(new { msg = 1 });
            }
        }

        /// <summary>
        /// 创建缩略图
        /// </summary>
        public void cutAvatar(string imgSrc, int x, int y, int width, int height, long Quality, string SavePath, int t)
        {


            Image original = Image.FromFile(imgSrc);

            Bitmap img = new Bitmap(t, t, PixelFormat.Format24bppRgb);

            img.MakeTransparent(img.GetPixel(0, 0));
            img.SetResolution(72, 72);
            using (Graphics gr = Graphics.FromImage(img))
            {
                if (original.RawFormat.Equals(ImageFormat.Jpeg) || original.RawFormat.Equals(ImageFormat.Png) || original.RawFormat.Equals(ImageFormat.Bmp))
                {
                    gr.Clear(Color.Transparent);
                }
                if (original.RawFormat.Equals(ImageFormat.Gif))
                {
                    gr.Clear(Color.White);
                }


                gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gr.SmoothingMode = SmoothingMode.AntiAlias;
                gr.CompositingQuality = CompositingQuality.HighQuality;
                gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                gr.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                using (var attribute = new System.Drawing.Imaging.ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    gr.DrawImage(original, new Rectangle(0, 0, t, t), x, y, width, height, GraphicsUnit.Pixel, attribute);
                }
            }
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
            if (original.RawFormat.Equals(ImageFormat.Jpeg))
            {
                myImageCodecInfo = GetEncoderInfo("image/jpeg");
            }
            else
                if (original.RawFormat.Equals(ImageFormat.Png))
            {
                myImageCodecInfo = GetEncoderInfo("image/png");
            }
            else
                    if (original.RawFormat.Equals(ImageFormat.Gif))
            {
                myImageCodecInfo = GetEncoderInfo("image/gif");
            }
            else
            if (original.RawFormat.Equals(ImageFormat.Bmp))
            {
                myImageCodecInfo = GetEncoderInfo("image/bmp");
            }

            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, Quality);
            myEncoderParameters.Param[0] = myEncoderParameter;
            img.Save(SavePath, myImageCodecInfo, myEncoderParameters);
        }

        //根据长宽自适应 按原图比例缩放 
        private static Size GetThumbnailSize(System.Drawing.Image original, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double)desiredWidth / original.Width;
            var heightScale = (double)desiredHeight / original.Height;
            var scale = widthScale < heightScale ? widthScale : heightScale;
            return new Size
            {
                Width = (int)(scale * original.Width),
                Height = (int)(scale * original.Height)
            };
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        [HttpPost]
        public ActionResult PreviewImage()
        {
            var bytes = new byte[0];
            ViewBag.Mime = "image/png";

            if (Request.Files.Count == 1)
            {
                bytes = new byte[Request.Files[0].ContentLength];
                Request.Files[0].InputStream.Read(bytes, 0, bytes.Length);
                ViewBag.Mime = Request.Files[0].ContentType;
            }

            ViewBag.Message = Convert.ToBase64String(bytes, Base64FormattingOptions.InsertLineBreaks);
            return PartialView();
        }
    }
}
