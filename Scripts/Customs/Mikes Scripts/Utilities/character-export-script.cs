using System;
using System.Xml;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Items;

namespace Server.Scripts.Commands
{
    public class ExportCharacter
    {
        public static void Initialize()
        {
            CommandSystem.Register("ExportChar", AccessLevel.Player, new CommandEventHandler(ExportChar_OnCommand));
        }

        [Usage("ExportChar")]
        [Description("Exports your character's stats and items to an XML file.")]
        public static void ExportChar_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from == null || !(from is PlayerMobile))
                return;

            PlayerMobile pm = (PlayerMobile)from;
            string fileName = string.Format("{0}_{1}.xml", pm.Name, DateTime.Now.ToString("yyyyMMddHHmmss"));

            XmlTextWriter writer = new XmlTextWriter(fileName, System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument(true);
            writer.WriteStartElement("Character");

            // Export Stats
            writer.WriteStartElement("Stats");
            writer.WriteElementString("Name", pm.Name);
            writer.WriteElementString("Str", pm.Str.ToString());
            writer.WriteElementString("Dex", pm.Dex.ToString());
            writer.WriteElementString("Int", pm.Int.ToString());
            writer.WriteElementString("Hits", pm.Hits.ToString());
            writer.WriteElementString("Mana", pm.Mana.ToString());
            writer.WriteElementString("Stam", pm.Stam.ToString());
            writer.WriteEndElement();

            // Export Skills
            writer.WriteStartElement("Skills");
            for (int i = 0; i < pm.Skills.Length; i++)
            {
                Skill skill = pm.Skills[i];
                if (skill.Base > 0)
                {
                    writer.WriteStartElement("Skill");
                    writer.WriteAttributeString("name", skill.Name);
                    writer.WriteAttributeString("value", skill.Base.ToString());
                    writer.WriteEndElement();
                }
            }
            writer.WriteEndElement();

            // Export Items
            writer.WriteStartElement("Items");
            foreach (Item item in pm.Items)
            {
                writer.WriteStartElement("Item");
                writer.WriteAttributeString("type", item.GetType().Name);
                writer.WriteAttributeString("name", item.Name);
                writer.WriteAttributeString("amount", item.Amount.ToString());
                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement(); // Character
            writer.WriteEndDocument();
            writer.Close();

            from.SendMessage("Character data exported to {0}", fileName);
        }
    }
}
