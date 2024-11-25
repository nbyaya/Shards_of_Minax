using System;
using System.Xml;
using Server;
using Server.Commands;
using Server.Mobiles;
using Server.Targeting;
using Server.Items;

namespace Server.Scripts.Commands
{
    public class ImportCharacter
    {
        public static void Initialize()
        {
            CommandSystem.Register("ImportChar", AccessLevel.GameMaster, new CommandEventHandler(ImportChar_OnCommand));
        }

        [Usage("ImportChar <filename>")]
        [Description("Imports character data from an XML file to a targeted player.")]
        public static void ImportChar_OnCommand(CommandEventArgs e)
        {
            if (e.Length == 1)
            {
                Mobile from = e.Mobile;
                string fileName = e.GetString(0);

                from.SendMessage("Select the player to import the data to.");
                from.Target = new ImportCharacterTarget(fileName);
            }
            else
            {
                e.Mobile.SendMessage("Usage: ImportChar <filename>");
            }
        }

        private class ImportCharacterTarget : Target
        {
            private string m_FileName;

            public ImportCharacterTarget(string fileName) : base(-1, false, TargetFlags.None)
            {
                m_FileName = fileName;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is PlayerMobile)
                {
                    PlayerMobile pm = (PlayerMobile)targeted;
                    ImportCharacterData(from, pm, m_FileName);
                }
                else
                {
                    from.SendMessage("You must target a player.");
                }
            }
        }

        private static void ImportCharacterData(Mobile from, PlayerMobile target, string fileName)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                XmlNode characterNode = doc.SelectSingleNode("Character");
                if (characterNode == null)
                    throw new Exception("Invalid character data file.");

                // Import Stats
                XmlNode statsNode = characterNode.SelectSingleNode("Stats");
                if (statsNode != null)
                {
                    target.Str = Convert.ToInt32(statsNode.SelectSingleNode("Str").InnerText);
                    target.Dex = Convert.ToInt32(statsNode.SelectSingleNode("Dex").InnerText);
                    target.Int = Convert.ToInt32(statsNode.SelectSingleNode("Int").InnerText);
                    target.Hits = Convert.ToInt32(statsNode.SelectSingleNode("Hits").InnerText);
                    target.Mana = Convert.ToInt32(statsNode.SelectSingleNode("Mana").InnerText);
                    target.Stam = Convert.ToInt32(statsNode.SelectSingleNode("Stam").InnerText);

                    // Added: Import StatCap, SkillsCap, FollowersMax
                    XmlNode statCapNode = statsNode.SelectSingleNode("StatCap");
                    if (statCapNode != null)
                        target.StatCap = Convert.ToInt32(statCapNode.InnerText);

                    XmlNode skillsCapNode = statsNode.SelectSingleNode("SkillsCap");
                    if (skillsCapNode != null)
                        target.SkillsCap = Convert.ToInt32(skillsCapNode.InnerText);

                    XmlNode followersMaxNode = statsNode.SelectSingleNode("FollowersMax");
                    if (followersMaxNode != null)
                        target.FollowersMax = Convert.ToInt32(followersMaxNode.InnerText);
                }

                // Import Skills
				// Import Skills
				XmlNodeList skillNodes = characterNode.SelectNodes("Skills/Skill");
				foreach (XmlNode skillNode in skillNodes)
				{
					string skillName = skillNode.Attributes["name"].Value;
					double skillValue = Convert.ToDouble(skillNode.Attributes["value"].Value);
					double skillCap = 100.0; // Default skill cap

					if (skillNode.Attributes["cap"] != null)
						skillCap = Convert.ToDouble(skillNode.Attributes["cap"].Value);

					// Parse the skill name as an enum
					if (Enum.TryParse(skillName, true, out SkillName parsedSkillName))
					{
						Skill skill = target.Skills[parsedSkillName];
						if (skill != null)
						{
							skill.Base = skillValue;
							skill.Cap = skillCap;
						}
					}
					else
					{
						from.SendMessage($"Unrecognized skill: {skillName}");
					}
				}


                // Import Items
                XmlNodeList itemNodes = characterNode.SelectNodes("Items/Item");
                foreach (XmlNode itemNode in itemNodes)
                {
                    string itemType = itemNode.Attributes["type"].Value;
                    string itemName = itemNode.Attributes["name"].Value;
                    int itemAmount = Convert.ToInt32(itemNode.Attributes["amount"].Value);

                    Type type = ScriptCompiler.FindTypeByName(itemType);
                    if (type != null && type.IsSubclassOf(typeof(Item)))
                    {
                        try
                        {
                            // Try to create an instance with a parameterless constructor.
                            Item item = (Item)Activator.CreateInstance(type);
                            item.Name = itemName;
                            item.Amount = itemAmount;
                            target.AddToBackpack(item);
                        }
                        catch (MissingMethodException)
                        {
                            // If no parameterless constructor exists, use a fallback logic.
                            from.SendMessage($"Unable to create item of type {itemType}. No parameterless constructor available.");
                        }
                    }
                    else
                    {
                        from.SendMessage($"Invalid item type: {itemType}");
                    }
                }

                from.SendMessage("Character data imported successfully.");
                target.SendMessage("Your character has been updated with imported data.");
            }
            catch (Exception ex)
            {
                from.SendMessage("Error importing character data: " + ex.Message);
            }
        }
    }
}
