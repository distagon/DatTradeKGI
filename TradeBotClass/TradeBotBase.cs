using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intelligence;
using Package;
using System.Data.SQLite;
using System.IO;

namespace TradeBot
{

    public enum TradeStatus
    {
        StandBy, //待命中
        WaitingBuySignal, //等待買入訊號
        WaitingBuy,  //等待買入
        ConfirmBuyOrder, //確認委托買單是否成功
        ConfirmBuyMatch, //確認買單是否成交
        WaitingSellSignal, //等待賣出訊號
        WaitingSell, //等待賣出
        ConfirmSellOrder, //確認委托賣單是否成功
        ConfirmSellMatch, //確認賣單是否成功且全部成交，紀錄成交價格
        Stop, //中止流程
        Error //錯誤
    };

    public enum BuyMode
    {
        Auto, //出現訊號，自動下單
        Notify, //出現訊號，只提醒，不下單
    };

    public enum StopLossMode
    {
        Auto, //固定停損點，觸發下單賣出
        Manual, //手動停損，觸發下單賣出
    };

    public enum LockGainMode
    {
        Auto, //自動停利，到達預設停利點，下單賣出
        Manual, //手動停利，人工下單賣出
    };

    public struct DealRpt //成交回報明細
    {
        public decimal Price;
        public ushort Qty;

        public DealRpt(decimal price, ushort qty)
        {
            Price = price;
            Qty = qty;
        }
    }

    public delegate void StatusChangeEventHandler(object sender, TradeStatus status, string msg);
    public delegate void FieldValueChangeEventHandler(object sender, String FieldName, object value);

    public abstract class TradeBotBase
    {
        public event StatusChangeEventHandler StatusChange;
        public event FieldValueChangeEventHandler FieldValueChange;
        protected Intelligence.QuoteCom quotecom; //接收證券行情物件
        protected Smart.TaiFexCom taifexcom;// 券證下單物件
        public TradeStatus trade_status; //交易現況
        public String stockid; //股票代號
        protected String brokerid; //分行
        protected String account;  //帳號
        protected decimal currentBuyMatchPrice = 0.0m; //買入成交金額平均
        protected decimal currentSellMatchPrice = 0.0m; //賣出成交金額平均
        public ushort BuyQty; //買進量
        public ushort SellQty; //賣出量
        protected long BuyRequestId; //下單封包序號
        protected string CNT; //電子單號
        protected ushort DealCheck = 0; //成交筆數檢查
        public BuyMode buy_mode = BuyMode.Notify; //買入模式
        public StopLossMode stoplossmode = StopLossMode.Manual; //停損模式
        public LockGainMode lockgainmode = LockGainMode.Manual; //停利模式
        protected List<DealRpt> DealList = new List<DealRpt>(); //成交回報暫存，因為成交可能不會全部一次回報完成
        //protected decimal StopLossRatio = 0.0m; //停損百分比
        //protected decimal LockGainPrice = 0.0m; //停利目標價
        public int AmountThreshold = 0; //第5分鐘該股成交量必須達多少才準備買進
        //CDP
        public decimal OpenPrice = 0.0m; //今日開盤價
        public decimal ClosePrice = 0.0m; //昨日收盤價
        public decimal RaiseStopPrice = 0.0m; //今日漲停價
        public decimal FallStopPrice = 0.0m;  //今日跌停價
        public decimal CDP_AH, CDP_NH, CDP_NL, CDP_AL;

        //成交明細與五檔暫存
        protected OrderedDictionary MatchLog = new OrderedDictionary(3);
        protected OrderedDictionary DepthLog = new OrderedDictionary(3);

        //五分鐘後第一筆紀錄
        protected PI31001 pi31001_5min;
        protected PI31002 pi31002_5min;

        //買入價格與方式
        protected Security_PriceFlag priceflag = Security_PriceFlag.SP_RiseStopPrice;
        protected decimal BuyPrice = 0.0m;

        //五分鐘內內盤最高價是否有向上突破CDP
        protected Boolean ExceedCDP = false;

        //賣出偵測是否突破成本價
        protected Boolean ExceedCost = false;

        //綠單次數紀錄
        protected int GreenMatchCount;

        //買入賣出成交紀錄目錄
        protected string MatchLogFolder = "";
        
