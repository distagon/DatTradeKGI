using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Intelligence;
using Package;
using Smart;
using TradeBot;
using System.IO;


namespace TradeBot
{
    delegate void SetAddInfoCallBack(string text);
    public partial class Form_Main : Form
    {
        public  QuoteCom quotecom;
        public  TaiFexCom tfcom;
        public  Dictionary<string, int> RecoverMap = new Dictionary<string, int>();
        public  UTF8Encoding encoding = new System.Text.UTF8Encoding();
        public  String host = "211.20.186.12";
        public  ushort port = 443;
        public  String SID = "API";
        public  String Token = "b6eb";
        public  String id = "H220566876";
        public  String pwd = "0000";
        public const char area = ' ';
        private string group = "";
        public  string brokerid = "";
        public  string account = "";
        public DataTable StockTable=new DataTable("StockList", "TradeBot");
        private string DownloadTarget = "";


        public Form_Main()
        {
            InitializeComponent();
            //初始化行情下載物件
            quotecom = new Intelligence.QuoteCom(host, port, SID, Token); //接收證券行情物件
            quotecom.SourceId = SID;
            quotecom.OnRcvMessage += OnQuoteRcvMessage;
            quotecom.OnGetStatus += OnQuoteGetStatus;
            quotecom.OnRecoverStatus += OnRecoverStatus;
            //初始化下單物件
            tfcom = new Smart.TaiFexCom(host, port, SID);// 券證下單物件

            tfcom.OnRcvMessage += OntfcomRcvMessage;          //資料接收事件
            tfcom.OnGetStatus += OntfcomGetStatus;               //狀態通知事件
            tfcom.OnRcvServerTime += OntfcomRcvServerTime;   //接收主機時間
            //tfcom.OnRecoverStatus += OntfcomRecoverStatus;   //回補狀態通知

            //初始化表格
            DataColumn StockIdCol = StockTable.Columns.Add("StockID", typeof(String)); //股票代號
            StockTable.PrimaryKey = new DataColumn[] { StockIdCol };
            StockTable.Columns.Add("Status", typeof(TradeStatus)); //目前狀態
            StockTable.Columns.Add("Qty", typeof(Int16)); //預計買入張數
            StockTable.Columns.Add("BuyAvgPrice", typeof(decimal)); //買入平均成本
            StockTable.Columns.Add("SellAvgPrice", typeof(decimal)); //賣出平均成本
            StockTable.Columns.Add("ROI", typeof(decimal)); //報酬率
            StockTable.Columns.Add("ClosePrice", typeof(double)); //昨日收盤價
            StockTable.Columns.Add("Open", typeof(decimal)); //今日開盤價
            StockTable.Columns.Add("MatchTime", typeof(Int32)); //即時成交時間
            StockTable.Columns.Add("MatchPrice", typeof(decimal)); //即時成交價
            StockTable.Columns.Add("MatchQty", typeof(decimal));//即時成交量
            StockTable.Columns.Add("TotalQty", typeof(decimal)); //累積成交量
            StockTable.Columns.Add("AH", typeof(double));
            StockTable.Columns.Add("NH", typeof(double));
            StockTable.Columns.Add("NL", typeof(double));
            StockTable.Columns.Add("AL", typeof(double));
            StockTable.Columns.Add("TradeBot", typeof(TradeBotBase));
            BindingSource bs = new BindingSource();
            bs.DataSource = StockTable;
            dgv_StockList.DataSource = bs;

            //調整表格欄位名稱
            dgv_StockList.Columns["StockID"].HeaderText = "代號";
            dgv_StockList.Columns["Status"].HeaderText = "狀態";
            dgv_StockList.Columns["Qty"].HeaderText = "買量";
            dgv_StockList.Columns["BuyAvgPrice"].HeaderText = "買入平均成本";
            dgv_StockList.Columns["SellAvgPrice"].HeaderText = "賣出平均成本";
            dgv_StockList.Columns["ROI"].HeaderText = "報酬率";
            dgv_StockList.Columns["ClosePrice"].HeaderText = "昨日收盤價";
            dgv_StockList.Columns["Open"].HeaderText = "今日開盤價";
            dgv_StockList.Columns["MatchTime"].HeaderText = "即時成交時間";
            dgv_StockList.Columns["MatchPrice"].HeaderText = "即時成交價";
            dgv_StockList.Columns["MatchQty"].HeaderText = "即時成交量";
            dgv_StockList.Columns["TotalQty"].HeaderText = "累積成交量";

            //隱藏不顯示欄位
            dgv_StockList.Columns["TradeBot"].Visible=false;

            //設定下拉選單預設值
            cb_Host.SelectedIndex = 0;
            cb_BuyMode.SelectedIndex = 0;
            cb_StopLossMode.SelectedIndex = 0;
            cb_LockGainMode.SelectedIndex = 0;

            //初始化成交明細下載目錄
            this.DownloadTarget = Path.Combine( Directory.GetCurrentDirectory(),DateTime.Now.ToString("yyyyMMdd"));
            //建立目錄
            if (!Directory.Exists(this.DownloadTarget))
            {
                Directory.CreateDirectory(this.DownloadTarget);
            }
            Console.WriteLine(DownloadTarget);


        }
        #region API 事件處理
        private void OnQuoteGetStatus(object sender, COM_STATUS staus, byte[] msg)
        {
            QuoteCom com = (QuoteCom)sender;
            string smsg = null;
            switch (staus)
            {
                case COM_STATUS.LOGIN_READY:
                    AddInfo(String.Format("LOGIN_READY:[{0}]", encoding.GetString(msg)));
                    break;
                case COM_STATUS.LOGIN_FAIL:
                    AddInfo(String.Format("LOGIN FAIL:[{0}]", encoding.GetString(msg)));
                    break;
                case COM_STATUS.LOGIN_UNKNOW:
                    AddInfo(String.Format("LOGIN UNKNOW:[{0}]", encoding.GetString(msg)));
                    break;
                case COM_STATUS.CONNECT_READY:
                    //quoteCom.Login(tfcom.Main_ID, tfcom.Main_PWD, tfcom.Main_CENTER);
                    smsg = "QuoteCom: [" + encoding.GetString(msg) + "] MyIP=" + quotecom.MyIP;
                    AddInfo(smsg);
                    break;
                case COM_STATUS.CONNECT_FAIL:
                    smsg = encoding.GetString(msg);
                    AddInfo("CONNECT_FAIL:" + smsg);
                    break;
                case COM_STATUS.DISCONNECTED:
                    smsg = encoding.GetString(msg);
                    AddInfo("DISCONNECTED:" + smsg);
                    break;
                case COM_STATUS.SUBSCRIBE:
                    smsg = encoding.GetString(msg, 0, msg.Length - 1);
                    AddInfo(String.Format("SUBSCRIBE:[{0}]", smsg));
                    //txtQuoteList.AppendText(String.Format("SUBSCRIBE:[{0}]", smsg));  //2012.02.16 LYNN TEMPORARY ; 
                    break;
                case COM_STATUS.UNSUBSCRIBE:
                    smsg = encoding.GetString(msg, 0, msg.Length - 1);
                    AddInfo(String.Format("UNSUBSCRIBE:[{0}]", smsg));
                    break;
                case COM_STATUS.ACK_REQUESTID:
                    long RequestId = BitConverter.ToInt64(msg, 0);
                    byte status = msg[8];
                    AddInfo("Request Id BACK: " + RequestId + " Status=" + status);
                    break;
                case COM_STATUS.RECOVER_DATA:
                    smsg = encoding.GetString(msg, 1, msg.Length - 1);
                    if (!RecoverMap.ContainsKey(smsg))
                        RecoverMap.Add(smsg, 0);

                    if (msg[0] == 0)
                    {
                        RecoverMap[smsg] = 0;
                        AddInfo(String.Format("開始回補 Topic:[{0}]", smsg));
                    }

                    if (msg[0] == 1)
                    {
                        AddInfo(String.Format("結束回補 Topic:[{0} 筆數:{1}]", smsg, RecoverMap[smsg]));
                    }
                    break;
            }
            com.Processed();
        }

