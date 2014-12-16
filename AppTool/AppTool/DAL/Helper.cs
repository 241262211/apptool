using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DAL
{
    public class Helper
    {
        private static int jrncount = 0;
        private static object synJrnObj = new object();

        public static bool IsDataSetValued(DataSet ds)
        {
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 取22位的日志号
        /// </summary>
        /// <returns></returns>
        public static string getJrnNo()
        {
            string jrnno = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            lock (synJrnObj)
            {
                jrnno += jrncount.ToString("D03");
                jrncount++;
                if (jrncount > 999)
                {
                    jrncount = 0;
                }
            }
            return jrnno;
        }
    }
}
