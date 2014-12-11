using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    //
    //6  日线表
    //DAYFLOW
    [DataObjectAttribute("DAYFLOW")]
    public class DayflowBean
    {
        /// <summary>
        /// 股票代码
        /// <summary>
        private string _code;

        [DataFieldAttribute("dfl_code", "char", 10, true)]
        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        /// <summary>
        /// 日期
        /// <summary>
        private string _date;

        [DataFieldAttribute("dfl_date", "char", 10, true)]
        public string Date
        {
            get { return _date; }
            set { _date = value; }
        }

        /// <summary>
        /// 股票名字
        /// <summary>
        private string _name;

        [DataFieldAttribute("dfl_name", "char", 40, false)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// 当前价格
        /// <summary>
        private double? _price;

        [DataFieldAttribute("dfl_price", "double", 0, false)]
        public double? Price
        {
            get { return _price; }
            set { _price = value; }
        }

        /// <summary>
        /// 今日开盘价
        /// <summary>
        private double? _startprice;

        [DataFieldAttribute("dfl_startprice", "double", 0, false)]
        public double? Startprice
        {
            get { return _startprice; }
            set { _startprice = value; }
        }

        /// <summary>
        /// 昨日收盘价
        /// <summary>
        private double? _lastprice;

        [DataFieldAttribute("dfl_lastprice", "double", 0, false)]
        public double? Lastprice
        {
            get { return _lastprice; }
            set { _lastprice = value; }
        }

        /// <summary>
        /// 今日最高价
        /// <summary>
        private double? _hightprice;

        [DataFieldAttribute("dfl_hightprice", "double", 0, false)]
        public double? Hightprice
        {
            get { return _hightprice; }
            set { _hightprice = value; }
        }

        /// <summary>
        /// 今日最低价
        /// <summary>
        private double? _lowprice;

        [DataFieldAttribute("dfl_lowprice", "double", 0, false)]
        public double? Lowprice
        {
            get { return _lowprice; }
            set { _lowprice = value; }
        }

        /// <summary>
        /// 今日收盘价
        /// <summary>
        private double? _endprice;

        [DataFieldAttribute("dfl_endprice", "double", 0, false)]
        public double? Endprice
        {
            get { return _endprice; }
            set { _endprice = value; }
        }

        /// <summary>
        /// 成交股数
        /// <summary>
        private double? _tradenum;

        [DataFieldAttribute("dfl_tradenum", "double", 0, false)]
        public double? Tradenum
        {
            get { return _tradenum; }
            set { _tradenum = value; }
        }

        /// <summary>
        /// 成交金额
        /// <summary>
        private double? _summoney;

        [DataFieldAttribute("dfl_summoney", "double", 0, false)]
        public double? Summoney
        {
            get { return _summoney; }
            set { _summoney = value; }
        }

        /// <summary>
        /// 买量
        /// <summary>
        private double? _buysum;

        [DataFieldAttribute("dfl_buysum", "double", 0, false)]
        public double? Buysum
        {
            get { return _buysum; }
            set { _buysum = value; }
        }

        /// <summary>
        /// 卖量
        /// <summary>
        private double? _salesum;

        [DataFieldAttribute("dfl_salesum", "double", 0, false)]
        public double? Salesum
        {
            get { return _salesum; }
            set { _salesum = value; }
        }

        /// <summary>
        /// 大单
        /// <summary>
        private double? _lsingle;

        [DataFieldAttribute("dfl_lsingle", "double", 0, false)]
        public double? Lsingle
        {
            get { return _lsingle; }
            set { _lsingle = value; }
        }

        /// <summary>
        /// 中单
        /// <summary>
        private double? _msingle;

        [DataFieldAttribute("dfl_msingle", "double", 0, false)]
        public double? Msingle
        {
            get { return _msingle; }
            set { _msingle = value; }
        }

        /// <summary>
        /// 小单
        /// <summary>
        private double? _ssingle;

        [DataFieldAttribute("dfl_ssingle", "double", 0, false)]
        public double? Ssingle
        {
            get { return _ssingle; }
            set { _ssingle = value; }
        }

        /// <summary>
        /// 流入资金
        /// <summary>
        private double? _insum;

        [DataFieldAttribute("dfl_insum", "double", 0, false)]
        public double? Insum
        {
            get { return _insum; }
            set { _insum = value; }
        }

        /// <summary>
        /// 流出资金
        /// <summary>
        private double? _outsum;

        [DataFieldAttribute("dfl_outsum", "double", 0, false)]
        public double? Outsum
        {
            get { return _outsum; }
            set { _outsum = value; }
        }

        /// <summary>
        /// MACD_DIFF
        /// <summary>
        private double? _macdema9;

        [DataFieldAttribute("dfl_macdema9", "double", 0, false)]
        public double? Macdema9
        {
            get { return _macdema9; }
            set { _macdema9 = value; }
        }

        /// <summary>
        /// MACD_DEA
        /// <summary>
        private double? _macdema26;

        [DataFieldAttribute("dfl_macdema26", "double", 0, false)]
        public double? Macdema26
        {
            get { return _macdema26; }
            set { _macdema26 = value; }
        }

        /// <summary>
        /// MACD
        /// <summary>
        private double? _macddea;

        [DataFieldAttribute("dfl_macddea", "double", 0, false)]
        public double? Macddea
        {
            get { return _macddea; }
            set { _macddea = value; }
        }

        /// <summary>
        /// KJD_KLINE
        /// <summary>
        private double? _kjdkline;

        [DataFieldAttribute("dfl_kjdkline", "double", 0, false)]
        public double? Kjdkline
        {
            get { return _kjdkline; }
            set { _kjdkline = value; }
        }

        /// <summary>
        /// KJD_JLINE
        /// <summary>
        private double? _djdjline;

        [DataFieldAttribute("dfl_djdjline", "double", 0, false)]
        public double? Djdjline
        {
            get { return _djdjline; }
            set { _djdjline = value; }
        }

        /// <summary>
        /// KJD_DLINE
        /// <summary>
        private double? _kjddline;

        [DataFieldAttribute("dfl_kjddline", "double", 0, false)]
        public double? Kjddline
        {
            get { return _kjddline; }
            set { _kjddline = value; }
        }

        /// <summary>
        /// BOLL_SUPER
        /// <summary>
        private double? _bollsuper;

        [DataFieldAttribute("dfl_bollsuper", "double", 0, false)]
        public double? Bollsuper
        {
            get { return _bollsuper; }
            set { _bollsuper = value; }
        }

        /// <summary>
        /// BOLL_MIDDLE
        /// <summary>
        private double? _bollmiddle;

        [DataFieldAttribute("dfl_bollmiddle", "double", 0, false)]
        public double? Bollmiddle
        {
            get { return _bollmiddle; }
            set { _bollmiddle = value; }
        }

        /// <summary>
        /// BOLL_LOWER
        /// <summary>
        private double? _bolllower;

        [DataFieldAttribute("dfl_bolllower", "double", 0, false)]
        public double? Bolllower
        {
            get { return _bolllower; }
            set { _bolllower = value; }
        }

        /// <summary>
        /// 更新时间
        /// <summary>
        private string _updatetime;

        [DataFieldAttribute("dfl_updatetime", "char", 60, false)]
        public string Updatetime
        {
            get { return _updatetime; }
            set { _updatetime = value; }
        }

        /// <summary>
        /// 备用字段1
        /// <summary>
        private string _bak1;

        [DataFieldAttribute("dfl_bak1", "char", 20, false)]
        public string Bak1
        {
            get { return _bak1; }
            set { _bak1 = value; }
        }

        /// <summary>
        /// 备用字段2
        /// <summary>
        private string _bak2;

        [DataFieldAttribute("dfl_bak2", "char", 60, false)]
        public string Bak2
        {
            get { return _bak2; }
            set { _bak2 = value; }
        }

        /// <summary>
        /// 备用字段3
        /// <summary>
        private string _bak3;

        [DataFieldAttribute("dfl_bak3", "char", 200, false)]
        public string Bak3
        {
            get { return _bak3; }
            set { _bak3 = value; }
        }

    }
}
