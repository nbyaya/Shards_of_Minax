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
        private Mobile m_From;
        private const int MaxSkillCap = 150;

        public AdvancementGump(Mobile from) : base(50, 50)
        {
            m_From = from;

            Closable = true;
            Disposable = true;
            Dragable = true;
            Resizable = false;

            AddPage(0);
            AddBackground(0, 0, 1150, 450, 9200);

            AddHtml(10, 10, 580, 20, "<CENTER><B>Character Advancement</B></CENTER>", false, false);

            // Add close button
            AddButton(560, 10, 0x15E1, 0x15E5, 0, GumpButtonType.Reply, 0);

            AddHtml(10, 40, 200, 20, $"Total Skill Cap: {from.SkillsCap}", false, false);
            AddButton(220, 40, 4005, 4007, 1, GumpButtonType.Reply, 0);

            AddHtml(10, 70, 200, 20, $"Total Stat Cap: {from.StatCap}", false, false);
            AddButton(220, 70, 4005, 4007, 2, GumpButtonType.Reply, 0);

            AddHtml(10, 100, 200, 20, $"Max Followers: {from.FollowersMax}", false, false);
            AddButton(220, 100, 4005, 4007, 3, GumpButtonType.Reply, 0);

            AddHtml(300, 10, 290, 20, "<CENTER><B>Individual Skill Caps</B></CENTER>", false, false);

            int x = 300;
            int y = 40;
            int buttonId = 4;

            for (int i = 0; i < from.Skills.Length; i++)
            {
                Skill skill = from.Skills[i];
                if (skill != null)
                {
                    AddHtml(x, y, 150, 20, $"{skill.Name}: {skill.Cap}", false, false);
                    AddButton(x + 160, y, 4005, 4007, buttonId, GumpButtonType.Reply, 0);

                    y += 25;
                    buttonId++;

                    if (y > 400)
                    {
                        y = 40;
                        x += 200;
                    }
                }
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 0) // Close button
            {
                return; // Just close the gump
            }

            switch (info.ButtonID)
            {
                case 1: // Increase Total Skill Cap
                    if (from.SkillsCap < 5000)
                    {
                        if (ConsumeScrolls(from, 1))
                        {
                            from.SkillsCap += 1;
                            from.SendMessage("Your total skill cap has been increased by 1 point.");
                        }
                        else
                        {
                            from.SendMessage("You need 1 MaxxiaScroll to increase your total skill cap.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You have reached the maximum total skill cap of 5000.");
                    }
                    break;

                case 2: // Increase Stat Cap
                    if (from.StatCap < 200)
                    {
                        if (ConsumeScrolls(from, 5))
                        {
                            from.StatCap += 1;
                            from.SendMessage("Your stat cap has been increased by 1 point.");
                        }
                        else
                        {
                            from.SendMessage("You need 5 MaxxiaScrolls to increase your stat cap.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You have reached the maximum stat cap of 200.");
                    }
                    break;

                case 3: // Increase Max Followers
                    if (from.FollowersMax < 5)
                    {
                        if (ConsumeScrolls(from, 20))
                        {
                            from.FollowersMax += 1;
                            from.SendMessage("Your maximum followers has been increased by 1.");
                        }
                        else
                        {
                            from.SendMessage("You need 20 MaxxiaScrolls to increase your maximum followers.");
                        }
                    }
                    else
                    {
                        from.SendMessage("You have reached the maximum of 5 followers.");
                    }
                    break;

                default: // Individual Skill Cap Increase
                    if (info.ButtonID >= 4 && info.ButtonID < 4 + from.Skills.Length)
                    {
                        int skillIndex = info.ButtonID - 4;
                        Skill skill = from.Skills[skillIndex];

                        if (skill != null && skill.Cap < MaxSkillCap)
                        {
                            if (ConsumeScrolls(from, 1))
                            {
                                skill.Cap += 1;
                                from.SendMessage($"Your {skill.Name} skill cap has been increased by 1 point.");
                            }
                            else
                            {
                                from.SendMessage("You need 1 MaxxiaScroll to increase this skill cap.");
                            }
                        }
                        else if (skill != null && skill.Cap >= MaxSkillCap)
                        {
                            from.SendMessage($"You have reached the maximum cap of {MaxSkillCap} for {skill.Name}.");
                        }
                    }
                    break;
            }

            // Refresh the gump unless it's the close button
            if (info.ButtonID != 0)
            {
                from.SendGump(new AdvancementGump(from));
            }
        }

        private bool ConsumeScrolls(Mobile from, int amount)
        {
            int found = 0;
            List<Item> itemsToConsume = new List<Item>();

            // Collect all scrolls to be consumed first
            RecursiveSearchForScrolls(from.Backpack, amount, ref found, itemsToConsume);

            // If enough scrolls are found, consume them
            if (found >= amount)
            {
                int remaining = amount;

                foreach (Item item in itemsToConsume)
                {
                    if (item.Amount > remaining)
                    {
                        item.Consume(remaining);
                        remaining = 0;
                        break;
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
