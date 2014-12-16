using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Ionic.Zlib;
using HtmlAgilityPack;
using System.Collections;
using System.Text.RegularExpressions;
using BLL;
using DAL;
using Model;
using System.Data;
using AppTool;
using log4net;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BLL
{
    public class StockHelper
    {
        /// <summary>
        /// 取基本表里的所有股票数据
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> getAllStockCode()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                StockbaseBean baseBean = new StockbaseBean();
                string errMsg = string.Empty;
                string sqlwhere = @"1=1";
                DataSet ds;
                SqlHelper.QueryByModel(baseBean, sqlwhere, out ds, out errMsg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["stb_code"].ToString()) && !string.IsNullOrEmpty(dr["stb_name"].ToString()))
                        {
                            dic.Add(dr["stb_code"].ToString(), dr["stb_name"].ToString());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.ErrorLog("StockHelper001:" + ex.Message);
            }
            return dic;
        }

        /// <summary>
        /// 按类型获取网络错误时需要重新获取数据的代码
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> getNetErrorStockCode(string type)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            try
            {
                NeterrBean netbean = new NeterrBean();
                string errMsg = string.Empty;
                string sqlwhere = string.Format("nte_type='{0}'",type);
                DataSet ds;
                SqlHelper.QueryByModel(netbean, sqlwhere, out ds, out errMsg);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["nte_code"].ToString()) && !string.IsNullOrEmpty(dr["nte_name"].ToString()))
                        {
                            dic.Add(dr["nte_code"].ToString(), dr["nte_name"].ToString());
                        }
                    }
                }
                SqlHelper.DeleteByModel(netbean, sqlwhere, out errMsg);
            }
            catch (System.Exception ex)
            {
                LogHelper.ErrorLog("StockHelper002:" + ex.Message);
            }
            return dic;
        }
    }
}
