using System;
using Server;
using Server.Commands;

namespace Server.Commands
{
    public class AutoSummonCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("autosummon", AccessLevel.Player, new CommandEventHandler(AutoSummon_OnCommand));
        }

        [Usage("autosummon")]
        [Description("Toggles auto summoning on/off for all your equipped items.")]
        private static void AutoSummon_OnCommand(CommandEventArgs e)
        {
            Mobile m = e.Mobile;
            AutoSummonManager.ToggleAutoSummon(m);
        }
    }
}
