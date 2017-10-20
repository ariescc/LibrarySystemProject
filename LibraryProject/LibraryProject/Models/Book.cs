using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Book
    {
        public int ID { get; set; }
        // 输入的 ISBN
        public string Isbn { get; set; }
        // 标题
        public string Title { get; set; }
        // 作者
        public string[] Author { get; set; }
        // 出版社
        public string Publisher { get; set; }
        // 图书封面
        public string Image { get; set; }
        //ISBN10
        public string Isbn10 { get; set; }
        // ISBN13
        public string Isbn13 { get; set; }
        // Bar code
        public string BarCode { get; set; }
        // 概述
        public string Summary { get; set; }
        // 页数
        public string Pages { get; set; }
        // 价格
        public string Price { get; set; }
        // 获取失败的返回信息
        public string msg { get; set; }
        public string code { get; set; }
        // 图书位置
        public string Location { get; set; }
        // 图书状态
        [Display(Name = "Avaliable?")]
        public bool IsAvaliable { get; set; }

        public bool IsDeleted { get; set; }
    }
}