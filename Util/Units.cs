using System;
using System.Collections.Generic;
using System.Text;

namespace Rounds_Rogelike.Util
{
    internal class Units
    {
        public static CardInfo[] Grub;
        public static CardInfo[] Spewer;


        public static void init()
        {
            Grub = new CardInfo[] {Cards.Grub.cardInfo};
            Spewer = new CardInfo[] { Cards.Spewer.cardInfo, ModdingUtils.Utils.Cards.instance.GetCardWithName("Demonic pact")  };

            foreach (CardInfo card in Spewer)
            UnityEngine.Debug.Log(card.name);
        }
    }
}
