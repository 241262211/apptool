//----------------------------------------------------------------------------
// Copyright (C) 2011, AGRICULTURAL BANK OF CHINA, Corp. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Collections;
using System.IO;
using System.Data;
using Application = Microsoft.Office.Interop.Word._Application;
using Document = Microsoft.Office.Interop.Word._Document;
using Table = Microsoft.Office.Interop.Word.Table;
using Tables = Microsoft.Office.Interop.Word.Tables;
using Word = Microsoft.Office.Interop.Word;
using DAL;
using Model;
using System.Xml;
using System.Web;

namespace BLL
{
    /// <summary>
    /// 读word
    /// </summary>
    public class ReadWordFile:IDisposable
    {
        /// <summary>
        /// 缺省值
        /// </summary>
        private object missing = System.Reflection.Missing.Value;

        /// <summary>
        /// word的应用
        /// </summary>
        private Application m_cls = null;

        /// <summary>
        /// word文档
        /// </summary>
        private Document m_doc = null;

        /// <summary>
        /// 关闭word
        /// </summary>
        public void Close()
        {
            try
            {
                if (m_cls == null || m_doc == null)
                {
                    return;
                }

                object oSaveChange = false;
                if (m_doc != null)
                {
                    m_doc.Close(ref oSaveChange, ref missing, ref missing);
                }
                m_cls.Quit(ref oSaveChange, ref missing, ref missing);
            }
            catch
            {
            	
            }            
        }

