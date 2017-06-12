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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.dgv_StockList = new System.Windows.Forms.DataGridView();
            this.btn_ConfigStock = new System.Windows.Forms.Button();
            this.btn_StartALL = new System.Windows.Forms.Button();
            this.btn_StopAll = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox_Login.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StockList)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.tb_Log, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgv_StockList, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox_Login, 0, 0);
            this.tableLayoutPanel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(920, 442);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // tb_Log
            // 
            this.tb_Log.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.tb_Log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tb_Log.Location = new System.Drawing.Point(3, 376);
            this.tb_Log.Multiline = true;
            this.tb_Log.Name = "tb_Log";
            this.tb_Log.ReadOnly = true;
            this.tb_Log.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tb_Log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_Log.Size = new System.Drawing.Size(914, 63);
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
            this.groupBox_Login.Location = new System.Drawing.Point(3, 3);
            this.groupBox_Login.Name = "groupBox_Login";
            this.groupBox_Login.Size = new System.Drawing.Size(914, 54);
            this.groupBox_Login.TabIndex = 1;
            this.groupBox_Login.TabStop = false;
            this.groupBox_Login.Text = "登入";
            // 
            // tb_ServerTime
            // 
            this.tb_ServerTime.Location = new System.Drawing.Point(779, 30);
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
            this.label6.Location = new System.Drawing.Point(811, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "主機時間";
            // 
            // tb_HeartBeats
            // 
            this.tb_HeartBeats.Location = new System.Drawing.Point(722, 30);
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
            this.label5.Location = new System.Drawing.Point(720, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "回應時間";
            // 
            // btn_Login
            // 
            this.btn_Login.Location = new System.Drawing.Point(601, 13);
            this.btn_Login.Name = "btn_Login";
            this.btn_Login.Size = new System.Drawing.Size(75, 23);
            this.btn_Login.TabIndex = 7;
            this.btn_Login.Text = "登入";
            this.btn_Login.UseVisualStyleBackColor = true;
            this.btn_Login.Click += new System.EventHandler(this.btn_Login_Click);
            // 
            // tb_UserPWD
            // 
            this.tb_UserPWD.Location = new System.Drawing.Point(464, 12);
            this.tb_UserPWD.Name = "tb_UserPWD";
            this.tb_UserPWD.Size = new System.Drawing.Size(121, 22);
            this.tb_UserPWD.TabIndex = 6;
            this.tb_UserPWD.Text = "0000";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(425, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(33, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "密碼";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tb_UserID
            // 
            this.tb_UserID.Location = new System.Drawing.Point(302, 12);
            this.tb_UserID.Name = "tb_UserID";
            this.tb_UserID.Size = new System.Drawing.Size(121, 22);
            this.tb_UserID.TabIndex = 4;
            this.tb_UserID.Text = "H220566876";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(263, 17);
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
            // dgv_StockList
            // 
            this.dgv_StockList.AllowUserToAddRows = false;
            this.dgv_StockList.AllowUserToDeleteRows = false;
            this.dgv_StockList.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv_StockList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_StockList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_StockList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_StockList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_StockList.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgv_StockList.Location = new System.Drawing.Point(3, 103);
            this.dgv_StockList.MultiSelect = false;
            this.dgv_StockList.Name = "dgv_StockList";
            this.dgv_StockList.ReadOnly = true;
            this.dgv_StockList.RowHeadersVisible = false;
            this.dgv_StockList.RowTemplate.Height = 24;
            this.dgv_StockList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_StockList.Size = new System.Drawing.Size(914, 267);
            this.dgv_StockList.TabIndex = 3;
            this.dgv_StockList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_StockList_CellClick);
            this.dgv_StockList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv_StockList_CellMouseDown);
            this.dgv_StockList.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgv_StockList_DataError);
            // 
            // btn_ConfigStock
            // 
            this.btn_ConfigStock.Location = new System.Drawing.Point(165, 3);
            this.btn_ConfigStock.Name = "btn_ConfigStock";
            this.btn_ConfigStock.Size = new System.Drawing.Size(90, 23);
            this.btn_ConfigStock.TabIndex = 20;
            this.btn_ConfigStock.Text = "編輯股票設定";
            this.btn_ConfigStock.UseVisualStyleBackColor = true;
            this.btn_ConfigStock.Click += new System.EventHandler(this.btn_ConfigStock_Click);
            // 
            // btn_StartALL
            // 
            this.btn_StartALL.Location = new System.Drawing.Point(3, 3);
            this.btn_StartALL.Name = "btn_StartALL";
            this.btn_StartALL.Size = new System.Drawing.Size(75, 23);
            this.btn_StartALL.TabIndex = 21;
            this.btn_StartALL.Text = "全部啟動";
            this.btn_StartALL.UseVisualStyleBackColor = true;
            this.btn_StartALL.Click += new System.EventHandler(this.btn_StartALL_Click);
            // 
            // btn_StopAll
            // 
            this.btn_StopAll.Location = new System.Drawing.Point(84, 3);
            this.btn_StopAll.Name = "btn_StopAll";
            this.btn_StopAll.Size = new System.Drawing.Size(75, 23);
            this.btn_StopAll.TabIndex = 22;
            this.btn_StopAll.Text = "全部停止";
            this.btn_StopAll.UseVisualStyleBackColor = true;
            this.btn_StopAll.Click += new System.EventHandler(this.btn_StopAll_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btn_StartALL);
            this.flowLayoutPanel1.Controls.Add(this.btn_StopAll);
            this.flowLayoutPanel1.Controls.Add(this.btn_ConfigStock);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 63);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(914, 34);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // Form_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(920, 442);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form_Main";
            this.Text = "大家來賺錢";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form_Main_FormClosed);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox_Login.ResumeLayout(false);
            this.groupBox_Login.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StockList)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridView dgv_StockList;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btn_StartALL;
        private System.Windows.Forms.Button btn_StopAll;
        private System.Windows.Forms.Button btn_ConfigStock;
    }
}

