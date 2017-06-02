using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intelligence;
using Package;

namespace TradeBot
{
    
    class TradeBot
    {
        public event StatusChangeEventHandler StatusChange;
        private Intelligence.QuoteCom quotecom; //接收證券行情物件
        private Smart.TaiFexCom taifexcom;// 券證下單物件
        public TradeStatus trade_status; //交易現況
        private String stockid;
        private String brokerid;
        private String account;
        private double currentBuyMatchPrice=0.0; //買入成交金額平均
        private double currentSellMatchPrice = 0.0; //賣出成交金額平均
        private ushort BuyQty; //買量
        private long BuyRequestId; //下單封包序號
        private string CNT; //電子單號
        private ushort DealCheck=0;
        public BuyMode buy_mode=BuyMode.Auto; //買入模式
        public StopLossMode stoplossmode = StopLossMode.Manual;
        public LockGainMode lockgainmode = LockGainMode.Manual;
        private List<DealRpt> DealList = new List<DealRpt>(); //成交回報暫存，因為成交可能不會全部一次回報完成
        private double StopLossRatio = 0.0; //停損百分比

        public TradeBot(String stockid,string brokerid,string account, ushort BuyQty,Intelligence.QuoteCom quotecom, Smart.TaiFexCom taifexcom,double stoplossratio) {
            this.stockid = stockid;
            this.quotecom = quotecom;
            this.taifexcom = taifexcom;
            this.brokerid = brokerid;
            this.account = account;
            this.BuyQty = BuyQty;
            this.StopLossRatio = stoplossratio;

        }

        public double CurrentBuyMatchPrice { get => currentBuyMatchPrice; }

        public void Start() {
            quotecom.OnRcvMessage += WaitingBuy;
            this.trade_status = TradeStatus.WaitingBuySignal;
            OnStatusChange(this.trade_status, stockid+":等待買入訊號");
        }
        public void BuyStock() {
            BuyRequestId = taifexcom.GetRequestId();
            taifexcom.SecurityOrder(BuyRequestId, Security_OrdType.OT_NEW, Security_Lot.Even_Lot, Security_Class.SC_Ordinary, brokerid, account, stockid, SIDE_FLAG.SF_BUY, BuyQty, 0.0m, Security_PriceFlag.SP_RiseStopPrice, "", "", "");
            this.trade_status = TradeStatus.ConfirmBuyOrder;
            quotecom.OnRcvMessage -= WaitingBuy;
            taifexcom.OnRcvMessage += ConfirmOrder;
            OnStatusChange(this.trade_status, stockid + ":下單買入");
        }
               

        public void WaitingBuy(object sender, PackageBase package) {

            Console.WriteLine("Buy");
            if (package.DT == (ushort)DT.QUOTE_STOCK_MATCH1 || package.DT == (ushort)DT.QUOTE_STOCK_MATCH2)
            {
                PI31001 pi31001 = (PI31001)package;
                if (pi31001.Status != 0) {
                    if (pi31001.StockNo == this.stockid)
                    {
                        //撰寫買入標準
                        if (buy_mode == BuyMode.Auto)
                        {
                            BuyStock(); 
                        }
                        else if (buy_mode == BuyMode.Notify) {
                            this.trade_status = TradeStatus.WaitingBuy;
                            quotecom.OnRcvMessage -= WaitingBuy;
                            OnStatusChange(this.trade_status, stockid + ":訊號出現可下單");
                        }
                        
                    }
                }
                
            }
            
        }

