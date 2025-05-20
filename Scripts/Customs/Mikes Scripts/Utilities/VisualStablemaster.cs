using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Targeting;
using Server.Commands;
using Server.Items;

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
            if (from is PlayerMobile pm)
            {
                from.CloseGump(typeof(VisualStablemasterGump));
                from.SendGump(new VisualStablemasterGump(pm, 0));
            }
        }
    }

    public class VisualStablemasterGump : Gump
    {
        private const int ItemsPerPage   = 6;
        private const int StableButtonID = 500;

        private PlayerMobile m_Owner;
        private int           m_Page;

        public VisualStablemasterGump(PlayerMobile owner, int page)
            : base(50, 50)
        {
            m_Owner = owner;
            m_Page  = page;

            Closable   = true;
            Disposable = true;
            Dragable   = true;

            AddPage(0);
            AddBackground(0, 0, 600, 650, 9270);
            AddAlphaRegion(10, 10, 580, 630);

            // Title
            AddHtml(20, 20, 400, 25,
                "<CENTER><BASEFONT COLOR=#FFFFFF><BIG>Stablemaster</BIG></BASEFONT></CENTER>",
                false, false);

            // --- Stable Pet button ---
            AddButton(430, 20, 4005, 4007, StableButtonID, GumpButtonType.Reply, 0);
            AddHtml(465, 20, 100, 25,
                "<BASEFONT COLOR=#FFFFFF>Stable Pet</BASEFONT>",
                false, false);
            // -----------------------------

            // Page counter
            List<Mobile> stabled   = owner.Stabled;
            int          pageCount = Math.Max(1, (stabled.Count + ItemsPerPage - 1) / ItemsPerPage);
            AddHtml(250, 45, 100, 25,
                String.Format("<CENTER><BASEFONT COLOR=#FFFF00><BIG>Page {0}/{1}</BIG></BASEFONT></CENTER>",
                              page + 1, pageCount),
                false, false);

            // Prev / Next
            if (page > 0)
                AddButton(530, 20, 4014, 4016, 1, GumpButtonType.Reply, 0);
            if (page < pageCount - 1)
                AddButton(560, 20, 4005, 4007, 2, GumpButtonType.Reply, 0);

            // List stabled pets
            int panelHeight = 90;
            int startY      = 60;

            for (int i = 0; i < ItemsPerPage; ++i)
            {
                int index = (page * ItemsPerPage) + i;
                if (index >= stabled.Count)
                    break;

                Mobile pet = stabled[index];
                if (pet == null || pet.Deleted)
                    continue;

                int panelX = 20;
                int panelY = startY + (i * (panelHeight + 5));
                AddImageTiled(panelX, panelY, 560, panelHeight, 2624);
                AddAlphaRegion(panelX + 5, panelY + 5, 550, panelHeight - 10);

                // Pet image
                AddItem(panelX + 10, panelY + 10, GetItemIDForCreature(pet));

                // Pet info
                int textX      = panelX + 60;
                int textWidth  = 340;
                string petName = pet.Name;
                string petType = pet.GetType().Name;
                string tame    = "N/A";
                string slots   = "N/A";

                if (pet is BaseCreature bc)
                {
                    tame  = bc.MinTameSkill.ToString("F1");
                    slots = bc.ControlSlots.ToString();
                }

                AddHtml(textX, panelY + 10,  textWidth, 18, $"<BASEFONT COLOR=#FFFFFF>Name: {petName}</BASEFONT>", false, false);
                AddHtml(textX, panelY + 30,  textWidth, 18, $"<BASEFONT COLOR=#FFFFFF>Type: {petType}</BASEFONT>", false, false);
                AddHtml(textX, panelY + 50,  textWidth/2, 18, $"<BASEFONT COLOR=#FFFFFF>Tame: {tame}</BASEFONT>", false, false);
                AddHtml(textX + textWidth/2, panelY + 50, textWidth/2, 18, $"<BASEFONT COLOR=#FFFFFF>Slots: {slots}</BASEFONT>", false, false);

                // Claim / Abandon / Stats buttons
                int btnX = panelX + 420;
                int btnY = panelY + 10;

                AddButton(btnX,       btnY,     4005, 4007, 100 + index, GumpButtonType.Reply, 0);
                AddHtml(btnX + 35,    btnY,     50,    20, "<BASEFONT COLOR=#FFFFFF>Claim</BASEFONT>", false, false);

                AddButton(btnX,       btnY + 25,4005, 4007, 200 + index, GumpButtonType.Reply, 0);
                AddHtml(btnX + 35,    btnY + 25,50,    20, "<BASEFONT COLOR=#FFFFFF>Abandon</BASEFONT>", false, false);

                AddButton(btnX,       btnY + 50,4005, 4007, 300 + index, GumpButtonType.Reply, 0);
                AddHtml(btnX + 35,    btnY + 50,70,    20, "<BASEFONT COLOR=#FFFFFF>Stats</BASEFONT>", false, false);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            if (info.ButtonID == 0)
                return;

            // Stable Pet button
            if (info.ButtonID == StableButtonID)
            {
                from.SendGump(new VisualStablemasterGump(m_Owner, m_Page));
                from.Target = new StablePetTarget(m_Owner);
                from.SendMessage("Who would you like to stable?");
                return;
            }

            // Navigation
            if (info.ButtonID == 1)
            {
                from.SendGump(new VisualStablemasterGump(m_Owner, m_Page - 1));
                return;
            }
            if (info.ButtonID == 2)
            {
                from.SendGump(new VisualStablemasterGump(m_Owner, m_Page + 1));
                return;
            }

            // Claim (100–199), Abandon (200–299), Stats (300–399)
            var stabled = m_Owner.Stabled;
            int btn    = info.ButtonID;

            if (btn >= 100 && btn < 200)
            {
                int idx = btn - 100;
                if (idx >= 0 && idx < stabled.Count && stabled[idx] is BaseCreature c1)
                {
                    c1.SetControlMaster(m_Owner);
                    c1.ControlTarget = m_Owner;
                    c1.ControlOrder  = OrderType.Follow;
                    c1.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    c1.IsStabled    = false;
                    stabled.RemoveAt(idx);
                    from.SendMessage("You have claimed {0}.", c1.Name);
                }
            }
            else if (btn >= 200 && btn < 300)
            {
                int idx = btn - 200;
                if (idx >= 0 && idx < stabled.Count)
                {
                    var mob = stabled[idx];
                    stabled.RemoveAt(idx);
                    mob.Delete();
                    from.SendMessage("You have deleted {0} from your stable.", mob.Name);
                }
            }
            else if (btn >= 300 && btn < 400)
            {
                int idx = btn - 300;
                if (idx >= 0 && idx < stabled.Count)
                {
                    var pet = stabled[idx];
                    from.CloseGump(typeof(VisualStablemasterStatsGump));
                    from.SendGump(new VisualStablemasterStatsGump(m_Owner, pet));
                    return;
                }
            }

            // Refresh
            from.SendGump(new VisualStablemasterGump(m_Owner, m_Page));
        }

        private int GetItemIDForCreature(Mobile creature)
        {
            if (creature is Horse) return 8480;
            if (creature is Dog)   return 8476;
            if (creature is Cat)   return 8475;
            if (creature is Cow)   return 8451;
            // etc.
            return 8413;
        }
    }

    public class VisualStablemasterStatsGump : Gump
    {
        private PlayerMobile m_Owner;
        private Mobile       m_Pet;

        public VisualStablemasterStatsGump(PlayerMobile owner, Mobile pet)
            : base(100, 100)
        {
            m_Owner = owner;
            m_Pet   = pet;

            AddPage(0);
            AddBackground(0, 0, 400, 300, 9270);
            AddAlphaRegion(10, 10, 380, 280);

            AddHtml(20, 20, 360, 25,
                String.Format("<CENTER><BASEFONT COLOR=#FFFFFF><BIG>{0}'s Stat Sheet</BIG></BASEFONT></CENTER>", pet.Name),
                false, false);

            int y = 60;
            AddHtml(20, y, 360, 20, $"<BASEFONT COLOR=#FFFFFF>Type: {pet.GetType().Name}</BASEFONT>", false, false);
            y += 25;

            if (pet is BaseCreature c)
            {
                AddHtml(20, y, 360, 20, $"<BASEFONT COLOR=#FFFFFF>Minimum Tame Skill: {c.MinTameSkill:F1}</BASEFONT>", false, false); y += 25;
                AddHtml(20, y, 360, 20, $"<BASEFONT COLOR=#FFFFFF>Control Slots: {c.ControlSlots}</BASEFONT>", false, false);     y += 25;
                AddHtml(20, y, 360, 20, $"<BASEFONT COLOR=#FFFFFF>Hits: {c.Hits}/{c.HitsMax}</BASEFONT>", false, false);        y += 25;
                AddHtml(20, y, 360, 20, $"<BASEFONT COLOR=#FFFFFF>Stamina: {c.Stam}/{c.StamMax}</BASEFONT>", false, false);     y += 25;
                AddHtml(20, y, 360, 20, $"<BASEFONT COLOR=#FFFFFF>Mana: {c.Mana}/{c.ManaMax}</BASEFONT>", false, false);        y += 25;
            }
            else
            {
                AddHtml(20, y, 360, 20, "<BASEFONT COLOR=#FFFFFF>No further stats available.</BASEFONT>", false, false);
                y += 25;
            }

            AddButton(350, 260, 4017, 4019, 1, GumpButtonType.Reply, 0);
            AddHtml(310, 260, 40, 20, "<BASEFONT COLOR=#FFFFFF>Close</BASEFONT>", false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            m_Owner.SendGump(new VisualStablemasterGump(m_Owner, 0));
        }
    }

    // --- New target for stabling ---
	public class StablePetTarget : Target
	{
		private PlayerMobile m_Owner;

		public StablePetTarget(PlayerMobile owner)
			: base(12, false, TargetFlags.None)
		{
			m_Owner = owner;
		}

		protected override void OnTarget(Mobile from, object targeted)
		{
			if (!(targeted is BaseCreature pet))
			{
				from.SendMessage("That is not a creature you can stable.");
				return;
			}

			if (pet.Deleted || !pet.Controlled || pet.ControlMaster != from)
			{
				from.SendMessage("You can only stable creatures you currently control.");
				return;
			}

			if (pet.IsStabled)
			{
				from.SendMessage("{0} is already stabled.", pet.Name);
				return;
			}

			// Cost to stable (same as NPC stablemaster: 30 gold)
			if ((from.Backpack != null && from.Backpack.ConsumeTotal(typeof(Gold), 30)) || Banker.Withdraw(from, 30))
			{
				pet.ControlTarget = null;
				pet.ControlOrder = OrderType.Stay;
				pet.Internalize(); // Removes pet from world

				pet.SetControlMaster(null);
				pet.SummonMaster = null;

				pet.IsStabled = true;
				pet.StabledBy = from;

				if (Core.SE)
					pet.Loyalty = BaseCreature.MaxLoyalty; // Wonderfully happy

				from.Stabled.Add(pet);

				from.SendMessage("Your pet has been stabled.");
			}
			else
			{
				from.SendMessage("You do not have enough gold to stable your pet.");
			}

			from.SendGump(new VisualStablemasterGump(m_Owner, 0));
		}
	}

}
