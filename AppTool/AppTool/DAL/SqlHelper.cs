//----------------------------------------------------------------------------
// Copyright (C) 2014, 刘宇鉴. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Model;
using System.Data;
using System.Collections;
using System.Linq.Expressions;
//using System.Web.Script.Serialization;

namespace DAL
{
    public static class SqlHelper
    {
        #region 初始化静态数值
        private static string[] arrTextField = new string[] { "char", "varchar", "text", "nvarchar", "datetime", "string" };
        private static string[] arrNonTextField = new string[] { "int" };

        /// <summary>
        /// 错误级别枚举值
        /// </summary>
        private enum LogLevel
        {
            // DEBUG
            DEBUG = 1,
            // 信息
            INFO = 5,
            // 错误
            ERROR = 9
        }

         #endregion

        #region 根据Model自动生成各类SQL语句并执行
        /// <summary>
        /// 根据Model生成插入SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean InsertByModel(object entity, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                if (GetInsertSql(entity, out strOutstr) == false)
                {
                    return false;
                }

                //log.SetP("GetInsertSql生成语句:" + strOutstr.Replace("'", "''"), 1);
                int iResult = dbAccess.ExecuteNonQuerry(strOutstr);

                if (iResult <= 0)
                {
                    strOutstr = "[HLPI01]未插入任何记录";
                    throw new ACMSCustomException("HLPI01");
                    //log.Set("HLPI01", "未插入任何记录");
                    //return false;
                }
                else
                {
                    strOutstr = iResult.ToString();
                    return true;
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                string strTest = e.Message;
                //log.Set("HLPI02", strTest);
                throw new ACMSCustomException("HLPI02");
            }
        }

        /// <summary>
        /// 根据Model生成【批量】插入SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean InsertByModel<T>(List<T> entity, out string strOutstr)
        {
            try
            {
                
                if (entity == null || entity.Count == 0)
                {
                    strOutstr = "要插入的列表为空";
                    return false;
                }
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                string strTmp = string.Empty;
                ArrayList arrOutStr = new ArrayList();

                //遍历entity并将返回的sql记入ArrayList
                for (int i = 0; i < entity.Count; i++)
                {
                    if (GetInsertSql(entity[i], out strTmp) == false)
                    {
                        return false;
                    }
                    arrOutStr.Add(strTmp);
                    //log.SetP("GetInsertSql生成语句:" + strTmp.Replace("'", "''"), 1);
                }

                string[] strSqlstr = (string[])arrOutStr.ToArray(typeof(string));
                string[] strResult = dbAccess.ExecuteNonQuerry(strSqlstr);

                int count = 0;
                int outnum = 0;

                for (int i = 0; i < strResult.Length; i++)
                {
                    if (int.TryParse(strResult[i], out outnum))
                    {
                        if (strResult[i] == "1" || Convert.ToInt16(strResult[i]) > 1)
                        {
                            count++;
                        }
                        else
                        {
                            //log.SetP(strResult[i].Replace("'", "''"), 9);
                        }
                    }
                    else
                    {
                        //log.SetP(strResult[i].Replace("'", "''"), 9);
                    }
                }

                if (strResult.Length == count && count != 0)
                {
                    //全部执行成功
                    strOutstr = "插入" + count + "条记录成功";
                    //log.Set(strOutstr);
                    return true;
                }
                else if (count > 0)
                {
                    //部分执行成功
                    //throw new ACMSCustomException("HLPI03");
                    //strOutstr = "[HLPI03]插入数据部分失败";
                    //log.Set("HLPI03", "插入" + strResult.Length + "条数据,失败" + (strResult.Length - count) + "条");
                    return true;
                }
                else
                {
                    //未执行成功
                    throw new ACMSCustomException("HLPI04");
                    //strOutstr = "[HLPI04]插入数据失败";
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ACMSCustomException("HLPI05");
                //string strTest = e.Message;
                //log.Set("HLPI05", strTest);
                //throw;
            }
        }

        /// <summary>
        /// 根据Model生成更新SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean UpdateByModel(object entity, string strWhereParam, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                if (GetUpdateSql(entity, strWhereParam, out strOutstr) == false)
                {
                    return false;
                }

                LogHelper.SetP("GetUpdateSql生成语句:" + strOutstr.Replace("'", "''"), (int)LogLevel.DEBUG);
                int iResult = dbAccess.ExecuteNonQuerry(strOutstr);

                if (iResult <= 0)
                {
                    LogHelper.SetP("[HLPU01]未更新任何记录", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPU01");
                }
                else
                {
                    strOutstr = iResult.ToString();
                    return true;
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPU02]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPU02");
            }
        }

        /// <summary>
        /// 根据Model生成【批量】更新SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean UpdateByModel<T>(List<T> entity, string[] strWhereParam, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                string strTmp = string.Empty;
                ArrayList arrOutStr = new ArrayList();

                //遍历entity并将返回的sql记入ArrayList
                for (int i = 0; i < entity.Count; i++)
                {
                    if (GetUpdateSql(entity[i], strWhereParam[i], out strTmp) == false)
                    {
                        return false;
                    }
                    arrOutStr.Add(strTmp);
                    LogHelper.SetP("GetUpdateSql生成语句:" + strTmp.Replace("'", "''"), (int)LogLevel.DEBUG);
                }

                string[] strSql = (string[])arrOutStr.ToArray(typeof(string));
                string[] strResult = dbAccess.ExecuteNonQuerry(strSql);

                int count = 0;
                int outnum = 0;

