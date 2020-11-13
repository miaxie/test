using Dapper;
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
        public virtual dynamic Insert<T>(T entity) where T : BaseEntity
        {
            return Connection.Insert(entity);
        }

        public virtual void Insert<T>(IEnumerable<T> entities) where T : BaseEntity
        {
            Connection.Insert(entities);
        }

        public virtual void Update<T>(T entity) where T : BaseEntity
        {
             Connection.Update(entity);
        }

        public virtual void Delete<T>(T entity) where T : BaseEntity
        {
            Connection.Delete(entity);
        }

        public T GetByID<T>(int id) where T : class
        {
            using (IDbConnection connection = Connection)
            {
                return Connection.Get<T>(id);
            }
        }

        public async Task<T> GetByIDAsync<T>(int id) where T : class
        {
            using (IDbConnection connection = Connection)
            {
                return await Connection.GetAsync<T>(id);
            }
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

        /// <summary>
        /// dapper通用分页方法
        /// </summary>
        /// <typeparam name="T">泛型集合实体类</typeparam>
        /// <param name="conn">数据库连接池连接对象</param>
        /// <param name="files">列</param>
        /// <param name="tableName">表</param>
        /// <param name="where">条件</param>
        /// <param name="orderby">排序</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">当前页显示条数</param>
        /// <param name="total">结果集总数</param>
        /// <returns></returns>
        public IEnumerable<T> GetPageList<T>(string select, string from, string where, string orderby, int pageIndex, int pageSize, out int total)
        {
            int skip = 1;
            if (pageIndex > 0)
            {
                skip = (pageIndex - 1) * pageSize;
            }
            using (IDbConnection conn = Connection)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("SELECT COUNT(1) {0} {1};", from, where);
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@"  {0} {1} {2} {3}
offset {4} rows fetch next {5} rows only", select, from, where, orderby, skip, pageSize);

                total = conn.QueryFirstOrDefault<int>(sb.ToString());

                return conn.Query<T>(sql.ToString());
            }
        
        }

    }
}