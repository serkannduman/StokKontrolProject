using Microsoft.EntityFrameworkCore;
using StokKontrolProject.Domain.Entities;
using StokKontrolProject.Repository.Abstract;
using StokKontrolProject.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace StokKontrolProject.Repository.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StokKontrolContext _context;

        public GenericRepository(StokKontrolContext context)
        {
            _context = context;
        }

        

        public bool Add(T item)
        {
            try
            {
                item.AddedDate = DateTime.Now;
                _context.Set<T>().Add(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Add(List<T> items)
        {
            try
            {
                using(TransactionScope ts = new TransactionScope())
                {
                    foreach (T item in items)
                    {
                        item.AddedDate = DateTime.Now;
                        _context.Set<T>().Add(item);
                    }
                    ts.Complete();

                    return Save() > 0; //1 veya 1 den daha fazla satır döndürüyorsa true döner.
                }
                
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Any(Expression<Func<T, bool>> exp) => _context.Set<T>().Any(exp);
        

        

        public List<T> GetActive() => _context.Set<T>().Where(x=>x.IsActive == true).ToList();
        

        public IQueryable<T> GetActive(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.IsActive == true);

            if (includes!=null)
            {
                query=includes.Aggregate(query,(current,include) => current.Include(include));//
            }

            return query;
        }

        public List<T> GetAll() => _context.Set<T>().ToList();

        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));//
            }

            return query;
        }

        public T GetByDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().FirstOrDefault(exp);

        public T GetById(int id) => _context.Set<T>().Find(id);

        public IQueryable<T> GetByID(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().Where(x => x.ID == id);

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));//
            }

            return query;
        }

        public List<T> GetDefault(Expression<Func<T, bool>> exp) => _context.Set<T>().Where(exp).ToList();

        public bool Remove(T item)
        {
            item.IsActive = false;
            return Update(item);
        }

        public bool Remove(int id)
        {
            try
            {
                using(TransactionScope ts = new TransactionScope())
                {
                    T item = GetById(id);
                    item.IsActive = false;
                    ts.Complete();
                    return Update(item);
                }
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool RemoveAll(Expression<Func<T, bool>> exp)
        {
            try
            {
                using(TransactionScope ts = new TransactionScope())
                {
                    var collection = GetDefault(exp); //verilen ifadeye göre ilgili nesneleri collection'a atıyoruz.
                    int counter = 0;

                    foreach (var item in collection)
                    {
                        item.IsActive = false;

                        bool operationResult = Update(item);
                        if (operationResult) counter++; // Sıradaki item'in silinme işlemi (yani silindi işaretleme) başarılı ise sayacı 1 artırıyoruz.
                    }

                    if (collection.Count == counter) ts.Complete(); // Koleksiyondaki eleman sayısı ile silme işlemi gerçekleşen eleman sayısı (counter'daki sayı) eşit ise bu işlemlerin tümü başarılıdır.
                    else return false;
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Activate(int id)
        {
            T item = GetById(id);
            item.IsActive = true;
            return Update(item);
        }

        public int Save()
        {
            return _context.SaveChanges(); //savechanges kac adet nesne eklendiyse o kadar sayı döner.
        }

        public bool Update(T item)
        {
            try
            {
                item.ModifiedDate = DateTime.Now;
                _context.Set<T>().Update(item);
                return Save() > 0;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public void DetachEntity(T entity)
        {
            _context.Entry<T>(entity).State = EntityState.Detached; //Bir entry'İ takip etmeyi bırakmak için kullanılır.
        }
    }
}
