using System;
using System.Collections.Generic;
using Server;
using Server.Gumps;
using Server.Network;
using Server.Mobiles;
using Server.Items;
using Server.Commands;
using Server.ACC.CSS.Systems.AlchemyMagic;
using Server.ACC.CSS.Systems.FishingMagic;
using Server.ACC.CSS.Systems.EvalIntMagic;
using Server.ACC.CSS.Systems.ArcheryMagic;
using Server.ACC.CSS.Systems.MageryMagic;
using Server.ACC.CSS.Systems.ArmsLoreMagic;
using Server.ACC.CSS.Systems.AnimalTamingMagic;
using Server.ACC.CSS.Systems.AnimalLoreMagic;
using Server.ACC.CSS.Systems.CarpentryMagic;
using Server.ACC.CSS.Systems.CartographyMagic;
using Server.ACC.CSS.Systems.TasteIDMagic;
using Server.ACC.CSS.Systems.CookingMagic;
using Server.ACC.CSS.Systems.DiscordanceMagic;
using Server.ACC.CSS.Systems.FletchingMagic;
using Server.ACC.CSS.Systems.ForensicsMagic;
using Server.ACC.CSS.Systems.WrestlingMagic;
using Server.ACC.CSS.Systems.ParryMagic;
using Server.ACC.CSS.Systems.HealingMagic;
using Server.ACC.CSS.Systems.DetectHiddenMagic;
using Server.ACC.CSS.Systems.ProvocationMagic;
using Server.ACC.CSS.Systems.LockpickingMagic;
using Server.ACC.CSS.Systems.MacingMagic;
using Server.ACC.CSS.Systems.MeditationMagic;
using Server.ACC.CSS.Systems.BeggingMagic;
using Server.ACC.CSS.Systems.MiningMagic;
using Server.ACC.CSS.Systems.ChivalryMagic;
using Server.ACC.CSS.Systems.StealingMagic;
using Server.ACC.CSS.Systems.InscribeMagic;
using Server.ACC.CSS.Systems.NinjitsuMagic;
using Server.ACC.CSS.Systems.HidingMagic;
using Server.ACC.CSS.Systems.StealthMagic;
using Server.ACC.CSS.Systems.BlacksmithMagic;
using Server.ACC.CSS.Systems.FencingMagic;
using Server.ACC.CSS.Systems.BlacksmithMagic;

namespace Server.Gumps
{
    public class GenericItemVendor
    {
        public static void Initialize()
        {
            CommandSystem.Register("maxxiastore", AccessLevel.Player, new CommandEventHandler(OpenVendor_OnCommand));
        }

