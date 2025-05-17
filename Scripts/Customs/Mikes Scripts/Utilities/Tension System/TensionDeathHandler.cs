using System;
using Server;
using Server.Mobiles;
using Server.Commands;

namespace Bittiez.CustomSystems
{
    public class TensionDeathHandler
    {
        public static void Initialize()
        {
            // Hook into the creature death event
            EventSink.CreatureDeath += OnCreatureDeath;

            // Register an admin command to open the tension gump.
            CommandSystem.Register("TensionGump", AccessLevel.Administrator, new CommandEventHandler(TensionGump_OnCommand));
        }

        private static void OnCreatureDeath(CreatureDeathEventArgs e)
        {
            if (e.Creature == null)
                return;

            // Optionally, add any checks here (e.g., ignore summoned creatures)
            if (e.Creature is BaseCreature baseCreature && baseCreature.Summoned)
                return;

            // Each death increases tension by 1
            TensionManager.IncreaseTension(0.0001);
        }

        private static void TensionGump_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new TensionAdminGump());
        }
    }
}
