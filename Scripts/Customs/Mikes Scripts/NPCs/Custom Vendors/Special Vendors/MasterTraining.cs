using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Gumps
{
    public class MasterTraining
    {
        public static void Initialize()
        {
            CommandSystem.Register("mastertraining", AccessLevel.Player, new CommandEventHandler(OpenMasterTraining_OnCommand));
        }

        [Usage("MasterTraining")]
        [Description("Opens the Master Training GUMP.")]
        public static void OpenMasterTraining_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from != null && from is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)from;
                from.CloseGump(typeof(MasterTrainingGump));
                from.SendGump(new MasterTrainingGump(pm, 0));
            }
        }
    }

    public class SkillScroll
    {
        public SkillName Skill { get; private set; }
        public int Level { get; private set; }
        public int Cost { get; private set; }
        public int Purchased { get; set; }

        public SkillScroll(SkillName skill, int level, int cost)
        {
            Skill = skill;
            Level = level;
            Cost = cost;
            Purchased = 0;
        }
    }

    public class MasterTrainingGump : Gump
    {
        private const int ItemsPerPage = 6;
        private PlayerMobile m_Owner;
        private int m_Page;

        private static List<SkillScroll> SkillScrolls = new List<SkillScroll>();

        static MasterTrainingGump()
        {
            // Add your skill scrolls here
            SkillScrolls.Add(new SkillScroll(SkillName.Throwing, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Anatomy, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.EvalInt, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Begging, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Tactics, 90, 5));
			SkillScrolls.Add(new SkillScroll(SkillName.Throwing, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Anatomy, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.EvalInt, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Begging, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Tactics, 90, 5));
			SkillScrolls.Add(new SkillScroll(SkillName.Throwing, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Anatomy, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.EvalInt, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Begging, 90, 5));
            SkillScrolls.Add(new SkillScroll(SkillName.Tactics, 90, 5));
            // Add more skills as needed
        }

        public MasterTrainingGump(PlayerMobile owner, int page)
            : base(50, 50)
        {
            m_Owner = owner;
            m_Page = page;

            AddPage(0);
            AddBackground(0, 0, 400, 400, 9270);
            AddAlphaRegion(10, 10, 380, 380);

            AddHtml(20, 20, 360, 20, "<CENTER><BASEFONT COLOR=#FFFFFF>Master Training</BASEFONT></CENTER>", false, false);

            int maxxiaScrollCount = owner.Backpack.GetAmount(typeof(MaxxiaScroll), true);
            AddHtml(20, 40, 360, 20, String.Format("<BASEFONT COLOR=#00FF00>You have: {0} Maxxia Scroll</BASEFONT>", maxxiaScrollCount), false, false);

            int pageCount = (SkillScrolls.Count + ItemsPerPage - 1) / ItemsPerPage;

            for (int i = 0; i < ItemsPerPage; ++i)
            {
                int index = (page * ItemsPerPage) + i;
                if (index >= SkillScrolls.Count)
                    break;

                SkillScroll scroll = SkillScrolls[index];

                int yOffset = 70 + (i * 50);

                AddItem(20, yOffset, 5357); // Scroll image
                AddHtml(60, yOffset, 150, 20, String.Format("<BASEFONT COLOR=#87CEEB>{0} {1}</BASEFONT>", scroll.Skill, scroll.Level), false, false);
                AddHtml(60, yOffset + 20, 150, 20, String.Format("<BASEFONT COLOR=#FFA500>Cost: {0}</BASEFONT>", scroll.Cost), false, false);
                AddHtml(160, yOffset + 20, 150, 20, String.Format("<BASEFONT COLOR=#FF0000>Purchased: {0}</BASEFONT>", scroll.Purchased), false, false);

                AddButton(320, yOffset + 10, 4005, 4006, 100 + index, GumpButtonType.Reply, 0); // Purchase button
            }

            if (page > 0)
                AddButton(350, 360, 4014, 4015, 1, GumpButtonType.Reply, 0); // Previous page

            if (page < pageCount - 1)
                AddButton(370, 360, 4005, 4006, 2, GumpButtonType.Reply, 0); // Next page
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 1: // Previous page
                    from.SendGump(new MasterTrainingGump(m_Owner, m_Page - 1));
                    break;
                case 2: // Next page
                    from.SendGump(new MasterTrainingGump(m_Owner, m_Page + 1));
                    break;
                default:
                    if (info.ButtonID >= 100)
                    {
                        int index = info.ButtonID - 100;
                        if (index >= 0 && index < SkillScrolls.Count)
                        {
                            SkillScroll scroll = SkillScrolls[index];
                            if (HasEnoughMaxxiaScrolls(from, scroll.Cost))
                            {
                                ConsumeMaxxiaScrolls(from, scroll.Cost);
                                GiveSkillScroll(from, scroll);
                                scroll.Purchased++;
                                from.SendMessage(String.Format("You have purchased a {0} {1} scroll.", scroll.Skill, scroll.Level));
                            }
                            else
                            {
                                from.SendMessage(String.Format("You don't have enough Maxxia Scrolls. You need {0} to purchase this skill scroll.", scroll.Cost));
                            }
                        }
                    }
                    from.SendGump(new MasterTrainingGump(m_Owner, m_Page));
                    break;
            }
        }

        private bool HasEnoughMaxxiaScrolls(Mobile from, int requiredAmount)
        {
            int count = from.Backpack.GetAmount(typeof(MaxxiaScroll), true);
            return count >= requiredAmount;
        }

        private void ConsumeMaxxiaScrolls(Mobile from, int amount)
        {
            from.Backpack.ConsumeTotal(typeof(MaxxiaScroll), amount);
        }

        private void GiveSkillScroll(Mobile from, SkillScroll scroll)
        {
            // This is a placeholder. You'll need to implement the actual skill scroll creation
            // based on your server's implementation of skill scrolls.
            Item skillScroll = new Item(0x14F0);
            skillScroll.Name = String.Format("{0} {1} Skill Scroll", scroll.Skill, scroll.Level);
            from.AddToBackpack(skillScroll);
        }
    }
}