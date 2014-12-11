using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    //
    //5  股票主档表
    //STOCKBASE
    [DataObjectAttribute("STOCKBASE")]
    public class StockbaseBean
    {
        /// <summary>
        /// 股票代码
        /// <summary>
        private string _code;

        [DataFieldAttribute("stb_code", "char", 16, true)]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 股票名字
        /// <summary>
        private string _name;

        [DataFieldAttribute("stb_name", "char", 40, false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 所属版块
        /// <summary>
        private string _section;

        [DataFieldAttribute("stb_section", "char", 20, false)]
        public string Section
        {
            get { return _section; }
            set { _section = value; }
        }

        /// <summary>
        /// 概念
        /// <summary>
        private string _concept;

        [DataFieldAttribute("stb_concept", "char", 200, false)]
        public string Concept
        {
            get { return _concept; }
            set { _concept = value; }
        }

        /// <summary>
        /// 地域
        /// <summary>
        private string _local;

        [DataFieldAttribute("stb_local", "char", 50, false)]
        public string Local
        {
            get { return _local; }
            set { _local = value; }
        }

        /// <summary>
        /// 最新评级
        /// <summary>
        private string _grade;

        [DataFieldAttribute("stb_grade", "char", 20, false)]
        public string Grade
        {
            get { return _grade; }
            set { _grade = value; }
        }

        /// <summary>
        /// 当前价格
        /// <summary>
        private double? _price;

        [DataFieldAttribute("stb_price", "double", 0, false)]
        public double? Price
        {
            get { return _price; }
            set { _price = value; }
        }

        /// <summary>
        /// 今日开盘价
        /// <summary>
        private double? _startprice;

        [DataFieldAttribute("stb_startprice", "double", 0, false)]
        public double? Startprice
        {
            get { return _startprice; }
            set { _startprice = value; }
        }

        /// <summary>
        /// 昨日收盘价
        /// <summary>
        private double? _lastprice;

        [DataFieldAttribute("stb_lastprice", "double", 0, false)]
        public double? Lastprice
        {
            get { return _lastprice; }
            set { _lastprice = value; }
        }

        /// <summary>
        /// 今日最高价
        /// <summary>
        private double? _hightprice;

        [DataFieldAttribute("stb_hightprice", "double", 0, false)]
        public double? Hightprice
        {
            get { return _hightprice; }
            set { _hightprice = value; }
        }

        /// <summary>
        /// 今日最低价
        /// <summary>
        private double? _lowprice;

        [DataFieldAttribute("stb_lowprice", "double", 0, false)]
        public double? Lowprice
        {
            get { return _lowprice; }
            set { _lowprice = value; }
        }

        /// <summary>
        /// 市盈率
        /// <summary>
        private double? _pe;

        [DataFieldAttribute("stb_pe", "double", 0, false)]
        public double? Pe
        {
            get { return _pe; }
            set { _pe = value; }
        }

        /// <summary>
        /// 市净率
        /// <summary>
        private double? _pb;

        [DataFieldAttribute("stb_pb", "double", 0, false)]
        public double? Pb
        {
            get { return _pb; }
            set { _pb = value; }
        }

        /// <summary>
        /// 更新时间
        /// <summary>
        private string _updatetime;

        [DataFieldAttribute("stb_updatetime", "char", 60, false)]
        public string Updatetime
        {
            get { return _updatetime; }
            set { _updatetime = value; }
        }

        /// <summary>
        /// 备用字段1
        /// <summary>
        private string _bak1;

        [DataFieldAttribute("stb_bak1", "char", 20, false)]
        public string Bak1
        {
            get { return _bak1; }
            set { _bak1 = value; }
        }

        /// <summary>
        /// 备用字段2
        /// <summary>
        private string _bak2;

        [DataFieldAttribute("stb_bak2", "char", 60, false)]
        public string Bak2
        {
            get { return _bak2; }
            set { _bak2 = value; }
        }

        /// <summary>
        /// 备用字段3
        /// <summary>
        private string _bak3;

        [DataFieldAttribute("stb_bak3", "char", 200, false)]
        public string Bak3
        {
            get { return _bak3; }
            set { _bak3 = value; }
        }

    }
}
