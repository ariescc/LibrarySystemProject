using LibraryProject.Models;
using LibraryProject.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryProject.DAL
{
    public class UnitOfWork : IDisposable
    {
        private LibraryContext context = new LibraryContext();

        private GenericRepository<User> userRepository;
        private GenericRepository<UserInfo> userInfoRepository;
        private GenericRepository<Department> departmentRepository;
        private GenericRepository<Book> bookRepository;
        private GenericRepository<BorrowAndReturn> borrowAndReturnRepository;
        private GenericRepository<BookType> bookTypeRepository;

        public GenericRepository<User> UserRepository
        {
            get
            {
                if(this.userRepository == null)
                {
                    this.userRepository = new GenericRepository<User>(context);
                }
                return userRepository;
            }
        }

        public GenericRepository<UserInfo> UserInfoRepository
        {
            get
            {
                if (this.userInfoRepository == null)
                {
                    this.userInfoRepository = new GenericRepository<UserInfo>(context);
                }
                return userInfoRepository;
            }
        }

        public GenericRepository<Department> DepartmentRepository
        {
            get
            {
                if (this.departmentRepository == null)
                {
                    this.departmentRepository = new GenericRepository<Department>(context);
                }
                return departmentRepository;
            }
        }

        public GenericRepository<Book> BookRepository
        {
            get
            {
                if (this.bookRepository == null)
                {
                    this.bookRepository = new GenericRepository<Book>(context);
                }
                return bookRepository;
            }
        }

        public GenericRepository<BorrowAndReturn> BorrowAndReturnRepository
        {
            get
            {
                if (this.borrowAndReturnRepository == null)
                {
                    this.borrowAndReturnRepository = new GenericRepository<BorrowAndReturn>(context);
                }
                return borrowAndReturnRepository;
            }
        }

        public GenericRepository<BookType> BookTypeRepository
        {
            get
            {
                if (this.bookTypeRepository == null)
                {
                    this.bookTypeRepository = new GenericRepository<BookType>(context);
                }
                return bookTypeRepository;
            }
        }
        #region Save & Disposes
        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
#endregion
}