using System.Collections.Generic;
using Server;

namespace Server
{
    public static class AutoSummonManager
    {
        private static Dictionary<Mobile, bool> m_AutoSummon = new Dictionary<Mobile, bool>();

        // Returns true if autosummon is enabled; defaults to false if not set.
        public static bool IsAutoSummonEnabled(Mobile m)
        {
            if (m == null)
                return false;

            if (m_AutoSummon.TryGetValue(m, out bool enabled))
                return enabled;

            return false; // Default is now OFF
        }

        // Toggles the autosummon setting and notifies the player.
        public static void ToggleAutoSummon(Mobile m)
        {
            if (m == null)
                return;

            bool newState = !IsAutoSummonEnabled(m);
            m_AutoSummon[m] = newState;
            m.SendMessage("Autosummon has been turned {0}.", newState ? "on" : "off");
        }
    }
}
