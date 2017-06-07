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
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StockEditor)).BeginInit();
            this.SuspendLayout();
            // 
            // dgv_StockEditor
            // 
            this.dgv_StockEditor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_StockEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_StockEditor.Location = new System.Drawing.Point(0, 0);
            this.dgv_StockEditor.Name = "dgv_StockEditor";
            this.dgv_StockEditor.RowTemplate.Height = 24;
            this.dgv_StockEditor.Size = new System.Drawing.Size(749, 488);
            this.dgv_StockEditor.TabIndex = 0;
            // 
            // StockEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(749, 488);
            this.Controls.Add(this.dgv_StockEditor);
            this.Name = "StockEditor";
            this.Text = "股票設定編輯";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StockEditor_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_StockEditor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgv_StockEditor;
    }
}