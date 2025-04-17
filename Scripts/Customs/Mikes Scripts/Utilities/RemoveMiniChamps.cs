using System;
using System.Collections.Generic;
using Server;
using Server.Commands;
using Server.Engines.MiniChamps;

namespace Server.Commands
{
    public class RemoveMiniChampsCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("RemoveMiniChamps", AccessLevel.Administrator, new CommandEventHandler(RemoveMiniChamps_OnCommand));
        }

        [Usage("RemoveMiniChamps")]
        [Description("Removes all MiniChamp spawners from the world.")]
        public static void RemoveMiniChamps_OnCommand(CommandEventArgs e)
        {
            List<Item> toDelete = new List<Item>();
            
            foreach (Item item in World.Items.Values)
            {
                if (item is MiniChamp)
                {
                    toDelete.Add(item);
                }
            }

            int count = toDelete.Count;
            foreach (Item item in toDelete)
            {
                item.Delete();
            }

            e.Mobile.SendMessage($"Removed {count} MiniChamp spawners from the world.");
        }
    }
}
