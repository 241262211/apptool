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
using Application = Microsoft.Office.Interop.Word.Application;
using Document = Microsoft.Office.Interop.Word.Document;
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
    /// 读word的类
    /// </summary>
    public class ReadWordToExl
    {
        /// <summary>
        /// 打开文件
        /// </summary>
        private ReadWordFile m_crtOpenFile;

        /// <summary>
        /// 获得一个字
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private string GetText(Word.Cell cell)
        {
            string str = cell.Range.Text.ToString().Trim();
            char[] chs = { '\r', '\a', '\n' };
            str = str.Trim(chs).Replace('\r', ' ');
            return str;

        }


        /// <summary>
        /// 获得根据英文名获得中文名称
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        private bool GetCnName(string strLine, string strEngName, out string strChnName)
        {
            strChnName = string.Empty;
            StringOp stringop = new StringOp();
            if (strLine.Length <= 0)
            {
                return false;
            }
            if (strLine.IndexOf('\r') > 0)
            {
                strLine = strLine.Split('\r')[0];
            }
            char[] chs = { '\r', ' ', '\a', '\t' };
            strLine = strLine.Trim(chs);

            int enindex =  strLine.IndexOf(strEngName);
            string first2 = strLine.Substring(0, 2);
            bool cnNameInLeft = false; 
            char[] trimchar = { ' ', '-', '_', '—', '\f', '', '─', '(', ')', '（', '）', ':', '：' };
            if (enindex >= 0)
            {
                int enlen = strEngName.Length;
                if (stringop.IsCnField(first2))
                {
                    cnNameInLeft = true;
                }
                string cnNameAll = string.Empty;
                if (cnNameInLeft)
                {
                    cnNameAll = strLine.Substring(0, enindex);
                }
                else
                {
                    cnNameAll = strLine.Substring(enindex + enlen);
                }


                strChnName = cnNameAll.Trim().Trim(trimchar);
                return true;
            }
            return false;
        }




        /// <summary>
        /// 获得名称
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        private bool GetName(string strLine, out string strEngName, out string strChnName)
        {
            strEngName = "";
            strChnName = "";

            if (strLine.Length <= 0)
            {
                return false;
            }
            if (strLine.IndexOf('\r') > 0)
            {
                strLine = strLine.Split('\r')[0];
            }

            char[] trimchar = { ' ', '\f', '' };
            strLine = strLine.Trim(trimchar);
            strLine = strLine.Replace('\t', ' ');

            if ((strLine[0] >= 'A' && strLine[0] <= 'Z') || strLine[0] == '_')
            {
                for (int n = 1; n < strLine.Length; n++)
                {
                    if ((strLine[n] >= 'A' && strLine[n] <= 'Z') ||  strLine[n] == '_' )
                    {
                        continue;
                    }
                    strEngName = strLine.Substring(0, n);
                    strChnName = strLine.Substring(n).Trim(trimchar);
                    return true;
                }
            }
            else
            {
                for (int n = strLine.Length - 1; n >= 0; n--)
                {
                    if ((strLine[n] >= 'A' && strLine[n] <= 'Z') || strLine[n] == '_')
                    {
                        continue;
                    }
                    strChnName = strLine.Substring(0, n).Trim(trimchar);
                    strEngName = strLine.Substring(n + 1).Trim();
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        ///  取得的表名为第二种类型ep_depart:部门定义表
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        public bool GetNameType2(string strLine, out string strEngName, out string strChnName)
        {
            strEngName = "";
            strChnName = "";

            if (strLine.Length <= 0)
            {
                return false;
            }

            char[] trimchar = { ' ', '\f', '', '(', ')', '（', '）', ':', '：' };
            strLine = strLine.Trim(trimchar);
            strLine = strLine.Replace('(', ' ');
            strLine = strLine.Replace(')', ' ');
            strLine = strLine.Replace('（', ' ');
            strLine = strLine.Replace('）', ' ');

            if (strLine.IndexOf(':') > 0)
            {
                strEngName = strLine.Split(':')[0];
                strChnName = strLine.Split(':')[1];
                return true;
            }
            else if (strLine.IndexOf('：') > 0)
            {
                strEngName = strLine.Split('：')[0];
                strChnName = strLine.Split('：')[1];
                return true;
            }

            return false;
        }


        /// <summary>
        ///  取得的表名为第三种类型 PRIVILEDGED_BUS(业务权限对照表)
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        public bool GetNameType3(string strLine, out string strEngName, out string strChnName)
        {
            strEngName = "";
            strChnName = "";

            if (strLine.Length <= 0)
            {
                return false;
            }

            char[] trimchar = { ' ', '\f', '', '(', ')', '（', '）', ':', '：' };
            strLine = strLine.Trim(trimchar);

            if (strLine.IndexOf('(') > 0)
            {
                strEngName = strLine.Split('(')[0];
                strChnName = strLine.Split('(')[1];
                return true;
            }
            else if (strLine.IndexOf('（') > 0)
            {
                strEngName = strLine.Split('（')[0];
                strChnName = strLine.Split('（')[1];
                return true;
            }

            return false;
        }


        /// <summary>
        ///  取得的表名为第四种类型 操作员注册表(imoprreg)
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        public bool GetNameType4(string strLine, out string strEngName, out string strChnName)
        {
            strEngName = "";
            strChnName = "";

            if (strLine.Length <= 0)
            {
                return false;
            }

            char[] trimchar = { ' ', '\f', '', '(', ')', '（', '）', ':', '：' };
            strLine = strLine.Trim(trimchar);

            if (strLine.IndexOf('(') > 0)
            {
                strEngName = strLine.Split('(')[1];
                strChnName = strLine.Split('(')[0];
                return true;
            }
            else if (strLine.IndexOf('（') > 0)
            {
                strEngName = strLine.Split('（')[1];
                strChnName = strLine.Split('（')[0];
                return true;
            }

            return false;
        }


        /// <summary>
        ///  取得的表名为第五种类型  汇差记录表  SW_IHHC
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        public bool GetNameType5(string strLine, out string strEngName, out string strChnName)
        {
            strEngName = "";
            strChnName = "";

            if (strLine.Length <= 0)
            {
                return false;
            }

            char[] trimchar = { ' ', '\f', '', '(', ')', '（', '）', ':', '：', '\t' };
            strLine = strLine.Trim(trimchar);
            if (strLine.IndexOf('\t') > 0)
            {
                for (int n = 1; n < strLine.Length; n++)
                {
                    if (strLine[n] != '\t')
                    {
                        continue;
                    }
                    strEngName = strLine.Substring(n).Trim(trimchar);
                    strChnName = strLine.Substring(0, n).Trim(trimchar);

                }
                return true;

            }
            if (strLine.IndexOf(' ') > 0)
            {
                for (int n = 1; n < strLine.Length; n++)
                {
                    if (strLine[n] != ' ')
                    {
                        continue;
                    }
                    strEngName = strLine.Substring(n).Trim(trimchar);
                    strChnName = strLine.Substring(0, n).Trim(trimchar);
                }
                return true;
            }


            return false;
        }


        /// <summary>
        ///  取得的表名为第六种类型  FARS利润表-farsprofit
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        public bool GetNameType6(string strLine, out string strEngName, out string strChnName)
        {
            strEngName = "";
            strChnName = "";

            if (strLine.Length <= 0)
            {
                return false;
            }

            char[] trimchar = { ' ', '\f', '', '(', ')', '（', '）', ':', '：', '\t' };
            strLine = strLine.Trim(trimchar);
            if (strLine.IndexOf('-') > 0)
            {

                strEngName = strLine.Split('-')[1].Trim(trimchar);
                strChnName = strLine.Split('-')[0].Trim(trimchar);
                return true;
            }
            if (strLine.IndexOf('—') > 0)
            {
                strEngName = strLine.Split('—')[1].Trim(trimchar);
                strChnName = strLine.Split('—')[0].Trim(trimchar);
                return true;
            }
            return false;
        }



        /// <summary>
        ///  取得的表名为第七种类型 2Attribute(s) of "c_addrtype" 中文名在另一个表格中
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        public bool GetNameType7(string strLine, out string strEngName, out string strChnName)
        {
            strEngName = "";
            strChnName = "";

            if (strLine.Length <= 0)
            {
                return false;
            }

            char[] trimchar = { ' ', '\f', '', '(', ')', '（', '）', ':', '：', '\t' };
            strLine = strLine.Trim(trimchar);
            if (strLine.IndexOf('"') > 0)
            {

                strEngName = strLine.Split('"')[1].Trim(trimchar);
                if (HttpRuntime.Cache[strEngName] != null)
                {
                    strChnName = HttpRuntime.Cache[strEngName].ToString();
                }

                return true;
            }
            return false;
        }





        /// <summary>
        ///  取得的表名为第八种类型  UT_CMTTJ转发类交易流水表
        /// </summary>
        /// <param name="strLine"></param>
        /// <param name="strEngName"></param>
        /// <param name="strChnName"></param>
        /// <returns></returns>
        public bool GetNameType8(string strLine, out string strEngName, out string strChnName)
        {
            strEngName = "";
            strChnName = "";

            if (strLine.Length <= 0)
            {
                return false;
            }

            char[] trimchar = { ' ', '\f', '', '(', ')', '（', '）', ':', '：', '\t' };

            strLine = strLine.Trim(trimchar);
            if (strLine[0] >= 'A' && strLine[0] <= 'Z')
            {
                for (int n = 1; n < strLine.Length; n++)
                {
                    if (strLine[n] >= 'A' && strLine[n] <= 'Z')
                    {
                        continue;
                    }
                    if (strLine[n] == '_')
                    {
                        continue;
                    }
                    strEngName = strLine.Substring(0, n);
                    strChnName = strLine.Substring(n).Trim(trimchar);
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// 根据table返回英文名所在列
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private int GetEnNamePostion(Table t)
        {
            int res = -1;
            StringOp stringop = new StringOp(); 
            //第一列
            string clm1 = GetText(t.Cell(2, 1));
            //第二列
            string clm2 = GetText(t.Cell(2, 2));
            //第三列
            string clm3 = GetText(t.Cell(2, 3));
            if (stringop.IsEnField(clm1))
            {
                return 1;
            }
            if (stringop.IsEnField(clm2))
            {
                return 2;
            }
            if (stringop.IsEnField(clm3))
            {
                return 3;
            }

            return res;
        }


        /// <summary>
        /// 读前三行,如果前三行中一列匹配全是英文加下划线组合，旁边一列是中文名，返回字段英文名，中文名，备份位置
        /// </summary>
        /// <param name="t"></param>
        /// <param name="enNamePostion"></param>
        /// <param name="cnNamePostion"></param>
        /// <param name="bakPostion"></param>
        /// <returns></returns>
        private string GetEnNameCnNameBakPostion(Table t,out int enNamePostion,out int cnNamePostion,out int bakPostion)
        {
            enNamePostion = -1;
            cnNamePostion = -1;
            bakPostion = -1;
            try{
                int rowCount = t.Rows.Count;
                int clmCount = t.Columns.Count;
                if (rowCount < 3)
                {
                    return "行数少，非数据库表";
                }
                if (clmCount < 4)
                {
                    return "列数少，非数据库表";
                }
                //找英文名
                int enpos = GetEnNamePostion(t);
                if(enpos  == -1)
                {
                    return "未找到英文名列";
                }
                //找中文名

                for (int i = 2; i <= t.Rows.Count; i++)
                {
                    
                }

               return "0";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }





        /// <summary>
        /// 分析类型为0的文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="sysid"></param>
        /// <param name="usrid"></param>
        /// <returns></returns>
        public string ReadDoc(ReadWordFile r, string sysEnName, out  ArrayList DbBeanOutArr)
        {
            string strResult = "0";
            DbBeanOutArr = new ArrayList();

            try
            {

                //读取配置文件
                //数据构结构的表格一共几列
                int strctable_columnNum = Convert.ToInt32(init.Get("Strcfind", "strctable_columnNum"));
                //数据构结构的表格之上几个表格就能找到表名称
                int tableNum_Before_Strctable = Convert.ToInt32(init.Get("Strcfind", "tableNum_Before_Strctable"));
                //表名称分隔符,不需要的添N
                string tableName_split_char = init.Get("Strcfind", "tableName_split_char");
                //英文名称在分隔符的位置L代表左边，R代表右边
                string tableEnName_postion_split = init.Get("Strcfind", "tableEnName_postion_split");
                //找到各列在的数据构结构表格的位置，是在第一列还在第二列，没有的写 -1
                //字段英文名
                int fieldEnName_index = Convert.ToInt32(init.Get("Strcfind", "fieldEnName_index"));
                //字段中文名
                int fieldCnName_index = Convert.ToInt32(init.Get("Strcfind", "fieldCnName_index"));
                //是否主键
                int ispk_index = Convert.ToInt32(init.Get("Strcfind", "ispk_index"));
                //字段类型
                int fieldType_index = Convert.ToInt32(init.Get("Strcfind", "fieldType_index"));
                //字段格式
                int fieldStyle_index = Convert.ToInt32(init.Get("Strcfind", "fieldStyle_index"));
                //字段长度
                int fieldLen_index = Convert.ToInt32(init.Get("Strcfind", "fieldLen_index"));
                //是否为空
                int isNull_index = Convert.ToInt32(init.Get("Strcfind", "isNull_index"));
                //小数点长度
                int decimalLen_index = Convert.ToInt32(init.Get("Strcfind", "decimalLen_index"));
                //取值范围
                int valRange_index = Convert.ToInt32(init.Get("Strcfind", "valRange_index"));
                //备注说明
                int bak_index = Convert.ToInt32(init.Get("Strcfind", "bak_index"));



                // 先获取到所有的表格
                m_crtOpenFile = r;
                Tables ts = r.GetAllTables();
                int realDocCellClm = 0;

                Word.Style _style;
                ArrayList strTextList = new ArrayList();
                string strNameLocal = "";
                foreach (Word.Paragraph pa in r.GetCls().ActiveDocument.Paragraphs)
                {
                    _style = (Word.Style)pa.get_Style();
                    strNameLocal = _style.NameLocal.ToString();

                    switch (strNameLocal)
                    {
                        case "正文":
                            break;
                        case "目录 1":
                            break;
                        case "表头":
                            break;
                        default:
                            strTextList.Add(pa.Range.Text);
                            break;
                    }
                }
                

                // 从1开始的，注意
                for (int i = 1; i <= ts.Count; i++)
                {

                    Table t1 = ts[i];
                    
                    realDocCellClm = t1.Columns.Count;
                    //去掉小于5列的表格
                    if (realDocCellClm < 5 )
                    {
                        continue;
                    }
                    int nRowIndex = 1;


                    //读前三行,如果前三行中一列匹配全是英文加下划线组合，旁边一列是中文名，返回字段英文名，中文名，备份位置


                    //读取表名
                    Table t2 = ts[i - tableNum_Before_Strctable];
                    string strLine = r.GetLinePreTable(t2);
                    if (strLine == null || strLine.Length == 0)
                    {
                        continue;
                    }
                    string strTableEnName = string.Empty;
                    string strTableCnName = string.Empty;
                    if (tableName_split_char == "N")
                    {
                        if (!GetName(strLine, out strTableEnName, out strTableCnName))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (strLine.IndexOf(tableName_split_char) > 0)
                        {
                            if (strLine.IndexOf('\r') > 0)
                            {
                                strLine = strLine.Split('\r')[0];
                            }
                            if (tableEnName_postion_split == "L")
                            {
                                strTableEnName = strLine.Split(tableName_split_char.ToCharArray()[0])[0];
                                strTableCnName = strLine.Split(tableName_split_char.ToCharArray()[0])[1];
                            }
                            else
                            {
                                strTableEnName = strLine.Split(tableName_split_char.ToCharArray()[0])[1];
                                strTableCnName = strLine.Split(tableName_split_char.ToCharArray()[0])[0];
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (string.IsNullOrEmpty(strTableEnName))
                    {
                        continue;
                    }

                    //读取字段名
                    int num = 1;
                    for (int j = nRowIndex + 1; j <= t1.Rows.Count; j++)
                    {
                        DbBean DbBeanOut = new DbBean();
                        DbBeanOut.SysEnName = sysEnName;
                        DbBeanOut.TableEnName = strTableEnName;
                        DbBeanOut.TableCnName = strTableCnName;


                        //英文名
                        if (fieldEnName_index != -1)
                        {
                            DbBeanOut.FieldEnName = GetText(t1.Cell(j, fieldEnName_index));
                        }

                        //中文名
                        if (fieldCnName_index != -1)
                        {
                            DbBeanOut.FieldCnName = GetText(t1.Cell(j, fieldCnName_index));
                        }

                        //序号
                        DbBeanOut.FieldNum = num.ToString();
                        num++;

                        //是否主键
                        if (ispk_index != -1)
                        {
                            DbBeanOut.IsPk = GetText(t1.Cell(j, ispk_index));
                        }

                        //字段类型
                        if (fieldType_index != -1)
                        {
                            DbBeanOut.FieldType = GetText(t1.Cell(j, fieldType_index));
                        }
                        string len = string.Empty;
                        if (DbBeanOut.FieldType.IndexOf('(') > 0)
                        {

                            string leneelse = DbBeanOut.FieldType.Split('(')[1];
                            DbBeanOut.FieldType = DbBeanOut.FieldType.Split('(')[0];
                            if (leneelse.IndexOf(')') > 0)
                            {
                                len = leneelse.Split(')')[0];
                            }
                            if (DbBeanOut.FieldType == "DECIMAL" || DbBeanOut.FieldType == "decimal" || DbBeanOut.FieldType == "numeric" || DbBeanOut.FieldType == "NUMERIC" || DbBeanOut.FieldType == "NUMBER")
                            {
                                if (len.IndexOf(',') > 0)
                                {
                                    string decimalLen = len.Split(',')[1];
                                    len = len.Split(',')[0];
                                    DbBeanOut.DecimalLen = decimalLen;
                                }
                            }
                            if (DbBeanOut.FieldType == "INTEGER" || DbBeanOut.FieldType == "integer" || DbBeanOut.FieldType == "int")
                            {
                                len = "";
                            }

                        }

                        //字段样式
                        if (fieldStyle_index != -1)
                        {
                            DbBeanOut.FieldStyle = GetText(t1.Cell(j, fieldStyle_index));
                        }
                        //字段长度
                        if (fieldLen_index != -1)
                        {
                            DbBeanOut.FieldStyle = GetText(t1.Cell(j, fieldLen_index));
                        }
                        else
                        {
                            DbBeanOut.FieldLen = len;
                        }


                        //是否为空
                        if (isNull_index != -1)
                        {
                            DbBeanOut.IsNull = GetText(t1.Cell(j, isNull_index));
                        }
                        //小数点长度
                        if (decimalLen_index != -1)
                        {
                            DbBeanOut.Bak = GetText(t1.Cell(j, decimalLen_index));
                        }

                        //取值范围
                        if (valRange_index != -1)
                        {
                            DbBeanOut.Bak = GetText(t1.Cell(j, valRange_index));
                        }
                        //备注说明
                        if (bak_index != -1)
                        {
                            DbBeanOut.Bak = GetText(t1.Cell(j, bak_index));
                        }
                        DbBeanOutArr.Add(DbBeanOut);
                    }


                }

                return "0";

            }
            catch (Exception e)
            {
                strResult = e.Message;
                return strResult;
            }


        }

       


        

        /// <summary>
        /// 分析类型为0的文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="sysid"></param>
        /// <param name="usrid"></param>
        /// <returns></returns>
        public string ReadDocType0(ReadWordFile r ,out  ArrayList DbBeanOutArr)
        {
            string strResult = "0";
            DbBeanOutArr = new ArrayList();
            
            try
            {

                //读取配置文件
                //数据构结构的表格一共几列
                int strctable_columnNum = Convert.ToInt32(init.Get("Strcfind", "strctable_columnNum"));
                //数据构结构的表格之上几个表格就能找到表名称
                int  tableNum_Before_Strctable = Convert.ToInt32(init.Get("Strcfind", "tableNum_Before_Strctable"));
                //表名称分隔符,不需要的添N
                string tableName_split_char = init.Get("Strcfind", "tableName_split_char");
                //英文名称在分隔符的位置L代表左边，R代表右边
                string tableEnName_postion_split = init.Get("Strcfind", "tableEnName_postion_split");
                //数据结构文档开始于第几行
                int strcTable_start_row = Convert.ToInt32(init.Get("Strcfind", "strcTable_start_row"));
                //找到各列在的数据构结构表格的位置，是在第一列还在第二列，没有的写 -1
                //字段英文名
                int fieldEnName_index = Convert.ToInt32(init.Get("Strcfind", "fieldEnName_index"));
                //字段中文名
                int fieldCnName_index = Convert.ToInt32(init.Get("Strcfind", "fieldCnName_index"));
                //是否主键
                int ispk_index = Convert.ToInt32(init.Get("Strcfind", "ispk_index"));
                //字段类型
                int fieldType_index = Convert.ToInt32(init.Get("Strcfind", "fieldType_index"));
                //字段格式
                int fieldStyle_index = Convert.ToInt32(init.Get("Strcfind", "fieldStyle_index"));
                //字段长度
                int fieldLen_index = Convert.ToInt32(init.Get("Strcfind", "fieldLen_index"));
                //是否为空
                int isNull_index = Convert.ToInt32(init.Get("Strcfind", "isNull_index"));
                //小数点长度
                int decimalLen_index = Convert.ToInt32(init.Get("Strcfind", "decimalLen_index"));
                //取值范围
                int valRange_index = Convert.ToInt32(init.Get("Strcfind", "valRange_index"));
                //备注说明
                int bak_index = Convert.ToInt32(init.Get("Strcfind", "bak_index"));

                StringOp stringop = new StringOp();

                // 先获取到所有的表格
                m_crtOpenFile = r;
                Tables ts = r.GetAllTables();
                int realDocCellClm = 0;

                // 从1开始的，注意
                for (int i = 1; i <= ts.Count; i++)
                {

                    Table t1 = ts[i];

                    realDocCellClm = t1.Columns.Count;

                    if (realDocCellClm != strctable_columnNum)
                    {
                        continue;
                    }

                    int nRowIndex = 1;

                    //读取表名
                    Table t2 = ts[i - tableNum_Before_Strctable];
                    string strLine = r.GetLinePreTable(t2);
                    //string strLine2 = r.GetLinePreTable2(t2);
                    if (string.IsNullOrEmpty(strLine))
                    {
                        continue;
                    }
                    string strTableEnName = string.Empty;
                    string strTableCnName = string.Empty;
                    if (tableName_split_char == "N")
                    {
                        if (!GetName(strLine, out strTableEnName, out strTableCnName))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        /*
                        if (strLine.IndexOf(tableName_split_char) < 1)
                        {
                            strLine = r.GetLinePreTable(t2);

                            continue;
                        }
                        */
                        if (strLine.IndexOf(tableName_split_char) > 0)
                        {
                            if (strLine.IndexOf('\r') > 0)
                            {
                                strLine = strLine.Split('\r')[0];
                            }
                            if (strLine.IndexOf('（') > 0)
                            {
                                strLine = strLine.Replace('（', '(');
                            }
                            if (strLine.IndexOf('－') > 0)
                            {
                                strLine = strLine.Replace('－', '-');
                            }
                            if (tableEnName_postion_split == "L")
                            {
                                strTableEnName = strLine.Split(tableName_split_char.ToCharArray()[0])[0];
                                strTableCnName = strLine.Split(tableName_split_char.ToCharArray()[0])[1];
                            }
                            else
                            {
                                strTableEnName = strLine.Split(tableName_split_char.ToCharArray()[0])[1];
                                strTableCnName = strLine.Split(tableName_split_char.ToCharArray()[0])[0];
                            }
                        }
                        else
                        {

                            continue;
                        }
                    }
                    if (string.IsNullOrEmpty(strTableEnName))
                    {
                        continue;
                    }
                    
                    //读取字段名
                    //int num = 1;
                    for (int j = strcTable_start_row + 1; j <= t1.Rows.Count; j++)
                    {
                        DbBean DbBeanOut = new DbBean();
                        DbBeanOut.TableEnName = strTableEnName.Trim();
                        DbBeanOut.TableCnName = stringop.GetRegulerTableName(strTableCnName).Trim();

                       
                        //英文名
                        if (fieldEnName_index != -1)
                        {
                            try
                            {
                                DbBeanOut.FieldEnName = GetText(t1.Cell(j, fieldEnName_index)).Trim();
                            }
                            catch(Exception cellEnex)
                            {
                                continue;
                            }
                            
                        }

                        //中文名
                        if (fieldCnName_index != -1)
                        {
                            try
                            {
                                DbBeanOut.FieldCnName = GetText(t1.Cell(j, fieldCnName_index)).Trim();
                            }
                            catch (Exception cellCnex)
                            {
                                continue;
                            }
                        }
                        /*
                        //序号
                        DbBeanOut.FieldNum = num.ToString();
                        num++;

                        //是否主键
                        if (ispk_index != -1)
                        {
                            DbBeanOut.IsPk = GetText(t1.Cell(j, ispk_index));
                        }

                        //字段类型
                        if (fieldType_index != -1)
                        {
                            DbBeanOut.FieldType = GetText(t1.Cell(j, fieldType_index));
                        }
                        string len = string.Empty;
                        if (DbBeanOut.FieldType.IndexOf('(') > 0)
                        {
                            
                            string leneelse = DbBeanOut.FieldType.Split('(')[1];
                            DbBeanOut.FieldType = DbBeanOut.FieldType.Split('(')[0];
                            if (leneelse.IndexOf(')') > 0)
                            {
                                len = leneelse.Split(')')[0];
                            }
                            if (DbBeanOut.FieldType == "DECIMAL" || DbBeanOut.FieldType == "decimal" || DbBeanOut.FieldType == "numeric" || DbBeanOut.FieldType == "NUMERIC" || DbBeanOut.FieldType == "NUMBER")
                            {
                                if(len.IndexOf(',') > 0)
                                {
                                    string decimalLen = len.Split(',')[1];
                                    len = len.Split(',')[0];
                                    DbBeanOut.DecimalLen = decimalLen;
                                }
                            }
                            if (DbBeanOut.FieldType == "INTEGER" || DbBeanOut.FieldType == "integer" || DbBeanOut.FieldType == "int")
                            {
                                len = "";
                            }

                        }

                        //字段样式
                        if (fieldStyle_index != -1)
                        {
                            DbBeanOut.FieldStyle = GetText(t1.Cell(j, fieldStyle_index));
                        }
                        //字段长度
                        if (fieldLen_index != -1)
                        {
                            DbBeanOut.FieldStyle = GetText(t1.Cell(j, fieldLen_index));
                        }
                        else
                        {
                            DbBeanOut.FieldLen = len;
                        }
                        

                        //是否为空
                        if (isNull_index != -1)
                        {
                            DbBeanOut.IsNull = GetText(t1.Cell(j, isNull_index));
                        }
                        //小数点长度
                        if (decimalLen_index != -1)
                        {
                            DbBeanOut.Bak = GetText(t1.Cell(j, decimalLen_index));
                        }

                        //取值范围
                        if (valRange_index != -1)
                        {
                            DbBeanOut.Bak = GetText(t1.Cell(j, valRange_index));
                        }
                         */ 
                        //备注说明
                        if (bak_index != -1)
                        {
                            try
                            {
                                DbBeanOut.Bak = GetText(t1.Cell(j, bak_index)).Trim();
                            }
                            catch(Exception bakex)
                            {
                                continue;
                            }
                        }
                        DbBeanOutArr.Add(DbBeanOut);
                    }


                }

                return "0";

            }
            catch (Exception e)
            {
                strResult = e.Message;
                return strResult;
            }
           

        }


        public string ReadDocTypeACMS(ReadWordFile r, out  ArrayList DbBeanOutArr)
        {
            string strResult = "0";
            DbBeanOutArr = new ArrayList();

            try
            {

                //读取配置文件
                //数据构结构的表格一共几列
                int strctable_columnNum = Convert.ToInt32(init.Get("Strcfind", "strctable_columnNum"));
                //数据构结构的表格之上几个表格就能找到表名称
                int tableNum_Before_Strctable = Convert.ToInt32(init.Get("Strcfind", "tableNum_Before_Strctable"));
                //表名称分隔符,不需要的添N
                string tableName_split_char = init.Get("Strcfind", "tableName_split_char");
                //英文名称在分隔符的位置L代表左边，R代表右边
                string tableEnName_postion_split = init.Get("Strcfind", "tableEnName_postion_split");
                //数据结构文档开始于第几行
                int strcTable_start_row = Convert.ToInt32(init.Get("Strcfind", "strcTable_start_row"));
                //找到各列在的数据构结构表格的位置，是在第一列还在第二列，没有的写 -1
                //字段英文名
                int fieldEnName_index = Convert.ToInt32(init.Get("Strcfind", "fieldEnName_index"));
                //字段中文名
                int fieldCnName_index = Convert.ToInt32(init.Get("Strcfind", "fieldCnName_index"));
                //是否主键
                int ispk_index = Convert.ToInt32(init.Get("Strcfind", "ispk_index"));
                //字段类型
                int fieldType_index = Convert.ToInt32(init.Get("Strcfind", "fieldType_index"));
                //字段格式
                int fieldStyle_index = Convert.ToInt32(init.Get("Strcfind", "fieldStyle_index"));
                //字段长度
                int fieldLen_index = Convert.ToInt32(init.Get("Strcfind", "fieldLen_index"));
                //是否为空
                int isNull_index = Convert.ToInt32(init.Get("Strcfind", "isNull_index"));
                //小数点长度
                int decimalLen_index = Convert.ToInt32(init.Get("Strcfind", "decimalLen_index"));
                //取值范围
                int valRange_index = Convert.ToInt32(init.Get("Strcfind", "valRange_index"));
                //备注说明
                int bak_index = Convert.ToInt32(init.Get("Strcfind", "bak_index"));

                StringOp stringop = new StringOp();

                // 先获取到所有的表格
                m_crtOpenFile = r;
                Tables ts = r.GetAllTables();
                int realDocCellClm = 0;

                // 从1开始的，注意
                for (int i = 1; i <= ts.Count; i++)
                {

                    Table t1 = ts[i];

                    realDocCellClm = t1.Columns.Count;

                    if (realDocCellClm != strctable_columnNum)
                    {
                        continue;
                    }

                    int nRowIndex = 1;

                    //读取表名
                    Table t2 = ts[i - tableNum_Before_Strctable];
                    string strLine = r.GetLinePreTable(t2);
                    //string strLine2 = r.GetLinePreTable2(t2);
                    if (string.IsNullOrEmpty(strLine))
                    {
                        continue;
                    }
                    string strTableEnName = string.Empty;
                    string strTableCnName = string.Empty;
                    if (tableName_split_char == "N")
                    {
                        if (!GetName(strLine, out strTableEnName, out strTableCnName))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        /*
                        if (strLine.IndexOf(tableName_split_char) < 1)
                        {
                            strLine = r.GetLinePreTable(t2);

                            continue;
                        }
                        */
                        if (strLine.IndexOf(tableName_split_char) > 0)
                        {
                            if (strLine.IndexOf('\r') > 0)
                            {
                                strLine = strLine.Split('\r')[0];
                            }
                            if (strLine.IndexOf('（') > 0)
                            {
                                strLine = strLine.Replace('（', '(');
                            }
                            if (strLine.IndexOf('－') > 0)
                            {
                                strLine = strLine.Replace('－', '-');
                            }
                            if (tableEnName_postion_split == "L")
                            {
                                strTableEnName = strLine.Split(tableName_split_char.ToCharArray()[0])[0];
                                strTableCnName = strLine.Split(tableName_split_char.ToCharArray()[0])[1];
                            }
                            else
                            {
                                strTableEnName = strLine.Split(tableName_split_char.ToCharArray()[0])[1];
                                strTableCnName = strLine.Split(tableName_split_char.ToCharArray()[0])[0];
                            }
                        }
                        else
                        {

                            continue;
                        }
                    }
                    if (string.IsNullOrEmpty(strTableEnName))
                    {
                        continue;
                    }

                    //读取字段名
                    //int num = 1;
                    for (int j = strcTable_start_row + 1; j <= t1.Rows.Count; j++)
                    {
                        DbBean DbBeanOut = new DbBean();
                        DbBeanOut.TableEnName = strTableEnName.Trim();
                        DbBeanOut.TableCnName = stringop.GetRegulerTableName(strTableCnName).Trim();
                        if (strTableCnName.IndexOf('.') > 0)
                        {
                            DbBeanOut.Bak = strTableCnName.Split('.')[0];
                        }
                        

                        //英文名
                        if (fieldEnName_index != -1)
                        {
                            try
                            {
                                DbBeanOut.FieldEnName = GetText(t1.Cell(j, fieldEnName_index)).Trim();
                            }
                            catch (Exception cellEnex)
                            {
                                continue;
                            }

                        }

                        //中文名
                        if (fieldCnName_index != -1)
                        {
                            try
                            {
                                DbBeanOut.FieldCnName = GetText(t1.Cell(j, fieldCnName_index)).Trim();
                            }
                            catch (Exception cellCnex)
                            {
                                continue;
                            }
                        }
                        //是否主键
                        if (ispk_index != -1)
                        {
                            DbBeanOut.IsPk = GetText(t1.Cell(j, ispk_index));
                        }

                        //字段类型
                        if (fieldType_index != -1)
                        {
                            DbBeanOut.FieldType = GetText(t1.Cell(j, fieldType_index));
                        }

                        /*
                        //序号
                        DbBeanOut.FieldNum = num.ToString();
                        num++;

                        //是否主键
                        if (ispk_index != -1)
                        {
                            DbBeanOut.IsPk = GetText(t1.Cell(j, ispk_index));
                        }

                        //字段类型
                        if (fieldType_index != -1)
                        {
                            DbBeanOut.FieldType = GetText(t1.Cell(j, fieldType_index));
                        }
                        string len = string.Empty;
                        if (DbBeanOut.FieldType.IndexOf('(') > 0)
                        {
                            
                            string leneelse = DbBeanOut.FieldType.Split('(')[1];
                            DbBeanOut.FieldType = DbBeanOut.FieldType.Split('(')[0];
                            if (leneelse.IndexOf(')') > 0)
                            {
                                len = leneelse.Split(')')[0];
                            }
                            if (DbBeanOut.FieldType == "DECIMAL" || DbBeanOut.FieldType == "decimal" || DbBeanOut.FieldType == "numeric" || DbBeanOut.FieldType == "NUMERIC" || DbBeanOut.FieldType == "NUMBER")
                            {
                                if(len.IndexOf(',') > 0)
                                {
                                    string decimalLen = len.Split(',')[1];
                                    len = len.Split(',')[0];
                                    DbBeanOut.DecimalLen = decimalLen;
                                }
                            }
                            if (DbBeanOut.FieldType == "INTEGER" || DbBeanOut.FieldType == "integer" || DbBeanOut.FieldType == "int")
                            {
                                len = "";
                            }

                        }

                        //字段样式
                        if (fieldStyle_index != -1)
                        {
                            DbBeanOut.FieldStyle = GetText(t1.Cell(j, fieldStyle_index));
                        }
                        //字段长度
                        if (fieldLen_index != -1)
                        {
                            DbBeanOut.FieldStyle = GetText(t1.Cell(j, fieldLen_index));
                        }
                        else
                        {
                            DbBeanOut.FieldLen = len;
                        }
                        

                        //是否为空
                        if (isNull_index != -1)
                        {
                            DbBeanOut.IsNull = GetText(t1.Cell(j, isNull_index));
                        }
                        //小数点长度
                        if (decimalLen_index != -1)
                        {
                            DbBeanOut.Bak = GetText(t1.Cell(j, decimalLen_index));
                        }

                        //取值范围
                        if (valRange_index != -1)
                        {
                            DbBeanOut.Bak = GetText(t1.Cell(j, valRange_index));
                        }
                         */
                        //备注说明
                        if (bak_index != -1)
                        {
                            try
                            {
                                DbBeanOut.Bak = GetText(t1.Cell(j, bak_index)).Trim();
                            }
                            catch (Exception bakex)
                            {
                                continue;
                            }
                        }
                        DbBeanOutArr.Add(DbBeanOut);
                    }


                }

                return "0";

            }
            catch (Exception e)
            {
                strResult = e.Message;
                return strResult;
            }


        }



        /// <summary>
        /// 分析类型为0的文件
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="sysid"></param>
        /// <param name="usrid"></param>
        /// <returns></returns>
        public string ReadXLSType(DataSet strcDS, out  ArrayList DbBeanOutArr)
        {
            string strResult = "0";
            DbBeanOutArr = new ArrayList();

            try
            {

                //读取配置文件
                //数据构结构的表格一共几列
                int strctable_columnNum = Convert.ToInt32(init.Get("Strcfind", "strctable_columnNum"));
                //数据构结构的表格之上几个表格就能找到表名称
                int tableNum_Before_Strctable = Convert.ToInt32(init.Get("Strcfind", "tableNum_Before_Strctable"));
                //表名称分隔符,不需要的添N
                string tableName_split_char = init.Get("Strcfind", "tableName_split_char");
                //英文名称在分隔符的位置L代表左边，R代表右边
                string tableEnName_postion_split = init.Get("Strcfind", "tableEnName_postion_split");
                //英文名称所在列
                int tableEnName_index =Convert.ToInt32(init.Get("Strcfind", "tableEnName_index"));
                //中名称所在列
                int tableCnName_index =Convert.ToInt32(init.Get("Strcfind", "tableCnName_index"));
                //数据结构文档开始于第几行
                int strcTable_start_row = Convert.ToInt32(init.Get("Strcfind", "strcTable_start_row"));
                //找到各列在的数据构结构表格的位置，是在第一列还在第二列，没有的写 -1
                //字段英文名
                int fieldEnName_index = Convert.ToInt32(init.Get("Strcfind", "fieldEnName_index"));
                //字段中文名
                int fieldCnName_index = Convert.ToInt32(init.Get("Strcfind", "fieldCnName_index"));
                //是否主键
                int ispk_index = Convert.ToInt32(init.Get("Strcfind", "ispk_index"));
                //字段类型
                int fieldType_index = Convert.ToInt32(init.Get("Strcfind", "fieldType_index"));
                //字段格式
                int fieldStyle_index = Convert.ToInt32(init.Get("Strcfind", "fieldStyle_index"));
                //字段长度
                int fieldLen_index = Convert.ToInt32(init.Get("Strcfind", "fieldLen_index"));
                //是否为空
                int isNull_index = Convert.ToInt32(init.Get("Strcfind", "isNull_index"));
                //小数点长度
                int decimalLen_index = Convert.ToInt32(init.Get("Strcfind", "decimalLen_index"));
                //取值范围
                int valRange_index = Convert.ToInt32(init.Get("Strcfind", "valRange_index"));
                //备注说明
                int bak_index = Convert.ToInt32(init.Get("Strcfind", "bak_index"));

                StringOp stringop = new StringOp();


                if (strcDS == null)
                {
                    return "未找到文件";
                }
                if (strcDS.Tables.Count < 1)
                {
                    return "未找到文件";
                }
                if (strcDS.Tables[0].Rows.Count < 1)
                {
                    return "数据源为空";
                }
                for (int i = 0; i < strcDS.Tables.Count; i++)
                {
                    foreach (DataRow dr in strcDS.Tables[i].Rows)
                    {
                        DbBean DbBeanOut = new DbBean();
                        //英文表名
                        if (tableEnName_index != -1)
                        {
                            try
                            {
                                DbBeanOut.TableEnName = dr[tableEnName_index - 1].ToString().Trim();
                            }
                            catch (Exception tbenex)
                            {
                                continue;
                            }
                        }
                        //中文表明
                        if (tableCnName_index != -1)
                        {
                            try
                            {
                                DbBeanOut.TableCnName = dr[tableCnName_index - 1].ToString().Trim();
                            }
                            catch (Exception tbcnex)
                            {
                                continue;
                            }
                        }
                        //英文名
                        if (fieldEnName_index != -1)
                        {
                            try
                            {
                                DbBeanOut.FieldEnName = dr[fieldEnName_index - 1].ToString().Trim();
                            }
                            catch (Exception cellEnex)
                            {
                                continue;
                            }

                        }

                        //中文名
                        if (fieldCnName_index != -1)
                        {
                            try
                            {
                                DbBeanOut.FieldCnName = dr[fieldCnName_index - 1].ToString().Trim();
                            }
                            catch (Exception cellCnex)
                            {
                                continue;
                            }
                        }
                        //备注说明
                        if (bak_index != -1)
                        {
                            try
                            {
                                DbBeanOut.Bak = dr[bak_index - 1].ToString().Trim();
                            }
                            catch (Exception bakex)
                            {
                                continue;
                            }
                        }

                        DbBeanOutArr.Add(DbBeanOut);

                    }
                }
                return "0";

            }
            catch (Exception e)
            {
                strResult = e.Message;
                return strResult;
            }


        }


        /// <summary>
        /// 在dbbean的Arraylist查找要找的findTableEnName
        /// </summary>
        /// <param name="findTableEnName"></param>
        /// <param name="findDocDBbeanArr"></param>
        /// <param name="dbbeanArr"></param>
        /// <returns></returns>
        private string findTableFromDBArr(string findTableEnName, ArrayList findDocDBbeanArr, out ArrayList dbbeanArr)
        {
            dbbeanArr = new ArrayList();
            bool hasfind = false;
            for (int i = 0; i < findDocDBbeanArr.Count; i++)
            {
                DbBean dbbean = new DbBean();
                dbbean=  (DbBean)findDocDBbeanArr[i];
                if (dbbean.TableEnName == findTableEnName)
                {
                    hasfind = true;
                    DbBean findDbbean = new DbBean();
                    findDbbean = dbbean;
                    dbbeanArr.Add(dbbean);
                }
                if (hasfind)
                {
                    if (dbbean.TableEnName != findTableEnName)
                    {
                        return "0";
                    }
                }
            }
                return "0";
        }


        /// <summary>
        /// 从DBbeanArr中找
        /// </summary>
        /// <param name="enTableName"></param>
        /// <param name="DBbeanArr"></param>
        /// <returns></returns>
        private string FindEnTableNameInDS(string enTableName, DataSet ds, out  DataRow findDR)
        {
            
            findDR = ds.Tables[0].NewRow();
            try
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (string.Equals(enTableName, dr["字段名"].ToString().Trim()))
                    {
                        findDR = dr;
                        return "0";
                    }
                }
                return "-1";
                    
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// 在DBbeanArr中找字段中文名
        /// </summary>
        /// <param name="enTableName"></param>
        /// <param name="DBbeanArr"></param>
        /// <param name="findDBbean"></param>
        /// <returns></returns>
        private string FindEnTableNameInDBBeanArr(string enFieldName,string enTableName,ArrayList DBbeanArr, out  DbBean findDBbean)
        {

            findDBbean = new DbBean();
            try
            {
                enFieldName = enFieldName.ToUpper();
                enTableName = enTableName.ToUpper();
                bool findField = false;
                bool findTable = false;
                string findFieldCnName = string.Empty;
                string findTableCnName = string.Empty;

                for (int i = 0; i < DBbeanArr.Count; i++)
                {
                    if (string.Equals(enFieldName, ((DbBean)DBbeanArr[i]).FieldEnName.ToUpper().Replace(" ","")) && string.Equals(enTableName, ((DbBean)DBbeanArr[i]).TableEnName.ToUpper()))
                    {
                        
                        findDBbean = (DbBean)DBbeanArr[i];
                        findField = true;
                        findTable = true;
                        return "0";
                    }
                    else if (string.Equals(enFieldName, ((DbBean)DBbeanArr[i]).FieldEnName.ToUpper().Replace(" ", "")) && !string.Equals(enTableName, ((DbBean)DBbeanArr[i]).TableEnName.ToUpper()))
                    {
                        findField = true;
                        findTable = false;
                        findFieldCnName = ((DbBean)DBbeanArr[i]).FieldCnName;
                    }
                    else if (!string.Equals(enFieldName, ((DbBean)DBbeanArr[i]).FieldEnName.ToUpper().Replace(" ", "")) && string.Equals(enTableName, ((DbBean)DBbeanArr[i]).TableEnName.ToUpper()))
                    {
                        findField = false;
                        findTable = true;
                        findTableCnName = ((DbBean)DBbeanArr[i]).TableCnName;
                    }
                    
                }
                if (findField == true && findTable == false)
                {
                    return "表名未找到，找到的字段名为:" + findFieldCnName;
                }
                else if (findField == false && findTable == true)
                {
                    return "字段名未找到，找到的表名为:" + findTableCnName;
                }
                return "-1";

            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// word文档分析并插入知识库分类型入口
        /// </summary>
        /// <param name="savePath"></param>
        /// <param name="sysid"></param>
        /// <param name="usrid"></param>
        /// <returns></returns>
        public string StrcDocToExl(string strcWordPath, string tableNamePath, string outfileURL, string outFindfileURL)
        {
            try
            {
                string strResult = "";
                ReadWordFile r = new ReadWordFile();
                strResult = r.OpenWord(strcWordPath);
                m_crtOpenFile = r;
                if (strResult != "0")
                {
                    strResult = "数据库结构文档路径错误";
                    return strResult;
                }
                //获取excelds
                ExcelOp excelop = new ExcelOp();
                DataSet excds = excelop.ExcelToDS(tableNamePath);
                string findTableEnName = string.Empty;
                ArrayList dbBeanArr = new ArrayList();
                ArrayList findDocDBbeanArr = new ArrayList();
                //将doc中的数据全部读出
                strResult = ReadDocType0(r, out findDocDBbeanArr);
                if (strResult != "0")
                {
                    if (m_crtOpenFile != null)
                    {
                        m_crtOpenFile.Close();
                    }
                    return strResult;
                }
                //把dbbean的ArrayList写人dataset
                DataSet newExcds = new DataSet();
                DataTable objTable = new DataTable("数据");
                objTable.Columns.Add("结果", typeof(string));
                objTable.Columns.Add("系统", typeof(string));
                objTable.Columns.Add("表名", typeof(string));
                objTable.Columns.Add("表中文名", typeof(string));
                objTable.Columns.Add("字段名", typeof(string));
                objTable.Columns.Add("字段序号", typeof(string));
                objTable.Columns.Add("是否主键", typeof(string));
                objTable.Columns.Add("字段中文描述", typeof(string));
                objTable.Columns.Add("字段类型", typeof(string));
                objTable.Columns.Add("字段长度", typeof(string));
                objTable.Columns.Add("是否为空", typeof(string));
                objTable.Columns.Add("小数点长度", typeof(string));
                objTable.Columns.Add("取值范围", typeof(string));
                objTable.Columns.Add("备注/说明", typeof(string));
                newExcds.Tables.Add(objTable);

                //找到的结果形成excel
                DataSet findExcds = new DataSet();
                DataTable findobjTable = new DataTable("数据");
                findobjTable.Columns.Add("系统", typeof(string));
                findobjTable.Columns.Add("表名", typeof(string));
                findobjTable.Columns.Add("表中文名", typeof(string));
                findobjTable.Columns.Add("字段名", typeof(string));
                findobjTable.Columns.Add("字段序号", typeof(string));
                findobjTable.Columns.Add("是否主键", typeof(string));
                findobjTable.Columns.Add("字段中文描述", typeof(string));
                findobjTable.Columns.Add("字段类型", typeof(string));
                findobjTable.Columns.Add("字段长度", typeof(string));
                findobjTable.Columns.Add("是否为空", typeof(string));
                findobjTable.Columns.Add("小数点长度", typeof(string));
                findobjTable.Columns.Add("取值范围", typeof(string));
                findobjTable.Columns.Add("备注/说明", typeof(string));
                findExcds.Tables.Add(findobjTable);
                foreach(DataRow dr in excds.Tables[0].Rows)
                {
                   DbBean findDBbean = new DbBean();
                   DataRow newdr = objTable.NewRow();
                   newdr["结果"] = "";
                   newdr["系统"] = dr["系统"].ToString();
                   newdr["表名"] = dr["表名"].ToString();
                   newdr["表中文名"] = dr["表中文名"].ToString();
                   newdr["字段名"] = dr["字段名"].ToString();
                   newdr["字段序号"] = dr["字段序号"].ToString();
                   newdr["是否主键"] = dr["是否主键"].ToString();
                   newdr["字段中文描述"] = dr["字段中文描述"].ToString();
                   newdr["字段类型"] = dr["字段类型"].ToString();
                   newdr["字段长度"] = dr["字段长度"].ToString();
                   newdr["是否为空"] = dr["是否为空"].ToString();
                   newdr["小数点长度"] = dr["小数点长度"].ToString();
                   newdr["取值范围"] = dr["取值范围"].ToString();
                   newdr["备注/说明"] = dr["备注/说明"].ToString();
                   if (!string.IsNullOrEmpty(dr["字段中文描述"].ToString()))
                   {
                       objTable.Rows.Add(newdr);
                       continue;
                   }
                   string findres = FindEnTableNameInDBBeanArr(dr["字段名"].ToString(),dr["表名"].ToString(), findDocDBbeanArr, out findDBbean);
                   if (findres != "0")
                   {
                       if (findres != "-1")
                       {
                           newdr["结果"] = findres;
                       }
                       objTable.Rows.Add(newdr);
                       continue;
                   }
                   newdr["结果"] = "s";
                   newdr["表中文名"] = findDBbean.TableCnName;
                   newdr["字段中文描述"] = findDBbean.FieldCnName;
                   newdr["备注/说明"] = findDBbean.Bak;
                   objTable.Rows.Add(newdr);

                    //找到的结果形成excel
                   DataRow findnewdr = findobjTable.NewRow();
                   findnewdr["系统"] = dr["系统"].ToString();
                   findnewdr["表名"] = dr["表名"].ToString();
                   findnewdr["表中文名"] = findDBbean.TableCnName;
                   findnewdr["字段名"] = dr["字段名"].ToString();
                   findnewdr["字段序号"] = dr["字段序号"].ToString();
                   findnewdr["是否主键"] = dr["是否主键"].ToString();
                   findnewdr["字段中文描述"] = findDBbean.FieldCnName;
                   findnewdr["字段类型"] = dr["字段类型"].ToString();
                   findnewdr["字段长度"] = dr["字段长度"].ToString();
                   findnewdr["是否为空"] = dr["是否为空"].ToString();
                   findnewdr["小数点长度"] = dr["小数点长度"].ToString();
                   findnewdr["取值范围"] = dr["取值范围"].ToString();
                   findnewdr["备注/说明"] = findDBbean.Bak;
                   findobjTable.Rows.Add(findnewdr);
                 
                }
                
                //把dbbean的ArrayList写人excel
                excelop.DSToExcel(newExcds, outfileURL);

                //把找到的内容写入excel
                excelop.DSToExcel(findExcds, outFindfileURL);
                return "0";
            }
            catch(Exception e)
            {
                return e.Message;
            }
             finally
            {
                if (m_crtOpenFile != null)
                {
                    m_crtOpenFile.Close();
                }

            }

        }




        /// <summary>
        /// 在EXL中搜索，查找到字段名表名对应后
        /// </summary>
        /// <param name="strcExlPath"></param>
        /// <param name="tableNamePath"></param>
        /// <param name="outfileURL"></param>
        /// <param name="outFindfileURL"></param>
        /// <returns></returns>
        public string StrcExlToExl(string strcExlPath, string tableNamePath, string outfileURL, string outFindfileURL)
        {
            try
            {
                string strResult = "";
                //获取excelds
                ExcelOp excelop = new ExcelOp();

                string res = excelop.CreateNoJoinCellStyleALLPath(strcExlPath, strcExlPath);
                if (res != "0")
                {
                   return "生成非合并格式不成功";
                }
               
                DataSet strcds = excelop.ExcelToDS(strcExlPath);
                DataSet excds = excelop.ExcelToDS(tableNamePath);
                string findTableEnName = string.Empty;
                ArrayList dbBeanArr = new ArrayList();
                ArrayList findDocDBbeanArr = new ArrayList();
                //将doc中的数据全部读出
                strResult = ReadXLSType(strcds, out findDocDBbeanArr);
                if (strResult != "0")
                {
                    return strResult;
                }
                //把dbbean的ArrayList写人dataset
                DataSet newExcds = new DataSet();
                DataTable objTable = new DataTable("数据");
                objTable.Columns.Add("结果", typeof(string));
                objTable.Columns.Add("系统", typeof(string));
                objTable.Columns.Add("表名", typeof(string));
                objTable.Columns.Add("表中文名", typeof(string));
                objTable.Columns.Add("字段名", typeof(string));
                objTable.Columns.Add("字段序号", typeof(string));
                objTable.Columns.Add("是否主键", typeof(string));
                objTable.Columns.Add("字段中文描述", typeof(string));
                objTable.Columns.Add("字段类型", typeof(string));
                objTable.Columns.Add("字段长度", typeof(string));
                objTable.Columns.Add("是否为空", typeof(string));
                objTable.Columns.Add("小数点长度", typeof(string));
                objTable.Columns.Add("取值范围", typeof(string));
                objTable.Columns.Add("备注/说明", typeof(string));
                newExcds.Tables.Add(objTable);

                //找到的结果形成excel
                DataSet findExcds = new DataSet();
                DataTable findobjTable = new DataTable("数据");
                findobjTable.Columns.Add("系统", typeof(string));
                findobjTable.Columns.Add("表名", typeof(string));
                findobjTable.Columns.Add("表中文名", typeof(string));
                findobjTable.Columns.Add("字段名", typeof(string));
                findobjTable.Columns.Add("字段序号", typeof(string));
                findobjTable.Columns.Add("是否主键", typeof(string));
                findobjTable.Columns.Add("字段中文描述", typeof(string));
                findobjTable.Columns.Add("字段类型", typeof(string));
                findobjTable.Columns.Add("字段长度", typeof(string));
                findobjTable.Columns.Add("是否为空", typeof(string));
                findobjTable.Columns.Add("小数点长度", typeof(string));
                findobjTable.Columns.Add("取值范围", typeof(string));
                findobjTable.Columns.Add("备注/说明", typeof(string));
                findExcds.Tables.Add(findobjTable);
                foreach (DataRow dr in excds.Tables[0].Rows)
                {
                    DbBean findDBbean = new DbBean();
                    DataRow newdr = objTable.NewRow();
                    newdr["结果"] = "";
                    newdr["系统"] = dr["系统"].ToString();
                    newdr["表名"] = dr["表名"].ToString();
                    newdr["表中文名"] = dr["表中文名"].ToString();
                    newdr["字段名"] = dr["字段名"].ToString();
                    newdr["字段序号"] = dr["字段序号"].ToString();
                    newdr["是否主键"] = dr["是否主键"].ToString();
                    newdr["字段中文描述"] = dr["字段中文描述"].ToString();
                    newdr["字段类型"] = dr["字段类型"].ToString();
                    newdr["字段长度"] = dr["字段长度"].ToString();
                    newdr["是否为空"] = dr["是否为空"].ToString();
                    newdr["小数点长度"] = dr["小数点长度"].ToString();
                    newdr["取值范围"] = dr["取值范围"].ToString();
                    newdr["备注/说明"] = dr["备注/说明"].ToString();
                    if (!string.IsNullOrEmpty(dr["字段中文描述"].ToString()))
                    {
                        objTable.Rows.Add(newdr);
                        continue;
                    }
                    string findres = FindEnTableNameInDBBeanArr(dr["字段名"].ToString(), dr["表名"].ToString(), findDocDBbeanArr, out findDBbean);
                    if (findres != "0")
                    {
                        if (findres != "-1")
                        {
                            newdr["结果"] = findres;
                        }
                        objTable.Rows.Add(newdr);
                        continue;
                    }
                    newdr["结果"] = "s";
                    newdr["表中文名"] = findDBbean.TableCnName;
                    newdr["字段中文描述"] = findDBbean.FieldCnName;
                    newdr["备注/说明"] = findDBbean.Bak;
                    objTable.Rows.Add(newdr);

                    //找到的结果形成excel
                    DataRow findnewdr = findobjTable.NewRow();
                    findnewdr["系统"] = dr["系统"].ToString();
                    findnewdr["表名"] = dr["表名"].ToString();
                    findnewdr["表中文名"] = findDBbean.TableCnName;
                    findnewdr["字段名"] = dr["字段名"].ToString();
                    findnewdr["字段序号"] = dr["字段序号"].ToString();
                    findnewdr["是否主键"] = dr["是否主键"].ToString();
                    findnewdr["字段中文描述"] = findDBbean.FieldCnName;
                    findnewdr["字段类型"] = dr["字段类型"].ToString();
                    findnewdr["字段长度"] = dr["字段长度"].ToString();
                    findnewdr["是否为空"] = dr["是否为空"].ToString();
                    findnewdr["小数点长度"] = dr["小数点长度"].ToString();
                    findnewdr["取值范围"] = dr["取值范围"].ToString();
                    findnewdr["备注/说明"] = findDBbean.Bak;
                    findobjTable.Rows.Add(findnewdr);

                }

                //把dbbean的ArrayList写人excel
                excelop.DSToExcel(newExcds, outfileURL);

                //把找到的内容写入excel
                excelop.DSToExcel(findExcds, outFindfileURL);
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            

        }



        /// <summary>
        /// 将数据结构word文件转化为建表语句
        /// </summary>
        /// <param name="strcWordPath"></param>
        /// <param name="outfileURL"></param>
        /// <returns></returns>
        public string StrcDocToTxt(string strcWordPath)
        {
            try
            {
                string strResult = "";
                ReadWordFile r = new ReadWordFile();
                strResult = r.OpenWord(strcWordPath);
                m_crtOpenFile = r;
                if (strResult != "0")
                {
                    strResult = "数据库结构文档路径错误";
                    return strResult;
                }

                string findTableEnName = string.Empty;
                ArrayList dbBeanArr = new ArrayList();
                ArrayList findDocDBbeanArr = new ArrayList();
                //将doc中的数据全部读出
                strResult = ReadDocTypeACMS(r, out findDocDBbeanArr);
                if (strResult != "0")
                {
                    if (m_crtOpenFile != null)
                    {
                        m_crtOpenFile.Close();
                    }
                    return strResult;
                }
                string tableEnName = string.Empty;
                string tableCnName = string.Empty;
                string lastTableEnName = string.Empty;
                string fieldEnName = string.Empty;
                string fieldType = string.Empty;
                string isPK = string.Empty;
                string lastPKField = string.Empty;
                string no = string.Empty;
                int tableNo = 1;

                ArrayList sqlArr = new ArrayList();
                for (int i = 0; i < findDocDBbeanArr.Count; i++)
                {
                   tableEnName = ((DbBean)findDocDBbeanArr[i]).TableEnName.Trim();
                   tableCnName = ((DbBean)findDocDBbeanArr[i]).TableCnName.Trim();
                   fieldEnName = ((DbBean)findDocDBbeanArr[i]).FieldEnName.Trim();
                   fieldType = ((DbBean)findDocDBbeanArr[i]).FieldType.Trim();
                   isPK = ((DbBean)findDocDBbeanArr[i]).IsPk.Trim();
                   //no = ((DbBean)findDocDBbeanArr[i]).Bak.Trim();                   
                   
                   if (!string.Equals(tableEnName,lastTableEnName))
                   { 
                       if (!string.IsNullOrEmpty(lastTableEnName))
                       {
                           lastPKField = lastPKField.Trim().TrimEnd(',');
                           sqlArr.Add("primary key(" + lastPKField + ")");
                           sqlArr.Add(") ENGINE = MyISAM DEFAULT CHARACTER SET utf8;"); 
                           lastPKField = string.Empty;                           
                           tableNo++;
                       }
                      sqlArr.Add(" ");
                      sqlArr.Add("---" + tableNo + "  " + tableCnName);
                      sqlArr.Add("---"+tableEnName);

                      sqlArr.Add(string.Format("DROP TABLE IF EXISTS `{0}`;",tableEnName));
                      //sqlArr.Add("           from  sysobjects");
                      //sqlArr.Add("           where  name = '"+ tableEnName +"'");
                      //sqlArr.Add("           and   type = 'U')");
                      //sqlArr.Add("   drop table "+ tableEnName);
                      //sqlArr.Add("go");
                      sqlArr.Add("CREATE TABLE " + tableEnName +"( ");
                      
                      lastTableEnName = tableEnName;
                   }

                   string isNull = "NULL";
                   bool isDesc = false;
                   
                   if (isPK.Contains("desc") || isPK.Contains("DESC"))
                   {
                       isDesc = true;
                   }
                   if (isPK.Contains("P(K)") || isPK.Contains("P（K）")  || isPK.Contains("P(k)")  )
                   {
                       isNull = "NOT NULL";
                       if (!isDesc)
                       {
                           lastPKField = fieldEnName;
                       }
                       else
                       {
                           lastPKField = fieldEnName + " desc ";
                       }
                   }
                   if (isPK.Contains("P(U)"))
                   {
                       isNull = "NOT NULL";
                       if (!isDesc)
                       {
                           lastPKField = lastPKField + fieldEnName + ",";
                       }
                       else
                       {
                           lastPKField = lastPKField + fieldEnName + " desc ,";
                       }
                   }
                   
                   sqlArr.Add(fieldEnName + "   " + fieldType + "    " +isNull+"   ,");
                   if (i == findDocDBbeanArr.Count - 1)
                   {
                       lastPKField = lastPKField.Trim().TrimEnd(',');
                       //sqlArr.Add("CONSTRAINT PK_" + lastTableEnName + " PRIMARY KEY CLUSTERED(" + lastPKField + ")");
                       sqlArr.Add("primary key(" + lastPKField + ")");
                       sqlArr.Add(")  ENGINE = MyISAM DEFAULT CHARACTER SET utf8;"); 
                       //sqlArr.Add("go");
                       lastPKField = string.Empty;
                   }
                }
                FileOp fileop = new FileOp();
                fileop.write("create_table_sql.txt", sqlArr);
               
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (m_crtOpenFile != null)
                {
                    m_crtOpenFile.Close();
                }

            }

        }


        /// <summary>
        /// 自动生成bean
        /// </summary>
        /// <param name="strcWordPath"></param>
        /// <returns></returns>
        public string StrcDocToBean(string strcWordPath)
        {
            try
            {
                string strResult = "";
                ReadWordFile r = new ReadWordFile();
                StringOp strop =new StringOp();
                strResult = r.OpenWord(strcWordPath);
                m_crtOpenFile = r;
                if (strResult != "0")
                {
                    strResult = "数据库结构文档路径错误";
                    return strResult;
                }

                string findTableEnName = string.Empty;
                ArrayList dbBeanArr = new ArrayList();
                ArrayList findDocDBbeanArr = new ArrayList();
                //将doc中的数据全部读出
                strResult = ReadDocTypeACMS(r, out findDocDBbeanArr);
                if (strResult != "0")
                {
                    if (m_crtOpenFile != null)
                    {
                        m_crtOpenFile.Close();
                    }
                    return strResult;
                }
                string tableEnName = string.Empty;
                string tableCnName = string.Empty;
                string lastTableEnName = string.Empty;
                string fieldEnName = string.Empty;
                string fieldCnName = string.Empty;
                string fieldType = string.Empty;
                string isPK = string.Empty;
                string lastPKField = string.Empty;
                string no = string.Empty;
                int tableNo = 1;
                ArrayList sqlArr = new ArrayList();
                for (int i = 0; i < findDocDBbeanArr.Count; i++)
                {
                    tableEnName = ((DbBean)findDocDBbeanArr[i]).TableEnName.Trim();
                    tableCnName = ((DbBean)findDocDBbeanArr[i]).TableCnName.Trim();
                    fieldEnName = ((DbBean)findDocDBbeanArr[i]).FieldEnName.Trim();
                    fieldCnName = ((DbBean)findDocDBbeanArr[i]).FieldCnName.Trim();
                    fieldType = ((DbBean)findDocDBbeanArr[i]).FieldType.Trim();
                    isPK = ((DbBean)findDocDBbeanArr[i]).IsPk.Trim();
                    //no = ((DbBean)findDocDBbeanArr[i]).Bak.Trim();
                    if (!string.Equals(tableEnName, lastTableEnName))
                    {
                        if (!string.IsNullOrEmpty(lastTableEnName))
                        {
                            lastPKField = lastPKField.Trim().TrimEnd(',');
                            sqlArr.Add("}");
                            lastPKField = string.Empty;
                            tableNo++;
                        }
                        sqlArr.Add("//");
                        sqlArr.Add("//" + tableNo + "  " + tableCnName);
                        sqlArr.Add("//" + tableEnName);

                        sqlArr.Add("[DataObjectAttribute(\"" + tableEnName + "\")]");
                        sqlArr.Add("public class " + strop.FirstUpper(tableEnName) + "Bean");
                        sqlArr.Add("{");

                        lastTableEnName = tableEnName;
                    }

                    string isNull = "false";
                    string type = "string";
                    string objfieldtype = "char";
                    string objNam = string.Empty;
                    int len = 0;
                    if (isPK.Trim() == "P(K)" || isPK.Trim() == "P（K）" || isPK.Trim() == "P(k)")
                    {
                        isNull = "true";
                        lastPKField = fieldEnName;
                    }
                    if (isPK == "P(U)")
                    {
                        isNull = "true";
                        lastPKField = lastPKField + fieldEnName + ",";
                    }
                    if(fieldType.Contains("char") || fieldType.Contains("Char"))
                    {
                        type = "string";
                        objfieldtype = "char";
                        len =Convert.ToInt32((fieldType.Split('(')[1].Split(')')[0]).ToString());
                    }
                    else if(fieldType.Contains("int") || fieldType.Contains("Int"))
                    {
                        type = "int?";
                        objfieldtype = "int";
                        len =0;
                    }
                    else if (fieldType.Contains("float") || fieldType.Contains("Float"))
                    {
                        type = "float?";
                        objfieldtype = "float";
                        len = 0;
                    }
                    else if (fieldType.Contains("double") || fieldType.Contains("Double"))
                    {
                        type = "double?";
                        objfieldtype = "double";
                        len = 0;
                    }
                    else if (fieldType.Contains("decimal") || fieldType.Contains("Decimal"))
                    {
                        type = "decimal?";
                        objfieldtype = "decimal";
                        len = 0;
                    }
                    else if (fieldType.Contains("varchar") || fieldType.Contains("Varchar"))
                    {
                        type = "varchar";
                        objfieldtype = "varchar";
                        len =Convert.ToInt32((fieldType.Split('(')[1].Split(')')[0]).ToString());
                    }
                    else if (fieldType.Contains("text") || fieldType.Contains("Text"))
                    {
                        type = "string";
                        objfieldtype = "text";
                        len = 0;
                    }
                    if(fieldEnName.IndexOf("_") > 0)
                    {
                      objNam = fieldEnName.Split('_')[1];
                    }
                   
                    sqlArr.Add("  /// <summary>");
                    sqlArr.Add("  /// "+fieldCnName);
                    sqlArr.Add("  /// <summary>");
                    if (objfieldtype != "int")
                    {
                        sqlArr.Add("  private" + " " + type + " " + "_" + objNam + ";");
                    }
                    else
                    {
                        sqlArr.Add("  private" + " " + type + " " + "_" + objNam + " = null;");
                    }
                    sqlArr.Add("   ");
                    sqlArr.Add("  [DataFieldAttribute(\"" + fieldEnName + "\", \"" + objfieldtype + "\"," + len + "," + isNull + ")]");
                    sqlArr.Add("  public "+type+ " " + strop.FirstUpper(objNam));
                    sqlArr.Add("  {");
                    sqlArr.Add("   get { return _"+objNam+"; }");
                    sqlArr.Add("   set { _"+objNam+" = value; }");
                    sqlArr.Add("   }");
                    sqlArr.Add("   ");

                    
                    if (i == findDocDBbeanArr.Count - 1)
                    {
                        lastPKField = lastPKField.Trim().TrimEnd(',');
                        sqlArr.Add("}");
                        lastPKField = string.Empty;
                    }
                    
                }
                FileOp fileop = new FileOp();
                fileop.write("create_bean.txt", sqlArr);

                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                if (m_crtOpenFile != null)
                {
                    m_crtOpenFile.Close();
                }

            }

        }


        /// <summary>
        /// 处理成规则表名
        /// </summary>
        /// <param name="willExeUrl"></param>
        /// <param name="outFindfileURL"></param>
        /// <returns></returns>
        public string ExeRegulerTableName(string willExeUrl,string outFindfileURL)
        {
            try
            {
                string strResult = "";
                ReadWordFile r = new ReadWordFile();
               
                //获取excelds
                ExcelOp excelop = new ExcelOp();
                StringOp stringop = new StringOp();
                DataSet excds = excelop.ExcelToDS(willExeUrl);
                string findTableEnName = string.Empty;
                
                //把dbbean的ArrayList写人dataset
                DataSet newExcds = new DataSet();
                DataTable objTable = new DataTable("数据");
                objTable.Columns.Add("结果", typeof(string));
                objTable.Columns.Add("系统", typeof(string));
                objTable.Columns.Add("表名", typeof(string));
                objTable.Columns.Add("表中文名", typeof(string));
                objTable.Columns.Add("字段名", typeof(string));
                objTable.Columns.Add("字段序号", typeof(string));
                objTable.Columns.Add("是否主键", typeof(string));
                objTable.Columns.Add("字段中文描述", typeof(string));
                objTable.Columns.Add("字段类型", typeof(string));
                objTable.Columns.Add("字段长度", typeof(string));
                objTable.Columns.Add("是否为空", typeof(string));
                objTable.Columns.Add("小数点长度", typeof(string));
                objTable.Columns.Add("取值范围", typeof(string));
                objTable.Columns.Add("备注/说明", typeof(string));
                newExcds.Tables.Add(objTable);

               
                foreach (DataRow dr in excds.Tables[0].Rows)
                {
                    DbBean findDBbean = new DbBean();
                    DataRow newdr = objTable.NewRow();
                    newdr["结果"] = dr["结果"].ToString();
                    newdr["系统"] = dr["系统"].ToString();
                    newdr["表名"] = dr["表名"].ToString();
                    newdr["表中文名"] = stringop.GetRegulerTableName(dr["表中文名"].ToString());
                    newdr["字段名"] = dr["字段名"].ToString();
                    newdr["字段序号"] = dr["字段序号"].ToString();
                    newdr["是否主键"] = dr["是否主键"].ToString();
                    newdr["字段中文描述"] = dr["字段中文描述"].ToString();
                    newdr["字段类型"] = dr["字段类型"].ToString();
                    newdr["字段长度"] = dr["字段长度"].ToString();
                    newdr["是否为空"] = dr["是否为空"].ToString();
                    newdr["小数点长度"] = dr["小数点长度"].ToString();
                    newdr["取值范围"] = dr["取值范围"].ToString();
                    newdr["备注/说明"] = dr["备注/说明"].ToString();
                    objTable.Rows.Add(newdr);

                }

                //把dbbean的ArrayList写人excel
                excelop.DSToExcel(newExcds, outFindfileURL);

              
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                

            }

        }



        /// <summary>
        /// 处理成规则表名
        /// </summary>
        /// <param name="willExeUrl"></param>
        /// <param name="outFindfileURL"></param>
        /// <returns></returns>
        public string DealBussinsDate(string willExeUrl, string outfileURL)
        {
            try
            {
                string strResult = "";
                ReadWordFile r = new ReadWordFile();

                //获取excelds
                ExcelOp excelop = new ExcelOp();
                StringOp stringop = new StringOp();
                DataSet excds = excelop.ExcelToDS(willExeUrl);
                string findTableEnName = string.Empty;

                //把dbbean的ArrayList写人dataset
                DataSet newExcds = new DataSet();
                DataTable objTable = new DataTable("数据");
                objTable.Columns.Add("结果", typeof(string));
                objTable.Columns.Add("系统", typeof(string));
                objTable.Columns.Add("表名", typeof(string));
                objTable.Columns.Add("表中文名", typeof(string));
                objTable.Columns.Add("字段名", typeof(string));
                objTable.Columns.Add("字段序号", typeof(string));
                objTable.Columns.Add("是否主键", typeof(string));
                objTable.Columns.Add("字段中文描述", typeof(string));
                objTable.Columns.Add("字段类型", typeof(string));
                objTable.Columns.Add("字段长度", typeof(string));
                objTable.Columns.Add("是否为空", typeof(string));
                objTable.Columns.Add("小数点长度", typeof(string));
                objTable.Columns.Add("取值范围", typeof(string));
                objTable.Columns.Add("备注/说明", typeof(string));
                newExcds.Tables.Add(objTable);

                int i = 0;
                foreach (DataRow dr in excds.Tables[0].Rows)
                {
                    DbBean findDBbean = new DbBean();
                    DataRow newdr = objTable.NewRow();
                    newdr["结果"] = dr["结果"].ToString();
                    newdr["系统"] = dr["系统"].ToString();
                    newdr["表名"] = dr["表名"].ToString();
                    newdr["表中文名"] = dr["表中文名"].ToString();
                    newdr["字段名"] = dr["字段名"].ToString();
                    newdr["字段序号"] = dr["字段序号"].ToString();
                    newdr["是否主键"] = dr["是否主键"].ToString();
                    newdr["字段中文描述"] = dr["字段中文描述"].ToString();
                    newdr["字段类型"] = dr["字段类型"].ToString();
                    newdr["字段长度"] = dr["字段长度"].ToString();
                    newdr["是否为空"] = dr["是否为空"].ToString();
                    newdr["小数点长度"] = dr["小数点长度"].ToString();
                    newdr["取值范围"] = dr["取值范围"].ToString();
                    newdr["备注/说明"] = dr["备注/说明"].ToString();
                    if (newdr["字段名"].ToString() == "BUSINESS_DATE")
                    {
                        newdr["字段中文描述"] = "分区日期";
                        try
                        {
                            newdr["表中文名"] = excds.Tables[0].Rows[i - 1]["表中文名"].ToString();
                        }
                        catch (Exception tbe)
                        {
                            i++;
                            continue;
                        }
                    }
                    else if (newdr["字段名"].ToString() == "PROV__CODE")
                    {
                        try
                        {
                            newdr["字段中文描述"] = "分区省市码";
                            newdr["表中文名"] = excds.Tables[0].Rows[i - 2]["表中文名"].ToString();
                        }
                        catch (Exception tbee)
                        {
                            i++;
                            continue;
                        }
                    }
                    
                    objTable.Rows.Add(newdr);
                    i++;
                }

                //把dbbean的ArrayList写人excel
                excelop.DSToExcel(newExcds, outfileURL);


                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {


            }

        }


        /// <summary>
        /// 处理成规则表名
        /// </summary>
        /// <param name="willExeUrl"></param>
        /// <param name="outFindfileURL"></param>
        /// <returns></returns>
        public string DealStdXls(string willExeUrl, string outfileURL)
        {
            try
            {
                //获取excelds
                ExcelOp excelop = new ExcelOp();

                string res = excelop.CreateNoJoinCellStyleALLPath(willExeUrl, willExeUrl);
                StringOp stringop = new StringOp();
                res = excelop.ARowToColumnEnd(willExeUrl, outfileURL, "Cname");
                if (res != "0")
                {
                    return res;
                }
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }


        /// <summary>
        /// 把其他excel中含有的表中文名，提取出来
        /// </summary>
        /// <param name="willExeUrl"></param>
        /// <param name="outfileURL"></param>
        /// <returns></returns>
        public string FindTableNameFromXls(string willExeUrl, string findXlsUrl,string outfileURL)
        {
            try
            {
                //获取excelds
                ExcelOp excelop = new ExcelOp();
                DSOp dsop = new DSOp();
                string res = excelop.CreateNoJoinCellStyleALLPath(findXlsUrl, findXlsUrl);
                DataSet excds = excelop.ExcelToDS(willExeUrl);
                DataSet findds = excelop.ExcelToDS(findXlsUrl);
                //把dbbean的ArrayList写人dataset
                DataSet newExcds = new DataSet();
                DataTable objTable = new DataTable("数据");
                objTable.Columns.Add("结果", typeof(string));
                objTable.Columns.Add("系统名", typeof(string));
                objTable.Columns.Add("表英文名", typeof(string));
                objTable.Columns.Add("表中文名", typeof(string));
                newExcds.Tables.Add(objTable);
                foreach (DataRow dr in excds.Tables[0].Rows)
                {
                    DbBean findDBbean = new DbBean();
                    DataRow newdr = objTable.NewRow();
                    string oldTableNameStr = dr["表英文名"].ToString().Trim();
                    newdr["结果"] = "";
                    newdr["系统名"] = "";
                    newdr["表英文名"] = "";
                    newdr["表中文名"] = "";
                    int splitIndex = oldTableNameStr.IndexOf('_');
                    if (splitIndex  < 1)
                    {
                        newdr["结果"] = "表名不规则";
                        objTable.Rows.Add(newdr);
                        continue;

                    }
                    newdr["系统名"] = oldTableNameStr.Split('_')[0].Trim();
                    newdr["表英文名"] = oldTableNameStr.Substring(splitIndex+1);
                    newdr["表中文名"] = dsop.TableFindWithColumnIndex(findds,0,1,newdr["系统名"].ToString(),newdr["表英文名"].ToString(),2);
                    objTable.Rows.Add(newdr);

                }

                //把dbbean的ArrayList写人excel
                excelop.DSToExcel(newExcds, outfileURL);


                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }


        }



        /// <summary>
        /// 打开指定路径的配置文件
        /// </summary>
        /// <param name="openPath">打开文件的路径</param>
        /// <returns></returns>
        public string GetCfgContent(string openPath)
        {
            string strResult = "";
            StreamReader myReader = null;
            try
            {
                if (!File.Exists(openPath))
                {

                    strResult = "{'success':'false', errdes:'[X636]配置文件不能打开'}";
                    return strResult;
                }

                myReader = new StreamReader(openPath, Encoding.GetEncoding("GB2312"));
                string cfgcontent = myReader.ReadToEnd();
                strResult = "{'success':'true', 'strcon':'" + cfgcontent + "'}";
                return strResult;
            }
            catch (Exception e)
            {
                strResult = "{'success':'false', errdes:'[X636]配置文件不能打开" + e.Message + "'}";
                return strResult;
            }
            finally
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
            }

        }



        /// <summary>
        /// 保存配置文件
        /// </summary>
        /// <param name="openPath">保存路径</param>
        /// <param name="updstr">文件内容</param>
        /// <returns></returns>
        public string UpdCfgContent(string openPath, string updstr)
        {
            string strResult = "";
            FileStream mystream = null;
            StreamWriter mywrite = null;
            try
            {
                if (!File.Exists(openPath))
                {

                    strResult = "{'success':'false', errdes:'[X636]配置文件不能打开'}";
                    return strResult;
                }

                mystream = new FileStream(openPath, FileMode.Create);
                mywrite = new StreamWriter(mystream, Encoding.GetEncoding("GB2312"));
                mywrite.Write(updstr);
                mywrite.Flush();
                strResult = "{'success':'true', 'strcon':''}";
                return strResult;
            }
            catch (Exception e)
            {
                strResult = "{'success':'false', errdes:'[X636]配置文件不能打开" + e.Message + "'}";
                return strResult;
            }
            finally
            {
                if (mywrite != null)
                {
                    mywrite.Close();
                }

                if (mystream != null)
                {
                    mystream.Close();

                }

            }

        }

    }
}

