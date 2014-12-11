using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Text.RegularExpressions;

namespace DAL
{
    public class DSOp
    {

        /// <summary>
        /// 将两个ds相加
        /// </summary>
        /// <param name="ds1"></param>
        /// <param name="ds2"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20120301    王强      创建代码
        ///----------------------------------------------------------
        public DataSet add(DataSet ds1, DataSet ds2)
        {
            DataSet ds = new DataSet();
            ds = ds1.Clone();
            ds = ds1.Copy();
            foreach (DataRow dr2 in ds2.Tables[0].Rows)
            {


                ds.Tables[0].Rows.Add(dr2.ItemArray);
            }
            return ds;

        }

        /// <summary>
        /// 两个dataset 相减
        /// </summary>
        /// <param name="ds1"></param>
        /// <param name="ds2"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20120301    王强      创建代码
        ///----------------------------------------------------------
        public DataSet sub(DataSet ds1, DataSet ds2)
        {
            DataSet ds = new DataSet();
            ds = ds1.Clone();
            ds = ds1.Copy();
            ds.Tables[0].PrimaryKey = new DataColumn[] { ds.Tables[0].Columns[0] };
            //ds2.Tables[0].PrimaryKey = new DataColumn[] { ds2.Tables[0].Columns[0] };
            foreach (DataRow dr2 in ds2.Tables[0].Rows)
            {
                DataRow rowCustomer = ds.Tables[0].NewRow();
                rowCustomer = ds.Tables[0].Rows.Find(dr2[0].ToString());
                if (rowCustomer != null)
                {
                    ds.Tables[0].Rows.Remove(rowCustomer);
                }
            }
            return ds;


        }

        /// <summary>
        /// 得到ds中某一列最多出现的数值
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="columnNum"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20120301    王强      创建代码
        ///----------------------------------------------------------
        public string GetMostVal(DataSet ds, int columnNum)
        {
            columnNum = columnNum - 1;
            if (ds == null || ds.Tables[0].Rows.Count < 1)
            {
                return "";
            }
            Hashtable valCountHs = new Hashtable();

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string val = ds.Tables[0].Rows[i][columnNum].ToString();
                if (!valCountHs.Contains(val))
                {
                    valCountHs[val] = 1;
                }
                else
                {
                    valCountHs[val] = (int)valCountHs[val] + 1;
                }
            }
            ArrayList sortArr = new ArrayList();
            foreach (string key in valCountHs.Keys)
            {
                sortArr.Add((int)valCountHs[key]);
            }
            sortArr.Sort();

            int mostCount = (int)sortArr[sortArr.Count - 1];
            foreach (string key in valCountHs.Keys)
            {
                if ((int)valCountHs[key] == mostCount)
                {
                    return key;
                }

            }

            return "";
        }


