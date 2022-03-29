using ModdingUtils.MonoBehaviours;
using Rounds_Rogelike.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnboundLib;
using UnboundLib.Cards;
using UnityEngine;

namespace Rounds_Rogelike.Cards
{
    class Spewer : CustomCard
    {
        public static CardInfo cardInfo;
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            gun.damage = 0.25f;
            block.cdMultiplier = 1000;
            gun.reloadTime = 0.01f;
            statModifiers.regen = 10;
        } 
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<ColorEffect>().SetColor(new Color(0.470f, 0, 0.580f));
            player.data.stats.GetAditionalData().gold_drop_min = 1;
            player.data.stats.GetAditionalData().gold_drop_max = 4;

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
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle()
        {
            return "Spewer";
        }
        protected override string GetDescription()
        {
            return "CardDescription";
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
        protected override CardInfo.Rarity GetRarity()
        {
            return CardInfo.Rarity.Common;
        }
        protected override CardInfoStat[] GetStats()
        {
            return new CardInfoStat[]
            {
            };
        }
        protected override CardThemeColor.CardThemeColorType GetTheme()
        {
            return CardThemeColor.CardThemeColorType.ColdBlue;
        }
        public override string GetModName()
        {
            return "RWOF_AI";
        }

        internal static void callback(CardInfo card)
        {
            cardInfo = card;
        }
    }
}
