using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using System.Data;
using System.Collections;
using System.IO;

namespace BLL
{
    public class JsCombine
    {

        ArrayList notedStartArr = new ArrayList();
        ArrayList notedEndArr = new ArrayList();

        private void BuildNotedArr(ArrayList inArr)
        {
            for (int i = 0; i < inArr.Count; i++)
            {
                if (inArr[i].ToString().Contains("<%--") && !inArr[i].ToString().Contains("--%>") && !inArr[i].ToString().Contains("%>"))
                {
                    notedStartArr.Add(i);
                }
                if (inArr[i].ToString().Contains("<!--") && !inArr[i].ToString().Contains("--!>") && !inArr[i].ToString().Contains("-->"))
                {
                    notedStartArr.Add(i);
                }
                if (inArr[i].ToString().Contains("--%>") && !inArr[i].ToString().Contains("<%--"))
                {
                    notedEndArr.Add(i);
                }
                if (inArr[i].ToString().Contains("-->") && !inArr[i].ToString().Contains("<!--"))
                {
                    notedEndArr.Add(i);
                }
            }
        }

        /// <summary>
        /// 是否被注释
        /// </summary>
        /// <param name="inArr"></param>
        /// <param name="rowNum"></param>
        /// <returns></returns>
        private bool IsNoted(ArrayList inArr, int rowNum)
        {
            string originalStr = inArr[rowNum].ToString().Trim();
            if (originalStr.Contains("<!--"))
            {
                return true;
            }
            if (originalStr.Contains("<%--"))
            {
                return true;
            }
            
           
            //从此行出发向上找最近的"<%--"
            int firstNotedStart = -1;
            int firstNotedEnd = -1;
            for (int j = rowNum-1; j >= 0; j--)
            {
                if (notedEndArr.Contains(j))
                {
                    break;
                }
                if (notedStartArr.Contains(j))
                {
                    firstNotedStart = j;
                    break;
                }
            }
            for (int k = rowNum+1; k < inArr.Count; k++)
            {
                if (notedStartArr.Contains(k))
                {
                    break;
                }
                if (notedEndArr.Contains(k))
                {
                    firstNotedEnd = k;
                    break;
                }
            }
            if (firstNotedStart == -1 || firstNotedEnd == -1)
            {
                return false;
            }
            if (rowNum > firstNotedStart && rowNum < firstNotedEnd)
            {
                return true;
            }
                return false;
        }

        /// <summary>
        /// 是否是一个js的src记录
        /// </summary>
        /// <param name="originalStr"></param>
        /// <returns></returns>
        private bool IsJsSrc(string originalStr)
        {
            
            if (originalStr.Contains("src") && originalStr.Contains("script"))
            {
                
                return true;
            }
            return false;
        }

        /// <summary>
        /// 得到src路径信息
        /// </summary>
        /// <param name="originalStr"></param>
        /// <returns></returns>
        private string GetJsSrc(string originalStr)
        {
            int srcAllStrat = originalStr.IndexOf("src");
            int srcAllEnd = originalStr.IndexOf(">");
            int srcAllLength = srcAllEnd - srcAllStrat - 1;
            string srcAll = originalStr.Substring(srcAllStrat, srcAllLength);
            if (srcAll.IndexOf("'") > 0)
            {
                string[] srcArr = srcAll.Split('\'');
                return srcArr[1].Trim();
            }
            else if (srcAll.IndexOf('"') > 0)
            {
                string[] srcArr = srcAll.Split('"');
                return srcArr[1].Trim();
            }
            else
            {
                return "-1";
            }
            
            
        }

        public string  create(string inFileurl)
        {
            FileOp fileop = new FileOp();
            ArrayList inArr = new ArrayList();
            ArrayList outArr = new ArrayList();
            inArr = fileop.read(inFileurl);
            BuildNotedArr(inArr);
            for (int i = 0; i < inArr.Count; i++)
            {
               string  originalStr = inArr[i].ToString().Trim();
               if (!IsNoted(inArr,i) && IsJsSrc(originalStr))
               {
                   string src = GetJsSrc(originalStr);
                   if (src.Contains(".js"))
                   {
                       int filenameStart = src.LastIndexOf('/') + 1;
                       string filename = src.Substring(filenameStart);
                       string fileurl = src.Substring(0, filenameStart);
                       outArr.Add("{");
                       outArr.Add(" text:'" + filename + "',");
                       outArr.Add(" path:'" + fileurl + "'");
                       outArr.Add("},");
                   }
                   else
                   {
                       outArr.Add("{");
                       outArr.Add(" text:'',");
                       outArr.Add(" path:''");
                       outArr.Add("},");
                   }
               }
            }
           
                string fileURL = "js合并.txt";

              fileop.write(fileURL, outArr);
              return fileURL;

        }







    }
}
