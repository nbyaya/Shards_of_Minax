using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Commands;

namespace Server.Commands
{
    public class TeleportMinaxCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("TeleportMinax", AccessLevel.Administrator, new CommandEventHandler(TeleportMinax_OnCommand));
        }

        [Usage("TeleportMinax")]
        [Description("Triggers all current Minax in the world to teleport.")]
        public static void TeleportMinax_OnCommand(CommandEventArgs e)
        {
            int count = 0;
            // Create a copy of the mobiles collection to avoid modification issues.
            List<Mobile> mobiles = new List<Mobile>(World.Mobiles.Values);
            foreach (Mobile m in mobiles)
            {
                if (m is MinaxTheTimeSorceress minax)
                {
                    minax.Teleport();
                    count++;
                }
            }
            e.Mobile.SendMessage("{0} Minax teleported.", count);
        }
    }
}
