using System;
using Server;
using Server.Mobiles;
using Server.Commands;

namespace Server.Commands
{
    public class DoTimeCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("DoTime", AccessLevel.Player, new CommandEventHandler(DoTime_OnCommand));
        }

        [Usage("DoTime")]
        [Description("Sends a criminal player to prison, clears their criminal flag, resurrects them if necessary, and announces their forgiveness.")]
        public static void DoTime_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from is PlayerMobile player)
            {
                // Check if the player is flagged as a criminal
                if (player.Criminal)
                {
                    // Teleport to the prison
                    Point3D prisonLocation = new Point3D(267, 765, 20);
                    Map prisonMap = Map.Trammel;

                    player.MoveToWorld(prisonLocation, prisonMap);

                    // Remove criminal flag
                    player.Criminal = false;

                    // Resurrect if they are dead
                    if (player.Alive == false)
                    {
                        player.Resurrect();
                    }

                    // Notify the player
                    player.SendMessage(0x35, "You have done time in jail. Lord British has forgiven your crimes.");

                    // Optionally, broadcast forgiveness (to the player or server-wide)
                    // from.Say("I have paid for my crimes and been forgiven by Lord British.");
                }
                else
                {
                    // Inform the player they are not a criminal
                    from.SendMessage(0x22, "You are not flagged as a criminal and do not need to do time.");
                }
            }
            else
            {
                from.SendMessage(0x22, "Only players can use this command.");
            }
        }
    }
}
