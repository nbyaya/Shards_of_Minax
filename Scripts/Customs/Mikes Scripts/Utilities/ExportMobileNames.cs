using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
    public class ExportMobileNames
    {
        public static void Initialize()
        {
            CommandSystem.Register("ExportMobileNames", AccessLevel.Administrator, new CommandEventHandler(OnExportMobileNames));
        }

        [Usage("ExportMobileNames")]
        [Description("Exports all internal mobile names to a .txt file.")]
        private static void OnExportMobileNames(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            string filePath = Path.Combine(Core.BaseDirectory, "MobileNames.txt");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Assembly asm in ScriptCompiler.Assemblies)
                {
                    foreach (Type type in asm.GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(Mobile)))
                        {
                            writer.WriteLine(type.Name);
                        }
                    }
                }
            }

            m.SendMessage("Mobile names have been exported to MobileNames.txt.");
        }
    }
}
