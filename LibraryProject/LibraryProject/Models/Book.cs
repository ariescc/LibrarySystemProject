using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Book
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsAvailable { get; set; }
        [Display(Name = "Bar Code")]
        public string Isbn { get; set; }
        public string Summary { get; set; }
        public string Author { get; set; }
        public string Type { get; set; }
        public decimal Price { get; set; }
        public DateTime PublishTime { get; set; }
        public string Location { get; set; }
    }
}