using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAL;
using Model;
using System.IO;
using System.Data;
using System.Collections;
using LitJson;
using System.Net;
using AppTool;

namespace BLL
{
    public class PackParse
    {
        private int _artid = 200;
        public int p_artid
        {
            get{_artid++;return _artid;}
        }        
        private int _usrid = 1000;
        public int p_usrid
        {
            get { _usrid++; return _usrid; }
        }
        private int _shopid = 100;
        public int p_shopid
        {
            get { _shopid++; return _shopid; }
        }
        

        public bool parseCMJson(string packdir,out string errMsg)
        {
            errMsg = string.Empty;
            if (!Directory.Exists(packdir) && !Directory.Exists("C:\\Users\\Administrator\\Desktop\\pack"))
            {
                errMsg = "请输入正确的文件夹位置";
                return false;
            }
            if (string.IsNullOrEmpty(packdir))
            {
                packdir = "C:\\Users\\Administrator\\Desktop\\pack";
            }
            //string packdir = "C:\\Users\\Administrator\\Desktop\\pack\\";
            string strRoot = AppDomain.CurrentDomain.BaseDirectory;
            FileOp fileOp = new FileOp();
            ArrayList sqlArr = new ArrayList();
            try
            {
                

                List<FileInfo> shortFileArr = fileOp.GetFilesByDir(packdir + "\\short");
                List<FileInfo> detailFileInfoArr = fileOp.GetFilesByDir(packdir + "\\detail");
                List<JsonData> detailJsonArr = new List<JsonData>();
                foreach (FileInfo fi in detailFileInfoArr)
                {
                    ArrayList textArr = fileOp.read(fi.FullName, true);
                    if (textArr.Count == 1)
                    {
                        JsonData filetxt = JsonMapper.ToObject(textArr[0].ToString());
                        //JsonData content = JsonMapper.ToObject(filetxt["artinfo"]["newcontent"].ToString());
                        detailJsonArr.Add(filetxt["artinfo"]);
                    }
                    else
                    {
                        errMsg = "文件内容不合格式:" + fi.FullName;
                        return false;
                    }
                }
                //string shortjsontxt = "156_.txt";
                foreach (FileInfo fi in shortFileArr)
                {

                    ArrayList textArr = fileOp.read(fi.FullName, true);
                    JsonData data;
                    if (textArr.Count == 1)
                    {
                        data = JsonMapper.ToObject(textArr[0].ToString());
                    }
                    else
                    {
                        errMsg = "文件内容不合格式:" + fi.FullName;
                        return false;
                    }

                    //string value = jsonobj["status"].ToString();

                    int atb_artid = p_artid;
                    int atb_shopid = p_shopid;
                    string atb_shopname = data["secinfo"]["short_addr"].ToString();
                    string atb_homepic = string.Empty;  //
                    string atb_maintitle = data["secinfo"]["name"].ToString();
                    string atb_secondtitle = data["artinfo"][0]["art_title"].ToString();
                    string atb_sectiontype = data["secinfo"]["sec_typename"].ToString();
                    string atb_abstract = data["secinfo"]["abstract_content"].ToString();
                    string atb_toppic = data["secinfo"]["app_toppic"].ToString();
                    string atb_sharetxt = data["secinfo"]["wxsharetxt"].ToString();
                    int atb_sharecnt = 60;
                    int atb_viewcnt = 100;
                    int atb_likecnt = 20;
                    int atb_collectcnt = 30;
                    int atb_authorid = p_usrid;
                    string atb_createtime = DateTime.Now.ToString("yyyyMMdd");
                    string artsql = string.Format("insert into artbase values ({0},{1},\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",{10},{11},{12},{13},{14},\"{15}\",'');",
                        atb_artid, atb_shopid, atb_shopname, atb_homepic, atb_maintitle, atb_secondtitle, atb_sectiontype, atb_abstract, atb_toppic, atb_sharetxt, atb_sharecnt, atb_viewcnt, atb_likecnt, atb_collectcnt, atb_authorid, atb_createtime);
                    sqlArr.Add(artsql);

                    int usr_usrid = atb_authorid;
                    int usr_gender = int.Parse(data["artinfo"][0]["authorinfo"]["gender"].ToString());
                    int usr_status = 0;
                    string usr_name = data["artinfo"][0]["authorinfo"]["uname"].ToString();
                    string usr_role = data["artinfo"][0]["authorinfo"]["role"].ToString();
                    string usr_loginname = null;
                    string usr_password = null;
                    string usr_province = data["artinfo"][0]["authorinfo"]["provincename"].ToString();
                    string usr_city = data["artinfo"][0]["authorinfo"]["cityname"].ToString();
                    string usr_avatar = data["artinfo"][0]["authorinfo"]["uavatar"].ToString();
                    string usr_exptag = string.Empty;
                    if (data["artinfo"][0]["expuserinfo"].Count > 0)
                    {
                        usr_exptag = data["artinfo"][0]["expuserinfo"]["exptag"].ToString();
                    }                    
                    string usr_sumary = data["artinfo"][0]["authorinfo"]["describe"].ToString();
                    string usrsql = string.Format("insert into appuser values ({0},{1},{2},\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",'');",
                        usr_usrid, usr_gender, usr_status, usr_name, usr_role, usr_loginname, usr_password, usr_province, usr_city, usr_avatar, usr_exptag, usr_sumary);
                    sqlArr.Add(usrsql);

                    int shp_shopid = atb_shopid;
                    string shp_name = data["secinfo"]["short_addr"].ToString();
                    string shp_map = string.Empty, shp_mapaddr = string.Empty, shp_address = string.Empty, shp_price = string.Empty, shp_contact = string.Empty, shp_opentime = string.Empty;
                    JsonData tmpjd = data["secinfo"]["sec_attr"];
                    for (int i = 0; i < tmpjd.Count; i++)
                    {
                        if ("地图".Equals(tmpjd[i]["show_name"].ToString()))
                        {
                            shp_map = tmpjd[i]["content"]["lnglat"].ToString();
                            shp_mapaddr = tmpjd[i]["content"]["address"].ToString();
                        }
                        else if ("地址".Equals(tmpjd[i]["show_name"].ToString()))
                        {
                            shp_address = tmpjd[i]["content"].ToString();
                        }
                        else if ("时间".Equals(tmpjd[i]["show_name"].ToString()))
                        {
                            shp_opentime = tmpjd[i]["content"].ToString();
                        }
                        else if ("价格".Equals(tmpjd[i]["show_name"].ToString()))
                        {
                            shp_price = tmpjd[i]["content"].ToString();
                        }
                        else if ("联系".Equals(tmpjd[i]["show_name"].ToString()))
                        {
                            shp_contact = tmpjd[i]["content"].ToString();
                        }
                    }
                    //string shp_map = data["secinfo"]["sec_attr"][0]["content"]["gaodelnglat"].ToString();
                    //string shp_mapaddr = data["secinfo"]["sec_attr"][0]["content"]["address"].ToString();
                    //string shp_address = data["secinfo"]["sec_attr"][1]["content"].ToString();
                    //string shp_price = data["secinfo"]["sec_attr"][3]["content"].ToString();
                    //string shp_contact = data["secinfo"]["sec_attr"][4]["content"].ToString();
                    //string shp_opentime = data["secinfo"]["sec_attr"][2]["content"].ToString();
                    string shopsql = string.Format("insert into shop values ({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",'');",
                        shp_shopid, shp_name, shp_map, shp_mapaddr, shp_address, shp_price, shp_contact, shp_opentime);
                    sqlArr.Add(shopsql);

                    foreach (JsonData detailjd in detailJsonArr)
                    {
                        if (atb_secondtitle.Equals(detailjd["art_title"].ToString()))
                        {
                            int atc_artid = atb_artid;
                            JsonData newcontent = JsonMapper.ToObject(detailjd["newcontent"].ToString());
                            for (int i = 0; i < newcontent.Count; i++)
                            {
                                int atc_no = i;
                                string atc_type = string.Empty;
                                string atc_contentpart = string.Empty;
                                foreach (string key in newcontent[i].Inst_Object.Keys)
                                {
                                    if ("ch".Equals(key) || "pic".Equals(key))
                                    {
                                        atc_type = key;
                                        atc_contentpart = newcontent[i].Inst_Object[key].ToString();
                                    }
                                    else
                                    {
                                        errMsg = "键类型不是ch 也不是pic ,为：" + key;
                                        return false;
                                    }
                                }
                                string contentsql = string.Format("insert into artcontent values ({0},{1},\"{2}\",\"{3}\",'');",
                                    atc_artid,atc_no,atc_type,atc_contentpart);
                                sqlArr.Add(contentsql);
                            }
                            break;
                        }
                    }
                    
                }
                //sqlArr.Add(";");
                fileOp.write("app_insert_sql.txt", sqlArr);
                return true;
            }
            catch (Exception ex)
            {
                string errfile = new System.Diagnostics.StackFrame(true).GetFileName().ToString();
                string errline = new System.Diagnostics.StackFrame(true).GetFileLineNumber().ToString();
                errMsg = errfile + "  " + errline + "  " + ex.Message;
                return false;
            }
        }

        public bool snifferCMPacker(Form1 form ,out string errMsg)
        {
            errMsg = string.Empty;
            try
            {
                string host = "http://api.chengmi.com";
                string urlhome = "/index?passdate={0}";
                string urlsection = "/sectionv17?sectionid={0}&showusercnt=3";
                string urlarticle = "/articleinfov17?articleid={0}&showusercnt=3 ";
                FileOp fileOp = new FileOp();
                ArrayList sqlArr = new ArrayList();
                ArrayList logArr = new ArrayList();
                string logfilename  = string.Format("cm_log_{0}.txt",DateTime.Now.ToString("yyyyMMdd"));
                int progressvalue = 0;
                for (int n = 0; n < 10; n++)
                {
                    progressvalue = n * 10;
                    form.setCMProgressValue(progressvalue);
                    string date = DateTime.Now.AddDays(n-9).ToString("yyyyMMdd");
                    fileOp.writeLog(logfilename,string.Format("CM date:{0} started.",date));                    
                    string homestr = string.Empty;
                    if(!GetMothed(host + string.Format(urlhome, date),out homestr))
                    {
                        fileOp.writeLog(logfilename,"ERROR  GetMethod get homelist failed,date:"+date);
                        continue;
                    }
                    JsonData homeObj = JsonMapper.ToObject(homestr);                    
                    foreach (JsonData hDataItem in homeObj["sectioninfo"])
                    {
                        form.setCMProgressValue(progressvalue ++);
                        string sid = hDataItem["sid"].ToString();
                        fileOp.writeLog(logfilename, "CM section started,sid is:" + sid);
                        int atb_artid = p_artid;//int.Parse(sid);
                        string atb_homepic = hDataItem["pic"].ToString(); ;  //
                        string sectionstr = string.Empty;
                        if (!GetMothed(host + string.Format(urlsection, sid),out sectionstr))
                        {
                            fileOp.writeLog(logfilename, "ERROR  GetMethod get section failed,sectionid:" + sid);
                            continue;
                        }
                        JsonData sectionObj = JsonMapper.ToObject(sectionstr);
                        int atb_shopid = p_shopid;
                        string atb_shopname = sectionObj["secinfo"]["short_addr"].ToString();
                        string atb_maintitle = sectionObj["secinfo"]["name"].ToString();
                        string atb_secondtitle = sectionObj["artinfo"][0]["art_title"].ToString();
                        string atb_sectiontype = sectionObj["secinfo"]["sec_typename"].ToString();
                        string atb_abstract = sectionObj["secinfo"]["abstract_content"].ToString();
                        string atb_toppic = sectionObj["secinfo"]["app_toppic"].ToString();
                        string atb_sharetxt = sectionObj["secinfo"]["wxsharetxt"].ToString();
                        int atb_sharecnt = 60;
                        int atb_viewcnt = 100;
                        int atb_likecnt = 20;
                        int atb_collectcnt = 30;
                        int atb_authorid = p_usrid;//int.Parse(sectionObj["artinfo"][0]["authorinfo"]["userid"].ToString());
                        string atb_createtime = DateTime.Now.ToString("yyyyMMdd");
                        string artsql = string.Format("insert into artbase values ({0},{1},\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",{10},{11},{12},{13},{14},\"{15}\",'');",
                            atb_artid, atb_shopid, atb_shopname, atb_homepic, atb_maintitle, atb_secondtitle, atb_sectiontype, atb_abstract, atb_toppic, atb_sharetxt, atb_sharecnt, atb_viewcnt, atb_likecnt, atb_collectcnt, atb_authorid, atb_createtime);
                        sqlArr.Add(artsql);

                        int usr_usrid = atb_authorid;
                        int usr_gender = int.Parse(sectionObj["artinfo"][0]["authorinfo"]["gender"].ToString());
                        int usr_status = 0;
                        string usr_name = sectionObj["artinfo"][0]["authorinfo"]["uname"].ToString();
                        string usr_role = sectionObj["artinfo"][0]["authorinfo"]["role"].ToString();
                        string usr_loginname = null;
                        string usr_password = null;
                        string usr_province = sectionObj["artinfo"][0]["authorinfo"]["provincename"].ToString();
                        string usr_city = sectionObj["artinfo"][0]["authorinfo"]["cityname"].ToString();
                        string usr_avatar = sectionObj["artinfo"][0]["authorinfo"]["uavatar"].ToString();
                        string usr_exptag = string.Empty;
                        if (sectionObj["artinfo"][0]["expuserinfo"].Count > 0)
                        {
                            usr_exptag = sectionObj["artinfo"][0]["expuserinfo"]["exptag"].ToString();
                        }
                        string usr_sumary = sectionObj["artinfo"][0]["authorinfo"]["describe"].ToString();
                        string usrsql = string.Format("insert into appuser values ({0},{1},{2},\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",\"{8}\",\"{9}\",\"{10}\",\"{11}\",'');",
                            usr_usrid, usr_gender, usr_status, usr_name, usr_role, usr_loginname, usr_password, usr_province, usr_city, usr_avatar, usr_exptag, usr_sumary);
                        sqlArr.Add(usrsql);

                        int shp_shopid = atb_shopid;
                        string shp_name = sectionObj["secinfo"]["short_addr"].ToString();
                        string shp_map = string.Empty, shp_mapaddr = string.Empty, shp_address = string.Empty, shp_price = string.Empty, shp_contact = string.Empty, shp_opentime = string.Empty;
                        JsonData tmpjd = sectionObj["secinfo"]["sec_attr"];
                        for (int i = 0; i < tmpjd.Count; i++)
                        {
                            if ("地图".Equals(tmpjd[i]["show_name"].ToString()))
                            {
                                shp_map = tmpjd[i]["content"]["lnglat"].ToString();
                                shp_mapaddr = tmpjd[i]["content"]["address"].ToString();
                            }
                            else if ("地址".Equals(tmpjd[i]["show_name"].ToString()))
                            {
                                shp_address = tmpjd[i]["content"].ToString();
                            }
                            else if ("时间".Equals(tmpjd[i]["show_name"].ToString()))
                            {
                                shp_opentime = tmpjd[i]["content"].ToString();
                            }
                            else if ("价格".Equals(tmpjd[i]["show_name"].ToString()))
                            {
                                shp_price = tmpjd[i]["content"].ToString();
                            }
                            else if ("联系".Equals(tmpjd[i]["show_name"].ToString()))
                            {
                                shp_contact = tmpjd[i]["content"].ToString();
                            }
                        }
                        string shopsql = string.Format("insert into shop values ({0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\",\"{7}\",'');",
                            shp_shopid, shp_name, shp_map, shp_mapaddr, shp_address, shp_price, shp_contact, shp_opentime);
                        sqlArr.Add(shopsql);

                        string artid = sectionObj["artinfo"][0]["artid"].ToString();
                        string articlestr = string.Empty;
                        if (!GetMothed(host + string.Format(urlarticle, artid),out articlestr))
                        {
                            fileOp.writeLog(logfilename, "ERROR  GetMethod get article content failed,artid:" + artid);
                            sqlArr.Clear();
                            continue;
                        }
                        JsonData articleObj = JsonMapper.ToObject(articlestr);
                        JsonData newcontent = JsonMapper.ToObject(articleObj["artinfo"]["newcontent"].ToString());
                        int atc_artid = atb_artid;
                        for (int i = 0; i < newcontent.Count; i++)
                        {
                            int atc_no = i;
                            string atc_type = string.Empty;
                            string atc_contentpart = string.Empty;
                            foreach (string key in newcontent[i].Inst_Object.Keys)
                            {
                                if ("ch".Equals(key) || "pic".Equals(key))
                                {
                                    atc_type = key;
                                    atc_contentpart = newcontent[i].Inst_Object[key].ToString();
                                }
                            }
                            string contentsql = string.Format("insert into artcontent values ({0},{1},\"{2}\",\"{3}\",'');",
                                atc_artid, atc_no, atc_type, atc_contentpart);
                            sqlArr.Add(contentsql);
                        }
                        fileOp.writeAdd(string.Format("app_insert_sql_{0}.txt", DateTime.Now.ToString("yyyyMMdd")), sqlArr);
                        sqlArr.Clear();
                    }
                }
            }
            catch (System.Exception ex)
            {
                string errfile = new System.Diagnostics.StackFrame(true).GetFileName().ToString();
                string errline = new System.Diagnostics.StackFrame(true).GetFileLineNumber().ToString();
                errMsg = errfile + "  " + errline + "  " + ex.Message;
                return false;
            }
            return true;
        }

        public static bool GetMothed(string url ,out string result)
        {
            result = string.Empty;
            try
            {
                CookieCollection cl = new CookieCollection();
                cl.Add(new Cookie("ASP.NET_SessionId", "hvflbg2akylk2fddexyn4rmd"));
                CookieContainer cookie = new CookieContainer();
                cookie.Add(new Uri("http://api.chengmi.com"), cl);
                string strURL = url;
                System.Net.HttpWebRequest request;
                request = (System.Net.HttpWebRequest)WebRequest.Create(strURL);
                request.Method = "GET";
                request.ContentType = "application/json;charset=UTF-8";
                //request.CookieContainer = cookie;
                request.Timeout = 100000;
                request.KeepAlive = true;
                //request.Headers.Add("Pragma", "no-cache");
                request.Headers.Add("Accept-Encoding", "gzip");
                /*
                request.ProtocolVersion = HttpVersion.Version10;
                string paraUrlCoded = param;
                byte[] payload;
                payload = System.Text.Encoding.UTF8.GetBytes(paraUrlCoded);
                request.ContentLength = payload.Length;
                Stream writer = request.GetRequestStream();
                writer.Write(payload, 0, payload.Length);
                writer.Close();
                 */
                System.Net.HttpWebResponse response;
                response = (System.Net.HttpWebResponse)request.GetResponse();
                System.IO.Stream s;
                s = response.GetResponseStream();
                string StrData = "";                
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrData = Reader.ReadLine()) != null)
                {
                    result += StrData + "\r\n";
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }            
            return true;
        }

    }
}