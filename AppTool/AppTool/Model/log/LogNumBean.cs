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
    /// 日志号表bean
    /// </summary>
    public class LogNumBean
    {
        /// <summary>
        /// 日志号表
        /// </summary>
        public LogNumBean()
        {
        }
        #region LogNumModel

        /// <summary>
        /// 日志时间
        /// </summary>
        private string _jrndate;
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
        /// 字段封装
        /// </summary>
        public int Jrnno
        {
            get { return _jrnno; }
            set { _jrnno = value; }
        }

        #endregion
    }
}
