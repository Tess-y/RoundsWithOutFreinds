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
    class Grub : AICardBase
    {
        public override void SetupCard(CardInfo cardInfo, Gun gun, ApplyCardStats cardStats, CharacterStatModifiers statModifiers, Block block)
        {
            gun.damage = 0.25f;
            statModifiers.movementSpeed = 0.5f;
            block.cdMultiplier = 1000;
        } 
        public override void OnAddCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
            player.gameObject.AddComponent<ColorEffect>().SetColor(new Color(0,0.6f,0));
            player.data.stats.GetAditionalData().gold_drop_min = 0;
            player.data.stats.GetAditionalData().gold_drop_max = 2;
            data.maxHealth *= 0.25f;
            data.health = data.maxHealth;

            base.OnAddCard(player, gun, gunAmmo, data, health, gravity, block, characterStats);

        } 
        public override void OnRemoveCard(Player player, Gun gun, GunAmmo gunAmmo, CharacterData data, HealthHandler health, Gravity gravity, Block block, CharacterStatModifiers characterStats)
        {
        }

        protected override string GetTitle()
        {
            return "Grub";
        }
        protected override GameObject GetCardArt()
        {
            return null;
        }

    }
}
