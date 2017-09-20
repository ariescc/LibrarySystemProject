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

            context.Users.AddOrUpdate(ctx => ctx.UserName,
                new User { UserName="admin",Password="12345678",PasswordComfirm="12345678",
                 Email="0000@qq.com",PhoneNum="1212123123",Role="admin"},
                new User { UserName="libraryadmin", Password="87654321", PasswordComfirm="87654321",
                 Email="1111@qq.com",PhoneNum="1231231", Role="libraryadmin"},
                new User { UserName="student", Password="88888888",PasswordComfirm="88888888",
                 Email="888@qq.com",PhoneNum="123123",Role="user"}
                );

        }
    }
}
