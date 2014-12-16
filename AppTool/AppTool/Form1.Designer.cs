namespace AppTool
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.memcached = new System.Windows.Forms.TabPage();
            this.show_cache_btn = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.crt_cache_btn = new System.Windows.Forms.Button();
            this.ptf_tb = new System.Windows.Forms.TabPage();
            this.exceltopdf_btn = new System.Windows.Forms.Button();
            this.dbAcv_tab = new System.Windows.Forms.TabPage();
            this.cmprogress = new System.Windows.Forms.ProgressBar();
            this.cmsniffer = new System.Windows.Forms.Button();
            this.app_file_dir_textbox = new System.Windows.Forms.TextBox();
            this.json_sel_file = new System.Windows.Forms.Button();
            this.crt_znzz_script_btn = new System.Windows.Forms.Button();
            this.CrtBean_btn = new System.Windows.Forms.Button();
            this.crt_table_btn = new System.Windows.Forms.Button();
            this.sqlOpTableTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.InsertBtn = new System.Windows.Forms.Button();
            this.UpdBtn = new System.Windows.Forms.Button();
            this.numStyleTB = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.whereTB = new System.Windows.Forms.TextBox();
            this.dbtool = new System.Windows.Forms.TabControl();
            this.stock_tab = new System.Windows.Forms.TabPage();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.DayKDJAnalysis = new System.Windows.Forms.Button();
            this.DayKDJCalc = new System.Windows.Forms.Button();
            this.DayMACDAnalysis = new System.Windows.Forms.Button();
            this.DayMACDCalc = new System.Windows.Forms.Button();
            this.test = new System.Windows.Forms.Button();
            this.Min5KLine = new System.Windows.Forms.Button();
            this.historydatabar = new System.Windows.Forms.ProgressBar();
            this.HistoryKLine = new System.Windows.Forms.Button();
            this.DayKLine = new System.Windows.Forms.Button();
            this.AllStockCode = new System.Windows.Forms.Button();
            this.WeekHisLine = new System.Windows.Forms.Button();
            this.memcached.SuspendLayout();
            this.ptf_tb.SuspendLayout();
            this.dbAcv_tab.SuspendLayout();
            this.dbtool.SuspendLayout();
            this.stock_tab.SuspendLayout();
            this.SuspendLayout();
            // 
            // memcached
            // 
            this.memcached.Controls.Add(this.show_cache_btn);
            this.memcached.Controls.Add(this.textBox1);
            this.memcached.Controls.Add(this.crt_cache_btn);
            this.memcached.Location = new System.Drawing.Point(4, 22);
            this.memcached.Name = "memcached";
            this.memcached.Padding = new System.Windows.Forms.Padding(3);
            this.memcached.Size = new System.Drawing.Size(668, 291);
            this.memcached.TabIndex = 10;
            this.memcached.Text = "memcached";
            this.memcached.UseVisualStyleBackColor = true;
            // 
            // show_cache_btn
            // 
            this.show_cache_btn.Location = new System.Drawing.Point(194, 95);
            this.show_cache_btn.Name = "show_cache_btn";
            this.show_cache_btn.Size = new System.Drawing.Size(75, 23);
            this.show_cache_btn.TabIndex = 2;
            this.show_cache_btn.Text = "显示cache";
            this.show_cache_btn.UseVisualStyleBackColor = true;
            this.show_cache_btn.Click += new System.EventHandler(this.show_cache_btn_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(56, 32);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 1;
            // 
            // crt_cache_btn
            // 
            this.crt_cache_btn.Location = new System.Drawing.Point(194, 30);
            this.crt_cache_btn.Name = "crt_cache_btn";
            this.crt_cache_btn.Size = new System.Drawing.Size(75, 23);
            this.crt_cache_btn.TabIndex = 0;
            this.crt_cache_btn.Text = "创建cache";
            this.crt_cache_btn.UseVisualStyleBackColor = true;
            this.crt_cache_btn.Click += new System.EventHandler(this.crt_cache_btn_Click);
            // 
            // ptf_tb
            // 
            this.ptf_tb.Controls.Add(this.exceltopdf_btn);
            this.ptf_tb.Location = new System.Drawing.Point(4, 22);
            this.ptf_tb.Name = "ptf_tb";
            this.ptf_tb.Padding = new System.Windows.Forms.Padding(3);
            this.ptf_tb.Size = new System.Drawing.Size(668, 291);
            this.ptf_tb.TabIndex = 9;
            this.ptf_tb.Text = "PDF";
            this.ptf_tb.UseVisualStyleBackColor = true;
            // 
            // exceltopdf_btn
            // 
            this.exceltopdf_btn.Location = new System.Drawing.Point(42, 51);
            this.exceltopdf_btn.Name = "exceltopdf_btn";
            this.exceltopdf_btn.Size = new System.Drawing.Size(75, 23);
            this.exceltopdf_btn.TabIndex = 0;
            this.exceltopdf_btn.Text = "xls转pdf";
            this.exceltopdf_btn.UseVisualStyleBackColor = true;
            this.exceltopdf_btn.Click += new System.EventHandler(this.exceltopdf_btn_Click);
            // 
            // dbAcv_tab
            // 
            this.dbAcv_tab.Controls.Add(this.cmprogress);
            this.dbAcv_tab.Controls.Add(this.cmsniffer);
            this.dbAcv_tab.Controls.Add(this.app_file_dir_textbox);
            this.dbAcv_tab.Controls.Add(this.json_sel_file);
            this.dbAcv_tab.Controls.Add(this.crt_znzz_script_btn);
            this.dbAcv_tab.Controls.Add(this.CrtBean_btn);
            this.dbAcv_tab.Controls.Add(this.crt_table_btn);
            this.dbAcv_tab.Location = new System.Drawing.Point(4, 22);
            this.dbAcv_tab.Name = "dbAcv_tab";
            this.dbAcv_tab.Padding = new System.Windows.Forms.Padding(3);
            this.dbAcv_tab.Size = new System.Drawing.Size(668, 291);
            this.dbAcv_tab.TabIndex = 8;
            this.dbAcv_tab.Text = "数据库文档";
            this.dbAcv_tab.UseVisualStyleBackColor = true;
            // 
            // cmprogress
            // 
            this.cmprogress.Location = new System.Drawing.Point(6, 132);
            this.cmprogress.Name = "cmprogress";
            this.cmprogress.Size = new System.Drawing.Size(326, 23);
            this.cmprogress.TabIndex = 21;
            // 
            // cmsniffer
            // 
            this.cmsniffer.Location = new System.Drawing.Point(359, 132);
            this.cmsniffer.Name = "cmsniffer";
            this.cmsniffer.Size = new System.Drawing.Size(75, 23);
            this.cmsniffer.TabIndex = 20;
            this.cmsniffer.Text = "城觅抓包";
            this.cmsniffer.UseVisualStyleBackColor = true;
            this.cmsniffer.Click += new System.EventHandler(this.cmsniffer_Click);
            // 
            // app_file_dir_textbox
            // 
            this.app_file_dir_textbox.Location = new System.Drawing.Point(6, 91);
            this.app_file_dir_textbox.Name = "app_file_dir_textbox";
            this.app_file_dir_textbox.Size = new System.Drawing.Size(326, 21);
            this.app_file_dir_textbox.TabIndex = 19;
            // 
            // json_sel_file
            // 
            this.json_sel_file.Location = new System.Drawing.Point(359, 89);
            this.json_sel_file.Name = "json_sel_file";
            this.json_sel_file.Size = new System.Drawing.Size(101, 23);
            this.json_sel_file.TabIndex = 18;
            this.json_sel_file.Text = "数据包文件解析";
            this.json_sel_file.UseVisualStyleBackColor = true;
            this.json_sel_file.Click += new System.EventHandler(this.json_sel_file_Click);
            // 
            // crt_znzz_script_btn
            // 
            this.crt_znzz_script_btn.Location = new System.Drawing.Point(359, 36);
            this.crt_znzz_script_btn.Name = "crt_znzz_script_btn";
            this.crt_znzz_script_btn.Size = new System.Drawing.Size(120, 23);
            this.crt_znzz_script_btn.TabIndex = 16;
            this.crt_znzz_script_btn.Text = "生成职能组长脚本";
            this.crt_znzz_script_btn.UseVisualStyleBackColor = true;
            this.crt_znzz_script_btn.Click += new System.EventHandler(this.crt_znzz_btn_click);
            // 
            // CrtBean_btn
            // 
            this.CrtBean_btn.Location = new System.Drawing.Point(170, 36);
            this.CrtBean_btn.Name = "CrtBean_btn";
            this.CrtBean_btn.Size = new System.Drawing.Size(120, 23);
            this.CrtBean_btn.TabIndex = 15;
            this.CrtBean_btn.Text = "1-生成bean";
            this.CrtBean_btn.UseVisualStyleBackColor = true;
            this.CrtBean_btn.Click += new System.EventHandler(this.CrtBean_btn_Click);
            // 
            // crt_table_btn
            // 
            this.crt_table_btn.Location = new System.Drawing.Point(6, 36);
            this.crt_table_btn.Name = "crt_table_btn";
            this.crt_table_btn.Size = new System.Drawing.Size(120, 23);
            this.crt_table_btn.TabIndex = 14;
            this.crt_table_btn.Text = "1-生成建表脚本";
            this.crt_table_btn.UseVisualStyleBackColor = true;
            this.crt_table_btn.Click += new System.EventHandler(this.crt_table_btn_Click);
            // 
            // sqlOpTableTB
            // 
            this.sqlOpTableTB.Location = new System.Drawing.Point(131, 33);
            this.sqlOpTableTB.Name = "sqlOpTableTB";
            this.sqlOpTableTB.Size = new System.Drawing.Size(100, 21);
            this.sqlOpTableTB.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "操作的表名：";
            // 
            // InsertBtn
            // 
            this.InsertBtn.Location = new System.Drawing.Point(0, 0);
            this.InsertBtn.Name = "InsertBtn";
            this.InsertBtn.Size = new System.Drawing.Size(75, 23);
            this.InsertBtn.TabIndex = 0;
            // 
            // UpdBtn
            // 
            this.UpdBtn.Location = new System.Drawing.Point(0, 0);
            this.UpdBtn.Name = "UpdBtn";
            this.UpdBtn.Size = new System.Drawing.Size(75, 23);
            this.UpdBtn.TabIndex = 0;
            // 
            // numStyleTB
            // 
            this.numStyleTB.Location = new System.Drawing.Point(0, 0);
            this.numStyleTB.Name = "numStyleTB";
            this.numStyleTB.Size = new System.Drawing.Size(100, 21);
            this.numStyleTB.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 79);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "为数字型的字段名：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(143, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "where条件开始的字段名：";
            // 
            // whereTB
            // 
            this.whereTB.Location = new System.Drawing.Point(176, 106);
            this.whereTB.Name = "whereTB";
            this.whereTB.Size = new System.Drawing.Size(195, 21);
            this.whereTB.TabIndex = 7;
            // 
            // dbtool
            // 
            this.dbtool.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.dbtool.Controls.Add(this.dbAcv_tab);
            this.dbtool.Controls.Add(this.ptf_tb);
            this.dbtool.Controls.Add(this.memcached);
            this.dbtool.Controls.Add(this.stock_tab);
            this.dbtool.Location = new System.Drawing.Point(2, 2);
            this.dbtool.Name = "dbtool";
            this.dbtool.SelectedIndex = 0;
            this.dbtool.Size = new System.Drawing.Size(676, 317);
            this.dbtool.TabIndex = 0;
            // 
            // stock_tab
            // 
            this.stock_tab.Controls.Add(this.WeekHisLine);
            this.stock_tab.Controls.Add(this.button8);
            this.stock_tab.Controls.Add(this.button7);
            this.stock_tab.Controls.Add(this.button6);
            this.stock_tab.Controls.Add(this.button5);
            this.stock_tab.Controls.Add(this.button4);
            this.stock_tab.Controls.Add(this.button3);
            this.stock_tab.Controls.Add(this.DayKDJAnalysis);
            this.stock_tab.Controls.Add(this.DayKDJCalc);
            this.stock_tab.Controls.Add(this.DayMACDAnalysis);
            this.stock_tab.Controls.Add(this.DayMACDCalc);
            this.stock_tab.Controls.Add(this.test);
            this.stock_tab.Controls.Add(this.Min5KLine);
            this.stock_tab.Controls.Add(this.historydatabar);
            this.stock_tab.Controls.Add(this.HistoryKLine);
            this.stock_tab.Controls.Add(this.DayKLine);
            this.stock_tab.Controls.Add(this.AllStockCode);
            this.stock_tab.Location = new System.Drawing.Point(4, 22);
            this.stock_tab.Name = "stock_tab";
            this.stock_tab.Padding = new System.Windows.Forms.Padding(3);
            this.stock_tab.Size = new System.Drawing.Size(668, 291);
            this.stock_tab.TabIndex = 11;
            this.stock_tab.Text = "stock";
            this.stock_tab.UseVisualStyleBackColor = true;
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(537, 194);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(101, 23);
            this.button8.TabIndex = 15;
            this.button8.Text = "button8";
            this.button8.UseVisualStyleBackColor = true;
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(407, 194);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(103, 23);
            this.button7.TabIndex = 14;
            this.button7.Text = "button7";
            this.button7.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(537, 149);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(101, 23);
            this.button6.TabIndex = 13;
            this.button6.Text = "button6";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(407, 149);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(103, 23);
            this.button5.TabIndex = 12;
            this.button5.Text = "button5";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(537, 105);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(407, 106);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(103, 23);
            this.button3.TabIndex = 10;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // DayKDJAnalysis
            // 
            this.DayKDJAnalysis.Location = new System.Drawing.Point(537, 68);
            this.DayKDJAnalysis.Name = "DayKDJAnalysis";
            this.DayKDJAnalysis.Size = new System.Drawing.Size(101, 23);
            this.DayKDJAnalysis.TabIndex = 9;
            this.DayKDJAnalysis.Text = "日KDJ指标分析";
            this.DayKDJAnalysis.UseVisualStyleBackColor = true;
            // 
            // DayKDJCalc
            // 
            this.DayKDJCalc.Location = new System.Drawing.Point(407, 68);
            this.DayKDJCalc.Name = "DayKDJCalc";
            this.DayKDJCalc.Size = new System.Drawing.Size(103, 23);
            this.DayKDJCalc.TabIndex = 8;
            this.DayKDJCalc.Text = "日KDJ指标计算";
            this.DayKDJCalc.UseVisualStyleBackColor = true;
            this.DayKDJCalc.Click += new System.EventHandler(this.DayKDJCalc_Click);
            // 
            // DayMACDAnalysis
            // 
            this.DayMACDAnalysis.Location = new System.Drawing.Point(537, 24);
            this.DayMACDAnalysis.Name = "DayMACDAnalysis";
            this.DayMACDAnalysis.Size = new System.Drawing.Size(101, 23);
            this.DayMACDAnalysis.TabIndex = 7;
            this.DayMACDAnalysis.Text = "日MACD指标分析";
            this.DayMACDAnalysis.UseVisualStyleBackColor = true;
            this.DayMACDAnalysis.Click += new System.EventHandler(this.DayMACDAnalysis_Click);
            // 
            // DayMACDCalc
            // 
            this.DayMACDCalc.Location = new System.Drawing.Point(407, 24);
            this.DayMACDCalc.Name = "DayMACDCalc";
            this.DayMACDCalc.Size = new System.Drawing.Size(103, 23);
            this.DayMACDCalc.TabIndex = 6;
            this.DayMACDCalc.Text = "日MACD指标计算";
            this.DayMACDCalc.UseVisualStyleBackColor = true;
            this.DayMACDCalc.Click += new System.EventHandler(this.DayMACD_Click);
            // 
            // test
            // 
            this.test.Location = new System.Drawing.Point(21, 24);
            this.test.Name = "test";
            this.test.Size = new System.Drawing.Size(75, 23);
            this.test.TabIndex = 5;
            this.test.Text = "测试";
            this.test.UseVisualStyleBackColor = true;
            this.test.Click += new System.EventHandler(this.test_Click);
            // 
            // Min5KLine
            // 
            this.Min5KLine.Location = new System.Drawing.Point(21, 149);
            this.Min5KLine.Name = "Min5KLine";
            this.Min5KLine.Size = new System.Drawing.Size(134, 23);
            this.Min5KLine.TabIndex = 4;
            this.Min5KLine.Text = "Min5KLine";
            this.Min5KLine.UseVisualStyleBackColor = true;
            this.Min5KLine.Click += new System.EventHandler(this.Min5KLine_Click);
            // 
            // historydatabar
            // 
            this.historydatabar.Location = new System.Drawing.Point(161, 68);
            this.historydatabar.Name = "historydatabar";
            this.historydatabar.Size = new System.Drawing.Size(134, 23);
            this.historydatabar.TabIndex = 3;
            // 
            // HistoryKLine
            // 
            this.HistoryKLine.Location = new System.Drawing.Point(21, 107);
            this.HistoryKLine.Name = "HistoryKLine";
            this.HistoryKLine.Size = new System.Drawing.Size(134, 23);
            this.HistoryKLine.TabIndex = 2;
            this.HistoryKLine.Text = "取DAYLINE数据";
            this.HistoryKLine.UseVisualStyleBackColor = true;
            this.HistoryKLine.Click += new System.EventHandler(this.HistoryKLine_Click);
            // 
            // DayKLine
            // 
            this.DayKLine.Location = new System.Drawing.Point(21, 68);
            this.DayKLine.Name = "DayKLine";
            this.DayKLine.Size = new System.Drawing.Size(134, 23);
            this.DayKLine.TabIndex = 1;
            this.DayKLine.Text = "取DayKLine数据";
            this.DayKLine.UseVisualStyleBackColor = true;
            this.DayKLine.Click += new System.EventHandler(this.DayKLine_Click);
            // 
            // AllStockCode
            // 
            this.AllStockCode.Location = new System.Drawing.Point(161, 24);
            this.AllStockCode.Name = "AllStockCode";
            this.AllStockCode.Size = new System.Drawing.Size(88, 23);
            this.AllStockCode.TabIndex = 0;
            this.AllStockCode.Text = "AllStockCode";
            this.AllStockCode.UseVisualStyleBackColor = true;
            this.AllStockCode.Click += new System.EventHandler(this.AllStockCode_Click);
            // 
            // WeekHisLine
            // 
            this.WeekHisLine.Location = new System.Drawing.Point(162, 105);
            this.WeekHisLine.Name = "WeekHisLine";
            this.WeekHisLine.Size = new System.Drawing.Size(133, 23);
            this.WeekHisLine.TabIndex = 16;
            this.WeekHisLine.Text = "取WEEKLINE数据";
            this.WeekHisLine.UseVisualStyleBackColor = true;
            this.WeekHisLine.Click += new System.EventHandler(this.WeekHisLine_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(690, 331);
            this.Controls.Add(this.dbtool);
            this.Name = "Form1";
            this.Text = "数据库工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.memcached.ResumeLayout(false);
            this.memcached.PerformLayout();
            this.ptf_tb.ResumeLayout(false);
            this.dbAcv_tab.ResumeLayout(false);
            this.dbAcv_tab.PerformLayout();
            this.dbtool.ResumeLayout(false);
            this.stock_tab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage memcached;
        private System.Windows.Forms.Button show_cache_btn;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button crt_cache_btn;
        private System.Windows.Forms.TabPage ptf_tb;
        private System.Windows.Forms.Button exceltopdf_btn;
        private System.Windows.Forms.TabPage dbAcv_tab;
        private System.Windows.Forms.Button crt_znzz_script_btn;
        private System.Windows.Forms.Button CrtBean_btn;
        private System.Windows.Forms.Button crt_table_btn;
        private System.Windows.Forms.TextBox whereTB;
        private System.Windows.Forms.TextBox numStyleTB;
        private System.Windows.Forms.TextBox sqlOpTableTB;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button UpdBtn;
        private System.Windows.Forms.Button InsertBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl dbtool;
        private System.Windows.Forms.Button json_sel_file;
        private System.Windows.Forms.TextBox app_file_dir_textbox;
        private System.Windows.Forms.Button cmsniffer;
        private System.Windows.Forms.ProgressBar cmprogress;
        private System.Windows.Forms.TabPage stock_tab;
        private System.Windows.Forms.Button AllStockCode;
        private System.Windows.Forms.Button DayKLine;
        private System.Windows.Forms.Button HistoryKLine;
        private System.Windows.Forms.ProgressBar historydatabar;
        private System.Windows.Forms.Button Min5KLine;
        private System.Windows.Forms.Button test;
        private System.Windows.Forms.Button DayMACDCalc;
        private System.Windows.Forms.Button DayMACDAnalysis;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button DayKDJAnalysis;
        private System.Windows.Forms.Button DayKDJCalc;
        private System.Windows.Forms.Button WeekHisLine;

    }
}