        public TradeBotBase(String stockid, string brokerid, string account, ushort BuyQty, Intelligence.QuoteCom quotecom, Smart.TaiFexCom taifexcom, int amountthreshold, BuyMode buymode, StopLossMode stoplossmode, LockGainMode lockgainmode)
        {
            this.stockid = stockid;
            this.quotecom = quotecom;
            this.taifexcom = taifexcom;
            this.brokerid = brokerid;
            this.account = account;
            this.BuyQty = BuyQty;
            //this.StopLossRatio = stoplossratio;
            //this.LockGainPrice = lockgainprice;
            this.AmountThreshold = amountthreshold;
            this.buy_mode = buymode;
            this.stoplossmode = stoplossmode;
            this.lockgainmode = lockgainmode;
       
            this.trade_status = TradeStatus.StandBy;
            OnStatusChange(this.trade_status, stockid + ":待命中");

            //初始化成交明細下載目錄
            this.MatchLogFolder = Path.Combine(Directory.GetCurrentDirectory(), DateTime.Now.ToString("yyyyMMdd"));
            //建立目錄
            if (!Directory.Exists(this.MatchLogFolder))
            {
                Directory.CreateDirectory(this.MatchLogFolder);
            }

        }

        public decimal CurrentBuyMatchPrice { get => currentBuyMatchPrice; }
        public decimal CurrentSellMatchPrice { get => currentSellMatchPrice; }

        //紀錄買入成交時間價位與賣出原因時間價位
        protected void MatchLoger(String msg)
        {
            string combined = Path.Combine(this.MatchLogFolder, this.stockid + ".txt");
            using (StreamWriter w = File.AppendText(combined))
            {
                string fMsg = String.Format("[{0}] {1}", DateTime.Now.ToString("hh:mm:ss:ffff"), msg);
                w.WriteLine(fMsg);
            }
        }

        //開始執行
        public void Start()
        {
            //清空資料
            currentBuyMatchPrice = 0.0m;
            currentSellMatchPrice = 0.0m;
            DealCheck = 0;
            DealList.Clear();
            MatchLog.Clear();
            DepthLog.Clear();
            pi31001_5min = null;
            pi31002_5min = null;
            ExceedCDP = false;
            ExceedCost = false;
            GreenMatchCount = 0;

            //連sqlite 讀CDP
            if (!getStockInfo())
            {
                this.trade_status = TradeStatus.Error;
                OnStatusChange(this.trade_status, stockid + ":股價資料讀取錯誤");
            }
            else
            {
                //如果開始偵測時間已超過9點，則須去主機下載今日開般價
                int CurrentTime = Convert.ToInt32(DateTime.Now.ToString("HHmmss"));
                if (CurrentTime > 90000)
                    GetOpenPrice(stockid);
                quotecom.OnRcvMessage += WaitingBuy;
                this.trade_status = TradeStatus.WaitingBuySignal;
                OnStatusChange(this.trade_status, stockid + ":等待買入訊號");

            }
        }

        //手動中止偵測
        public void Stop() {
            if (this.trade_status == TradeStatus.WaitingBuy || this.trade_status == TradeStatus.WaitingBuySignal) {
                quotecom.OnRcvMessage -= WaitingBuy;
                this.trade_status = TradeStatus.Stop;
                OnStatusChange(this.trade_status, stockid + ":中止偵測");
            }
            
        }


        //等待買入時中止偵測
        protected void BreakTrade(string msg) {
            quotecom.OnRcvMessage -= WaitingBuy;
            this.trade_status = TradeStatus.Stop;
            OnStatusChange(this.trade_status, stockid + ":條件不符，中止買入偵測:" + msg);
        }



        public virtual void BuyStock()
        {
            if (this.trade_status == TradeStatus.WaitingBuySignal || this.trade_status == TradeStatus.WaitingBuy)
            {
                BuyRequestId = taifexcom.GetRequestId();
                taifexcom.SecurityOrder(BuyRequestId, Security_OrdType.OT_NEW, Security_Lot.Even_Lot, Security_Class.SC_Ordinary, brokerid, account, stockid, SIDE_FLAG.SF_BUY, BuyQty, 0.0m, Security_PriceFlag.SP_RiseStopPrice, "", "", "");
                this.trade_status = TradeStatus.ConfirmBuyOrder;
                quotecom.OnRcvMessage -= WaitingBuy;
                taifexcom.OnRcvMessage += ConfirmOrder;
                OnStatusChange(this.trade_status, stockid + ":下單買入");
            }
            else {
                OnStatusChange(this.trade_status, stockid + ":狀態不符，無法買入");
            }


        }

