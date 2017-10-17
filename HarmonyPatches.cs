using Harmony;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using Verse.AI;
using System.Reflection.Emit;
using System.Runtime.CompilerServices;



namespace TradeEconomy
{
    [StaticConstructorOnStartup]
    static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("com.walkingproblem.tradingeconomy");
            //harmony.PatchAll(Assembly.GetExecutingAssembly());
            harmony.Patch(AccessTools.Method(typeof(Tradeable), nameof(Tradeable.GetPriceFor)), null, new HarmonyMethod(typeof(HarmonyPatches), nameof(Tradeable_GetPriceFor_Patch)), null);
            
        }
       
        public static void Tradeable_GetPriceFor_Patch(Tradeable __instance) =>
            __instance.InitPriceDataIfNeeded();
            if (action == TradeAction.PlayerBuys)
            {
                return __instance.pricePlayerBuy;
            }
            return __instance.pricePlayerSell;




[HarmonyPatch(typeof(Tradeable), "InitPriceDataIfNeeded", null)]

    public static class Tradeable_InitPriceDataIfNeeded_Patch
    {

        static void Prefix(Tradeable __instance)
        {
            Log.Message("test");
            float pricePlayerBuy = -1f;
            float pricePlayerSell = -1f;
            float priceFactorBuy_TraderPriceType;
            float priceFactorSell_TraderPriceType;
            float priceGain_PlayerNegotiator;
            float priceGain_FactionBase;
            float priceFactorSell_ItemSellPriceFactor;
            float priceGain_TraderNegotiator;
            if (pricePlayerBuy > 0f)
            {
                return;
            }
            priceFactorBuy_TraderPriceType = __instance.PriceTypeFor(TradeAction.PlayerBuys).PriceMultiplier();
            priceFactorSell_TraderPriceType = __instance.PriceTypeFor(TradeAction.PlayerSells).PriceMultiplier();
            priceGain_PlayerNegotiator = TradeSession.playerNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true);
            priceGain_TraderNegotiator = 0.5f;// TradeSession.trader.TradeNegotiator.GetStatValue(StatDefOf.TradePriceImprovement, true);
            priceGain_FactionBase = TradeSession.trader.TradePriceImprovementOffsetForPlayer;
            pricePlayerBuy = __instance.BaseMarketValue * 1.5f * priceFactorBuy_TraderPriceType * (1f + Find.Storyteller.difficulty.tradePriceFactorLoss);
            pricePlayerBuy *= 1f - priceGain_PlayerNegotiator - priceGain_FactionBase + priceGain_TraderNegotiator;
            pricePlayerBuy *= 100f;
            //pricePlayerBuy = Mathf.Max(pricePlayerBuy, 0.5f);
            Log.Message("pricePlayerBuy" + pricePlayerBuy);
            if (pricePlayerBuy > 99.5f)
            {
                Log.Message("chicken you" + pricePlayerBuy);
                pricePlayerBuy = Mathf.Round(pricePlayerBuy);
            }
            priceFactorSell_ItemSellPriceFactor = __instance.AnyThing.GetStatValue(StatDefOf.SellPriceFactor, true);
            pricePlayerSell = __instance.BaseMarketValue * 0.5f * priceFactorSell_TraderPriceType * priceFactorSell_ItemSellPriceFactor * (1f - Find.Storyteller.difficulty.tradePriceFactorLoss);
            pricePlayerSell *= 1f + priceGain_PlayerNegotiator + priceGain_FactionBase - priceGain_TraderNegotiator;
            pricePlayerSell *= 100f;
            //pricePlayerSell = Mathf.Max(pricePlayerSell, 0.01f);
            Log.Message("pricePlayerSell" + pricePlayerSell);
            if (pricePlayerSell > 99.5f)
            {
                Log.Message("fuck you" + pricePlayerSell);
                pricePlayerSell = Mathf.Round(pricePlayerSell);
            }
            if (pricePlayerSell >= pricePlayerBuy)
            {
                Log.Message("Trying to put player-sells price above player-buys price for " + __instance.AnyThing);
                pricePlayerSell = pricePlayerBuy;
            }

        }

    }
    
}
