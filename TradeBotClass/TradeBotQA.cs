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
        
        public TradeBotQA(string stockid, string brokerid, string account, ushort BuyQty, QuoteCom quotecom, TaiFexCom taifexcom, decimal stoplossratio,decimal lockgainprice, int amountthreshold, BuyMode buymode, StopLossMode stoplossmode, LockGainMode lockgainmode) : base(stockid, brokerid, account, BuyQty, quotecom, taifexcom, stoplossratio,lockgainprice,amountthreshold,buymode,stoplossmode,lockgainmode)
        {
            
        }

        //判斷目前價位是否超過成本
        private Boolean isExceedCost(PI31002 pi31002)
        {
            int ticknum = 0;
            if (CurrentBuyMatchPrice >= 11 && CurrentBuyMatchPrice <= 15)
            {
                ticknum = 2;
            }
            else if (CurrentBuyMatchPrice >= 15.05m && CurrentBuyMatchPrice <= 30) {
                ticknum = 4;
            }
            else if (CurrentBuyMatchPrice >= 30.05m && CurrentBuyMatchPrice <= 49.9m)
            {
                ticknum = 6;
            }
            else if (CurrentBuyMatchPrice >= 50 && CurrentBuyMatchPrice <= 60)
            {
                ticknum = 4;
            }
            else if (CurrentBuyMatchPrice >= 60.1m && CurrentBuyMatchPrice <= 75)
            {
                ticknum = 5;
            }
            else if (CurrentBuyMatchPrice >= 75.1m && CurrentBuyMatchPrice <= 99.9m)
            {
                ticknum = 6;
            }
            else if (CurrentBuyMatchPrice >= 100 && CurrentBuyMatchPrice <= 150)
            {
                ticknum = 2;
            }
            else if (CurrentBuyMatchPrice >= 150.5m && CurrentBuyMatchPrice <= 300)
            {
                ticknum = 4;
            }

            if (pi31002.SELL_DEPTH[0].PRICE >= CurrentBuyMatchPrice + (ticknum * TickInfo(CurrentBuyMatchPrice)))
                return true;

                return false;
        }

        private Boolean isStopLoss(PI31002 pi31002) {

            int ticknum = 0;
            decimal stopbase = 0;
            if (CurrentBuyMatchPrice >= 11 && CurrentBuyMatchPrice <= 15)
            {
                ticknum = 3;
            }
            else if (CurrentBuyMatchPrice >= 15.05m && CurrentBuyMatchPrice <= 30)
            {
                ticknum = 5;
            }
            else if (CurrentBuyMatchPrice >= 30.05m && CurrentBuyMatchPrice <= 49.9m)
            {
                ticknum = 5;
            }
            else if (CurrentBuyMatchPrice >= 50 && CurrentBuyMatchPrice <= 60)
            {
                ticknum = 5;
            }
            else if (CurrentBuyMatchPrice >= 60.1m && CurrentBuyMatchPrice <= 75)
            {
                ticknum = 5;
            }
            else if (CurrentBuyMatchPrice >= 75.1m && CurrentBuyMatchPrice <= 99.9m)
            {
                ticknum = 6;
            }
            else if (CurrentBuyMatchPrice >= 100 && CurrentBuyMatchPrice <= 150)
            {
                ticknum = 3;
            }
            else if (CurrentBuyMatchPrice >= 150.5m && CurrentBuyMatchPrice <= 300)
            {
                ticknum = 5;
            }

            if (OpenPrice > ClosePrice && OpenPrice < CDP_NH)
                stopbase = CDP_NH;
            else if (OpenPrice > CDP_NH && OpenPrice < CDP_AH)
                stopbase = CDP_AH;
            if (pi31002.BUY_DEPTH[0].PRICE <= stopbase - (ticknum * TickInfo(CurrentBuyMatchPrice)))
                return true;

            return false;
        }
        protected override Boolean SellSignal(string UpdateType, string MatchType)
        {
            PI31002 pi31002 = (PI31002)DepthLog[0];
            PI31001 pi31001_0= (PI31001)MatchLog[0];
            PI31001 pi31001_1 = (PI31001)MatchLog[1];
            PI31001 pi31001_2 = (PI31001)MatchLog[2];

            //判斷是否已突破成本
            ExceedCost = isExceedCost(pi31002);

            if (ExceedCost) {
                if (UpdateType == "Match" && MatchType == "G")
                {
                    GreenMatchCount += 1;
                    //已突破成本後，如果連續出現兩筆錄單，賣出
                    if (GreenMatchCount >= 2) {
                        return true;
                    }
                }
                else {
                    GreenMatchCount =0;
                }

                if (pi31001_0.Match_Price < pi31001_1.Match_Price && pi31001_1.Match_Price < pi31001_2.Match_Price) {

                    return true;
                }

                //距離漲停價位2檔時賣出

                //停利賣出，內盤大於停利點
                if (pi31002.BUY_DEPTH[0].PRICE > LockGainPrice)
                    return true;

            }
            //停損賣出

            if (isStopLoss(pi31002))
                return true;



            return false;
        }
        protected override Boolean BuySignal(String UpdateType,String MatchType)
        {
            PI31001 pi31001 = (PI31001)MatchLog[0];//最新成交明細
            PI31002 pi31002 = (PI31002)DepthLog[0];//最新五檔明細
            //開盤篩選
            if (pi31001.Match_Qty == pi31001.Total_Qty)
            {
                //Open < close 開盤低於左日收盤價:不監控
                if (OpenPrice < ClosePrice)
                {
                    BreakTrade("開盤低於左日收盤價");
                    return false;
                }
                //開盤價大於CDP AH: 不監控
                if (OpenPrice > CDP_AH)
                {
                    BreakTrade("開盤價大於CDP AH");
                    return false;
                }
                return false;
            }
            else {
            //盤中篩選

                //盤中變綠棒: 不監控
                if (pi31001.Match_Price < OpenPrice) {
                    BreakTrade("盤中低於開盤價");
                    return false;
                }
                //如果五分鐘後成交量未達門檻: 不監控
                if (pi31001.Match_Time > 90500 && pi31001.Total_Qty < AmountThreshold) {
                    BreakTrade("五分鐘後成交量未達門檻");
                    return false;
                }

                //五分鐘前判斷是否超越CDP
                if (pi31002.Match_Time < 90500 && pi31001.Match_Time < 90500) {

                    if (OpenPrice > ClosePrice && OpenPrice < CDP_NH) {
                        if (pi31002.BUY_DEPTH[0].PRICE > CDP_NH) {
                            ExceedCDP = true;
                        }
                    }
                    if (OpenPrice > CDP_NH && OpenPrice < CDP_AH) {
                        if (pi31002.BUY_DEPTH[0].PRICE > CDP_AH)
                        {
                            ExceedCDP = true;
                        }
                    }
                }


                if (pi31002.Match_Time > 90500 && pi31001.Match_Time > 90500) {
                    //紀錄5分鐘後第一筆成交與五檔
                    if (pi31001_5min is null)
                        pi31001_5min = pi31001;
                    if (pi31002_5min is null)
                        pi31002_5min = pi31002;
                    
                    //開盤介於昨收與CDP_NH之間
                    if (OpenPrice > ClosePrice && OpenPrice < CDP_NH)
                    {
                        //五分鐘後第一筆內盤最高價超越CDP_AH: 不監控
                        if (pi31002_5min.BUY_DEPTH[0].PRICE > CDP_AH)
                        {
                            BreakTrade("五分鐘後第一筆內盤最高價超越CDP_AH");
                            return false;
                        }
                        //如果五分鐘後內盤最高已超過CDP_NH則直接掛CDP2 價位買入
                        else if (pi31002_5min.BUY_DEPTH[0].PRICE > CDP_NH && pi31002_5min.BUY_DEPTH[0].PRICE < CDP_AH)
                        {
                            priceflag = Security_PriceFlag.SP_FixedPrice;
                            BuyPrice = CDP_NH;
                            return true;
                        }
                        //五分鐘內有突破CDP_NH，但又掉下來，在五分鐘後第一筆沒有超過CDP_NH: 不監控
                        else if (ExceedCDP) {
                            BreakTrade("五分鐘內有突破，但又掉下來，在五分鐘後第一筆沒有超過CDP_NH");
                            return false;
                        }
                        else if (pi31002.BUY_DEPTH[0].PRICE > CDP_NH)
                        {
                            return true;
                        }
                    }
                    //開盤介於CDP_NH與CDP_AH之間
                    if (OpenPrice > CDP_NH && OpenPrice < CDP_AH)
                    {
                        //五分鐘後第一筆內盤最高價超越CDP_AH: 不監控
                        if (pi31002_5min.BUY_DEPTH[0].PRICE > CDP_AH)
                        {
                            BreakTrade("五分鐘後第一筆內盤最高價超越CDP_AH");
                            return false;
                        }
                        //五分鐘內有突破CDP_AH，但又掉下來，在五分鐘後第一筆沒有超過CDP_AH: 不監控
                        else if (ExceedCDP) {
                            BreakTrade("五分鐘內有突破，但又掉下來，在五分鐘後第一筆沒有超過CDP_AH");
                            return false;
                        }
                        else if (pi31001.Match_Price > CDP_AH)
                        {
                            return true;
                        }
                    }
                }
               

                return false;
            }            
        }
    }
}
