using System;
using System.IO;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Commands;

namespace Server.Misc
{
    public class SpawnSosariaSignsFromFile
    {
        private const int SosariaMapIndex = 32; // Ensure this matches your custom map registration

        public static void Initialize()
        {
            CommandSystem.Register("SpawnSosariaSigns", AccessLevel.Administrator, new CommandEventHandler(SpawnSigns_OnCommand));
        }

        [Usage("SpawnSosariaSigns")]
        [Description("Reads sign data from Data/SosariaSigns.txt and spawns signs in the Sosaria facet.")]
        public static void SpawnSigns_OnCommand(CommandEventArgs e)
        {
            string filePath = Path.Combine(Core.BaseDirectory, "Data/SosariaSigns.txt");

            if (!File.Exists(filePath))
            {
                e.Mobile.SendMessage("Sign data file not found at: " + filePath);
                return;
            }

            Map sosaria = Map.Maps[SosariaMapIndex];
            if (sosaria == null)
            {
                e.Mobile.SendMessage("Custom map (Sosaria) not loaded or invalid.");
                return;
            }

            int count = 0;
            foreach (string line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                string[] parts = line.Trim().Split(new[] { ' ' }, 6);
                if (parts.Length < 6)
                    continue;

                try
                {
                    int itemID = int.Parse(parts[1]);
                    int x = int.Parse(parts[2]);
                    int y = int.Parse(parts[3]);
                    int z = int.Parse(parts[4]);
                    string label = parts[5];

                    Static sign = new Static(itemID)
                    {
                        Name = label,
                        Movable = false
                    };
                    sign.MoveToWorld(new Point3D(x, y, z), sosaria);
                    count++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error parsing sign line: " + line);
                    Console.WriteLine(ex);
                }
            }

            e.Mobile.SendMessage($"Spawned {count} signs in Sosaria.");
        }
    }
}
