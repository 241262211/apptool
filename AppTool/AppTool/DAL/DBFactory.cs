//----------------------------------------------------------------------------
// Copyright (C) 2013, 王强. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using MySql.Data.MySqlClient;
using System.Threading;
using System.Xml;
using System.IO;
using System.Web;
using System.Diagnostics;

namespace DAL
{
    /// <summary>
    /// 获得db连接
    /// </summary>
    public class DBFactory
    {
        /// <summary>
        /// DBFactory
        /// </summary>
        private static DBFactory dbFactory = null;
        /// <summary>
        /// 是否锁定
        /// </summary>
        private static object lockFlag = new object();
        /// <summary>
        /// List DBSource
        /// </summary>
        private List<DBSource> dbSoureceList;
        /// <summary>
        /// 当前db的id
        /// </summary>
        private string currentDBID;
        /// <summary>
        /// 读取的INI文件中的db的id
        /// </summary>
        private string localDBID;
        /// <summary>
        /// 构造
        /// </summary>
        private DBFactory()
        {
            try
            {
                this.dbSoureceList = new List<DBSource>();
                this.SetLocalDBSource();
                //this.LoadAMPDBSource();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得db实例
        /// </summary>
        /// <returns></returns>
        public static DBFactory GetDBFactoryInstance()
        {
            try
            {
                lock (lockFlag)
                {
                    if (dbFactory == null)
                    {
                        dbFactory = new DBFactory();
                        //dbFactory.LoadDBSource();
                    }
                }
                return dbFactory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得db类型
        /// </summary>
        /// <param name="dbType"></param>
        /// <returns></returns>
        public DBOperator GetDBOperatorByType(string dbType)
        {
            try
            {
                DBOperator dbAccess = null;
                switch (dbType)
                {
                    case "MYSQL":
                        dbAccess = MysqlOperator.GetDBOperatorInstance();
                        break;
                    default:
                        break;
                }
                return dbAccess;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得db操作
        /// </summary>
        /// <param name="dbID"></param>
        /// <returns></returns>
        public DBOperator GetDBOperator(string dbID)
        {
            try
            {
                //Console.WriteLine("debug:GetDBOperator 0,dbID=" + dbID);
                DBSource dbSource = this.GetDBSource(dbID);
                if (dbSource != null)
                {

                    //DBOperator dbAccess = null;
                    return this.GetDBOperatorByType(dbSource.DBType);
                }
                //Console.WriteLine("debug:GetDBOperator 0,dbSource == null");
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得本地db操作
        /// </summary>
        /// <returns></returns>
        public DBOperator GetLocalDBOperator()
        {
            try
            {
                //System.Configuration.AppSettingsReader reader = new System.Configuration.AppSettingsReader();
                //string localDBID = reader.GetValue("DBID", typeof(string)) as string;
                //return this.GetDBOperator(localDBID);
                return this.GetDBOperator(localDBID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region AMP DBsource

        //
        /// <summary>
        /// 初始化AMP数据库
        /// </summary>
        public void LoadAppDBSource()
        {
            DBSource dbs = new DBSource();
            dbs.DBID = "00";
            dbs.ConnName = "DEV";
            dbs.DBName = "stockdb";
            dbs.DBType = "MYSQL";
            dbs.IPAddress = "127.0.0.1";
            dbs.Port = 3306;
            dbs.DefaultUser = "root";
            dbs.DefaultPWD = "123456";
            dbs.MaxConnNo = 100;
            dbs.CurrentConnNo = 0;
            dbs.DBDescription = "mysql 5.6.20";

            this.dbSoureceList.Add(dbs);
        }
        #endregion

        #region DBSource Management
        /// <summary>
        /// 设置本地数据源
        /// </summary>
        /// <returns></returns>
        private bool SetLocalDBSource()
        {
            //LoadAppDBSource();
            //return true;

            try
            {
                //获得当前程序存放目录
                string strRoot = AppDomain.CurrentDomain.BaseDirectory;
                string strFullName = strRoot + @"INI\DB.config";

                if (!File.Exists(strFullName))
                {
                    if(strRoot.Contains("bin\\"))
                    {
                        strRoot = strRoot.Substring(0, strRoot.IndexOf("bin\\"));
                        strFullName = strRoot + @"INI\DB.config";
                    }
                    else
                    {
                        LoadAppDBSource();
                        return true;  
                    }                                      
                }

                XmlDocument xmlDoc = new XmlDocument();                
                xmlDoc.Load(strFullName);

                XmlNode xnNode;
                XmlAttributeCollection xmlAC;
                XmlAttribute xmlAttKey;
                XmlAttribute xmlAttValue;
                string strKey;
                string strValue;
                XmlNodeList xmlNL;
                IEnumerator iEnum;
                Hashtable htDB = new Hashtable();

                localDBID = xmlDoc.SelectSingleNode("//DataBase//DataBaseCurrentKey").InnerText;
                //获得数据库               
                XmlNodeList dbList = xmlDoc.SelectNodes("//DataBase//DataBaseItem");
                if (null == dbList || dbList.Count < 1)
                {
                    return false;
                }

                foreach (XmlNode dbNode in dbList)
                {
                    xmlNL = dbNode.ChildNodes;
                    iEnum = xmlNL.GetEnumerator();

                    while (iEnum.MoveNext())
                    {
                        xnNode = (XmlNode)iEnum.Current;
                        xmlAC = xnNode.Attributes;

                        xmlAttKey = xmlAC["key"];
                        xmlAttValue = xmlAC["value"];

                        strKey = xmlAttKey.Value;
                        strValue = xmlAttValue.Value;
                        htDB.Add(strKey, strValue);
                    }

                    DBSource localSource = new DBSource();
                    localSource.DBID = htDB["DBID"].ToString();
                    localSource.DBType = htDB["DBType"].ToString();
                    localSource.IPAddress = htDB["DBIP"].ToString();
                    localSource.Port = Convert.ToInt32(htDB["DBPort"].ToString());
                    localSource.ConnName = htDB["DBConnName"].ToString();
                    localSource.DBName = htDB["DBName"].ToString();
                    localSource.DefaultUser = htDB["DBUser"].ToString();
                    localSource.DefaultPWD = htDB["DBPWD"].ToString();
                    localSource.MaxConnNo = Convert.ToInt32(htDB["MaxConnNo"].ToString());
                    localSource.DBDescription = htDB["DBDesc"].ToString();

                    this.dbSoureceList.Add(localSource);
                    htDB.Clear();
                }
                
                
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得db数据源
        /// </summary>
        private void LoadDBSource()
        {
            try
            {
                DBOperator dbOperator = this.GetLocalDBOperator();
                if (dbOperator != null)
                {
                    string sqlStr = "select DBID,DBConnName,DBName,DBType,DBIP,DBPort,DBDefaultUser,DBDefaultPwd,DBMaxConnNo,DBDescription from DBSource";
                    DataSet ds = dbOperator.ExecuteQuerry(sqlStr);
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dt = ds.Tables[0];
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            DataRow row = dt.Rows[i];
                            DBSource dbs = new DBSource();
                            dbs.DBID = Convert.ToString(row["DBID"]);
                            dbs.ConnName = Convert.ToString(row["DBConnName"]);
                            dbs.DBName = Convert.ToString(row["DBName"]);
                            dbs.DBType = Convert.ToString(row["DBType"]);
                            dbs.IPAddress = Convert.ToString(row["DBIP"]);
                            dbs.Port = Convert.ToInt32(row["DBPort"]);
                            dbs.DefaultUser = Convert.ToString(row["DBDefaultUser"]);
                            dbs.DefaultPWD = Convert.ToString(row["DBDefaultPwd"]);
                            dbs.MaxConnNo = Convert.ToInt32(row["DBMaxConnNo"]);
                            dbs.DBDescription = Convert.ToString(row["DBDescription"]);

                            this.dbSoureceList.Add(dbs);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获得db数据源列表
        /// </summary>
        /// <param name="dbSource"></param>
        public void UpdateDBSourceList(DBSource dbSource)
        {
            try
            {
                lock (lockFlag)
                {
                    this.currentDBID = dbSource.DBID;
                    DBSource dbs = this.dbSoureceList.Find(this.IsInDBSourceList);
                    if (dbs == null)
                    {
                        this.dbSoureceList.Add(dbSource);
                    }
                    else
                    {
                        this.dbSoureceList.Remove(dbs);
                        this.dbSoureceList.Add(dbSource);
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
        /// <summary>
        /// 移除数据源
        /// </summary>
        /// <param name="dbSource"></param>
        public void RemoveDBSource(DBSource dbSource)
        {
            try
            {
                lock (lockFlag)
                {
                    this.currentDBID = dbSource.DBID;
                    DBSource dbs = this.dbSoureceList.Find(this.IsInDBSourceList);
                    if (dbs != null)
                    {
                        this.dbSoureceList.Remove(dbs);
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
        /// <summary>
        /// 移除数据源
        /// </summary>
        /// <param name="dbID"></param>
        public void RemoveDBSource(string dbID)
        {
            try
            {
                lock (lockFlag)
                {
                    this.currentDBID = dbID;
                    DBSource dbs = this.dbSoureceList.Find(this.IsInDBSourceList);
                    if (dbs != null)
                    {
                        this.dbSoureceList.Remove(dbs);
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
        /// <summary>
        /// 获得数据源
        /// </summary>
        /// <param name="dbID"></param>
        /// <returns></returns>
        public DBSource GetDBSource(string dbID)
        {
            try
            {

                if (this.dbSoureceList != null)
                {
                    this.currentDBID = dbID;
                    DBSource dbSource = this.dbSoureceList.Find(this.IsInDBSourceList);
                    return dbSource;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 是否在数据列表里
        /// </summary>
        /// <param name="dbSource"></param>
        /// <returns></returns>
        private bool IsInDBSourceList(DBSource dbSource)
        {
            try
            {
                if (dbSource.DBID == this.currentDBID)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


    }
}
