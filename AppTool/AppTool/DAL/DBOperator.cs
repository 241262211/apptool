//----------------------------------------------------------------------------
// Copyright (C) 2013, 王强. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Data.OleDb;
using System.Diagnostics;
using System.Web;

namespace DAL
{
    /// <summary>
    /// db操作
    /// </summary>
    public class DBOperator
    {

        /// <summary>
        /// dbid
        /// </summary>
        private static string localDBID = "";
        /// <summary>
        /// 默认超时时间
        /// </summary>
        private int nDefaultDBTimeOut = 3000;
        /// <summary>
        /// 构造
        /// </summary>
        public DBOperator()
        {
            try
            {                
                localDBID = "stockdb";
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
           

        /// <summary>
        /// 返回数据查询第一列第一行数据
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlStr, int timeout, string dbID, string userID, string password)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            command.Connection = conn;
                            command.CommandText = sqlStr;
                            command.CommandTimeout = timeout;
                            object o = command.ExecuteScalar();
                            command.Dispose();
                            this.ReleaseConnection(conn, dbSource);
                            return o;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回数据查询第一列第一行数据,使用数据源缺省用户，密码
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <param name="sqlParams"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlStr, string dbID, params object[] sqlParams)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, dbSource.DefaultUser, dbSource.DefaultPWD);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {

                            command.Connection = conn;
                            command.CommandText = sqlStr;

                            //add by lwl 接受参数 2008-3-20
                            IDbDataParameter[] myParams = this.GetDBParameter(sqlParams);
                            if (myParams != null && myParams.Length > 0)
                            {
                                for (int i = 0; i < myParams.Length; i++)
                                    command.Parameters.Add(myParams[i]);
                            }
                            //add end

                            object o = command.ExecuteScalar();
                            command.Dispose();
                            this.ReleaseConnection(conn, dbSource);
                            return o;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 返回数据查询第一列第一行数据,使用配置文件配置的本地数据源
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sqlStr, params object[] sqlParams)
        {
            try
            {
                return this.ExecuteScalar(sqlStr, localDBID, sqlParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Test Dic
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataSet ExecuteQuerry(string[] sqlStr, string[] tableName, int timeout, string dbID, string userID, string password)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {

                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    if (!string.IsNullOrEmpty(dbSource.DefaultUser))
                    {
                        userID = dbSource.DefaultUser;
                    }
                    if (!string.IsNullOrEmpty(dbSource.DefaultPWD))
                    {
                        password = dbSource.DefaultPWD;
                    }
                    IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            command.Connection = conn;
                            command.CommandTimeout = timeout;
                            DataSet ds = new DataSet();
                            for (int i = 0; i < sqlStr.Length; i++)
                            {
                                command.CommandText = sqlStr[i];
                                FillQueryData(command, ds, tableName[i]);
                            }
                            this.ReleaseConnection(conn, dbSource);
                            return ds;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据查询，返回数据集
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public DataSet ExecuteQuerry(string sqlStr, int timeout, string dbID, string userID, string password)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {

                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    if (!string.IsNullOrEmpty(dbSource.DefaultUser))
                    {
                        userID = dbSource.DefaultUser;
                    }
                    if (!string.IsNullOrEmpty(dbSource.DefaultPWD))
                    {
                        password = dbSource.DefaultPWD;
                    }
                    IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            command.Connection = conn;
                            command.CommandText = sqlStr;
                            command.CommandTimeout = timeout;
                            DataSet ds = new DataSet();
                            FillQueryData(command, ds);
                            this.ReleaseConnection(conn, dbSource);
                            return ds;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据查询，返回数据集,使用数据源缺省用户，密码
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <param name="sqlParams">SqlParamers参数</param>
        /// <returns></returns>
        public DataSet ExecuteQuerry(string sqlStr, string dbID, params object[] sqlParams)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {

                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, dbSource.DefaultUser, dbSource.DefaultPWD);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            command.Connection = conn;
                            command.CommandText = sqlStr;
                            DataSet ds = new DataSet();
                            FillQueryData(command, ds, sqlParams);
                            this.ReleaseConnection(conn, dbSource);
                            return ds;
                        }
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 数据查询，返回DataReader
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public OleDbDataReader ExecuteReaderQuery(string sqlStr, int timeout, string dbID, string userID, string password)
        {
            try
            {

                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    OleDbConnection conn = this.OpenOleConnection(dbSource, userID, password);
                    if (conn != null)
                    {

                        OleDbCommand command = new OleDbCommand(sqlStr, conn);
                        command.CommandTimeout = timeout;
                        return command.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 测试数据查询，返回DataReader，DbDataReader,add by zhanglinjian
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IDataReader ExecuteDbReader(string sqlStr, int timeout, string dbID, string userID, string password)
        {
            try
            {

                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConForDataReader(dbSource, userID, password);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            command.Connection = conn;
                            command.CommandText = sqlStr;
                            command.CommandTimeout = timeout;
                            return command.ExecuteReader(CommandBehavior.CloseConnection);
                        }

                        //OleDbCommand command = new OleDbCommand(sqlStr, conn);
                        //command.CommandTimeout = timeout;
                        //return command.ExecuteReader(CommandBehavior.CloseConnection);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 数据查询，返回数据集,使用配置文件配置的本地数据源
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="sqlParams">command参数，可不写此参数</param>
        /// <returns></returns>
        public DataSet ExecuteQuerry(string sqlStr, params object[] sqlParams)
        {
            try
            {
                return this.ExecuteQuerry(sqlStr, localDBID, sqlParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 数据查询，返回数据集,使用配置文件配置的本地数据源
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public DataSet ExecuteQuerry(string[] sqlStr, string[] tablenam, int timeout)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(localDBID);
                    return this.ExecuteQuerry(sqlStr, tablenam, timeout, localDBID, dbSource.DefaultUser, dbSource.DefaultPWD);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 数据查询，返回数据集,使用配置文件配置的本地数据源
        /// @auth:wq
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public DataSet ExecuteQuerry(string[] sqlStr)
        {
            try
            {
                string[] tablenam = new string[sqlStr.Length];
                for (int i = 0; i < sqlStr.Length; i++)
                {
                    tablenam[i] = "t" + (i + 1).ToString();
                }
                int timeout = nDefaultDBTimeOut;
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(localDBID);
                    return this.ExecuteQuerry(sqlStr, tablenam, timeout, localDBID, dbSource.DefaultUser, dbSource.DefaultPWD);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 非查询类数据操作，返回受影响行数
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int ExecuteNonQuerry(string sqlStr, int timeout, string dbID, string userID, string password)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            command.Connection = conn;
                            command.CommandText = sqlStr;
                            command.CommandTimeout = timeout;
                            int result = command.ExecuteNonQuery();
                            command.Dispose();
                            this.ReleaseConnection(conn, dbSource);
                            return result;
                        }
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 非查询类数据操作，返回受影响行数,使用数据源缺省用户，密码
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <param name="sqlParams">command参数，可不写此参数</param>
        /// <returns></returns>
        public int ExecuteNonQuerry(string sqlStr, string dbID, params object[] sqlParams)
        {
            int result = -1;
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, dbSource.DefaultUser, dbSource.DefaultPWD);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            command.Connection = conn;
                            command.CommandText = sqlStr;

                            //add by lwl 接受参数 2008-3-20
                            try
                            {
                                command.Transaction = conn.BeginTransaction();


                                for (int i = 0; i < sqlParams.Length; i++)
                                    command.Parameters.Add(sqlParams[i]);

                                result = command.ExecuteNonQuery();
                                command.Transaction.Commit();
                            }
                            catch (Exception e)
                            {
                                command.Transaction.Rollback();
                                throw e;
                            }
                            finally
                            {
                                command.Dispose();

                                this.ReleaseConnection(conn, dbSource);
                            }
                            //add end mdf 2008-3-27
                            return result;
                        }
                    }
                }
                return -1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 非查询类数据操作，返回受影响行数,使用配置文件配置的本地数据源
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="sqlParams">command参数，可不写此参数</param>
        /// <returns></returns>
        public int ExecuteNonQuerry(string sqlStr, params object[] sqlParams)
        {
            try
            {
                return this.ExecuteNonQuerry(sqlStr, localDBID, sqlParams);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 多条非查询类数据操作，返回每条语句受影响数目和错误
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerry(string[] sqlStr, string dbID, string userID, string password, int timeout)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    string connName = dbSource.ConnName;
                    IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            string[] result = new string[sqlStr.Length + 1];
                            command.Connection = conn;
                            command.CommandTimeout = timeout;

                            DateTime stime, etime;
                            TimeSpan ctime;
                            stime = DateTime.Now;

                            int j = 0, k = 0;
                            for (int i = 0; i < sqlStr.Length; i++)
                            {
                                command.CommandText = sqlStr[i];
                                try
                                {
                                    int t = 0;
                                    t = command.ExecuteNonQuery();
                                    if (t != 0)
                                    {
                                        result[i] = sqlStr[i] + ";\r\nSUCCESSFUL:影响" + t.ToString() + "条记录。";
                                        j++;
                                    }
                                    else
                                    {
                                        result[i] = sqlStr[i] + ";\r\n" + "ERROR:记录不存在。";
                                        k++;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    result[i] = sqlStr[i] + "\r\n" + ex.Message;
                                    k++;
                                }
                            }
                            etime = DateTime.Now;
                            ctime = etime.Subtract(stime);

                            result[sqlStr.Length] = "\r\n成功：" + j.ToString() + "条语句\t失败：" + k.ToString() + "条语句\r\n"
                                + "执行开始时间：" + stime.ToString("yyyy年MM月dd日HH时mm分ss秒") + "\r\n"
                                + "执行结束时间：" + etime.ToString("yyyy年MM月dd日HH时mm分ss秒") + "\r\n"
                                + "变更执行时间：" + ctime.TotalSeconds.ToString() + "秒\r\n"
                                + "数据库名称：" + connName + "\t用户名：" + userID + "\r\n\r\n"
                                + "##################################################\r\n";
                            command.Dispose();
                            this.ReleaseConnection(conn, dbSource);
                            return result;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 多条非查询类数据操作，返回每条语句受影响数目和错误,使用数据源缺省用户，密码
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerry(string[] sqlStr, string dbID)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, dbSource.DefaultUser, dbSource.DefaultPWD);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            string[] result = new string[sqlStr.Length];
                            command.Connection = conn;
                            for (int i = 0; i < sqlStr.Length; i++)
                            {
                                command.CommandText = sqlStr[i];
                                try
                                {
                                    result[i] = command.ExecuteNonQuery().ToString();
                                }
                                catch (Exception ex)
                                {
                                    result[i] = ex.Message;
                                }
                            }
                            command.Dispose();
                            this.ReleaseConnection(conn, dbSource);
                            return result;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 多条非查询类数据操作，返回每条语句受影响数目和错误,使用配置文件配置的本地数据源
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerry(string[] sqlStr)
        {
            try
            {
                return this.ExecuteNonQuerry(sqlStr, localDBID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 事务中非查询类数据操作，返回受影响行数,需输入事务隔离级别
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerryInTrans(string[] sqlStr, int timeout, string dbID, string userID, string password, IsolationLevel isolationLevel)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            string[] result = new string[sqlStr.Length];
                            int cur = 0;
                            command.Connection = conn;
                            command.CommandTimeout = timeout;
                            command.Transaction = conn.BeginTransaction(isolationLevel);
                            try
                            {
                                for (int i = 0; i < sqlStr.Length; i++)
                                {
                                    cur = i;
                                    command.CommandText = sqlStr[i];
                                    result[i] = command.ExecuteNonQuery().ToString();
                                }
                                command.Transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                result[cur] = ex.Message;
                                command.Transaction.Rollback();
                            }
                            command.Dispose();
                            this.ReleaseConnection(conn, dbSource);
                            return result;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
//         /// <summary>
//         /// 事务中非查询类数据操作，返回受影响行数,需输入事务隔离级别
//         /// </summary>
//         /// <param name="sqlStr"></param>
//         /// <param name="dbID"></param>
//         /// <param name="userID"></param>
//         /// <param name="password"></param>
//         /// <param name="isolationLevel"></param>
//         /// <returns></returns>
//         public string[] ExecuteNonQuerryInTransWithCancel(string[] sqlStr, int timeout, string dbID, string userID, string password, int taskID, IsolationLevel isolationLevel)
//         {
//             try
//             {
//                 InMemoryProgressMonitor progMonitor = new InMemoryProgressMonitor();
//                 DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
//                 if (dbFactory != null)
//                 {
//                     DBSource dbSource = dbFactory.GetDBSource(dbID);
//                     IDbConnection conn = this.OpenConnection(dbSource, userID, password);
//                     if (conn != null)
//                     {
//                         IDbCommand command = this.GetDBCommand();
//                         if (command != null)
//                         {
//                             string[] result = new string[sqlStr.Length];
//                             int cur = 0;
//                             command.Connection = conn;
//                             command.CommandTimeout = timeout;
//                             command.Transaction = conn.BeginTransaction(isolationLevel);
//                             try
//                             {
// 
//                                 for (int i = 0; i < sqlStr.Length; i++)
//                                 {
//                                     cur = i;
//                                     command.CommandText = sqlStr[i];
//                                     result[i] = command.ExecuteNonQuery().ToString();
//                                     progMonitor.SetStatus(taskID, string.Format("{0}|正在执行第{1}条变更[共{2}条]......", (int)((i + 1) * 100 / sqlStr.Length), i + 1, sqlStr.Length));
//                                     if (progMonitor.ShouldTerminate(taskID))
//                                     {
//                                         if (i + 1 == sqlStr.Length) continue;
//                                         result[cur] = string.Format("-1变更被终止，总变更{0}笔，已提交变更{1}笔", sqlStr.Length, i + 1);
//                                         break;
//                                     }
//                                 }
//                                 command.Transaction.Commit();
//                             }
//                             catch (Exception ex)
//                             {
//                                 result[cur] = string.Format("-2第{0}条语句执行失败，原因：{1}", cur + 1, ex.Message);
//                                 command.Transaction.Rollback();
//                             }
//                             command.Dispose();
//                             this.ReleaseConnection(conn, dbSource);
//                             return result;
//                         }
//                     }
//                 }
//                 return null;
//             }
//             catch (Exception ex)
//             {
//                 throw ex;
//             }
//         }

        /// <summary>
        /// 事务中非查询类数据操作，返回受影响行数,需输入事务隔离级别,使用数据源缺省用户，密码
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerryInTrans(string[] sqlStr, string dbID, IsolationLevel isolationLevel)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, dbSource.DefaultUser, dbSource.DefaultPWD);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            string[] result = new string[sqlStr.Length];
                            int cur = 0;
                            command.Connection = conn;
                            command.Transaction = conn.BeginTransaction(isolationLevel);
                            try
                            {
                                for (int i = 0; i < sqlStr.Length; i++)
                                {
                                    cur = i;
                                    command.CommandText = sqlStr[i];
                                    result[i] = command.ExecuteNonQuery().ToString();
                                }
                                command.Transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                result[cur] = ex.Message;
                                command.Transaction.Rollback();
                            }
                            command.Dispose();
                            this.ReleaseConnection(conn, dbSource);
                            return result;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 事务中非查询类数据操作，返回受影响行数,需输入事务隔离级别,使用配置文件配置的本地数据源
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="isolationLevel"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerryInTrans(string[] sqlStr, IsolationLevel isolationLevel)
        {
            try
            {
                return this.ExecuteNonQuerryInTrans(sqlStr, localDBID, isolationLevel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 事务中一句查询+一句更新数据操作，查询返回dataset，事务隔离级别为ReadCommitted,使用配置文件配置的本地数据源
        /// </summary>
        /// <param name="sqlStr">Sql字符串数组</param>
        /// <returns></returns>
        public DataSet ExecuteSelUpdInTrans(string[] sqlStr)
        {
            try
            {
                DataSet ds = new DataSet();
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(localDBID);
                    IDbConnection conn = this.OpenConnection(dbSource, dbSource.DefaultUser, dbSource.DefaultPWD);
                    if (conn != null)
                    {
                        IDbCommand command = this.GetDBCommand();
                        if (command != null)
                        {
                            string[] result = new string[sqlStr.Length];
                            int cur = 0;
                            command.Connection = conn;
                            command.Transaction = conn.BeginTransaction(IsolationLevel.ReadCommitted);
                            try
                            {

                                for (int i = 0; i < sqlStr.Length; i++)
                                {

                                    cur = i;
                                    command.CommandText = sqlStr[i];
                                    if (i == 0)
                                    {
                                        FillQueryData(command, ds);

                                    }
                                    else
                                    {
                                        result[i] = command.ExecuteNonQuery().ToString();
                                    }
                                }
                                command.Transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                result[cur] = ex.Message;
                                command.Transaction.Rollback();
                            }
                            command.Dispose();
                            this.ReleaseConnection(conn, dbSource);
                            return ds;
                        }
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 事务中非查询类数据操作，返回受影响行数,缺省隔离级别ReadUncommitted
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerryInTrans(string[] sqlStr, int timeout, string dbID, string userID, string password)
        {
            try
            {
                return ExecuteNonQuerryInTrans(sqlStr, timeout, dbID, userID, password, IsolationLevel.ReadUncommitted);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 事务中非查询类数据操作，返回受影响行数,缺省隔离级别ReadUncommitted,使用数据源缺省用户，密码
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="dbID"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerryInTrans(string[] sqlStr, string dbID)
        {
            try
            {
                return ExecuteNonQuerryInTrans(sqlStr, dbID, IsolationLevel.ReadUncommitted);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 事务中非查询类数据操作，返回受影响行数,缺省隔离级别ReadUncommitted,使用配置文件配置的本地数据源
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <returns></returns>
        public string[] ExecuteNonQuerryInTrans(string[] sqlStr)
        {
            try
            {
                return ExecuteNonQuerryInTrans(sqlStr, localDBID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 批量更新sql返回更新错误的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        ///----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20110519    王强      创建代码
        ///----------------------------------------------------------
        public string MulSqlToDB(string[] sql)
        {
            string strResult = "0";

            string[] result = new string[sql.Length];
            try
            {
                result = ExecuteNonQuerryInTrans(sql);
                for (int i = 0; i < sql.Length; i++)
                {
                    if (result[i] != "1")
                    {
                        strResult = strResult.TrimStart('0');
                        strResult += "第" + (i + 1).ToString() + "条信息批量更新或插入错误。" + "\n";

                    }

                }
                return strResult;
            }
            catch (Exception e)
            {
                throw e;
            }
        }




        /// <summary>
        /// 测试数据连接是否能够打开
        /// </summary>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns>true：能够打开 false：不能打开</returns>
        public bool TestConnection(string dbID, string userID, string password)
        {
            try
            {
                bool result = false;
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    if (dbSource != null)
                    {
                        IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                        if (conn != null)
                        {
                            result = true;
                            this.ReleaseConnection(conn, dbSource);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 测试数据连接是否能够打开
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="dbName"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns>true：能够打开 false：不能打开</returns>
        public bool TestConnection(string ip, int port, string dbName, string userID, string password)
        {
            try
            {
                bool result = false;
                DBSource dbSource = new DBSource();
                dbSource.IPAddress = ip;
                dbSource.Port = port;
                dbSource.DBName = dbName;
                dbSource.MaxConnNo = 1;
                dbSource.DefaultUser = userID;
                dbSource.DefaultPWD = password;
                IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                if (conn != null)
                {
                    result = true;
                    this.ReleaseConnection(conn, dbSource);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private IDbConnection OpenConnection(DBSource dbSource, string userID, string password)
        {
            try
            {
                IDbConnection conn = null;
                for (int i = 0; i < 10; i++)
                {
                    if (conn == null)
                    {
                        lock (dbSource.LockFlag)
                        {
                            if (dbSource.MaxConnNo > dbSource.CurrentConnNo)
                            {
                                string connStr = this.GetConnnectionString(dbSource, userID, password);
                                if (connStr != null)
                                {
                                    conn = this.GetDBConnection(connStr);
                                    dbSource.CurrentConnNo++;
                                    conn.Open();
                                }
                            }
                        }
                        Thread.Sleep(10);
                    }
                    else
                    {
                        break;
                    }
                }

                return conn;
            }
            catch (Exception ex)
            {
                //return null;
                throw ex;
            }
        }
        /// <summary>
        /// 打开连接
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private IDbConnection OpenConForDataReader(DBSource dbSource, string userID, string password)
        {
            try
            {
                IDbConnection conn = null;
                for (int i = 0; i < 10; i++)
                {
                    if (conn == null)
                    {

                        string connStr = this.GetConnnectionString(dbSource, userID, password);
                        if (connStr != null)
                        {
                            conn = this.GetDBConnection(connStr);

                            conn.Open();
                        }

                        // Thread.Sleep(10);
                    }
                    else
                    {
                        break;
                    }
                }

                return conn;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// 用ole方式连接
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private OleDbConnection OpenOleConnection(DBSource dbSource, string userID, string password)
        {
            try
            {
                OleDbConnection conn = null;
                for (int i = 0; i < 10; i++)
                {
                    if (conn == null)
                    {

                        string connStr = this.GetOleConnnectionString(dbSource, userID, password);

                        if (connStr != null)
                        {
                            conn = this.GetOleDBConnection(connStr);
                            conn.Open();
                        }

                        Thread.Sleep(10);
                    }
                    else
                    {
                        break;
                    }
                }

                return conn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="dbSource"></param>
        private void ReleaseConnection(IDbConnection conn, DBSource dbSource)
        {
            try
            {
                if (conn != null)
                {
                    if (conn.State != ConnectionState.Closed)
                    {
                        lock (dbSource.LockFlag)
                        {
                            conn.Close();
                            dbSource.CurrentConnNo--;
                        }
                    }
                }
                else
                {
                    lock (dbSource.LockFlag)
                    {
                        dbSource.CurrentConnNo--;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        #region transaction
        /// <summary>
        /// 开始事务
        /// </summary>
        /// <param name="dbID"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <param name="isolationLevel"></param>
        /// <returns>返回获取的事务</returns>
        public IDbTransaction BeginTransaction(string dbID, string userID, string password, IsolationLevel isolationLevel)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, userID, password);
                    if (conn != null)
                    {
                        return conn.BeginTransaction(isolationLevel);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 开始事务，使用缺省的用户密码
        /// </summary>
        /// <param name="dbID"></param>
        /// <param name="isolationLevel"></param>
        /// <returns>返回获取的事务</returns>
        public IDbTransaction BeginTransaction(string dbID, IsolationLevel isolationLevel)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                if (dbFactory != null)
                {
                    DBSource dbSource = dbFactory.GetDBSource(dbID);
                    IDbConnection conn = this.OpenConnection(dbSource, dbSource.DefaultUser, dbSource.DefaultPWD);
                    if (conn != null)
                    {
                        return conn.BeginTransaction(isolationLevel);

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 开始本地数据库事务
        /// </summary>
        /// <param name="isolationLevel"></param>
        /// <returns>返回获取的事务对象</returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            try
            {
                return this.BeginTransaction(localDBID, isolationLevel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 结束事务
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="dbID"></param>
        public void EndTransaction(IDbTransaction trans, string dbID)
        {
            try
            {
                if (trans != null)
                {
                    if (trans.Connection.State == ConnectionState.Open)
                    {
                        trans.Commit();
                        DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                        if (dbFactory != null)
                        {
                            DBSource dbSource = dbFactory.GetDBSource(dbID);
                            if (dbSource != null)
                            {
                                this.ReleaseConnection(trans.Connection, dbSource);
                            }
                        }
                    }
                    trans.Dispose();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 结束本地数据事务
        /// </summary>
        /// <param name="trans"></param>
        public void EndTransaction(IDbTransaction trans)
        {
            try
            {
                this.EndTransaction(trans, localDBID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 显式事务中执行查询类SQL,返回结果数据第一列第一行数据，当事务失败时，事务回滚，释放数据连接，并抛出异常
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public object ExecuteScalarWithTrans(IDbTransaction trans, string sqlStr, int timeout)
        {
            try
            {
                IDbCommand command = this.GetDBCommand();
                command.Connection = trans.Connection;
                command.CommandText = sqlStr;
                command.CommandTimeout = timeout;
                object o = command.ExecuteScalar();
                command.Dispose();
                return o;
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                    this.EndTransaction(trans);
                }
                throw ex;
            }
        }

        /// <summary>
        /// 显式事务中执行查询类SQL，当事务失败时，事务回滚，释放数据连接，并抛出异常
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public DataSet ExecuteSQLWithTrans(IDbTransaction trans, string sqlStr, int timeout)
        {
            try
            {
                IDbCommand command = this.GetDBCommand();
                command.Transaction = trans;
                command.Connection = trans.Connection;
                command.CommandText = sqlStr;
                command.CommandTimeout = timeout;
                DataSet ds = new DataSet();
                FillQueryData(command, ds);
                command.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                    this.EndTransaction(trans);
                }
                throw ex;
            }

        }

        /// <summary>
        /// 显式事务中执行查询类SQL，当事务失败时，事务回滚，释放数据连接，并抛出异常
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="sqlStr"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public int ExecuteNonSQLWithTrans(IDbTransaction trans, string sqlStr, int timeout)
        {
            try
            {
                IDbCommand command = this.GetDBCommand();
                command.Transaction = trans;
                command.Connection = trans.Connection;
                command.CommandText = sqlStr;
                command.CommandTimeout = timeout;
                int result = command.ExecuteNonQuery();
                command.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                if (trans != null)
                {
                    trans.Rollback();
                    this.EndTransaction(trans);
                }
                throw ex;
            }
        }
        #endregion

        #region need to override
        /// <summary>
        /// 获得连接字串
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected virtual string GetConnnectionString(DBSource dbSource, string userID, string password)
        {
            return null;
        }
        /// <summary>
        /// 获得ole方式的连接
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected virtual string GetOleConnnectionString(DBSource dbSource, string userID, string password)
        {
            return null;
        }
        /// <summary>
        /// 得到db连接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        protected virtual IDbConnection GetDBConnection(string connStr)
        {
            return null;
        }
        /// <summary>
        /// 获得oledb连接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        protected virtual OleDbConnection GetOleDBConnection(string connStr)
        {
            return null;
        }
        /// <summary>
        /// 获得db命令
        /// </summary>
        /// <returns></returns>
        protected virtual IDbCommand GetDBCommand()
        {
            return null;
        }
        /// <summary>
        /// 获得db参数
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        protected virtual IDbDataParameter[] GetDBParameter(object[] pArams)
        {
            return null;
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ds"></param>
        /// <param name="sqlParams"></param>
        protected virtual void FillQueryData(IDbCommand command, DataSet ds, params object[] sqlParams)
        {

        }

        /// <summary>
        /// for test
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ds"></param>
        /// <param name="tableName"></param>
        /// <param name="sqlParams"></param>
        protected virtual void FillQueryData(IDbCommand command, DataSet ds, string tableName, params object[] sqlParams)
        {

        }

        /// <summary>
        /// 根据数据源，创建同类型的Command参数
        /// </summary>
        /// <param name="parameterName">参数名eg:@UserName</param>
        /// <param name="Value">参数值eg:zhangsan</param>
        /// <returns></returns>
        public virtual IDataParameter CreateParameter(string parameterName, object value)
        {
            return null;
        }
        /// <summary>
        /// 根据数据源，创建同类型的Command参数数组
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        public virtual IDataParameter[] CreateParameterCount(int count)
        {
            return null;
        }
        #endregion
    }

    /// <summary>
    /// Summary description for the progress-monitor server API 
    /// </summary>
    public interface IProgressMonitor
    {
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="taskID"></param>
        /// <param name="message"></param>
        void SetStatus(int taskID, object message);
        /// <summary>
        /// 获得状态
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        string GetStatus(int taskID);
        /// <summary>
        /// 设置终结
        /// </summary>
        /// <param name="taskID"></param>
        /// <returns></returns>
        bool ShouldTerminate(int taskID);
        /// <summary>
        /// 取得终结
        /// </summary>
        /// <param name="taskID"></param>
        void RequestTermination(int taskID);
    }

}
