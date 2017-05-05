using System;
using System.Collections.Generic;
using SalesAgentDistribution.Model;
using SQLite;
using Xamarin.Forms;
using SalesAgentDistribution.SQLLite;
using System.Linq;
using System.Linq.Expressions;

namespace SalesAgentDistribution.Service
{
    internal interface IDBHandler<T> : IDisposable where T : new()
    {
        int DeleteItem(int id);        
        List<T> GetAll(Expression<Func<T, bool>> selector = null);
        /// <summary>
        /// add all and return the no of rows effected
        /// </summary>
        /// <param name="Ts"> list of objects inserted or added or updated</param>
        /// <returns>no of rows effected</returns>
        int Add(List<T> Ts);
        int Add(T t);
        void DeleteAll();
        int Update(T t);
    }

    internal class DBHandler<T> : IDBHandler<T> where T : new()
    {
        bool disposed = false;
        private SQLiteConnection database;
        private static object collisionLock = new object();

        public DBHandler()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<T>();
        }

        public int Add(List<T> Ts)
        {
            int result = database.InsertAll(Ts);
            return result;
        }

        public int Update(T t)
        {
            int result = database.Update(t);
            return result;
        }

        public int Add(T t)
        {            
            return database.Insert(t);
        }

        //public List<Tt> GetAll<Tt>() where Tt: new()
        public List<T> GetAll(Expression<Func<T, bool>> selector = null)
        {
            if (selector !=null)
            {
                return database.Table<T>().Where(selector).ToList();
            }
            
            return database.Table<T>().ToList();
        }

        //public IEnumerable<T> GetItems()
        //{
        //    return database.Table<Tt>().Select(i=>i).ToList();
        //    //return (from i in database.Table<T>() select i).ToList();
        //}
        //public IEnumerable<T> GetItemsNotDone()
        //{
        //    return database.Query<T>("SELECT * FROM [T] WHERE [Done] = 0");
        //}
        //public T GetItem(int id)
        //{
        //    return database.Table<T>().FirstOrDefault(x => x.ID == id);
        //}

        public int DeleteItem(int id)
        {
            return database.Delete<T>(id);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                if (database != null)
                {
                    database.Close();
                    database.Dispose();
                    database = null;
                }
            }
            disposed = true;
        }

        public void DeleteAll()
        {
            database.DeleteAll<T>();
        }
    }
}