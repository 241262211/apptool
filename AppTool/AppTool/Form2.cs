using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppTool
{
    public partial class Form2 : Form
    {
        //public static Form2 pBarForm = null;
        public Form2()
        {

            InitializeComponent();
           // pBarForm = this;
        }

        

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 指定为百分之percentValue的位置
        /// </summary>
        /// <param name="percentValue"></param>
        public void SetPBar(int percentValue)
        {
            progressBar1.Value = percentValue;
           

        }
        public void SetTextVal(string value)
        {
            LogTB.Text = LogTB.Text + value + "\r\n";


        }

    }
}
