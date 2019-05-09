using System;
using System.Collections.Generic;
using System.Text;

namespace PersonalBlog.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>()
            where TEntity : class;

        int SaveChanges();
    }
}
