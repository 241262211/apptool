using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLL;
using DAL;
using Model;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using System.Text.RegularExpressions;
using Memcached.ClientLibrary;
using System.Net;


namespace AppTool
{
    public partial class Form1 : Form
    {
        private DataSet tmpWillFindDs;
        private DataSet tmpInsertDS; 
        public static Form1 mainform = null;
        private Form2 pBarForm = null;
        public static MemcachedClient _Memcache = null;

        public Form1()
        {
            InitializeComponent();
            mainform = this;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pBarForm = new Form2();
            pBarForm.Hide();
        }
 
        /// <summary>
        /// 将dataset 保存为excel
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private string Saveds(DataSet ds)
        {
            //获得当前程序存放目录
            string strRoot = AppDomain.CurrentDomain.BaseDirectory;

            //string strFullName = strRoot + @"bin\Config\AMPTask.config";
            string destination = strRoot + @"out";

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);//创建文件夹
            }
            string filename = "查找结果.xls";
            string fileURL = destination + "\\" + filename;
            if (File.Exists(fileURL))
            {
                File.Delete(fileURL);
            }
            ExcelOp excelop = new ExcelOp();

            excelop.DataSetToExcel(ds, fileURL);
            return "0";
        }

        /// <summary>
        /// 建立要查询内容的dataset
        /// </summary>
        /// <returns></returns>
        private string BuildWillFindDs()
        {
            //获得当前程序存放目录
            string strRoot = AppDomain.CurrentDomain.BaseDirectory;

            string destination = strRoot + @"in";

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);//创建文件夹
            }

            string filename = "查找内容.xls";
            string fileURL = destination + "\\" + filename;
            if (!File.Exists(fileURL))
            {
                return "在" + destination + "未找到:" + filename;
            }
            
            ExcelOp excelop = new ExcelOp();
            tmpWillFindDs  = excelop.ExcelAllSheetsToDS(fileURL);
            return "0";
        }

        /// <summary>
        /// 建立生成插入语句的dataset
        /// </summary>
        /// <returns></returns>
        private string BuildInsertDs()
        {
            //获得当前程序存放目录
            string strRoot = AppDomain.CurrentDomain.BaseDirectory;

            string destination = strRoot + @"in";

            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);//创建文件夹
            }

            string filename = "insert.xls";
            string fileURL = destination + "\\" + filename;
            if (!File.Exists(fileURL))
            {
                return "在" + destination + "未找到:" + filename;
            }

            ExcelOp excelop = new ExcelOp();
            tmpInsertDS = excelop.ExcelAllSheetsToDS(fileURL);
            return "0";
        }

  

        /// <summary>
        /// 生成建表语句文档
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void crt_table_btn_Click(object sender, EventArgs e)
        {
            try
            {
                string res = "0";
                //string sysennam = sysEnName_TB.Text;
                //获得数据库文档存放目录

                string strRoot = AppDomain.CurrentDomain.BaseDirectory;
                string strcFilename = "数据结构.doc";
                //string strcDestination = strRoot + @"in" + "\\" + "数据结构";
                //if (!Directory.Exists(strcDestination))
                //{
                //    Directory.CreateDirectory(strcDestination);//创建文件夹
                //}
                string strcfileURL = strRoot+ strcFilename;
                ReadWordToExl readwordtoexcel = new ReadWordToExl();
                if (File.Exists(strcfileURL))
                {
                    res = readwordtoexcel.StrcDocToTxt(strcfileURL);
                }
               
                else
                {
                    MessageBox.Show("待查找文件未找,请把数据源命名为'数据结构.doc'");
                    return;
                }
                if (res != "0")
                {
                    MessageBox.Show(res);
                    return;
                }
                else
                {
                    MessageBox.Show("脚本生成成功");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }



        /// <summary>
        /// 自动生成bean 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CrtBean_btn_Click(object sender, EventArgs e)
        {
            try
            {
                string res = "0";
                //string sysennam = sysEnName_TB.Text;
                //获得数据库文档存放目录

                string strRoot = AppDomain.CurrentDomain.BaseDirectory;
                string strcFilename = "数据结构.doc";
                //string strcDestination = strRoot + @"in" + "\\" + "数据结构";
                //if (!Directory.Exists(strcDestination))
                //{
                //    Directory.CreateDirectory(strcDestination);//创建文件夹
                //}
                string strcfileURL = strRoot + "\\" + strcFilename;
                ReadWordToExl readwordtoexcel = new ReadWordToExl();
                if (File.Exists(strcfileURL))
                {
                    res = readwordtoexcel.StrcDocToBean(strcfileURL);
                }

                else
                {
                    MessageBox.Show("待查找文件未找,请把数据源命名为'数据结构.doc'");
                    return;
                }
                if (res != "0")
                {
                    MessageBox.Show(res);
                    return;
                }
                else
                {
                    MessageBox.Show("脚本生成成功");
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }



        /// <summary>
        /// excel转pdf
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exceltopdf_btn_Click(object sender, EventArgs e)
        {
            try
            {
                string strRoot = AppDomain.CurrentDomain.BaseDirectory;
                string strcFilename = "topdf.xls";
                string strcDestination = strRoot + @"in" + "\\" + "格式转换";
                if (!Directory.Exists(strcDestination))
                {
                    Directory.CreateDirectory(strcDestination);//创建文件夹
                }
                string strcfileURL = strcDestination + "\\" + strcFilename;

                ExcelToPdf exceltopdf = new ExcelToPdf();

                string res = string.Empty;
                   //res = exceltopdf.Save(strcFilename);
                if (res != "0")
                {
                    MessageBox.Show(res);
                }
                MessageBox.Show("转换成功");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 创建cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void crt_cache_btn_Click(object sender, EventArgs e)
        {
            _Memcache = new MemcachedClient();
            string[] serverlist = { "10.233.86.213:11211" };
            //初始化池
            string poolName = "ACMSCache";
            SockIOPool pool = SockIOPool.GetInstance(poolName);
            pool.SetServers(serverlist);
            pool.InitConnections = 1;
            pool.MinConnections = 1;
            pool.MaxConnections = 500;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize();//容器初始化
            _Memcache.PoolName = poolName;
            _Memcache.EnableCompression = false;
            _Memcache.EnableCompression = false;
            _Memcache.Set("wangqiangkf", "王强", System.DateTime.Now.AddMinutes(20));
           // return true;

        }

        /// <summary>
        /// 显示cache
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void show_cache_btn_Click(object sender, EventArgs e)
        {
            _Memcache = new MemcachedClient();
            string[] serverlist = { "10.233.86.213:11211" };
            //初始化池
            string poolName = "ACMSCache";
            SockIOPool pool = SockIOPool.GetInstance(poolName);
            pool.SetServers(serverlist);
            pool.InitConnections = 1;
            pool.MinConnections = 1;
            pool.MaxConnections = 500;
            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;
            pool.MaintenanceSleep = 30;
            pool.Failover = true;
            pool.Nagle = false;
            pool.Initialize();//容器初始化
            _Memcache.PoolName = poolName;
            _Memcache.EnableCompression = false;
            string res = _Memcache.Get("wangqiangkf").ToString();
            MessageBox.Show(res);
        }

        private void crt_znzz_btn_click(object sender, EventArgs e)
        {
            try
            {
                //string res = "0";
                //string sysennam = sysEnName_TB.Text;
                //获得数据库文档存放目录

                string strRoot = AppDomain.CurrentDomain.BaseDirectory;
                string xlsFileName = "中心各处室职能组长数据.xls";
                string outFileName = "znzzdata.sql";

                FileStream xlsstream = new FileStream(xlsFileName, FileMode.Open);
                IWorkbook workbook = new HSSFWorkbook(xlsstream);
                ISheet sheet = workbook.GetSheetAt(0);

               // string txtstr = System.IO.File.ReadAllText(xlsFileName);

                FileStream outfile = new FileStream(outFileName, FileMode.Create);
                StreamWriter sw = new StreamWriter(outfile);
                sw.WriteLine("update USR set usr_headshipIdName = null");
                for (int i = 1; i < sheet.LastRowNum; i++)
                {
                //    string cid = sheet.GetRow(i).GetCell(3).ToString().Trim();
                    string notesid = sheet.GetRow(i).GetCell(0).ToString().Trim();
                    string rolename = sheet.GetRow(i).GetCell(5).ToString().Trim();
                    string sql = string.Format("update USR set usr_headshipIdName = '{0}' where usr_id = '{1}'",rolename,notesid);
                    sw.WriteLine(sql);
                }
                sw.Flush();
                outfile.Close();
                MessageBox.Show("脚本已生成");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void json_sel_file_Click(object sender, EventArgs e)
        {
            try
            {
                string dir = app_file_dir_textbox.Text;                
                string errMsg = string.Empty;
                if (! new PackParse().parseCMJson(dir ,out errMsg))
                {
                    MessageBox.Show(errMsg);
                }
                else
                {
                    MessageBox.Show("insert sql 已生成");
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void cmsniffer_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            if (!new PackParse().snifferCMPacker(this,out errMsg))
            {
                MessageBox.Show(errMsg);                 
            }
            else
            {
                MessageBox.Show("城觅抓包已完成");
            }     
        }

        public void setCMProgressValue(int value)
        {
            this.cmprogress.Value = value;
        }

       
        public static string PostMoths(string url, string param)
        {
            CookieCollection cl = new CookieCollection();
            cl.Add(new Cookie("ASP.NET_SessionId", "hvflbg2akylk2fddexyn4rmd"));
            CookieContainer cookie = new CookieContainer();
            cookie.Add(new Uri("http://api.chengmi.com"), cl);

            string strURL = url;
            System.Net.HttpWebRequest request;
            request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
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
            System.IO.Stream s;
            s = response.GetResponseStream();
            string StrDate = "";
            string strValue = "";
            StreamReader Reader = new StreamReader(s, Encoding.UTF8);
            while ((StrDate = Reader.ReadLine()) != null)
            {
                strValue += StrDate + "\r\n";
            }
            return strValue; 
        }

        private void AllStockCode_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {                
                if (!new StockBll().getAllStockCode(out errMsg))
                {
                    MessageBox.Show(errMsg);
                }
                else
                {
                    MessageBox.Show("股票代码已获取成功");
                }   
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("出错了");
            }
            
        }

        private void DayKLine_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (!new StockBll().getDayKLine(out errMsg))
                {
                    MessageBox.Show(errMsg);
                }
                else
                {
                    MessageBox.Show("DayKLine数据已获取完成");
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("出错了");
            }
        }

        private void HistoryKLine_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (!new StockBll().getDayKHistory(this ,out errMsg))
                {
                    MessageBox.Show(errMsg);
                }
                else
                {
                    MessageBox.Show("DayKHistory数据已获取完成");
                }
            }
            catch
            {
                MessageBox.Show("出错了");
            }
        }

        public void SetHisBarValue(int value)
        {
            this.historydatabar.Value = value;
        }

        private void Min5KLine_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (!new StockBll().getMin5History(this, out errMsg))
                {
                    MessageBox.Show(errMsg);
                }
                else
                {
                    MessageBox.Show("Min5History数据已获取完成");
                }
            }
            catch
            {
                MessageBox.Show("出错了");
            }
        }

        private void test_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (new StockBll().CodeTest())
                {
                    MessageBox.Show("测试完成");
                }
                else
                {
                    MessageBox.Show("测试出错了");
                }
                           
            }
            catch(Exception ex)
            {
                MessageBox.Show("出错了"+ex.Message);
            }
        }

        private void DayMACD_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (new StockIndex().IndexMACD())
                {
                    MessageBox.Show("MACD计算完成");
                }
                else
                {
                    MessageBox.Show("MACD计算出错了");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了" + ex.Message);
            }
        }

        private void DayMACDAnalysis_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (new StockIndex().MACDAnalysis())
                {
                    MessageBox.Show("MACD计算完成");
                }
                else
                {
                    MessageBox.Show("MACD计算出错了");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了" + ex.Message);
            }
        }

        private void DayKDJCalc_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (new StockIndex().IndexKDJ())
                {
                    MessageBox.Show("KDJ计算完成");
                }
                else
                {
                    MessageBox.Show("KDJ计算出错了");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("出错了" + ex.Message);
            }
        }

        private void WeekHisLine_Click(object sender, EventArgs e)
        {
            string errMsg = string.Empty;
            try
            {
                if (!new StockBll().getWeekHistory(out errMsg))
                {
                    MessageBox.Show(errMsg);
                }
                else
                {
                    MessageBox.Show("WeekHistory数据已获取完成");
                }
            }
            catch
            {
                MessageBox.Show("出错了");
            }
        }        
    }

}
