using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intelligence;
using Package;
using Smart;

namespace TradeBot
{
    class Program
    {
        public static Intelligence.QuoteCom quotecom;
        public static TaiFexCom tfcom;
        public static Dictionary<string, int> RecoverMap = new Dictionary<string, int>();
        public static UTF8Encoding encoding = new System.Text.UTF8Encoding();
        public const String host = "211.20.186.12";
        public const int port = 443;
        public const String SID = "API";
        public const String Token = "b6eb";
        public const String id = "H220566876";
        public const String pwd = "0000";
        public const char area = ' ';
        private string group = "";
        public static string brokerid = "";
        public static string account = "";


        static void Main(string[] args)
        {
            quotecom = new Intelligence.QuoteCom(host, port, SID, Token); //接收證券行情物件
            quotecom.SourceId = SID;
            quotecom.OnRcvMessage += OnQuoteRcvMessage;
            quotecom.OnGetStatus += OnQuoteGetStatus;
            quotecom.OnRecoverStatus += OnRecoverStatus;


            tfcom = new Smart.TaiFexCom(host,port,SID);// 券證下單物件
           
            tfcom.OnRcvMessage += OntfcomRcvMessage;          //資料接收事件
            tfcom.OnGetStatus += OntfcomGetStatus;               //狀態通知事件
            //tfcom.OnRcvServerTime += OntfcomRcvServerTime;   //接收主機時間
            //tfcom.OnRecoverStatus += OntfcomRecoverStatus;   //回補狀態通知
            tfcom.LoginDirect(host,port,id,pwd,' ');
            tfcom.AutoSubReportSecurity = true;
            tfcom.AutoRecoverReport = true;
            Console.ReadLine();
            quotecom.Connect2Quote(host, port, id, pwd, area, "");
            Console.ReadLine();
            quotecom.SubQuotesDepth("6223");
            quotecom.SubQuotesMatch("6223");
            Console.ReadLine();
            TradeBotBase tb = new TradeBotQA("6223", brokerid, account, 1, quotecom, tfcom,1.5,109,100,BuyMode.Auto,StopLossMode.Auto,LockGainMode.Auto);
            tb.StatusChange += ShowChanges;
            tb.Start();
            //AddInfo(tfcom.Accounts);
            //Console.WriteLine(brokerid);
            //Console.WriteLine(account);
            Console.ReadLine();
            tb.BuyStock();
            Console.ReadLine();
            tb.SellStock();
            //quotecom.SubQuotesDepth("2317");
            //quotecom.SubQuotesMatch("2317");
            //Console.ReadLine();
            //TradeBot tb2 = new TradeBot("2317", brokerid, account, 1, quotecom, tfcom,1.5);
            //tb2.StatusChange += ShowChanges;
            //tb2.Start();
           




        }
        static void ShowChanges(object sender,TradeStatus tradestatus,string msg) {
            Console.WriteLine("Status:"+tradestatus);
            Console.WriteLine("Message:" + msg);
        }
        static void AddInfo(string msg) {
            Console.WriteLine(msg);
        }

        static void OnQuoteGetStatus(object sender, COM_STATUS staus, byte[] msg)
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

        public static void OnRecoverStatus(object sender, string Topic, RECOVER_STATUS status, uint RecoverCount)
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

        public static void OnQuoteRcvMessage(object sender, PackageBase package)
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
                        var newLine = string.Format("{0},{1},{2},{3},{4},{5}", pi31001.StockNo, pi31001.Match_Time, pi31001.Match_Price, pi31001.Match_Qty, pi31001.Total_Qty, "<試撮>");
                        AddInfo(newLine);
                    }
                    else
                    {
                        var newLine = string.Format("{0},{1},{2},{3},{4}", pi31001.StockNo, pi31001.Match_Time, pi31001.Match_Price, pi31001.Match_Qty, pi31001.Total_Qty);
                        AddInfo(newLine);
                        String filename = string.Format("{0}_{1}_{2}.csv", "MATCH", pi31001.StockNo, DateTime.Now.ToString("yyyyMMdd"));
                       
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
                        var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", pi31002.StockNo, pi31002.Match_Time, buy_price.ToString(), buy_qty.ToString(), sell_price.ToString(), sell_qty.ToString(), "<試撮>");
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

        public static void OntfcomGetStatus(object sender, COM_STATUS staus, byte[] msg)
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

        public static void OntfcomRcvMessage(object sender, PackageBase package)
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
        public static void OntfcomRcvServerTime(Object sender, DateTime serverTime, int ConnQuality)
        {
            
            //labelServerTime.Text = String.Format("{0:yyyy/MM/dd hh:mm:ss.fff}", serverTime);
            AddInfo(String.Format("{0:hh:mm:ss.fff}", serverTime));
            AddInfo("[" + ConnQuality + "]");
        }

        public static void OntfcomRecoverStatus(object sender, string Topic, RECOVER_STATUS status, uint RecoverCount)
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
    }
}
