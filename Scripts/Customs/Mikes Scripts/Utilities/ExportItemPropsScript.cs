using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Text;
using System.Reflection;
using Server;
using Server.Commands;
using Server.Targeting;

namespace Server.Scripts.Commands
{
    public class ExportItemPropsCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("ExportItemProps", AccessLevel.Administrator, new CommandEventHandler(ExportItemProps_OnCommand));
        }

        [Serializable]
        public class ItemProperties
        {
            public string ItemType { get; set; }
            public Dictionary<string, string> Properties { get; set; }
        }

        [Usage("ExportItemProps")]
        [Description("Exports all properties of a selected item to an XML file and generates a C# script.")]
        public static void ExportItemProps_OnCommand(CommandEventArgs e)
        {
            e.Mobile.SendMessage("Target an item to export its properties and generate a C# script.");
            e.Mobile.Target = new ExportItemPropsTarget();
        }

        private class ExportItemPropsTarget : Target
        {
            public ExportItemPropsTarget() : base(-1, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Item item)
                {
                    ItemProperties itemProps = new ItemProperties
                    {
                        ItemType = item.GetType().Name,
                        Properties = new Dictionary<string, string>()
                    };

                    foreach (var prop in item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        try
                        {
                            if (prop.CanRead)
                            {
                                var value = prop.GetValue(item);
                                if (value != null)
                                {
                                    itemProps.Properties[prop.Name] = value.ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the property that caused an issue
                            Console.WriteLine($"Error getting property {prop.Name}: {ex.Message}");
                        }
                    }

                    string baseName = $"ItemExport_{item.Serial}";
                    string xmlFileName = $"{baseName}.xml";
                    string scriptFileName = $"{baseName}.cs";
                    string xmlFilePath = Path.Combine(Core.BaseDirectory, "Exports", xmlFileName);
                    string scriptFilePath = Path.Combine(Core.BaseDirectory, "Exports", scriptFileName);

                    try
                    {
                        Directory.CreateDirectory(Path.Combine(Core.BaseDirectory, "Exports"));

                        // Export properties to a text file instead of XML
                        using (StreamWriter writer = new StreamWriter(xmlFilePath))
                        {
                            writer.WriteLine($"Item Type: {itemProps.ItemType}");
                            writer.WriteLine("Properties:");
                            foreach (var prop in itemProps.Properties)
                            {
                                writer.WriteLine($"{prop.Key}: {prop.Value}");
                            }
                        }

                        // Generate and export C# script
                        string script = GenerateCSharpScript(itemProps);
                        File.WriteAllText(scriptFilePath, script);

                        from.SendMessage($"Item properties exported to {xmlFileName} and C# script generated as {scriptFileName}");
                    }
                    catch (Exception ex)
                    {
                        from.SendMessage($"Error exporting item properties: {ex.Message}");
                        Console.WriteLine($"Detailed error: {ex}");
                    }
                }
                else
                {
                    from.SendMessage("That is not an item.");
                }
            }

            private string GenerateCSharpScript(ItemProperties itemProps)
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("using System;");
                sb.AppendLine("using Server;");
                sb.AppendLine("using Server.Items;");
                sb.AppendLine();
                sb.AppendLine($"namespace Server.Scripts.Items");
                sb.AppendLine("{");
                sb.AppendLine($"    public class Custom{itemProps.ItemType} : {itemProps.ItemType}");
                sb.AppendLine("    {");
                sb.AppendLine("        [Constructable]");
                sb.AppendLine($"        public Custom{itemProps.ItemType}()");
                sb.AppendLine("        {");

                foreach (var prop in itemProps.Properties)
                {
                    if (prop.Key != "Serial" && prop.Key != "Parent" && prop.Key != "Map")
                    {
                        sb.AppendLine($"            // {prop.Key} = {FormatPropertyValue(prop.Value)};");
                    }
                }

                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine($"        public Custom{itemProps.ItemType}(Serial serial) : base(serial)");
                sb.AppendLine("        {");
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        public override void Serialize(GenericWriter writer)");
                sb.AppendLine("        {");
                sb.AppendLine("            base.Serialize(writer);");
                sb.AppendLine("            writer.Write(0); // version");
                sb.AppendLine("        }");
                sb.AppendLine();
                sb.AppendLine("        public override void Deserialize(GenericReader reader)");
                sb.AppendLine("        {");
                sb.AppendLine("            base.Deserialize(reader);");
                sb.AppendLine("            int version = reader.ReadInt();");
                sb.AppendLine("        }");
                sb.AppendLine("    }");
                sb.AppendLine("}");

                return sb.ToString();
            }

            private string FormatPropertyValue(string value)
            {
                if (bool.TryParse(value, out bool boolResult))
                {
                    return boolResult.ToString().ToLower();
                }
                if (int.TryParse(value, out int intResult))
                {
                    return intResult.ToString();
                }
                if (double.TryParse(value, out double doubleResult))
                {
                    return doubleResult.ToString();
                }
                return $"\"{value}\"";
            }
        }
    }
}