        private  void OnRecoverStatus(object sender, string Topic, RECOVER_STATUS status, uint RecoverCount)
        {

            QuoteCom com = (QuoteCom)sender;
            switch (status)
            {
                case RECOVER_STATUS.RS_DONE:        //回補資料結束
                    AddInfo(String.Format("結束回補 Topic:[{0}]{1}", Topic, RecoverCount));
                    break;
                case RECOVER_STATUS.RS_BEGIN:       //開始回補資料
                    AddInfo(String.Format("開始回補 Topic:[{0}]", Topic));
                    break;
            }
        }

        private  void OnQuoteRcvMessage(object sender, PackageBase package)
        {
            if (package.TOPIC != null)
                if (RecoverMap.ContainsKey(package.TOPIC))
                    RecoverMap[package.TOPIC]++;


            StringBuilder sb;

            switch (package.DT)
            {
                case (ushort)DT.LOGIN:
                    P001503 _p001503 = (P001503)package;
                    if (_p001503.Code == 0)
                    {
                        AddInfo("可註冊檔數：" + _p001503.Qnum);
                        if (quotecom.QuoteFuture) AddInfo("可註冊期貨報價");
                        if (quotecom.QuoteStock) AddInfo("可註冊證券報價");
                    }
                    break;

                case (ushort)DT.QUOTE_STOCK_MATCH1:   //上市成交
                case (ushort)DT.QUOTE_STOCK_MATCH2:   //上櫃成交
                    PI31001 pi31001 = (PI31001)package;
                    //if (!cbShow.Checked) break;
                    //sb = new StringBuilder(Environment.NewLine);
                    //sb.Append((package.DT == (ushort)DT.QUOTE_STOCK_MATCH1) ? "上市 " : "上櫃 ");
                    //if (pi31001.Status == 0) sb.Append("<試撮>");
                    //sb.Append("商品代號: ").Append(pi31001.StockNo).Append("  更新時間: ").Append(pi31001.Match_Time).Append(Environment.NewLine);
                    //sb.Append(" 成交價: ").Append(pi31001.Match_Price).Append("  單量: ").Append(pi31001.Match_Qty);
                    //sb.Append(" 總量: ").Append(pi31001.Total_Qty).Append("  來源: ").Append(pi31001.Source).Append(Environment.NewLine);
                    //sb.Append("=========================================");
                    //AddInfo(sb.ToString());

                    if (pi31001.Status == 0)
                    {
                        //var newLine = string.Format("{0},{1},{2},{3},{4},{5}", pi31001.StockNo, pi31001.Match_Time, pi31001.Match_Price, pi31001.Match_Qty, pi31001.Total_Qty, "<試撮>");
                        //AddInfo(newLine);
                    }
                    else
                    {
                        var newLine = string.Format("{0},{1},{2},{3},{4}", pi31001.StockNo, pi31001.Match_Time, pi31001.Match_Price, pi31001.Match_Qty, pi31001.Total_Qty);
                        //是否輸出到log視窗
                        //AddInfo(newLine);
                        //紀錄成交明細至檔案
                        String filename = string.Format("{0}_{1}_{2}.csv", "MATCH", pi31001.StockNo, DateTime.Now.ToString("yyyyMMdd"));
                        WriteToCSV(filename, newLine);
                        //更新GridView成交明細
                        UpdateGridViewMatchPrice(pi31001);


                    }

                    break;

                case (ushort)DT.QUOTE_STOCK_DEPTH1: //上市五檔
                case (ushort)DT.QUOTE_STOCK_DEPTH2: //上櫃五檔
                    PI31002 pi31002 = (PI31002)package;
                    //if (!cbShow.Checked) break;
                    //sb = new StringBuilder(Environment.NewLine);
                    //sb.Append((package.DT == (ushort)DT.QUOTE_STOCK_DEPTH1) ? "上市 " : "上櫃 ");
                    //if (i31002.Status == 0) sb.Append("<試撮> ");
                    //sb.Append("商品代號: ").Append(i31002.StockNo).Append(" 更新時間: ").Append(i31002.Match_Time).Append("  來源: ").Append(i31002.Source).Append(Environment.NewLine);
                    //for (int i = 0; i < 5; i++)
                    //    sb.Append(String.Format("五檔[{0}] 買[價:{1:N} 量:{2:N}]    賣[價:{3:N} 量:{4:N}]", i + 1, i31002.BUY_DEPTH[i].PRICE, i31002.BUY_DEPTH[i].QUANTITY, i31002.SELL_DEPTH[i].PRICE, i31002.SELL_DEPTH[i].QUANTITY)).Append(Environment.NewLine);


                    //sb.Append("=========================================");

                    //AddInfo(sb.ToString());
                    if (pi31002.Status == 0)
                    {
                        //StringBuilder buy_price = new StringBuilder();
                        //StringBuilder buy_qty = new StringBuilder();
                        //StringBuilder sell_price = new StringBuilder();
                        //StringBuilder sell_qty = new StringBuilder();
                        //for (int i = 0; i < 5; i++)
                        //{
                        //    buy_price.Append(pi31002.BUY_DEPTH[i].PRICE).Append("_");
                        //    buy_qty.Append(pi31002.BUY_DEPTH[i].QUANTITY).Append("_");
                        //    sell_price.Append(pi31002.SELL_DEPTH[i].PRICE).Append("_");
                        //    sell_qty.Append(pi31002.SELL_DEPTH[i].QUANTITY).Append("_");
                        //}
                        //var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", pi31002.StockNo, pi31002.Match_Time, buy_price.ToString(), buy_qty.ToString(), sell_price.ToString(), sell_qty.ToString(), "<試撮>");
                        //AddInfo(newLine);
                    }
                    else
                    {
                        StringBuilder buy_price = new StringBuilder();
                        StringBuilder buy_qty = new StringBuilder();
                        StringBuilder sell_price = new StringBuilder();
                        StringBuilder sell_qty = new StringBuilder();
                        for (int i = 0; i < 5; i++)
                        {
                            buy_price.Append(pi31002.BUY_DEPTH[i].PRICE).Append("_");
                            buy_qty.Append(pi31002.BUY_DEPTH[i].QUANTITY).Append("_");
                            sell_price.Append(pi31002.SELL_DEPTH[i].PRICE).Append("_");
                            sell_qty.Append(pi31002.SELL_DEPTH[i].QUANTITY).Append("_");
                        }
                        var newLine = string.Format("{0},{1},{2},{3},{4},{5}", pi31002.StockNo, pi31002.Match_Time, buy_price.ToString(), buy_qty.ToString(), sell_price.ToString(), sell_qty.ToString());
                        //AddInfo(newLine);
                        String filename = string.Format("{0}_{1}_{2}.csv", "DEPTH", pi31002.StockNo, DateTime.Now.ToString("yyyyMMdd"));
                        WriteToCSV(filename, newLine);
                    }
                    break;
                case (ushort)DT.QUOTE_LAST_PRICE_STOCK:
                    PI30026 pi30026 = (PI30026)package;
                    //sb = new StringBuilder(Environment.NewLine);
                    //sb.Append("商品代號:").Append(pi30026.StockNo).Append(" 最後價格:").Append(pi30026.LastMatchPrice).Append(Environment.NewLine);
                    //sb.Append("當日最高成交價格:").Append(pi30026.DayHighPrice).Append(" 當日最低成交價格:").Append(pi30026.DayLowPrice);
                    //sb.Append("開盤價:").Append(pi30026.FirstMatchPrice).Append(" 開盤量:").Append(pi30026.FirstMatchQty).Append(Environment.NewLine);
                    //sb.Append("參考價:").Append(pi30026.ReferencePrice).Append(Environment.NewLine);
                    //sb.Append("成交單量:").Append(pi30026.LastMatchQty).Append(Environment.NewLine);
                    //sb.Append("成交總量:").Append(pi30026.TotalMatchQty).Append(Environment.NewLine);
                    //for (int i = 0; i < 5; i++)
                    //    sb.Append(String.Format("五檔[{0}] 買[價:{1:N} 量:{2:N}]    賣[價:{3:N} 量:{4:N}]", i + 1, pi30026.BUY_DEPTH[i].PRICE, pi30026.BUY_DEPTH[i].QUANTITY, pi30026.SELL_DEPTH[i].PRICE, pi30026.SELL_DEPTH[i].QUANTITY)).Append(Environment.NewLine);
                    //sb.Append("==============================================");
                    //AddInfo(sb.ToString());
                    var lastprice = string.Format("{0},{1},{2},{3},{4}", pi30026.StockNo, 0, pi30026.ReferencePrice, 0, 0);
                    AddInfo(lastprice);
                    String targetfile = string.Format("{0}_{1}_{2}.csv", "MATCH", pi30026.StockNo, DateTime.Now.ToString("yyyyMMdd"));


                    break;
                case (ushort)DT.QUOTE_STOCK_INDEX1:  //上市指數
                    PI31011 pi31011 = (PI31011)package;
                    sb = new StringBuilder(Environment.NewLine);
                    sb.Append("[上市指數]更新時間：").Append(pi31011.Match_Time).Append("   筆數: ").Append(pi31011.COUNT).Append(Environment.NewLine);
                    for (int i = 0; i < pi31011.COUNT; i++)
                        sb.Append(" [" + (i + 1) + "] ").Append(pi31011.IDX[i].VALUE);
                    sb.Append("==============================================");
                    AddInfo(sb.ToString());
                    break;
                case (ushort)DT.QUOTE_STOCK_INDEX2:  //上櫃指數
                    PI31011 pi32011 = (PI31011)package;
                    sb = new StringBuilder(Environment.NewLine);
                    sb.Append("[上櫃指數]更新時間：").Append(pi32011.Match_Time).Append("   筆數: ").Append(pi32011.COUNT).Append(Environment.NewLine);
                    for (int i = 0; i < pi32011.COUNT; i++)
                        sb.Append(" [" + (i + 1) + "]").Append(pi32011.IDX[i].VALUE);
                    sb.Append("==============================================");
                    AddInfo(sb.ToString());
                    break;
                case (ushort)DT.QUOTE_STOCK_NEWINDEX1:  //上市新編指數
                    PI31021 pi31021 = (PI31021)package;
                    sb = new StringBuilder(Environment.NewLine);
                    sb.Append("上市新編指數[").Append(pi31021.IndexNo).Append("] 時間:").Append(pi31021.IndexTime);
                    sb.Append("指數:  ").Append(pi31021.LatestIndex).Append(Environment.NewLine);
                    AddInfo(sb.ToString());
                    break;
                case (ushort)DT.QUOTE_STOCK_NEWINDEX2:  //上櫃新編指數
                    PI31021 pi32021 = (PI31021)package;
                    sb = new StringBuilder(Environment.NewLine);
                    sb.Append("上櫃新編指數[").Append(pi32021.IndexNo).Append("] 時間:").Append(pi32021.IndexTime);
                    sb.Append("最新指數: ").Append(pi32021.LatestIndex).Append(Environment.NewLine);
                    AddInfo(sb.ToString());
                    break;
                case (ushort)DT.QUOTE_LAST_INDEX1:  //上市最新指數查詢
                    PI31026 pi31026 = (PI31026)package;
                    sb = new StringBuilder(Environment.NewLine);
                    sb.Append("  最新上市指數  筆數: ").Append(pi31026.COUNT).Append(Environment.NewLine);
                    for (int i = 0; i < pi31026.COUNT; i++)
                    {
                        sb.Append(" [" + (i + 1) + "] ").Append(" 昨日收盤指數:").Append(pi31026.IDX[i].RefIndex);
                        sb.Append(" 開盤指數:").Append(pi31026.IDX[i].FirstIndex).Append(" 最新指數:").Append(pi31026.IDX[i].LastIndex);
                        sb.Append(" 最高指數:").Append(pi31026.IDX[i].DayHighIndex).Append(" 最低指數:").Append(pi31026.IDX[i].DayLowIndex).Append(Environment.NewLine);
                        sb.Append("==============================================");
                    }
                    AddInfo(sb.ToString());
                    break;
                case (ushort)DT.QUOTE_LAST_INDEX2:  //上櫃最新指數查詢
                    PI31026 pi32026 = (PI31026)package;
                    sb = new StringBuilder(Environment.NewLine);
                    sb.Append("  最新上櫃指數  筆數: ").Append(pi32026.COUNT).Append(Environment.NewLine);
                    for (int i = 0; i < pi32026.COUNT; i++)
                    {
                        sb.Append(" [" + (i + 1) + "] ").Append(" 昨日收盤指數:").Append(pi32026.IDX[i].RefIndex);
                        sb.Append(" 開盤指數:").Append(pi32026.IDX[i].FirstIndex).Append(" 最新指數:").Append(pi32026.IDX[i].LastIndex);
                        sb.Append(" 最高指數:").Append(pi32026.IDX[i].DayHighIndex).Append(" 最低指數:").Append(pi32026.IDX[i].DayLowIndex).Append(Environment.NewLine);
                        sb.Append("==============================================");
                    }
                    AddInfo(sb.ToString());
                    break;
                case (ushort)DT.QUOTE_STOCK_AVGINDEX:  //加權平均指數 2014.8.6 ADD ; 
                    PI31022 pi31022 = (PI31022)package;
                    sb = new StringBuilder(Environment.NewLine);
                    sb.Append("加權平均指數[").Append(pi31022.IndexNo).Append("] 時間:").Append(pi31022.IndexTime);
                    sb.Append("最新指數: ").Append(pi31022.LatestIndex).Append(Environment.NewLine);
                    AddInfo(sb.ToString());
                    break;
            }
        }

