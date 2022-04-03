using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HarmonyLib;
using ItemShops.Utils;
using Rounds_Rogelike.Cards;
using Rounds_Rogelike.Events;
using Rounds_Rogelike.Extentions;
using Rounds_Rogelike.GameModes;
using UnboundLib;
using UnityEngine;
using Random = System.Random;

namespace Rounds_Rogelike.Floors
{
    public class FloorHandler
    {
        public int floor;

        public FloorHandler()
        {
            this.floor = 0;
        }


        public IEnumerator getNextFloor(GM_RWOF gm)
        {
            Player player = PlayerManager.instance.players.Find(p => p.playerID == 0);
            SetUpPickPhase(player);
            CardChoiceVisuals.instance.Show(0, true);
            yield return CardChoice.instance.DoPick(1, 0, PickerType.Player);
            yield return new WaitForSecondsRealtime(0.1f);
            CardChoiceVisuals.instance.Hide();

            Random rand = new Random();
            UnityEngine.Debug.Log(player.data.stats.GetAditionalData().path.ToString());
            switch (player.data.stats.GetAditionalData().path)
            {
                case Path.Shop:
                    Shop shop = ShopManager.instance.CreateShop("Floor " + floor + " shop");
                    List<UnboundLib.Utils.Card> allCards = UnboundLib.Utils.CardManager.cards.Values.Where(c => !c.category.ToUpper().StartsWith("RWOF_")).ToList();
                    allCards.Shuffle();
                    UnityEngine.Debug.Log(allCards.Count);
                    for (int i = 0; i < 4; i++)
                    {
                        Dictionary<string, int> cost = new Dictionary<string, int>();
                        switch (allCards[i].cardInfo.rarity)
                        {
                            case CardInfo.Rarity.Common:
                                cost.Add(Main.Gold, rand.Next(1, 4));
                                break;
                            case CardInfo.Rarity.Uncommon:
                                cost.Add(Main.Gold, rand.Next(4, 8));
                                break;
                            case CardInfo.Rarity.Rare:
                                cost.Add(Main.Gold, rand.Next(7, 13));
                                break;
                        }
                        shop.AddItem(allCards[i].cardInfo.name, new PurchasableCard(allCards[i].cardInfo, cost, new Tag[] { }), new PurchaseLimit(1, 1));
                    }
                    gm.floor = new Floor(shop);
                    break;
                case Path.Fight:
                    List<CardInfo[]> AI_to_spawn = new List<CardInfo[]>();
                    for (int i = rand.Next(8); i < 8; i++)
                    {
                        if (i == 7 && floor > 5)
                            AI_to_spawn.Add(Util.Units.Spewer);
                        else
                            AI_to_spawn.Add(Util.Units.Grub);
                    }
                    gm.floor = new Floor(AI_to_spawn);
                    break;
                case Path.Event:
                    List<Event> events = Event.Events.Where(e => e.IsValid(player,floor)).ToList();
                    events.Shuffle();
                    gm.floor = new Floor(events.First());
                    break;
            }
            yield return new WaitForSecondsRealtime(0.1f);
            ModdingUtils.Utils.Cards.instance.RemoveCardFromPlayer(player, player.data.currentCards.Count-1, true);
            yield return new WaitForSecondsRealtime(0.1f);
            ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.Remove(Main.nonPathCard);
            ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.RemoveAll(c => Util.Util.GetPathCadigories().Contains(c));
            yield break;
        }


        private void SetUpPickPhase(Player player)
        {
            ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.Add(Main.nonPathCard);
            if (floor < 3 || player.data.stats.GetAditionalData().path == Path.Shop) ModdingUtils.Extensions.CharacterStatModifiersExtension.GetAdditionalData(player.data.stats).blacklistedCategories.Add(Shop_Path.category);
            Traverse.Create(typeof(DrawNCards.DrawNCards)).Field("numDraws").SetValue((floor < 3 || player.data.stats.GetAditionalData().path == Path.Shop) ? 1 : 2);
        }
    }

    public class Floor
    {
        public Path Path;
        public List<CardInfo[]>? AI_to_spawn;
        public Shop? Shop;
        public Event? Event;

        public Floor(List<CardInfo[]> aI_to_spawn)
        {
            Path = Path.Fight;
            AI_to_spawn = aI_to_spawn;
        }

        public Floor(Shop shop)
        {
            Path = Path.Shop;
            Shop = shop;
        }

        public Floor(Event @event)
        {
            Path = Path.Event;
            Event = @event;
        }

        public Floor(Path path, List<CardInfo[]>? aI_to_spawn, Shop? shop, Event? @event)
        { 
            Path = path;
            AI_to_spawn = aI_to_spawn;
            Shop = shop;
            Event = @event;
        }
    }

}
