using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Rounds_Rogelike.Cards;

namespace Rounds_Rogelike.Util
{
    public class Util
    {
        public static CardCategory[] GetPathCadigories()
        {
            return new CardCategory[] {Fight_Path.category, Shop_Path.category };
        }
    }

    public class DestroyOnUnparent : MonoBehaviour
    {
        void LateUpdate()
        {
            if (this.gameObject.transform.parent == null) { Destroy(this.gameObject); }
        }
    }
}
