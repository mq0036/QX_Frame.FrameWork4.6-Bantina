using QX_Frame.Helper_DG.Bantina;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace QX_Frame.App.Base
{
    /**
     * author:qixiao
     * sj:2017-3-1 15:06:24
     **/
    [Serializable]
    public class Entity<DataBaseEntity, TEntity> : IEntity<DataBaseEntity, TEntity> where DataBaseEntity : Bantina
    {
        private static readonly object locker = new object();//locker object
        //New Entity Instance
        public static TEntity Build()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public static TEntity Build(params dynamic[] valueParms)
        {
            TEntity entity = System.Activator.CreateInstance<TEntity>();        // new instance of TEntity
            PropertyInfo[] propertyInfos = entity.GetType().GetProperties();    //get the all public Properties
            if (propertyInfos.Length != valueParms.Length)
                throw new ArgumentException("arguments count not matching --qixiao");    //if arguments`s count not matching throw an exception
            for (int i = 0; i < propertyInfos.Length; i++)
                propertyInfos[i].SetValue(entity, valueParms[i]);               //set value for properties
            return entity;
        }

        protected Type DataBaseType { get { return typeof(DataBaseEntity); } }
        protected Type EntityType { get { return typeof(TEntity); } }

        //Entity to SqlServer DataBase
        public Boolean Add<TEntity2>(TEntity2 entity) where TEntity2 : class
        {
            if (this == null)
            {
                throw new ArgumentNullException(nameof(TEntity));
            }
            lock (locker)
            {
                using (var db = Activator.CreateInstance<DataBaseEntity>())
                {
                    return db.Add(entity).Result;
                }
            }
        }

        public Boolean Update<TEntity2>(TEntity2 entity) where TEntity2 : class
        {
            if (this == null)
            {
                throw new ArgumentNullException(nameof(TEntity));
            }
            lock (locker)
            {
                using (var db = Activator.CreateInstance<DataBaseEntity>())
                {
                    return db.Update(entity).Result;
                }
            }
        }
        public Boolean Update<TEntity2>(TEntity2 entity, Expression<Func<TEntity2, bool>> where) where TEntity2 : class
        {
            if (this == null)
            {
                throw new ArgumentNullException(nameof(TEntity));
            }
            lock (locker)
            {
                using (var db = Activator.CreateInstance<DataBaseEntity>())
                {
                    return db.Update(entity,where).Result;
                }
            }
        }
        public Boolean Delete<TEntity2>(TEntity2 entity) where TEntity2:class
        {
            if (this == null)
            {
                throw new ArgumentNullException(nameof(TEntity));
            }
            lock (locker)
            {
                using (var db = Activator.CreateInstance<DataBaseEntity>())
                {
                    return db.Delete(entity).Result;
                }
            }
        }
        public Boolean Delete<TEntity2>(Expression<Func<TEntity2, bool>> where) where TEntity2 : class
        {
            if (this == null)
            {
                throw new ArgumentNullException(nameof(TEntity));
            }
            lock (locker)
            {
                using (var db = Activator.CreateInstance<DataBaseEntity>())
                {
                    return db.Delete(where).Result;
                }
            }
        }
    }
}
