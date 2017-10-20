using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class UploadImage
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public string HeadFileName { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}