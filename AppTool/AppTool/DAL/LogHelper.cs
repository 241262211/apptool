using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using System.Diagnostics;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "../../log4net.config",Watch = true)]
namespace DAL
{
    public static class LogHelper
    {
        /// <summary>
        /// 写日志
        /// log.set("用户名重复");
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20140704    王强      创建代码
        ///----------------------------------------------------------
        public static string Set(string content)
        {
            string res = string.Empty;
            try
            {
                
//                 LogBean logbean = new LogBean();
//                 logbean.Jrn = Jrn.Get();
//                 logbean.New = content;
//                 logbean.Rcdtime = DateTime.Now;
//                 string errStr = string.Empty;
//                 SqlHelper.InsertByModel(logbean, out errStr);
                return res;
            }
            catch (Exception ex)
            {

                return res + ex.Message;
            }
        }

        /// <summary>
        /// 写日志,加入错误码信息
        /// log.set("SD001","用户名重复");
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20140704    王强      创建代码
        ///----------------------------------------------------------
        public static string Set(string errcod, string content)
        {
            string res = string.Empty;
            try
            {

//                 LogBean logbean = new LogBean();
//                 logbean.Jrn = Jrn.Get();
//                 logbean.New = content;
//                 logbean.Opid = errcod;
//                 logbean.Rcdtime = DateTime.Now;
//                 string errStr = string.Empty;
//                 SqlHelper.InsertByModel(logbean, out errStr);
                return res;
            }
            catch (Exception ex)
            {

                return res + ex.Message;
            }
        }


        /// <summary>
        /// 写日志,直接的错误码信息
        /// log.SetErrCode("SD001");
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20140704    王强      创建代码
        ///----------------------------------------------------------
        public static string SetErrCode(string errcod)
        {
            string res = string.Empty;
            try
            {

//                 LogBean logbean = new LogBean();
//                 logbean.Jrn = Jrn.Get();
//                 logbean.New = "";
//                 logbean.Opid = errcod;
//                 logbean.Rcdtime = DateTime.Now;
//                 string errStr = string.Empty;
//                 SqlHelper.InsertByModel(logbean, out errStr);
                return res;
            }
            catch (Exception ex)
            {
                return res + ex.Message;
            }
        }

        /// <summary>
        /// 按优先级设置系统日志记录的级别
        /// 9级最高，1级最低,9级的是肯定要记得
        /// log.SetP("参数设置错误",5)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="priority">1-debug;5-info;9-erro</param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20140704    王强      创建代码
        ///----------------------------------------------------------
        public static string SetP(string content,int priority)
        {
            string res = string.Empty;
            try
            {
                /*
                int sysPriority = 0;
                LogBean logbean = new LogBean();
                logbean.Jrn = Jrn.Get();
                logbean.New = content;
                logbean.Rcdtime = DateTime.Now;
                string errStr = string.Empty;
                if (priority >= sysPriority)
                {
                    SqlHelper.InsertByModel(logbean, out errStr);
                }
                 */
                return res;
            }
            catch (Exception ex)
            {
                return res + ex.Message;
            }
        }


        /// <summary>
        /// 按错误码，优先级设置系统日志记录的级别
        /// 9级最高，1级最低
        /// log.SetP("","参数设置错误",5)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20140704    王强      创建代码
        ///----------------------------------------------------------
        public static string SetP(string errno,string content, int priority)
        {
            string res = string.Empty;
            try
            {
                /*
                int sysPriority = 0;
                LogBean logbean = new LogBean();
                logbean.Jrn = Jrn.Get();
                logbean.Opid = errno;
                logbean.New = content;
                logbean.Rcdtime = DateTime.Now;
                string errStr = string.Empty;
                if (priority >= sysPriority)
                {
                    SqlHelper.InsertByModel(logbean, out errStr);
                }
                 */
                return res;
            }
            catch (Exception ex)
            {
                return res + ex.Message;
            }
        }

        /// <summary>
        /// 用log4net记录错误日志
        /// </summary>
        /// <param name="loggername"></param>
        /// <param name="msg"></param>
        public static void ErrorLog(string msg, string loggername = "Default")
        {
            ILog log = log4net.LogManager.GetLogger(loggername);
            log.Error(msg);
        }

        /// <summary>
        /// 用log4net记录信息日志
        /// </summary>
        /// <param name="loggername"></param>
        /// <param name="msg"></param>
        public static void InfoLog(string msg, string loggername = "Default")
        {
            ILog log = log4net.LogManager.GetLogger(loggername);            
            log.Info(msg);
        }
    }

    public class ACMSCustomException : Exception
    {
        public string strdata;
        public ACMSCustomException(string message):base(message)
        {            
        }

        public ACMSCustomException(string message, string data)
            : base(message)
        {
            strdata = data;
        }
    }
}