        public void ConfirmOrder(object sender, PackageBase package) {
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
                            taifexcom.OnRcvMessage -= ConfirmBuyOrder;
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
                        else if (p4010.Side == 'S') {
                            this.trade_status = TradeStatus.ConfirmSellOrder;
                            taifexcom.OnRcvMessage -= ConfirmOrder;
                            taifexcom.OnRcvMessage += ConfirmSellMatch;
                            OnStatusChange(this.trade_status, stockid + ":賣出委托成功");
                        }
                        
                    }
                    break;
            }
        }

        public void ConfirmBuyOrder(object sender, PackageBase package) {
            Console.WriteLine("ConfirmBuyOrder");
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
                            OnStatusChange(this.trade_status, stockid + ":買入下單成功");

                        }
                        else
                        {//下單失敗
                            this.trade_status = TradeStatus.Error;
                            taifexcom.OnRcvMessage -= ConfirmBuyOrder;
                            OnStatusChange(this.trade_status, p4002.ToLog() + "訊息:" + taifexcom.GetMessageMap(p4002.ErrorCode));
                        }
                    }
                    break;
                case DT.SECU_ORDER_RPT: //委託回報
                    PT04010 p4010 = (PT04010)package;
                    if (p4010.StockID == stockid && p4010.CNT == CNT) {
                        
                        this.trade_status = TradeStatus.ConfirmBuyMatch;
                        taifexcom.OnRcvMessage -= ConfirmBuyOrder;
                        taifexcom.OnRcvMessage += ConfirmBuyMatch;
                        OnStatusChange(this.trade_status, stockid+ ":買入委托成功");
                    }
                    break;
            }
            
            
        }

        private double CalAvgMatchPrice(List<DealRpt> list)
        {
            double weight=0.0;
            ushort count=0;
            foreach (DealRpt deal in list) {
                weight += deal.Price * deal.Qty;
                count += deal.Qty;
            }
            return weight / count;
        }

        public void ConfirmBuyMatch(object sender, PackageBase package) {
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
                    if (DealCheck == BuyQty) {
                        currentBuyMatchPrice = CalAvgMatchPrice(DealList);
                        this.trade_status = TradeStatus.WaitingSell;
                        taifexcom.OnRcvMessage -= ConfirmBuyMatch;
                        quotecom.OnRcvMessage += WaitingSell;
                        OnStatusChange(this.trade_status, stockid + ":買入委托成交");
                    }
                    
                }
            }
            
        }

        public void SellStock() {
            BuyRequestId = taifexcom.GetRequestId();
            taifexcom.SecurityOrder(BuyRequestId, Security_OrdType.OT_NEW, Security_Lot.Even_Lot, Security_Class.SC_Ordinary, brokerid, account, stockid, SIDE_FLAG.SF_SELL, BuyQty, 0.0m, Security_PriceFlag.SP_FallStopPrice, "", "", "");
            this.trade_status = TradeStatus.ConfirmSellOrder;
            quotecom.OnRcvMessage -= WaitingSell;
            taifexcom.OnRcvMessage += ConfirmSellOrder;
            OnStatusChange(this.trade_status, stockid + ":下單賣出");
        }

        public void WaitingSell(object sender, PackageBase package) {
            Console.WriteLine("Sell");

            if (package.DT == (ushort)DT.QUOTE_STOCK_MATCH1 || package.DT == (ushort)DT.QUOTE_STOCK_MATCH2)
            {
                PI31001 pi31001 = (PI31001)package;
                if (pi31001.Status != 0)
                {
                    if (pi31001.StockNo == this.stockid)
                    {

                        //撰寫停損賣出標準
                        SellStock();


                    }
                }
            }
            

        }
        public void ConfirmSellOrder(object sender, PackageBase package) {
            Console.WriteLine("ConfirmSellOrder");

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
                            OnStatusChange(this.trade_status, stockid + ":賣出下單成功");

                        }
                        else
                        {//下單失敗
                            this.trade_status = TradeStatus.Error;
                            taifexcom.OnRcvMessage -= ConfirmBuyOrder;
                            OnStatusChange(this.trade_status, p4002.ToLog() + "訊息:" + taifexcom.GetMessageMap(p4002.ErrorCode));
                        }
                    }
                    break;
                case DT.SECU_ORDER_RPT: //委託回報
                    PT04010 p4010 = (PT04010)package;
                    if (p4010.StockID == stockid && p4010.CNT == CNT)
                    {
                        this.trade_status = TradeStatus.ConfirmSellOrder;
                        taifexcom.OnRcvMessage -= ConfirmSellOrder;
                        taifexcom.OnRcvMessage += ConfirmSellMatch;
                        OnStatusChange(this.trade_status, stockid + ":賣出委托成功");
                    }
                    break;
            }

           
            
        }
        public void ConfirmSellMatch(object sender, PackageBase package) {
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
                        OnStatusChange(this.trade_status, stockid + ":當沖成功");
                    }

                }
            }

        }

        protected void OnStatusChange(TradeStatus status,string msg) {
            StatusChange?.Invoke(this, status,msg);
        }
    }
}