        public virtual void SellStock()
        {
            if (this.trade_status == TradeStatus.WaitingSell || this.trade_status == TradeStatus.WaitingSellSignal)
            {
                BuyRequestId = taifexcom.GetRequestId();
                taifexcom.SecurityOrder(BuyRequestId, Security_OrdType.OT_NEW, Security_Lot.Even_Lot, Security_Class.SC_Ordinary, brokerid, account, stockid, SIDE_FLAG.SF_SELL, BuyQty, 0.0m, Security_PriceFlag.SP_FallStopPrice, "", "", "");
                this.trade_status = TradeStatus.ConfirmSellOrder;
                quotecom.OnRcvMessage -= WaitingSell;
                taifexcom.OnRcvMessage += ConfirmOrder;
                OnStatusChange(this.trade_status, stockid + ":下單賣出");
            }
            else
            {
                OnStatusChange(this.trade_status, stockid + ":狀態不符，無法賣出");
            }

        }

        public void GetOpenPrice(String idlist)
        {
            quotecom.OnRcvMessage += UpdateOpenPrice;
            short istatus = quotecom.RetriveLastPriceStock(idlist);
            if (istatus < 0)
                OnStatusChange(this.trade_status, stockid + ":" + quotecom.GetSubQuoteMsg(istatus));
        }

        public void UpdateOpenPrice(object sender, PackageBase package) {
            if (package.DT == (ushort)DT.QUOTE_LAST_PRICE_STOCK) {
                PI30026 pi30026 = (PI30026)package;
                if (pi30026.StockNo == this.stockid) {
                    OpenPrice = pi30026.FirstMatchPrice;
                    OnFieldValueChange("OpenPrice", OpenPrice);
                }
            }
        }

        protected decimal CalRaiseStopPrice(decimal closeprice) {

            decimal raisestopprice = closeprice * 1.1m;
            if (closeprice < 10)
            {
                return Math.Floor(raisestopprice * 100m) / 100m;
            }
            else if (closeprice >= 10 && closeprice < 50)
            {

                if (Math.Floor(raisestopprice * 100m) % 10 >= 5)
                    return (Math.Floor(raisestopprice * 10m) / 10m) + 0.05m;
                else
                    return Math.Floor(raisestopprice * 10m) / 10m;
                
            }
            else if (closeprice >= 50 && closeprice < 100)
            {
                return Math.Floor(raisestopprice * 10m) / 10m;
            }
            else if (closeprice >= 100 && closeprice < 500)
            {
                if (Math.Floor(raisestopprice * 10m) % 10 >= 5)
                    return Math.Floor(raisestopprice) + 0.5m;
                else
                    return Math.Floor(raisestopprice);
            }
            else if (closeprice >= 500 && closeprice < 1000)
            {
                return Math.Floor(raisestopprice);
            }
            else
            {
                if (Math.Floor(raisestopprice) % 10 >= 5)
                    return Math.Floor(raisestopprice / 10m) * 10m + 5.0m;
                else
                    return Math.Floor(raisestopprice / 10m) * 10m;
            }
        }

        protected decimal CalFallStopPrice(decimal closeprice)
        {
            decimal fallstopprice = closeprice * 0.9m;
            if (closeprice < 10)
            {
                return Math.Ceiling(fallstopprice * 100m) / 100m;
            }
            else if (closeprice >= 10 && closeprice < 50)
            {
                decimal modevalue = Math.Ceiling(fallstopprice * 100m) % 10;
                if (modevalue > 5)
                    return (Math.Floor(fallstopprice * 10m) / 10m) + 0.1m;
                else if (modevalue < 5 && modevalue > 0)
                    return (Math.Floor(fallstopprice * 10m) / 10m) + 0.05m;
                else
                    return Math.Ceiling(fallstopprice * 100m) / 100m;
            }
            else if (closeprice >= 50 && closeprice < 100)
            {
                return Math.Ceiling(fallstopprice * 10m) / 10m;
            }
            else if (closeprice >= 100 && closeprice < 500)
            {
                decimal modevalue = Math.Ceiling(fallstopprice * 10m) % 10;
                if (modevalue > 5)
                    return Math.Floor(fallstopprice) + 1.0m;
                else if (modevalue < 5 && modevalue > 0)
                    return Math.Floor(fallstopprice) + 0.5m;
                else
                    return Math.Ceiling(fallstopprice * 10m) / 10m;
            }
            else if (closeprice >= 500 && closeprice < 1000)
            {
                return Math.Ceiling(fallstopprice);
            }
            else
            {
                decimal modevalue = Math.Ceiling(fallstopprice) % 10;
                if (modevalue > 5)
                    return Math.Floor(fallstopprice / 10m) * 10m + 10m;
                else if (modevalue < 5 && modevalue > 0)
                    return Math.Floor(fallstopprice / 10m) * 10m + 5m;
                else
                    return Math.Ceiling(fallstopprice);
            }
        }

