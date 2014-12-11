using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace DAL
{
    /// <summary>
    /// wq文件操作类
    /// </summary>
    public class FileOp
    {
        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="fielname"></param>
        /// <param name="lineArr"></param>
        public void write(string filename,ArrayList lineArr)
        {
            
            try
            {
                //获得当前程序存放目录
                string strRoot = AppDomain.CurrentDomain.BaseDirectory;

                string destination = strRoot + @"out";

                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);//创建文件夹
                }
                string fileURL = destination + "\\" + filename;

                //Open the File  
                FileStream myStream = new FileStream(fileURL, FileMode.Create);
                StreamWriter sw = new StreamWriter(myStream, Encoding.UTF8);


                for (int i = 0; i < lineArr.Count; i++)
                {
                    sw.WriteLine(lineArr[i].ToString());
                }

                //close the file 
                sw.Flush();
                sw.Close();
                myStream.Close();
            }
            catch (Exception e)
            {
                
            }
            finally
            {
                  //MessageBox.Show("Executing finally block.");
            }  
        }

        /// <summary>
        /// 以追加方式写入数据
        /// </summary>
        /// <param name="fielname"></param>
        /// <param name="lineArr"></param>
        public void writeAdd(string filename, ArrayList lineArr)
        {

            try
            {
                //获得当前程序存放目录
                string strRoot = AppDomain.CurrentDomain.BaseDirectory;
                string destination = strRoot + @"out";
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);//创建文件夹
                }
                string fileURL = destination + "\\" + filename;
                //Open the File  
                FileStream myStream = new FileStream(fileURL, FileMode.Append);
                StreamWriter sw = new StreamWriter(myStream, Encoding.UTF8);
                for (int i = 0; i < lineArr.Count; i++)
                {
                    sw.WriteLine(lineArr[i].ToString());
                }
                //close the file 
                sw.Flush();
                sw.Close();
                myStream.Close();
            }
            catch (Exception e)
            {

            }
            finally
            {
                //MessageBox.Show("Executing finally block.");
            }
        }

        public void writeLog(string filename, string logstr)
        {
            try
            {
                //获得当前程序存放目录
                string strRoot = AppDomain.CurrentDomain.BaseDirectory;
                string destination = strRoot + @"out";
                if (!Directory.Exists(destination))
                {
                    Directory.CreateDirectory(destination);//创建文件夹
                }
                string fileURL = destination + "\\" + filename;
                //Open the File  
                FileStream myStream = new FileStream(fileURL, FileMode.Append);
                StreamWriter sw = new StreamWriter(myStream, Encoding.UTF8);
                sw.WriteLine(logstr);                
                sw.Flush();
                sw.Close();
                myStream.Close();
            }
            catch (Exception e)
            {

            }            
        }

        /// <summary>
        /// 通过相对路径，读取文件，并将文件中每一行形成arraylist返回
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public ArrayList read(string filename ,bool bfullname = false)
        {
            try
            {
                string fileURL = string.Empty;
                ArrayList resArr = new ArrayList();
                if (true)
                {
                    fileURL = filename;
                }
                else
                {
                    //获得当前程序存放目录
                    string strRoot = AppDomain.CurrentDomain.BaseDirectory;

                    string destination = strRoot;

                    if (!Directory.Exists(destination))
                    {
                        Directory.CreateDirectory(destination);//创建文件夹
                    }
                    fileURL = destination + filename;                    
                }
                

                //Open the File  
                StreamReader myReader = new StreamReader(fileURL, Encoding.UTF8);

                while (myReader.Peek() > -1)
                {
                    string sql = myReader.ReadLine();
                    resArr.Add(sql);
                }

                myReader.Close();
                return resArr;
            }
            catch (Exception e)
            {
                ArrayList resArr = new ArrayList();
                return resArr;
            }
            finally
            {
                //MessageBox.Show("Executing finally block.");
            }
        }

        /// <summary>  
        /// 获得指定目录下的所有文件  
        /// </summary>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        public List<FileInfo> GetFilesByDir(string path)
        {
            DirectoryInfo di = new DirectoryInfo(path);

            //找到该目录下的文件  
            FileInfo[] fi = di.GetFiles();

            //把FileInfo[]数组转换为List  
            List<FileInfo> list = fi.ToList<FileInfo>();
            return list;
        }

        /// <summary>  
        /// 获得指定目录及其子目录的所有文件  
        /// </summary>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        public List<FileInfo> GetAllFilesByDir(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);

            //找到该目录下的文件  
            FileInfo[] fi = dir.GetFiles();

            //把FileInfo[]数组转换为List  
            List<FileInfo> list = fi.ToList<FileInfo>();

            //找到该目录下的所有目录里的文件  
            DirectoryInfo[] subDir = dir.GetDirectories();
            foreach (DirectoryInfo d in subDir)
            {
                List<FileInfo> subList = GetFilesByDir(d.FullName);
                foreach (FileInfo subFile in subList)
                {
                    list.Add(subFile);
                }
            }
            return list;
        }

        public static void Log(string logstr,string logFileName = null)
        {
            if (string.IsNullOrEmpty(logFileName))
            {
                logFileName = string.Format("log_{0}.txt", DateTime.Now.ToString("MMdd"));
            }
            string strRoot = AppDomain.CurrentDomain.BaseDirectory;
            string destination = strRoot+@"LOG";
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);//创建文件夹
            }
            string fileURL = destination + "\\"+logFileName;
            FileStream myStream = new FileStream(fileURL, FileMode.Append);
            StreamWriter sw = new StreamWriter(myStream, Encoding.UTF8);
            sw.WriteLine(logstr);
            sw.Flush();
            sw.Close();
            myStream.Close();
        }
    }
}
