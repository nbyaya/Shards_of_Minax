using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Commands;

namespace Server.Stables
{
    public class VisualStablemaster
    {
        public static void Initialize()
        {
            CommandSystem.Register("openstable", AccessLevel.Player, new CommandEventHandler(OpenStable_OnCommand));
        }

        [Usage("OpenStable")]
        [Description("Opens a visual representation of the player's stabled animals.")]
        public static void OpenStable_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from != null && from is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)from;
                from.CloseGump(typeof(VisualStablemasterGump));
                from.SendGump(new VisualStablemasterGump(pm, 0));
            }
        }
    }

    public class VisualStablemasterGump : Gump
    {
        private const int ItemsPerPage = 6;
        private PlayerMobile m_Owner;
        private int m_Page;

        public VisualStablemasterGump(PlayerMobile owner, int page) : base(50, 50)
        {
            m_Owner = owner;
            m_Page = page;

            AddPage(0);
            AddBackground(0, 0, 600, 400, 9270);
            AddAlphaRegion(10, 10, 580, 380);

            AddHtml(20, 20, 560, 20, "<CENTER><BASEFONT COLOR=#FFFFFF>Stablemaster</BASEFONT></CENTER>", false, false);

            List<Mobile> stabled = owner.Stabled;
            int pageCount = (stabled.Count + ItemsPerPage - 1) / ItemsPerPage;

            if (page > 0)
                AddButton(550, 20, 4014, 4016, 1, GumpButtonType.Reply, 0); // Previous page

            if (page < pageCount - 1)
                AddButton(575, 20, 4005, 4007, 2, GumpButtonType.Reply, 0); // Next page

            AddHtml(20, 360, 560, 20, String.Format("<CENTER><BASEFONT COLOR=#FFFFFF>Page {0}/{1}</BASEFONT></CENTER>", page + 1, pageCount), false, false);

            for (int i = 0; i < ItemsPerPage; ++i)
            {
                int index = (page * ItemsPerPage) + i;
                if (index >= stabled.Count)
                    break;

                Mobile pet = stabled[index];
                if (pet == null || pet.Deleted)
                    continue;

                int xOffset = (i % 3) * 190;
                int yOffset = (i / 3) * 180;

                AddImageTiled(20 + xOffset, 50 + yOffset, 180, 170, 2624);
                AddAlphaRegion(25 + xOffset, 55 + yOffset, 170, 160);

                AddItem(70 + xOffset, 100 + yOffset, GetItemIDForCreature(pet));
                AddHtml(25 + xOffset, 55 + yOffset, 170, 20, String.Format("<CENTER><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></CENTER>", pet.Name), false, false);
                AddHtml(25 + xOffset, 180 + yOffset, 170, 20, String.Format("<CENTER><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></CENTER>", pet.GetType().Name), false, false);

                AddButton(155 + xOffset, 185 + yOffset, 4005, 4007, 100 + index, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 1: // Previous page
                    from.SendGump(new VisualStablemasterGump(m_Owner, m_Page - 1));
                    break;
                case 2: // Next page
                    from.SendGump(new VisualStablemasterGump(m_Owner, m_Page + 1));
                    break;
                default:
                    if (info.ButtonID >= 100)
                    {
                        int index = info.ButtonID - 100;
                        if (index >= 0 && index < m_Owner.Stabled.Count)
                        {
                            Mobile pet = m_Owner.Stabled[index];
                            if (pet != null && !pet.Deleted && pet is BaseCreature)
                            {
                                BaseCreature creature = (BaseCreature)pet;
                                creature.SetControlMaster(m_Owner);
                                creature.ControlTarget = m_Owner;
                                creature.ControlOrder = OrderType.Follow;
                                creature.MoveToWorld(m_Owner.Location, m_Owner.Map);
                                creature.IsStabled = false;
                                m_Owner.Stabled.RemoveAt(index);
                                from.SendMessage("You have claimed {0}.", creature.Name);
                            }
                        }
                        from.SendGump(new VisualStablemasterGump(m_Owner, m_Page));
                    }
                    break;
            }
        }

        private int GetItemIDForCreature(Mobile creature)
        {
            // This method should return an appropriate item ID for each creature type
            // You'll need to expand this based on your specific creature types
            if (creature is Horse)
                return 8480;
            if (creature is Dog)
                return 8476;
            if (creature is Cat)
                return 8475;
			if (creature is Cow)
                return 8451;
            // Add more creature types as needed
            return 8413; // Default to cat icon if unknown
        }
    }
}