        private  void OntfcomGetStatus(object sender, COM_STATUS staus, byte[] msg)
        {
            TaiFexCom com = (TaiFexCom)sender;
            string smsg = null;
            switch (staus)
            {
                case COM_STATUS.LOGIN_READY:          //登入成功
                    AddInfo("登入成功:" + com.Accounts);
                    break;
                case COM_STATUS.LOGIN_FAIL:             //登入失敗
                    AddInfo(String.Format("登入失敗:[{0}]", encoding.GetString(msg)));
                    break;
                case COM_STATUS.LOGIN_UNKNOW:       //登入狀態不明
                    AddInfo(String.Format("登入狀態不明:[{0}]", encoding.GetString(msg)));
                    break;
                case COM_STATUS.CONNECT_READY:      //連線成功
                    smsg = "伺服器" + tfcom.ServerHost + ":" + tfcom.ServerPort + Environment.NewLine +
                              "伺服器回應: [" + encoding.GetString(msg) + "]" + Environment.NewLine +
                              "本身為" + ((tfcom.isInternal) ? "內" : "外") + "部 IP:" + tfcom.MyIP;
                    AddInfo(smsg);
                    break;
                case COM_STATUS.CONNECT_FAIL:       //連線失敗
                    smsg = encoding.GetString(msg);
                    AddInfo("連線失敗:" + smsg + " " + tfcom.ServerHost + ":" + tfcom.ServerPort);
                    break;
                case COM_STATUS.DISCONNECTED:       //斷線
                    smsg = encoding.GetString(msg);
                    AddInfo("斷線:" + smsg);
                    break;
                case COM_STATUS.AS400_CONNECTED:
                    AddInfo("AS400 連線成功:" + encoding.GetString(msg));
                    break;
                case COM_STATUS.AS400_CONNECTFAIL:
                    AddInfo("AS400 連線失敗:" + encoding.GetString(msg));
                    break;
                case COM_STATUS.AS400_DISCONNECTED:
                    AddInfo("AS400 連線斷線:" + encoding.GetString(msg));
                    break;
                case COM_STATUS.SUBSCRIBE:
                    com.WriterLog("msg.Length=" + msg.Length);
                    smsg = encoding.GetString(msg);
                    com.WriterLog(String.Format("註冊:[{0}]", smsg));
                    AddInfo(String.Format("註冊:[{0}]", smsg));
                    break;
                case COM_STATUS.UNSUBSCRIBE:
                    smsg = encoding.GetString(msg);
                    AddInfo(String.Format("取消註冊:[{0}]", smsg));
                    break;
                case COM_STATUS.ACK_REQUESTID:          //下單或改單第一次回覆
                    long RequestId = BitConverter.ToInt64(msg, 0);
                    byte status = msg[8];
                    //***TEST AddInfo("序號回覆: " + RequestId + " 狀態=" + ((status == 1) ? "收單" : "失敗"));
                    break;
            }

        }