                for (int i = 0; i < strResult.Length; i++)
                {
                    if (int.TryParse(strResult[i], out outnum))
                    {
                        if (strResult[i] == "1" || Convert.ToInt16(strResult[i]) > 1)
                        {
                            count++;
                        }
                        else
                        {
                            LogHelper.SetP(strResult[i].Replace("'", "''"), (int)LogLevel.ERROR);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strResult[i]))
                        {
                            LogHelper.SetP(strResult[i].Replace("'", "''"), (int)LogLevel.ERROR);
                        }
                    }
                }

                if (strResult.Length == count && count != 0)
                {
                    //全部执行成功
                    strOutstr = "更新" + count + "条记录成功";
                    //log.Set(strOutstr);
                    return true;
                }
                else if (count > 0)
                {
                    //部分执行成功
                    LogHelper.SetP("[HLPU03]更新数据部分失败", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPU03");
                }
                else
                {
                    //未执行成功
                    LogHelper.SetP("[HLPU04]更新数据失败", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPU04");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPU05]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPU05");
            }
        }

        /// <summary>
        /// 根据Model生成更新SQL语句并执行(根据Model生成Where条件)
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean UpdateByModel(object entity, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                if (GetUpdateSql(entity, out strOutstr) == false)
                {
                    return false;
                }

                LogHelper.SetP("GetUpdateSql生成语句(自动生成Where条件):" + strOutstr.Replace("'", "''"), (int)LogLevel.DEBUG);
                int iResult = dbAccess.ExecuteNonQuerry(strOutstr);

                if (iResult <= 0)
                {
                    LogHelper.SetP("[HLPU01]未修改任何记录(自动生成Where条件)", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPU01");
                }
                else
                {
                    strOutstr = iResult.ToString();
                    return true;
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPU02]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPU02");
            }
        }

        /// <summary>
        /// 根据Model生成【批量】更新SQL语句并执行(根据Model生成Where条件)
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean UpdateByModel<T>(List<T> entity, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                string strTmp = string.Empty;
                ArrayList arrOutStr = new ArrayList();

                //遍历entity并将返回的sql记入ArrayList
                for (int i = 0; i < entity.Count; i++)
                {
                    if (GetUpdateSql(entity[i], out strTmp) == false)
                    {
                        return false;
                    }
                    arrOutStr.Add(strTmp);
                    LogHelper.SetP("GetUpdateSql生成语句(自动生成Where条件):" + strTmp.Replace("'", "''"), (int)LogLevel.DEBUG);
                }

                string[] strSql = (string[])arrOutStr.ToArray(typeof(string));
                string[] strResult = dbAccess.ExecuteNonQuerry(strSql);

                int count = 0;
                int outnum = 0;

                for (int i = 0; i < strResult.Length; i++)
                {
                    if (int.TryParse(strResult[i], out outnum))
                    {
                        if (strResult[i] == "1" || Convert.ToInt16(strResult[i]) > 1)
                        {
                            count++;
                        }
                        else
                        {
                            LogHelper.SetP(strResult[i].Replace("'", "''"), (int)LogLevel.ERROR);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strResult[i]))
                        {
                            LogHelper.SetP(strResult[i].Replace("'", "''"), (int)LogLevel.ERROR);
                        }
                    }
                }

                if (strResult.Length == count && count != 0)
                {
                    //全部执行成功
                    strOutstr = "更新" + count + "条记录成功";
                    //log.Set(strOutstr);
                    return true;
                }
                else if (count > 0)
                {
                    //部分执行成功
                    LogHelper.SetP("[HLPU03]更新数据部分失败", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPU03");
                }
                else
                {
                    //未执行成功
                    LogHelper.SetP("[HLPU04]更新数据失败", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPU04");

                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPU05]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPU05");
            }
        }

        /// <summary>
        /// 根据Model和Where条件生成删除SQL语句并执行
        /// </summary>
        /// <param name="entity">Model</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean DeleteByModel(object entity, string strWhereParam, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                if (GetDeleteSql(entity, strWhereParam, out strOutstr) == false)
                {
                    return false;
                }

                LogHelper.SetP("GetDeleteSql生成语句:" + strOutstr.Replace("'", "''"), (int)LogLevel.DEBUG);
                int iResult = dbAccess.ExecuteNonQuerry(strOutstr);

                if (iResult < 0)
                {
                    LogHelper.SetP("[HLPD01]未删除任何记录", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPD01");
                }
                else
                {
                    strOutstr = iResult.ToString();
                    return true;
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPD02]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPD02");
            }
        }

        /// <summary>
        /// 根据Model生成删除SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean DeleteByModel(object entity, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                if (GetDeleteSql(entity, out strOutstr) == false)
                {
                    return false;
                }

                LogHelper.SetP("GetDeleteSql生成语句:" + strOutstr.Replace("'", "''"), (int)LogLevel.DEBUG);
                int iResult = dbAccess.ExecuteNonQuerry(strOutstr);

                if (iResult < 0)
                {
                    LogHelper.SetP("[HLPD01]未删除任何记录", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPD01");
                }
                else
                {
                    strOutstr = iResult.ToString();
                    return true;
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPD02]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPD02");
            }
        }

        /// <summary>
        /// 根据Model生成【批量】删除SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean DeleteByModel<T>(List<T> entity, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                string strTmp = string.Empty;
                ArrayList arrOutStr = new ArrayList();

                //遍历entity并将返回的sql记入ArrayList
                for (int i = 0; i < entity.Count; i++)
                {
                    if (GetDeleteSql(entity[i], out strTmp) == false)
                    {
                        return false;
                    }
                    arrOutStr.Add(strTmp);
                    LogHelper.SetP("GetDeleteSql生成语句:" + strTmp.Replace("'", "''"), (int)LogLevel.DEBUG);
                }

                string[] strSqlstr = (string[])arrOutStr.ToArray(typeof(string));
                string[] strResult = dbAccess.ExecuteNonQuerry(strSqlstr);

                int count = 0;
                int outnum = 0;

                for (int i = 0; i < strResult.Length; i++)
                {
                    if (int.TryParse(strResult[i], out outnum))
                    {
                        if (Convert.ToInt16(strResult[i]) >= 0)
                        {
                            count++;
                        }
                        else
                        {
                            LogHelper.SetP(strResult[i].Replace("'", "''"), (int)LogLevel.ERROR);
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(strResult[i]))
                        {
                            LogHelper.SetP(strResult[i].Replace("'", "''"), (int)LogLevel.ERROR);
                        }
                    }
                }

                if (strResult.Length == count && count != 0)
                {
                    //全部执行成功
                    strOutstr = "删除" + count + "条记录成功";
                    //log.Set(strOutstr);
                    return true;
                }
                else if (count > 0)
                {
                    //部分执行成功
                    LogHelper.SetP("[HLPD03]删除数据部分失败", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPD03");
                }
                else
                {
                    //未执行成功
                    LogHelper.SetP("[HLPD04]删除数据失败", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPD04");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPD05]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPD05");
            }
        }

        /// <summary>
        /// 根据Model和Where条件查询符合条件的条数
        /// </summary>
        /// <param name="entity">Model</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="strOutstr">返回的结果</param>
        /// <returns></returns>
        public static Boolean QueryCountByModel(object entity, string strWhereParam, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                if (GetCountAllSql(entity, strWhereParam, out strOutstr) == false)
                {
                    return false;
                }

                LogHelper.SetP("GetCountAllSql生成语句:" + strOutstr.Replace("'", "''"), (int)LogLevel.DEBUG);
                strOutstr = dbAccess.ExecuteScalar(strOutstr).ToString();

                return true;
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPS02]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPS02");
            }
        }



        /// <summary>
        /// 根据Model生成查询SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModel(object entity, string strWhereParam, out DataSet ds, out string strOutstr)
        {
            try
            {
                return QueryByModel(entity, strWhereParam, true, out ds, out strOutstr);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Model生成查询SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModel(object entity, string strWhereParam, Boolean bDBName, out DataSet ds, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                ds = new DataSet();

                if (GetSelectSql(entity, strWhereParam, bDBName, out strOutstr) == false)
                {
                    return false;
                }

                LogHelper.SetP("GetSelectSql生成语句:" + strOutstr.Replace("'", "''"), (int)LogLevel.DEBUG);
                ds = dbAccess.ExecuteQuerry(strOutstr);

                strOutstr = "查询成功";
                return true;
            }
            catch (ACMSCustomException e)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPS01]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPS01");
            }
        }

        /// <summary>
        /// 根据Model生成查询SQL语句和Where条件并执行
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModelWithNoParam(object entity, out DataSet ds, out string strOutstr)
        {
            try
            {
                return QueryByModelWithNoParam(entity, true, out ds, out strOutstr);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Model生成查询SQL语句和Where条件并执行
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModelWithNoParam(object entity, Boolean bDBName, out DataSet ds, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                ds = new DataSet();

                if (GetSelectSqlWithNoParam(entity, bDBName, out strOutstr) == false)
                {
                    return false;
                }

                LogHelper.SetP("GetSelectSqlWithNoParam生成语句:" + strOutstr.Replace("'", "''"), (int)LogLevel.DEBUG);
                ds = dbAccess.ExecuteQuerry(strOutstr);

                strOutstr = "查询成功";
                return true;
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPS01]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPS01");
            }
        }

        /// <summary>
        /// 根据Model生成查询SQL语句和Where条件并执行(只按主键生成查询条件)
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModelWithPK(object entity, out DataSet ds, out string strOutstr)
        {
            try
            {
                return QueryByModelWithPK(entity, true, out ds, out strOutstr);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Model生成查询SQL语句和Where条件并执行(只按主键生成查询条件)
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModelWithPK(object entity, Boolean bDBName, out DataSet ds, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                ds = new DataSet();

                if (GetSelectSqlWithNoParam(entity, bDBName, true, out strOutstr) == false)
                {
                    return false;
                }

                LogHelper.SetP("GetSelectSqlWithNoParam生成语句:" + strOutstr.Replace("'", "''"), (int)LogLevel.DEBUG);
                ds = dbAccess.ExecuteQuerry(strOutstr);

                strOutstr = "查询成功";
                return true;
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPS01]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPS01");
            }
        }

        /// <summary>
        /// 根据Model生成【批量】查询SQL语句和Where条件并执行
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModelWithNoParam<T>(List<T> entity, out DataSet ds, out string strOutstr)
        {
            try
            {
                return QueryByModelWithNoParam<T>(entity, true, out ds, out strOutstr);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Model生成【批量】查询SQL语句和Where条件并执行
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModelWithNoParam<T>(List<T> entity, Boolean bDBName, out DataSet ds, out string strOutstr)
        {
            try
            {
                return QueryByModelWithNoParam<T>(entity, true, false, out ds, out strOutstr);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Model生成【批量】查询SQL语句和Where条件并执行
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="bParseTableName">返回的结果ds中是否以数据库中表名作为Table名（false时使用默认的t1,t2...）</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModelWithNoParam<T>(List<T> entity, Boolean bDBName, Boolean bParseTableName, out DataSet ds, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                string strTmp = string.Empty;
                string strTableName = string.Empty;
                ds = new DataSet();
                ArrayList arrOutStr = new ArrayList();

                //遍历entity并将返回的sql记入ArrayList
                for (int i = 0; i < entity.Count; i++)
                {
                    if (GetSelectSqlWithNoParam(entity[i], bDBName, out strTmp) == false)
                    {
                        return false;
                    }
                    arrOutStr.Add(strTmp);
                    LogHelper.SetP("GetSelectSqlWithNoParam生成语句:" + strTmp.Replace("'", "''"), (int)LogLevel.DEBUG);
                }

                string[] strSql = (string[])arrOutStr.ToArray(typeof(string));
                ds = dbAccess.ExecuteQuerry(strSql);

                //根据entity所属数据库表名更改返回的DataSet中DataTable名
                if (bParseTableName)
                {
                    for (int i = 0; i < ds.Tables.Count; i++)
                    {
                        strTableName = string.Empty;
                        if (GetTableName(entity[i], out strTableName) == true)
                        {
                            if (!ds.Tables.Contains(strTableName))
                            {
                                //若Tablename不重名则改名
                                ds.Tables[i].TableName = strTableName;
                            }
                        }
                    }
                }
                strOutstr = "查询成功";
                return true;
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPS01]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPS01");
            }
        }

        /// <summary>
        /// 根据Model生成【批量】查询SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModel<T>(List<T> entity, string[] strWhereParam, out DataSet ds, out string strOutstr)
        {
            try
            {
                return QueryByModel<T>(entity, strWhereParam, true, out ds, out strOutstr);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Model生成【批量】查询SQL语句并执行
        /// </summary>
        /// <param name="entity">Model对象List</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="ds">返回的DataSet</param>
        /// <param name="strOutstr">失败信息</param>
        /// <returns>是否成功</returns>
        public static Boolean QueryByModel<T>(List<T> entity, string[] strWhereParam, Boolean bDBName, out DataSet ds, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                string strTmp = string.Empty;
                ds = new DataSet();
                ArrayList arrOutStr = new ArrayList();

                //遍历entity并将返回的sql记入ArrayList
                for (int i = 0; i < entity.Count; i++)
                {
                    if (GetSelectSql(entity[i], strWhereParam[i], bDBName, out strTmp) == false)
                    {
                        return false;
                    }
                    arrOutStr.Add(strTmp);
                    LogHelper.SetP("GetSelecteSql生成语句:" + strTmp.Replace("'", "''"), (int)LogLevel.DEBUG);
                }

                string[] strSql = (string[])arrOutStr.ToArray(typeof(string));
                ds = dbAccess.ExecuteQuerry(strSql);

                strOutstr = "查询成功";
                return true;
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPS01]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPS01");
            }
        }


        #endregion 

        #region 根据Model自动生成各类SQL语句

        /// <summary>
        /// 根据Model生成Insert语句
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetInsertSql(object entity, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                //用于拼接sql语句的字段名和字段值
                StringBuilder strFields = new StringBuilder();
                StringBuilder strValues = new StringBuilder();

                //获取模型对应的属性
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;


                string strInsertSQL = "INSERT INTO {0}({1}) VALUES({2})";
                string strTablename = string.Empty;
                string strFieldName = string.Empty;
                string strFieldType = string.Empty;
                object objFieldValue = null;

                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }

                int i = 0;
                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;
                        if (FieldAttr != null)
                        {
                            if (FieldAttr is DataFieldAttribute)
                            {
                                strFieldName = FieldAttr.Fieldname;
                                strFieldType = FieldAttr.Fieldtype;
                                objFieldValue = prop.GetValue(entity, null);

                                //判断值是否为空
                                if (objFieldValue != null && objFieldValue.ToString() != "")
                                {
                                    if (i > 0)
                                    {
                                        strFields.Append(",");
                                        strValues.Append(",");
                                    }

                                    strFields.Append(strFieldName);
                                    if (arrTextField.Contains(strFieldType))
                                    {
                                        //为文本字段增加前后引号
                                        strValues.Append("\'" + objFieldValue.ToString().Replace("'", "''") + "\'");
                                    }
                                    else
                                    {
                                        strValues.Append(objFieldValue);
                                    }

                                    i++;
                                }
                                else if (FieldAttr.Isrequired == true)
                                {
                                    //当值为空时判断该字段是否为必填字段
                                    //log.SetP("[HLPG01]必要的字段为空值" , 9);
                                    throw new ACMSCustomException("HLPG01");

                                }
                            }
                        }
                    }
                }

                if (strFields.ToString().Trim() != "" && strValues.ToString().Trim() != "")
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strTablename, strFields.ToString(), strValues.ToString() };
                    strOutSql = string.Format(strInsertSQL, Args);
                    return true;
                }
                else
                {
                    //log.SetP("[HLPG02]构造Insert语句失败，字段字符串为空。", 9);
                    throw new ACMSCustomException("HLPG02");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                //log.SetP("[HLPG09]" + e.Message, 9);
                throw new ACMSCustomException("HLPG09");
            }
        }

        /// <summary>
        /// 根据Model生成Update语句
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strWhereParam">where查询条件</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetUpdateSql(object entity, string strWhereParam, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                //用于拼接sql语句的字段名和字段值
                StringBuilder strFields = new StringBuilder();

                //获取模型对应的属性
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;


                string strUpdateSQL = "UPDATE {0} SET {1} WHERE {2}";
                string strTablename = string.Empty;
                string strFieldName = string.Empty;
                string strFieldType = string.Empty;
                object objFieldValue = null;

                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }

                int i = 0;
                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;
                        if (FieldAttr != null)
                        {
                            if (FieldAttr is DataFieldAttribute)
                            {
                                strFieldName = FieldAttr.Fieldname;
                                strFieldType = FieldAttr.Fieldtype;
                                objFieldValue = prop.GetValue(entity, null);

                                //判断值是否为空
                                if (objFieldValue != null && objFieldValue.ToString() != "")
                                {
                                    if (i > 0)
                                    {
                                        strFields.Append(",");
                                    }

                                    strFields.Append(strFieldName + "=");
                                    if (arrTextField.Contains(strFieldType))
                                    {
                                        //为文本字段增加前后引号
                                        strFields.Append("\'" + objFieldValue.ToString().Replace("'", "''") + "\'");
                                    }
                                    else
                                    {
                                        strFields.Append(objFieldValue);
                                    }

                                    i++;
                                }
                                else if (FieldAttr.Isrequired == true)
                                {
                                    //当值为空时判断该字段是否为必填字段
                                    LogHelper.SetP("[HLPG03]必要的字段为空值", (int)LogLevel.ERROR);
                                    throw new ACMSCustomException("HLPG03");
                                }
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(strWhereParam))
                {
                    strWhereParam = "1=2";
                }

                if (strFields.ToString().Trim() != "")
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strTablename, strFields.ToString(), strWhereParam };
                    strOutSql = string.Format(strUpdateSQL, Args);
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG04]构造Update语句失败，字段字符串为空。", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG04");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG10]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG10");
            }
        }

        /// <summary>
        /// 根据Model生成Update语句(根据Model生成Where条件)
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetUpdateSql(object entity, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                //用于拼接sql语句的字段名和字段值
                StringBuilder strFields = new StringBuilder();
                StringBuilder strWhereParam = new StringBuilder();

                //获取模型对应的属性
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;


                string strUpdateSQL = "UPDATE {0} SET {1} WHERE {2}";
                string strTablename = string.Empty;
                string strFieldName = string.Empty;
                string strFieldType = string.Empty;
                object objFieldValue = null;

                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }

                int i = 0;
                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;
                        if (FieldAttr != null)
                        {
                            if (FieldAttr is DataFieldAttribute)
                            {
                                strFieldName = FieldAttr.Fieldname;
                                strFieldType = FieldAttr.Fieldtype;
                                objFieldValue = prop.GetValue(entity, null);

                                //判断值是否为空
                                if (objFieldValue != null && objFieldValue.ToString() != "")
                                {
                                    if (i > 0)
                                    {
                                        strFields.Append(",");
                                        if (FieldAttr.Isrequired == true) strWhereParam.Append(" and ");
                                    }

                                    strFields.Append(strFieldName + "=");
                                    if (arrTextField.Contains(strFieldType))
                                    {
                                        //为文本字段增加前后引号
                                        strFields.Append("\'" + objFieldValue.ToString().Replace("'", "''") + "\'");
                                        if (FieldAttr.Isrequired == true) strWhereParam.Append(strFieldName + " = \'" + objFieldValue + "\'");
                                    }
                                    else
                                    {
                                        strFields.Append(objFieldValue);
                                        if (FieldAttr.Isrequired == true) strWhereParam.Append(strFieldName + " = " + objFieldValue + "");
                                    }

                                    i++;
                                }
                                else if (FieldAttr.Isrequired == true)
                                {
                                    //当值为空时判断该字段是否为必填字段
                                    LogHelper.SetP("[HLPG03]必要的字段为空值（自动生成Where条件）", (int)LogLevel.ERROR);
                                    throw new ACMSCustomException("HLPG03");
                                }
                            }
                        }
                    }
                }

                if (strFields.ToString().Trim() != "" && strWhereParam.ToString() != "")
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strTablename, strFields.ToString(), strWhereParam.ToString() };
                    strOutSql = string.Format(strUpdateSQL, Args);
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG04]构造Update语句失败，字段字符串为空。（自动生成Where条件）", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG04");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG10]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG10");
            }
        }

        /// <summary>
        /// 根据Model生成Delete语句
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetDeleteSql(object entity, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                //用于拼接sql语句的字段名和字段值
                StringBuilder strWhereParam = new StringBuilder();

                //获取模型对应的属性
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;


                string strDeleteSQL = "DELETE FROM {0} WHERE {1}";
                string strTablename = string.Empty;
                string strFieldName = string.Empty;
                string strFieldType = string.Empty;
                object objFieldValue = null;

                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }

                int i = 0;
                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;
                        if (FieldAttr != null)
                        {
                            if (FieldAttr is DataFieldAttribute)
                            {
                                strFieldName = FieldAttr.Fieldname;
                                strFieldType = FieldAttr.Fieldtype;
                                objFieldValue = prop.GetValue(entity, null);

                                if (FieldAttr.Isrequired == true)
                                {
                                    //判断值是否为空
                                    if (objFieldValue != null && objFieldValue.ToString() != "")
                                    {
                                        if (i > 0)
                                        {
                                            strWhereParam.Append(" and ");
                                        }

                                        if (arrTextField.Contains(strFieldType))
                                        {
                                            //为文本字段增加前后引号
                                            strWhereParam.Append(strFieldName + " = \'" + objFieldValue.ToString().Replace("'", "''") + "\'");
                                        }
                                        else
                                        {
                                            strWhereParam.Append(strFieldName + " = " + objFieldValue + "");
                                        }

                                        i++;
                                    }
                                    else
                                    {
                                        LogHelper.SetP("[HLPG17]必要的字段为空值", (int)LogLevel.ERROR);
                                        throw new ACMSCustomException("HLPG17");
                                    }
                                }
                            }
                        }
                    }
                }


                if (strWhereParam.ToString().Trim() != "")
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strTablename, strWhereParam.ToString() };
                    strOutSql = string.Format(strDeleteSQL, Args);
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG18]构造Delete语句失败，字段字符串为空。", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG18");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG11]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG11");
            }
        }

        /// <summary>
        /// 根据Model和Where条件生成Delete语句
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetDeleteSql(object entity, string strWhereParam, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                string strDeleteSQL = "DELETE FROM {0} WHERE {1}";
                string strTablename = string.Empty;


                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }

                if (strWhereParam.ToString().Trim() != "")
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strTablename, strWhereParam };
                    strOutSql = string.Format(strDeleteSQL, Args);
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG18]构造Delete语句失败，字段字符串为空。", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG18");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG11]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG11");
            }
        }


        /// <summary>
        /// 根据Model和Where条件构造查询满足条件条数的语句
        /// </summary>
        /// <param name="entity">Model</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="strOutSql">返回的sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetCountAllSql(object entity, string strWhereParam, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                string strDeleteSQL = "SELECT COUNT(*) FROM {0} WHERE {1}";
                string strTablename = string.Empty;


                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }

                if (strWhereParam.ToString().Trim() != "")
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strTablename, strWhereParam };
                    strOutSql = string.Format(strDeleteSQL, Args);
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG12]构造Select Count语句失败，条件为空。", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG12");
                    
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG13]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG13");
            }
        }



        /// <summary>
        /// 根据Model生成Select语句
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strWhereParam">where查询条件</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetSelectSql(object entity, string strWhereParam, out string strOutSql)
        {
            try
            {
                return GetSelectSql(entity, strWhereParam, false, out strOutSql);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Model生成Select语句
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="strWhereParam">where查询条件</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetSelectSql(object entity,string strWhereParam,Boolean bDBName, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                //用于拼接sql语句的字段名和字段值
                StringBuilder strFields = new StringBuilder();

                //获取模型对应的属性
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;

                string strSelectSQL = "SELECT {0} FROM {1} WHERE {2}";
                string strTablename = string.Empty;
                string strFieldName = string.Empty;

                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }

                int i = 0;
                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;
                        if (FieldAttr != null)
                        {
                            if (FieldAttr is DataFieldAttribute)
                            {
                                strFieldName = FieldAttr.Fieldname;

                                if (i > 0)
                                {
                                    strFields.Append(",");
                                }

                                if (bDBName)
                                {
                                    //使用数据库中字段名
                                    strFields.Append(FieldAttr.Fieldname);
                                }
                                else
                                {
                                    //使用封装的字段名
                                    strFields.Append(FieldAttr.Fieldname + " AS " + prop.Name);
                                }

                                i++;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(strWhereParam))
                {
                    strWhereParam = "1=2";
                }

                if (strFields.ToString().Trim() != "")
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strFields.ToString(), strTablename, strWhereParam };
                    strOutSql = string.Format(strSelectSQL, Args);
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG05]构造Select语句失败，字段字符串为空。", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG05");

                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG16]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG16");
            }
        }

        /// <summary>
        /// 根据Model生成Select语句和Where条件
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="bPKonly">生成的Where条件中是否是按主键生成</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetSelectSqlWithNoParam(object entity, Boolean bDBName, out string strOutSql)
        {
            try
            {
                return GetSelectSqlWithNoParam(entity, bDBName, false, out strOutSql);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Model生成Select语句和Where条件
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="bPKonly">生成的Where条件中是否是按主键生成</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetSelectSqlWithNoParam(object entity, Boolean bDBName,Boolean bPKonly, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                //用于拼接sql语句的字段名和字段值
                StringBuilder strFields = new StringBuilder();
                StringBuilder strWhereParam = new StringBuilder();

                //获取模型对应的属性
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;

                string strSelectSQL = "SELECT {0} FROM {1} WHERE {2}";
                string strTablename = string.Empty;
                string strFieldName = string.Empty;
                string strFieldType = string.Empty;
                object objFieldValue = null;

                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }

                int i = 0;
                int j = 0;
                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;

                        if (FieldAttr != null)
                        {
                            if (FieldAttr is DataFieldAttribute)
                            {
                                strFieldName = FieldAttr.Fieldname;
                                strFieldType = FieldAttr.Fieldtype;
                                objFieldValue = prop.GetValue(entity, null);

                                if (i > 0)
                                {
                                    strFields.Append(",");
                                }

                                if (bDBName)
                                {
                                    //使用数据库中字段名
                                    strFields.Append(FieldAttr.Fieldname);
                                }
                                else
                                {
                                    //使用封装的字段名
                                    strFields.Append(FieldAttr.Fieldname + " AS " + prop.Name);
                                }

                                i++;

                                //生成Where条件
                                if (bPKonly)
                                {
                                    //只按照主键生成Where语句
                                    if (FieldAttr.Isrequired == true)
                                    {
                                        if (objFieldValue != null && objFieldValue.ToString() != "")
                                        {
                                            if (j > 0)
                                            {
                                                strWhereParam.Append(" and ");
                                            }

                                            if (arrTextField.Contains(strFieldType))
                                            {
                                                //为文本字段增加前后引号
                                                strWhereParam.Append(strFieldName + " = \'" + objFieldValue.ToString().Replace("'", "''") + "\'");
                                            }
                                            else
                                            {
                                                strWhereParam.Append(strFieldName + " = " + objFieldValue + "");
                                            }

                                            j++;
                                        }
                                    }
                                }
                                else if (objFieldValue != null && objFieldValue.ToString() != "")
                                {
                                    if (j > 0)
                                    {
                                        strWhereParam.Append(" and ");
                                    }

                                    if (arrTextField.Contains(strFieldType))
                                    {
                                        //为文本字段增加前后引号
                                        strWhereParam.Append(strFieldName + " = \'" + objFieldValue.ToString().Replace("'", "''") + "\'");
                                    }
                                    else
                                    {
                                        strWhereParam.Append(strFieldName + " = " + objFieldValue + "");
                                    }

                                    j++;
                                }
                            }
                        }
                    }
                }

                if (strFields.ToString().Trim() != "" && strWhereParam.ToString().Trim() != "")
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strFields.ToString(), strTablename, strWhereParam.ToString() };
                    strOutSql = string.Format(strSelectSQL, Args);
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG05]构造Select语句失败，字段字符串为空。", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG05");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG16]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG16");
            }
        }

        #endregion


        #region 公共方法

        /// <summary>
        /// 根据查询范围获取本次查询页面条数
        /// </summary>
        /// <param name="nRange">查询范围</param>
        /// <param name="nLimit">设置的页面条数</param>
        /// <param name="nTotalCount">符合条件的总条数</param>
        /// <param name="strOutstr">本次查询页面条数</param>
        /// <returns>是否成功</returns>
        public static Boolean GetCurrentLimit(int nRange, int nLimit, int nTotalCount, out string strOutstr)
        {
            try
            {
                strOutstr = string.Empty;

                //确定查询范围
                int nCurrentLimit;

                if (nTotalCount < nRange)
                {
                    //最后一页limit少于正常情况
                    int nDiff = nRange - nTotalCount;
                    if (nDiff >= nLimit)
                    {
                        strOutstr = "查询参数错误";
                        return false;
                    }
                    nCurrentLimit = nLimit - nDiff;
                }
                else
                {
                    nCurrentLimit = nLimit;
                }

                strOutstr = nCurrentLimit.ToString();
                return true;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG19]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG19");
            }
        }

        /// <summary>
        /// 获取entity对应数据库表名
        /// </summary>
        /// <param name="entity">entity实例</param>
        /// <returns>是否成功</returns>
        public static Boolean GetTableName(object entity, out string strOut)
        {
            try
            {
                return GetTableName(entity.GetType(), out strOut);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据类型返回数据库表名
        /// </summary>
        /// <param name="objType">类型</param>
        /// <returns>是否成功</returns>
        public static Boolean GetTableName(Type objType,out string strOut)
        {
            try
            {
                DataObjectAttribute attr = objType.GetCustomAttributes(typeof(DataObjectAttribute), false)[0] as DataObjectAttribute;
                if (attr != null)
                {
                    if (attr.Tablename == "")
                    {
                        LogHelper.SetP("[HLPG06]" + objType.ToString() + "TableName为空", (int)LogLevel.ERROR);
                        throw new ACMSCustomException("HLPG06"); 
                    }
                    else
                    {
                        strOut = attr.Tablename;
                        return true;
                    }
                }
                else
                {
                    LogHelper.SetP("[HLPG07]" + objType.ToString() + "没有设置TableName属性", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG07"); 
                }
            }
            catch(Exception e)
            {
                LogHelper.SetP("[HLPG08]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG08"); 
            }
        }

        /// <summary>
        /// 根据Model和封装字段名获取数据库对应字段名
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="keyValue">封装字段名</param>
        /// <returns>数据库字段名</returns>
        /// <example>
        /// string strFieldName = SqlHelper.GetFieldName(entity, "Remark");
        /// </example>
        public static string GetFieldName(object entity, object keyValue)
        {
            try
            {
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;
                string strFieldName = string.Empty;

                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;

                        if (keyValue.ToString() == prop.Name.ToString())
                        {
                            if (FieldAttr != null)
                            {
                                if (FieldAttr is DataFieldAttribute)
                                {
                                    return FieldAttr.Fieldname;
                                }
                            }
                            return "";
                        }

                    }
                }

                return "";
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPC01]" + e.Message, (int)LogLevel.ERROR);
                return "";
            }
        }

        /// <summary>
        /// 根据Model和数据库字段名获取Model封装的字段名
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="keyValue">数据库字段名</param>
        /// <returns>封装字段名</returns>
        /// <example>
        /// string strFieldName = SqlHelper.GetFieldNameByDBName(entity, "mnt_remarks");
        /// </example>
        public static string GetFieldNameByDBName(object entity, object keyValue)
        {
            try
            {
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;
                string strFieldName = string.Empty;

                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;

                        if (FieldAttr != null)
                        {
                            if (FieldAttr is DataFieldAttribute)
                            {
                                if (keyValue.ToString() == FieldAttr.Fieldname)
                                {
                                    return prop.Name.ToString();
                                }
                            }
                        }
                    }
                }

                return "";
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPC02]" + e.Message, (int)LogLevel.ERROR);
                return "";
            }
        }

        /// <summary>
        /// 根据Model和封装字段名获取数据库对应字段名
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="propertyLambda">封装字段表达式</param>
        /// <returns>数据库字段名</returns>
        /// <example>
        /// string strFieldName = SqlHelper.GetFieldName(entity, () => entity.Remark);
        /// </example>
        public static string GetFieldName<T>(object entity, Expression<Func<T>> propertyLambda)
        {
            try
            {
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;
                string strFieldName = string.Empty;

                var me = propertyLambda.Body as MemberExpression;
                string strKeyValue = me.Member.Name;

                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;

                        if (strKeyValue == prop.Name.ToString())
                        {
                            if (FieldAttr != null)
                            {
                                if (FieldAttr is DataFieldAttribute)
                                {
                                    return FieldAttr.Fieldname;
                                }
                            }
                            return "";
                        }

                    }
                }

                return "";
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPC03]" + e.Message, (int)LogLevel.ERROR);
                return "";
            }
        }

        /// <summary>
        /// 根据Model.Name反查Name
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="propertyLambda"></param>
        /// <returns>字段名称</returns>
        /// <example>
        /// object keyValue = SqlHelper.GetPropertyName(() => entity.Rowno);
        /// </example>
        public static string GetPropertyName<T>(Expression<Func<T>> propertyLambda)
        {
            var me = propertyLambda.Body as MemberExpression;
            return me.Member.Name;
        }

        /// <summary>
        /// 根据DataTable转换回Model
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns>IList</returns>
        public static IList<T> ConvertToModalByDataTable<T>(DataTable dt)
        {
            try
            {
                return ConvertToModalByDataTable<T>(dt, false);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据DataTable转换回Model
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="dt">DataTable</param>
        /// <returns>IList</returns>
        public static IList<T> ConvertToModalByDataTable<T>(DataTable dt, Boolean bDBName)
        {
            try
            {
                IList<T> ts = new List<T>();
                T t = default(T);
                Type type = typeof(T);
                PropertyInfo[] props = null;
                string strTempName = string.Empty;

                foreach (DataRow dr in dt.Rows)
                {
                    t = Activator.CreateInstance<T>();
                    props = t.GetType().GetProperties();
                    foreach (PropertyInfo pi in props)
                    {
                        strTempName = pi.Name;
                        if (bDBName)
                        {
                            object[] CustomAttributes = pi.GetCustomAttributes(typeof(DataFieldAttribute), false);
                            if (CustomAttributes.Length > 0)
                            {
                                DataFieldAttribute FieldAttr = CustomAttributes[0] as DataFieldAttribute;

                                if (FieldAttr != null)
                                {
                                    if (FieldAttr is DataFieldAttribute)
                                    {
                                        strTempName = FieldAttr.Fieldname;
                                    }
                                }
                            }
                        }
                        if (dt.Columns.Contains(strTempName))
                        {
                            //if (!pi.CanWrite) continue;
                            object value = dr[strTempName];
                            if (value != DBNull.Value)
                            {
                                pi.SetValue(t, value, null);
                            }
                        }

                    }
                    ts.Add(t);
                }
                return ts;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPC04]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPC04"); 
            }
        }
        
        /// <summary>
        /// 将Model转化为DataTable
        /// </summary>
        /// <param name="entity">List Model</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTableByModel<T>(List<T> entity)
        {
            try
            {
                return ConvertToDataTableByModel<T>(entity, false, false);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 将Model转化为DataTable
        /// </summary>
        /// <param name="entity">List Model</param>
        /// <param name="bDBName">是否使用数据库字段名</param>
        /// <returns></returns>
        public static DataTable ConvertToDataTableByModel<T>(List<T> entity,Boolean bDBName,Boolean bStructureOnly)
        {
            try
            {
                var props = entity[0].GetType().GetProperties();
                var dt = new DataTable();

                foreach (PropertyInfo pi in props)
                {
                    //建立Column，并判断是否为Nullable类型，如果是则使用原定义字段建立表
                    Type columnType = pi.PropertyType;
                    if (columnType.IsGenericType && columnType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        columnType = columnType.GetGenericArguments()[0];
                    }

                    if (bDBName)
                    {
                        object[] CustomAttributes = pi.GetCustomAttributes(typeof(DataFieldAttribute), false);

                        if (CustomAttributes.Length > 0)
                        {
                            DataFieldAttribute FieldAttr = CustomAttributes[0] as DataFieldAttribute;

                            if (FieldAttr != null)
                            {
                                if (FieldAttr is DataFieldAttribute)
                                {
                                    dt.Columns.Add(FieldAttr.Fieldname, columnType);
                                }
                            }
                        }
                    }
                    else
                    {
                        dt.Columns.Add(pi.Name, columnType);
                    }
                }

                if (!bStructureOnly)
                {
                    if (entity.Count > 0)
                    {
                        for (int i = 0; i < entity.Count; i++)
                        {
                            ArrayList arrTempList = new ArrayList();
                            foreach (PropertyInfo pi in props)
                            {
                                object obj = pi.GetValue(entity.ElementAt(i), null);
                                arrTempList.Add(obj);
                            }
                            object[] objTemp = arrTempList.ToArray();
                            dt.LoadDataRow(objTemp, true);
                        }
                    }
                }
                return dt;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPC05]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPC05"); 
            }
        }

        /// <summary>
        /// 根据Hashtable填充Model（HashTable的Key为数据库字段名）
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="hsData">HashTable</param>
        /// <returns>按HashTable填充完成的Model对象</returns>
        public static object FillModelbyHashtable(object entity, Hashtable hsData)
        {
            try
            {
                return FillModelbyHashtable(entity, hsData, true);
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 根据Hashtable填充Model
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="hsData">HashTable</param>
        /// <param name="bDBName">HashTable中的Key是否为数据库字段名</param>
        /// <returns>按HashTable填充完成的Model对象</returns>
        public static object FillModelbyHashtable(object entity, Hashtable hsData, Boolean bDBName)
        {
            try
            {
                string strKeyName = string.Empty;
                string strFieldName = string.Empty;
                PropertyInfo prop = null;

                foreach (DictionaryEntry de in hsData)
                {
                    strKeyName = de.Key.ToString();
                    if (bDBName)
                    {
                        //hashtable中key为数据库字段名
                        //根据数据库字段名获取封装字段名
                        strFieldName = GetFieldNameByDBName(entity, strKeyName);
                    }
                    else
                    {
                        //hashtable中key为封装字段名
                        strFieldName = strKeyName;
                    }

                    if (!string.IsNullOrEmpty(strFieldName))
                    {
                        prop = entity.GetType().GetProperty(strFieldName);
                        if (prop != null)
                        {
                            if (de.Value != null)
                            {
                                if (!prop.PropertyType.IsGenericType)
                                {
                                    prop.SetValue(entity, string.IsNullOrEmpty(de.Value.ToString()) ? null : Convert.ChangeType(de.Value, prop.PropertyType), null);
                                }
                                else
                                {
                                    //用于处理nullable类型数据
                                    Type genericTypeDef = prop.PropertyType.GetGenericTypeDefinition();
                                    if (genericTypeDef == typeof(Nullable<>))
                                    {
                                        prop.SetValue(entity, string.IsNullOrEmpty(de.Value.ToString()) ? null : Convert.ChangeType(de.Value, Nullable.GetUnderlyingType(prop.PropertyType)), null);
                                    }
                                }
                            }
                        }
                    }
                }

                return entity;
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPC06]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPC06"); 
            }
        }

        /// <summary>
        /// 比对DataTable数据内容并返回不一致项
        /// </summary>
        /// <param name="FirstDataTable">更新后的DataTable</param>
        /// <param name="SecondDataTable">更新前的DataTable</param>
        /// <returns>包含差别的DataSet数据集</returns>
        public static DataSet GetDifferentRecords(DataTable dsNewDataTable, DataTable dsOriginalDataTable)
        {
            try
            {
                //创建空Table  
                DataTable InsertModifiedDataTable = new DataTable("InsertModifiedDataTable");
                DataTable DeletedDataTable = new DataTable("DeletedDataTable");
                DataSet dsReturn = new DataSet();


                //使用DataSet建立DataRelation  
                using (DataSet ds = new DataSet())
                {
                    //将两张DataTable加入DataSet  
                    ds.Tables.AddRange(new DataTable[] { dsNewDataTable.Copy(), dsOriginalDataTable.Copy() });

                    //获取Columns  
                    DataColumn[] firstColumns = new DataColumn[ds.Tables[0].Columns.Count];
                    for (int i = 0; i < firstColumns.Length; i++)
                    {
                        firstColumns[i] = ds.Tables[0].Columns[i];
                    }

                    DataColumn[] secondColumns = new DataColumn[ds.Tables[1].Columns.Count];
                    for (int i = 0; i < secondColumns.Length; i++)
                    {
                        secondColumns[i] = ds.Tables[1].Columns[i];
                    }

                    //建立DataRelation  
                    DataRelation r1 = new DataRelation(string.Empty, firstColumns, secondColumns, false);
                    ds.Relations.Add(r1);

                    DataRelation r2 = new DataRelation(string.Empty, secondColumns, firstColumns, false);
                    ds.Relations.Add(r2);

                    //建立返回的DataSet  
                    for (int i = 0; i < dsNewDataTable.Columns.Count; i++)
                    {
                        InsertModifiedDataTable.Columns.Add(dsNewDataTable.Columns[i].ColumnName, dsNewDataTable.Columns[i].DataType);
                        DeletedDataTable.Columns.Add(dsOriginalDataTable.Columns[i].ColumnName, dsNewDataTable.Columns[i].DataType);
                    }

                    //新表的数据不在老表中，则为新增或修改的数据
                    InsertModifiedDataTable.BeginLoadData();
                    foreach (DataRow parentrow in ds.Tables[0].Rows)
                    {
                        DataRow[] childrows = parentrow.GetChildRows(r1);
                        if (childrows == null || childrows.Length == 0)
                            InsertModifiedDataTable.LoadDataRow(parentrow.ItemArray, true);
                    }
                    InsertModifiedDataTable.EndLoadData();


                    //老表的数据不在新表中，则为删除的数据
                    DeletedDataTable.BeginLoadData();
                    foreach (DataRow parentrow in ds.Tables[1].Rows)
                    {
                        DataRow[] childrows = parentrow.GetChildRows(r2);
                        if (childrows == null || childrows.Length == 0)
                            DeletedDataTable.LoadDataRow(parentrow.ItemArray, true);
                    }
                    DeletedDataTable.EndLoadData();
                }

                dsReturn.Tables.Add(InsertModifiedDataTable);
                dsReturn.Tables.Add(DeletedDataTable);

                return dsReturn;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPC07]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPC07"); 
            }
        }

        /// <summary>
        /// by px
        /// 得到SQL的where语句,数据库中字段必须为字符型的.(int型的不能用这个方法).
        /// </summary>
        /// <param name="jsonstr">传入的类json 格式的string 如:"key1:val1,key2,val2,..."</param>       
        /// <returns>SQL中的where语句</returns>
        /// 
        public static string GetSqlfromJsonStr(string jsonstr)
        {
            string retstr = string.Empty;
            if (jsonstr != null)
            {
                string[] arr = jsonstr.Split(',');
                bool andflag = false;
                foreach (string itor in arr)
                {
                    string[] map = itor.Split(':');
                    if(map.Length == 2)
                    {
                        if(andflag)
                        {
                            retstr += " and ";
                        }
                        retstr += map[0];
                        retstr += " = '" + map[1] + "'";
                        andflag = true;
                    }
                }
            }
            return retstr;
        }

        
        /// <summary>
        /// 根据Model生成Where条件
        /// </summary>
        /// <param name="entity">Model对象</param>
        /// <param name="bDBName">返回的结果ds中是否以数据库中字段名作为列名（false时使用Model中封装名称）</param>
        /// <param name="bPKonly">生成的Where条件中是否是按主键生成</param>
        /// <param name="strOutSql">返回的错误信息或sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetWhereSqlWithNoParam(object entity, Boolean bDBName, Boolean bPKonly, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                //用于拼接sql语句的字段名和字段值
                StringBuilder strFields = new StringBuilder();
                StringBuilder strWhereParam = new StringBuilder();

                //获取模型对应的属性
                PropertyInfo[] props = entity.GetType().GetProperties();
                DataFieldAttribute FieldAttr = null;
                object[] CustomAttributes;


                string strTablename = string.Empty;
                string strFieldName = string.Empty;
                string strFieldType = string.Empty;
                object objFieldValue = null;
               
                int j = 0;
                foreach (PropertyInfo prop in props)
                {
                    CustomAttributes = prop.GetCustomAttributes(typeof(DataFieldAttribute), false);
                    if (CustomAttributes.Length > 0)
                    {
                        FieldAttr = CustomAttributes[0] as DataFieldAttribute;

                        if (FieldAttr != null)
                        {
                            if (FieldAttr is DataFieldAttribute)
                            {
                                strFieldName = FieldAttr.Fieldname;
                                strFieldType = FieldAttr.Fieldtype;
                                objFieldValue = prop.GetValue(entity, null);                                

                                //生成Where条件
                                if (bPKonly)
                                {
                                    //只按照主键生成Where语句
                                    if (FieldAttr.Isrequired == true)
                                    {
                                        if (objFieldValue != null && objFieldValue.ToString() != "")
                                        {
                                            if (j > 0)
                                            {
                                                strWhereParam.Append(" and ");
                                            }

                                            if (arrTextField.Contains(strFieldType))
                                            {
                                                //为文本字段增加前后引号
                                                strWhereParam.Append(strFieldName + " = \'" + objFieldValue.ToString().Replace("'", "''") + "\'");
                                            }
                                            else
                                            {
                                                strWhereParam.Append(strFieldName + " = " + objFieldValue + "");
                                            }

                                            j++;
                                        }
                                    }
                                }
                                else if (objFieldValue != null && objFieldValue.ToString() != "")
                                {
                                    if (j > 0)
                                    {
                                        strWhereParam.Append(" and ");
                                    }

                                    if (arrTextField.Contains(strFieldType))
                                    {
                                        //为文本字段增加前后引号
                                        strWhereParam.Append(strFieldName + " = \'" + objFieldValue.ToString().Replace("'", "''") + "\'");
                                    }
                                    else
                                    {
                                        strWhereParam.Append(strFieldName + " = " + objFieldValue + "");
                                    }

                                    j++;
                                }
                            }
                        }
                    }
                }

                if (strFields.ToString().Trim() != "" && strWhereParam.ToString().Trim() != "")
                {                   
                    strOutSql = strWhereParam.ToString();
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG05]构造Select语句失败，字段字符串为空。", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG05");
                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG16]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG16");
            }
        }

        /// <summary>
        /// 根据Model中的PK查询条件构造查询满足条件条数的语句
        /// </summary>
        /// <param name="entity">Model</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="strOutSql">返回的sql语句</param>
        /// <returns>是否成功</returns>
        public static Boolean GetCountAllSqlForPK(object entity, out string strOutSql)
        {
            try
            {
                strOutSql = string.Empty;

                string strDeleteSQL = "SELECT COUNT(*) FROM {0} WHERE {1}";
                string strTablename = string.Empty;
                string strWhereParam = string.Empty;

                //取数据库表名
                if (GetTableName(entity, out strTablename) == false)
                {
                    strOutSql = strTablename;
                    return false;
                }


                if (GetWhereSqlWithNoParam(entity, true, true, out strWhereParam))
                {
                    //按照固定格式生成insert语句
                    string[] Args = new string[] { strTablename, strWhereParam };
                    strOutSql = string.Format(strDeleteSQL, Args);
                    return true;
                }
                else
                {
                    LogHelper.SetP("[HLPG12]构造Select Count语句失败，条件为空。", (int)LogLevel.ERROR);
                    throw new ACMSCustomException("HLPG12");

                }
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPG13]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPG13");
            }
        }

        /// <summary>
        /// 根据Model的主键查询是否数据库中是否有数据，如果有就更新，没有就插入数据
        /// </summary>
        /// <param name="entity">Model</param>
        /// <param name="strWhereParam">Where条件</param>
        /// <param name="strOutstr">返回的结果</param>
        /// <returns></returns>
        public static Boolean InsertOrUpdateByModal(object entity, out string strOutstr)
        {
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();

                strOutstr = string.Empty;
                if (GetCountAllSqlForPK(entity, out strOutstr) == false)
                {
                    return false;
                }
                                
                strOutstr = dbAccess.ExecuteScalar(strOutstr).ToString();
                if(string.IsNullOrEmpty(strOutstr) || "0".Equals(strOutstr))
                {
                    SqlHelper.InsertByModel(entity, out strOutstr);
                }
                else
                {
                    SqlHelper.UpdateByModel(entity, out strOutstr);
                }
                return true;
            }
            catch (ACMSCustomException)
            {
                throw;
            }
            catch (Exception e)
            {
                LogHelper.SetP("[HLPS02]" + e.Message, (int)LogLevel.ERROR);
                throw new ACMSCustomException("HLPS02");
            }
        }
        #endregion
    }
}
