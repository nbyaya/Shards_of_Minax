using System;
using Server;
using Server.Mobiles;
using Server.Commands;
using Bittiez.CustomSystems; // 确保引用 TensionManager 的命名空间

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

            // 将 1 改为 0.001d (使用 d 后缀明确表示 double)
            TensionManager.IncreaseTension(0.001d);
        }

        private static void TensionGump_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new TensionAdminGump());
        }
    }
}
