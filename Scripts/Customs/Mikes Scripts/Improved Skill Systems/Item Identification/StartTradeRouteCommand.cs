using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Commands;
using Server.Items;
using Server.Custom;

namespace Server.Stables
{
    public class StartTradeRouteCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("starttrade", AccessLevel.Player, new CommandEventHandler(OpenStable_OnCommand));
        }

        [Usage("StartTradeRoute")]
        [Description("Opens the trade route menu")]
        public static void OpenStable_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from != null && from is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)from;
                from.CloseGump(typeof(TradeRouteGump));
                from.SendGump(new TradeRouteGump(e.Mobile));
            }
        }
    }
}