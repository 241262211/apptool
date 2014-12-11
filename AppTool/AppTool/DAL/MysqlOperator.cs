//----------------------------------------------------------------------------
// Copyright (C) 2013, 王强. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using MySql.Data.MySqlClient;
using System.Data.OleDb;

namespace DAL
{
    /// <summary>
    /// sybase操作类
    /// </summary>
    public class MysqlOperator : DBOperator
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
        private MysqlOperator()
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
                        dbOperator = new MysqlOperator();                        
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
                string connectionString = "server=" + dbSource.IPAddress + ";user id=" + userID + ";password=" + password +
                    ";database=" + dbSource.DBName + ";charset=utf8;Persist Security Info=True;";
                return connectionString;
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

                return new MySqlConnection(connStr);
                    
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
            return string.Format("Provider=MySQLProv;Server Name={0},{1};Initial Catalog={2};User ID={3};Password={4}", dbSource.IPAddress, dbSource.Port.ToString(), dbSource.DBName, userID, password);

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
                return new MySqlCommand();
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
                MySqlDataAdapter adapter = new MySqlDataAdapter(command as MySqlCommand);
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
                MySqlDataAdapter adapter = new MySqlDataAdapter(command as MySqlCommand);
                adapter.Fill(ds, tableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
