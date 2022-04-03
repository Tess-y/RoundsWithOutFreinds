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
    class Spewer : AICardBase
    {
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

            base.OnAddCard(player, gun, gunAmmo, data, health, gravity, block, characterStats);

        } 
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle()
        {
            return "Spewer";
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }
    }
}
