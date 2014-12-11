using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.InteropServices;
using Sybase.Data.AseClient;
using System.Data.OleDb;
using Model;
using Excel = Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using System.IO;
using System.Windows.Forms;

namespace DAL
{
    public class ExcelOp
    {


        private ArrayList nameCellArr = new ArrayList();


        /// <summary>
        /// 读取excel  
        /// 默认第一行为标头  
        /// </summary>  
        /// <param name="strFileName">excel文档路径</param>  
        /// <returns></returns>  
        public DataSet ExcelToDS(string strFileName)
        {

            try
            {
                HSSFWorkbook hssfworkbook;
                DataSet ds = new DataSet();

                FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read);
                hssfworkbook = new HSSFWorkbook(file);
                int sheetCount = hssfworkbook.NumberOfSheets;
                for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
                {
                    ISheet isheet = hssfworkbook.GetSheetAt(sheetIndex);
                    DataTable dt = new DataTable();
                    if (isheet == null)
                    {
                        continue;
                    }
                    System.Collections.IEnumerator rows = isheet.GetRowEnumerator();

                    IRow headerRow = isheet.GetRow(0);
                    if (headerRow == null)
                    {
                        continue;
                    }
                    int cellCount = headerRow.LastCellNum;

                    for (int j = 0; j < cellCount; j++)
                    {
                        ICell cell = headerRow.GetCell(j);
                        if (cell == null)
                        {
                            continue;
                        }
                        if (!dt.Columns.Contains(cell.ToString()))
                        {
                            dt.Columns.Add(cell.ToString());
                        }
                        else
                        {
                            dt.Columns.Add(j.ToString());
                        }

                    }

                    for (int i = (isheet.FirstRowNum + 1); i <= isheet.LastRowNum; i++)
                    {
                        IRow row = isheet.GetRow(i);
                        if (row == null)
                        {
                            continue;
                        }
                        DataRow dataRow = dt.NewRow();

                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = row.GetCell(j).ToString();
                        }

                        dt.Rows.Add(dataRow);
                    }
                    ds.Tables.Add(dt);
                }
                file.Close();
                hssfworkbook = null;
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 由DataSet导出Excel
        /// </summary>
        /// <param name="sourceTable">要导出数据的DataTable</param>
        /// <param name="sheetName">工作表名称</param>
        /// <returns>Excel工作表</returns>
        private HSSFWorkbook ExportDataSetToExcel(DataSet sourceDs)
        {
            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook();
                int tableCount = sourceDs.Tables.Count;
                for (int tableIndex = 0; tableIndex < tableCount; tableIndex++)
                {
                    string sheetName = sourceDs.Tables[tableIndex].TableName;
                    ISheet sheet = workbook.CreateSheet(sheetName);
                    IRow headerRow = sheet.CreateRow(0);
                    foreach (DataColumn column in sourceDs.Tables[tableIndex].Columns)
                    {
                        headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                    }
                    int rowIndex = 1;

                    foreach (DataRow row in sourceDs.Tables[tableIndex].Rows)
                    {
                        IRow dataRow = sheet.CreateRow(rowIndex);

                        foreach (DataColumn column in sourceDs.Tables[tableIndex].Columns)
                        {
                            ICell newCell = dataRow.CreateCell(column.Ordinal);
                            string drValue = row[column].ToString();
                            switch (column.DataType.ToString())
                            {
                                case "System.String"://字符串类型          
                                    newCell.SetCellValue(drValue);
                                    break;
                                case "System.DateTime"://日期类型
                                    newCell.SetCellValue(drValue);
                                   // newCell.SetCellValue(dateV.ToShortDateString()); // 转换成段日期格式
                                    break;
                                case "System.Boolean"://布尔型
                                    bool boolV = false;
                                    bool.TryParse(drValue, out boolV);
                                    newCell.SetCellValue(boolV);
                                    break;
                                case "System.Int16"://整型
                                case "System.Int32":
                                case "System.Int64":
                                case "System.Byte":
                                    int intV = 0;
                                    int.TryParse(drValue, out intV);
                                    newCell.SetCellValue(intV);
                                    break;
                                case "System.Decimal"://浮点型
                                case "System.Double":
                                    double doubV = 0;
                                    double.TryParse(drValue, out doubV);
                                    newCell.SetCellValue(doubV);
                                    break;
                                case "System.DBNull"://空值处理
                                    newCell.SetCellValue("");
                                    break;
                                default:
                                    newCell.SetCellValue("");
                                    break;
                            }
                            
                        }

                        rowIndex++;
                    }
                }

                return workbook;
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// 将dataset存入Excel
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="SaveFile"></param>
        public void DSToExcel(DataSet ds, string SaveFile)
        {
            HSSFWorkbook workbook = ExportDataSetToExcel(ds);
            Save(SaveFile, workbook);
        }


        

        /// <summary>
        /// 将Excel特定sheet的特定范围转换为Dataset
        /// </summary>
        /// <param name="Path">Excel物理路径</param>
        /// <param name="table">sheet表名</param>
        /// <param name="condition">选取列范围</param>
        /// <returns>DataSet数据集</returns>
        public DataSet RptExcelToDS(string strPath, string table, string condition)
        {
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strPath + ";" + "Extended Properties=Excel 8.0;";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                string strExcel = "";
                OleDbDataAdapter myCommand = null;
                DataSet ds = null;
                strExcel = "select * from [" + table + "$" + condition + "]";
                myCommand = new OleDbDataAdapter(strExcel, strConn);
                ds = new DataSet();
                myCommand.Fill(ds, "table1");
                conn.Close();
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 根据参数将Excel内容全部转化为DataSet
        /// </summary>
        /// <param name="Path">Excel文件物理路径</param>
        /// <param name="TableList">sheet页列表</param>
        /// <param name="AreaConfigList">区域列表</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExcelAllSheetsToDS(string strPath, string strTableList, string strAreaConfigList)
        {
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strPath + ";" + "Extended Properties=Excel 8.0;";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();

                string[] strarrTableNames = strTableList.Split(',');
                string[] strarrAreaConfig = strAreaConfigList.Split(',');

                if (strarrTableNames.Length != strarrAreaConfig.Length)
                {
                    return null;
                }

                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = new OleDbCommand();
                adapter.SelectCommand.Connection = conn;
                DataSet excelData = new DataSet();

                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    DataRow dr = schemaTable.Rows[i];
                    string tbname = dr["TABLE_NAME"].ToString().Replace("$", "").Replace("'", "");
                    int tbpos = Array.IndexOf<string>(strarrTableNames, tbname);

                    if (tbpos > -1)
                    {
                        adapter.SelectCommand.CommandText = string.Format("SELECT * FROM [{0}${1}]", tbname, strarrAreaConfig[tbpos]);
                        adapter.Fill(excelData);
                        excelData.Tables[tbpos].TableName = tbname;
                    }
                }

                return excelData;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 将Excel全部转化为DataSet
        /// </summary>
        /// <param name="Path">Excel文件物理路径</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExcelAllSheetsToDS(string strPath)
        {
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + strPath + ";" + "Extended Properties='Excel 8.0;IMEX=1';";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();

                DataTable schemaTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = new OleDbCommand();
                adapter.SelectCommand.Connection = conn;
                DataSet excelData = new DataSet();

                for (int i = 0; i < schemaTable.Rows.Count; i++)
                {
                    DataRow dr = schemaTable.Rows[i];
                    string tbname = dr["TABLE_NAME"].ToString().Replace("$", "").Replace("'", "");
                    adapter.SelectCommand.CommandText = string.Format("SELECT * FROM [{0}]", dr["TABLE_NAME"].ToString());
                    adapter.Fill(excelData);
                    excelData.Tables[i].TableName = tbname;
                }
                return excelData;
            }
            catch (Exception)
            {
                throw;
            }
        }


        

        public void writeExcel(DataSet ds, string SaveFile)
        {
            try
            {
                string strCmd = string.Empty;
                OleDbCommand cmd;
                OleDbConnection conn;
                string strConn = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + SaveFile+ ";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";
                //HDR=YES 表示将sheet的第一行作为列名，所以我们默认excel的首行是列名。
                //IMEX=1 表示大致的意思是使用导入的模式，把数字也作为字符串来操作
              //有一点很重要。IMEX=1，是一种导入的模式，所以首先这个文件要存在，如果不存在会报错：
               //“Microsoft Jet 数据库引擎找不到对象'…\Customer.xls'。请确定对象是否存在，并正
              //确地写出它的名称和路径”，而且这样写了以后就算文件是存在的，还有一个问题是不能对文
              //件更新的，会提示“不能修改表 'sheet1' 的设计。它在只读数据库中”等错误，甚至还有提
               // 示权限的问题。
                conn = new OleDbConnection(strConn);
                //conn = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;"+ "Data Source=" + @"D:\test.xls"+";Extended Properties= 'Excel 8.0;HDR=Yes;'" );
                cmd = new OleDbCommand();
                cmd.Connection = conn;
                conn.Open();
                strCmd = "CREATE TABLE [Sheet1]( ";
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    strCmd += "[" + dc.ColumnName + "]   nvarchar(20), ";
                }
                strCmd = strCmd.Trim().Substring(0, strCmd.Length - 2);
                strCmd += ") ";
               

                cmd.CommandText = strCmd;

                cmd.ExecuteNonQuery();

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    if (dr.RowState != System.Data.DataRowState.Deleted)
                    {
                        strCmd = "insert   into   [Sheet1$]   values( ";
                        foreach (DataColumn dc in ds.Tables[0].Columns)
                        {
                            strCmd += " ' " + dr[dc.ColumnName].ToString() + " ', ";
                        }

                        strCmd = strCmd.Substring(0, strCmd.Length - 2);
                        strCmd += ") ";

                        cmd.CommandText = strCmd;

                        cmd.ExecuteNonQuery();


                    }
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }    
   
        } 
        
