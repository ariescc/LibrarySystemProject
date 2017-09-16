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