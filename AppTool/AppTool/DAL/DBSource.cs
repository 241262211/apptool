//----------------------------------------------------------------------------
// Copyright (C) 2013, 王强. All rights reserved.
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    /// <summary>
    /// 获得db源
    /// </summary>
    [Serializable]
    public class DBSource
    {
        /// <summary>
        /// 数据库18位ID编号
        /// </summary>
        private string propDBID = "";           //
        /// <summary>
        /// 数据库连接描述名称 
        /// </summary>
        private string propConnName = "";       //
        /// <summary>
        /// 数据库类型
        /// </summary>
        private string propDBType = "";         //
        /// <summary>
        /// 数据库名称
        /// </summary>
        private string propDBName = "";         //
        /// <summary>
        /// 数据库IP地址
        /// </summary>
        private string propIPAddress = "";      //
        /// <summary>
        /// 数据库端口
        /// </summary>
        private int propPort = 0;            //
        /// <summary>
        /// 默认用户名
        /// </summary>
        private string propDefaultUser = "";    //
        /// <summary>
        /// 默认密码
        /// </summary>
        private string propDefaultPWD = "";     //
        /// <summary>
        /// 最大连接数
        /// </summary>
        private int propMaxConnNo = 0;       //
        /// <summary>
        /// 已有连接数
        /// </summary>
        private int propCurrentConnNo = 0;   //
        /// <summary>
        /// 数据库描述
        /// </summary>
        private string propDBDescription = "";  //
        /// <summary>
        /// 锁定标志
        /// </summary>
        public object LockFlag = new object();
        /// <summary>
        /// 字段封装
        /// </summary>
        public string DBID
        {
            get
            {
                return this.propDBID;
            }
            set
            {
                this.propDBID = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string ConnName
        {
            get
            {
                return this.propConnName;
            }
            set
            {
                this.propConnName = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string DBType
        {
            get
            {
                return this.propDBType;
            }
            set
            {
                this.propDBType = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string DBName
        {
            get
            {
                return this.propDBName;
            }
            set
            {
                this.propDBName = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string IPAddress
        {
            get
            {
                return this.propIPAddress;
            }
            set
            {
                this.propIPAddress = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public int Port
        {
            get
            {
                return this.propPort;
            }
            set
            {
                this.propPort = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string DefaultUser
        {
            get
            {
                return this.propDefaultUser;
            }
            set
            {
                this.propDefaultUser = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string DefaultPWD
        {
            get
            {
                return this.propDefaultPWD;
            }
            set
            {
                this.propDefaultPWD = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public int MaxConnNo
        {
            get
            {
                return this.propMaxConnNo;
            }
            set
            {
                this.propMaxConnNo = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public int CurrentConnNo
        {
            get
            {
                return this.propCurrentConnNo;
            }
            set
            {
                this.propCurrentConnNo = value;
            }
        }
        /// <summary>
        /// 字段封装
        /// </summary>
        public string DBDescription
        {
            get
            {
                return this.propDBDescription;
            }
            set
            {
                this.propDBDescription = value;
            }
        }

    }
}
