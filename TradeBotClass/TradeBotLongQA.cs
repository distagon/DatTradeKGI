using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Intelligence;
using Smart;
using Package;

namespace TradeBot
{
    public class TradeBotLongQA : TradeBotLong
    {
        public TradeBotLongQA(string stockid, string brokerid, string account, ushort BuyQty, QuoteCom quotecom, TaiFexCom taifexcom, int amountthreshold, BuyMode buymode, StopLossMode stoplossmode, LockGainMode lockgainmode) : base(stockid, brokerid, account, BuyQty, quotecom, taifexcom, amountthreshold, buymode, stoplossmode, lockgainmode)
        {

        }

        public override void BuyStock()
        {
            if (this.trade_status == TradeStatus.WaitingBuySignal || this.trade_status == TradeStatus.WaitingBuy)
            {

                this.trade_status = TradeStatus.WaitingSellSignal;
                PI31002 pi31002 = (PI31002)DepthLog[0];
                decimal maxsellprice = pi31002.SELL_DEPTH[0].PRICE;
                MatchLoger("下單買入:" + pi31002.Match_Time.ToString() + " 買入價位可能為:" + maxsellprice.ToString());
                currentBuyMatchPrice = maxsellprice;
                quotecom.OnRcvMessage -= WaitingBuy;
                quotecom.OnRcvMessage += WaitingSell;
                OnStatusChange(this.trade_status, stockid + ":下單買入");

            }
            else
            {
                OnStatusChange(this.trade_status, stockid + ":狀態不符，無法買入");
            }
        }

        public override void SellStock()
        {
            if (this.trade_status == TradeStatus.WaitingSell || this.trade_status == TradeStatus.WaitingSellSignal)
            {
                PI31002 pi31002 = (PI31002)DepthLog[0];
                decimal maxbuyprice = pi31002.BUY_DEPTH[0].PRICE;
                MatchLoger("下單賣出:" + pi31002.Match_Time.ToString() + " 賣出價位可能為:" + maxbuyprice.ToString());
                currentSellMatchPrice = maxbuyprice;
                this.trade_status = TradeStatus.StandBy;
                quotecom.OnRcvMessage -= WaitingSell;

                OnStatusChange(this.trade_status, stockid + ":下單賣出");
            }
            else
            {
                OnStatusChange(this.trade_status, stockid + ":狀態不符，無法賣出");
            }
        }
    }
}