        private  void OntfcomRcvMessage(object sender, PackageBase package)
        {

            StringBuilder sbtmp = new StringBuilder();
            switch ((DT)package.DT)
            {
                case DT.LOGIN:
                    P001503 p1503 = (P001503)package;
                    if (p1503.Code != 0)
                        AddInfo("登入失敗 CODE = " + p1503.Code + " " + tfcom.GetMessageMap(p1503.Code));
                    else
                    {
                        if (p1503.p001503_2.Length > 0)
                        {
                            brokerid = p1503.p001503_2[0].BrokeId;
                            account = p1503.p001503_2[0].Account;
                        }
                        AddInfo("登入成功 ");
                    }
                    break;
                case DT.SECU_ALLOWANCE_RPT:   //子帳額度控管: 回補
                    PT05002 p5002 = (PT05002)package;
                    AddInfo(p5002.ToLog());
                    break;
                case DT.SECU_ALLOWANCE:
                    PT05003 p5003 = (PT05003)package;
                    AddInfo(p5003.ToLog());
                    break;
                #region 證券下單回報
                case DT.SECU_ORDER_ACK:   //下單第二回覆

                    PT04002 p4002 = (PT04002)package;
                    AddInfo(p4002.ToLog() + "訊息:" + tfcom.GetMessageMap(p4002.ErrorCode));
                    break;
                case DT.SECU_ORDER_RPT: //委託回報

                    PT04010 p4010 = (PT04010)package;
                    AddInfo("RCV 4010 [" + p4010.CNT + "," + p4010.OrderNo + "]");
                    // "委託型態", "分公司代號", "帳號", "綜合帳戶", "營業員代碼",                                                                                     "委託書號",            "交易日期",             "回報時間",           "委託日期時間",            "商品代號", "下單序號",             "委託來源別",                     "市場別",                        "買賣",                    "委託別",                         "委託種類",                      "委託價格",    "改量前數量",              "改量後數量",    "錯誤代碼", "錯誤訊息" 
                    string[] row4010 = { p4010.OrderFunc.ToString(), p4010.BrokerId, p4010.Account, p4010.SubAccount, p4010.OmniAccount, p4010.AgentId, p4010.OrderNo, p4010.TradeDate, p4010.ReportTime, p4010.ClientOrderTime, p4010.StockID, p4010.CNT, p4010.Channel.ToString(), p4010.Market.ToString(), p4010.Side.ToString(), p4010.OrdLot.ToString(), p4010.OrdClass.ToString(), p4010.Price, p4010.BeforeQty, p4010.AfterQty, p4010.ErrCode, p4010.ErrMsg };
                    AddInfo(string.Join(",", row4010));
                    break;
                case DT.SECU_DEAL_RPT:   //成交回報

                    PT04011 p4011 = (PT04011)package;
                    AddInfo("RCV 4011 [" + p4011.CNT + "]");
                    //                            "委託型態",                             "分公司代號", "帳號",              "綜合帳戶", "營業員代碼",                  "委託書號",              "交易日期",               "回報時間", "電子單號",     "來源別",                          "市場別",                         "商品代碼",            "買賣別",             "委託別",                            ",委託種類",                          "成交價格", "成交數量", "市場成交序號" 
                    string[] row4011 = { p4011.OrderFunc.ToString(), p4011.BrokerId, p4011.Account, p4011.SubAccount, p4011.OmniAccount, p4011.AgentId, p4011.OrderNo, p4011.TradeDate, p4011.ReportTime, p4011.CNT, p4011.Channel.ToString(), p4011.Market.ToString(), p4011.StockID, p4011.Side.ToString(), p4011.OrdLot.ToString(), p4011.OrdClass.ToString(), p4011.Price, p4011.DealQty, p4011.MarketNo };
                    AddInfo(string.Join(",", row4011));
                    break;
                    #endregion

            }


        }
        private  void OntfcomRcvServerTime(Object sender, DateTime serverTime, int ConnQuality)
        {
            if (this.InvokeRequired)
            {
                Smart.OnRcvServerTime_EventHandler d = new Smart.OnRcvServerTime_EventHandler(OntfcomRcvServerTime);
                this.Invoke(d, new object[] { sender, serverTime, ConnQuality });
                return;
            }
            tb_ServerTime.Text = String.Format("{0:yyyy/MM/dd hh:mm:ss.fff}", serverTime);
            tb_HeartBeats.Text = ConnQuality.ToString();
            
            //AddInfo(String.Format("{0:hh:mm:ss.fff}", serverTime));
            //AddInfo("[" + ConnQuality + "]");
        }

