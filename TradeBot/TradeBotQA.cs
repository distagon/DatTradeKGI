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
   public class TradeBotQA : TradeBotBase
    {
        
        public TradeBotQA(string stockid, string brokerid, string account, ushort BuyQty, QuoteCom quotecom, TaiFexCom taifexcom, double stoplossratio,double lockgainprice) : base(stockid, brokerid, account, BuyQty, quotecom, taifexcom, stoplossratio,lockgainprice)
        {
            buy_mode = BuyMode.Auto;
            stoplossmode = StopLossMode.Manual;
            lockgainmode = LockGainMode.Manual;
        }
        protected override Boolean BuySignal(PI31001 pi31001)
        {
            //開盤篩選
            if (pi31001.Match_Qty == pi31001.Total_Qty)
            {
                //Open < close 開盤低於左日收盤價:不監控
                if (OpenPrice < ClosePrice)
                {
                    BreakTrade();
                    return false;
                }
                //開盤價大於CDP AH: 不監控
                if (OpenPrice > CDP_AH)
                {
                    BreakTrade();
                    return false;
                }
                return false;
            }
            else {
                //盤中篩選
                //盤中變綠棒: 不監控
                if ((double)pi31001.Match_Price < OpenPrice) {
                    BreakTrade();
                    return false;
                }
                //開盤介於左收與CDP_NH之間
                if (OpenPrice > ClosePrice && OpenPrice < CDP_NH) {
                    if ((double)pi31001.Match_Price > AdjustPrice(CDP_NH)) {
                        return true;
                    }
                }
                //開盤介於CDP_NH與CDP_AH之間
                if (OpenPrice > CDP_NH && OpenPrice < CDP_AH)
                {
                    if ((double)pi31001.Match_Price > AdjustPrice(CDP_AH))
                    {
                        return true;
                    }
                }

                return false;
            }            
        }
    }
}