        private Boolean getStockInfo() {
            //DateTime dt = DateTime.Now;
            Boolean cdp_status = false;
            using (SQLiteConnection sqlite_conn = new SQLiteConnection("Data source=stock.db")) {
                
                sqlite_conn.Open();
                // 要下任何命令先取得該連結的執行命令物件
                SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();
                //sqlite_cmd.CommandText = "select * from StockDaily where TradeDate="+ String.Format("{0:yyyyMMdd}", dt) +" and ID='" +stockid+"' ";
                sqlite_cmd.CommandText = "select * from StockDaily where ID='" + stockid + "' order by TradeDate desc limit 1";
                SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();
                while (sqlite_datareader.Read())
                {
                    // Print out the content of the text field:
                    ClosePrice =Convert.ToDecimal(sqlite_datareader["Close"]);
                    CDP_AH= Convert.ToDecimal(sqlite_datareader["AH"]);
                    CDP_NH = Convert.ToDecimal(sqlite_datareader["NH"]);
                    CDP_NL = Convert.ToDecimal(sqlite_datareader["NL"]);
                    CDP_AL = Convert.ToDecimal(sqlite_datareader["AL"]);
                    //計算漲跌停價
                    RaiseStopPrice = CalRaiseStopPrice(ClosePrice);
                    FallStopPrice = CalFallStopPrice(ClosePrice);

                    //Console.WriteLine(ClosePrice);
                    //Console.WriteLine(CDP_AH);
                    //Console.WriteLine(CDP_NH);
                    //Console.WriteLine(CDP_NL);
                    //Console.WriteLine(CDP_AL);
                    cdp_status = true;


                }
                sqlite_conn.Close();
            }
                
            
            return cdp_status;

        }
        protected decimal AdjustPrice(decimal price) {
            if (price < 10)
            {
                return price + 0.01m;
            }
            else if (price >= 10 && price < 50)
            {
                return price + 0.05m;
            }
            else if (price >= 50 && price < 100)
            {
                return price + 0.1m;
            }
            else if (price >= 100 && price < 500)
            {
                return price + 0.5m;
            }
            else if (price >= 500 && price < 1000)
            {
                return price + 1.0m;
            }
            else {
                return price + 5.0m;
            }
        }

        protected decimal TickInfo(decimal price) {
            if (price < 10)
            {
                return 0.01m;
            }
            else if (price >= 10 && price < 50)
            {
                return 0.05m;
            }
            else if (price >= 50 && price < 100)
            {
                return 0.1m;
            }
            else if (price >= 100 && price < 500)
            {
                return 0.5m;
            }
            else if (price >= 500 && price < 1000)
            {
                return 1.0m;
            }
            else
            {
                return 5.0m;
            }
        }

