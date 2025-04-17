using System;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Items;
using System.Collections.Generic;

namespace Server.Gumps
{
    public class AdvancementGump : Gump
    {
        private const int MaxSkillCap = 150;
        private const int MaxTotalSkillCap = 5000;
        private const int MaxStatCap = 200;
        private const int MaxFollowers = 5;
        private const int SkillsPerPage = 28; // Adjusted for double skills per row and tighter Y-axis spacing

        private Mobile m_From;
        private List<Skill> sortedSkills;
        private int m_CurrentPage;
        private Dictionary<int, int> skillIndexByButtonId = new Dictionary<int, int>();

        public AdvancementGump(Mobile from, int currentPage = 0) : base(50, 50)
        {
            m_From = from;
            m_CurrentPage = currentPage;

            sortedSkills = new List<Skill>(from.Skills);
            sortedSkills.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

            int totalPages = (int)Math.Ceiling(sortedSkills.Count / (double)SkillsPerPage);

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddImage(723, 193, 30236, 0);

            // Bottom Panel Information
            AddLabel(860, 720, 0, "Total Skill Cap");
            AddLabel(860, 760, 0, "Total Stat Cap");
            AddLabel(860, 800, 0, "Max Followers");
            AddLabel(860, 740, 0, "Total Current Skill");
            AddLabel(860, 780, 0, "Total Current Stat");

            AddHtml(10, 10, 580, 20, "<CENTER><B>Character Advancement</B></CENTER>", false, false);

            // Bottom Panel Values
            AddLabel(1030, 720, 0, $"{(int)(from.SkillsCap / 10)}");
            AddLabel(1030, 760, 0, $"{from.StatCap}");
            AddLabel(1030, 800, 0, $"{from.FollowersMax}");
            AddLabel(1030, 740, 0, $"{CalculateTotalCurrentSkill(from)}");
            AddLabel(1030, 780, 0, $"{CalculateTotalCurrentStat(from)}");

            // Add Buttons for Increasing Values
            AddButton(1000, 720, 250, 251, 2000, GumpButtonType.Reply, 0); // Increase Total Skill Cap
            AddButton(1000, 760, 250, 251, 2001, GumpButtonType.Reply, 0); // Increase Total Stat Cap
            AddButton(1000, 800, 250, 251, 2002, GumpButtonType.Reply, 0); // Increase Max Followers

            // Skill Cap and Value Section
            int x1 = 865;
            int x2 = 1065; // Second column for skills
            int y = 244;
            int buttonX1 = 836;
            int buttonX2 = 1036; // Second column button
            int buttonId = 3; // Start button ID after navigation buttons

            int startIndex = m_CurrentPage * SkillsPerPage;
            for (int i = startIndex; i < sortedSkills.Count && i < startIndex + SkillsPerPage; i += 4)
            {
                Skill skill1 = sortedSkills[i];
                Skill skill2 = (i + 1 < sortedSkills.Count) ? sortedSkills[i + 1] : null;
                Skill skill3 = (i + 2 < sortedSkills.Count) ? sortedSkills[i + 2] : null;
                Skill skill4 = (i + 3 < sortedSkills.Count) ? sortedSkills[i + 3] : null;

                if (skill1 != null)
                {
                    AddLabel(x1, y, 0, $"{AbbreviateSkillDisplay(skill1.Name, skill1.Base, skill1.Cap)}");
                    AddButton(buttonX1, y, 250, 251, buttonId, GumpButtonType.Reply, 0);
                    skillIndexByButtonId[buttonId] = i; // Map button ID to skill index
                    buttonId++;
                }

                if (skill2 != null)
                {
                    AddLabel(x2, y, 0, $"{AbbreviateSkillDisplay(skill2.Name, skill2.Base, skill2.Cap)}");
                    AddButton(buttonX2, y, 250, 251, buttonId, GumpButtonType.Reply, 0);
                    skillIndexByButtonId[buttonId] = i + 1; // Map button ID to skill index
                    buttonId++;
                }

                y += 28; // Half the original row spacing to fit more rows

                if (skill3 != null)
                {
                    AddLabel(x1, y, 0, $"{AbbreviateSkillDisplay(skill3.Name, skill3.Base, skill3.Cap)}");
                    AddButton(buttonX1, y, 250, 251, buttonId, GumpButtonType.Reply, 0);
                    skillIndexByButtonId[buttonId] = i + 2; // Map button ID to skill index
                    buttonId++;
                }

                if (skill4 != null)
                {
                    AddLabel(x2, y, 0, $"{AbbreviateSkillDisplay(skill4.Name, skill4.Base, skill4.Cap)}");
                    AddButton(buttonX2, y, 250, 251, buttonId, GumpButtonType.Reply, 0);
                    skillIndexByButtonId[buttonId] = i + 3; // Map button ID to skill index
                    buttonId++;
                }

                y += 28; // Adjust for the next set of rows
            }

            // Navigation Buttons
            if (m_CurrentPage > 0)
                AddButton(837, 661, 4014, 4015, 1000, GumpButtonType.Reply, 0); // Previous page

            if (m_CurrentPage < totalPages - 1)
                AddButton(878, 660, 4005, 4006, 1001, GumpButtonType.Reply, 0); // Next page
        }