        public  void DataSetToExcel(DataSet ds, string SaveFile)
        {
            Excel.Application excel;
            Excel._Workbook xbk;
            Excel._Worksheet xst;
            object misValue = System.Reflection.Missing.Value;

            excel = new Excel.ApplicationClass();
            xbk = excel.Workbooks.Add(misValue);
            xst = (Excel._Worksheet)xbk.ActiveSheet;

            int rowIndex = 1;
            int colIndex = 0;

            //取得标题
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                colIndex++;
                excel.Cells[1, colIndex] = col.ColumnName;
            }

            //取得表格中的数据
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                rowIndex++;
                colIndex = 0;
                foreach (DataColumn col in ds.Tables[0].Columns)
                {
                    colIndex++;
                    excel.Cells[rowIndex, colIndex] = "'"+row[col.ColumnName].ToString();
                    //设置表格内容居中对齐
                   // xst.get_Range(excel.Cells[rowIndex, colIndex], excel.Cells[rowIndex, colIndex]).HorizontalAlignment = Excel.XlVAlign.xlVAlignCenter;
                }
            }

            excel.Visible = false;
            xbk.SaveAs(SaveFile, Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

            ds = null;
            xbk.Close(true, misValue, misValue);
            excel.Quit();
            PublicMethod.Kill(excel);//调用kill当前excel进程

            releaseObject(xst);
            releaseObject(xbk);
            releaseObject(excel);

        }
        

