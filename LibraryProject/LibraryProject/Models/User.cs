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
        [StringLength(16,MinimumLength = 8,ErrorMessage ="length 8 ~16")]
        [RegularExpression(@"[A-Za-z0-9]+",ErrorMessage ="Number or word needed!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage ="type twice not same")]
        [DataType(DataType.Password)]
        public string PasswordComfirm { get; set; }

        [Required]
        [RegularExpression(@"[0-9]+",ErrorMessage ="Phone number format is wrong")]
        public string PhoneNum { get; set; }

        [Required]
        [RegularExpression(@"[A-Za-z0-9.%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage ="Email format is wrong")]
        public string Email { get; set; }

        public string Role { get; set; }
    }

}