        private  void OntfcomRecoverStatus(object sender, string Topic, RECOVER_STATUS status, uint RecoverCount)
        {

            TaiFexCom com = (TaiFexCom)sender;
            switch (status)
            {
                case RECOVER_STATUS.RS_DONE:        //回補資料結束
                    if (RecoverCount == 0)
                        AddInfo(String.Format("結束回補 Topic:[{0}]", Topic));
                    else AddInfo(String.Format("結束回補 Topic:[{0} 筆數:{1}]", Topic, RecoverCount));
                    break;
                case RECOVER_STATUS.RS_BEGIN:       //開始回補資料
                    AddInfo(String.Format("開始回補 Topic:[{0}]", Topic));
                    break;
            }
        }
        #endregion

        private void AddInfo(string msg)
        {
            if (this.tb_Log.InvokeRequired)
            {
                SetAddInfoCallBack d = new SetAddInfoCallBack(AddInfo);
                this.Invoke(d, new object[] { msg });
            }
            else
            {
                string fMsg = String.Format("[{0}] {1} {2}", DateTime.Now.ToString("hh:mm:ss:ffff"), msg, Environment.NewLine);
                try
                {
                    tb_Log.AppendText(fMsg);
                }
                catch { };
            }
        }
        private void WriteToCSV(String filename, String msg)
        {
            string combined = Path.Combine(this.DownloadTarget, filename);
            using (StreamWriter w = File.AppendText(combined))
            {
                w.WriteLine(msg);
            }
        }

