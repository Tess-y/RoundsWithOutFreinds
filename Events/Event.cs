using Rounds_Rogelike.GameModes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Rounds_Rogelike.Events
{
    public abstract class Event
    {
        public static List<Event> Events = new List<Event>(); 
        public abstract bool IsValid(Player player, int floor);
        public abstract IEnumerator DoEvent(Player player, GM_RWOF gm);
    }
}
