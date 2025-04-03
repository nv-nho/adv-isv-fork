using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;
using System.Data;

namespace MailProcess
{
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

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public DB(string connectionString)
        {
            this.connectionString = connectionString;

            this.con = new SqlConnection(this.connectionString);
            con.Open();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isolationLevel">IsolationLevel</param>
        public DB(string connectionString, System.Data.IsolationLevel isolationLevel)
        {
            this.connectionString = connectionString;

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
                cmd.CommandType = CommandType.Text;
                ret = cmd.ExecuteNonQuery();
            }

            return ret;
        }

        #region Commit
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

        #endregion

        #region Private Method
        /// <summary>
        /// GetConnection
        /// </summary>
        /// <returns></returns>
        private DbConnection GetConnection()
        {
            return new SqlConnection(this.connectionString);
        }

        /// <summary>
        /// SetCommand
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cmdText"></param>
        /// <param name="value"></param>
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

        #region IDisposable
        /// <summary>
        /// Dispose
        /// </summary>
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
    }
}