        private string AbbreviateSkillDisplay(string skillName, double currentValue, double skillCap)
        {
            string valuePortion = $": {currentValue}/{skillCap}";
            int maxLineLength = 18;
            int availableNameLength = maxLineLength - valuePortion.Length;

            if (availableNameLength <= 0)
                return valuePortion; // If there's no space for the name, only show the value

            string[] words = skillName.Split(' ');
            int totalLength = 0;

            for (int i = 0; i < words.Length; i++)
            {
                int remainingLength = availableNameLength - totalLength;

                if (remainingLength > 0 && words[i].Length > remainingLength / (words.Length - i))
                {
                    words[i] = words[i].Substring(0, Math.Max(remainingLength / (words.Length - i), 1));
                }

                totalLength += words[i].Length;

                if (totalLength >= availableNameLength)
                {
                    break;
                }
            }

            return string.Join(" ", words) + valuePortion;
        }

        private int CalculateTotalCurrentSkill(Mobile from)
        {
            int total = 0;
            foreach (Skill skill in from.Skills)
            {
                total += (int)skill.Base;
            }
            return total;
        }

        private int CalculateTotalCurrentStat(Mobile from)
        {
            return from.Str + from.Dex + from.Int;
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 0) // Close button
                return;

            if (info.ButtonID == 1000) // Previous page
            {
                from.SendGump(new AdvancementGump(from, m_CurrentPage - 1));
                return;
            }

            if (info.ButtonID == 1001) // Next page
            {
                from.SendGump(new AdvancementGump(from, m_CurrentPage + 1));
                return;
            }

            // Handle skill adjustment
            if (skillIndexByButtonId.TryGetValue(info.ButtonID, out int skillIndex))
            {
                Skill skill = sortedSkills[skillIndex];

                if (skill.Cap < MaxSkillCap && ConsumeScrolls(from, 1))
                {
                    skill.Cap += 1;
                    if (skill.Cap > MaxSkillCap)
                        skill.Cap = MaxSkillCap;

                    from.SendMessage($"Your {skill.Name} skill cap has been increased.");
                }
                else
                {
                    from.SendMessage($"You need 1 MaxxiaScroll to increase the skill cap for {skill.Name}, or it has reached the maximum cap.");
                }
            }

            switch (info.ButtonID)
            {
                case 2000: // Increase Total Skill Cap
                    if (from.SkillsCap < MaxTotalSkillCap && ConsumeScrolls(from, 1))
                    {
                        from.SkillsCap += 10;
                        if (from.SkillsCap > MaxTotalSkillCap)
                            from.SkillsCap = MaxTotalSkillCap;
                        from.SendMessage("Your total skill cap has been increased.");
                    }
                    else if (from.SkillsCap >= MaxTotalSkillCap)
                    {
                        from.SendMessage("You've reached the soft cap of 500 skill points. Visit the Royal Steward at the Throne Room and buy Skill Orbs to fruther increase the cap.");
                    }
                    else
                    {
                        from.SendMessage("You need 1 MaxxiaScroll to increase your total skill cap.");
                    }
                    break;

                case 2001: // Increase Total Stat Cap
                    if (from.StatCap < MaxStatCap && ConsumeScrolls(from, 5))
                    {
                        from.StatCap += 1;
                        if (from.StatCap > MaxStatCap)
                            from.StatCap = MaxStatCap;
                        from.SendMessage("Your stat cap has been increased.");
                    }
                    else if (from.StatCap >= MaxStatCap)
                    {
                        from.SendMessage("You've reached the soft cap of 200 stat points. Visit the Royal Steward at the Throne Room and buy Stat Orbs to fruther increase the cap.");
                    }
                    else
                    {
                        from.SendMessage("You need 5 MaxxiaScroll to increase your total stat cap.");
                    }
                    break;

                case 2002: // Increase Max Followers
                    if (from.FollowersMax < MaxFollowers && ConsumeScrolls(from, 20))
                    {
                        from.FollowersMax += 1;
                        if (from.FollowersMax > MaxFollowers)
                            from.FollowersMax = MaxFollowers;
                        from.SendMessage("Your maximum followers have been increased.");
                    }
                    else if (from.FollowersMax >= MaxFollowers)
                    {
                        from.SendMessage("You've reached the soft cap of 5 Followers Slots. Visit the Royal Steward at the Throne Room and buy Pet Slot Deeds to fruther increase the cap.");
                    }
                    else
                    {
                        from.SendMessage("You need 20 MaxxiaScroll to increase your Follower Slots.");
                    }
                    break;
            }

            // Refresh the gump
            from.SendGump(new AdvancementGump(from, m_CurrentPage));
        }

        private bool ConsumeScrolls(Mobile from, int amount)
        {
            int found = 0;
            List<Item> itemsToConsume = new List<Item>();
            RecursiveSearchForScrolls(from.Backpack, amount, ref found, itemsToConsume);

            if (found >= amount)
            {
                int remaining = amount;
                foreach (Item item in itemsToConsume)
                {
                    if (item.Amount > remaining)
                    {
                        item.Consume(remaining);
                        return true;
                    }
                    else
                    {
                        remaining -= item.Amount;
                        item.Delete();
                    }
                }

                return true;
            }

            return false;
        }

        private void RecursiveSearchForScrolls(Container container, int amountNeeded, ref int found, List<Item> itemsToConsume)
        {
            if (container == null)
                return;

            foreach (Item item in container.Items)
            {
                if (item is MaxxiaScroll)
                {
                    itemsToConsume.Add(item);
                    found += item.Amount;

                    if (found >= amountNeeded)
                        return;
                }
                else if (item is Container)
                {
                    RecursiveSearchForScrolls((Container)item, amountNeeded, ref found, itemsToConsume);

                    if (found >= amountNeeded)
                        return;
                }
            }
        }
    }

    public class AdvancementCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("advancement", AccessLevel.Player, new CommandEventHandler(Advancement_OnCommand));
        }

        [Usage("advancement")]
        [Description("Opens the Character Advancement gump.")]
        public static void Advancement_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            from.SendGump(new AdvancementGump(from));
        }
    }
}
