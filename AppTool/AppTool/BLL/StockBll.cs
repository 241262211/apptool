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
    public class StockBll
    {
        static string dayKLineLogErr = string.Format("DayKLineLog_err_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
        static string dayKLineLog = string.Format("DayKLineLog_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
        //static string dayKHistoryLog = string.Format("DayKHistoryLog_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
        //static string dayKHistoryLogErr = string.Format("DayKHistoryLog_err_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
        //static string min5KLineLog = string.Format("min5KLineLog_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));
        //static string min5KLineLogErr = string.Format("min5KLineLog_err_{0}.txt", DateTime.Now.ToString("yyyyMMdd"));

        /// <summary>
        /// 测试
        /// </summary>
        public bool CodeTest()
        {
            string result = string.Empty;
            string netstr = "k_daily_600376_14=20140102,5.02,5.09,4.98,4.99,9636642.00,48444239.00|20140103,4.99,5.00,4.86,4.90,5481625.00,26906805.00|20140104,4.99,5.00,4.86,4.90,5481625.00,26906805.00|20141205,7.65,7.69,7.02,7.25,94959716.00,696813280.00|";
            //Regex reg = new Regex(@".*?(?:=|\|)(20140104.*?\|)?");
            string url = @"http://qd.10jqka.com.cn/api.php?year=2014&p=stock_day&fq=&info=k_sh_600376";
            GetMothed(url, out netstr);
            Regex reg = new Regex(@"(?:=|\|)((\d+).*?)(?=\|)");
            MatchCollection ms = reg.Matches(netstr);
            var matchcount = ms.Count;
            var date = ms[0].Groups[2];
            var content = ms[0].Groups[1];
            if (reg.IsMatch(netstr))
            {
                //result = reg.Match(netstr).Groups[1].ToString();
                result = reg.Replace(netstr, "$1");
            }
            //ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            //LogHelper.InfoLog("infostring", "Min5Log");
            //return true;

            //ConcurrentQueue<string> netResQueue = new ConcurrentQueue<string>();
            //StockbaseBean baseBean = new StockbaseBean();
            //string errMsg = string.Empty;
            //string sqlwhere = @"1=1";
            //DataSet ds;
            //SqlHelper.QueryByModel(baseBean, sqlwhere, out ds, out errMsg);
            //List<DayflowBean> beanList = new List<DayflowBean>();
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{                
            //    int count = 0;
            //    var watch = Stopwatch.StartNew();
            //    watch.Start();
            //    Parallel.ForEach<DataRow>(ds.Tables[0].AsEnumerable(), (dr) =>
            //        {
            //            count++;
            //            //string urlformat = @"http://qd.10jqka.com.cn/api.php?year=2014&p=stock_day&fq=&info=k_{0}_{1}";
            //            string urlformat = @"http://hq.sinajs.cn/list={0}{1}";
            //            //string urlformat = @"http://qd.10jqka.com.cn/api.php?p=stock_min5&info=k_{0}_{1}&fq=";
            //            string urltype = string.Empty;
            //            string code = dr["stb_code"].ToString();
            //            string name = dr["stb_name"].ToString();
            //            LogHelper.InfoLog(string.Format("Line:{0}  code:{1},date:{2} starting ,nowtime:{3}", count, code, name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff")));
            //            char firstchar = code[0];
            //            if (firstchar == '6' || firstchar == '9')
            //            {
            //                urltype = "sh";
            //            }
            //            else if (firstchar == '0' || firstchar == '2' || firstchar == '3')
            //            {
            //                urltype = "sz";
            //            }
            //            else
            //            {
            //                LogHelper.ErrorLog("urlcodetype is not sh and sz.  " + code);                            
            //            }
            //            string getstr = string.Empty;
            //            string url = string.Format(urlformat, urltype, code);
            //            if (GetMothed(url, out getstr))
            //            {
            //                netResQueue.Enqueue(getstr);
            //            }
            //        });
            //    LogHelper.InfoLog(string.Format("net耗用时间总共为:{0},总数为:{1}", watch.Elapsed.ToString(),netResQueue.Count));
            //    watch.Stop();
            //}
            return true;
            
        }

        /// <summary>
        /// 从新浪上获取所有的股票代码（实际上只有大部分股票代码)
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool getAllStockCode(out string errMsg)
        {
            errMsg = string.Empty;
            try
            {               
                Parallel.For(0,67,(i)=>{
                    HtmlDocument doc = new HtmlDocument();
                    string url = string.Format("http://vip.stock.finance.sina.com.cn/q/go.php/vIR_CustomSearch/index.phtml?p={0}&sr_p=-1", (i + 1).ToString());
                    if (GetMothed(url, out doc))
                    {
                        try
                        {
                            doc.Save(string.Format("./out/vipstock_{0}.htm", (i + 1).ToString()));
                            Regex reg0 = new Regex(@"(?:.|\s)*?>\s*(\d+)\s*</a>$");
                            Regex reg1 = new Regex(@"(?:.|\s)*?>\s*(\S+)\s*</span></a>$");
                            var table = doc.DocumentNode.SelectNodes("//table[@class=\"list_table\"]//tr");
                            List<StockbaseBean> beanList = new List<StockbaseBean>();
                            for (int j = 1; j < table.Count; j++)
                            {
                                var tdList = table[j].SelectNodes("./td");
                                string codeNum = string.Empty;
                                string codeName = string.Empty;
                                string sectionName = string.Empty;
                                string grade = string.Empty;
                                if (reg0.IsMatch(tdList[0].InnerHtml))
                                {
                                    codeNum = reg0.Replace(tdList[0].InnerHtml, "$1");
                                    if (codeNum.Length < 6 || (codeNum[0] != '0' && codeNum[0] != '3' && codeNum[0] != '6'))//代码不符合要求                                    
                                    {                                        
                                        continue;
                                    }
                                }
                                else
                                {
                                    LogHelper.ErrorLog("getAllStockCode RegError for code:" + tdList[0].InnerHtml);
                                    continue;
                                }
                                if (reg1.IsMatch(tdList[1].InnerHtml))
                                {
                                    codeName = reg1.Replace(tdList[1].InnerHtml, "$1");
                                }
                                else
                                {
                                    LogHelper.ErrorLog("getAllStockCode RegError for name:" + tdList[1].InnerHtml);
                                }
                                sectionName = tdList[8].InnerHtml;
                                grade = tdList[2].InnerHtml.Trim();
                                LogHelper.InfoLog(string.Format("page:{0}  row:{1} code:{2}    codeName:{3}    sectionName:{4}", i+1, j, codeNum, codeName, sectionName));
                                StockbaseBean baseBean = new StockbaseBean();
                                baseBean.Code = codeNum;
                                baseBean.Name = codeName;
                                baseBean.Section = sectionName;
                                baseBean.Grade = grade;
                                beanList.Add(baseBean);
                            }
                            string eMsg = string.Empty;
                            SqlHelper.InsertByModel(beanList, out eMsg);
                        }
                        catch (System.Exception ex)
                        {
                            LogHelper.ErrorLog(string.Format("getAllStockCode   URL:{0},ExceptMsg:{1}", url, ex.Message));
                        }                        
                    }
                });                
            }
            catch (Exception ex)
            {
                string errfile = new System.Diagnostics.StackFrame(true).GetFileName().ToString();
                string errline = new System.Diagnostics.StackFrame(true).GetFileLineNumber().ToString();
                errMsg = errfile + "  " + errline + "  " + ex.StackTrace;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从新浪上获取实时数据,日终时即为日终数据
        /// http://hq.sinajs.cn/list=sh600376
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool getDayKLine(out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                HtmlDocument doc = new HtmlDocument();
                ArrayList resList = new ArrayList();
                FileOp fileOp = new FileOp();
                DayflowBean dflowBean = new DayflowBean();
                ArrayList urlList = getSinaDayLineUrl();
                foreach (string url in urlList)
                {
                    if (GetMothed(url, out resList))
                    {
                        Regex reg = new Regex(@"(?:.|\s)*?(\d+)=""(.+)"";");

                        foreach (string listdata in resList)
                        {
                            try
                            {
                                if (reg.IsMatch(listdata))
                                {
                                    string code = reg.Replace(listdata, "$1");
                                    string data = reg.Replace(listdata, "$2");
                                    string[] strArr = data.Split(',');
                                    FileOp.Log(data, dayKLineLog);
                                    dflowBean.Code = code;
                                    dflowBean.Name = strArr[0];
                                    dflowBean.Date = strArr[30].Replace("-", "");
                                    dflowBean.Startprice = Convert.ToDouble(strArr[1]);
                                    dflowBean.Lastprice = Convert.ToDouble(strArr[2]);
                                    dflowBean.Endprice = Convert.ToDouble(strArr[3]);
                                    dflowBean.Hightprice = Convert.ToDouble(strArr[4]);
                                    dflowBean.Lowprice = Convert.ToDouble(strArr[5]);
                                    dflowBean.Tradenum = Convert.ToDouble(strArr[8]);
                                    dflowBean.Summoney = Convert.ToDouble(strArr[9]);
                                    dflowBean.Updatetime = strArr[31];
                                    SqlHelper.InsertByModel(dflowBean, out errMsg);
                                }
                                else
                                {
                                    LogHelper.ErrorLog("reg not match:" + listdata, "DayKLog");
                                }
                            }
                            catch (System.Exception ex)
                            {
                                LogHelper.ErrorLog("Except:" + listdata, "DayKLog");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string errfile = new System.Diagnostics.StackFrame(true).GetFileName().ToString();
                string errline = new System.Diagnostics.StackFrame(true).GetFileLineNumber().ToString();
                errMsg = errfile + "  " + errline + "  " + ex.StackTrace;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从同花顺上获取所有的日线历史数据
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool getDayKHistory(Form1 form, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                LogHelper.InfoLog("starttime:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss fff"), "DayKLog");
                Hashtable codedateHt = new Hashtable();
                if (!getNewestDateForDayKLine(out codedateHt))
                {
                    return false;
                }
                //StockbaseBean baseBean = new StockbaseBean();
                //string sqlwhere = @"1=1";
                //DataSet ds;
                //SqlHelper.QueryByModel(baseBean, sqlwhere, out ds, out errMsg);
                             
                //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                //{
                //    int count = 0;
                //    Parallel.ForEach(ds.Tables[0].AsEnumerable(), (dr) => {
                int count = 0;
                Regex reg = new Regex(@"(?:=|\|)((\d+).*?)(?=\|)");   
                Dictionary<string, string> codeDic = StockHelper.getAllStockCode();   
                Parallel.ForEach(codeDic, (dicEntry) =>{
                        count++;
                        //form.SetHisBarValue(100 * count++ / ds.Tables[0].Rows.Count);
                        string urlformat = @"http://qd.10jqka.com.cn/api.php?year=2014&p=stock_day&fq=&info=k_{0}_{1}";
                        string urltype = string.Empty;
                        string code = dicEntry.Key; ;
                        string name = dicEntry.Value;
                        LogHelper.InfoLog(string.Format("Line:{0}  code:{1},date:{2} starting ", count, code, name), "DayKLog");
                        char firstchar = code[0];
                        if (firstchar == '6' || firstchar == '9')
                        {
                            urltype = "sh";
                        }
                        else if (firstchar == '0' || firstchar == '2' || firstchar == '3')
                        {
                            urltype = "sz";
                        }
                        else
                        {
                            LogHelper.ErrorLog("urlcodetype is not sh and sz.  " + code, "DayKLog");
                            return;
                        }
                        string getstr = string.Empty;
                        string url = string.Format(urlformat, urltype, code);
                        if (GetMothed(url, out getstr))
                        {
                            List<DayflowBean> beanList = new List<DayflowBean>();
                            try
                            {
                                MatchCollection mc = reg.Matches(getstr);
                                double? lastEndPrice = 0;
                                bool bCodeExist = false;
                                string maxdate = string.Empty;
                                if (codedateHt.Contains(code))
                                {
                                    bCodeExist = true;
                                    maxdate = codedateHt[code].ToString();
                                }
                                foreach (Match mi in mc)
                                {
                                    if (bCodeExist)
                                    {
                                        if (string.Compare(mi.Groups[2].ToString(), maxdate) < 0)
                                        {                                            
                                            continue;
                                        }
                                        else if (string.Compare(mi.Groups[2].ToString(), maxdate) == 0)
                                        {                                            
                                            lastEndPrice = Convert.ToDouble(mi.Groups[1].ToString().Split(',')[4]);
                                            continue;
                                        }
                                    }                                    
                                    string data = mi.Groups[1].ToString();
                                    string[] strArr = data.Split(',');
                                    DayflowBean dflowBean = new DayflowBean();
                                    dflowBean.Code = code;
                                    dflowBean.Date = strArr[0];
                                    dflowBean.Name = name;
                                    dflowBean.Startprice = Convert.ToDouble(strArr[1]);
                                    dflowBean.Hightprice = Convert.ToDouble(strArr[2]);
                                    dflowBean.Lowprice = Convert.ToDouble(strArr[3]);
                                    dflowBean.Endprice = Convert.ToDouble(strArr[4]);
                                    dflowBean.Tradenum = Convert.ToDouble(strArr[5]);
                                    dflowBean.Summoney = Convert.ToDouble(strArr[6]);
                                    if (lastEndPrice < 0.001)
                                    {
                                        dflowBean.Lastprice = dflowBean.Endprice;
                                    }
                                    else
                                    {
                                        dflowBean.Lastprice = lastEndPrice;
                                    }  
                                    beanList.Add(dflowBean);
                                    lastEndPrice = dflowBean.Endprice;                                    
                                }
                                string eMsg = string.Empty;
                                SqlHelper.InsertByModel(beanList, out eMsg);
                                                               
                                //LogHelper.InfoLog(string.Format("code:{0} ending ", code), "DayKLog");
                            }
                            catch (System.Exception ex)
                            {
                                LogHelper.ErrorLog(string.Format("error  Code:{0},ExceptMsg:{1}", code, ex.Message), "DayKLog");
                            }
                        }
                        else
                        {
                            string eMsg = string.Empty;
                            NeterrBean netbean = new NeterrBean();
                            netbean.Id = Helper.getJrnNo();
                            netbean.Code = code;
                            netbean.Name = name;
                            netbean.Type = "HISDAYLINE";
                            netbean.Remark = url;
                            SqlHelper.InsertByModel(netbean, out eMsg);
                        }
                    });                 
                
            }
            catch (Exception ex)
            {
                string errfile = new System.Diagnostics.StackFrame(true).GetFileName().ToString();
                string errline = new System.Diagnostics.StackFrame(true).GetFileLineNumber().ToString();
                errMsg = errfile + "  " + errline + "  " + ex.StackTrace;
                return false;
            }
            return true;
        }


        ///// <summary>
        ///// 从同花顺上获取所有的日线历史数据
        ///// </summary>
        ///// <param name="errMsg"></param>
        ///// <returns></returns>
        //public bool getDayKHistory(Form1 form,out string errMsg)
        //{
        //    errMsg = string.Empty;
        //    try
        //    {
        //        LogHelper.InfoLog("starttime:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"), "DayKLog");
        //        Hashtable codedateHt = new Hashtable();
        //        if (!getNewestDateForDayKLine(out codedateHt)) 
        //        {
        //            return false;
        //        }
        //        HtmlDocument doc = new HtmlDocument();
        //        string logfile = string.Format("stock_log_{0}.txt", DateTime.Now.ToString("yyyMMdd"));
        //        string errfile = string.Format("stock_errlog_{0}.txt", DateTime.Now.ToString("yyyMMdd"));                
        //        FileOp fileOp = new FileOp();                
        //        StockbaseBean baseBean = new StockbaseBean();
        //        string sqlwhere = @"1=1";
        //        DataSet ds;
        //        SqlHelper.QueryByModel(baseBean, sqlwhere, out ds, out errMsg);
        //        string dateregstr = @".*?(?:=|\|)({0}.*?\|)?";
        //        List<DayflowBean> beanList = new List<DayflowBean>();
        //        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //        {
        //            int count = 0;
        //            foreach (DataRow dr in ds.Tables[0].Rows)
        //            {
        //                form.SetHisBarValue(100*count++/ds.Tables[0].Rows.Count);
        //                string urlformat = @"http://qd.10jqka.com.cn/api.php?year=2014&p=stock_day&fq=&info=k_{0}_{1}";
        //                string urltype = string.Empty;                        
        //                string code = dr["stb_code"].ToString();
        //                string name = dr["stb_name"].ToString();
        //                LogHelper.InfoLog(string.Format("Line:{0}  code:{1},date:{2} starting ,nowtime:{3}", count, code, name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff")), "DayKLog");
        //                char firstchar = code[0];
        //                if (firstchar == '6' || firstchar == '9')
        //                {
        //                    urltype = "sh";
        //                }
        //                else if (firstchar == '0' || firstchar == '2' || firstchar == '3')
        //                {
        //                    urltype = "sz";
        //                }
        //                else 
        //                {
        //                    LogHelper.ErrorLog("urlcodetype is not sh and sz.  " + code, "DayKLog");
        //                    continue;
        //                }
        //                string getstr = string.Empty;
        //                string url = string.Format(urlformat,urltype,code);
        //                if (GetMothed(url, out getstr))
        //                {                            
        //                    try
        //                    {
        //                        string contentstr = string.Empty;
        //                        if (codedateHt.Contains(code))
        //                        {
        //                            Regex reg = new Regex(string.Format(dateregstr, codedateHt[code]));
        //                            string dateExist = reg.Replace(getstr, "$1");   //找到当前最新日期的子串
        //                            if (!string.IsNullOrEmpty(dateExist))
        //                            {
        //                                contentstr = getstr.Substring(getstr.IndexOf(dateExist) + dateExist.Length);
        //                            }
        //                            else
        //                            {
        //                                contentstr = getstr.Substring(getstr.IndexOf('=') + 1);
        //                            }
        //                        }
        //                        else
        //                        {
        //                            contentstr = getstr.Substring(getstr.IndexOf('=') + 1);
        //                        }
        //                        if (string.IsNullOrEmpty(contentstr))
        //                        {
        //                            continue;
        //                        }
        //                        string[] daylist = contentstr.Split('|');                                
        //                        foreach (string data in daylist)
        //                        {
        //                            if (string.IsNullOrEmpty(data))
        //                            {
        //                                continue;
        //                            }
        //                            string[] strArr = data.Split(',');                                    
        //                            DayflowBean dflowBean = new DayflowBean();                                    
        //                            dflowBean.Code = code;                                    
        //                            dflowBean.Date = strArr[0];
        //                            //FileOp.Log(string.Format("code:{0},date:{1} starting",code,dflowBean.Date), dayKHistoryLog);
        //                            //if (SqlHelper.QueryIsNoData(dflowBean, out errMsg))
        //                            //{
        //                            dflowBean.Name = name;
        //                            dflowBean.Startprice = Convert.ToDouble(strArr[1]);
        //                            dflowBean.Hightprice = Convert.ToDouble(strArr[2]);
        //                            dflowBean.Lowprice = Convert.ToDouble(strArr[3]);
        //                            dflowBean.Endprice = Convert.ToDouble(strArr[4]);
        //                            dflowBean.Tradenum = Convert.ToDouble(strArr[5]);
        //                            dflowBean.Summoney = Convert.ToDouble(strArr[6]);
        //                            //SqlHelper.InsertByModel(dflowBean, out errMsg);
        //                            beanList.Add(dflowBean);
                                                                         
        //                        }
        //                        if (beanList.Count > 800)   //800条以上数据时commmit一次
        //                        {
        //                            SqlHelper.InsertByModel(beanList, out errMsg);
        //                            beanList.Clear();
        //                        }                                
        //                    }
        //                    catch (System.Exception ex)
        //                    {
        //                        LogHelper.ErrorLog(string.Format("error   Except:{0},exMsg:", code, ex.StackTrace), "DayKLog");
        //                    }
        //                }                        
        //            }
        //        }
        //        if (beanList.Count > 0)
        //        {
        //            SqlHelper.InsertByModel(beanList, out errMsg);
        //        }  
        //    }
        //    catch (Exception ex)
        //    {
        //        string errfile = new System.Diagnostics.StackFrame(true).GetFileName().ToString();
        //        string errline = new System.Diagnostics.StackFrame(true).GetFileLineNumber().ToString();
        //        errMsg = errfile + "  " + errline + "  " + ex.StackTrace;
        //        return false;
        //    }
        //    return true;
        //}


        /// <summary>
        /// 获取日K线表中的每个股票代码对应的最新日期，结果在hashtable中
        /// </summary>
        /// <param name="codedateHt"></param>
        /// <returns></returns>
        public bool getNewestDateForDayKLine(out Hashtable codedateHt)
        {
            codedateHt = new Hashtable();
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();
                string sqlStr = @"select dfl_code,MAX(dfl_date) as maxdate from dayflow group by dfl_code";
                DataSet ds = dbAccess.ExecuteQuerry(sqlStr);
                
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["dfl_code"].ToString()) && !string.IsNullOrEmpty(dr["maxdate"].ToString()))
                        {
                            codedateHt.Add(dr["dfl_code"].ToString(), dr["maxdate"].ToString());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.ErrorLog("getNewestDateForDayKLine error,errMsg:" + ex.StackTrace,"DayKLog");
                return false;
            }           
            return true;
        }

        /// <summary>
        /// 从同花顺上获取所有的周线历史数据
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool getWeekHistory(out string errMsg)
        {
            errMsg = string.Empty;
            string eMsg = string.Empty;
            try
            {
                LogHelper.InfoLog("week line starting", "DayKLog");
                Hashtable codedateHt = new Hashtable();
                if (!getNewestDateForWeekLine(out codedateHt))
                {
                    return false;
                }                
                int count = 0;
                Regex reg = new Regex(@"(?:=|\|)((\d+).*?)(?=\|)");
                Dictionary<string, string> codeDic = StockHelper.getAllStockCode();
                Parallel.ForEach(codeDic, (dicEntry) =>
                {
                    count++;
                    //form.SetHisBarValue(100 * count++ / ds.Tables[0].Rows.Count);
                    string urlformat = @"http://qd.10jqka.com.cn/api.php?year=2014&p=stock_week&fq=&info=k_{0}_{1}";
                    string urltype = string.Empty;
                    string code = dicEntry.Key; ;
                    string name = dicEntry.Value;
                    LogHelper.InfoLog(string.Format("Line:{0}  code:{1},date:{2} starting ", count, code, name), "DayKLog");
                    char firstchar = code[0];
                    if (firstchar == '6' || firstchar == '9')
                    {
                        urltype = "sh";
                    }
                    else if (firstchar == '0' || firstchar == '2' || firstchar == '3')
                    {
                        urltype = "sz";
                    }
                    else
                    {
                        LogHelper.ErrorLog("urlcodetype is not sh and sz.  " + code, "DayKLog");
                        return;
                    }
                    string getstr = string.Empty;
                    string url = string.Format(urlformat, urltype, code);
                    if (GetMothed(url, out getstr))
                    {
                        List<WeekflowBean> beanList = new List<WeekflowBean>();
                        try
                        {
                            MatchCollection mc = reg.Matches(getstr);
                            double? lastEndPrice = 0;
                            bool bCodeExist = false;
                            string maxdate = string.Empty;
                            if (codedateHt.Contains(code))
                            {
                                bCodeExist = true;
                                maxdate = codedateHt[code].ToString();
                            }
                            foreach (Match mi in mc)
                            {
                                if (bCodeExist)
                                {
                                    if (string.Compare(mi.Groups[2].ToString(), maxdate) < 0)
                                    {
                                        continue;
                                    }
                                    else if (string.Compare(mi.Groups[2].ToString(), maxdate) == 0)
                                    {//周线数据如果相等需要更新表中最后一周的数据
                                        
                                        string tdata = mi.Groups[1].ToString();
                                        string[] tstrArr = tdata.Split(',');
                                        WeekflowBean twflowBean = new WeekflowBean();
                                        twflowBean.Code = code;
                                        twflowBean.Date = tstrArr[0];
                                        twflowBean.Name = name;
                                        twflowBean.Startprice = Convert.ToDouble(tstrArr[1]);
                                        twflowBean.Hightprice = Convert.ToDouble(tstrArr[2]);
                                        twflowBean.Lowprice = Convert.ToDouble(tstrArr[3]);
                                        twflowBean.Endprice = Convert.ToDouble(tstrArr[4]);
                                        twflowBean.Tradenum = Convert.ToDouble(tstrArr[5]);
                                        twflowBean.Summoney = Convert.ToDouble(tstrArr[6]);
                                        lastEndPrice = twflowBean.Endprice;
                                        SqlHelper.UpdateByModel(twflowBean, out eMsg);                                        
                                        continue;
                                    }
                                }
                                string data = mi.Groups[1].ToString();
                                string[] strArr = data.Split(',');
                                WeekflowBean wflowBean = new WeekflowBean();
                                wflowBean.Code = code;
                                wflowBean.Date = strArr[0];
                                wflowBean.Name = name;
                                wflowBean.Startprice = Convert.ToDouble(strArr[1]);
                                wflowBean.Hightprice = Convert.ToDouble(strArr[2]);
                                wflowBean.Lowprice = Convert.ToDouble(strArr[3]);
                                wflowBean.Endprice = Convert.ToDouble(strArr[4]);
                                wflowBean.Tradenum = Convert.ToDouble(strArr[5]);
                                wflowBean.Summoney = Convert.ToDouble(strArr[6]);
                                if (lastEndPrice < 0.001)
                                {
                                    wflowBean.Lastprice = wflowBean.Endprice;
                                }
                                else
                                {
                                    wflowBean.Lastprice = lastEndPrice;
                                }                                
                                beanList.Add(wflowBean);
                                lastEndPrice = wflowBean.Endprice;
                            }
                            
                            SqlHelper.InsertByModel(beanList, out eMsg);
                            
                            //LogHelper.InfoLog(string.Format("code:{0} ending ", code), "DayKLog");
                        }
                        catch (System.Exception ex)
                        {
                            LogHelper.ErrorLog(string.Format("error  Code:{0},ExceptMsg:{1}", code, ex.Message), "DayKLog");
                        }
                    }
                    else
                    {                        
                        NeterrBean netbean = new NeterrBean();
                        netbean.Id = Helper.getJrnNo();
                        netbean.Code = code;
                        netbean.Name = name;
                        netbean.Type = "HISWEEKLINE";
                        netbean.Remark = url;
                        SqlHelper.InsertByModel(netbean, out eMsg);
                    }
                });

            }
            catch (Exception ex)
            {
                string errfile = new System.Diagnostics.StackFrame(true).GetFileName().ToString();
                string errline = new System.Diagnostics.StackFrame(true).GetFileLineNumber().ToString();
                errMsg = errfile + "  " + errline + "  " + ex.StackTrace;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获取周线表中的每个股票代码对应的最新日期，结果在hashtable中
        /// </summary>
        /// <param name="codedateHt"></param>
        /// <returns></returns>
        public bool getNewestDateForWeekLine(out Hashtable codedateHt)
        {
            codedateHt = new Hashtable();
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();
                string sqlStr = @"select wfl_code,MAX(wfl_date) as maxdate from weekflow group by wfl_code";
                DataSet ds = dbAccess.ExecuteQuerry(sqlStr);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["wfl_code"].ToString()) && !string.IsNullOrEmpty(dr["maxdate"].ToString()))
                        {
                            codedateHt.Add(dr["dfl_wode"].ToString(), dr["maxdate"].ToString());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.ErrorLog("getNewestDateForWeekLine error,errMsg:" + ex.StackTrace);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 从同花顺上获取分时数据
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        public bool getMin5History(Form1 form, out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                LogHelper.InfoLog("starttime:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"), "Min5Log");
                Hashtable codedateHt = new Hashtable();
                if (!getNewestDateForMin5KLine(out codedateHt))
                {
                    return false;
                }
                HtmlDocument doc = new HtmlDocument();                
                FileOp fileOp = new FileOp();
                StockbaseBean baseBean = new StockbaseBean();
                string sqlwhere = @"1=1";
                DataSet ds;
                SqlHelper.QueryByModel(baseBean, sqlwhere, out ds, out errMsg);
                List<Min5flowBean> beanList = new List<Min5flowBean>();
                string dateregstr = @".*?(?:=|\|)({0}.*?\|)?";
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    int count = 0;
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        form.SetHisBarValue(100 * count++ / ds.Tables[0].Rows.Count);
                        string urlformat = @"http://qd.10jqka.com.cn/api.php?p=stock_min5&info=k_{0}_{1}&fq=";
                        string urltype = string.Empty;
                        string code = dr["stb_code"].ToString();
                        string name = dr["stb_name"].ToString();
                        LogHelper.InfoLog(string.Format("Line:{0}  code:{1},name:{2} starting. nowtime:{3}", count, code, name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff")), "Min5Log");
                        char firstchar = code[0];
                        if (firstchar == '6' || firstchar == '9')
                        {
                            urltype = "sh";
                        }
                        else if (firstchar == '0' || firstchar == '2' || firstchar == '3')
                        {
                            urltype = "sz";
                        }
                        else
                        {
                            LogHelper.ErrorLog("urlcodetype is not sh and sz.  " + code, "Min5Log");
                            continue;
                        }
                        string getstr = string.Empty;
                        string url = string.Format(urlformat, urltype, code);
                        if (GetMothed(url, out getstr))
                        {
                            try
                            {
                                string getvaluestr = string.Empty;
                                string datestr = @"20141201";
                                if (codedateHt.Contains(code))
                                {
                                    datestr = codedateHt[code].ToString();
                                }
                                Regex reg = new Regex(string.Format(dateregstr, datestr));
                                string dateExist = reg.Replace(getstr, "$1");   //找到当前最新日期的子串
                                if (!string.IsNullOrEmpty(dateExist))
                                {
                                    getvaluestr = getstr.Substring(getstr.IndexOf(dateExist) + dateExist.Length);
                                }
                                else
                                {   
                                    //如果获取的数据里没有预设的1201日的数据，就取指定的1205日的数据。
                                    string date = "20141205";
                                    Regex regin = new Regex(string.Format(@".*?(?:=|\|)(?:({0}.*?)\|)?", date));
                                    getvaluestr = regin.Replace(getstr, "$1");
                                    if (string.IsNullOrEmpty(getvaluestr))
                                    {
                                        continue;
                                    }
                                }
                                string[] dateContentArr = getvaluestr.Split('|');

                                foreach (string dateData in dateContentArr)
                                {
                                    if (string.IsNullOrEmpty(dateData))
                                    {
                                        continue;
                                    }
                                    string[] headAndBody = dateData.Split('~');
                                    if (headAndBody.Length != 2 || string.IsNullOrEmpty(headAndBody[0]) || string.IsNullOrEmpty(headAndBody[1]))
                                    {
                                        continue;
                                    }
                                    string contentstr = headAndBody[1];
                                    string[] min5datalist = contentstr.Split(';');
                                    foreach (string data in min5datalist)
                                    {
                                        if (string.IsNullOrEmpty(data))
                                        {
                                            continue;
                                        }
                                        string[] strArr = data.Split(',');
                                        Min5flowBean m5flowBean = new Min5flowBean();
                                        m5flowBean.Code = code;
                                        m5flowBean.Date = headAndBody[0];
                                        m5flowBean.Time = strArr[6];
                                        m5flowBean.Name = name;
                                        m5flowBean.Startprice = Convert.ToDouble(strArr[0]);
                                        m5flowBean.Hightprice = Convert.ToDouble(strArr[1]);
                                        m5flowBean.Lowprice = Convert.ToDouble(strArr[2]);
                                        m5flowBean.Endprice = Convert.ToDouble(strArr[3]);
                                        m5flowBean.Tradenum = Convert.ToDouble(strArr[4]);
                                        m5flowBean.Summoney = Convert.ToDouble(strArr[5]);
                                        beanList.Add(m5flowBean);
                                    }
                                }
                                
                                if (beanList.Count > 800)   //800条以上数据时commmit一次
                                {                                    
                                    SqlHelper.InsertByModel(beanList, out errMsg);
                                    beanList.Clear();                                    
                                }    
                            }
                            catch (System.Exception ex)
                            {
                                LogHelper.ErrorLog(string.Format("error   Except:{0},exMsg:", code, ex.StackTrace), "Min5Log");
                            }
                        }
                    }
                }
                if (beanList.Count > 0)
                {
                    SqlHelper.InsertByModel(beanList, out errMsg);
                }  
            }
            catch (Exception ex)
            {
                string errfile = new System.Diagnostics.StackFrame(true).GetFileName().ToString();
                string errline = new System.Diagnostics.StackFrame(true).GetFileLineNumber().ToString();
                errMsg = errfile + "  " + errline + "  " + ex.StackTrace;
                return false;
            }
            return true;
        }


        /// <summary>
        /// 获取日K线表中的每个股票代码对应的最新日期，结果在hashtable中
        /// </summary>
        /// <param name="codedateHt"></param>
        /// <returns></returns>
        public bool getNewestDateForMin5KLine(out Hashtable codedateHt)
        {
            codedateHt = new Hashtable();
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();
                string sqlStr = @"select m5f_code,MAX(m5f_date) as maxdate from min5flow group by m5f_code";
                DataSet ds = dbAccess.ExecuteQuerry(sqlStr);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        if (!string.IsNullOrEmpty(dr["m5f_code"].ToString()) && !string.IsNullOrEmpty(dr["maxdate"].ToString()))
                        {
                            codedateHt.Add(dr["m5f_code"].ToString(), dr["maxdate"].ToString());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogHelper.ErrorLog("getNewestDateForDayKLine error,errMsg:" + ex.StackTrace,"Min5Log");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从新浪网上获取所有的股票代码数据。
        /// http://vip.stock.finance.sina.com.cn/q/go.php/vIR_CustomSearch/index.phtml?p={0}
        /// </summary>
        /// <param name="url"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static bool GetMothed(string url, out HtmlDocument doc, int reconnectTimes= 0)
        {
            doc = new HtmlDocument();
            try
            {
                //CookieCollection cl = new CookieCollection();
                //cl.Add(new Cookie("ASP.NET_SessionId", "hvflbg2akylk2fddexyn4rmd"));
                //CookieContainer cookie = new CookieContainer();
                //cookie.Add(new Uri("http://api.chengmi.com"), cl);
                //string strURL = url;
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json;charset=UTF-8";
                //request.CookieContainer = cookie;
                request.Timeout = 100000;
                request.KeepAlive = true;
                //request.Headers.Add("Pragma", "no-cache");
                request.Headers.Add("Accept-Encoding", "gzip");
                /*
                request.ProtocolVersion = HttpVersion.Version10;
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                 */
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream st;
                st = response.GetResponseStream();
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    st = new GZipStream(st, CompressionMode.Decompress);
                }
                //string StrData = "";
                //StreamReader Reader = new StreamReader(st, Encoding.UTF8);
                doc.Load(st, Encoding.GetEncoding("gb2312"));
                //while ((StrData = Reader.ReadLine()) != null)
                //{
                //    result += StrData + "\r\n";
                //}
            }
            catch (System.Exception ex)
            {
                if (reconnectTimes >= 2)
                {
                    LogHelper.ErrorLog("GetMothd Error ,url is:" + url);
                    return false;
                }
                else
                {
                    return GetMothed(url, out doc, ++reconnectTimes);
                } 
            }
            return true;
        }

        /// <summary>
        /// 获取新浪上的日终数据的GET方法
        /// http://hq.sinajs.cn/list=sh...
        /// </summary>
        /// <param name="url"></param>
        /// <param name="resList"></param>
        /// <returns></returns>
        private static bool GetMothed(string url, out ArrayList resList, int reconnectTimes = 0)
        {
            resList = new ArrayList();
            try
            {
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json;charset=UTF-8";
                request.Timeout = 100000;
                request.KeepAlive = true;
                //request.Headers.Add("Accept-Encoding", "gzip,deflate");                
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream st;
                st = response.GetResponseStream();
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    st = new GZipStream(st, CompressionMode.Decompress);
                }
                StreamReader Reader = new StreamReader(st, Encoding.GetEncoding("GBK"));
                string strData = string.Empty;
                while ((strData = Reader.ReadLine()) != null)
                {
                    resList.Add(strData);
                }
            }
            catch (System.Exception ex)
            {
                if (reconnectTimes >= 2)
                {
                    LogHelper.ErrorLog("GetMothd Error ,url is:" + url);
                    return false;
                }
                else
                {
                    return GetMothed(url, out resList, ++reconnectTimes);
                } 
            }
            return true;
        }


        /// <summary>
        /// 获取同花顺的历史数据,分时数据的GET方法
        /// http://qd.10jqka.com.cn/api.php?year=2013,2014&p=stock_day&fq=&info=k_sh_600376
        /// http://qd.10jqka.com.cn/api.php?p=stock_min5&info=k_sz_300377&fq=
        /// </summary>
        /// <param name="url"></param>
        /// <param name="resList"></param>
        /// <returns></returns>
        private static bool GetMothed(string url, out string result,int reconnectTimes = 0)
        {
            result = string.Empty;
            try
            {
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.ContentType = "application/json;charset=UTF-8";
                request.Timeout = 100000;
                request.KeepAlive = true;
                //request.Headers.Add("Accept-Encoding", "gzip,deflate");                
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream st;
                st = response.GetResponseStream();
                if (response.ContentEncoding.ToLower().Contains("gzip"))
                {
                    st = new GZipStream(st, CompressionMode.Decompress);
                }
                StreamReader Reader = new StreamReader(st, Encoding.GetEncoding("GBK"));
                result = Reader.ReadToEnd();
            }
            catch (System.Exception ex)
            {
                //出现异常时再重连二次.
                if (reconnectTimes >= 2)
                {
                    LogHelper.ErrorLog("GetMothd Error ,url is:" + url);
                    return false;
                }
                else
                {
                    return GetMothed(url, out result,++reconnectTimes);
                } 
            }
            return true;
        }


        private static ArrayList getSinaDayLineUrl()
        {
            StockbaseBean baseBean = new StockbaseBean();
            string errMsg = string.Empty;
            string sqlwhere = @"1=1";
            string urlbase = @"http://hq.sinajs.cn/list=";
            ArrayList urlList = new ArrayList();
            DataSet ds;
            SqlHelper.QueryByModel(baseBean, sqlwhere, out ds, out errMsg);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string url = urlbase;
                int count = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string code = dr["stb_code"].ToString();
                    char firstchar = code[0];
                    if (firstchar == '6' || firstchar == '9')
                    {
                        code = "sh" + code;
                        url += code + ",";
                    }
                    else if (firstchar == '0' || firstchar == '2' || firstchar == '3')
                    {
                        code = "sz" + code;
                        url += code + ",";
                    }
                    else
                    {
                        FileOp.Log("error stock code:" + code, dayKLineLogErr);
                    }
                    count++;
                    if (count >= 100)
                    {
                        urlList.Add(url);
                        url = urlbase;
                        count = 0;
                    }
                }
                urlList.Add(url);
            }
            return urlList;
        }

        private bool calcMACDOfDay()
        {
            //double curEndPrice;
            //double curEMA, lastEMA;
            //double a;
            //curEMA = lastEMA * 11/13 + curEndPrice * 2/13;  //12日EMA
            return true;
        }

    }
}
