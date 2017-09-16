using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibraryProject.Models
{
    public class Department
    {
        public int ID { get; set; }
        [Required]
        public string DepartmentName { get; set; }
    }
}