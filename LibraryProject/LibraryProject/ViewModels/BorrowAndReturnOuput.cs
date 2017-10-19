using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryProject.ViewModels
{
    public class BorrowAndReturnOuput
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string BookName { get; set; }
        public int BarCode { get; set; }
        public DateTime BorrowTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public int ExpiredDays { get; set; }
        public bool IsReturn { get; set; }
    }
}