using System;
using System.Collections.Generic;
using System.Text;
using UnboundLib.Cards;

namespace Rounds_Rogelike.Cards
{
    public abstract class PathCardBase : CustomCard
    {
        public static CardInfo cardInfo;


        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
            };
        }

        public override string GetModName()
        {
            return "RWOF_PATH";
        }
        public static void callback(CardInfo card)
        {
            cardInfo = card;
        }
    }
}