        private void UpdateGridViewMatchPrice(PI31001 pi31001) {
            DataRow StockRow = StockTable.Rows.Find(pi31001.StockNo);
            if (StockRow != null)
            {
                StockRow["MatchTime"] = pi31001.Match_Time;
                StockRow["MatchPrice"] = pi31001.Match_Price;
                StockRow["MatchQty"] = pi31001.Match_Qty;
                StockRow["TotalQty"] = pi31001.Total_Qty;
                if (pi31001.Match_Qty== pi31001.Total_Qty)
                    StockRow["Open"] = pi31001.Match_Price;
            }
            else
            {
                //MessageBox.Show("A row with the primary key of " + pi31001.StockNo + " could not be found");
            }
        }
        private void TradeBotStatusChanges(object sender, TradeStatus tradestatus, string msg)
        {
            TradeBotBase tb = (TradeBotBase)sender;
            DataRow StockRow = StockTable.Rows.Find(tb.stockid);
            if (StockRow != null)
            {
                StockRow["Status"] = tradestatus;
            }
                AddInfo("Status:" + tradestatus + ", Message:" + msg);
            //Console.WriteLine("Message:" + msg);
        }
        private void btn_Login_Click(object sender, EventArgs e)
        {
            host = this.cb_Host.Text;
            port = Convert.ToUInt16(this.tb_Port.Text);
            id = this.tb_UserID.Text;
            pwd = this.tb_UserPWD.Text;

            //登入下單系統
            tfcom.LoginDirect(host, port, id, pwd, ' ');
            tfcom.AutoSubReportSecurity = true;
            tfcom.AutoRecoverReport = true;

            //登入行情下載系統
            quotecom.Connect2Quote(host, port, id, pwd, area, "");
        }

