namespace TradeBot
{
    partial class StockEditor
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
            this.dgv_StockEditor = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.nud_AmountThreshold = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.cb_BuyMode = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.nud_BuyQty = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.tb_StockID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox_AddStock = new System.Windows.Forms.GroupBox();
            this.btn_AddStock = new System.Windows.Forms.Button();
            this.cb_LockGainMode = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cb_StopLossMode = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.btn_LoadHistoryData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StockEditor)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_AmountThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BuyQty)).BeginInit();
            this.groupBox_AddStock.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgv_StockEditor
            // 
            this.dgv_StockEditor.AllowUserToAddRows = false;
            this.dgv_StockEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_StockEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_StockEditor.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv_StockEditor.Location = new System.Drawing.Point(3, 65);
            this.dgv_StockEditor.Name = "dgv_StockEditor";
            this.dgv_StockEditor.RowTemplate.Height = 24;
            this.dgv_StockEditor.Size = new System.Drawing.Size(827, 554);
            this.dgv_StockEditor.TabIndex = 0;
            this.dgv_StockEditor.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgv_StockEditor_UserDeletingRow);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.groupBox_AddStock, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgv_StockEditor, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(833, 622);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // nud_AmountThreshold
            // 
            this.nud_AmountThreshold.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_AmountThreshold.Location = new System.Drawing.Point(140, 31);
            this.nud_AmountThreshold.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nud_AmountThreshold.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_AmountThreshold.Name = "nud_AmountThreshold";
            this.nud_AmountThreshold.Size = new System.Drawing.Size(65, 22);
            this.nud_AmountThreshold.TabIndex = 19;
            this.nud_AmountThreshold.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(119, 15);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(113, 12);
            this.label14.TabIndex = 18;
            this.label14.Text = "前五分鐘成交量門檻";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cb_BuyMode
            // 
            this.cb_BuyMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_BuyMode.FormattingEnabled = true;
            this.cb_BuyMode.Items.AddRange(new object[] {
            "自動",
            "手動"});
            this.cb_BuyMode.Location = new System.Drawing.Point(235, 30);
            this.cb_BuyMode.Name = "cb_BuyMode";
            this.cb_BuyMode.Size = new System.Drawing.Size(61, 20);
            this.cb_BuyMode.TabIndex = 12;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(238, 15);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 11;
            this.label11.Text = "買進模式";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nud_BuyQty
            // 
            this.nud_BuyQty.Location = new System.Drawing.Point(73, 30);
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
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(78, 15);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "買量";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tb_StockID
            // 
            this.tb_StockID.Location = new System.Drawing.Point(9, 30);
            this.tb_StockID.Name = "tb_StockID";
            this.tb_StockID.Size = new System.Drawing.Size(57, 22);
            this.tb_StockID.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "代號";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox_AddStock
            // 
            this.groupBox_AddStock.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox_AddStock.Controls.Add(this.btn_LoadHistoryData);
            this.groupBox_AddStock.Controls.Add(this.nud_AmountThreshold);
            this.groupBox_AddStock.Controls.Add(this.label14);
            this.groupBox_AddStock.Controls.Add(this.btn_AddStock);
            this.groupBox_AddStock.Controls.Add(this.cb_LockGainMode);
            this.groupBox_AddStock.Controls.Add(this.label13);
            this.groupBox_AddStock.Controls.Add(this.cb_StopLossMode);
            this.groupBox_AddStock.Controls.Add(this.label12);
            this.groupBox_AddStock.Controls.Add(this.cb_BuyMode);
            this.groupBox_AddStock.Controls.Add(this.label11);
            this.groupBox_AddStock.Controls.Add(this.nud_BuyQty);
            this.groupBox_AddStock.Controls.Add(this.label8);
            this.groupBox_AddStock.Controls.Add(this.tb_StockID);
            this.groupBox_AddStock.Controls.Add(this.label7);
            this.groupBox_AddStock.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox_AddStock.Location = new System.Drawing.Point(3, 3);
            this.groupBox_AddStock.Name = "groupBox_AddStock";
            this.groupBox_AddStock.Size = new System.Drawing.Size(827, 56);
            this.groupBox_AddStock.TabIndex = 3;
            this.groupBox_AddStock.TabStop = false;
            this.groupBox_AddStock.Text = "新增股票";
            // 
            // btn_AddStock
            // 
            this.btn_AddStock.Location = new System.Drawing.Point(446, 25);
            this.btn_AddStock.Name = "btn_AddStock";
            this.btn_AddStock.Size = new System.Drawing.Size(62, 23);
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
            this.cb_LockGainMode.Location = new System.Drawing.Point(376, 28);
            this.cb_LockGainMode.Name = "cb_LockGainMode";
            this.cb_LockGainMode.Size = new System.Drawing.Size(64, 20);
            this.cb_LockGainMode.TabIndex = 16;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(380, 13);
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
            this.cb_StopLossMode.Location = new System.Drawing.Point(302, 29);
            this.cb_StopLossMode.Name = "cb_StopLossMode";
            this.cb_StopLossMode.Size = new System.Drawing.Size(68, 20);
            this.cb_StopLossMode.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(308, 15);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 13;
            this.label12.Text = "停損模式";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_LoadHistoryData
            // 
            this.btn_LoadHistoryData.Location = new System.Drawing.Point(582, 26);
            this.btn_LoadHistoryData.Name = "btn_LoadHistoryData";
            this.btn_LoadHistoryData.Size = new System.Drawing.Size(104, 23);
            this.btn_LoadHistoryData.TabIndex = 20;
            this.btn_LoadHistoryData.Text = "載入前一次資料";
            this.btn_LoadHistoryData.UseVisualStyleBackColor = true;
            this.btn_LoadHistoryData.Click += new System.EventHandler(this.btn_LoadHistoryData_Click);
            // 
            // StockEditor
            // 
            this.AcceptButton = this.btn_AddStock;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 622);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "StockEditor";
            this.Text = "股票設定編輯";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StockEditor_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StockEditor)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nud_AmountThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_BuyQty)).EndInit();
            this.groupBox_AddStock.ResumeLayout(false);
            this.groupBox_AddStock.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_StockEditor;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox_AddStock;
        private System.Windows.Forms.NumericUpDown nud_AmountThreshold;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_AddStock;
        private System.Windows.Forms.ComboBox cb_LockGainMode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cb_StopLossMode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cb_BuyMode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown nud_BuyQty;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tb_StockID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_LoadHistoryData;
    }
}