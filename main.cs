using UnboundLib;
using UnboundLib.Cards;
using HarmonyLib;
using UnboundLib.GameModes;
using BepInEx;
using Rounds_Rogelike.Cards;
using CardChoiceSpawnUniqueCardPatch.CustomCategories;
using System.Collections.Generic;
using System.Linq;

namespace Rounds_Rogelike
{
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("com.willuwontu.rounds.itemshops", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.pickncards", BepInDependency.DependencyFlags.HardDependency)]
    [BepInPlugin(ModId, ModName, Version)]
    [BepInProcess("Rounds.exe")]
    public class Main : BaseUnityPlugin
    {
        public static readonly string Gold = "Gold"; 

        private const string ModId = "Root.Round.Rogelite";
        private const string ModName = "RRL";
        public const string Version = "0.0.0"; // What version are we on (major.minor.patch)?

        public static CardCategory pathCard;
        public static CardCategory nonPathCard;

        void Awake()
        {
            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }

        void Start()
        {
            pathCard = CustomCardCategories.instance.CardCategory("__Path__");
            nonPathCard = CustomCardCategories.instance.CardCategory("__NotPath__");
            GameModeManager.AddHandler<GameModes.GM_RWOF>("Adventure", new GameModes.RWOFHandler());

            Unbound.Instance.ExecuteAfterSeconds(0.4f, BuildDefaultCategory);



            CustomCard.BuildCard<Grub>(Grub.callback);
            CustomCard.BuildCard<Spewer>(Spewer.callback);


            CustomCard.BuildCard<Shop_Path>(Shop_Path.callback);
            CustomCard.BuildCard<Fight_Path>(Fight_Path.callback);

            this.ExecuteAfterSeconds(1f, Util.Units.init);
        }


        private static void BuildDefaultCategory()
        {
            List<UnboundLib.Utils.Card> allCards = UnboundLib.Utils.CardManager.cards.Values.ToList();
            foreach (var currentCard in allCards)
            {
                var currentCardsCategories = currentCard.cardInfo.categories.ToList();

                if (currentCardsCategories.Contains(pathCard))
                {
                    continue;
                }

                currentCardsCategories.Add(nonPathCard);

                currentCard.cardInfo.categories = currentCardsCategories.ToArray();
            }
        }
    }
}
