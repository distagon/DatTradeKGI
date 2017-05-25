using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using Intelligence;
using Package;
using System.IO;


namespace DayTradeKGI
{
    class GetTrade
    {
        Intelligence.QuoteCom quoteCom;
        Dictionary<string, int> RecoverMap = new Dictionary<string, int>();
        private UTF8Encoding encoding = new System.Text.UTF8Encoding();
        public const String host = "211.20.186.12";
        public const int port =443;
        public const String SID = "API";
        public const String Token = "b6eb";
        public const String id = "H220566876";
        public const String pwd = "0000";
        public const char area = ' ';
        private string group = "";
        private string workspace = @"C:\daytradedownload\";
        private string downloadtarget = "";
        public string idlist = "";
        static void Main(string[] args)
        {
            GetTrade gt = new GetTrade(args[0]);
            string combined = Path.Combine(gt.workspace, args[0] + ".txt");
            if (File.Exists(combined))
            {   
                string grouplist = System.IO.File.ReadAllText(combined);
                gt.idlist = grouplist;
                gt.connect();
            }
            else {
                Environment.Exit(0);
                
            }
            
            //Console.ReadLine();
            
            //"1568|1589|2231|2338|2841|3018|3026|3144|3218|3317|3362|3406|3428|3661|3673|4912|4943|4968|5289|5529|6426|6531|8027|9802|";
            //gt.SubQuotesMath(idlist);
            //gt.SubQuotesDepth(idlist);
            //Console.ReadLine();
            //gt.GetLastPrice("2337");
            //Console.ReadLine();
            //gt.quoteCom.Logout();


        }

        GetTrade(String groupid) {
           quoteCom = new Intelligence.QuoteCom(host, port, SID, Token);
            quoteCom.OnRcvMessage += OnQuoteRcvMessage;
            quoteCom.OnGetStatus += OnQuoteGetStatus;
            quoteCom.OnRecoverStatus += OnRecoverStatus;
            quoteCom.QDebugLog = false;
            quoteCom.FQDebugLog = false;
            this.group = groupid;
            this.downloadtarget = this.workspace + DateTime.Now.ToString("yyyyMMdd");
            //建立目錄
            if (!Directory.Exists(this.downloadtarget))
            {
                Directory.CreateDirectory(this.downloadtarget);
            }
        }
      

        public void connect() {
            
            quoteCom.SourceId = SID;
            quoteCom.Connect2Quote(host, port, id, pwd, area, "");
        }

        private void SubQuotesMath(String idlist) {
            short istatus;
            istatus = quoteCom.SubQuotesMatch(idlist);
            if (istatus < 0)   //
                    AddInfo("成交:" + quoteCom.GetSubQuoteMsg(istatus));
         }

        private void SubQuotesDepth(String idlist)
        {
            short istatus;
            istatus = quoteCom.SubQuotesDepth(idlist);
            if (istatus < 0)   //
                AddInfo("五檔:" + quoteCom.GetSubQuoteMsg(istatus));
        }

        public void SubQuotesStock(String idlist)
        {
            
            quoteCom.SubQuotesStock(idlist);
            
        }

        public void GetLastPrice(String idlist) {
            short istatus = quoteCom.RetriveLastPriceStock(idlist);
            if (istatus < 0)   //
                AddInfo(quoteCom.GetSubQuoteMsg(istatus));
        }
        private void OnQuoteGetStatus(object sender, COM_STATUS staus, byte[] msg)
        {
            QuoteCom com = (QuoteCom)sender;
            string smsg = null;
            switch (staus)
            {
                case COM_STATUS.LOGIN_READY:
                    AddInfo(String.Format("LOGIN_READY:[{0}]", encoding.GetString(msg)));
                    GetLastPrice(this.idlist);
                    SubQuotesMath(this.idlist);
                    SubQuotesDepth(this.idlist);
                    break;
                case COM_STATUS.LOGIN_FAIL:
                    AddInfo(String.Format("LOGIN FAIL:[{0}]", encoding.GetString(msg)));
                    break;
                case COM_STATUS.LOGIN_UNKNOW:
                    AddInfo(String.Format("LOGIN UNKNOW:[{0}]", encoding.GetString(msg)));
                    break;
                case COM_STATUS.CONNECT_READY:
                    //quoteCom.Login(tfcom.Main_ID, tfcom.Main_PWD, tfcom.Main_CENTER);
                    smsg = "QuoteCom: [" + encoding.GetString(msg) + "] MyIP=" + quoteCom.MyIP;
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

        private void OnRecoverStatus(object sender, string Topic, RECOVER_STATUS status, uint RecoverCount)
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

        private void OnQuoteRcvMessage(object sender, PackageBase package)
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
                        if (quoteCom.QuoteFuture) AddInfo("可註冊期貨報價");
                        if (quoteCom.QuoteStock) AddInfo("可註冊證券報價");
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

                    if (pi31001.Status == 0) {
                       var newLine = string.Format("{0},{1},{2},{3},{4},{5}", pi31001.StockNo, pi31001.Match_Time, pi31001.Match_Price, pi31001.Match_Qty, pi31001.Total_Qty, "<試撮>");
                        AddInfo(newLine);
                    }
                    else {
                       var newLine = string.Format("{0},{1},{2},{3},{4}", pi31001.StockNo, pi31001.Match_Time, pi31001.Match_Price, pi31001.Match_Qty, pi31001.Total_Qty);
                        AddInfo(newLine);
                        String filename = string.Format("{0}_{1}_{2}.csv","MATCH",pi31001.StockNo, DateTime.Now.ToString("yyyyMMdd"));
                        WriteToCSV(filename,newLine);
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
                        for (int i = 0; i < 5; i++) {
                            buy_price.Append(pi31002.BUY_DEPTH[i].PRICE).Append("_");
                            buy_qty.Append(pi31002.BUY_DEPTH[i].QUANTITY).Append("_");
                            sell_price.Append(pi31002.SELL_DEPTH[i].PRICE).Append("_");
                            sell_qty.Append(pi31002.SELL_DEPTH[i].QUANTITY).Append("_");
                        }
                        var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6}", pi31002.StockNo, pi31002.Match_Time, buy_price.ToString(),buy_qty.ToString(),sell_price.ToString(),sell_qty.ToString() , "<試撮>");
                        AddInfo(newLine);
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
                        AddInfo(newLine);
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
                    var lastprice =  string.Format("{0},{1},{2},{3},{4}", pi30026.StockNo, 0, pi30026.ReferencePrice, 0, 0);
                    AddInfo(lastprice);
                    String targetfile = string.Format("{0}_{1}_{2}.csv", "MATCH", pi30026.StockNo, DateTime.Now.ToString("yyyyMMdd"));
                    WriteToCSV(targetfile, lastprice);

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

        private void AddInfo(string msg)
        {
            string combined = Path.Combine(this.workspace, string.Format("{0}_{1}_log.txt", DateTime.Now.ToString("yyyyMMdd"), this.group));
            Console.WriteLine(msg);
            //using (StreamWriter w = File.AppendText(combined))
            //{
            //    w.WriteLine(msg);
            //}

        }

        private void WriteToCSV(String filename,String msg) {
            string combined = Path.Combine(this.downloadtarget, filename);
            using (StreamWriter w = File.AppendText(combined))
            {
                w.WriteLine(msg);
            }
        }

    }
}