        private void btn_AddStock_Click(object sender, EventArgs e)
        {
            string stockid = tb_StockID.Text;
            ushort buyqty = (ushort)nud_BuyQty.Value;
            decimal stoplossratio = (decimal)nud_stoplossratio.Value;
            decimal lockgainprice = (decimal)nud_LockGainPrice.Value;
            int AmountThreshold = (Int32)nud_AmountThreshold.Value;
            BuyMode buymode = cb_BuyMode.SelectedIndex == 0 ? BuyMode.Auto : BuyMode.Notify;
            StopLossMode stoplossmode = cb_StopLossMode.SelectedIndex == 0 ? StopLossMode.Auto : StopLossMode.Manual;
            LockGainMode lockgainmode = cb_LockGainMode.SelectedIndex == 0 ? LockGainMode.Auto : LockGainMode.Manual; 
            DataRow row = StockTable.NewRow();
            row["StockID"] = stockid;
            row["Qty"] = buyqty;

            quotecom.SubQuotesDepth(stockid);
            quotecom.SubQuotesMatch(stockid);
            
            TradeBotBase tb = new TradeBotQA(stockid, brokerid, account, buyqty, quotecom, tfcom, stoplossratio, lockgainprice, AmountThreshold,buymode,stoplossmode,lockgainmode);
            tb.StatusChange += TradeBotStatusChanges;
            tb.Start();
            row["Status"] = tb.trade_status;
            row["ClosePrice"] = tb.ClosePrice;
            row["AH"] = tb.CDP_AH;
            row["NH"] = tb.CDP_NH;
            row["NL"] = tb.CDP_NL;
            row["AL"] = tb.CDP_AL;
            row["TradeBot"] = tb;

            StockTable.Rows.Add(row);
        }

