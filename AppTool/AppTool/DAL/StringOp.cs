using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.PinYinConverter;
using System.Collections.ObjectModel;

namespace DAL
{
    public class StringOp
    {


        private char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };

        /// <summary>
        /// 是否是座机号码形式
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public bool IsTel(string baseStr)
        {
            baseStr = baseStr.Replace(" ", "").Trim();
            baseStr = baseStr.Replace("\r\n", "");
            baseStr = baseStr.Replace("\n", "");
            Regex resultEndRegex = new Regex(@"^[2-90*][0-9*/-]{1,}$");
            Match m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 是否是手机号码形式
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public bool IsMpho(string baseStr)
        {
            Regex resultEndRegex = new Regex(@"^[1][0-9]{10}$");
            Match m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否是姓名形式
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public bool IsName(string baseStr)
        {
             baseStr = baseStr.Replace(" ", "").Trim();
             if (baseStr == "" || baseStr == "姓名")
             {
                 return false;
             }
             if (baseStr.IndexOf("：") > 0 || baseStr.IndexOf(":") > 0)
             {
                 return false;
             }
             if (IsOrg(baseStr))
             {
                 return false;
             }
             Regex resultEndRegex = new Regex(@"^[\u0391-\uFFE5]{2,4}");
             Match m = resultEndRegex.Match(baseStr);
             if (m.Success)
             {
                 return true;
             }
            return false;
        }


        /// <summary>
        /// 是否是英文字段名
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public bool IsEnField(string baseStr)
        {
            baseStr = baseStr.Trim();
            Regex resultEndRegex = new Regex(@"^\w{1}[0-9a-zA-Z_-]{1,}$");
            Match m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 是否是中文字段名
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public bool IsCnField(string baseStr)
        {
            baseStr = baseStr.Trim();
            Regex resultEndRegex = new Regex(@"^[\u0391-\uFFE5]{2,}");
            Match m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 处理成规则表名
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public string GetRegulerTableName(string tableName)
        {
            char[] chs = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.', '．' };
            Regex resultEndRegex = new Regex(@"^[0-9.．]{1,}");
            Match m = resultEndRegex.Match(tableName);
            if (m.Success)
            {
                int unregIndex = tableName.LastIndexOfAny(chs);
                tableName = tableName.Substring(unregIndex+1);
            }
            return tableName.Trim();

        }

        /// <summary>
        /// 是否是职别形式
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public bool IsTitle(string baseStr)
        {
            baseStr = baseStr.Replace(" ", "").Trim();
            Regex resultEndRegex = new Regex(@"[长员编理级任记人家师]$");
            Match m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                if (init.IsHasUsrTitleVal(baseStr))
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 是否是机构形式
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public bool IsOrg(string baseStr)
        {
            baseStr = baseStr.Replace(" ", "").Trim();
            if (IsTitle(baseStr))
            {
                return false;
            }
            if (baseStr.Length < 3)
            {
                return false;
            }
            //含有括号且以处结尾的不认为是机构形式
            if (baseStr.IndexOf('(') >= 0 || baseStr.IndexOf(')') >= 0 || baseStr.IndexOf('（') >= 0 || baseStr.IndexOf('（') >= 0)
            {
                if (baseStr.Contains("处长"))
                {
                    return false;
                }
            }
            if (baseStr.Contains("中心"))
            {
                return true;
            }
            Regex resultEndRegex = new Regex(@"[处部室组办]$");
            Match m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                return true;
            }
            resultEndRegex = new Regex(@"[处部室组办]{1,}");
            m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// 提取机构形式
        /// </summary>
        /// <param name="baseStr"></param>
        /// <returns></returns>
        public string GetOrgStr(string baseStr)
        {
            baseStr = baseStr.Replace(" ", "").Trim();
            Regex resultEndRegex = new Regex(@"[处部室组]$");
            Match m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                return baseStr;
            }
            resultEndRegex = new Regex(@"[处部室组]{1,}");
            m = resultEndRegex.Match(baseStr);
            if (m.Success)
            {
                if (baseStr.IndexOf("组") > 0)
                {
                    baseStr = baseStr.Substring(0, baseStr.IndexOf("组") + 1);
                }
                else if (baseStr.IndexOf("处") > 0)
                {
                  baseStr = baseStr.Substring(0, baseStr.IndexOf("处")+1);
                }
                else if (baseStr.IndexOf("部") > 0)
                {
                    baseStr = baseStr.Substring(0, baseStr.IndexOf("部")+1);
                }
                else if (baseStr.IndexOf("室") > 0)
                {
                    baseStr = baseStr.Substring(0, baseStr.IndexOf("室")+1);
                }
                
               
            }
            return baseStr;
        }


        /// <summary>
        /// 是否全是英文或者数字组成
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        private static bool ValidString(string str)
        {
            str = str.ToUpper();
            for (int i = 0; i < str.Length; i++)
            {
                string ch = str.Substring(i, 1);
                if (ch.CompareTo("A") >= 0 && ch.CompareTo("Z") <= 0)
                {
                    continue;
                }
                if (ch.CompareTo("0") >= 0 && ch.CompareTo("9") <= 0)
                {
                    continue;
                }
                return false;
            }
            return true;
        }


        /// <summary>
        /// 检查字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="name"></param>
        /// <param name="maxLen"></param>
        /// <param name="minLen"></param>
        /// <param name="strErrInfo"></param>
        /// <returns></returns>
        public  bool CheckString(string str, string name,
            int maxLen, int minLen, bool bOnlyLetterNumber, out string strErrInfo)
        {
            strErrInfo = "";
            if (minLen == -1)
            {
                if (string.IsNullOrEmpty(str))
                {
                    return true;
                }
            }
            if (string.IsNullOrEmpty(str))
            {
                str = "";
            }
            str = str.Trim();
            if (minLen > 0)
            {
                if (str.Length == 0)
                {
                    strErrInfo = "[" + name + "]不能为空";
                    return false;
                }
                else if (str.Length < minLen)
                {
                    strErrInfo = "[" + name + "]长度为" + str.Length.ToString() + ",至少为" + minLen.ToString();
                    return false;
                }
            }

            if (maxLen > 0 && str.Length > maxLen)
            {
                strErrInfo = "[" + name + "]长度为" + str.Length.ToString() + ",最多为" + maxLen.ToString();
                return false;
            }

            if (bOnlyLetterNumber)
            {
                if (!ValidString(str))
                {
                    strErrInfo = "[" + name + "]只能是字母或者数字";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 将姓名转换为拼音
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
         public string  TransUsrNamToPY(string name)
        {

            int L = name.Length;
            string Res = "";
            for (int i = 0; i < L; i++)
            {
                char C = name[i];

                if (ChineseChar.IsValidChar(C))
                {
                    ChineseChar CC = new ChineseChar(C);
                    //返回单个汉字拼音的所有集合，包括不同读音
                    ReadOnlyCollection<string> roc = CC.Pinyins;
                    //只获取第一个拼音
                    string Py = CC.Pinyins[0];
                    //去掉后面的数字，只截取拼音
                    Res += Py.Substring(0, Py.Length - 1);
                }
                //不是汉字返回问号
                else { Res += "?,"; }
            }
            return Res;
        }


         /// <summary>
         /// 将姓名转换为所有遇到的拼音
         /// </summary>
         /// <param name="name"></param>
         /// <returns></returns>
         public string TransUsrNamToPYAll(string name)
         {

             int L = name.Length;
             string Res = "";
             for (int i = 0; i < L; i++)
             {
                 char C = name[i];

                 if (ChineseChar.IsValidChar(C))
                 {
                     ChineseChar CC = new ChineseChar(C);
                     //返回单个汉字拼音的所有集合，包括不同读音
                     ReadOnlyCollection<string> roc = CC.Pinyins;
                     //只获取第一个拼音
                //     for (int j = 0; j < CC.Pinyins.Count; j++)
               //      {
                         string Py = CC.Pinyins[0];
                         //去掉后面的数字，只截取拼音
                         Res += Py.Substring(0, Py.Length - 1);
                //     }
                 }
                 //不是汉字返回问号
                 else { Res += C; }
             }
             return Res;
         }


         /// <summary>
         /// 将姓名转换为拼音首字母
         /// </summary>
         /// <param name="name"></param>
         /// <returns></returns>
         public string TransUsrNamToPYFirst(string name)
         {

             int L = name.Length;
             string Res = string.Empty;
             for (int i = 0; i < L; i++)
             {
                 char C = name[i];

                 if (ChineseChar.IsValidChar(C))
                 {
                     ChineseChar CC = new ChineseChar(C);
                     //返回单个汉字拼音的所有集合，包括不同读音
                     ReadOnlyCollection<string> roc = CC.Pinyins;
                     //只获取第一个拼音
                     string Py = CC.Pinyins[0];
                     //去掉后面的数字，只截取拼音
                     Res += Py.Substring(0, 1);
                 }
                 //不是汉字返回问号
                 else { Res += C; }
             }
             return Res;
         }

        

         public string ToHexString(byte[] bytes)
         {
             char[] chars = new char[(bytes.Length) * 2];
             for (int i = 0; i < bytes.Length; i++)
             {
                 int b = bytes[i];
                 chars[i* 2] = hexDigits[b >> 4];
                 chars[i * 2 + 1] = hexDigits[b & 0xF];
             }
             return new string(chars);
         }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
         public string FirstUpper(string str)
         {
             string res = string.Empty;
             for (int i = 0; i < str.Length; i++)
             {
                 if (i == 0)
                 {
                     res = str[i].ToString().ToUpper();
                 }
                 else
                 {
                     res = res + str[i].ToString().ToLower();
                 }

                 
             }
             return res;
         }




    }
}
