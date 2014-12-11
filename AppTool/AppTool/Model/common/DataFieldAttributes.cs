using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class DataFieldAttribute : Attribute
    {
        /// <summary>
        /// 对应数据库字段名
        /// </summary>
        private string _fieldname;

        public string Fieldname
        {
            get { return _fieldname; }
            set { _fieldname = value; }
        }

        /// <summary>
        /// 对应数据库类型
        /// </summary>
        private string _fieldtype;

        public string Fieldtype
        {
            get { return _fieldtype; }
            set { _fieldtype = value; }
        }

        /// <summary>
        /// 对应数据库长度
        /// </summary>
        private int _fieldlength;

        public int Fieldlength
        {
            get { return _fieldlength; }
            set { _fieldlength = value; }
        }

        /// <summary>
        /// 是否是必须字段
        /// </summary>
        private Boolean _isrequired;

        public Boolean Isrequired
        {
            get { return _isrequired; }
            set { _isrequired = value; }
        }

        public DataFieldAttribute(string fieldname, string fieldtype)
            : this(fieldname, fieldtype, 0) { }

        public DataFieldAttribute(string fieldname, string fieldtype, int fieldlength)
            : this(fieldname, fieldtype, fieldlength, false) { }

        public DataFieldAttribute(string fieldname, string fieldtype, int fieldlength, Boolean isrequired)
        {
            this._fieldname = fieldname;
            this._fieldtype = fieldtype;
            this._fieldlength = fieldlength;
            this._isrequired = isrequired;
        }
    }

    public class DataObjectAttribute : Attribute
    {
        /// <summary>
        /// 数据库表名
        /// </summary>
        private string _tablename;

        public string Tablename
        {
            get { return _tablename; }
            set { _tablename = value; }
        }

        public DataObjectAttribute(string tablename)
        {
            this._tablename = tablename;
        }
    }
}
