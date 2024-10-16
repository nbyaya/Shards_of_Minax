using System;
using System.IO;
using Server;
using Server.Commands;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
    public class OutputAllItems
    {
        public static void Initialize()
        {
            CommandSystem.Register("OutputAllItems", AccessLevel.Administrator, new CommandEventHandler(OutputAllItems_OnCommand));
        }

        [Usage("OutputAllItems")]
        [Description("Outputs all items on the server to a file.")]
        public static void OutputAllItems_OnCommand(CommandEventArgs e)
        {
            string filePath = Path.Combine(Core.BaseDirectory, "AllItems.txt");
            using (StreamWriter writer = new StreamWriter(filePath, false))
            {
                foreach (Item item in World.Items.Values)
                {
                    writer.WriteLine("Item: {0}, Serial: {1}, Location: {2}, Map: {3}, Type: {4}", 
                        item.Name ?? "Unnamed", 
                        item.Serial, 
                        item.Location, 
                        item.Map, 
                        item.GetType().Name);
                }
            }

            e.Mobile.SendMessage("All items have been output to {0}", filePath);
        }
    }
}
