using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Article
    {
        public int ID { get; set; }

        [Display(Name ="文章编号")]
        public int ArticleID { get; set; }

        [Display(Name ="图书栏目")]
        public int BookTypeID { get; set; }

        [Display(Name ="文章")]
        public string Title { get; set; }

        [Display(Name ="正文")]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Display(Name ="作者")]
        public string AuthorName { get; set; }

        [Display(Name ="发表时间")]
        public DateTime CreateTime { get; set; }
    }
}