        public virtual void WaitingBuy(object sender, PackageBase package)
        {

            //Console.WriteLine("Buy");

            String UpdateType = "";  //是更新成交或五檔明細
            String MatchType = ""; //成交類別，是紅單或綠單 R or G
            String StockID = ""; //股票代號
            //讀取成交明細，寫入暫存物件
            if (package.DT == (ushort)DT.QUOTE_STOCK_MATCH1 || package.DT == (ushort)DT.QUOTE_STOCK_MATCH2)
            {
                PI31001 pi31001 = (PI31001)package;
                if (pi31001.Status != 0)
                {
                    if (pi31001.StockNo == this.stockid)
                    {
                        StockID = pi31001.StockNo;
                        UpdateType = "Match";
                        //取得開盤價
                        if (pi31001.Match_Qty == pi31001.Total_Qty)
                            OpenPrice = pi31001.Match_Price;

                        //判斷是紅單或綠單
                        if (DepthLog.Count >= 1) {
                            PI31002 depth = (PI31002)DepthLog[0];
                            if (pi31001.Match_Price <= depth.BUY_DEPTH[0].PRICE)
                                MatchType = "G";
                            else
                                MatchType = "R";
                        }
                        //寫入暫存物件
                        if (MatchLog.Count > 2)
                        {
                            MatchLog.RemoveAt(2);
                            MatchLog.Insert(0, pi31001.Match_Time, pi31001);
                        }
                        else
                            MatchLog.Insert(0, pi31001.Match_Time, pi31001);
                    }
                }
            }

            //讀取五檔明細，寫入暫存物件
            if (package.DT == (ushort)DT.QUOTE_STOCK_DEPTH1 || package.DT == (ushort)DT.QUOTE_STOCK_DEPTH2)
            {
                PI31002 pi31002 = (PI31002)package;
                if (pi31002.Status != 0)
                {
                    if (pi31002.StockNo == this.stockid)
                    {
                        StockID = pi31002.StockNo;
                        UpdateType = "Depth";
                        if (DepthLog.Count > 2)
                        {
                            DepthLog.RemoveAt(2);
                            DepthLog.Insert(0, pi31002.Match_Time, pi31002);
                        }
                        else
                            DepthLog.Insert(0, pi31002.Match_Time, pi31002);
                    }
                }
            }

            //撰寫買入標準判斷
            if (StockID==this.stockid && (UpdateType == "Match" || UpdateType == "Depth") && MatchLog.Count>=1 && DepthLog.Count>=1) {
                if (BuySignal(UpdateType,MatchType) == true)
                {
                    if (buy_mode == BuyMode.Auto)
                    {
                        BuyStock();
                    }
                    else if (buy_mode == BuyMode.Notify)
                    {
                        this.trade_status = TradeStatus.WaitingBuy;
                        //quotecom.OnRcvMessage -= WaitingBuy;
                        OnStatusChange(this.trade_status, stockid + ":訊號出現可下單");
                    }
                }
            }
            
        }

        public virtual void WaitingSell(object sender, PackageBase package)
        {
            //Console.WriteLine("Sell");

            String UpdateType = "";  //是更新成交或五檔明細
            String MatchType = ""; //成交類別，是紅單或綠單 R or G
            //讀取成交明細，寫入暫存物件
            if (package.DT == (ushort)DT.QUOTE_STOCK_MATCH1 || package.DT == (ushort)DT.QUOTE_STOCK_MATCH2)
            {
                PI31001 pi31001 = (PI31001)package;
                if (pi31001.Status != 0)
                {
                    if (pi31001.StockNo == this.stockid)
                    {
                        UpdateType = "Match";
                        //取得開盤價
                        if (pi31001.Match_Qty == pi31001.Total_Qty)
                            OpenPrice = pi31001.Match_Price;

                        //判斷是紅單或綠單
                        if (DepthLog.Count >= 1)
                        {
                            PI31002 depth = (PI31002)DepthLog[0];
                            if (pi31001.Match_Price <= depth.BUY_DEPTH[0].PRICE)
                                MatchType = "G";
                            else
                                MatchType = "R";
                        }
                        //寫入暫存物件
                        if (MatchLog.Count > 2)
                        {
                            MatchLog.RemoveAt(2);
                            MatchLog.Insert(0, pi31001.Match_Time, pi31001);
                        }
                        else
                            MatchLog.Insert(0, pi31001.Match_Time, pi31001);
                    }
                }
            }

            //讀取五檔明細，寫入暫存物件
            if (package.DT == (ushort)DT.QUOTE_STOCK_DEPTH1 || package.DT == (ushort)DT.QUOTE_STOCK_DEPTH2)
            {
                PI31002 pi31002 = (PI31002)package;
                if (pi31002.Status != 0)
                {
                    if (pi31002.StockNo == this.stockid)
                    {
                        UpdateType = "Depth";
                        if (DepthLog.Count > 2)
                        {
                            DepthLog.RemoveAt(2);
                            DepthLog.Insert(0, pi31002.Match_Time, pi31002);
                        }
                        else
                            DepthLog.Insert(0, pi31002.Match_Time, pi31002);
                    }
                }
            }

            //撰寫停損賣出標準
            if (UpdateType == "Match" || UpdateType == "Depth")
            {
                if (SellSignal(UpdateType, MatchType) == true)
                {
                    //if (stoplossmode == StopLossMode.Auto)
                        SellStock();
                    //else
                    //{
                    //    this.trade_status = TradeStatus.WaitingSell;
                        //quotecom.OnRcvMessage -= WaitingSell;
                    //    OnStatusChange(this.trade_status, stockid + ":已達停損標準可以賣出");
                    //}

                }
            }
        }

        protected virtual Boolean  BuySignal(String UpdateType,String MatchType) {

            return true;
        }

        protected virtual Boolean SellSignal(String UpdateType, String MatchType)
        {

            return true;
        }