        private  void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }

        /// <summary>
        /// 创建一个Excel
        /// </summary>
        /// <returns></returns>
        public IWorkbook CreateExcel()
        {
            IWorkbook hssfworkbook = new HSSFWorkbook();

            ////create a entry of DocumentSummaryInformation
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "abc";

            ////create a entry of SummaryInformation
            SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
            si.Subject = "wq";

            //here, we must insert at least one sheet to the workbook. otherwise, Excel will say 'data lost in file'
            //So we insert three sheet just like what Excel does
            hssfworkbook.CreateSheet("Sheet1");
            hssfworkbook.CreateSheet("Sheet2");
            hssfworkbook.CreateSheet("Sheet3");
            hssfworkbook.CreateSheet("Sheet4");

            ((HSSFSheet)hssfworkbook.GetSheetAt(0)).AlternativeFormula = false;
            ((HSSFSheet)hssfworkbook.GetSheetAt(0)).AlternativeExpression = false;
            return hssfworkbook;

        }

        /// <summary>
        /// 保存excel
        /// </summary>
        /// <param name="url"></param>
        /// <param name="hssfworkbook"></param>
        public void Save(string url, IWorkbook hssfworkbook)
        {
            FileStream file = new FileStream(url, FileMode.Create);
            hssfworkbook.Write(file);
            file.Close();
        }


        /// <summary>
        /// 查找某字符串返回cell的ArrayList
        /// </summary>
        /// <param name="sht"></param>
        /// <param name="findStr"></param>
        /// <returns></returns>
        private ArrayList Find(ISheet sht,string findStr)
        {
            int rowsCount = sht.PhysicalNumberOfRows;
            ArrayList cellArr = new ArrayList();
            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                int colsCount = 0;
                if (sht.GetRow(rowIndex) != null)
                {
                    //取出此行的列数
                    colsCount = sht.GetRow(rowIndex).PhysicalNumberOfCells;
                }
                for (int colIndex = 0; colIndex < colsCount; colIndex++)
                {
                       //没有这一行则跳过
                        if (sht.GetRow(rowIndex) == null)
                        {
                            continue;
                        }
                        //没有这一列则跳过
                        if (sht.GetRow(rowIndex).GetCell(colIndex) == null)
                        {
                            continue;
                        }
                    ICell basecell = sht.GetRow(rowIndex).GetCell(colIndex);
                    string baseStr = basecell.ToString().Replace(" ", "").Trim();
                    if(string.Equals(baseStr,findStr))
                    {
                        cellArr.Add(basecell);
                    }
                    
                }
            }

            return cellArr;


        }

        /// <summary>
        /// 返回地址，房间，房间号,房号，办公室所在列
        /// </summary>
        /// <param name="sht"></param>
        /// <returns></returns>
        private Hashtable FindLocCellIndex(ISheet sht)
        {
            int rowsCount = sht.PhysicalNumberOfRows;
            Hashtable locCellName = new Hashtable();
            locCellName["地址"] = true;
            locCellName["房间"] = true;
            locCellName["房间号"] = true;
            locCellName["房号"] = true;
            locCellName["办公室"] = true;
            

            Hashtable findLocCellIndex = new Hashtable();
            for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
            {
                int colsCount = 0;
                if (sht.GetRow(rowIndex) != null)
                {
                    //取出此行的列数
                    colsCount = sht.GetRow(rowIndex).PhysicalNumberOfCells;
                }
                for (int colIndex = 0; colIndex < colsCount; colIndex++)
                {
                    //没有这一行则跳过
                    if (sht.GetRow(rowIndex) == null)
                    {
                        continue;
                    }
                    //没有这一列则跳过
                    if (sht.GetRow(rowIndex).GetCell(colIndex) == null)
                    {
                        continue;
                    }
                    ICell basecell = sht.GetRow(rowIndex).GetCell(colIndex);
                    string baseStr = basecell.ToString().Replace(" ", "").Trim();
                    if (locCellName.ContainsKey(baseStr))
                    {
                        findLocCellIndex[basecell.ColumnIndex] = true;
                    }

                }
            }

            return findLocCellIndex;


        }


        /// <summary>
        /// 通过找到的行，和姓名所在列查找电话并返回
        /// </summary>
        /// <param name="findRow"></param>
        /// <param name="nameCellIndex"></param>
        /// <returns></returns>
        public string FindTel(IRow findRow, int nameCellIndex)
        {
            string resTel = string.Empty;
            StringOp stringOp = new StringOp();
            //是否已跨过姓名列
            bool isSpanNam = false;
            //从姓名列跨出四列去找
            for (int i = nameCellIndex + 1; i <= nameCellIndex + 3; i++)
            {
                if (findRow.GetCell(i) == null)
                {
                    continue;
                }
                for(int j = 0;j < nameCellArr.Count;j++)
                {
                    if (i == ((ICell)nameCellArr[j]).ColumnIndex)
                    {
                        isSpanNam = true;
                        break;
                    }
                }
                if (isSpanNam)
                {
                    isSpanNam = false;
                    break;
                }
                string cellStr = findRow.GetCell(i).ToString();
                if (stringOp.IsTel(cellStr))
                {
                    cellStr = cellStr.Trim();
                    cellStr = cellStr.Replace("\r\n", "/");
                    cellStr = cellStr.Replace("\n", "/");
                    resTel = resTel + cellStr + "/";
                }

            }
            if (resTel.IndexOf("/") >= 0)
            {
                resTel = resTel.TrimEnd('/');
            }
                return resTel;
        }

        /// <summary>
        /// 查找电话躲开容易混淆的房间号
        /// </summary>
        /// <param name="findRow"></param>
        /// <param name="nameCellIndex"></param>
        /// <param name="locCellIndexHS"></param>
        /// <returns></returns>
        public string FindTel(IRow findRow, int nameCellIndex,Hashtable locCellIndexHS)
        {
            string resTel = string.Empty;
            StringOp stringOp = new StringOp();
            //从姓名列跨出三列去找
            for (int i = nameCellIndex + 1; i <= nameCellIndex + 3; i++)
            {
                if (findRow.GetCell(i) == null)
                {
                    continue;
                }
                string cellStr = findRow.GetCell(i).ToString();
                if (stringOp.IsTel(cellStr))
                {
                    if (!locCellIndexHS.ContainsKey(i))
                    {
                        resTel = resTel + cellStr + "/";
                    }
                }

            }
            if (resTel.IndexOf("/") >= 0)
            {
                resTel = resTel.TrimEnd('/');
            }
            return resTel;
        }



        /// <summary>
        /// 通过找到的行，和姓名所在列查找手机并返回
        /// </summary>
        /// <param name="findRow"></param>
        /// <param name="nameCellIndex"></param>
        /// <returns></returns>
        public string FindMpho(IRow findRow, int nameCellIndex)
        {
            string resMpho = string.Empty;
            StringOp stringOp = new StringOp();
            //从姓名列跨出四列去找
            for (int i = nameCellIndex + 1; i <= nameCellIndex + 4; i++)
            {
                if (findRow.GetCell(i) == null)
                {
                    continue;
                }
                string cellStr = findRow.GetCell(i).ToString();
                if (stringOp.IsMpho(cellStr))
                {
                    resMpho = cellStr ;
                    break;
                }

            }
            return resMpho;
        }


        /// <summary>
        /// 通过找到的行，和姓名所在列查找职别并返回
        /// </summary>
        /// <param name="findRow"></param>
        /// <param name="nameCellIndex"></param>
        /// <returns></returns>
        public string FindTitle(IRow findRow, int nameCellIndex)
        {
            string resTitle = string.Empty;
            StringOp stringOp = new StringOp();
            string cellStr = string.Empty; 
            //从姓名列左边一列去找
            if (findRow.GetCell(nameCellIndex - 1) != null)
            {


                cellStr = findRow.GetCell(nameCellIndex - 1).ToString().Replace(" ", "").Trim();
                if (stringOp.IsTitle(cellStr))
                {
                    resTitle = cellStr;
                    return resTitle;
                }
            }
                //从姓名列又边一列去找
            if (findRow.GetCell(nameCellIndex + 1) != null)
            {
                cellStr = findRow.GetCell(nameCellIndex + 1).ToString().Replace(" ", "").Trim();
                if (stringOp.IsTitle(cellStr))
                {
                    resTitle = cellStr;
                }
            }
            return resTitle;
        }

        /// <summary>
        /// 通过找到的办公地点所在列，查找办公地点
        /// </summary>
        /// <param name="findRow"></param>
        /// <param name="nameCellIndex"></param>
        /// <param name="locCellIndexHS"></param>
        /// <returns></returns>
        public string FindLoc(IRow findRow, int nameCellIndex,Hashtable locCellIndexHS)
        {
            string resLoc = string.Empty;
            StringOp stringOp = new StringOp();
            string cellStr = string.Empty;
            if (locCellIndexHS.Count == 0)
            {
                return resLoc;
            }
            //从姓名列右边2列去找
            if (locCellIndexHS.ContainsKey(nameCellIndex + 2))
            {
                ICell findCell = findRow.GetCell(nameCellIndex + 2);
                if (findCell != null)
                {
                    resLoc = findCell.ToString().Replace(" ", "").Trim();
                }
                return resLoc;
            }
            //从姓名列左边一列去找
            if (locCellIndexHS.ContainsKey(nameCellIndex - 1))
            {
                ICell findCell = findRow.GetCell(nameCellIndex - 1);
                if (findCell != null)
                {
                    resLoc = findCell.ToString().Replace(" ", "").Trim();
                }
                return resLoc;
            }
            for (int i = nameCellIndex + 1; i <= nameCellIndex + 6; i++)
            {
                if (locCellIndexHS.ContainsKey(i))
                {
                    ICell findCell = findRow.GetCell(i);
                    if (findCell != null)
                    {
                        resLoc = findCell.ToString().Replace(" ", "").Trim();
                    }
                    return resLoc;
                }
            }
            
            return resLoc;
        }

        /// <summary>
        /// 通过找到所在机构并返回
        /// </summary>
        /// <param name="findRow"></param>
        /// <param name="nameCellIndex"></param>
        /// <returns></returns>
        public string FindOrg(IRow findRow, int nameCellIndex)
        {
            string resOrg = string.Empty;
            StringOp stringOp = new StringOp();
            string cellStr = string.Empty;
            //从姓名列左边一列去找
            if (findRow.GetCell(nameCellIndex - 1) != null)
            {
                cellStr = findRow.GetCell(nameCellIndex - 1).ToString().Replace(" ", "").Replace("\n", "").Replace("\r\n", "").Trim();
                if (stringOp.IsOrg(cellStr))
                {
                    resOrg = stringOp.GetOrgStr(cellStr);
                    return resOrg;
                }
            }
            //从姓名列做边第二列去找
            if (findRow.GetCell(nameCellIndex - 2) != null)
            {
                cellStr = findRow.GetCell(nameCellIndex - 2).ToString().Replace(" ", "").Replace("\n","").Replace("\r\n","").Trim();
                if (stringOp.IsOrg(cellStr))
                {
                    resOrg = stringOp.GetOrgStr(cellStr);
                    return resOrg;
                }
            }
            //如果都找不到，从第一列找
            if (findRow.GetCell(0) != null)
            {
                cellStr = findRow.GetCell(0).ToString().Replace(" ", "").Replace("\n","").Replace("\r\n","").Trim();
                if (stringOp.IsOrg(cellStr))
                {
                    resOrg = stringOp.GetOrgStr(cellStr);
                    return resOrg;
                }
            }
            return resOrg;
        }

        /// <summary>
        /// 通过找到所在机构并返回
        /// </summary>
        /// <param name="findRow"></param>
        /// <param name="nameRowIndex"></param>
        /// <returns></returns>
        public string FindOrg(ISheet sht, int nameCellIndex, int nameRowIndex)
        {
            string resOrg = string.Empty;
            StringOp stringOp = new StringOp();
            string cellStr = string.Empty;
            //循环向上去找是否有匹配机构的，找到则返回
            for (int i = nameRowIndex-1; i > 0; i--)
            {
                
                IRow findRow = sht.GetRow(i);
                if (findRow == null)
                {
                    continue;
                }
                ICell findCeel = findRow.GetCell(nameCellIndex);
                if (findCeel == null)
                {
                    continue;
                }
                cellStr = findCeel.ToString().Replace(" ", "").Replace("\n","").Replace("\r\n","").Trim();
                if (stringOp.IsOrg(cellStr))
                {
                    resOrg = stringOp.GetOrgStr(cellStr);
                    return resOrg;
                }

            }
            return resOrg;
        }

        /// <summary>
        /// 处理telworkbook，将按orgnam排序的IWorkbook返回,并加入下行
        /// 姓名 电话 手机 房间 组别 职别
        /// </summary>
        /// <param name="telworkbook"></param>
        /// <param name="orgSheetNamArr"></param>
        /// <returns></returns>
        private IWorkbook GetTelFormatOrderByOrg(IWorkbook wb, ArrayList orgSheetNamArr)
        {
            IWorkbook telworkbook = CreateExcel();
            int sheetCount = wb.NumberOfSheets;
            for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
            {
                if (sheetIndex >= orgSheetNamArr.Count)
                {
                    continue;
                }
                int telworkbookShCount =0;

                 ISheet sht = wb.GetSheetAt(sheetIndex);
                 if (sht == null)
                 {
                     continue;
                 }
                 int rowsCount = sht.PhysicalNumberOfRows;
                 telworkbook.SetSheetName(sheetIndex, sht.SheetName);
                
                 ArrayList orgnamArr = (ArrayList)orgSheetNamArr[sheetIndex];
                 //加入一行title列
                 IRow irowTitle = telworkbook.GetSheetAt(sheetIndex).CreateRow(telworkbookShCount);
                 telworkbookShCount++;
                 ICell icellTitleName = irowTitle.CreateCell(0);
                 ICell icellTitleTel = irowTitle.CreateCell(1);
                 ICell icellTitleMpho = irowTitle.CreateCell(2);
                 ICell icellTitleLoc = irowTitle.CreateCell(3);
                 ICell icellTitleOrg = irowTitle.CreateCell(4);
                 ICell icellTitleTitle = irowTitle.CreateCell(5);
                 icellTitleName.SetCellValue("姓名");
                 icellTitleTel.SetCellValue("电话");
                 icellTitleMpho.SetCellValue("手机");
                 icellTitleLoc.SetCellValue("房间");
                 icellTitleOrg.SetCellValue("组别");
                 icellTitleTitle.SetCellValue("职别");

                 //按orgnam查找wb中的机构名称，按orgnamArr中的顺序重新排序
                 for (int i = 0; i < orgnamArr.Count; i++)
                 {
                     for (int j = 0; j < rowsCount; j++)
                     {
                         if (sht.GetRow(j) == null)
                         {
                             continue;
                         }
                         if (sht.GetRow(j).GetCell(0) == null || sht.GetRow(j).GetCell(1) == null || sht.GetRow(j).GetCell(2) == null || sht.GetRow(j).GetCell(3) == null || sht.GetRow(j).GetCell(4) == null || sht.GetRow(j).GetCell(5) == null)
                         {
                             continue;
                         }

                         if (string.Equals((string)orgnamArr[i], sht.GetRow(j).GetCell(4).ToString()))
                         {
                             //如果sht机构名与orgnamArr[i]中的相同，则加入telworkbook
                             IRow irow = telworkbook.GetSheetAt(sheetIndex).CreateRow(telworkbookShCount);
                             telworkbookShCount++;
                             ICell icellName = irow.CreateCell(0);
                             ICell icellTel = irow.CreateCell(1);
                             ICell icellMpho = irow.CreateCell(2);
                             ICell icellLoc = irow.CreateCell(3);
                             ICell icellOrg = irow.CreateCell(4);
                             ICell icellTitle = irow.CreateCell(5);
                             icellName.SetCellValue(sht.GetRow(j).GetCell(0).ToString());
                             icellTel.SetCellValue(sht.GetRow(j).GetCell(1).ToString());
                             icellMpho.SetCellValue(sht.GetRow(j).GetCell(2).ToString());
                             icellLoc.SetCellValue(sht.GetRow(j).GetCell(3).ToString());
                             icellOrg.SetCellValue(sht.GetRow(j).GetCell(4).ToString());
                             icellTitle.SetCellValue(sht.GetRow(j).GetCell(5).ToString());
                         }
                     }
                 }
            }

            return telworkbook;


        }

        /// <summary>
        /// 处理没有跨行跨列的excel,把它处理为以下形式
        /// 姓名 电话 手机 房间 组别 职别
        /// </summary>
        /// <returns></returns>
        public IWorkbook GetTelFormat(IWorkbook wb,Hashtable topOrgShHS,string orgex)
        {
            try
            {
                IWorkbook telworkbook = CreateExcel();
                StringOp stringop = new StringOp();
                int sheetCount = wb.NumberOfSheets;
                //记录每个sheet包含的orgNamArr
                ArrayList orgSheetArr = new ArrayList();
                for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
                {
                    //记录本sheet页有每个机构名称
                    ArrayList orgNamArr = new ArrayList();
                    ISheet sht = wb.GetSheetAt(sheetIndex);
                    if (sheetIndex > 3)
                    {
                        sht = telworkbook.CreateSheet();
                        sht = wb.GetSheetAt(sheetIndex);
                    }

                    telworkbook.SetSheetName(sheetIndex, sht.SheetName);
                    Hashtable locCellIndexHS = new Hashtable();
                    //返回姓名列cellindex
                    nameCellArr = Find(sht, "姓名");
                    locCellIndexHS = FindLocCellIndex(sht);
                    if (nameCellArr.Count < 1)
                    {
                        orgSheetArr.Add(orgNamArr);
                        continue;
                    }
                    //储存已处理的姓名列
                    Hashtable findCellIndexHS = new Hashtable();
                    //取行Excel的最大行数
                    int rowsCount = sht.PhysicalNumberOfRows;
                    int telworkbookShCount = 0;
                    for (int i = 0; i < nameCellArr.Count; i++)
                    {



                        ICell findNameCell = ((ICell)nameCellArr[i]);
                        if (findCellIndexHS.ContainsKey(findNameCell.ColumnIndex))
                        {
                            continue;
                        }

                        for (int j = findNameCell.RowIndex + 1; j < rowsCount; j++)
                        {
                            if (telworkbook.GetSheetAt(sheetIndex).GetRow(telworkbookShCount) != null)
                            {
                                continue;
                            }
                            
                            IRow findRow = sht.GetRow(j);
                            

                            if (findRow == null)
                            {
                                continue;
                            }
                            //上数1行，若上面有和自己同名的，则合并，电话号码
                            IRow findRowb1 = sht.GetRow(j-1);
                            string findRowb1Val = string.Empty;

                            ICell findCell = findRow.GetCell(findNameCell.ColumnIndex);
                            if (findCell == null)
                            {
                                continue;
                            }
                            if (findRowb1 != null)
                            {
                                ICell findCell1 = findRowb1.GetCell(findNameCell.ColumnIndex);
                                if (findCell1 != null)
                                {
                                    findRowb1Val = findCell1.ToString().Replace(" ", "").Trim();
                                }
                            }
                            
                            string cellNameVal = findCell.ToString().Replace(" ", "").Trim();
                            cellNameVal = cellNameVal.Replace("　", "");
                            string cellTitleVal = string.Empty;
                            //处理王 彤（处长）的情况，将姓名和职别分别提取出来
                            if (cellNameVal.IndexOf("（") > 0)
                            {
                                cellTitleVal = cellNameVal.Split('（')[1];
                                cellNameVal = cellNameVal.Split('（')[0];
                                if (cellTitleVal.IndexOf("）") > 0)
                                {
                                    cellTitleVal = cellTitleVal.Split('）')[0];
                                }
                                else if (cellTitleVal.IndexOf(")") > 0)
                                {
                                    cellTitleVal = cellTitleVal.Split(')')[0];
                                }
                            }
                            else if (cellNameVal.IndexOf("(") > 0)
                            {
                                cellTitleVal = cellNameVal.Split('(')[1];
                                cellNameVal = cellNameVal.Split('(')[0];
                                if (cellTitleVal.IndexOf(")") > 0)
                                {
                                    cellTitleVal = cellTitleVal.Split(')')[0];
                                }
                                else if (cellTitleVal.IndexOf("）") > 0)
                                {
                                    cellTitleVal = cellTitleVal.Split('）')[0];
                                }
                            }
                            
                            if (!stringop.IsName(cellNameVal))
                            {
                                continue;
                            }
                            //如果提取出的姓名是职别名称，则把该姓名加到上一行的职别列
                            if (stringop.IsTitle(cellNameVal))
                            {
                                telworkbook.GetSheetAt(sheetIndex).GetRow(telworkbookShCount-1).GetCell(5).SetCellValue(cellNameVal);
                                //如果该列的下一列是电话，这把该电话加到上一行的电话列
                                string spanTel = findRow.GetCell(findNameCell.ColumnIndex + 1).ToString().Replace(" ", "").Trim();
                                if (stringop.IsTel(spanTel))
                                {
                                    //取上一行的值
                                   string talb1 = telworkbook.GetSheetAt(sheetIndex).GetRow(telworkbookShCount - 1).GetCell(1).ToString();
                                   telworkbook.GetSheetAt(sheetIndex).GetRow(telworkbookShCount - 1).GetCell(1).SetCellValue(talb1 + "/" + spanTel);

                                }
                                continue;
                            }
                            string cellTelVal = string.Empty;
                            //如果没有办公地点列，查找电话列
                            if (locCellIndexHS.Count == 0)
                            {
                               cellTelVal = FindTel(findRow, findNameCell.ColumnIndex);
                            }
                            else//如果有办公地点列，查找电话列
                            {
                               cellTelVal = FindTel(findRow, findNameCell.ColumnIndex, locCellIndexHS);
                            }
                            if (cellNameVal == findRowb1Val )
                            {
                                IRow rowb1 = telworkbook.GetSheetAt(sheetIndex).GetRow(telworkbookShCount - 1);
                                if (rowb1 != null)
                                {
                                    ICell cellb1Tel = rowb1.GetCell(1);
                                    if (cellb1Tel != null)
                                    {
                                        if (!string.IsNullOrEmpty(cellTelVal))
                                        {
                                            cellb1Tel.SetCellValue(cellb1Tel + "/" + cellTelVal);
                                        }
                                    }
                                }
                                continue;
                            }

                            IRow irow = telworkbook.GetSheetAt(sheetIndex).CreateRow(telworkbookShCount);
                            telworkbookShCount++;
                            ICell icellName = irow.CreateCell(0);
                            ICell icellTel = irow.CreateCell(1);
                            ICell icellMpho = irow.CreateCell(2);
                            ICell icellLoc = irow.CreateCell(3);
                            ICell icellOrg = irow.CreateCell(4);
                            ICell icellTitle = irow.CreateCell(5);
                            
                            icellName.SetCellValue(cellNameVal);
                            //如果没有办公地点列，查找电话列
                            if (locCellIndexHS.Count == 0)
                            {
                                icellTel.SetCellValue(cellTelVal);
                            }
                            else//如果有办公地点列，查找电话列
                            {
                                icellTel.SetCellValue(cellTelVal);
                            }

                            icellMpho.SetCellValue(FindMpho(findRow, findNameCell.ColumnIndex));
                            icellLoc.SetCellValue(FindLoc(findRow, findNameCell.ColumnIndex, locCellIndexHS));
                            //机构名称列
                            string orgnam = string.Empty;
                            if (topOrgShHS.ContainsKey(telworkbook.GetSheetAt(sheetIndex).SheetName))
                            {
                                orgnam = FindOrg(sht, findNameCell.ColumnIndex, findCell.RowIndex);
                                
                            }
                            else
                            {
                                orgnam = FindOrg(findRow, findNameCell.ColumnIndex);
                            }
                            if (!string.IsNullOrEmpty(orgex))
                            {
                                orgnam = orgnam + orgex;
                            }

                            icellOrg.SetCellValue(orgnam);
                            //将不重复的机构名称加入orgNamArr
                            if (!orgNamArr.Contains(orgnam))
                            {
                                orgNamArr.Add(orgnam);
                            }
                            if (string.IsNullOrEmpty(cellTitleVal))
                            {
                                if (!stringop.IsTitle(cellTitleVal))
                                {
                                    cellTitleVal = string.Empty;
                                }
                                cellTitleVal = FindTitle(findRow, findNameCell.ColumnIndex);
                            }
                            icellTitle.SetCellValue(cellTitleVal);
                        }
                        findCellIndexHS[findNameCell.ColumnIndex] = true;
                        
                       
                       
                    }
                    orgSheetArr.Add(orgNamArr);


                }
                //将各sheet页按orgnam重新整理，将orgnam相同的合并在一块,并加入行
                //姓名 电话 手机 房间 组别 职别
                telworkbook = GetTelFormatOrderByOrg(telworkbook, orgSheetArr);
                return telworkbook;
            }
            catch (Exception ex)
            {
                IWorkbook telworkbook = CreateExcel();
                return telworkbook;
            }
 
        }

        /// <summary>
        /// 为某sheet页,某列添加一个后缀
        /// </summary>
        /// <param name="wb"></param>
        /// <param name="sheetIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="addexStr"></param>
        /// <returns></returns>
        public IWorkbook AddEndForColumn(IWorkbook wb,int sheetIndex,int columnIndex,string addexStr)
        {
            try
            {
                StringOp stringop = new StringOp();
                int sheetCount = wb.NumberOfSheets;
                ISheet sht = wb.GetSheetAt(sheetIndex);
                //取行Excel的最大行数
                int rowsCount = sht.PhysicalNumberOfRows;
                for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                {
                    IRow findRow = sht.GetRow(rowIndex);
                    if (findRow == null)
                    {
                        continue;
                    }
                    //没有这一列则跳过
                    ICell findCell = findRow.GetCell(columnIndex);
                    if (findCell == null)
                    {
                        continue;
                    }
                    string findCellVal = findCell.ToString();
                    string myaddexStr = addexStr;
                    if (string.IsNullOrEmpty(findCellVal))
                    {
                        if (addexStr.IndexOf("/") == 0 || addexStr.IndexOf("\\") == 0)
                        {
                            myaddexStr = myaddexStr.Substring(1, addexStr.Length - 1);
                        }
                    }
                    string CellVal = findCell.ToString() + myaddexStr;
                    findCell.SetCellValue(CellVal);
                }


                return wb;
            }
            catch (Exception ex)
            {
                return wb;
            }

        }


        /// <summary>
        /// 通过hs表建立excel
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        private IWorkbook CreateExcelByHs(NoSortHashtable hs,string title)
        {

                IWorkbook iworkbook = CreateExcel();
                ISheet sht =  iworkbook.GetSheetAt(0);
               
                string[] titleArr = null;
                if (title.IndexOf(",") > 0)
                {
                    titleArr = title.Split(',');
                }
                else
                {
                    titleArr = new string[1];
                    titleArr[0] = title;
                }
                IRow titlerow = sht.CreateRow(0);
                for (int i = 0; i < titleArr.Length; i++)
                {
                    ICell titleCell = titlerow.CreateCell(i);
                    titleCell.SetCellValue(titleArr[i]);
                }
                int count = 1;
                foreach (string str in hs.Keys)
                {
                  IRow irow = sht.CreateRow(count);
                  ICell icellhskey = irow.CreateCell(0);
                  ICell icellhsval = irow.CreateCell(1);
                  icellhskey.SetCellValue(str);
                  icellhsval.SetCellValue(hs[str].ToString());
                  count ++;
                }
                return iworkbook;

        }

        /// <summary>
        /// 通过hs表建立excel
        /// </summary>
        /// <param name="path"></param>
        /// <param name="hs"></param>
        public string CreateExcelByHs(string fileURL, NoSortHashtable hs,string title)
        {
            try
            {
                IWorkbook iworkbook = CreateExcelByHs(hs, title);
                ExcelOp excelop = new ExcelOp();

                string destination = System.IO.Path.GetDirectoryName(fileURL);

                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);//创建文件夹
                }

                excelop.Save(fileURL, iworkbook);
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }


        /// <summary>
        /// 得到合并列信息
        /// </summary>
        /// <param name="tmpworkbook"></param>
        /// <param name="sht"></param>
        /// <param name="sheetIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        /// <param name="colspan"></param>
        /// <param name="rowspan"></param>
        /// <param name="isByRowMerged"></param>
        private void GetTdMergedInfo(IWorkbook tmpworkbook ,  ISheet sht, int sheetIndex, int rowIndex, int colIndex, out int colspan, out int rowspan, out bool isByRowMerged)
        {
            colspan = 1;
            rowspan = 1;
            isByRowMerged = false;
            int regionsCuont = sht.NumMergedRegions;
            CellRangeAddress region;
            for (int i = 0; i < regionsCuont; i++)
            {
                region = sht.GetMergedRegion(i);
                if (region.FirstRow == rowIndex && region.FirstColumn == colIndex)
                {
                    colspan = region.LastColumn - region.FirstColumn + 1;
                    rowspan = region.LastRow - region.FirstRow + 1;
                    string rowVal = sht.GetRow(rowIndex).GetCell(colIndex).ToString().Trim();
                    for (int j = region.FirstRow; j <= region.LastRow; j++)
                    {
                        IRow irow = tmpworkbook.GetSheetAt(sheetIndex).GetRow(j);
                        if (irow == null)
                        {
                            irow = tmpworkbook.GetSheetAt(sheetIndex).CreateRow(j);
                        }

                        for (int k = region.FirstColumn; k <= region.LastColumn; k++)
                        {
                            ICell icell = irow.GetCell(k);
                            if (icell == null)
                            {
                                icell = irow.CreateCell(k);
                            }
                            icell.SetCellValue(rowVal);
                        }
                    }

                    return;
                }
                else if (rowIndex > region.FirstRow && rowIndex <= region.LastRow && colIndex >= region.FirstColumn && colIndex <= region.LastColumn)
                {
                    isByRowMerged = true;
                }

            }
        }


        /// <summary>
        /// 生成非合并格式
        /// </summary>
        /// <param name="inFileName"></param>
        /// <param name="outFileName"></param>
        /// <returns></returns>
        public string CreateNoJoinCellStyle(string inFileName,string outFileName)
        {
            try
            {
                ExcelOp excelop = new ExcelOp();

                //获得当前程序存放目录
                string strRoot = AppDomain.CurrentDomain.BaseDirectory;
                string destination = strRoot + @"in";
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);//创建文件夹
                }
                string inFileUrl = destination + "\\" + inFileName;


                if (!File.Exists(inFileUrl))
                {
                    return "未找到:" + inFileUrl;
                }

                //获得当前程序存放目录
                IWorkbook wb = new HSSFWorkbook(new FileStream(inFileUrl, FileMode.Open));
                int sheetCount = wb.NumberOfSheets;
                IWorkbook tmpworkbook = excelop.CreateExcel();
                for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
                {

                    ISheet sht = wb.GetSheetAt(sheetIndex);
                    if (sheetIndex > 3)
                    {
                        sht = tmpworkbook.CreateSheet();
                        sht = wb.GetSheetAt(sheetIndex);
                    }

                    tmpworkbook.SetSheetName(sheetIndex, sht.SheetName);
                    //取行Excel的最大行数
                    int rowsCount = sht.PhysicalNumberOfRows;
                    //为保证Table布局与Excel一样，这里应该取所有行中的最大列数（需要遍历整个Sheet）。
                    //为少一交全Excel遍历，提高性能，我们可以人为把第0行的列数调整至所有行中的最大列数。


                    int colSpan;
                    int rowSpan;
                    bool isByRowMerged;

                    for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                    {
                        int colsCount = 0;
                        if (sht.GetRow(rowIndex) != null)
                        {
                            colsCount = sht.GetRow(rowIndex).PhysicalNumberOfCells;
                        }
                        for (int colIndex = 0; colIndex < colsCount; colIndex++)
                        {
                            //没有这一行则跳过
                            if (sht.GetRow(rowIndex) == null)
                            {
                                continue;
                            }
                            //没有这一列则跳过
                            if (sht.GetRow(rowIndex).GetCell(colIndex) == null)
                            {
                                continue;
                            }
                            if (sht.GetRow(rowIndex).GetCell(colIndex).IsMergedCell)
                            {
                                GetTdMergedInfo(tmpworkbook, sht, sheetIndex, rowIndex, colIndex, out colSpan, out rowSpan, out isByRowMerged);
                                continue;
                            }
                            //如果已经被行合并包含进去了就不输出TD了。
                            //注意被合并的行或列不输出的处理方式不一样，见下面一处的注释说明了列合并后不输出TD的处理方式。


                            IRow irow = tmpworkbook.GetSheetAt(sheetIndex).GetRow(rowIndex);
                            if (irow == null)
                            {
                                irow = tmpworkbook.GetSheetAt(sheetIndex).CreateRow(rowIndex);
                            }
                            ICell icell = irow.GetCell(colIndex);
                            if (icell == null)
                            {
                                icell = irow.CreateCell(colIndex);
                            }
                           
                            ICell sourceCell = sht.GetRow(rowIndex).GetCell(colIndex);
                            //设置格式
                            ICellStyle cellStyle = tmpworkbook.CreateCellStyle();
                            cellStyle.DataFormat = sourceCell.CellStyle.DataFormat;
                            icell.CellStyle = cellStyle;
                            //设置类型
                            icell.SetCellType(sourceCell.CellType);
                            switch (sourceCell.CellType.ToString())
                            {
                                case "STRING" :
                                    icell.SetCellValue(sourceCell.StringCellValue);
                                    break;
                                case "NUMERIC" :
                                    icell.SetCellValue(sourceCell.NumericCellValue);
                                    break;
                                case "BOOLEAN" :
                                    icell.SetCellValue(sourceCell.BooleanCellValue);
                                    break;
                                case "BLANK" :
                                    icell.SetCellValue("");
                                    break;
                                default :
                                    icell.SetCellValue(sourceCell.ToString().Trim());
                                    break;

                            }
                           

                        }
                    }


                }

               

                destination = strRoot + @"out";

                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);//创建文件夹
                }

                string outfileURL = destination + "\\" + outFileName;
                excelop.Save(outfileURL, tmpworkbook);
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 合并单元格以全路径的格式
        /// </summary>
        /// <param name="inFilePath"></param>
        /// <param name="outFilePath"></param>
        /// <returns></returns>
        public string CreateNoJoinCellStyleALLPath(string inFilePath, string outFilePath)
        {
            try
            {
                ExcelOp excelop = new ExcelOp();


                if (!File.Exists(inFilePath))
                {
                    return "未找到:" + inFilePath;
                }

                //获得当前程序存放目录
                IWorkbook wb = new HSSFWorkbook(new FileStream(inFilePath, FileMode.Open));
                int sheetCount = wb.NumberOfSheets;
                IWorkbook tmpworkbook = excelop.CreateExcel();
                for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
                {

                    ISheet sht = wb.GetSheetAt(sheetIndex);
                    if (sheetIndex > 3)
                    {
                        sht = tmpworkbook.CreateSheet();
                        sht = wb.GetSheetAt(sheetIndex);
                    }

                    tmpworkbook.SetSheetName(sheetIndex, sht.SheetName);
                    //取行Excel的最大行数
                    int rowsCount = sht.PhysicalNumberOfRows;
                    //为保证Table布局与Excel一样，这里应该取所有行中的最大列数（需要遍历整个Sheet）。
                    //为少一交全Excel遍历，提高性能，我们可以人为把第0行的列数调整至所有行中的最大列数。


                    int colSpan;
                    int rowSpan;
                    bool isByRowMerged;
                    //设置格式
                    ICellStyle cellStyle = tmpworkbook.CreateCellStyle();
                    for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                    {
                        int colsCount = 0;
                        if (sht.GetRow(rowIndex) != null)
                        {
                            colsCount = sht.GetRow(rowIndex).PhysicalNumberOfCells;
                        }
                        for (int colIndex = 0; colIndex < colsCount; colIndex++)
                        {
                            //没有这一行则跳过
                            if (sht.GetRow(rowIndex) == null)
                            {
                                continue;
                            }
                            //没有这一列则跳过
                            if (sht.GetRow(rowIndex).GetCell(colIndex) == null)
                            {
                                continue;
                            }
                            if (sht.GetRow(rowIndex).GetCell(colIndex).IsMergedCell)
                            {
                                GetTdMergedInfo(tmpworkbook, sht, sheetIndex, rowIndex, colIndex, out colSpan, out rowSpan, out isByRowMerged);
                                continue;
                            }
                            //如果已经被行合并包含进去了就不输出TD了。
                            //注意被合并的行或列不输出的处理方式不一样，见下面一处的注释说明了列合并后不输出TD的处理方式。


                            IRow irow = tmpworkbook.GetSheetAt(sheetIndex).GetRow(rowIndex);
                            if (irow == null)
                            {
                                irow = tmpworkbook.GetSheetAt(sheetIndex).CreateRow(rowIndex);
                            }
                            ICell icell = irow.GetCell(colIndex);
                            if (icell == null)
                            {
                                icell = irow.CreateCell(colIndex);
                            }

                            ICell sourceCell = sht.GetRow(rowIndex).GetCell(colIndex);
                            cellStyle.DataFormat = sourceCell.CellStyle.DataFormat;
                            icell.CellStyle = cellStyle;
                            //设置类型
                            icell.SetCellType(sourceCell.CellType);
                            switch (sourceCell.CellType.ToString())
                            {
                                case "STRING":
                                    icell.SetCellValue(sourceCell.StringCellValue);
                                    break;
                                case "NUMERIC":
                                    icell.SetCellValue(sourceCell.NumericCellValue);
                                    break;
                                case "BOOLEAN":
                                    icell.SetCellValue(sourceCell.BooleanCellValue);
                                    break;
                                case "BLANK":
                                    icell.SetCellValue("");
                                    break;
                                default:
                                    icell.SetCellValue(sourceCell.ToString().Trim());
                                    break;

                            }


                        }
                    }


                }




                excelop.Save(outFilePath, tmpworkbook);
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }



        public string ARowToColumnEnd(string inFilePath, string outFilePath,string flagColumnName)
        {
            try
            {
                ExcelOp excelop = new ExcelOp();


                if (!File.Exists(inFilePath))
                {
                    return "未找到:" + inFilePath;
                }

                //获得当前程序存放目录
                IWorkbook wb = new HSSFWorkbook(new FileStream(inFilePath, FileMode.Open));
                int sheetCount = wb.NumberOfSheets;
                IWorkbook tmpworkbook = excelop.CreateExcel();
                for (int sheetIndex = 0; sheetIndex < sheetCount; sheetIndex++)
                {

                    ISheet sht = wb.GetSheetAt(sheetIndex);
                    if (sheetIndex > 3)
                    {
                        sht = tmpworkbook.CreateSheet();
                        sht = wb.GetSheetAt(sheetIndex);
                    }

                    tmpworkbook.SetSheetName(sheetIndex, sht.SheetName);
                    //取行Excel的最大行数
                    int rowsCount = sht.PhysicalNumberOfRows;
                    //为保证Table布局与Excel一样，这里应该取所有行中的最大列数（需要遍历整个Sheet）。
                    //为少一交全Excel遍历，提高性能，我们可以人为把第0行的列数调整至所有行中的最大列数。
                    //设置格式
                    ICellStyle cellStyle = tmpworkbook.CreateCellStyle();
                    for (int rowIndex = 0; rowIndex < rowsCount; rowIndex++)
                    {
                        int colsCount = 0;
                        if (sht.GetRow(rowIndex) != null)
                        {
                            colsCount = sht.GetRow(rowIndex).PhysicalNumberOfCells;
                        }
                        for (int colIndex = 0; colIndex < colsCount; colIndex++)
                        {
                            //没有这一行则跳过
                            if (sht.GetRow(rowIndex) == null)
                            {
                                continue;
                            }
                            //没有这一列则跳过
                            if (sht.GetRow(rowIndex).GetCell(colIndex) == null)
                            {
                                continue;
                            }

                            IRow irow = tmpworkbook.GetSheetAt(sheetIndex).GetRow(rowIndex);
                            if (irow == null)
                            {
                                irow = tmpworkbook.GetSheetAt(sheetIndex).CreateRow(rowIndex);
                            }
                            ICell icell = irow.GetCell(colIndex);
                            if (icell == null)
                            {
                                icell = irow.CreateCell(colIndex);
                            }
                            ICell sourceCell = sht.GetRow(rowIndex).GetCell(colIndex);
                            cellStyle.DataFormat = sourceCell.CellStyle.DataFormat;
                            icell.CellStyle = cellStyle;
                            //设置类型
                            icell.SetCellType(sourceCell.CellType);
                            switch (sourceCell.CellType.ToString())
                            {
                                case "STRING":
                                    icell.SetCellValue(sourceCell.StringCellValue);
                                    break;
                                case "NUMERIC":
                                    icell.SetCellValue(sourceCell.NumericCellValue);
                                    break;
                                case "BOOLEAN":
                                    icell.SetCellValue(sourceCell.BooleanCellValue);
                                    break;
                                case "BLANK":
                                    icell.SetCellValue("");
                                    break;
                                default:
                                    icell.SetCellValue(sourceCell.ToString().Trim());
                                    break;

                            }

                            //如果找到标志cell，把cell上面的数值提下来放到第一列
                             ICell icellcompare =  sht.GetRow(rowIndex).GetCell(colIndex);
                            if(!string.Equals(sourceCell.CellType.ToString(),"STRING"))
                            {
                                continue;
                            }
                            if (string.Equals(flagColumnName, icellcompare.StringCellValue.Trim()))
                            {
                                IRow irow0 = tmpworkbook.GetSheetAt(sheetIndex).GetRow(rowIndex-1);
                                if (irow0 == null)
                                {
                                    irow0 = tmpworkbook.GetSheetAt(sheetIndex).CreateRow(rowIndex-1);
                                }
                                ICell icell0 = irow0.GetCell(colsCount);
                                if (icell0 == null)
                                {
                                    icell0 = irow0.CreateCell(colsCount);
                                }
                                icell0.SetCellValue("添加列1");

                                ICell icell00 = irow0.GetCell(colsCount + 1);
                                if (icell00 == null)
                                {
                                    icell00 = irow0.CreateCell(colsCount + 1);
                                }
                                icell00.SetCellValue("添加列2");

                                IRow irow1 = tmpworkbook.GetSheetAt(sheetIndex).GetRow(rowIndex);
                                if (irow1 == null)
                                {
                                    irow1 = tmpworkbook.GetSheetAt(sheetIndex).CreateRow(rowIndex);
                                }
                                ICell icell1 = irow1.GetCell(colsCount);
                                if (icell1 == null)
                                {
                                    icell1 = irow1.CreateCell(colsCount);
                                }
                                icell1.SetCellValue("添加列");

                                IRow irow11 = tmpworkbook.GetSheetAt(sheetIndex).GetRow(rowIndex);
                                if (irow11 == null)
                                {
                                    irow11 = tmpworkbook.GetSheetAt(sheetIndex).CreateRow(rowIndex);
                                }
                                ICell icell11= irow11.GetCell(colsCount +1);
                                if (icell11 == null)
                                {
                                    icell11= irow11.CreateCell(colsCount +1);
                                }
                                icell1.SetCellValue("添加列1");
                                icell11.SetCellValue("添加列2");
                                string addColumnVal = string.Empty;
                                try
                                {
                                   addColumnVal = sht.GetRow(rowIndex - 1).GetCell(colIndex).StringCellValue.Trim();
                                }
                                catch
                                {
                                    continue;
                                }
                                for (int k = rowIndex + 1; k < rowsCount; k++)
                                {
                                    IRow irow2 = tmpworkbook.GetSheetAt(sheetIndex).GetRow(k);
                                    if (irow2 == null)
                                    {
                                        irow2 = tmpworkbook.GetSheetAt(sheetIndex).CreateRow(k);
                                    }
                                    ICell icell2 = irow2.GetCell(colsCount);
                                    if (icell2 == null)
                                    {
                                        icell2 = irow2.CreateCell(colsCount);
                                    }
                                    icell2.SetCellType(CellType.STRING);
                                    ICell icell3 = irow2.GetCell(colsCount+1);
                                    if (icell3 == null)
                                    {
                                        icell3 = irow2.CreateCell(colsCount+1);
                                    }
                                    icell2.SetCellType(CellType.STRING);
                                    icell3.SetCellType(CellType.STRING);
                                    string icell2val = string.Empty;
                                    string icell3val = string.Empty;
                                    if (addColumnVal.IndexOf("：") > 0)
                                    {
                                        icell2val = addColumnVal.Split('：')[0];
                                        icell3val = addColumnVal.Split('：')[1];

                                    }
                                    else if (addColumnVal.IndexOf(":") > 0)
                                    {
                                        icell2val = addColumnVal.Split(':')[0];
                                        icell3val = addColumnVal.Split(':')[1];
                                    }
                                    else
                                    {
                                        icell2val = addColumnVal;
                                        icell3val = string.Empty;
                                    }
                                   
                                    icell2.SetCellValue(icell2val);
                                    icell3.SetCellValue(icell3val);
                                    
                                }

                            }
                            
                            
                           



                           
                            


                        }
                    }


                }




                excelop.Save(outFilePath, tmpworkbook);
                return "0";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }



        /// <summary>
        /// 从指定的路径获取IWorkbook
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public IWorkbook GetWorkbook(string filepath,out string errmsg)
        {
            IWorkbook wb = null;
            errmsg = string.Empty;
            try
            {
                       
                if (!File.Exists(filepath))
                {
                    errmsg = "未找到:" + filepath;
                    return wb;
                }
                
                wb = new HSSFWorkbook(new FileStream(filepath, FileMode.Open));
                errmsg = "0";
                return wb;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return wb;
            }

        }





       






    }




}

public class PublicMethod
{
    [DllImport("User32.dll", CharSet = CharSet.Auto)]
    public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

    public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
    {
        IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口
        int k = 0;
        GetWindowThreadProcessId(t, out k);   //得到本进程唯一标志k
        System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
        p.Kill();     //关闭进程k
    }
}