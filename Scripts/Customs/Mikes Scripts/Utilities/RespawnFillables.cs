using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Commands;
using Server.Items;

namespace Server.Commands
{
    public class RespawnFillables
    {
        public static void Initialize()
        {
            CommandSystem.Register("RespawnFillables", AccessLevel.Administrator, new CommandEventHandler(RespawnFillables_OnCommand));
        }

        [Usage("RespawnFillables")]
        [Description("Forces all fillable containers in the world to respawn their contents.")]
        public static void RespawnFillables_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            int count = 0;
            int fail = 0;

            List<FillableContainer> containers = World.Items.Values.OfType<FillableContainer>().ToList();

            foreach (FillableContainer container in containers)
            {
                if (container.Deleted || container.Map == Map.Internal)
                    continue;

                if (container.ContentType == FillableContentType.None)
                {
                    container.AcquireContent();
                }

                if (container.ContentType != FillableContentType.None)
                {
                    container.Respawn(true);
                    count++;
                }
                else
                {
                    fail++;
                }
            }

            from.SendMessage(0x35, $"Respawned {count} fillable containers. {fail} failed to acquire content.");
        }
    }
}