        [Usage("MaxxiaStore")]
        [Description("Opens the Maxxia Scroll Store")]
        public static void OpenVendor_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            if (from != null && from is PlayerMobile)
            {
                PlayerMobile pm = (PlayerMobile)from;
                from.CloseGump(typeof(GenericItemVendorGump));
                from.SendGump(new GenericItemVendorGump(pm, 0));
            }
        }
    }

    public class MaxxiaVendorItem
    {
        public Type ItemType { get; private set; }
        public string Name { get; private set; }
        public int ItemID { get; private set; }
        public int Price { get; private set; }

        public MaxxiaVendorItem(Type itemType, string name, int itemID, int price)
        {
            ItemType = itemType;
            Name = name;
            ItemID = itemID;
            Price = price;
        }
    }

    public class GenericItemVendorGump : Gump
    {
        private const int ItemsPerPage = 6;
        private PlayerMobile m_Owner;
        private int m_Page;

        private static List<MaxxiaVendorItem> MaxxiaVendorItems = new List<MaxxiaVendorItem>();

        static GenericItemVendorGump()
        {
            // Add your items here
            MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(RoyalSkillCharter), "Royal Skill Charter", 5360, 2));
            MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(RoyalStatCharter), "Royal Stat Charter", 5360, 3));
            MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(RoyalPetsCharter), "Royal Pets Charter", 5360, 20));
            MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(SkillOrb), "Skill Orb", 6249, 10));
            MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(StatCapOrb), "Stat Cap Orb", 6249, 20));
            MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(PetSlotDeed), "Pet Slot Deed", 5360, 50));
			MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(CapacityIncreaseDeed), "Backpack Increase Deed", 5360, 50));
			MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(WeaponOil), "Weapon Oil", 3622, 20));
			MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(MasterWeaponOil), "Master Weapon Oil", 3622, 30));
			MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(BankCrystal), "Bank Crystal", 7964, 100));
			MaxxiaVendorItems.Add(new MaxxiaVendorItem(typeof(VirtueDistillationFlask), "Virtue Distillation Flask", 6194, 1));


            // Add more items as needed
        }

        public GenericItemVendorGump(PlayerMobile owner, int page)
            : base(50, 50)
        {
            m_Owner = owner;
            m_Page = page;

            AddPage(0);
            AddBackground(0, 0, 600, 430, 9270);
            AddAlphaRegion(10, 10, 580, 410);

            AddHtml(20, 20, 560, 20, "<CENTER><BASEFONT COLOR=#FFFFFF>Maxxia Store</BASEFONT></CENTER>", false, false);

            int pageCount = (MaxxiaVendorItems.Count + ItemsPerPage - 1) / ItemsPerPage;

            if (page > 0)
                AddButton(550, 20, 4014, 4016, 1, GumpButtonType.Reply, 0); // Previous page

            if (page < pageCount - 1)
                AddButton(575, 20, 4005, 4007, 2, GumpButtonType.Reply, 0); // Next page

            AddHtml(-200, 20, 560, 20, String.Format("<CENTER><BASEFONT COLOR=#FFFFFF>Page {0}/{1}</BASEFONT></CENTER>", page + 1, pageCount), false, false);

            for (int i = 0; i < ItemsPerPage; ++i)
            {
                int index = (page * ItemsPerPage) + i;
                if (index >= MaxxiaVendorItems.Count)
                    break;

                MaxxiaVendorItem item = MaxxiaVendorItems[index];

                int xOffset = (i % 3) * 190;
                int yOffset = (i / 3) * 180;

                AddImageTiled(20 + xOffset, 50 + yOffset, 180, 170, 2624);
                AddAlphaRegion(25 + xOffset, 55 + yOffset, 170, 160);

                AddItem(70 + xOffset, 100 + yOffset, item.ItemID);
                AddHtml(25 + xOffset, 55 + yOffset, 170, 20, String.Format("<CENTER><BASEFONT COLOR=#FFFFFF>{0}</BASEFONT></CENTER>", item.Name), false, false);
                AddHtml(25 + xOffset, 180 + yOffset, 170, 20, String.Format("<CENTER><BASEFONT COLOR=#FFFFFF>Cost: {0} MaxxiaScroll</BASEFONT></CENTER>", item.Price), false, false);

                AddButton(155 + xOffset, 145 + yOffset, 4005, 4007, 100 + index, GumpButtonType.Reply, 0);
            }
        }

        public override void OnResponse(NetState sender, RelayInfo info)
        {
            Mobile from = sender.Mobile;

            switch (info.ButtonID)
            {
                case 1: // Previous page
                    from.SendGump(new GenericItemVendorGump(m_Owner, m_Page - 1));
                    break;
                case 2: // Next page
                    from.SendGump(new GenericItemVendorGump(m_Owner, m_Page + 1));
                    break;
                default:
                    if (info.ButtonID >= 100)
                    {
                        int index = info.ButtonID - 100;
                        if (index >= 0 && index < MaxxiaVendorItems.Count)
                        {
                            MaxxiaVendorItem MaxxiaVendorItem = MaxxiaVendorItems[index];
                            if (HasEnoughMaxxiaScrolls(from, MaxxiaVendorItem.Price))
                            {
                                ConsumeMaxxiaScrolls(from, MaxxiaVendorItem.Price);
                                GiveItem(from, MaxxiaVendorItem);
                                from.SendMessage(String.Format("You have purchased {0} for {1} MaxxiaScrolls.", MaxxiaVendorItem.Name, MaxxiaVendorItem.Price));
                            }
                            else
                            {
                                from.SendMessage(String.Format("You don't have enough MaxxiaScrolls. You need {0} to purchase {1}.", MaxxiaVendorItem.Price, MaxxiaVendorItem.Name));
                            }
                        }
                        from.SendGump(new GenericItemVendorGump(m_Owner, m_Page));
                    }
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

        private void GiveItem(Mobile from, MaxxiaVendorItem MaxxiaVendorItem)
        {
            Item item = (Item)Activator.CreateInstance(MaxxiaVendorItem.ItemType);
            from.AddToBackpack(item);
        }
    }
}