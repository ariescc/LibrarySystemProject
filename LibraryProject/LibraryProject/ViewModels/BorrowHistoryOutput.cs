using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.ViewModels
{
    public class BorrowHistoryOutput
    {
        public int ID { get; set; }

        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Display(Name = "Borrow Time")]
        public DateTime BorrowTime { get; set; }

        [Display(Name = "Return Time")]
        public DateTime ReturnTime { get; set; }

        [Display(Name = "Is Return ?")]
        public bool IsReturn { get; set; }

        [Display(Name = "Expired Days")]
        public int ExpiredDays { get; set; }
    }
}