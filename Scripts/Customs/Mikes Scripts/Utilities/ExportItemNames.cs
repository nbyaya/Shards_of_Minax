using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server;
using Server.Commands;
using Server.Items;

namespace Server.Scripts.Commands
{
    public class ExportItemNames
    {
        public static void Initialize()
        {
            CommandSystem.Register("ExportItemNames", AccessLevel.Administrator, new CommandEventHandler(OnExportItemNames));
        }

        [Usage("ExportItemNames")]
        [Description("Exports all internal item names to a .txt file.")]
        private static void OnExportItemNames(CommandEventArgs e)
        {
            Mobile m = e.Mobile;

            string filePath = Path.Combine(Core.BaseDirectory, "ItemNames.txt");

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (Assembly asm in ScriptCompiler.Assemblies)
                {
                    foreach (Type type in asm.GetTypes())
                    {
                        if (type.IsSubclassOf(typeof(Item)))
                        {
                            writer.WriteLine(type.Name);
                        }
                    }
                }
            }

            m.SendMessage("Item names have been exported to ItemNames.txt.");
        }
    }
}
