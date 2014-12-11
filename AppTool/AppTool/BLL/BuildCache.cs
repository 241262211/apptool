using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;

namespace BLL
{
    public class BuildCache
    {
        /// <summary>
        /// 建立人员缓冲
        /// </summary>
        /// <returns></returns>
        public string  BuildUsr()
        {
            try
            {
                string res = string.Empty;
                HttpOp httpop = new HttpOp();
                string ip = init.Get("Server","ip");
                res = httpop.SendUrl("http://"+ip+"/Login/UpdUsr");
                return res;
            }
            catch (Exception ex)
            {     
                return ex.Message; 
            }
        }

        /// <summary>
        /// 重建机构缓冲
        /// </summary>
        /// <returns></returns>
        public string BuildOrg()
        {
            try
            {
                string res = string.Empty;
                HttpOp httpop = new HttpOp();
                string ip = init.Get("Server", "ip");
                res = httpop.SendUrl("http://" + ip + "/Login/UpdOrg");
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        /// <summary>
        /// 重建机构树缓冲
        /// </summary>
        /// <returns></returns>
        public string BuildOrgTree()
        {
            try
            {
                string res = string.Empty;
                HttpOp httpop = new HttpOp();
                string ip = init.Get("Server", "ip");
                res = httpop.SendUrl("http://" + ip + "/Login/UpdOrgTree");
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// 重建系统缓冲
        /// </summary>
        /// <returns></returns>
        public string BuildSys()
        {
            try
            {
                string res = string.Empty;
                HttpOp httpop = new HttpOp();
                string ip = init.Get("Server", "ip");
                res = httpop.SendUrl("http://" + ip + "/Login/UpdSys");
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// 重建系统树缓冲
        /// </summary>
        /// <returns></returns>
        public string BuildSysTree()
        {
            try
            {
                string res = string.Empty;
                HttpOp httpop = new HttpOp();
                string ip = init.Get("Server", "ip");
                res = httpop.SendUrl("http://" + ip + "/Login/UpdSysTree");
                return res;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }




    }
}
