using Dapper;
//using DapperExtensions;
using Microsoft.Extensions.Options;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TorchPoints.Core.Domain;

namespace TorchPoints.Core.DataAccess
{
    public class DapperClient
    {
        public ConnectionConfig CurrentConnectionConfig { get; set; }

        public DapperClient(IOptionsMonitor<ConnectionConfig> config)
        {
            CurrentConnectionConfig = config.CurrentValue;
        }

        public DapperClient(ConnectionConfig config) { CurrentConnectionConfig = config; }

        IDbConnection _connection = null;
        public IDbConnection Connection
        {
            get
            {
                switch (CurrentConnectionConfig.DbType)
                {
                    case DbStoreType.MySql:
                        _connection = new MySql.Data.MySqlClient.MySqlConnection(CurrentConnectionConfig.ConnectionString);
                        Dapper.SimpleCRUD.SetDialect(Dapper.SimpleCRUD.Dialect.MySQL);
                        break;
                    //case DbStoreType.Sqlite:
                    //    _connection = new SQLiteConnection(CurrentConnectionConfig.ConnectionString);
                    //    break;
                    case DbStoreType.SqlServer:
                        _connection = new System.Data.SqlClient.SqlConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    case DbStoreType.Oracle:
                        _connection = new OracleConnection(CurrentConnectionConfig.ConnectionString);
                        break;
                    default:
                        throw new Exception("未指定数据库类型！");
                }
                return _connection;
            }
        }
        public virtual dynamic Insert<T>(T entity,IDbTransaction transaction = null) where T : BaseEntity
        {
            return Connection.Insert(entity, transaction);
        }
        public virtual void Insert<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            Connection.Insert(entities);
        }

        public virtual int Update<T>(T entity, IDbTransaction transaction = null) where T : BaseEntity
        {
            return Connection.Update(entity, transaction);
        }

        public virtual int Delete<T>(T entity) where T : BaseEntity
        {
            return Connection.Delete(entity);
        }

        public virtual int Delete<T, TIdType>(IEnumerable<TIdType> ids, string tablename = null, string schema = null, string keyname = null)
        {
            if (ids == null) throw new ArgumentNullException("ids");

            if (!ids.Any()) return 0;

            schema = string.IsNullOrWhiteSpace(schema) ? string.Empty : (schema.TrimEnd('.') + ".");

            if (string.IsNullOrWhiteSpace(keyname)) keyname = "Id";

            var sql = string.Format("DELETE t FROM {0}{1} t WHERE t.{2} IN @ids;", schema, tablename ?? typeof(T).Name, keyname);

            //return Connection.Execute(sql, ids.ToList());
            return Connection.Execute(sql, new { @ids = ids.ToList() });
        }

        public virtual T Get<T>(dynamic id) where T : BaseEntity
        {
            return DapperExtensions.DapperExtensions.Get<T>(Connection, id);
        }

        /// <summary>
        /// 执行SQL返回集合
        /// </summary>
        /// <param name="strSql">sql语句</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, null).ToList();
            }
        }

        /// <summary>
        /// 执行SQL返回集合
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual List<T> Query<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql, param).ToList();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.Query<T>(strSql).FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <returns></returns>
        public virtual async Task<T> QueryFirstAsync<T>(string strSql)
        {
            using (IDbConnection conn = Connection)
            {
                var res = await conn.QueryAsync<T>(strSql);
                return res.FirstOrDefault<T>();
            }
        }

        /// <summary>
        /// 执行SQL返回一个对象
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="obj">参数model</param>
        /// <returns></returns>
        public virtual T QueryFirst<T>(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                return conn.QueryFirstOrDefault<T>(strSql, param);
            }
        }

        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="strSql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <returns>0成功，-1执行失败</returns>
        public virtual int Execute(string strSql, object param)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strSql, param) > 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <returns></returns>
        public virtual int ExecuteStoredProcedure(string strProcedure)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strProcedure, null, null, null, CommandType.StoredProcedure) == 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="strProcedure">过程名</param>
        /// <param name="param">参数</param>
        /// <returns></returns>
        public virtual int ExecuteStoredProcedure(string strProcedure, object param)
        {
            using (IDbConnection conn = Connection)
            {
                try
                {
                    return conn.Execute(strProcedure, param, null, null, CommandType.StoredProcedure) == 0 ? 0 : -1;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public IPagedList<T> GetPaged<T>(string queryFields, string fromClause, string whereClause, string orderBy,
            int pageIndex, int pageSize, IDictionary<string, object> param = null,
            IDbTransaction tran = null, int? commandTimeout = null, CommandType? commandType = null, bool buffered = true) where T : class, new()
        {
            if (pageIndex<0)
            {
                pageIndex = 0;
            }
            using (IDbConnection conn = Connection)
            {
                var totalCount = conn.ExecuteScalar<int>(string.Format("select count(1) {0} {1}", fromClause, whereClause), param);

                if (totalCount > 0)
                {
                    var sql = string.Format("select {0} {1} {2} {3}", queryFields, fromClause, whereClause, orderBy);
                    var pagingSql = DapperExtensions.DapperExtensions.SqlDialect.GetPagingSql(sql, pageIndex, pageSize, param);
                    var result = Connection.Query<T>(pagingSql, param, tran, buffered, commandTimeout, commandType);

                    return new PagedList<T>(result, pageIndex, pageSize, totalCount);
                }
                return new PagedList<T>(Enumerable.Empty<T>(), pageIndex, pageSize, totalCount);
            }
        }

        public virtual IPagedList<T> GetPaged<T>(string sql, int pageIndex,
          int pageSize, IDictionary<string, object> param = null, string orderBy = null, IDbTransaction tran = null, int? commandTimeout = null,
          CommandType? commandType = null, bool buffered = true) where T : class, new()
        {
            using (IDbConnection conn = Connection)
            {
                var totalCount = conn.ExecuteScalar<int>(string.Format("select count(1) from ({0}) as temp", sql), param);
                if (totalCount > 0)
                {
                    sql = string.Format("{0} {1}", sql, orderBy);
                    var pagingSql = DapperExtensions.DapperExtensions.SqlDialect.GetPagingSql(sql, pageIndex, pageSize, param);
                    var result = Connection.Query<T>(pagingSql, param, tran, buffered, commandTimeout, commandType);

                    return new PagedList<T>(result, pageIndex, pageSize, totalCount);
                }
                return new PagedList<T>(Enumerable.Empty<T>(), pageIndex, pageSize, totalCount);
            }
        }

        #region 事务
        public virtual IDbTransaction GetTransaction()
        {
            using (IDbConnection conn = Connection)
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();

                return conn.BeginTransaction();
            }
        }

        public virtual void TransactionCommit(IDbTransaction tran, bool closeConnection = false)
        {
            using (IDbConnection conn = Connection)
            {
                tran.Commit();

                if (closeConnection && conn.State == ConnectionState.Open)
                    tran.Connection.Close();
            }
        }

        public virtual void TranRollback(IDbTransaction tran, bool closeConnection = false)
        {
            using (IDbConnection conn = Connection)
            {
                tran.Rollback();

                if (closeConnection && conn.State == ConnectionState.Open)
                    tran.Connection.Close();
            }
        }
        #endregion

    }
}