using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    //
    //3  指标分析表
    //INDEXANALYS
    [DataObjectAttribute("INDEXANALYS")]
    public class IndexanalysBean
    {
        /// <summary>
        /// 编号
        /// <summary>
        private string _id;

        [DataFieldAttribute("ina_id", "char", 30, true)]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// 股票代码
        /// <summary>
        private string _code;

        [DataFieldAttribute("ina_code", "char", 10, false)]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 日期
        /// <summary>
        private string _date;

        [DataFieldAttribute("ina_date", "char", 10, false)]
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// 时间
        /// <summary>
        private string _time;

        [DataFieldAttribute("ina_time", "char", 10, false)]
        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// 股票名称
        /// <summary>
        private string _name;

        [DataFieldAttribute("ina_name", "char", 40, false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 指标类型
        /// <summary>
        private string _type;

        [DataFieldAttribute("ina_type", "char", 10, false)]
        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        /// <summary>
        /// 指标子类型
        /// <summary>
        private string _subtype;

        [DataFieldAttribute("ina_subtype", "char", 10, false)]
        public string Subtype
        {
            get { return _subtype; }
            set { _subtype = value; }
        }

        /// <summary>
        /// 标志
        /// <summary>
        private string _flag;

        [DataFieldAttribute("ina_flag", "char", 10, false)]
        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

        /// <summary>
        /// 当日收盘价
        /// <summary>
        private double? _markprice;

        [DataFieldAttribute("ina_markprice", "double", 0, false)]
        public double? Markprice
        {
            get { return _markprice; }
            set { _markprice = value; }
        }
        /// <summary>
        /// 当日涨幅
        /// <summary>
        private double? _todayincrease;

        [DataFieldAttribute("ina_todayincrease", "double", 0, false)]
        public double? Todayincrease
        {
            get { return _todayincrease; }
            set { _todayincrease = value; }
        }

        /// <summary>
        /// 第二天涨幅
        /// <summary>
        private double? _nextincrease;

        [DataFieldAttribute("ina_nextincrease", "double", 0, false)]
        public double? Nextincrease
        {
            get { return _nextincrease; }
            set { _nextincrease = value; }
        }

        /// <summary>
        /// 5日涨幅
        /// <summary>
        private double? _day5increase;

        [DataFieldAttribute("ina_day5increase", "double", 0, false)]
        public double? Day5increase
        {
            get { return _day5increase; }
            set { _day5increase = value; }
        }

        /// <summary>
        /// 10日涨幅
        /// <summary>
        private double? _day10increase;

        [DataFieldAttribute("ina_day10increase", "double", 0, false)]
        public double? Day10increase
        {
            get { return _day10increase; }
            set { _day10increase = value; }
        }

    }
}
