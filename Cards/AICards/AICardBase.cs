using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Rounds_Rogelike.Cards
{ 
    public abstract class AICardBase : CustomCard
    {
        public static CardInfo cardInfo;
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {

            GameObject component = new GameObject();
            component.transform.SetParent(player.transform, false);
            component.GetOrAddComponent<TextMeshProUGUI>().text = GetTitle();
            component.GetOrAddComponent<TextMeshProUGUI>().fontSize = 0.5f;
            component.transform.localPosition = Vector3.up * 2;
            component.GetOrAddComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
            component.GetOrAddComponent<Canvas>().sortingLayerName = "MostFront";
            component.AddComponent<Util.DestroyOnUnparent>();
            component.transform.SetParent(player.transform, false);

        }


        public override string GetModName()
        {
            return "RWOF_AI";
        }


        protected override string GetDescription()
        {
            return "AI_CARD FOR: " + GetTitle();
        }

        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
            };
        }

        internal static void callback(CardInfo card)
        {
            cardInfo = card;
        }

        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.TechWhite;
        }
    }
}
