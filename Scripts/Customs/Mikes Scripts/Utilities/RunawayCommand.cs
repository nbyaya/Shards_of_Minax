using System;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands
{
    public class RunawayCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("runaway", AccessLevel.Player, new CommandEventHandler(Runaway_OnCommand));
        }

        [Usage("runaway")]
        [Description("Instantly teleports you to a predefined location and displays a message.")]
        public static void Runaway_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            // Teleport the player to the coordinates (1323, 1624, 55) in Trammel
            Point3D location = new Point3D(1323, 1624, 55);
            Map map = Map.Trammel;

            from.MoveToWorld(location, map);

            // Display the message
            from.SendMessage("You bravely run away!");
        }
    }
}
