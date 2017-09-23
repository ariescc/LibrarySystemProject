using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryProject.ViewModels
{
    public class BorrowAndReturnInput
    {
        public int ID { get; set; }
        public string StudentID { get; set; }
        public int BookID { get; set; }
        public DateTime BorrowTime { get; set; }
        public DateTime ReturnTime { get; set; }
        public int ExpiredDays { get; set; }
        public bool IsReturn { get; set; }
    }
}