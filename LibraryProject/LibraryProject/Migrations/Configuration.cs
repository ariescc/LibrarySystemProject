namespace LibraryProject.Migrations
{
    using LibraryProject.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LibraryProject.DAL.LibraryContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(LibraryProject.DAL.LibraryContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            context.Users.AddOrUpdate(p => p.UserName,
                new User {ID=1, UserName="libraryadmin",Password="11111111",PasswordComfirm="11111111",
                    Email="11@qq.com",PhoneNum="18829589353",Role="libraryadmin"},
                new User
                {
                    ID=2,
                    UserName = "student",
                    Password = "22222222",
                    PasswordComfirm = "22222222",
                    Email = "11@qq.com",
                    PhoneNum = "18829589353",
                    Role = "user"
                }
                );

            context.UserInfoes.AddOrUpdate(p => p.UserName,
                new UserInfo {UserID=2003,UserName="lilijie",StudentID="2015303309",Name="李立杰",
                    Email="123@qq.com",Phone="10041",DepartmentName="软件与微电子学院"});
        }
    }
}
