using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intelligence;
using Package;
using System.Data.SQLite;

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
        public double Price;
        public ushort Qty;

        public DealRpt(double price, ushort qty)
        {
            Price = price;
            Qty = qty;
        }
    }

    public delegate void StatusChangeEventHandler(object sender, TradeStatus status, string msg);

    abstract class  TradeBotBase
    {
        public event StatusChangeEventHandler StatusChange;
        private Intelligence.QuoteCom quotecom; //接收證券行情物件
        private Smart.TaiFexCom taifexcom;// 券證下單物件
        public TradeStatus trade_status; //交易現況
        private String stockid; //股票代號
        private String brokerid; //分行
        private String account;  //帳號
        private double currentBuyMatchPrice = 0.0; //買入成交金額平均
        private double currentSellMatchPrice = 0.0; //賣出成交金額平均
        private ushort BuyQty; //買量
        private long BuyRequestId; //下單封包序號
        private string CNT; //電子單號
        private ushort DealCheck = 0; //成交筆數檢查
        public BuyMode buy_mode = BuyMode.Notify; //買入模式
        public StopLossMode stoplossmode = StopLossMode.Manual; //停損模式
        public LockGainMode lockgainmode = LockGainMode.Manual; //停利模式
        private List<DealRpt> DealList = new List<DealRpt>(); //成交回報暫存，因為成交可能不會全部一次回報完成
        private double StopLossRatio = 0.0; //停損百分比
        //CDP
        public double OpenPrice; //今日開盤價
        public double ClosePrice; //昨日收盤價
        public double CDP_AH, CDP_NH, CDP_NL, CDP_AL;
        public TradeBotBase(String stockid, string brokerid, string account, ushort BuyQty, Intelligence.QuoteCom quotecom, Smart.TaiFexCom taifexcom, double stoplossratio)
        {
            this.stockid = stockid;
            this.quotecom = quotecom;
            this.taifexcom = taifexcom;
            this.brokerid = brokerid;
            this.account = account;
            this.BuyQty = BuyQty;
            this.StopLossRatio = stoplossratio;
            

        }

        public double CurrentBuyMatchPrice { get => currentBuyMatchPrice; }

        public void Start()
        {
            //連sqlite 讀CDP
            if (!getCDP())
            {
                this.trade_status = TradeStatus.Error;
                OnStatusChange(this.trade_status, stockid + ":CDP讀取錯誤");
            }
            else {
                quotecom.OnRcvMessage += WaitingBuy;
                this.trade_status = TradeStatus.WaitingBuySignal;
                OnStatusChange(this.trade_status, stockid + ":等待買入訊號");
            }
            
        }
        protected void BreakTrade() {
            quotecom.OnRcvMessage -= WaitingBuy;
            this.trade_status = TradeStatus.Stop;
            OnStatusChange(this.trade_status, stockid + ":條件不符，中止流程");
        }

        public virtual void BuyStock()
        {
            BuyRequestId = taifexcom.GetRequestId();
            taifexcom.SecurityOrder(BuyRequestId, Security_OrdType.OT_NEW, Security_Lot.Even_Lot, Security_Class.SC_Ordinary, brokerid, account, stockid, SIDE_FLAG.SF_BUY, BuyQty, 0.0m, Security_PriceFlag.SP_RiseStopPrice, "", "", "");
            this.trade_status = TradeStatus.ConfirmBuyOrder;
            quotecom.OnRcvMessage -= WaitingBuy;
            taifexcom.OnRcvMessage += ConfirmOrder;
            OnStatusChange(this.trade_status, stockid + ":下單買入");
        }

        private Boolean getCDP() {
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
                    ClosePrice =Convert.ToDouble(sqlite_datareader["Close"]);
                    CDP_AH= Convert.ToDouble(sqlite_datareader["AH"]);
                    CDP_NH = Convert.ToDouble(sqlite_datareader["NH"]);
                    CDP_NL = Convert.ToDouble(sqlite_datareader["NL"]);
                    CDP_AL = Convert.ToDouble(sqlite_datareader["AL"]);
                    
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
        protected double AdjustPrice(double price) {
            if (price < 10)
            {
                return price + 0.01;
            }
            else if (price >= 10 && price < 50)
            {
                return price + 0.05;
            }
            else if (price >= 50 && price < 100)
            {
                return price + 0.1;
            }
            else if (price >= 100 && price < 500)
            {
                return price + 0.5;
            }
            else if (price >= 500 && price < 1000)
            {
                return price + 1.0;
            }
            else {
                return price + 5.0;
            }
        }

        public virtual void WaitingBuy(object sender, PackageBase package)
        {

            //Console.WriteLine("Buy");
            if (package.DT == (ushort)DT.QUOTE_STOCK_MATCH1 || package.DT == (ushort)DT.QUOTE_STOCK_MATCH2)
            {
                PI31001 pi31001 = (PI31001)package;
                if (pi31001.Status != 0)
                {
                    if (pi31001.StockNo == this.stockid)
                    {
                        //取得開盤價
                        if (pi31001.Match_Qty == pi31001.Total_Qty)
                            OpenPrice = (double)pi31001.Match_Price;
                        //撰寫買入標準
                        if (BuySignal(pi31001) == true) {
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

            }

        }

        protected virtual Boolean  BuySignal(PI31001 pi31001) {

            return true;
        }

        protected virtual Boolean SellSignal(PI31001 pi31001)
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

        

        private double CalAvgMatchPrice(List<DealRpt> list)
        {
            double weight = 0.0;
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
                    DealList.Add(new DealRpt(Convert.ToDouble(p4011.Price), Convert.ToUInt16(p4011.DealQty)));
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

        public virtual void SellStock()
        {
            if (this.trade_status == TradeStatus.WaitingSell)
            {
                BuyRequestId = taifexcom.GetRequestId();
                taifexcom.SecurityOrder(BuyRequestId, Security_OrdType.OT_NEW, Security_Lot.Even_Lot, Security_Class.SC_Ordinary, brokerid, account, stockid, SIDE_FLAG.SF_SELL, BuyQty, 0.0m, Security_PriceFlag.SP_FallStopPrice, "", "", "");
                this.trade_status = TradeStatus.ConfirmSellOrder;
                quotecom.OnRcvMessage -= WaitingSell;
                taifexcom.OnRcvMessage += ConfirmOrder;
                OnStatusChange(this.trade_status, stockid + ":下單賣出");
            }
            else {
                OnStatusChange(this.trade_status, stockid + ":尚未買進，無法賣出");
            }
            
        }

        public virtual void WaitingSell(object sender, PackageBase package)
        {
            Console.WriteLine("Sell");

            if (package.DT == (ushort)DT.QUOTE_STOCK_MATCH1 || package.DT == (ushort)DT.QUOTE_STOCK_MATCH2)
            {
                PI31001 pi31001 = (PI31001)package;
                if (pi31001.Status != 0)
                {
                    if (pi31001.StockNo == this.stockid)
                    {
                        //撰寫停損賣出標準
                        if (SellSignal(pi31001) == true) {
                            if (stoplossmode == StopLossMode.Auto)
                                SellStock();
                            else {
                                this.trade_status = TradeStatus.WaitingSell;
                                //quotecom.OnRcvMessage -= WaitingSell;
                                OnStatusChange(this.trade_status, stockid + ":已達停損標準可以賣出");
                            }

                        }
                            
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
                    DealList.Add(new DealRpt(Convert.ToDouble(p4011.Price), Convert.ToUInt16(p4011.DealQty)));
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
    }

}

