using Rounds_Rogelike.GameModes;
using System;
using System.Text;
using TMPro;
using UnboundLib;
using UnityEngine;


namespace Rounds_Rogelike.UI
{
    public class UIHandaler
    {
        public static GameObject Create_UI(GM_RWOF gm)
        {
            GameObject UI = new GameObject("RWOF_UI");
            GameObject UI_floor = new GameObject("RWOF_UI-floor");
            UI_floor.AddComponent<Util.DestroyOnUnparent>();
            UI_floor.transform.SetParent(UI.transform, false);
            UI_floor.GetOrAddComponent<Canvas>().sortingLayerName = "MostFront";
            UI_floor.GetOrAddComponent<TextMeshProUGUI>().fontSize = 1.5f;
            UI_floor.AddComponent<UpdateFloor>().gm = gm;
            UI_floor.GetOrAddComponent<RectTransform>().sizeDelta = new Vector2(10,10);
            GameObject UI_gold = new GameObject("RWOF_UI-gold");
            UI_gold.AddComponent<Util.DestroyOnUnparent>();
            UI_gold.transform.SetParent(UI.transform, false);
            UI_gold.GetOrAddComponent<Canvas>().sortingLayerName = "MostFront";
            UI_gold.GetOrAddComponent<TextMeshProUGUI>().fontSize = 1.5f;
            UI_gold.AddComponent<UpdateGold>().gm = gm;
            UI_gold.transform.localPosition = Vector3.down*2;
            UI_gold.GetOrAddComponent<RectTransform>().sizeDelta = new Vector2(10, 10);
            UI.transform.position = new Vector3(-30, 15, 0);
            return UI;
        }
    }

    internal class UpdateUI : MonoBehaviour
    {
        public virtual string Get_Value()
        {
            return "";
        }
        public void Update()
        {
            gameObject.GetComponent<TextMeshProUGUI>().text = Get_Value();
        }
    }

    internal class UpdateFloor : UpdateUI
    {
        internal GM_RWOF gm;
        public override string Get_Value()
        {
            return "Floor: "+gm.floorHandler.floor;
        }
    }
    internal class UpdateGold : UpdateUI
    {
        internal GM_RWOF gm;
        public override string Get_Value()
        {
            return "Gold: "+ ItemShops.Extensions.PlayerExtension.GetAdditionalData(PlayerManager.instance.players.Find(p => p.playerID == 0)).bankAccount.Money.GetValueOrDefault(Main.Gold,0);
        }
    }
}
