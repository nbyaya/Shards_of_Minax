using System;
using System.Collections.Generic;
using System.Linq;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Commands;
using Server.Items;
using Server.Mobiles;

namespace Server.Gumps
{
    public class AdvancementGump : Gump
    {
        private const int MaxSkillCap = 200;
        private const int MaxTotalSkillCap = 10000;
        private const int MaxStatCap = 300;
        private const int MaxFollowers = 10;
        private const int SkillsPerPage = 28; // Adjusted for double skills per row and tighter Y-axis spacing

        private Mobile m_From;
        private List<Skill> sortedSkills;
        private int m_CurrentPage;
        private Dictionary<int, int> skillIndexByButtonId = new Dictionary<int, int>();

        public AdvancementGump(Mobile from, int currentPage = 0)
            : base(50, 50)
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

            // Bottom Panel Information Labels
            AddLabel(860, 720, 0, "Total Skill Cap");
            AddLabel(860, 760, 0, "Total Stat Cap");
            AddLabel(860, 800, 0, "Max Followers");
            AddLabel(860, 740, 0, "Total Current Skill");
            AddLabel(860, 780, 0, "Total Current Stat");

            // Retrieve the player's Talent profile.
            var player = m_From as PlayerMobile;
            var profile = player?.AcquireTalents();

            // Get Ancient Knowledge (or create a temporary one if none exists)
            Talent ancientKnowledge = null;
            if (profile?.Talents.TryGetValue(TalentID.AncientKnowledge, out ancientKnowledge) != true)
            {
                ancientKnowledge = new Talent(TalentID.AncientKnowledge);
            }
            int ancientKnowledgePoints = ancientKnowledge.Points;

            // Display Ancient Knowledge points
            AddLabel(860, 820, 0, "Maxxia Points:");
            AddLabel(1030, 820, 0, $"{ancientKnowledgePoints}");

            // NEW: Display current Level and XP information.
            int currentLevel = profile?.Level ?? 1;
            int currentXP = profile?.XP ?? 0;
            AddLabel(860, 680, 0, $"Level: {currentLevel}");
            int xpForNext = Talents.GetXPThresholdForLevel(currentLevel + 1);
            AddLabel(860, 700, 0, $"XP: {currentXP}/{xpForNext}");



            // Bottom Panel Values
            AddLabel(1030, 720, 0, $"{(int)(m_From.SkillsCap / 10)}");
            AddLabel(1030, 760, 0, $"{m_From.StatCap}");
            AddLabel(1030, 800, 0, $"{m_From.FollowersMax}");
            AddLabel(1030, 740, 0, $"{CalculateTotalCurrentSkill(m_From)}");
            AddLabel(1030, 780, 0, $"{CalculateTotalCurrentStat(m_From)}");

            // Buttons for increasing values
            AddButton(1000, 720, 250, 251, 2000, GumpButtonType.Reply, 0); // Increase Total Skill Cap
            AddButton(1000, 760, 250, 251, 2001, GumpButtonType.Reply, 0); // Increase Total Stat Cap
            AddButton(1000, 800, 250, 251, 2002, GumpButtonType.Reply, 0); // Increase Max Followers

            // Skill Cap and Value Section
            int x1 = 865;
            int x2 = 1065; // Second column for skills
            int y = 244;
            int buttonX1 = 836;
            int buttonX2 = 1036; // Second column button X
            int buttonId = 3; // Start button ID (after navigation buttons)

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

                y += 28; // Adjust row spacing

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

                y += 28; // Space for next set of rows
            }

            // Navigation Buttons
            if (m_CurrentPage > 0)
                AddButton(837, 641, 4014, 4015, 1000, GumpButtonType.Reply, 0); // Previous page

