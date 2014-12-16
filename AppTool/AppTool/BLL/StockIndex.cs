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
    /// <summary>
    /// 用于记录指标分析结果的中间结构
    /// </summary>
    public class AnalysRow
    {
        public int  MAXROW = 10;
        public int intervalcount;          //记录已扫过的ROW的行数
        public double markPrice;            //指标分析到时的价格，即当天的收盘价 
        public DataRow sourceRow;          //原记录的Row
        public IndexanalysBean resBean;    //结果
        
    }

    /// <summary>
    /// 用于记录KDJ结构的最低价和最高价
    /// </summary>
    public class LineRowData
    {
        public double lowprice;
        public double hightprice;
        public double endprice;
    }



    public class StockIndex
    {


        /// <summary>
        /// 计算MACD指标
        /// </summary>
        public bool IndexMACD()
        {
            LogHelper.InfoLog("MACD_INDEX start", "IndexLog");
            Dictionary<string,string> codeDic = StockHelper.getAllStockCode();
            //int[] arr = new int[3];
            //ArrayList list = new ArrayList(arr);
            //Parallel.ForEach(new Partitioner<string>(list), (dicEntry) => { });

            Parallel.ForEach(codeDic, (dicEntry) =>
            {
                //foreach(KeyValuePair<string,string> dicEntry in codeDic){
                string code = dicEntry.Key;
                string name = dicEntry.Value;
                string sqlstr = string.Format("select * from dayflow where dfl_code = '{0}' order by dfl_date", code);
                try
                {
                    DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                    DBOperator dbAccess = dbFactory.GetLocalDBOperator();
                    DataSet ds = dbAccess.ExecuteQuerry(sqlstr);
                    Boolean bFirstRow = true;
                    if (Helper.IsDataSetValued(ds))
                    {
                        List<DayflowBean> beanList = new List<DayflowBean>();
                        DataRow lastdr = null;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {

                            if (!string.IsNullOrEmpty(dr["dfl_nearema"].ToString()) && !string.IsNullOrEmpty(dr["dfl_farema"].ToString())
                                && !string.IsNullOrEmpty(dr["dfl_macddea"].ToString()) && !string.IsNullOrEmpty(dr["dfl_macdbar"].ToString()))
                            {
                                lastdr = dr;
                                bFirstRow = false;
                                continue;
                            }
                            if (string.IsNullOrEmpty(dr["dfl_endprice"].ToString()))
                            {
                                LogHelper.ErrorLog(string.Format("MACD EndPrice cannot be NULL.code:{0},date:{1}", code, dr["dfl_date"].ToString()), "IndexLog");
                                return;
                            }
                            DayflowBean dfbean = new DayflowBean();
                            dfbean.Code = code;
                            dfbean.Date = dr["dfl_date"].ToString();
                            double endprice = (double)dr["dfl_endprice"];
                            if (bFirstRow)
                            {
                                dr["dfl_nearema"] = endprice;
                                dr["dfl_farema"] = endprice;
                                dr["dfl_macddea"] = 0;
                                dr["dfl_macdbar"] = 0;
                                dfbean.Nearema = endprice;
                                dfbean.Farema = endprice;
                                dfbean.Macddea = 0;
                                dfbean.Macdbar = 0;
                            }
                            else
                            {
                                dfbean.Nearema = (double)lastdr["dfl_nearema"] * 11 / 13 + endprice * 2 / 13;
                                dfbean.Farema = (double)lastdr["dfl_farema"] * 25 / 27 + endprice * 2 / 27;
                                dfbean.Macddea = (double)lastdr["dfl_macddea"] * 8 / 10 + (dfbean.Nearema - dfbean.Farema) * 2 / 10;
                                dfbean.Macdbar = 2 * (dfbean.Nearema - dfbean.Farema - dfbean.Macddea);
                                dr["dfl_nearema"] = dfbean.Nearema;
                                dr["dfl_farema"] = dfbean.Farema;
                                dr["dfl_macddea"] = dfbean.Macddea;
                                dr["dfl_macdbar"] = dfbean.Macdbar;
                            }
                            beanList.Add(dfbean);
                            lastdr = dr;
                            bFirstRow = false;
                        }
                        string errMsg = string.Empty;
                        SqlHelper.UpdateByModel(beanList, out errMsg);
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.ErrorLog(string.Format("MACD IndexError.code:{0}", code), "IndexLog");
                }
            });
            LogHelper.InfoLog("MACD_INDEX end", "IndexLog"); 
            return true;
        }

        /// <summary>
        /// MACD指标分析
        /// </summary>
        /// <returns></returns>
        public bool MACDAnalysis()
        {

            LogHelper.InfoLog("MACD_INDEX Analysis start", "IndexAnalyLog");
            Dictionary<string, string> codeDic = StockHelper.getAllStockCode(); 
            Parallel.ForEach(codeDic, (dicEntry) =>
            {                
                string code = dicEntry.Key;
                string name = dicEntry.Value;
                try
                {
                    string sqlstr = string.Format("select * from dayflow where dfl_code = '{0}' order by dfl_date", code);
                    DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                    DBOperator dbAccess = dbFactory.GetLocalDBOperator();
                    DataSet ds = dbAccess.ExecuteQuerry(sqlstr);
                    Boolean bFirstRow = true;
                    if (Helper.IsDataSetValued(ds))                
                    {                    
                        DataRow lastdr = null;
                        List<AnalysRow> rowList = new List<AnalysRow>();
                        List<IndexanalysBean> beanList = new List<IndexanalysBean>();                        
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {   
                            if(bFirstRow)
                            {
                                lastdr = dr;
                                bFirstRow = false;
                                continue;
                            }
                            double endprice = Convert.ToDouble(dr["dfl_endprice"].ToString());
                            double lastprice = Convert.ToDouble(dr["dfl_lastprice"].ToString());
                            foreach (var itrow in rowList)
                            {
                                if (itrow.intervalcount < 0)    //此时这个记录已记录完成,跳过计算
                                {
                                    continue;
                                }
                                itrow.intervalcount++;
                                if (itrow.intervalcount == 1)   //计算第二天涨幅
                                {
                                    itrow.resBean.Nextincrease = (endprice - lastprice)*100 / lastprice;
                                }
                                else if (itrow.intervalcount == 5)
                                {
                                    itrow.resBean.Day5increase = (endprice - itrow.markPrice) * 100 / lastprice;
                                }
                                else if (itrow.intervalcount == 10)
                                {
                                    itrow.resBean.Day10increase = (endprice - itrow.markPrice) * 100 / lastprice;
                                }
                                if (itrow.intervalcount > itrow.MAXROW)
                                {
                                    beanList.Add(itrow.resBean);
                                    itrow.intervalcount = -1;
                                }
                            }
                            //subtype 00 和01 的差别为以均线和以bar柱为标准
                            if (Convert.ToDouble(dr["dfl_nearema"].ToString()) > Convert.ToDouble(dr["dfl_farema"].ToString()) &&
                                Convert.ToDouble(lastdr["dfl_nearema"].ToString()) < Convert.ToDouble(lastdr["dfl_farema"].ToString()) &&
                                Convert.ToDouble(dr["dfl_macdbar"].ToString()) > 0 &&
                                Convert.ToDouble(lastdr["dfl_kdjkline"].ToString()) < 70 && Convert.ToDouble(lastdr["dfl_kdjjline"].ToString()) < 80 &&
                                (Convert.ToDouble(dr["dfl_kdjkline"].ToString()) > Convert.ToDouble(lastdr["dfl_kdjkline"].ToString())))
                            //if (Convert.ToDouble(lastdr["dfl_nearema"].ToString()) < Convert.ToDouble(lastdr["dfl_farema"].ToString()) &&
                            //    Convert.ToDouble(dr["dfl_macdbar"].ToString()) > 0 && Convert.ToDouble(lastdr["dfl_macdbar"].ToString()) < 0 &&
                            //    (Convert.ToDouble(dr["dfl_kdjkline"].ToString()) > Convert.ToDouble(lastdr["dfl_kdjkline"].ToString())) &&
                            //    Convert.ToDouble(lastdr["dfl_kdjkline"].ToString()) < 70 && Convert.ToDouble(lastdr["dfl_kdjjline"].ToString()) < 80)
                            {
                                //LogHelper.InfoLog("MACDAnalysis     code:" + dr["dfl_code"].ToString() + "  date:" + dr["dfl_date"].ToString(), "IndexAnalyLog");
                                IndexanalysBean tmpbean = new IndexanalysBean();
                                tmpbean.Id = Helper.getJrnNo();
                                tmpbean.Code = code;
                                tmpbean.Date = dr["dfl_date"].ToString();
                                tmpbean.Name = name;
                                tmpbean.Type = "MACD";
                                tmpbean.Subtype = "00";
                                tmpbean.Markprice = endprice;
                                tmpbean.Todayincrease = (endprice - lastprice)*100/lastprice;
                                AnalysRow tmprow = new AnalysRow();
                                tmprow.intervalcount = 0;
                                tmprow.markPrice = endprice;
                                tmprow.sourceRow = dr;                                
                                tmprow.resBean = tmpbean;
                                rowList.Add(tmprow);
                            }
                            lastdr = dr;
                            bFirstRow = false;                            
                        }
                        foreach (var itrow in rowList)
                        {
                            if (itrow.intervalcount < 0)    //此时这个记录已记录完成,跳过计算
                            {
                                continue;
                            }
                            beanList.Add(itrow.resBean);
                        }
                        string eMsg = string.Empty;
                        SqlHelper.InsertByModel(beanList, out eMsg);
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.ErrorLog(string.Format("MACD MACDAnalysis.code:{0}",code));
                }                
            });
            LogHelper.InfoLog("MACD_INDEX Analysis end", "IndexAnalyLog");
            return true;
        }

        /// <summary>
        /// 计算KDJ指标
        /// </summary>
        public bool IndexKDJ()
        {
            LogHelper.InfoLog("KDJ_INDEX start", "IndexLog");
            Dictionary<string, string> codeDic = StockHelper.getAllStockCode();            

            Parallel.ForEach(codeDic, (dicEntry) =>
            {                
                string code = dicEntry.Key;
                string name = dicEntry.Value;
                string sqlstr = string.Format("select * from dayflow where dfl_code = '{0}' order by dfl_date", code);
                try
                {
                    DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                    DBOperator dbAccess = dbFactory.GetLocalDBOperator();
                    DataSet ds = dbAccess.ExecuteQuerry(sqlstr);
                    Boolean bFirstRow = true;
                    if (Helper.IsDataSetValued(ds))
                    {
                        List<DayflowBean> beanList = new List<DayflowBean>();
                        DataRow lastdr = null;
                        Queue<LineRowData> rowQueue = new Queue<LineRowData>();
                        ArrayList arrList = new ArrayList();
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (rowQueue.Count >= 9)//KDJ以九日为计算周期
                            {
                                LineRowData row = rowQueue.Dequeue();
                                row.lowprice = Convert.ToDouble(dr["dfl_lowprice"].ToString());
                                row.hightprice = Convert.ToDouble(dr["dfl_hightprice"].ToString());
                                rowQueue.Enqueue(row);
                            }
                            else
                            {
                                LineRowData row = new LineRowData();
                                row.lowprice = Convert.ToDouble(dr["dfl_lowprice"].ToString());
                                row.hightprice = Convert.ToDouble(dr["dfl_hightprice"].ToString());
                                rowQueue.Enqueue(row);
                            }

                            if (!string.IsNullOrEmpty(dr["dfl_kdjkline"].ToString()) && !string.IsNullOrEmpty(dr["dfl_kdjjline"].ToString())
                                && !string.IsNullOrEmpty(dr["dfl_kdjdline"].ToString()) )
                            {
                                lastdr = dr;
                                bFirstRow = false;                                                                
                                continue;
                            }                            
                            DayflowBean dfbean = new DayflowBean();
                            dfbean.Code = code;
                            dfbean.Date = dr["dfl_date"].ToString();
                            double endprice = (double)dr["dfl_endprice"];
                            if (bFirstRow)
                            {                               
                                dfbean.Kdjkline = 50;
                                dfbean.Kdjdline = 50;
                                dfbean.Kdjjline = 50;
                                dr["dfl_kdjkline"] = 50;
                                dr["dfl_kdjdline"] = 50;
                                dr["dfl_kdjjline"] = 50;                                
                            }
                            else
                            {
                                double Ln = 10000;
                                double Hn = 0;
                                foreach (var row in rowQueue)
                                {
                                    if (row.lowprice < Ln)
                                        Ln = row.lowprice;
                                    if (row.hightprice > Hn)
                                        Hn = row.hightprice;
                                }
                                double rsv = 100;
                                if (Hn - Ln > 0.001)
                                {
                                    rsv = (endprice - Ln) * 100 / (Hn - Ln);
                                }

                                dfbean.Kdjkline = (double)lastdr["dfl_kdjkline"] * 2 / 3 + rsv / 3;
                                dfbean.Kdjdline = (double)lastdr["dfl_kdjdline"] * 2 / 3 + dfbean.Kdjkline / 3;
                                dfbean.Kdjjline = 3 * dfbean.Kdjkline - 2 * dfbean.Kdjdline;
                                dr["dfl_kdjkline"] = dfbean.Kdjkline;
                                dr["dfl_kdjdline"] = dfbean.Kdjdline;
                                dr["dfl_kdjjline"] = dfbean.Kdjjline; 
                                
                            }
                            beanList.Add(dfbean);
                            lastdr = dr;
                            bFirstRow = false;                            
                        }
                        string errMsg = string.Empty;
                        SqlHelper.UpdateByModel(beanList, out errMsg);
                    }
                }
                catch (System.Exception ex)
                {
                    LogHelper.ErrorLog(string.Format("MACD IndexError.code:{0}", code), "IndexLog");
                }
            });
            LogHelper.InfoLog("KDJ_INDEX end", "IndexLog");
            return true;
        }

    }//class END
}
