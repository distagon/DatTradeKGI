using Intelligence;
using Smart;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TradeBot
{
    public partial class StockEditor : Form
    {
        private DataTable StockTable;
        public QuoteCom quotecom;
        public TaiFexCom tfcom;
        public string brokerid = "";
        public string account = "";
        public DataTable StatusTable = new DataTable("StatusList", "TradeBot");
        public DataTable BuyModeTable = new DataTable("BuyModeList", "TradeBot");
        public DataTable StopLossModeTable = new DataTable("StopLossModeList", "TradeBot");
        public DataTable LockGainModeTable = new DataTable("LockGainModeList", "TradeBot");
        private string StockHistoryFile;

        public StockEditor(DataTable table,QuoteCom quotecom,TaiFexCom tfcom,string brokerid,string account,string StockHistoryFile)
        {
            InitializeComponent();
            this.StockTable = table;
            this.quotecom = quotecom;
            this.tfcom = tfcom;
            this.brokerid = brokerid;
            this.account = account;
            this.StockHistoryFile = StockHistoryFile;

            //建立下拉選單表格
            BuyModeTable.Columns.Add("BuyMode", typeof(string));
            BuyModeTable.Columns.Add("BuyMode_Text", typeof(string));
            BuyModeTable.Rows.Add("Auto", "自動");
            BuyModeTable.Rows.Add("Notify", "手動");
            BindingSource bindingBuyMode = new BindingSource();
            bindingBuyMode.DataSource = BuyModeTable;

            StatusTable.Columns.Add("Status", typeof(string));
            StatusTable.Columns.Add("Status_Text", typeof(string));
            StatusTable.Rows.Add("StandBy", "待命中");
            StatusTable.Rows.Add("WaitingBuySignal", "等待買入訊號");
            StatusTable.Rows.Add("WaitingBuy", "等待買入");
            StatusTable.Rows.Add("ConfirmBuyOrder", "確認委托買單");
            StatusTable.Rows.Add("ConfirmBuyMatch", "確認買單成交");
            StatusTable.Rows.Add("WaitingSellSignal", "等待賣出訊號");
            StatusTable.Rows.Add("WaitingSell", "等待賣出");
            StatusTable.Rows.Add("ConfirmSellOrder", "確認委托賣單");
            StatusTable.Rows.Add("ConfirmSellMatch", "確認賣單成交");
            StatusTable.Rows.Add("Stop", "中止流程");
            StatusTable.Rows.Add("Error", "錯誤");

            BindingSource bindingStatus = new BindingSource();
            bindingStatus.DataSource = StatusTable;

            StopLossModeTable.Columns.Add("StopLossMode", typeof(string));
            StopLossModeTable.Columns.Add("StopLossMode_Text", typeof(string));
            StopLossModeTable.Rows.Add("Auto", "自動");
            StopLossModeTable.Rows.Add("Manual", "手動");
            BindingSource bindingStopLossMode = new BindingSource();
            bindingStopLossMode.DataSource = StopLossModeTable;

            LockGainModeTable.Columns.Add("LockGainMode", typeof(string));
            LockGainModeTable.Columns.Add("LockGainMode_Text", typeof(string));
            LockGainModeTable.Rows.Add("Auto", "自動");
            LockGainModeTable.Rows.Add("Manual", "手動");
            BindingSource bindingLockGainMode = new BindingSource();
            bindingLockGainMode.DataSource = LockGainModeTable;

            //增加欄位名稱
            dgv_StockEditor.Columns.Add("StockID", "代號");
            dgv_StockEditor.Columns["StockID"].DataPropertyName = "StockID";
            dgv_StockEditor.Columns["StockID"].ReadOnly = true;


            DataGridViewComboBoxColumn StatusCol = new DataGridViewComboBoxColumn();
            StatusCol.ValueMember = "Status";
            StatusCol.DisplayMember = "Status_Text";
            StatusCol.DataSource = bindingStatus;
            StatusCol.Name = "Status";
            StatusCol.DataPropertyName = "Status";
            StatusCol.HeaderText = "狀態";
            StatusCol.ReadOnly = true;
            StatusCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            StatusCol.SortMode = DataGridViewColumnSortMode.Automatic;
            dgv_StockEditor.Columns.Add(StatusCol);

            dgv_StockEditor.Columns.Add("AmountThreshold", "成交量門檻");
            dgv_StockEditor.Columns["AmountThreshold"].DataPropertyName = "AmountThreshold";
            dgv_StockEditor.Columns["AmountThreshold"].ToolTipText = "前五分鐘成交量門檻";
            //dgv_StockEditor.Columns["AmountThreshold"].ReadOnly = true;



            DataGridViewComboBoxColumn BuyModeCol = new DataGridViewComboBoxColumn();
            BuyModeCol.ValueMember = "BuyMode";
            BuyModeCol.DisplayMember = "BuyMode_Text";
            BuyModeCol.DataSource = bindingBuyMode;
            BuyModeCol.Name = "BuyMode";
            BuyModeCol.DataPropertyName = "BuyMode";
            BuyModeCol.HeaderText = "買入模式";
            //BuyModeCol.ReadOnly = true;
            BuyModeCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            BuyModeCol.SortMode = DataGridViewColumnSortMode.Automatic;
            dgv_StockEditor.Columns.Add(BuyModeCol);

            DataGridViewComboBoxColumn StopLossModeCol = new DataGridViewComboBoxColumn();
            StopLossModeCol.ValueMember = "StopLossMode";
            StopLossModeCol.DisplayMember = "StopLossMode_Text";
            StopLossModeCol.DataSource = bindingStopLossMode;
            StopLossModeCol.Name = "StopLossMode";
            StopLossModeCol.DataPropertyName = "StopLossMode";
            StopLossModeCol.HeaderText = "停損模式";
            //StopLossModeCol.ReadOnly = true;
            StopLossModeCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            StopLossModeCol.SortMode = DataGridViewColumnSortMode.Automatic;
            dgv_StockEditor.Columns.Add(StopLossModeCol);

            DataGridViewComboBoxColumn LockGainModeCol = new DataGridViewComboBoxColumn();
            LockGainModeCol.ValueMember = "LockGainMode";
            LockGainModeCol.DisplayMember = "LockGainMode_Text";
            LockGainModeCol.DataSource = bindingLockGainMode;
            LockGainModeCol.Name = "LockGainMode";
            LockGainModeCol.DataPropertyName = "LockGainMode";
            LockGainModeCol.HeaderText = "停利模式";
            //LockGainModeCol.ReadOnly = true;
            LockGainModeCol.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            LockGainModeCol.SortMode = DataGridViewColumnSortMode.Automatic;
            dgv_StockEditor.Columns.Add(LockGainModeCol);

            //DataGridViewComboBoxColumn BuyQtyCol = new DataGridViewComboBoxColumn();
            //BuyQtyCol.Items.AddRange(1, 2, 3, 4, 5, 6, 7, 8, 9, 10);
            //BuyQtyCol.Name = "BuyQty";
            //BuyQtyCol.DataPropertyName = "BuyQty";
            //BuyQtyCol.HeaderText = "設定買量";
            //dgv_StockList.Columns.Add(BuyQtyCol);
            dgv_StockEditor.Columns.Add("BuyQty", "設定買量");
            dgv_StockEditor.Columns["BuyQty"].DataPropertyName = "BuyQty";
            //dgv_StockEditor.Columns["BuyQty"].ReadOnly = true;


            BindingSource bs = new BindingSource();
            bs.DataSource = StockTable;
            dgv_StockEditor.DataSource = bs;

            ////隱藏不顯示欄位
            dgv_StockEditor.Columns["TradeBot"].Visible = false;
            dgv_StockEditor.Columns["BuyAvgPrice"].Visible = false;
            dgv_StockEditor.Columns["MatchBuyQty"].Visible = false;
            dgv_StockEditor.Columns["SellAvgPrice"].Visible = false;
            dgv_StockEditor.Columns["MatchSellQty"].Visible = false;
            dgv_StockEditor.Columns["ROI"].Visible = false;
            dgv_StockEditor.Columns["ClosePrice"].Visible = false;
            dgv_StockEditor.Columns["OpenPrice"].Visible = false;
            dgv_StockEditor.Columns["MatchTime"].Visible = false;
            dgv_StockEditor.Columns["MatchPrice"].Visible = false;
            dgv_StockEditor.Columns["MatchQty"].Visible = false;
            dgv_StockEditor.Columns["TotalQty"].Visible = false;
            dgv_StockEditor.Columns["AH"].Visible = false;
            dgv_StockEditor.Columns["NH"].Visible = false;
            dgv_StockEditor.Columns["NL"].Visible = false;
            dgv_StockEditor.Columns["AL"].Visible = false;

            //設定下拉選單預設值
            cb_BuyMode.SelectedIndex = 0;
            cb_StopLossMode.SelectedIndex = 0;
            cb_LockGainMode.SelectedIndex = 0;

        }

        private void StockEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void btn_AddStock_Click(object sender, EventArgs e)
        {
            string stockid = tb_StockID.Text;
            if (!StockTable.Rows.Contains(stockid))
            {
                ushort buyqty = (ushort)nud_BuyQty.Value;
                //decimal stoplossratio = (decimal)nud_stoplossratio.Value;
                //decimal lockgainprice = (decimal)nud_LockGainPrice.Value;
                int AmountThreshold = (Int32)nud_AmountThreshold.Value;
                BuyMode buymode = cb_BuyMode.SelectedIndex == 0 ? BuyMode.Auto : BuyMode.Notify;
                StopLossMode stoplossmode = cb_StopLossMode.SelectedIndex == 0 ? StopLossMode.Auto : StopLossMode.Manual;
                LockGainMode lockgainmode = cb_LockGainMode.SelectedIndex == 0 ? LockGainMode.Auto : LockGainMode.Manual;
                AddStock(stockid,AmountThreshold,buyqty,buymode,stoplossmode,lockgainmode);
            }
            else
                MessageBox.Show("該檔股票已存在");
        }

        private void AddStock(string stockid,int AmountThreshold,ushort buyqty,BuyMode buymode,StopLossMode stoplossmode,LockGainMode lockgainmode) {
            DataRow row = StockTable.NewRow();
            row["StockID"] = stockid;
            row["AmountThreshold"] = AmountThreshold;
            row["BuyQty"] = buyqty;
            row["BuyMode"] = buymode;
            row["StopLossMode"] = stoplossmode;
            row["LockGainMode"] = lockgainmode;
            TradeBotBase tb = new TradeBotLongQA(stockid, brokerid, account, buyqty, quotecom, tfcom, AmountThreshold, buymode, stoplossmode, lockgainmode);
            row["Status"] = tb.trade_status;
            row["TradeBot"] = tb;
            StockTable.Rows.Add(row);
        }

        private void dgv_StockEditor_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            string stockid = e.Row.Cells[0].Value.ToString();
            DataRow row = StockTable.Rows.Find(stockid);
            if (row != null)
            {
                string ts = row["Status"].ToString();
                if (ts == TradeStatus.WaitingSell.ToString() || ts == TradeStatus.WaitingSellSignal.ToString())
                {
                    MessageBox.Show("該檔股票已經買入，等待賣出，無法刪除");
                    e.Cancel = true;
                }
                else {
                    DialogResult result = MessageBox.Show("是否要刪除 "+stockid+ " ", "警告", MessageBoxButtons.YesNo);
                    if (result != DialogResult.Yes) {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void btn_LoadHistoryData_Click(object sender, EventArgs e)
        {
            if (File.Exists(StockHistoryFile))
            {
                string[] lines = File.ReadAllLines(StockHistoryFile);
                foreach (string line in lines)
                {
                    string[] data = line.Split(new char[] { ',' },StringSplitOptions.RemoveEmptyEntries);
                    string stockid = data[0];                    
                    int AmountThreshold = Convert.ToInt32(data[1]) ;
                    BuyMode buymode = data[2] == "Auto" ? BuyMode.Auto : BuyMode.Notify;
                    StopLossMode stoplossmode = data[3] == "Auto" ? StopLossMode.Auto : StopLossMode.Manual;
                    LockGainMode lockgainmode = data[4] == "Auto" ? LockGainMode.Auto : LockGainMode.Manual;
                    ushort buyqty = Convert.ToUInt16(data[5]);
                    AddStock(stockid, AmountThreshold, buyqty, buymode, stoplossmode, lockgainmode);
                }
            }
                
        }
    }
}
