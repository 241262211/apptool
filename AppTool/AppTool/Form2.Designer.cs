namespace AppTool
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.LogTB = new System.Windows.Forms.RichTextBox();
            this.backBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(69, 33);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(384, 23);
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Click += new System.EventHandler(this.progressBar1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "进度";
            // 
            // LogTB
            // 
            this.LogTB.Location = new System.Drawing.Point(69, 84);
            this.LogTB.Name = "LogTB";
            this.LogTB.Size = new System.Drawing.Size(384, 175);
            this.LogTB.TabIndex = 2;
            this.LogTB.Text = "";
            // 
            // backBtn
            // 
            //this.backBtn.Location = new System.Drawing.Point(212, 266);
            //this.backBtn.Name = "backBtn";
            //this.backBtn.Size = new System.Drawing.Size(75, 23);
            //this.backBtn.TabIndex = 3;
            //this.backBtn.Text = "返回";
            //this.backBtn.UseVisualStyleBackColor = true;
            //this.backBtn.Click += new System.EventHandler(this.backBtn_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 308);
            this.Controls.Add(this.backBtn);
            this.Controls.Add(this.LogTB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.Name = "Form2";
            this.Text = "DBtool1.0";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox LogTB;
        private System.Windows.Forms.Button backBtn;
    }
}