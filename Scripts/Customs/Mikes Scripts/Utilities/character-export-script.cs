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

            // Added: Export StatCap, SkillsCap, FollowersMax
            writer.WriteElementString("StatCap", pm.StatCap.ToString());
            writer.WriteElementString("SkillsCap", pm.SkillsCap.ToString());
            writer.WriteElementString("FollowersMax", pm.FollowersMax.ToString());

            writer.WriteEndElement(); // End Stats

            // Export Skills
			// Export Skills
			writer.WriteStartElement("Skills");
			for (int i = 0; i < pm.Skills.Length; i++)
			{
				Skill skill = pm.Skills[i];
				if (skill != null)
				{
					writer.WriteStartElement("Skill");
					writer.WriteAttributeString("name", skill.SkillName.ToString()); // Use enum name
					writer.WriteAttributeString("value", skill.Base.ToString());
					writer.WriteAttributeString("cap", skill.Cap.ToString());
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement(); // End Skills


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
            writer.WriteEndElement(); // End Items

            writer.WriteEndElement(); // End Character
            writer.WriteEndDocument();
            writer.Close();

            from.SendMessage("Character data exported to {0}", fileName);
        }
    }
}
