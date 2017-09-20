using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryProject.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Remote("CheckUserName", "Users")]
        public string UserName { get; set; }

        [Required]
        [StringLength(16,MinimumLength = 8,ErrorMessage ="密码为8位-16位")]
        [RegularExpression(@"[A-Za-z0-9]+",ErrorMessage ="密码为数字和字母的组合")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage ="两次输入的密码不同！")]
        [DataType(DataType.Password)]
        public string PasswordComfirm { get; set; }

        [Required]
        [RegularExpression(@"[0-9]+",ErrorMessage ="手机号码格式错误")]
        public string PhoneNum { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9.%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage ="邮箱格式错误")]
        public string Email { get; set; }

        public string Role { get; set; }
    }

}