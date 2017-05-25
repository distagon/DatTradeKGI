namespace DayTradeKGIWinForm
{
    partial class Form_Main
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tb_Log = new System.Windows.Forms.TextBox();
            this.groupBox_Login = new System.Windows.Forms.GroupBox();
            this.btn_Login = new System.Windows.Forms.Button();
            this.tb_UserPWD = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_UserID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_Host = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_Login.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.tb_Log, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_Login, 0, 0);
            this.tableLayoutPanel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(704, 442);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tb_Log
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tb_Log, 2);
            this.tb_Log.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Log.Location = new System.Drawing.Point(3, 400);
            this.tb_Log.Multiline = true;
            this.tb_Log.Name = "tb_Log";
            this.tb_Log.ReadOnly = true;
            this.tb_Log.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tb_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Log.Size = new System.Drawing.Size(698, 39);
            this.tb_Log.TabIndex = 0;
            this.tb_Log.TabStop = false;
            // 
            // groupBox_Login
            // 
            this.groupBox_Login.Controls.Add(this.btn_Login);
            this.groupBox_Login.Controls.Add(this.tb_UserPWD);
            this.groupBox_Login.Controls.Add(this.label4);
            this.groupBox_Login.Controls.Add(this.tb_UserID);
            this.groupBox_Login.Controls.Add(this.label3);
            this.groupBox_Login.Controls.Add(this.tb_Port);
            this.groupBox_Login.Controls.Add(this.label2);
            this.groupBox_Login.Controls.Add(this.label1);
            this.groupBox_Login.Controls.Add(this.cb_Host);
            this.groupBox_Login.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_Login.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Login.Name = "groupBox_Login";
            this.groupBox_Login.Size = new System.Drawing.Size(346, 91);
            this.groupBox_Login.TabIndex = 1;
            this.groupBox_Login.TabStop = false;
            this.groupBox_Login.Text = "登入";
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(194, 63);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(75, 23);
            this.btn_Login.TabIndex = 7;
            this.btn_Login.Text = "登入";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // tb_UserPWD
            // 
            this.tb_UserPWD.Location = new System.Drawing.Point(45, 63);
            this.tb_UserPWD.Name = "tb_UserPWD";
            this.tb_UserPWD.Size = new System.Drawing.Size(121, 22);
            this.tb_UserPWD.TabIndex = 6;
            this.tb_UserPWD.Text = "0000";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "密碼";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_UserID
            // 
            this.tb_UserID.Location = new System.Drawing.Point(45, 35);
            this.tb_UserID.Name = "tb_UserID";
            this.tb_UserID.Size = new System.Drawing.Size(121, 22);
            this.tb_UserID.TabIndex = 4;
            this.tb_UserID.Text = "H220566876";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(33, 12);
            this.label3.TabIndex = 0;
            this.label3.Text = "ID";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_Port
            // 
            this.tb_Port.Location = new System.Drawing.Point(204, 12);
            this.tb_Port.Name = "tb_Port";
            this.tb_Port.Size = new System.Drawing.Size(52, 22);
            this.tb_Port.TabIndex = 3;
            this.tb_Port.Text = "443";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(174, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "port";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "主機";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cb_Host
            // 
            this.cb_Host.FormattingEnabled = true;
            this.cb_Host.Items.AddRange(new object[] {
            "211.20.186.12"});
            this.cb_Host.Location = new System.Drawing.Point(45, 12);
            this.cb_Host.Name = "cb_Host";
            this.cb_Host.Size = new System.Drawing.Size(121, 20);
            this.cb_Host.TabIndex = 0;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 442);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form_Main";
            this.Text = "大家來賺錢";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox_Login.ResumeLayout(false);
            this.groupBox_Login.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox tb_Log;
        private System.Windows.Forms.GroupBox groupBox_Login;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_Host;
        private System.Windows.Forms.TextBox tb_Port;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tb_UserPWD;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_UserID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_Login;
    }
}

