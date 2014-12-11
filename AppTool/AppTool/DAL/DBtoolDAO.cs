using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;

namespace  DAL
{
    public  class DBtoolDAO
    {


        /// <summary>
        /// 返回dataset ,根据查询语句
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet SelSql(string sql)
        {
            DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
            DBOperator dbAccess = dbFactory.GetLocalDBOperator();
            DataSet ds = new DataSet();
            try
            {
                ds = dbAccess.ExecuteQuerry(sql);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            return ds;

        }

        /// <summary>
        /// 多个sql语句加入到数据库中
        /// </summary>
        public string MulSqlToDB(string[] sql)
        {
            string strResult = "";

            string[] result = new string[sql.Length];
            try
            {
                DBFactory dbFactory = DBFactory.GetDBFactoryInstance();
                DBOperator dbAccess = dbFactory.GetLocalDBOperator();
                result = dbAccess.ExecuteNonQuerryInTrans(sql);
                for (int i = 0; i < sql.Length; i++)
                {
                    Regex resultUpdRegex = new Regex(@"^update");
                    Match updm = resultUpdRegex.Match(sql[i]);
                    if (updm.Success)
                    {
                        continue;
                    }
                    Regex resultExRegex = new Regex(@"^[0-9]");
                    Match exm = resultExRegex.Match(sql[i]);
                    if (exm.Success)
                    {
                        continue;
                    }
                    if (result[i] != "1")
                    {
                        strResult = "[XAAU]" + "第" + (i + 1).ToString() + "条批量更新或插入错误。" + result[i].Replace("\n", "") + "引起错误的sql:" + sql[i].Replace("\r\n", " ").Replace("\n", "");
                        return strResult;
                    }
                }
                return "0";
            }
            catch (Exception e)
            {
                strResult = "[XAAU]" + e.Message.Replace("\n", "");
                return strResult;
            }
        }


        /// <summary>
        /// 根据sql ArrayList 插入数据库
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>0-正确；其它-错误的条数</returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20120216    王强      创建代码
        ///----------------------------------------------------------
        public string MulSqlToDB(ArrayList sql)
        {
            //实际没有可供插入的行
            if (sql.Count == 0)
            {
                return "0";
            }
            //实际插入数据库
            string[] arrsql = new string[sql.Count];
            for (int j = 0; j < sql.Count; j++)
            {
                arrsql[j] = sql[j].ToString();
            }

            var strResult = MulSqlToDB(arrsql);
            if (strResult == "0")
            {
                return "0";
            }
            return strResult;
        }


        /// <summary>
        /// 读出sql语句并执行
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string ExeSql(string url)
        {
            try
            {
                FileOp fileop = new FileOp();
                ArrayList sqlArr = fileop.read(url);
                string res = MulSqlToDB(sqlArr);
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }




    }
}
