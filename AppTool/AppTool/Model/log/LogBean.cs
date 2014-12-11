//----------------------------------------------------------------------------
// Copyright (C) 2011, AGRICULTURAL BANK OF CHINA, Corp. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    /// <summary>
    /// 日志表bean
    /// </summary>
    public class LogBean
    {
        /// <summary>
        /// 日志表
        /// </summary>
        public LogBean()
        {
        }
        #region LogModel


        /// <summary>
        /// 表名
        /// </summary>
        private string _tbnam = "AMWD";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Tbnam
        {
            get { return _tbnam; }
            set { _tbnam = value; }
        }

        /// <summary>
        /// 日期
        /// </summary>
        private string _jrndate;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _jrndate_dbnam = "AMWDJRNDATE";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Jrndate_dbnam
        {
            get { return _jrndate_dbnam; }
            set { _jrndate_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Jrndate
        {
            get { return _jrndate; }
            set { _jrndate = value; }
        }


        /// <summary>
        /// 日志号
        /// </summary>
        private int _jrnno;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _jrnno_dbnam = "AMWDJRNNO";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Jrnno_dbnam
        {
            get { return _jrnno_dbnam; }
            set { _jrnno_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public int Jrnno
        {
            get { return _jrnno; }
            set { _jrnno = value; }
        }



        /// <summary>
        /// 省市代码
        /// </summary>
        private string _procod;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _procod_dbnam = "AMWDPROCOD";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Procod_dbnam
        {
            get { return _procod_dbnam; }
            set { _procod_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Procod
        {
            get { return _procod; }
            set { _procod = value; }
        }


        /// <summary>
        /// 主键
        /// </summary>
        private string _key = "";
        /// <summary>
        /// 主键
        /// </summary>
        public string key
        {
            get { return _key; }
            set { _key = value; }
        }

        /// <summary>
        /// 部门代码
        /// </summary>
        private string _dptcod;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _dptcod_dbnam = "AMWDDPTCOD";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Dptcod_dbnam
        {
            get { return _dptcod_dbnam; }
            set { _dptcod_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Dptcod
        {
            get { return _dptcod; }
            set { _dptcod = value; }
        }

        /// <summary>
        /// 组码
        /// </summary>
        private string _grpcod;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _grpcod_dbnam = "AMWDGRPCOD";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Grpcod_dbnam
        {
            get { return _grpcod_dbnam; }
            set { _grpcod_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Grpcod
        {
            get { return _grpcod; }
            set { _grpcod = value; }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string _userid;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _userid_dbnam = "AMWDUSERID";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Userid_dbnam
        {
            get { return _userid_dbnam; }
            set { _userid_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Userid
        {
            get { return _userid; }
            set { _userid = value; }
        }

        /// <summary>
        /// 操作名称
        /// </summary>
        private string _opid;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _opid_dbnam = "AMWDOPID";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Opid_dbnam
        {
            get { return _opid_dbnam; }
            set { _opid_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Opid
        {
            get { return _opid; }
            set { _opid = value; }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        private string _ipaddr;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _ipaddr_dbnam = "AMWDIPADDR";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Ipaddr_dbnam
        {
            get { return _ipaddr_dbnam; }
            set { _ipaddr_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Ipaddr
        {
            get { return _ipaddr; }
            set { _ipaddr = value; }
        }


        /// <summary>
        /// 记录时间
        /// </summary>
        private DateTime _recordtime;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _recordtime_dbnam = "AMWDRECORDTIME";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Recordtime_dbnam
        {
            get { return _recordtime_dbnam; }
            set { _recordtime_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public DateTime Recordtime
        {
            get { return _recordtime; }
            set { _recordtime = value; }
        }


        /// <summary>
        /// 操作前内容
        /// </summary>
        private string _oldtext;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _oldtext_dbnam = "AMWDOLDtext";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Oldtext_dbnam
        {
            get { return _oldtext_dbnam; }
            set { _oldtext_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Oldtext
        {
            get { return _oldtext; }
            set { _oldtext = value; }
        }


        /// <summary>
        /// 操作内容
        /// </summary>
        private string _newtext;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _newtext_dbnam = "AMWDNEWtext";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Newtext_dbnam
        {
            get { return _newtext_dbnam; }
            set { _newtext_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Newtext
        {
            get { return _newtext; }
            set { _newtext = value; }
        }


        /// <summary>
        /// 备注
        /// </summary>
        private string _appdix;
        /// <summary>
        /// 见表AMWD
        /// </summary>
        private string _appdix_dbnam = "AMWDAPPENDIX";
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Appdix_dbnam
        {
            get { return _appdix_dbnam; }
            set { _appdix_dbnam = value; }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string Appdix
        {
            get { return _appdix; }
            set { _appdix = value; }
        }


        #endregion

        /// <summary>
        ///  清空
        /// </summary>
        public void Clear()
        {
            _jrndate = string.Empty;
            _jrnno = 0;
            _procod = string.Empty;
            _dptcod = string.Empty;
            _grpcod = string.Empty;
            _userid = string.Empty;
            _opid = string.Empty;
            _ipaddr = string.Empty;
            _recordtime = DateTime.MinValue;
            _oldtext = string.Empty;
            _newtext = string.Empty;
            _appdix = string.Empty;

        }
    }
}
