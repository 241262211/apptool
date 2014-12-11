using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    //
    //7  分线表
    //MIN5FLOW
    [DataObjectAttribute("MIN5FLOW")]
    public class Min5flowBean
    {
        /// <summary>
        /// 股票代码
        /// <summary>
        private string _code;

        [DataFieldAttribute("m5f_code", "char", 10, true)]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 日期
        /// <summary>
        private string _date;

        [DataFieldAttribute("m5f_date", "char", 10, true)]
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// 时间
        /// <summary>
        private string _time;

        [DataFieldAttribute("m5f_time", "char", 10, true)]
        public string Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// 股票名字
        /// <summary>
        private string _name;

        [DataFieldAttribute("m5f_name", "char", 40, false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 当前价格
        /// <summary>
        private double? _price;

        [DataFieldAttribute("m5f_price", "double", 0, false)]
        public double? Price
        {
            get { return _price; }
            set { _price = value; }
        }

        /// <summary>
        /// 开盘价
        /// <summary>
        private double? _startprice;

        [DataFieldAttribute("m5f_startprice", "double", 0, false)]
        public double? Startprice
        {
            get { return _startprice; }
            set { _startprice = value; }
        }

        /// <summary>
        /// 最高价
        /// <summary>
        private double? _hightprice;

        [DataFieldAttribute("m5f_hightprice", "double", 0, false)]
        public double? Hightprice
        {
            get { return _hightprice; }
            set { _hightprice = value; }
        }

        /// <summary>
        /// 最低价
        /// <summary>
        private double? _lowprice;

        [DataFieldAttribute("m5f_lowprice", "double", 0, false)]
        public double? Lowprice
        {
            get { return _lowprice; }
            set { _lowprice = value; }
        }

        /// <summary>
        /// 收盘价
        /// <summary>
        private double? _endprice;

        [DataFieldAttribute("m5f_endprice", "double", 0, false)]
        public double? Endprice
        {
            get { return _endprice; }
            set { _endprice = value; }
        }

        /// <summary>
        /// 成交股数
        /// <summary>
        private double? _tradenum;

        [DataFieldAttribute("m5f_tradenum", "double", 0, false)]
        public double? Tradenum
        {
            get { return _tradenum; }
            set { _tradenum = value; }
        }

        /// <summary>
        /// 成交金额
        /// <summary>
        private double? _summoney;

        [DataFieldAttribute("m5f_summoney", "double", 0, false)]
        public double? Summoney
        {
            get { return _summoney; }
            set { _summoney = value; }
        }

        /// <summary>
        /// MACD_DIFF
        /// <summary>
        private double? _macddiff;

        [DataFieldAttribute("m5f_macddiff", "double", 0, false)]
        public double? Macddiff
        {
            get { return _macddiff; }
            set { _macddiff = value; }
        }

        /// <summary>
        /// MACD_DEA
        /// <summary>
        private double? _macddea;

        [DataFieldAttribute("m5f_macddea", "double", 0, false)]
        public double? Macddea
        {
            get { return _macddea; }
            set { _macddea = value; }
        }

        /// <summary>
        /// MACD
        /// <summary>
        private double? _macd;

        [DataFieldAttribute("m5f_macd", "double", 0, false)]
        public double? Macd
        {
            get { return _macd; }
            set { _macd = value; }
        }

        /// <summary>
        /// KJD_KLINE
        /// <summary>
        private double? _kjdkline;

        [DataFieldAttribute("m5f_kjdkline", "double", 0, false)]
        public double? Kjdkline
        {
            get { return _kjdkline; }
            set { _kjdkline = value; }
        }

        /// <summary>
        /// KJD_JLINE
        /// <summary>
        private double? _djdjline;

        [DataFieldAttribute("m5f_djdjline", "double", 0, false)]
        public double? Djdjline
        {
            get { return _djdjline; }
            set { _djdjline = value; }
        }

        /// <summary>
        /// KJD_DLINE
        /// <summary>
        private double? _kjddline;

        [DataFieldAttribute("m5f_kjddline", "double", 0, false)]
        public double? Kjddline
        {
            get { return _kjddline; }
            set { _kjddline = value; }
        }

        /// <summary>
        /// BOLL_SUPER
        /// <summary>
        private double? _bollsuper;

        [DataFieldAttribute("m5f_bollsuper", "double", 0, false)]
        public double? Bollsuper
        {
            get { return _bollsuper; }
            set { _bollsuper = value; }
        }

        /// <summary>
        /// BOLL_MIDDLE
        /// <summary>
        private double? _bollmiddle;

        [DataFieldAttribute("m5f_bollmiddle", "double", 0, false)]
        public double? Bollmiddle
        {
            get { return _bollmiddle; }
            set { _bollmiddle = value; }
        }

        /// <summary>
        /// BOLL_LOWER
        /// <summary>
        private double? _bolllower;

        [DataFieldAttribute("m5f_bolllower", "double", 0, false)]
        public double? Bolllower
        {
            get { return _bolllower; }
            set { _bolllower = value; }
        }

        /// <summary>
        /// 备用字段1
        /// <summary>
        private string _bak1;

        [DataFieldAttribute("m5f_bak1", "char", 20, false)]
        public string Bak1
        {
            get { return _bak1; }
            set { _bak1 = value; }
        }

        /// <summary>
        /// 备用字段2
        /// <summary>
        private string _bak2;

        [DataFieldAttribute("m5f_bak2", "char", 60, false)]
        public string Bak2
        {
            get { return _bak2; }
            set { _bak2 = value; }
        }

        /// <summary>
        /// 备用字段3
        /// <summary>
        private string _bak3;

        [DataFieldAttribute("m5f_bak3", "char", 200, false)]
        public string Bak3
        {
            get { return _bak3; }
            set { _bak3 = value; }
        }

    }
}
