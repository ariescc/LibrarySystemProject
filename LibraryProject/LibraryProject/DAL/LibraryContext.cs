using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace LibraryProject.DAL
{
    public class LibraryContext:DbContext
    {
        public LibraryContext()
            : base("LibraryContext")
        { }

        // Add Tables
        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UserInfoes { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}