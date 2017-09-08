/*********************************************************
 * CopyRight: QIXIAO CODE BUILDER. 
 * Version:4.2.0
 * Author:qixiao(柒小)
 * Create:2017-1-13 14:09:05
 * Update:2017-09-08 11:45:49
 * E-mail: dong@qixiao.me | wd8622088@foxmail.com 
 * GitHub: https://github.com/dong666 
 * Personal web site: http://qixiao.me 
 * Technical WebSit: http://www.cnblogs.com/qixiaoyizhan/ 
 * Description:
 * Thx , Best Regards ~
 *********************************************************/
using Autofac;
using QX_Frame.Bantina;
using QX_Frame.Bantina.Bankinate;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;

namespace QX_Frame.App.Base
{
    public abstract class WcfService : Dependency, IWcfService
    {
        static WcfService(){}

        #region Ioc Factory
        private static IContainer _container;
        private static int ExecuteTimes = 0;    //register execute times
        /// <summary>
        /// RegisterComplex  execute when register complex !
        /// </summary>
        protected static void RegisterComplex()
        {
            if (ExecuteTimes <= 0)
            {
                _container = Dependency.Factory();
                ExecuteTimes++;
            }
            else
            {
                new Exception(nameof(RegisterComplex) + " Method can not be used more than one times in a class -- QX_Frame");
            }
        }
        public static ChannelFactory<TService> Wcf<TService>()
        {
            return new ChannelFactory<TService>(_container);
        }
        #endregion

