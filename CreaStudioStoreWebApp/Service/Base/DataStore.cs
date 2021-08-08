using AspnetRun.Core.Entities.Base;
using CreaStudioStoreWebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CreaStudioStoreWebApp.Service.Base
{
    public static class BaseFill<T> where T : EntityBase
    {
        public static T Fill(T table)
        {
            BaseFill<T>.FillData((EntityBase)table);
            return table;
        }

        private static void FillData(EntityBase table)
        {
            table.Id = Guid.NewGuid();
            table.CreatedOn = table.CreatedOn == DateTimeOffset.MinValue ? DateTime.UtcNow : table.CreatedOn;
            table.LatestUpdatedOn = new DateTime?(DateTime.UtcNow);
            table.IsDeleted = false;
        }
    }
    public static class DataStore<T> where T : EntityBase
    {
        private static double? _PerformenceLogTime = new double?(200);
        private static ApplicationDbContext entities = ApplicationDbContext.doGetDbContext();

        public static T doAddEntity(T model)
        {
            entities.Set<T>().Add(BaseFill<T>.Fill(model));
            try
            {
                var result = entities.SaveChanges();
                return model;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static int doAddEntityRange(List<T> tables)
        {
            try
            {
                foreach (T table in tables)
                {
                    BaseFill<T>.Fill(table);
                }
                entities.Set<T>().AddRange((IEnumerable<T>)tables);
                var result = entities.SaveChanges();
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        public static T doUpdateEntity(T model)
        {
            try
            {

                var local = entities.Set<T>().Local.FirstOrDefault(f => f.Id == model.Id);
                if (local != null)
                {
                    entities.Entry(local).State = EntityState.Detached;
                }

                model.LatestUpdatedOn = new DateTime?(DateTime.Now);
                entities.Entry<T>(model).State = EntityState.Modified;

                var result = entities.SaveChanges();
                return model;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static int doUpdateEntityRange(IEnumerable<T> tables)
        {
            try
            {
                foreach (T table in tables)
                {
                    table.LatestUpdatedOn = new DateTime?(DateTime.Now);
                    entities.Entry<T>(table).State = EntityState.Modified;
                }
                return entities.SaveChanges();
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }

        public static int doSoftDeleteEntity(T Model)
        {
            try
            {
                entities.Set<T>().Remove(Model);
                var result = entities.SaveChanges();
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }

        public static int doDeleteEntity(T model)
        {
            try
            {
                var local = entities.Set<T>().Local.FirstOrDefault(f => f.Id == model.Id);
                if (local != null)
                {
                    entities.Entry(local).State = EntityState.Detached;
                }
                model.LatestUpdatedOn = new DateTime?(DateTime.Now);
                model.IsDeleted = true;
                entities.Entry<T>(model).State = EntityState.Modified;
                var result = entities.SaveChanges();
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }

        public static int doDeleteEntityRange(IEnumerable<T> tables)
        {
            try
            {
                foreach (T table in tables)
                {
                    table.IsDeleted = true;
                    table.LatestUpdatedOn = new DateTime?(DateTime.Now);
                    entities.Entry<T>(table).State = EntityState.Modified;
                }
                var result = entities.SaveChanges();
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }

        public static List<T> doGetListEntity(Expression<Func<T, bool>> filter = null, string includeProperties = "", Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int skip = 0, int take = 0)
        {
            try
            {
                IQueryable<T> query = entities.Set<T>().AsNoTracking().Where(x => x.IsDeleted == false);
                if (filter != null)
                    query = query.Where(filter);

                foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

                if (orderBy != null)
                    return orderBy(query).ToList();

                if (skip > 0 && take > 0)
                    query = query.OrderByDescending<T, DateTimeOffset>((Expression<Func<T, DateTimeOffset>>)(c => c.CreatedOn)).Skip<T>(skip).Take<T>(take);
                else if (skip == 0 && take > 0)
                    query = query.OrderByDescending<T, DateTimeOffset>((Expression<Func<T, DateTimeOffset>>)(c => c.CreatedOn)).Take<T>(take);

                var result = query.AsNoTracking().ToList();
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static T doGetEntity(Expression<Func<T, bool>> predicate, string includeProperties = "")
        {
            try
            {
                IQueryable<T> query = entities.Set<T>().Where(x => x.IsDeleted == false);
                if (predicate != null)
                    query = query.Where(predicate);
                foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

                var result = query.AsNoTracking().FirstOrDefault();
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static T doGetEntityByID(Guid id, string includeProperties = "")
        {
            try
            {
                IQueryable<T> query = entities.Set<T>().Where(x => x.IsDeleted == false && x.Id == id);
                foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                    query = query.Include(includeProperty);

                var result = query.AsNoTracking().FirstOrDefault();
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static T doFindEntityByID(object id)
        {
            var result = entities.Set<T>().Find(id);
            return result;
        }

        public static T doFindEntity(Expression<Func<T, bool>> predicate, string includeProperties = "")
        {
            string quary = "";
            int rowscount = 0;
            try
            {
                IQueryable<T> query = entities.Set<T>();
                foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
                quary = query.ToString();
                int num1 = query.Count<T>();
                rowscount = num1;
                var result = query.Where(predicate).FirstOrDefault();
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
                return null;
            }
        }

        public static int doGetCount(Expression<Func<T, bool>> expression = null)
        {
            string quary = "";
            int num = 0;
            IQueryable<T> source = entities.Set<T>();
            try
            {
                if (expression == null)
                    source = source.Where<T>((Expression<Func<T, bool>>)(c => c.IsDeleted == false));
                else
                    source = source.Where<T>((Expression<Func<T, bool>>)(c => c.IsDeleted == false)).Where<T>(expression).AsQueryable<T>();

                quary = source.ToString();
                num = source.Count<T>();
            }
            catch (System.Exception ex)
            {
                return 0;
                
            }
            return num;
        }

    }
}