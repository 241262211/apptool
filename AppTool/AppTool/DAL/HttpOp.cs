using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace  DAL
{
    public  class HttpOp
    {

        /// <summary>
        /// 发送url地址，并返回内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string SendUrl(string url)
        {
            try
            {
                string res = string.Empty;
                WebRequest myRequest = WebRequest.Create(url);
                myRequest.Method = "GET";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                // Return the response. 
                WebResponse myResponse = myRequest.GetResponse();
                // Code to use the WebResponse goes here.
                // Close the response to free resources.
                Stream instream = myResponse.GetResponseStream();
                Encoding encoding = Encoding.GetEncoding("UTF-8");
                StreamReader sr = new StreamReader(instream, encoding);

                //返回结果网页（html）代码      

                res = sr.ReadToEnd();
                instream.Close();
                myResponse.Close();
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




    }
}