        #region  help method use reflector
        //use reflector to getMethod
        private static readonly MethodInfo _getCount = typeof(WcfService).GetMethod("GetCount", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo _getEntities = typeof(WcfService).GetMethod("GetEntities", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo _getEntitiesPaging = typeof(WcfService).GetMethod("GetEntitiesPaging", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo _getEntity = typeof(WcfService).GetMethod("GetEntity", BindingFlags.NonPublic | BindingFlags.Static);
        private static readonly MethodInfo _executeSql = typeof(WcfService).GetMethod("ExecuteSql", BindingFlags.NonPublic | BindingFlags.Static);

        private static int _totalCount { get; set; } = 0;//the query result count

        private static int GetCount<DBEntity, TBEntity>(WcfQueryObject<DBEntity, TBEntity> query) where DBEntity : Bankinate where TBEntity : class
        {
            using (var db = Activator.CreateInstance<DBEntity>())
            {
                return db.QueryCount(query.BuildQueryFunc<TBEntity>());
            }
        }

        private static object GetEntities<DBEntity, TBEntity>(WcfQueryObject<DBEntity, TBEntity> query) where DBEntity : Bankinate where TBEntity : class
        {
            List<TBEntity> source = null;
            using (var db = Activator.CreateInstance<DBEntity>())
            {
                source = db.QueryEntities(query.BuildQueryFunc<TBEntity>());
            }
            _totalCount = source.Count;
            return source;
        }

        private static object GetEntitiesPaging<DBEntity, TBEntity>(WcfQueryObject<DBEntity, TBEntity> query, Expression<Func<TBEntity, object>> orderBy) where DBEntity : Bankinate where TBEntity : class
        {
            List<TBEntity> source = null;
            int count = 0;
            
            using (var db = Activator.CreateInstance<DBEntity>())
            {
                if (query.PageIndex >= 0 && query.PageSize > 0)
                {
                    source = db.QueryEntitiesPaging(query.PageIndex, query.PageSize, orderBy, query.BuildQueryFunc<TBEntity>(),out count, query.IsDESC);
                }
                else
                {
                    source = db.QueryEntitiesPaging(1, 10, orderBy, query.BuildQueryFunc<TBEntity>(), out count, query.IsDESC);
                }
            }
            _totalCount = source.Count;
            return source;
        }

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "type")]
        private static object GetEntity<DBEntity, TBEntity>(WcfQueryObject<DBEntity, TBEntity> query) where DBEntity : Bankinate where TBEntity : class
        {
            using (var db = Activator.CreateInstance<DBEntity>())
            {
                return db.QueryEntity(query.BuildQueryFunc<TBEntity>());
            }
        }
        //sql query
        private static object ExecuteSql<DBEntity, TBEntity>(WcfQueryObject<DBEntity, TBEntity> query) where DBEntity : Bankinate where TBEntity : class
        {
            if (String.IsNullOrEmpty(query.SqlConnectionString))
            {
                throw new Exception("SqlConnectionString can not be null ! -- QX_Frame");
            }
            if (String.IsNullOrEmpty(query.SqlStatementTextOrSpName))
            {
                throw new Exception("QuerySqlStatementTextOrSpName can not be null ! -- QX_Frame");
            }
            //query execute
            object executeResult = new object() ;
            switch (query.SqlExecuteType)
            {
                case Options.ExecuteType.ExecuteNonQuery:
                    executeResult = Sql_Helper_DG.ExecuteNonQuery(query.SqlStatementTextOrSpName,query.SqlCommandType,query.SqlParameters);
                    break;
                case Options.ExecuteType.ExecuteScalar:
                    executeResult = Sql_Helper_DG.ExecuteScalar(query.SqlStatementTextOrSpName, query.SqlCommandType, query.SqlParameters);
                    break;
                case Options.ExecuteType.ExecuteReader:
                    executeResult = Sql_Helper_DG.ExecuteReader(query.SqlStatementTextOrSpName, query.SqlCommandType, query.SqlParameters);
                    break;
                case Options.ExecuteType.ExecuteDataTable:
                    executeResult = Sql_Helper_DG.ExecuteDataTable(query.SqlStatementTextOrSpName, query.SqlCommandType, query.SqlParameters);
                    break;
                case Options.ExecuteType.ExecuteDataSet:
                    executeResult = Sql_Helper_DG.ExecuteDataSet(query.SqlStatementTextOrSpName, query.SqlCommandType, query.SqlParameters);
                    break;
                case Options.ExecuteType.Execute_Model_T:
                    executeResult = Sql_Helper_DG.Return_T_ByDataReader<TBEntity>(Sql_Helper_DG.ExecuteReader(query.SqlStatementTextOrSpName, query.SqlCommandType, query.SqlParameters));
                    break;
                case Options.ExecuteType.Execute_List_T:
                    executeResult = Sql_Helper_DG.Return_List_T_ByDataSet<TBEntity>(Sql_Helper_DG.ExecuteDataSet(query.SqlStatementTextOrSpName,query.SqlCommandType,query.SqlParameters));
                    break;
                case Options.ExecuteType._ChooseOthers_IfYouChooseThisYouWillGetAnException:
                    throw new Exception("must choose the right ExecuteType ! -- QX_Frame");
            }
            return executeResult;
        }
        #endregion

        #region public query method region

        public WcfQueryResult QueryAll(WcfQueryObject query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            System.Type[] typeArguments = new System.Type[] { query.db_type, query.tb_type };
            object[] parameters = new object[] { query };
            return new WcfQueryResult(_getEntities.MakeGenericMethod(typeArguments).Invoke(null, parameters)) { TotalCount = _totalCount };
        }
        public WcfQueryResult QueryAllPaging<TBEntity, TKey>(WcfQueryObject query, Expression<Func<TBEntity, TKey>> orderBy) where TBEntity : class
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (orderBy == null)
            {
                throw new ArgumentNullException("if you want to paging must use OrderBy arguments  -- QX_Frame");
            }
            System.Type[] typeArguments = new System.Type[] { query.db_type, query.tb_type, typeof(TKey) };
            object[] parameters = new object[] { query, orderBy };
            return new WcfQueryResult(_getEntitiesPaging.MakeGenericMethod(typeArguments).Invoke(null, parameters)) { TotalCount = _totalCount };
        }

        public int QueryCount(WcfQueryObject query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            System.Type[] typeArguments = new System.Type[] { query.db_type, query.tb_type };
            object[] parameters = new object[] { query };
            return (int)_getCount.MakeGenericMethod(typeArguments).Invoke(null, parameters);
        }

        public WcfQueryResult QuerySingle(WcfQueryObject query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            System.Type[] typeArguments = new System.Type[] { query.db_type, query.tb_type };
            object[] parameters = new object[] { query };
            return new WcfQueryResult(_getEntity.MakeGenericMethod(typeArguments).Invoke(null, parameters)) { TotalCount = 1 };
        }

        public WcfQueryResult QuerySql(WcfQueryObject query)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            System.Type[] typeArguments = new System.Type[] { query.db_type, query.tb_type };
            object[] parameters = new object[] { query };
            return new WcfQueryResult(_executeSql.MakeGenericMethod(typeArguments).Invoke(null, parameters)) { TotalCount = 1 };
        }

        #endregion
    }
}
