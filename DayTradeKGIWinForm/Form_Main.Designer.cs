namespace TradeBot
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
            this.tb_ServerTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_HeartBeats = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btn_Login = new System.Windows.Forms.Button();
            this.tb_UserPWD = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_UserID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_Port = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_Host = new System.Windows.Forms.ComboBox();
            this.groupBox_AddStock = new System.Windows.Forms.GroupBox();
            this.btn_AddStock = new System.Windows.Forms.Button();
            this.cb_LockGainMode = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cb_StopLossMode = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cb_BuyMode = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.nud_stoplossratio = new System.Windows.Forms.NumericUpDown();
            this.nud_LockGainPrice = new System.Windows.Forms.NumericUpDown();
            this.nud_BuyQty = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_StockID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dgv_StockList = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_Login.SuspendLayout();
            this.groupBox_AddStock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_stoplossratio)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_LockGainPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BuyQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StockList)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 56.53409F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.46591F));
            this.tableLayoutPanel1.Controls.Add(this.tb_Log, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_Login, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_AddStock, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgv_StockList, 0, 1);
            this.tableLayoutPanel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(920, 442);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // tb_Log
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.tb_Log, 2);
            this.tb_Log.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Log.Location = new System.Drawing.Point(3, 378);
            this.tb_Log.Multiline = true;
            this.tb_Log.Name = "tb_Log";
            this.tb_Log.ReadOnly = true;
            this.tb_Log.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tb_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Log.Size = new System.Drawing.Size(914, 61);
            this.tb_Log.TabIndex = 0;
            this.tb_Log.TabStop = false;
            // 
            // groupBox_Login
            // 
            this.groupBox_Login.AutoSize = true;
            this.groupBox_Login.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_Login.Controls.Add(this.tb_ServerTime);
            this.groupBox_Login.Controls.Add(this.label6);
            this.groupBox_Login.Controls.Add(this.tb_HeartBeats);
            this.groupBox_Login.Controls.Add(this.label5);
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
            this.groupBox_Login.Location = new System.Drawing.Point(523, 3);
            this.groupBox_Login.Name = "groupBox_Login";
            this.groupBox_Login.Size = new System.Drawing.Size(394, 104);
            this.groupBox_Login.TabIndex = 1;
            this.groupBox_Login.TabStop = false;
            this.groupBox_Login.Text = "登入";
            // 
            // tb_ServerTime
            // 
            this.tb_ServerTime.Location = new System.Drawing.Point(257, 73);
            this.tb_ServerTime.Name = "tb_ServerTime";
            this.tb_ServerTime.ReadOnly = true;
            this.tb_ServerTime.Size = new System.Drawing.Size(129, 22);
            this.tb_ServerTime.TabIndex = 11;
            this.tb_ServerTime.TabStop = false;
            this.tb_ServerTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(297, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "主機時間";
            // 
            // tb_HeartBeats
            // 
            this.tb_HeartBeats.Location = new System.Drawing.Point(299, 30);
            this.tb_HeartBeats.Name = "tb_HeartBeats";
            this.tb_HeartBeats.ReadOnly = true;
            this.tb_HeartBeats.Size = new System.Drawing.Size(51, 22);
            this.tb_HeartBeats.TabIndex = 9;
            this.tb_HeartBeats.TabStop = false;
            this.tb_HeartBeats.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(297, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "回應時間";
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(176, 58);
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
            // groupBox_AddStock
            // 
            this.groupBox_AddStock.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_AddStock.Controls.Add(this.btn_AddStock);
            this.groupBox_AddStock.Controls.Add(this.cb_LockGainMode);
            this.groupBox_AddStock.Controls.Add(this.label13);
            this.groupBox_AddStock.Controls.Add(this.cb_StopLossMode);
            this.groupBox_AddStock.Controls.Add(this.label12);
            this.groupBox_AddStock.Controls.Add(this.cb_BuyMode);
            this.groupBox_AddStock.Controls.Add(this.label11);
            this.groupBox_AddStock.Controls.Add(this.nud_stoplossratio);
            this.groupBox_AddStock.Controls.Add(this.nud_LockGainPrice);
            this.groupBox_AddStock.Controls.Add(this.nud_BuyQty);
            this.groupBox_AddStock.Controls.Add(this.label10);
            this.groupBox_AddStock.Controls.Add(this.label9);
            this.groupBox_AddStock.Controls.Add(this.label8);
            this.groupBox_AddStock.Controls.Add(this.tb_StockID);
            this.groupBox_AddStock.Controls.Add(this.label7);
            this.groupBox_AddStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_AddStock.Location = new System.Drawing.Point(3, 3);
            this.groupBox_AddStock.Name = "groupBox_AddStock";
            this.groupBox_AddStock.Size = new System.Drawing.Size(514, 104);
            this.groupBox_AddStock.TabIndex = 2;
            this.groupBox_AddStock.TabStop = false;
            this.groupBox_AddStock.Text = "新增股票";
            // 
            // btn_AddStock
            // 
            this.btn_AddStock.Location = new System.Drawing.Point(411, 57);
            this.btn_AddStock.Name = "btn_AddStock";
            this.btn_AddStock.Size = new System.Drawing.Size(75, 23);
            this.btn_AddStock.TabIndex = 17;
            this.btn_AddStock.Text = "新增\r\n";
            this.btn_AddStock.UseVisualStyleBackColor = true;
            this.btn_AddStock.Click += new System.EventHandler(this.btn_AddStock_Click);
            // 
            // cb_LockGainMode
            // 
            this.cb_LockGainMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_LockGainMode.FormattingEnabled = true;
            this.cb_LockGainMode.Items.AddRange(new object[] {
            "自動",
            "手動"});
            this.cb_LockGainMode.Location = new System.Drawing.Point(327, 54);
            this.cb_LockGainMode.Name = "cb_LockGainMode";
            this.cb_LockGainMode.Size = new System.Drawing.Size(64, 20);
            this.cb_LockGainMode.TabIndex = 16;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(275, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 15;
            this.label13.Text = "停利模式";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cb_StopLossMode
            // 
            this.cb_StopLossMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_StopLossMode.FormattingEnabled = true;
            this.cb_StopLossMode.Items.AddRange(new object[] {
            "自動",
            "手動"});
            this.cb_StopLossMode.Location = new System.Drawing.Point(199, 55);
            this.cb_StopLossMode.Name = "cb_StopLossMode";
            this.cb_StopLossMode.Size = new System.Drawing.Size(68, 20);
            this.cb_StopLossMode.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(140, 58);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 13;
            this.label12.Text = "停損模式";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cb_BuyMode
            // 
            this.cb_BuyMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_BuyMode.FormattingEnabled = true;
            this.cb_BuyMode.Items.AddRange(new object[] {
            "自動",
            "手動"});
            this.cb_BuyMode.Location = new System.Drawing.Point(63, 55);
            this.cb_BuyMode.Name = "cb_BuyMode";
            this.cb_BuyMode.Size = new System.Drawing.Size(61, 20);
            this.cb_BuyMode.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 58);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 11;
            this.label11.Text = "買進模式";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nud_stoplossratio
            // 
            this.nud_stoplossratio.DecimalPlaces = 1;
            this.nud_stoplossratio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nud_stoplossratio.Location = new System.Drawing.Point(397, 19);
            this.nud_stoplossratio.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_stoplossratio.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_stoplossratio.Name = "nud_stoplossratio";
            this.nud_stoplossratio.Size = new System.Drawing.Size(69, 22);
            this.nud_stoplossratio.TabIndex = 10;
            this.nud_stoplossratio.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nud_LockGainPrice
            // 
            this.nud_LockGainPrice.DecimalPlaces = 2;
            this.nud_LockGainPrice.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nud_LockGainPrice.Location = new System.Drawing.Point(251, 18);
            this.nud_LockGainPrice.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.nud_LockGainPrice.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_LockGainPrice.Name = "nud_LockGainPrice";
            this.nud_LockGainPrice.Size = new System.Drawing.Size(69, 22);
            this.nud_LockGainPrice.TabIndex = 9;
            this.nud_LockGainPrice.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nud_BuyQty
            // 
            this.nud_BuyQty.Location = new System.Drawing.Point(142, 18);
            this.nud_BuyQty.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_BuyQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_BuyQty.Name = "nud_BuyQty";
            this.nud_BuyQty.Size = new System.Drawing.Size(42, 22);
            this.nud_BuyQty.TabIndex = 8;
            this.nud_BuyQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(326, 22);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 6;
            this.label10.Text = "停損百分比";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(204, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 4;
            this.label9.Text = "停利價";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(114, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "買量";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tb_StockID
            // 
            this.tb_StockID.Location = new System.Drawing.Point(42, 18);
            this.tb_StockID.Name = "tb_StockID";
            this.tb_StockID.Size = new System.Drawing.Size(57, 22);
            this.tb_StockID.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "代號";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dgv_StockList
            // 
            this.dgv_StockList.AllowUserToAddRows = false;
            this.dgv_StockList.AllowUserToDeleteRows = false;
            this.dgv_StockList.AllowUserToOrderColumns = true;
            this.dgv_StockList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dgv_StockList, 2);
            this.dgv_StockList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_StockList.Location = new System.Drawing.Point(3, 113);
            this.dgv_StockList.MultiSelect = false;
            this.dgv_StockList.Name = "dgv_StockList";
            this.dgv_StockList.ReadOnly = true;
            this.dgv_StockList.RowHeadersVisible = false;
            this.dgv_StockList.RowTemplate.Height = 24;
            this.dgv_StockList.Size = new System.Drawing.Size(914, 259);
            this.dgv_StockList.TabIndex = 3;
            this.dgv_StockList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_StockList_CellMouseDown);
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(920, 442);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form_Main";
            this.Text = "大家來賺錢";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox_Login.ResumeLayout(false);
            this.groupBox_Login.PerformLayout();
            this.groupBox_AddStock.ResumeLayout(false);
            this.groupBox_AddStock.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_stoplossratio)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_LockGainPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BuyQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StockList)).EndInit();
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
        private System.Windows.Forms.TextBox tb_ServerTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_HeartBeats;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox_AddStock;
        private System.Windows.Forms.TextBox tb_StockID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nud_LockGainPrice;
        private System.Windows.Forms.NumericUpDown nud_BuyQty;
        private System.Windows.Forms.NumericUpDown nud_stoplossratio;
        private System.Windows.Forms.ComboBox cb_BuyMode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cb_StopLossMode;
        private System.Windows.Forms.ComboBox cb_LockGainMode;
        private System.Windows.Forms.Button btn_AddStock;
        private System.Windows.Forms.DataGridView dgv_StockList;
    }
}