        /// <summary>
        /// 打开word文档
        /// </summary>
        /// <param name="strPath"></param>
        /// <returns></returns>
        public string OpenWord(string strPath)
        {
            m_cls = null;
            m_doc = null;
            object path = strPath;
            try
            {
                m_cls = new Microsoft.Office.Interop.Word.Application();
                if (m_cls != null)
                {
                    object oReadOnly = true;
                    m_doc = m_cls.Documents.Open(ref path, ref missing, ref oReadOnly, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing,
                        ref missing, ref missing, ref missing, ref missing);
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
            if (m_cls == null || m_doc == null)
            {
                return "-1";
            }
            return "0";
        }




        public Application GetCls()
        {
            return m_cls;
        }

        /// <summary>
        /// 是否已经打开了word文档
        /// </summary>
        /// <returns></returns>
        public bool IsOpen()
        {
            return m_cls != null && m_doc != null;
        }

        /// <summary>
        /// 获取word文档中的所有表格
        /// </summary>
        /// <returns></returns>
        public Tables GetAllTables()
        {
            return m_doc.Tables;
        }

        /// <summary>
        /// 获取一个表格之前的一个标签（段落标记）
        /// </summary>
        /// <param name="t1"></param>
        /// <returns></returns>
        public string GetLinePreTable(Table t1)
        {
            // 获取表格之前一行文字            
            //Word.Range range1 = range.GoToPrevious(Microsoft.Office.Interop.Word.WdGoToItem.wdGoToLine);
            //string str = range1.Text;
            
            string str1 = string.Empty;
            lock (m_cls)
            {
                Word.Range range = t1.Range;
                int n1 = range.Start;
                m_cls.Selection.Start = n1 - 100;
                m_cls.Selection.End = n1;
                str1 = m_cls.Selection.Range.Text;
            }            
            char[] chs = { '\r', ' ', '\a', '\t' };
            str1 = str1.Trim(chs).ToUpper();
            /*
            int indexR = str1.IndexOf('\r');
            if (indexR  > 0)
            {
                if (str1.Length - 1 > indexR)//如果回车不是最后一位
                {
                    Word.Style _style;
                    string strNameLocal = "";
                    foreach (Word.Paragraph pa in m_cls.Selection.Range.Paragraphs)
                    {
                        _style = (Word.Style)pa.get_Style();
                        strNameLocal = _style.NameLocal.ToString();
                        if (strNameLocal != "正文" && strNameLocal != "表格文字")
                        {
                            return pa.Range.Text;
                        }
                        
                    }
                }

            }
             */ 
            bool b1 = false;
            for (int i = str1.Length - 1; i >= 0; i--)
            {
                if (!b1)
                {
                    if (str1[i] == '_' || str1[i] == '\r' || str1[i] == '-' || str1[i] == '(' || str1[i] == '（')
                    {
                        b1 = true;
                    }
                }
                else
                {
                    if (str1[i] == '_' || str1[i] == '\t' || str1[i] == ' ')
                    {
                        continue;

                    }

                    if (str1[i] != '\r')
                    {
                        if (i != 0)
                        {
                            continue;
                        }
                    }

                    if (i == 0)
                    {
                        i--;
                    }

                    string strRes = str1.Substring(i + 1);
                    //Console.WriteLine(strRes);
                    strRes = strRes.Trim(chs).ToUpper();
                    return strRes;

                }
            }



            return str1;
            //int nIndex = str1.LastIndexOf('\f');
            //if (nIndex < 0)
            //{
            //    nIndex = str1.LastIndexOf('\r');
            //}
            //if (nIndex < 0)
            //{
            //    return "";
            //}

            //string strRes = str1.Substring(nIndex + 1, str1.Length - nIndex - 2);
            //return strRes;


        }


        /// <summary>
        /// 获取一个表格之前的一行表名下有一行说明，形如： 2.2	银行汇票挂失止付/法院止付登记簿	  IPS
        ///收到挂失止付（解付挂失止付）请求或法院止付（解除法院止付）通知，用于记录的交易流水，记录保留一个月。
        /// </summary>
        /// <param name="t1"></param>
        /// <returns></returns>
        public string GetLinePreTable2(Table t1)
        {
            // 获取表格之前一行文字

            Word.Range range = t1.Range;
            Word.Range range1 = range.GoToPrevious(Microsoft.Office.Interop.Word.WdGoToItem.wdGoToLine);
            string str = range1.Text;
            int n1 = range.Start;
            m_cls.Selection.Start = n1 - 100;
            m_cls.Selection.End = n1;
            string str1 = m_cls.Selection.Range.Text;
            char[] chs = { '\r', ' ', '\a', '\t' };

            if (str1[str1.Length - 1] == '\r' && str1[str1.Length - 2] == '\r')
            {
                str1 = str1.Trim(chs).ToUpper();
                string strRes = GetLinePreTable(t1);
                return strRes;
            }
            if (str1[str1.Length - 1] == '\r' && str1[str1.Length - 2] >= 'A' && str1[str1.Length - 2] <= 'z')
            {
                str1 = str1.Trim(chs).ToUpper();
                string strRes = GetLinePreTable(t1);
                return strRes;
            }
            if (str1[str1.Length - 1] == '\r' && str1[str1.Length - 2] == '：' && str1[str1.Length - 3] == '）')
            {
                str1 = str1.Trim(chs).ToUpper();
                string strRes = GetLinePreTable(t1);
                return strRes;
            }
            if (str1[str1.Length - 1] == '\r' && str1[str1.Length - 2] == ':' && str1[str1.Length - 3] == '）')
            {
                str1 = str1.Trim(chs).ToUpper();
                string strRes = GetLinePreTable(t1);
                return strRes;
            }
            if (str1[str1.Length - 1] == '\r' && str1[str1.Length - 2] == '：' && str1[str1.Length - 3] == ')')
            {
                str1 = str1.Trim(chs).ToUpper();
                string strRes = GetLinePreTable(t1);
                return strRes;
            }
            if (str1[str1.Length - 1] == '\r' && str1[str1.Length - 2] == ':' && str1[str1.Length - 3] == ')')
            {
                str1 = str1.Trim(chs).ToUpper();
                string strRes = GetLinePreTable(t1);
                return strRes;
            }

            if (str1[str1.Length - 1] == '\r' && str1[str1.Length - 2] == ')' && str1[str1.Length - 3] >= 'A' && str1[str1.Length - 3] <= 'z')
            {
                str1 = str1.Trim(chs).ToUpper();
                string strRes = GetLinePreTable(t1);
                return strRes;
            }

            if (str1[str1.Length - 1] == '\r' && str1[str1.Length - 2] == '）' && str1[str1.Length - 3] >= 'A' && str1[str1.Length - 3] <= 'z')
            {
                str1 = str1.Trim(chs).ToUpper();
                string strRes = GetLinePreTable(t1);
                return strRes;
            }


            str1 = str1.Trim(chs).ToUpper();
            if (str1.IndexOf('\r') > 0)
            {
                string[] str1Arr = str1.Split('\r');
                str1 = str1Arr[str1Arr.Length - 2];
            }

            return str1;


        }





        public void Dispose()
        {
            this.Close();
        }

    }
}
