using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;


namespace BLL
{
    public class ExcelToPdf
    {


        /*
        /// <summary>
        /// 将excel转换为pdf
        /// </summary>
        /// <param name="srcFile"></param>
        /// <returns></returns>
        public string Save(string srcFile)
        {
            Excel.Application m_app = new Excel.Application();
            try
            {
                
                m_app.Visible = true;
                string fileName = Path.GetFileNameWithoutExtension(srcFile);
                string destPath = Path.Combine(Path.GetDirectoryName(srcFile), string.Format("{0}.pdf", fileName));
                m_app.ActiveWorkbook.ExportAsFixedFormat(Excel.XlFixedFormatType.xlTypePDF, destPath);
                return "0";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                m_app.Quit();
            }

        }
        */



    }
}
