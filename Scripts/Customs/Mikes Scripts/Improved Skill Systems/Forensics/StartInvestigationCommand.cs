using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Commands;
using Server.Items;

namespace Server.Stables
{
    public class StartInvestigationCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("startinvestigation", AccessLevel.Player, new CommandEventHandler(OpenStable_OnCommand));
        }

        [Usage("StartInvestigation")]
        [Description("Opens the detective menu")]
        public static void OpenStable_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from != null && from is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)from;
                from.CloseGump(typeof(ForensicDetectiveGump));
                from.SendGump(new ForensicDetectiveGump(e.Mobile));
            }
        }
    }
}