            if (m_CurrentPage < totalPages - 1)
                AddButton(878, 640, 4005, 4006, 1001, GumpButtonType.Reply, 0); // Next page
        }

        private string AbbreviateSkillDisplay(string skillName, double currentValue, double skillCap)
        {
            string valuePortion = $": {currentValue}/{skillCap}";
            int maxLineLength = 18;
            int availableNameLength = maxLineLength - valuePortion.Length;

            if (availableNameLength <= 0)
                return valuePortion; // No space for name—only show value

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
                    break;
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
            var player = from as PlayerMobile;
            if (player == null)
                return;

            // First, handle navigation buttons so that scrolling works regardless of talent points.
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

            // Now retrieve the player's talent profile.
            var profile = player.AcquireTalents();
            // Ensure the Ancient Knowledge talent exists.
            if (!profile.Talents.TryGetValue(TalentID.AncientKnowledge, out var ancientKnowledge))
            {
                ancientKnowledge = new Talent(TalentID.AncientKnowledge);
                profile.Talents[TalentID.AncientKnowledge] = ancientKnowledge;
            }

            // Handle individual skill cap increases
            if (skillIndexByButtonId.TryGetValue(info.ButtonID, out int skillIndex))
            {
                Skill skill = sortedSkills[skillIndex];

                if (skill.Cap < MaxSkillCap && ConsumeTalentPoints(ancientKnowledge, 1))
                {
                    skill.Cap += 1;
                    if (skill.Cap > MaxSkillCap)
                        skill.Cap = MaxSkillCap;

                    from.SendMessage($"Your {skill.Name} skill cap has been increased.");
                }
                else if (skill.Cap >= MaxSkillCap)
                {
                    from.SendMessage($"{skill.Name} reached the 200 soft cap. Hunt power scrolls to increase more.");
                }
                else
                {
                    from.SendMessage($"You need 1 Maxxia Point to increase the skill cap for {skill.Name} by 1.");
                }
            }

            // Handle other advancement options
            switch (info.ButtonID)
            {
                case 2000: // Increase Total Skill Cap
                    if (from.SkillsCap < MaxTotalSkillCap && ConsumeTalentPoints(ancientKnowledge, 2))
                    {
                        from.SkillsCap += 50;
                        if (from.SkillsCap > MaxTotalSkillCap)
                            from.SkillsCap = MaxTotalSkillCap;
                        from.SendMessage("Your total skill cap has been increased by 5.");
                    }
                    else if (from.SkillsCap >= MaxTotalSkillCap)
                    {
                        from.SendMessage("You've reached the soft cap of 500 skill points. Visit the Royal Steward at the Throne Room and buy Royal Skil Charters to further increase the cap.");
                    }
                    else
                    {
                        from.SendMessage("You need 2 Maxxia Points to increase your total skill cap by 5.");
                    }
                    break;

                case 2001: // Increase Total Stat Cap
                    if (from.StatCap < MaxStatCap && ConsumeTalentPoints(ancientKnowledge, 3))
                    {
                        from.StatCap += 1;
                        if (from.StatCap > MaxStatCap)
                            from.StatCap = MaxStatCap;
                        from.SendMessage("Your stat cap has been increased by 1.");
                    }
                    else if (from.StatCap >= MaxStatCap)
                    {
                        from.SendMessage("You've reached the soft cap of 200 stat points. Visit the Royal Steward at the Throne Room and buy Royal Stat Charters to further increase the cap.");
                    }
                    else
                    {
                        from.SendMessage("You need 3 Maxxia Points to increase your total stat cap by 1.");
                    }
                    break;

                case 2002: // Increase Max Followers
                    if (from.FollowersMax < MaxFollowers && ConsumeTalentPoints(ancientKnowledge, 20))
                    {
                        from.FollowersMax += 1;
                        if (from.FollowersMax > MaxFollowers)
                            from.FollowersMax = MaxFollowers;
                        from.SendMessage("Your maximum followers have been increased.");
                    }
                    else if (from.FollowersMax >= MaxFollowers)
                    {
                        from.SendMessage("You've reached the soft cap of 5 Followers Slots. Visit the Royal Steward at the Throne Room and buy Pet Slot Deeds to further increase the cap.");
                    }
                    else
                    {
                        from.SendMessage("You need 20 Maxxia Points to increase your Follower Slots by 1.");
                    }
                    break;
            }

            // Refresh the gump
            from.SendGump(new AdvancementGump(from, m_CurrentPage));
        }

        private bool ConsumeTalentPoints(Talent ancientKnowledge, int requiredPoints)
        {
            if (ancientKnowledge.Points >= requiredPoints)
            {
                ancientKnowledge.Points -= requiredPoints;
                return true;
            }
            else
            {
                m_From.SendMessage($"You need {requiredPoints} Maxxia Points talent points to perform this action.");
                return false;
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
            from.CloseGump(typeof(AdvancementGump));
            from.SendGump(new AdvancementGump(from));
        }
    }
}
