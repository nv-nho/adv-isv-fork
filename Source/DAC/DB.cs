using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace OMS.DAC
{
    /// <summary>
    /// DB Class
    /// </summary>
    public class DB : IDisposable
    {
        #region Variable

        /// <summary>
        /// Connection String
        /// </summary>
        private string connectionString;

        /// <summary>
        /// Connection
        /// </summary>
        internal DbConnection con;

        /// <summary>
        /// Transaction
        /// </summary>
        internal DbTransaction trans;

        #endregion

        /// <summary>
        /// Get Now date
        /// </summary>
        public DateTime NowDate
        {
            get
            {
                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.Transaction = this.trans;
                    cmd.CommandText = "SELECT GETDATE()";
                    return (DateTime)cmd.ExecuteScalar();
                }
            }
        }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DB()
        {
            var cnnStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            this.connectionString = cnnStr;
            
            this.con = new SqlConnection(this.connectionString);
            con.Open();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isolationLevel">IsolationLevel</param>
        public DB(System.Data.IsolationLevel isolationLevel)
        {
            var cnnStr = ConfigurationManager.ConnectionStrings["ApplicationServices"].ConnectionString;
            this.connectionString = cnnStr;

            this.con = new SqlConnection(this.connectionString);
            con.Open();
            this.trans = con.BeginTransaction(isolationLevel);
        }

        ~DB()
        {
            this.connectionString = null;
            if (this.trans != null)
            {
                this.trans.Rollback();
            }
            this.trans = null;

            this.con = null;
        }

        #endregion

        #region Method

        /// <summary>
        /// Find
        /// </summary>
        /// <typeparam name="T">Class T</typeparam>
        /// <param name="cmdText">CommandText</param>
        /// <param name="value">Parameters</param>
        /// <returns>Class T</returns>
        internal T Find<T>(string cmdText, Hashtable value = null)
        {
            using (DbCommand cmd = this.con.CreateCommand())
            {
                cmd.Transaction = this.trans;
                this.SetCommand(cmd, cmdText, value);
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return (T)typeof(T).GetConstructor(new System.Type[] { typeof(DbDataReader) }).Invoke(new object[] { dr });
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// Find
        /// </summary>
        /// <typeparam name="T">Class T</typeparam>
        /// <param name="cmdText">CommandText</param>
        /// <param name="value">Parameters</param>
        /// <returns>Class T</returns>
        internal T Find1<T>(string cmdText, Hashtable value = null)
        {
            using (DbCommand cmd = this.con.CreateCommand())
            {
                cmd.Transaction = this.trans;
                this.SetCommand(cmd, cmdText, value);
                cmd.CommandType = CommandType.Text;
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return (T)typeof(T).GetConstructor(new System.Type[] { typeof(DbDataReader) }).Invoke(new object[] { dr });
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// Find list
        /// </summary>
        /// <typeparam name="T">Class T</typeparam>
        /// <param name="cmdText">CommandText</param>
        /// <param name="value">Parameters</param>
        /// <returns>List Class T</returns>
        internal IList<T> FindList1<T>(string cmdText, Hashtable value = null)
        {
            IList<T> ret = new List<T>();
            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = this.trans;
                this.SetCommand(cmd, cmdText, value);
                cmd.CommandType = CommandType.Text;
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ret.Add((T)typeof(T).GetConstructor(new System.Type[] { typeof(DbDataReader) }).Invoke(new object[] { dr }));
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// Find list
        /// </summary>
        /// <typeparam name="T">Class T</typeparam>
        /// <param name="cmdText">CommandText</param>
        /// <param name="value">Parameters</param>
        /// <returns>List Class T</returns>
        internal IList<T> FindList<T>(string cmdText, Hashtable value = null)
        {
            IList<T> ret = new List<T>();
            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = this.trans;
                this.SetCommand(cmd, cmdText, value);
                using (DbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ret.Add((T)typeof(T).GetConstructor(new System.Type[] { typeof(DbDataReader) }).Invoke(new object[] { dr }));
                    }
                }
            }

            return ret;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="cmdText">CommandText</param>
        /// <param name="value">Parameters</param>
        /// <returns>Number record</returns>
        internal int ExecuteNonQuery(string cmdText, Hashtable value = null)
        {
            int ret = 0;
            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = this.trans;
                this.SetCommand(cmd, cmdText, value);

                ret = cmd.ExecuteNonQuery();
            }

            return ret;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="cmdText">CommandText</param>
        /// <param name="value">Parameters</param>
        /// <returns>Number record</returns>
        internal int ExecuteNonQuery1(string cmdText, Hashtable value = null)
        {
            int ret = 0;
            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = this.trans;
                this.SetCommand(cmd, cmdText, value);
                cmd.CommandType = CommandType.Text;
                ret = cmd.ExecuteNonQuery();
            }

            return ret;
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="cmdText">CommandText</param>
        /// <param name="value">Parameters</param>
        /// <returns>Number record</returns>
        internal object ExecuteScalar1(string cmdText, Hashtable value = null)
        {
            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = this.trans;
                this.SetCommand(cmd, cmdText, value);
                cmd.CommandType = CommandType.Text;
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// ExecuteNonQuery
        /// </summary>
        /// <param name="cmdText">CommandText</param>
        /// <param name="value">Parameters</param>
        /// <returns>Number record</returns>
        internal object ExecuteScalar(string cmdText, Hashtable value = null)
        {
            using (DbCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = this.trans;
                this.SetCommand(cmd, cmdText, value);
                return cmd.ExecuteScalar();
            }
        }

        /// <summary>
        /// GetIdentityId
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int GetIdentityId<T>()
        {
            var tableName = typeof(T).Name;
            string cmdText = "P_GetIdentityId_W";
            Hashtable paras = new Hashtable();
            paras.Add("IN_TableName", tableName);
            string ret = ExecuteScalar(cmdText, paras).ToString();

            return string.IsNullOrEmpty(ret) ? 0 : int.Parse(ret);
        }

        /// <summary>
        /// Rollback
        /// </summary>
        public void Commit()
        {
            if (this.trans != null)
            {
                this.trans.Commit();
                this.trans.Dispose();
                this.trans = null;
            }            
            this.con.Dispose();
            this.con = null;
        }           

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (this.con != null)
            {
                if (this.trans != null)
                {
                    this.trans.Rollback();
                    this.trans.Dispose();
                    this.trans = null;
                }

                this.con.Dispose();
                this.con = null;
            }
        }

        #endregion

        #region Private Method

        private System.Data.Common.DbConnection GetConnection()
        {
            return new SqlConnection(this.connectionString);
        }

        private void SetCommand(DbCommand cmd, string cmdText, Hashtable value)
        {
            cmd.CommandText = cmdText;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 90;
            if (null != value)
            {
                foreach (string key in value.Keys)
                {
                    cmd.Parameters.Add(new SqlParameter(key, value[key]));
                }
            }
        }

        #endregion
    }

}