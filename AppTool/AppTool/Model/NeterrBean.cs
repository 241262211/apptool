using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    //
    //9  网络错误重提表
    //NETERR
    [DataObjectAttribute("NETERR")]
    public class NeterrBean
    {
        /// <summary>
        /// 编号
        /// <summary>
        private string _id;

        [DataFieldAttribute("nte_id", "char", 30, true)]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 股票代码
        /// <summary>
        private string _code;

        [DataFieldAttribute("nte_code", "char", 10, false)]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 股票名称
        /// <summary>
        private string _name;

        [DataFieldAttribute("nte_name", "char", 40, false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 日期
        /// <summary>
        private string _date;

        [DataFieldAttribute("nte_date", "char", 10, false)]
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// 错误类型
        /// <summary>
        private string _type;

        [DataFieldAttribute("nte_type", "char", 40, false)]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 备注
        /// <summary>
        private string _remark;

        [DataFieldAttribute("nte_remark", "char", 200, false)]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

    }
}
