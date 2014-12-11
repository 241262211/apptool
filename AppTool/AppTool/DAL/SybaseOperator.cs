//----------------------------------------------------------------------------
// Copyright (C) 2011, AGRICULTURAL BANK OF CHINA, Corp. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Sybase.Data.AseClient;
using System.Data.OleDb;

namespace DAL
{
    /// <summary>
    /// sybase操作类
    /// </summary>
    public class SybaseOperator : DBOperator
    {
        /// <summary>
        /// 锁定标志
        /// </summary>
        private static object lockFlag = new object();
        /// <summary>
        /// DBOperator
        /// </summary>
        private static DBOperator dbOperator;
        /// <summary>
        /// 构造
        /// </summary>
        private SybaseOperator()
            : base()
        {
        }
        /// <summary>
        /// 获得实例
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
                        dbOperator = new SybaseOperator();
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
        /// 获取连接字串
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected override string GetConnnectionString(DBSource dbSource, string userID, string password)
        {
            try
            {
                return string.Format("Data Source='{0}';Port='{1}';UID='{2}';PWD='{3}';Database='{4}';charset='cp936';Language='us_english';Connect Timeout='300000';Pooling='false';TextSize='1024000000'", dbSource.IPAddress, dbSource.Port, userID, password, dbSource.DBName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得db连接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        protected override IDbConnection GetDBConnection(string connStr)
        {
            try
            {
                return new AseConnection(connStr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得ole连接字串
        /// </summary>
        /// <param name="dbSource"></param>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        protected override string GetOleConnnectionString(DBSource dbSource, string userID, string password)
        {
            return string.Format("Provider=Sybase.ASEOLEDBProvider.2;Server Name={0},{1};Initial Catalog={2};User ID={3};Password={4}", dbSource.IPAddress, dbSource.Port.ToString(), dbSource.DBName, userID, password);

        }
        /// <summary>
        /// 获得ole连接
        /// </summary>
        /// <param name="connStr"></param>
        /// <returns></returns>
        protected override OleDbConnection GetOleDBConnection(string connStr)
        {
            try
            {
                return new OleDbConnection(connStr);
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
                return new AseCommand();
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
        protected override void FillQueryData(System.Data.IDbCommand command, DataSet ds, params object[] sqlParams)
        {
            try
            {
                AseDataAdapter adapter = new AseDataAdapter(command as AseCommand);
                adapter.Fill(ds);
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
        /// <param name="tableName"></param>
        /// <param name="sqlParams"></param>
        protected override void FillQueryData(IDbCommand command, DataSet ds, string tableName, params object[] sqlParams)
        {
            try
            {
                AseDataAdapter adapter = new AseDataAdapter(command as AseCommand);
                adapter.Fill(ds, tableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