        /// <summary>
        /// 取得制定列的值出现的次数
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="columnNum"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20120301    王强      创建代码
        ///----------------------------------------------------------
        public Hashtable GetValCount(DataSet ds, int columnNum)
        {
            columnNum = columnNum - 1;
            Hashtable valCountHs = new Hashtable();
            if (ds == null || ds.Tables[0].Rows.Count < 1)
            {
                return valCountHs;
            }


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string val = ds.Tables[0].Rows[i][columnNum].ToString().Trim();
                if (!valCountHs.Contains(val))
                {
                    valCountHs[val] = 1;
                }
                else
                {
                    valCountHs[val] = (int)valCountHs[val] + 1;
                }
            }
            return valCountHs;
        }


        /// <summary>
        /// 查找指定列的值，找到则返回这一行
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="findFieldArr"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20120323    王强      创建代码
        ///----------------------------------------------------------
        public DataRow find(DataTable dt, ArrayList findFieldNamArr, ArrayList findFieldValArr)
        {
            DataRow newdr = dt.NewRow();
            foreach (DataRow dr in dt.Rows)
            {
                int rightfieldCout = 0;
                for (int i = 0; i < findFieldNamArr.Count; i++)
                {
                    if (dr[findFieldNamArr[i].ToString()] != null)
                    {
                        if (dr[findFieldNamArr[i].ToString()].ToString().Trim() == findFieldValArr[i].ToString().Trim())
                        {
                            rightfieldCout++;
                        }
                    }
                }
                if (rightfieldCout == findFieldNamArr.Count)
                {
                    return dr;
                }
            }
            return newdr;

        }




        /// <summary>
        /// 按照某行在ds的位置往后移动几个位置
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="rownum"></param>
        /// <param name="moveBackNum"></param>
        /// <returns></returns>
        public DataSet RowMoveBack(DataSet ds, int rownum, int moveBackNum)
        {
            if (ds == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                return ds;
            }
            //如果往后移动超过了行数，则只移动到最后一行
            if (ds.Tables[0].Rows.Count < rownum + moveBackNum)
            {
                DataRow addTailrow = ds.Tables[0].NewRow();
                addTailrow.ItemArray = ds.Tables[0].Rows[rownum].ItemArray;
                ds.Tables[0].Rows.InsertAt(addTailrow, ds.Tables[0].Rows.Count);
                ds.Tables[0].Rows.RemoveAt(rownum);
                return ds;

            }

            DataRow addrow = ds.Tables[0].NewRow();
            addrow.ItemArray = ds.Tables[0].Rows[rownum].ItemArray;
            ds.Tables[0].Rows.InsertAt(addrow, rownum + moveBackNum);
            ds.Tables[0].Rows.RemoveAt(rownum);
            return ds;


        }



        /// <summary>
        /// 按条件查询某ds并返回
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public DataSet Sel(DataSet ds, string strWhere)
        {
            DataRow[] dr = ds.Tables[0].Select(strWhere);
            DataSet selds = new DataSet();
            DataTable dt = ds.Tables[0].Clone();
            DataRow temp;
            foreach (DataRow r in dr)
            {
                temp = dt.NewRow();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    temp[i] = r[i];
                }
                dt.Rows.Add(temp);
            }
            selds.Tables.Add(dt);
            return selds;
        }


        /// <summary>
        /// 根据某一行的列表，进行重新排序
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="orderlist"></param>
        /// <param name="columname"></param>
        /// <returns></returns>
        ///  ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20130917    王强      创建代码 
        ///----------------------------------------------------------
        public DataSet GetDsOrderbyList(DataSet ds,ArrayList orderlist,string columname)
        {
            DataSet orderds = new DataSet();
            DataTable dt = ds.Tables[0].Clone();
            for(int i = 0;i< orderlist.Count; i++)
            {
                DataRow[] dr = ds.Tables[0].Select(columname + " = '" + orderlist[i].ToString() + "'");
                DataRow temp;
                foreach (DataRow r in dr)
                {
                    temp = dt.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        temp[j] = r[j];
                    }
                    dt.Rows.Add(temp);
                }
            }
            orderds.Tables.Add(dt);
            return orderds;
        }


        /// <summary>
        /// 某一列加入vallist的指定值
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="vallist"></param>
        /// <param name="columname"></param>
        /// <returns></returns>
        ///  ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20130918    王强      创建代码 
        ///----------------------------------------------------------
        public DataSet AddDsColumnVal(DataSet ds, ArrayList vallist, string columname)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ds.Tables[0].Rows[i][columname] = vallist[i].ToString();
            }
            return ds;
        }


        /// <summary>
        /// 返回某一行regColumnnameArr里指定的列是否包括指定的
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="regColumnnameArr"></param>
        /// <param name="regexStr"></param>
        /// <returns></returns>
        ///  ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20130918    王强      创建代码 
        ///----------------------------------------------------------
        private bool IsRowContainVal(DataRow dr, ArrayList regColumnnameArr, Regex regexStr,string compareVal)
        {
            bool res = false;
            for (int k = 0; k < regColumnnameArr.Count; k++)
            {
                string regColumnName = regColumnnameArr[k].ToString();
                string colval = dr[regColumnName].ToString().Trim();
                MatchCollection m = regexStr.Matches(colval);
                for (int l = 0; l < m.Count; l++)
                {
                    string regVal = m[l].Value.ToString();
                    if (regVal == compareVal)
                    {
                        return  true;
                    }
                }
            }
            return res;
        }




        /// <summary>
        /// 根据某几列包含的字符串，把含有字符串的几行排在一起，形成列表
        /// 如根据这几列中包含的变更单号‘210765’等形式，把相关的变更按顺序排在一起.返回ArryList：
        /// 变更单号 实施顺序 前后台关联 BJC关联 TRT关联 TRT数据库关联  同需求关联  系统类关联 应用类关联
        /// reltsk   prio     back       bjc     trt     db             sr          sysrel     apprel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="orderColumname"></param>
        /// <param name="regColumnnameArr"></param>
        /// <param name="regexStr"></param>
        /// <returns></returns>
        ///  ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20130917    王强      创建代码 
        ///----------------------------------------------------------
        private ArrayList GetOrderListByContainRegStr(DataSet ds, string orderColumname, ArrayList regColumnnameArr, Regex regexStr,out ArrayList relnoList)
        {
            ArrayList orderList = new ArrayList();
            Hashtable rowhs = new Hashtable();//记录某一单号+关联变更到orderList的位置
            ArrayList orderColumnList = new ArrayList();//将预排序的列的值装入list
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
               orderColumnList.Add(dr[orderColumname].ToString().Trim());
            }
            int count = 0;
            relnoList = new ArrayList();

            for(int i =0 ;i < ds.Tables[0].Rows.Count;i++)
            {
                string ordercolvalt = ds.Tables[0].Rows[i][orderColumname].ToString().Trim();
                if (ordercolvalt == "216065")
                {
                    string res = "0";
                }
                
                //把此行的单号填入
                string ordercolval = ds.Tables[0].Rows[i][orderColumname].ToString().Trim();
                if (rowhs.ContainsKey(ordercolval))
                {
                    continue;
                }
                rowhs.Add(ordercolval, true);
                orderList.Add(ordercolval);
                relnoList.Add("0");
                //把某行的关联单号取出来
                for (int k = 0; k < regColumnnameArr.Count; k++)
                {
                    string regColumnName = regColumnnameArr[k].ToString();
                    string colval = ds.Tables[0].Rows[i][regColumnName].ToString().Trim();
                    MatchCollection m = regexStr.Matches(colval);

                    for (int j = 0; j < m.Count; j++)
                    {
                        string regVal = m[j].Value.ToString();
                        if (rowhs.ContainsKey(regVal) || !orderColumnList.Contains(regVal))
                        {
                            continue;
                        }
                        rowhs.Add(regVal, true);
                        orderList.Add(regVal);
                        int maintskindex = orderList.IndexOf(ordercolval);
                        if(relnoList[maintskindex] == "0")
                        {
                            relnoList[maintskindex] = ordercolval;
                        }
                        relnoList.Add(ordercolval);

                    }
                }
                //往下搜索，穷尽所有的关联变更，添加到orderList
                for(int j = i + 1; j < ds.Tables[0].Rows.Count; j++)
                {
                    //如果某行正好关联变更包含这个变更单号
                    if (!IsRowContainVal(ds.Tables[0].Rows[j], regColumnnameArr, regexStr, ordercolval))
                    {
                        continue;
                    }
                    //如果报含单号话，那就把所有的这一行的单号，这一行的关联变更都加入
                    string ordercolvalj = ds.Tables[0].Rows[j][orderColumname].ToString().Trim();
                    if (!rowhs.ContainsKey(ordercolvalj))
                    {
                        rowhs.Add(ordercolvalj, true);
                        orderList.Add(ordercolvalj);
                        int maintskindex = orderList.IndexOf(ordercolval);
                        if (relnoList[maintskindex] == "0")
                        {
                            relnoList[maintskindex] = ordercolval;
                        }
                        relnoList.Add(ordercolval);
                    }
                    for (int k = 0; k < regColumnnameArr.Count; k++)
                    {
                        string regColumnName = regColumnnameArr[k].ToString();
                        string colval = ds.Tables[0].Rows[j][regColumnName].ToString().Trim();
                        MatchCollection m = regexStr.Matches(colval);
                        for (int ll = 0; ll < m.Count; ll++)
                        {
                            string regVall = m[ll].Value.ToString();
                            if (rowhs.ContainsKey(regVall) || !orderColumnList.Contains(regVall))
                            {
                                continue;
                            }
                            rowhs.Add(regVall, true);
                            orderList.Add(regVall);
                            int maintskindex = orderList.IndexOf(ordercolval);
                            if (relnoList[maintskindex] == "0")
                            {
                                relnoList[maintskindex] = ordercolval;
                            }
                            relnoList.Add(ordercolval);
                            

                        }
                    }
                               
                        
                }

                

              
             
             
            }

            return orderList;
        }


        /// <summary>
        /// 根据某几列包含的字符串，把含有字符串的几行排在一起，形成列表
        /// 如根据这几列中包含的变更单号‘210765’等形式，把相关的变更按顺序排在一起.返回DataSet：
        /// 变更单号 实施顺序 前后台关联 BJC关联 TRT关联 TRT数据库关联  同需求关联  系统类关联 应用类关联
        /// reltsk   prio     back       bjc     trt     db             sr          sysrel     apprel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="orderColumname"></param>
        /// <param name="regColumnnameArr"></param>
        /// <param name="regexStr"></param>
        /// <returns></returns>
        ///  ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20130917    王强      创建代码 
        ///----------------------------------------------------------
        public DataSet GetDsByContainRegStr(DataSet ds, string orderColumname, ArrayList regColumnnameArr, Regex regexStr,string addColumnName)
        {
            try
            {
                DataSet orderDs = new DataSet();
                ArrayList orderList = new ArrayList();
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    //加入关联的哪个单号的列
                    ArrayList columnNameArr = new ArrayList();
                    columnNameArr.Add(addColumnName);
                    DataSet updds = new DataSet();
                    updds = TableAddColumn(ds, columnNameArr);
                    ArrayList relnoList = new ArrayList();
                    orderList = GetOrderListByContainRegStr(updds, orderColumname, regColumnnameArr, regexStr, out relnoList);
                    orderDs = GetDsOrderbyList(updds, orderList, orderColumname);
                    //加入关联哪个变更单的信息
                    orderDs = AddDsColumnVal(orderDs, relnoList, addColumnName);
                    return orderDs;
                }
                else
                {
                    return ds;
                }
            }
            catch(Exception e)
            {
                string res = e.Message;
                return ds;
            }
        }


        /// <summary>
        /// ds加几列
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="columnnameList"></param>
        /// <returns></returns>
        ///  ----------------------------------------------------------
        ///修改历史
        ///日期       修改人      修改
        ///----------------------------------------------------------
        ///20130918    王强      创建代码 
        ///----------------------------------------------------------
        public DataSet TableAddColumn(DataSet ds,ArrayList columnnameList)
        {
            DataSet updds = new DataSet();
            updds = ds.Clone();
            updds = ds.Copy();
            for (int i = 0; i < columnnameList.Count; i++)
            {
                updds.Tables[0].Columns.Add(columnnameList[i].ToString());
            }
            return updds;
        }




        /// <summary>
        /// 按照指定列比较，然后返回结果
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="compareIndex1"></param>
        /// <param name="compareIndex2"></param>
        /// <param name="compareStr1"></param>
        /// <param name="compareStr2"></param>
        /// <param name="resIndex"></param>
        /// <returns></returns>
        public string TableFindWithColumnIndex(DataSet ds, int compareIndex1, int compareIndex2, string compareStr1, string compareStr2, int resIndex)
        {
            try
            {
                if (ds == null)
                {
                    return "";
                }
                if(ds.Tables.Count < 1)
                {
                    return "";
                }
                string compareStr = string.Empty;
                string findStr = string.Empty;
                findStr = compareStr1 + compareStr2;
                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    for (int j = 0; j < ds.Tables[i].Rows.Count; j++)
                    {
                       compareStr = ds.Tables[i].Rows[j][compareIndex1].ToString().Trim() + ds.Tables[i].Rows[j][compareIndex2].ToString().Trim();
                       if (string.Equals(compareStr, findStr))
                       {
                           return ds.Tables[i].Rows[j][resIndex].ToString().Trim();
                       }

                    }
                }
                return "";
            }
            catch (Exception e)
            {
                return "";
            }
        }







    }
}
