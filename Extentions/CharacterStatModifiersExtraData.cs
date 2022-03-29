using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rounds_Rogelike.Extentions
{
    [Serializable]
    public class CharacterStatModifiersExtraData
    {
        public int gold_drop_min;
        public int gold_drop_max;
        public Path path;


        public CharacterStatModifiersExtraData()
        {
            this.gold_drop_min = 0;
            this.gold_drop_max = 0;
            path = Path.Fight;
        }
        public void Reset()
        {
            gold_drop_min = 0;
            gold_drop_max = 0;
        }
    }

    public enum Path
    {
        Shop,
        Fight,
        Event,
        Mini_Boss,
        Boss
    }
    public static class CharacterStatModifiersExtension
    {
        public static readonly ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersExtraData> data =
          new ConditionalWeakTable<CharacterStatModifiers, CharacterStatModifiersExtraData>();


        public static CharacterStatModifiersExtraData GetAditionalData(this CharacterStatModifiers characterstats)
        {
            return data.GetOrCreateValue(characterstats);
        }


        public static void AddData(this CharacterStatModifiers characterstats, CharacterStatModifiersExtraData value)
        {
            try
            {
                data.Add(characterstats, value);
            }
            catch (Exception) { }
        }

    }
    [HarmonyPatch(typeof(CharacterStatModifiers), "ResetStats")]
    class CharacterStatModifiersPatchResetStats
    {
        private static void Prefix(CharacterStatModifiers __instance)
        {
            __instance.GetAditionalData().Reset();
        }
    }
}
