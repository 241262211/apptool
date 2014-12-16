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
        /// MACD_NEAREMA
        /// <summary>
        private double? _nearema;

        [DataFieldAttribute("dfl_nearema", "double", 0, false)]
        public double? Nearema
        {
            get { return _nearema; }
            set { _nearema = value; }
        }

        /// <summary>
        /// MACD_FAREMA
        /// <summary>
        private double? _farema;

        [DataFieldAttribute("dfl_farema", "double", 0, false)]
        public double? Farema
        {
            get { return _farema; }
            set { _farema = value; }
        }

        /// <summary>
        /// MACD_DEA
        /// <summary>
        private double? _macddea;

        [DataFieldAttribute("dfl_macddea", "double", 0, false)]
        public double? Macddea
        {
            get { return _macddea; }
            set { _macddea = value; }
        }

        /// <summary>
        /// MACD_BAR
        /// <summary>
        private double? _macdbar;

        [DataFieldAttribute("dfl_macdbar", "double", 0, false)]
        public double? Macdbar
        {
            get { return _macdbar; }
            set { _macdbar = value; }
        }

        /// <summary>
        /// KJD_KLINE
        /// <summary>
        private double? _kdjkline;

        [DataFieldAttribute("dfl_kdjkline", "double", 0, false)]
        public double? Kdjkline
        {
            get { return _kdjkline; }
            set { _kdjkline = value; }
        }

        /// <summary>
        /// KJD_JLINE
        /// <summary>
        private double? _kdjjline;

        [DataFieldAttribute("dfl_kdjjline", "double", 0, false)]
        public double? Kdjjline
        {
            get { return _kdjjline; }
            set { _kdjjline = value; }
        }

        /// <summary>
        /// KJD_DLINE
        /// <summary>
        private double? _kdjdline;

        [DataFieldAttribute("dfl_kdjdline", "double", 0, false)]
        public double? Kdjdline
        {
            get { return _kdjdline; }
            set { _kdjdline = value; }
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
