using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        //Dễ dàng thay đổi DbContext - Lấy ở tầng Data
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        //Dễ dàng sửa đổi DbSet
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }

        //dbSet = public DbSet<Category> Categories { get; set; }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        //Get entity theo Id
        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        //Fillter
        public IEnumerable<T> GetAll(Expression<Func<T, bool>> fillter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {

            IQueryable<T> query = dbSet;

            //Kiểm tra Fillter có không
            if(fillter != null)
            {
                query = query.Where(fillter);
            }

            //Liên kết sản phẩm danh mục, ví dụ sản phẩm A có danh mục B,C
            if(includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public T GetFirstOrDefualt(Expression<Func<T, bool>> fillter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;

            //Kiểm tra Fillter có không
            if (fillter != null)
            {
                query = query.Where(fillter);
            }

            //Liên kết sản phẩm danh mục, ví dụ sản phẩm A có danh mục B,C
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        //T là một thực thể, một entity, một class
        public void Remove(int id)
        {
            T entity = dbSet.Find(id);
            Remove(entity);
        }

        public void Remove(T entity)
        {
            Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