        private void dgv_StockList_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu ctxmenu = new ContextMenu();
                if (e.RowIndex >= 0)
                {
                    String stockid = dgv_StockList.Rows[e.RowIndex].Cells[0].Value.ToString();
                    ctxmenu.MenuItems.Add(new MenuItem(stockid));
                    ctxmenu.MenuItems.Add(new MenuItem("買進",new EventHandler((menuitem,eventArgs) => {
                        DataRow row = StockTable.Rows.Find(stockid);
                        TradeStatus ts = (TradeStatus)row["Status"];
                        if (ts == TradeStatus.WaitingBuy || ts == TradeStatus.WaitingBuySignal)
                        {
                            TradeBotBase tb = (TradeBotBase)row["TradeBot"];
                            tb.BuyStock();
                        }
                        else {
                            MessageBox.Show("目前狀態無法買進");
                        }
                        
                    })));
                    ctxmenu.MenuItems.Add(new MenuItem("賣出", new EventHandler((menuitem, eventArgs) => {
                        DataRow row = StockTable.Rows.Find(stockid);
                        TradeStatus ts = (TradeStatus)row["Status"];
                        if (ts == TradeStatus.WaitingSell || ts == TradeStatus.WaitingSellSignal)
                        {
                            TradeBotBase tb = (TradeBotBase)row["TradeBot"];
                            tb.SellStock();
                        }
                        else {
                            MessageBox.Show("目前狀態無法賣出");
                        }
                            
                    })));
                    ctxmenu.MenuItems.Add(new MenuItem("開啟五檔明細"));
                    ctxmenu.MenuItems.Add(new MenuItem("暫停"));
                    ctxmenu.MenuItems.Add(new MenuItem("刪除"));
                    ctxmenu.Show(this, dgv_StockList.PointToClient(Cursor.Position));
                }
                
            }
        }


        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