        public void ConfirmOrder(object sender, PackageBase package)
        {
            switch ((DT)package.DT)
            {
                case DT.SECU_ORDER_ACK://下單回覆
                    PT04002 p4002 = (PT04002)package;
                    //Console.WriteLine(p4002.RequestId);
                    //Console.WriteLine(BuyRequestId);
                    if (p4002.RequestId == BuyRequestId)
                    {

                        if (p4002.ErrorCode == 0)//下單成功
                        {
                            CNT = p4002.CNT;
                            OnStatusChange(this.trade_status, stockid + ":下單成功");

                        }
                        else
                        {//下單失敗
                            this.trade_status = TradeStatus.Error;
                            taifexcom.OnRcvMessage -= ConfirmOrder;
                            OnStatusChange(this.trade_status, p4002.ToLog() + "訊息:" + taifexcom.GetMessageMap(p4002.ErrorCode));
                        }
                    }
                    break;
                case DT.SECU_ORDER_RPT: //委託回報
                    PT04010 p4010 = (PT04010)package;
                    if (p4010.StockID == stockid && p4010.CNT == CNT)
                    {
                        if (p4010.Side == 'B')
                        {
                            this.trade_status = TradeStatus.ConfirmBuyMatch;
                            taifexcom.OnRcvMessage -= ConfirmOrder;
                            taifexcom.OnRcvMessage += ConfirmBuyMatch;
                            OnStatusChange(this.trade_status, stockid + ":買入委托成功");
                        }
                        else if (p4010.Side == 'S')
                        {
                            this.trade_status = TradeStatus.ConfirmSellOrder;
                            taifexcom.OnRcvMessage -= ConfirmOrder;
                            taifexcom.OnRcvMessage += ConfirmSellMatch;
                            OnStatusChange(this.trade_status, stockid + ":賣出委托成功");
                        }

                    }
                    break;
            }
        }

        
        //計算平均成交價
        private decimal CalAvgMatchPrice(List<DealRpt> list)
        {
            decimal weight = 0.0m;
            ushort count = 0;
            foreach (DealRpt deal in list)
            {
                weight += deal.Price * deal.Qty;
                count += deal.Qty;
            }
            return weight / count;
        }

        public void ConfirmBuyMatch(object sender, PackageBase package)
        {
            Console.WriteLine("ConfirmBuyMatch");

            if (package.DT == (ushort)DT.SECU_DEAL_RPT)
            {
                PT04011 p4011 = (PT04011)package;
                if (p4011.StockID == stockid && p4011.CNT == CNT)
                {
                    DealList.Clear();
                    DealCheck = 0;
                    DealList.Add(new DealRpt(Convert.ToDecimal(p4011.Price), Convert.ToUInt16(p4011.DealQty)));
                    DealCheck += Convert.ToUInt16(p4011.DealQty);
                    if (DealCheck == BuyQty)
                    {
                        currentBuyMatchPrice = CalAvgMatchPrice(DealList);
                        this.trade_status = TradeStatus.WaitingSellSignal;
                        taifexcom.OnRcvMessage -= ConfirmBuyMatch;
                        quotecom.OnRcvMessage += WaitingSell;
                        OnStatusChange(this.trade_status, stockid + ":買入委托成交");
                    }

                }
            }

        }

        public void ConfirmSellMatch(object sender, PackageBase package)
        {
            Console.WriteLine("ConfirmSellMatch");

            if (package.DT == (ushort)DT.SECU_DEAL_RPT)
            {
                PT04011 p4011 = (PT04011)package;
                if (p4011.StockID == stockid && p4011.CNT == CNT)
                {
                    DealList.Clear();
                    DealCheck = 0;
                    DealList.Add(new DealRpt(Convert.ToDecimal(p4011.Price), Convert.ToUInt16(p4011.DealQty)));
                    DealCheck += Convert.ToUInt16(p4011.DealQty);
                    if (DealCheck == BuyQty)
                    {
                        currentSellMatchPrice = CalAvgMatchPrice(DealList);
                        this.trade_status = TradeStatus.StandBy;
                        taifexcom.OnRcvMessage -= ConfirmSellMatch;
                        OnStatusChange(this.trade_status, stockid + ":賣出成交，當沖成功");
                    }

                }
            }

        }

        protected void OnStatusChange(TradeStatus status, string msg)
        {
            StatusChange?.Invoke(this, status, msg);
        }

        protected void OnFieldValueChange(String FieldName, object value)
        {
            FieldValueChange?.Invoke(this, FieldName, value);
        }
    }

}

