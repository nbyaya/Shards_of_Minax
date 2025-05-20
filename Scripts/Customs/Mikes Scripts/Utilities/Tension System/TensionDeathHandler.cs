using System;
using System.IO;
using Server;
using Server.Mobiles;
using Server.Commands;
using Bittiez.CustomSystems;

namespace Bittiez.CustomSystems
{
    public class TensionDeathHandler
    {
        private static bool _initialized = false;
        private static readonly string LogFilePath = Path.Combine("Logs", "TensionSystem.log");

        public static void Initialize()
        {
            if (_initialized)
                return;

            _initialized = true;

            // 确保日志目录存在
            Directory.CreateDirectory("Logs");

            EventSink.CreatureDeath += OnCreatureDeath;
            CommandSystem.Register("TensionGump", AccessLevel.Administrator, new CommandEventHandler(TensionGump_OnCommand));
        }

        private static void OnCreatureDeath(CreatureDeathEventArgs e)
        {
            if (e.Creature == null)
                return;

            if (e.Creature is BaseCreature baseCreature && baseCreature.Summoned)
                return;

            TensionManager.IncreaseTension(TensionManager.CreatureDeathTensionIncrement);
            LogTensionChange(e.Creature);
        }

        private static void LogTensionChange(Mobile creature)
        {
            try
            {
                string logEntry = string.Format("[{0}] {1} 死亡, 张力增加: {2}, 当前总张力: {3}",
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    creature.Name,
                    TensionManager.CreatureDeathTensionIncrement,
                    TensionManager.Tension);

                // 写入日志文件
                File.AppendAllText(LogFilePath, logEntry + Environment.NewLine);
            }
            catch
            {
                // 忽略日志错误
            }
        }

        private static void TensionGump_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendGump(new TensionAdminGump());
        }
    }
}