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
        // Now showing 6 items per page.
        private const int ItemsPerPage = 6;
        private PlayerMobile m_Owner;
        private int m_Page;

        public VisualStablemasterGump(PlayerMobile owner, int page)
            : base(50, 50)
        {
            m_Owner = owner;
            m_Page = page;

            // Ensure the gump can be closed with right-click and moved.
            this.Closable = true;
            this.Disposable = true;
            this.Dragable = true;

            // Set the background to be shorter now.
            // Background size: 600 x 650; inner alpha region: 580 x 630.
            AddPage(0);
            AddBackground(0, 0, 600, 650, 9270);
            AddAlphaRegion(10, 10, 580, 630);

            // Title text.
            AddHtml(20, 20, 560, 25, "<CENTER><BASEFONT COLOR=#FFFFFF><BIG>Stablemaster</BIG></BASEFONT></CENTER>", false, false);
            // Page number: shifted down to avoid overlapping the title and set to yellow.
            List<Mobile> stabled = m_Owner.Stabled;
            int pageCount = (stabled.Count + ItemsPerPage - 1) / ItemsPerPage;
            if (pageCount < 1)
                pageCount = 1;
            AddHtml(250, 45, 100, 25, String.Format("<CENTER><BASEFONT COLOR=#FFFF00><BIG>Page {0}/{1}</BIG></BASEFONT></CENTER>", m_Page + 1, pageCount), false, false);

            // Navigation buttons.
            if (m_Page > 0)
                AddButton(530, 20, 4014, 4016, 1, GumpButtonType.Reply, 0); // Previous page

            if (m_Page < pageCount - 1)
                AddButton(560, 20, 4005, 4007, 2, GumpButtonType.Reply, 0); // Next page

            // Set up the vertical list.
            // Each pet panel is 90 pixels high.
            int panelHeight = 90;
            int startY = 60;
            for (int i = 0; i < ItemsPerPage; ++i)
            {
                int index = (m_Page * ItemsPerPage) + i;
                if (index >= stabled.Count)
                    break;

                Mobile pet = stabled[index];
                if (pet == null || pet.Deleted)
                    continue;

                // Draw the background panel with a small gap (5px) between panels.
                int panelX = 20;
                int panelY = startY + (i * (panelHeight + 5));
                AddImageTiled(panelX, panelY, 560, panelHeight, 2624);
                AddAlphaRegion(panelX + 5, panelY + 5, 550, panelHeight - 10);

                // Display pet image.
                int petImageX = panelX + 10;
                int petImageY = panelY + 10;
                AddItem(petImageX, petImageY, GetItemIDForCreature(pet));

                // Display pet information.
                // Pet's Name on the first line.
                int textX = petImageX + 50;  // Space after the image.
                int textWidth = 340;         // Width for the pet info text area.
                int nameY = panelY + 10;
                int typeY = nameY + 20;

                string petName = pet.Name;
                string petType = pet.GetType().Name;
                string tamingSkill = "";
                string controlSlots = "";
                if (pet is BaseCreature creature)
                {
                    tamingSkill = creature.MinTameSkill.ToString("F1");
                    controlSlots = creature.ControlSlots.ToString();
                }
                else
                {
                    tamingSkill = "N/A";
                    controlSlots = "N/A";
                }

                AddHtml(textX, nameY, textWidth, 18, String.Format("<BASEFONT COLOR=#FFFFFF>Name: {0}</BASEFONT>", petName), false, false);
                AddHtml(textX, typeY, textWidth, 18, String.Format("<BASEFONT COLOR=#FFFFFF>Type: {0}</BASEFONT>", petType), false, false);

                // Display Tame and Slots on the next line.
                int statsY = typeY + 20;
                int statsWidth = textWidth / 2;
                AddHtml(textX, statsY, statsWidth, 18, String.Format("<BASEFONT COLOR=#FFFFFF>Tame: {0}</BASEFONT>", tamingSkill), false, false);
                AddHtml(textX + statsWidth, statsY, statsWidth, 18, String.Format("<BASEFONT COLOR=#FFFFFF>Slots: {0}</BASEFONT>", controlSlots), false, false);

                // Add buttons: Claim, Abandon, and Full Stat Sheet.
                // Adjust button label positions by shifting them to the right.
                int buttonX = panelX + 420;
                int buttonY = panelY + 10;
                // Claim button (IDs 100+):
                AddButton(buttonX, buttonY, 4005, 4007, 100 + index, GumpButtonType.Reply, 0);
                AddHtml(buttonX + 35, buttonY, 50, 20, "<BASEFONT COLOR=#FFFFFF>Claim</BASEFONT>", false, false);
                // Abandon button (IDs 200+):
                AddButton(buttonX, buttonY + 25, 4005, 4007, 200 + index, GumpButtonType.Reply, 0);
                AddHtml(buttonX + 35, buttonY + 25, 50, 20, "<BASEFONT COLOR=#FFFFFF>Abandon</BASEFONT>", false, false);
                // Stat Sheet button (IDs 300+):
                AddButton(buttonX, buttonY + 50, 4005, 4007, 300 + index, GumpButtonType.Reply, 0);
                AddHtml(buttonX + 35, buttonY + 50, 70, 20, "<BASEFONT COLOR=#FFFFFF>Stats</BASEFONT>", false, false);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            // ButtonID 0 indicates a right-click (or no response) and will close the gump.
            if (info.ButtonID == 0)
                return;

            List<Mobile> stabled = m_Owner.Stabled;
            // Navigation buttons.
            if (info.ButtonID == 1) // Previous page
            {
                from.SendGump(new VisualStablemasterGump(m_Owner, m_Page - 1));
                return;
            }
            if (info.ButtonID == 2) // Next page
            {
                from.SendGump(new VisualStablemasterGump(m_Owner, m_Page + 1));
                return;
            }

            // Determine which pet was clicked.
            // Claim buttons: IDs 100-199; Abandon: IDs 200-299; Stats: IDs 300-399.
            if (info.ButtonID >= 100 && info.ButtonID < 200)
            {
                int index = info.ButtonID - 100;
                if (index >= 0 && index < stabled.Count)
                {
                    Mobile pet = stabled[index];
                    if (pet != null && !pet.Deleted && pet is BaseCreature creature)
                    {
                        creature.SetControlMaster(m_Owner);
                        creature.ControlTarget = m_Owner;
                        creature.ControlOrder = OrderType.Follow;
                        creature.MoveToWorld(m_Owner.Location, m_Owner.Map);
                        creature.IsStabled = false;
                        stabled.RemoveAt(index);
                        from.SendMessage("You have claimed {0}.", creature.Name);
                    }
                }
            }
            else if (info.ButtonID >= 200 && info.ButtonID < 300)
            {
                int index = info.ButtonID - 200;
                if (index >= 0 && index < stabled.Count)
                {
                    Mobile pet = stabled[index];
                    if (pet != null && !pet.Deleted)
                    {
                        stabled.RemoveAt(index);
                        pet.Delete();
                        from.SendMessage("You have deleted {0} from your stable.", pet.Name);
                    }
                }
            }
            else if (info.ButtonID >= 300 && info.ButtonID < 400)
            {
                int index = info.ButtonID - 300;
                if (index >= 0 && index < stabled.Count)
                {
                    Mobile pet = stabled[index];
                    if (pet != null && !pet.Deleted)
                    {
                        // Open the pet stat sheet gump.
                        from.CloseGump(typeof(VisualStablemasterStatsGump));
                        from.SendGump(new VisualStablemasterStatsGump(m_Owner, pet));
                        return;
                    }
                }
            }

            // Refresh the stable gump.
            from.SendGump(new VisualStablemasterGump(m_Owner, m_Page));
        }

        private int GetItemIDForCreature(Mobile creature)
        {
            // Return an appropriate image item ID based on the creature type.
            if (creature is Horse)
                return 8480;
            if (creature is Dog)
                return 8476;
            if (creature is Cat)
                return 8475;
            if (creature is Cow)
                return 8451;
            // Add additional types as needed.
            return 8413; // Default image.
        }
    }

    public class VisualStablemasterStatsGump : Gump
    {
        private PlayerMobile m_Owner;
        private Mobile m_Pet;

        public VisualStablemasterStatsGump(PlayerMobile owner, Mobile pet)
            : base(100, 100)
        {
            m_Owner = owner;
            m_Pet = pet;

            // Basic stat sheet gump size.
            AddPage(0);
            AddBackground(0, 0, 400, 300, 9270);
            AddAlphaRegion(10, 10, 380, 280);

            AddHtml(20, 20, 360, 25, 
                String.Format("<CENTER><BASEFONT COLOR=#FFFFFF><BIG>{0}'s Stat Sheet</BIG></BASEFONT></CENTER>", pet.Name), 
                false, false);

            int y = 60;
            AddHtml(20, y, 360, 20, 
                String.Format("<BASEFONT COLOR=#FFFFFF>Type: {0}</BASEFONT>", pet.GetType().Name), 
                false, false);
            y += 25;

            if (pet is BaseCreature creature)
            {
                AddHtml(20, y, 360, 20, 
                    String.Format("<BASEFONT COLOR=#FFFFFF>Minimum Tame Skill: {0:F1}</BASEFONT>", creature.MinTameSkill), 
                    false, false);
                y += 25;
                AddHtml(20, y, 360, 20, 
                    String.Format("<BASEFONT COLOR=#FFFFFF>Control Slots: {0}</BASEFONT>", creature.ControlSlots), 
                    false, false);
                y += 25;
                AddHtml(20, y, 360, 20, 
                    String.Format("<BASEFONT COLOR=#FFFFFF>Hits: {0}/{1}</BASEFONT>", creature.Hits, creature.HitsMax), 
                    false, false);
                y += 25;
                AddHtml(20, y, 360, 20, 
                    String.Format("<BASEFONT COLOR=#FFFFFF>Stamina: {0}/{1}</BASEFONT>", creature.Stam, creature.StamMax), 
                    false, false);
                y += 25;
                AddHtml(20, y, 360, 20, 
                    String.Format("<BASEFONT COLOR=#FFFFFF>Mana: {0}/{1}</BASEFONT>", creature.Mana, creature.ManaMax), 
                    false, false);
                y += 25;
            }
            else
            {
                AddHtml(20, y, 360, 20, 
                    "<BASEFONT COLOR=#FFFFFF>No further stats available.</BASEFONT>", 
                    false, false);
                y += 25;
            }

            // Add a close button.
            AddButton(350, 260, 4017, 4019, 1, GumpButtonType.Reply, 0);
            AddHtml(310, 260, 40, 20, "<BASEFONT COLOR=#FFFFFF>Close</BASEFONT>", false, false);
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            // Return to the main stable gump.
            m_Owner.SendGump(new VisualStablemasterGump(m_Owner, 0));
        }
    }
}
