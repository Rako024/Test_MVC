using Core.RepositoryAsbtracts;
using Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.RepositoryConcretes
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        AppDbContext _db;

        public GenericRepository(AppDbContext db)
        {
            _db = db;
        }

        public void Add(T entity)
        {
            _db.Set<T>().Add(entity);
        }

        public void Commit()
        {
            _db.SaveChanges();
        }

        public void Delete(T entity)
        {
            _db.Set<T>().Remove(entity);
        }

        public T Get(Func<T, bool>? func = null)
        {
            return func == null ? _db.Set<T>().FirstOrDefault() :
                                  _db.Set<T>().Where(func).FirstOrDefault();
        }

        public List<T> GetAll(Func<T, bool>? func = null)
        {
            return func == null ? _db.Set<T>().ToList() :
                                 _db.Set<T>().Where(func).ToList();
        }
    }
}
