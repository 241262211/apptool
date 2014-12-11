//----------------------------------------------------------------------------
// Copyright (C) 2011, AGRICULTURAL BANK OF CHINA, Corp. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    /// <summary>
    /// 获得sqlserver的操作
    /// </summary>
    public class SQLServerOperator : DBOperator
    {
        /// <summary>
        /// 锁定标志
        /// </summary>
        private static object lockFlag = new object();
        /// <summary>
        /// db操作码
        /// </summary>
        private static DBOperator dbOperator;
        /// <summary>
        /// sqlserver操作码
        /// </summary>
        private SQLServerOperator()
            : base()
        {
        }
        /// <summary>
        /// 获得db操作实例
        /// </summary>
        /// <returns></returns>
        public static DBOperator GetDBOperatorInstance()
        {
            try
            {
                lock (lockFlag)
                {
                    if (dbOperator == null)
                    {
                        dbOperator = new SQLServerOperator();
                    }
                }
                return dbOperator;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得db命令
        /// </summary>
        /// <returns></returns>
        protected override IDbCommand GetDBCommand()
        {
            try
            {
                return new SqlCommand();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得连接字串
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected override string GetConnnectionString(DBSource dbSource, string userID, string password)
        {
            try
            {
                return string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Max Pool Size={4}", dbSource.IPAddress, dbSource.DBName, dbSource.DefaultUser, dbSource.DefaultPWD, dbSource.MaxConnNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得连接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        protected override System.Data.IDbConnection GetDBConnection(string connStr)
        {
            try
            {
                return new SqlConnection(connStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ds"></param>
        /// <param name="sqlParams"></param>
        protected override void FillQueryData(IDbCommand command, DataSet ds, params object[] sqlParams)
        {
            try
            {
                //add by lwl 2008-3-20
                if (sqlParams != null && sqlParams.Length > 0)
                {
                    SqlParameter[] parAms = (SqlParameter[])sqlParams;
                    for (int i = 0; i < parAms.Length; i++)
                        command.Parameters.Add(parAms[i]);
                }
                //add end

                SqlDataAdapter adapter = new SqlDataAdapter(command as SqlCommand);
                adapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得sql参数
        /// </summary>
        /// <param name="parAms"></param>
        /// <returns></returns>
        protected override IDbDataParameter[] GetDBParameter(object[] parAms)
        {
            try
            {
                if (parAms != null && parAms.Length > 0)
                    return (SqlParameter[])parAms;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public override IDataParameter CreateParameter(string parameterName, object value)
        {
            return new SqlParameter(parameterName, value);
        }
        /// <summary>
        /// 重载
        /// </summary>
        /// <param name="Count"></param>
        /// <returns></returns>
        public override IDataParameter[] CreateParameterCount(int count)
        {
            return new SqlParameter[count];
        }
    }
}
