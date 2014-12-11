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
    /// 数据库操作
    /// </summary>
    public class DbBean
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DbBean()
        {
        }

        /// <summary>
        /// 系统英文名
        /// </summary>
        private string _sysEnName = "";

        /// <summary>
        /// 获取英文名
        /// </summary>
        public string SysEnName
        {
            get { return _sysEnName; }
            set { _sysEnName = value; }
        }

        /// <summary>
        /// 系统中文名
        /// </summary>
        private string _sysCnName = "";

        /// <summary>
        /// 获取系统中文名
        /// </summary>
        public string SysCnName
        {
            get { return _sysCnName; }
            set { _sysCnName = value; }
        }

        /// <summary>
        /// 表英文名
        /// </summary>
        private string _tableEnName = "";

        /// <summary>
        /// 获取表英文名
        /// </summary>
        public string TableEnName
        {
            get { return _tableEnName; }
            set { _tableEnName = value; }
        }

        /// <summary>
        /// 表中文名
        /// </summary>
        private string _tableCnName = "";

        /// <summary>
        /// 表中文名
        /// </summary>
        public string TableCnName
        {
            get { return _tableCnName; }
            set { _tableCnName = value; }
        }

        /// <summary>
        /// 字段英文名
        /// </summary>
        private string _fieldEnName = "";

        /// <summary>
        /// 字段英文名
        /// </summary>
        public string FieldEnName
        {
            get { return _fieldEnName; }
            set { _fieldEnName = value; }
        }

        /// <summary>
        /// 字段中文名
        /// </summary>
        private string _fieldCnName = "";

        /// <summary>
        /// 字段中文名
        /// </summary>
        public string FieldCnName
        {
            get { return _fieldCnName; }
            set { _fieldCnName = value; }
        }

        /// <summary>
        /// 字段序号
        /// </summary>
        private string _fieldNum = "";

        /// <summary>
        /// 字段序号
        /// </summary>
        public string FieldNum
        {
            get { return _fieldNum; }
            set { _fieldNum = value; }
        }
        /// <summary>
        /// 是否主键
        /// </summary>
        private string _isPk = "";

        /// <summary>
        /// 是否主键
        /// </summary>
        public string IsPk
        {
            get { return _isPk; }
            set { _isPk = value; }
        }

        /// <summary>
        /// 字段类型
        /// </summary>
        private string _fieldType = "";

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType
        {
            get { return _fieldType; }
            set { _fieldType = value; }
        }

        /// <summary>
        /// 字段格式
        /// </summary>
        private string _fieldStyle = "";

        /// <summary>
        /// 字段格式
        /// </summary>
        public string FieldStyle
        {
            get { return _fieldStyle; }
            set { _fieldStyle = value; }
        }

        /// <summary>
        /// 字段长度
        /// </summary>
        private string _fieldLen = "";

        /// <summary>
        /// 字段长度
        /// </summary>
        public string FieldLen
        {
            get { return _fieldLen; }
            set { _fieldLen = value; }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        private string _isNull = "";

        /// <summary>
        /// 是否为空
        /// </summary>
        public string IsNull
        {
            get { return _isNull; }
            set { _isNull = value; }
        }

        /// <summary>
        /// 小数点长度
        /// </summary>
        private string _decimalLen = "";

        /// <summary>
        /// 小数点长度
        /// </summary>
        public string DecimalLen
        {
            get { return _decimalLen; }
            set { _decimalLen = value; }
        }



        /// <summary>
        /// 取值范围
        /// </summary>
        private string _valRange = "";

        /// <summary>
        /// 取值范围
        /// </summary>
        public string ValRange
        {
            get { return _valRange; }
            set { _valRange = value; }
        }


        /// <summary>
        /// 备份
        /// </summary>
        private string _bak = "";

        /// <summary>
        /// 备份
        /// </summary>
        public string Bak
        {
            get { return _bak; }
            set { _bak = value; }
        }












    }
}
