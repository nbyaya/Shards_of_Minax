using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class AmberCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // required pieces of Amber

    [Constructable]
    public AmberCollector() : base("the amber collector")
    {
        Name = "Amber Armand";
        Body = 0x190; // Human body type
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(60);
        SetInt(60);

        SetHits(100);
        SetMana(0);
        SetStam(100);

        Fame = 1000;
        Karma = 1000;

        VirtualArmor = 10;

        Frozen = true; // Prevent movement
        CantWalk = true;
    }

    public AmberCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in enough Amber.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        // Ensure the AmberCollection talent exists
        if (!profile.Talents.ContainsKey(TalentID.AmberCollection))
            return false;

        return (profile.Talents[TalentID.AmberCollection].Points >= RequiredAmount);
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        if (IsQuestComplete(player))
        {
            OpenVendorGump(player);
        }
        else
        {
            DialogueModule module = CreateDialogueModule();
            player.SendGump(new DialogueGump(player, module));
        }
    }

    // Only show vendor buy/sell context menu entries if quest is complete.
    public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
    {
        base.GetContextMenuEntries(from, list);
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            list.RemoveAll(entry => entry is VendorBuyEntry || entry is VendorSellEntry);
        }
    }

    // Prevent vendor actions unless quest is complete.
    public override void VendorBuy(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I don’t trade until you’ve gathered enough Amber!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I’m not a vendor yet! Please bring me more Amber.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I seek 1000 pieces of pure Amber. Bring them to me, and I shall open my wares to you.");

        module.AddOption("I have collected the Amber.", p =>
        {
            // Option always available
            return true;
        }, p =>
        {
            if (ProcessPartialAmberTurnIn(p))
            {
                DialogueModule success = new DialogueModule("Marvelous! You have gathered enough Amber. My shop is now open for you.");
                success.AddOption("Thank you.", pla =>
                {
                    return true;
                }, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the Amber. Your progress has been updated. Return when you have turned in a total of 1000 pieces.");
                partial.AddOption("Understood.", pla =>
                {
                    return true;
                }, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require a total of 1000 pieces of Amber. Gather them and return to me.");
            info.AddOption("Understood.", pla =>
            {
                return true;
            }, pla =>
            {
                pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
            });
            p.SendGump(new DialogueGump(p, info));
        });

        module.AddOption("I must be going.", p => true, p =>
        {
            p.SendMessage("Safe travels, friend.");
        });

        return module;
    }

    // Process the Amber turn-in. Returns true if the quest is now complete.
    private bool ProcessPartialAmberTurnIn(PlayerMobile player)
    {
        int availableAmber = 0;
        List<Item> amberItems = new List<Item>();

        // Count Amber in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is Amber)
            {
                availableAmber += item.Amount;
                amberItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();

        // Ensure the talent exists.
        if (!profile.Talents.ContainsKey(TalentID.AmberCollection))
            profile.Talents[TalentID.AmberCollection] = new Talent(TalentID.AmberCollection);

        int currentPoints = profile.Talents[TalentID.AmberCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0 || availableAmber <= 0)
            return false;

        int toTurnIn = Math.Min(needed, availableAmber);
        RemoveItems(player.Backpack, amberItems, toTurnIn);
        profile.Talents[TalentID.AmberCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return IsQuestComplete(player);
    }

    // Helper to remove a given amount of items from a container.
    private void RemoveItems(Container container, List<Item> items, int amountToRemove)
    {
        int removed = 0;
        foreach (Item item in items)
        {
            if (removed >= amountToRemove)
                break;

            if (item.Amount <= (amountToRemove - removed))
            {
                removed += item.Amount;
                item.Delete();
            }
            else
            {
                item.Amount -= (amountToRemove - removed);
                removed = amountToRemove;
            }
        }
    }

    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Excellent work! My shop is now open to you.");
        // Open your vendor gump (if you have one) or simply call the base method.
        // For example:
        // player.SendGump(new VendorGump(player, this));
    }

    // Vendor inventory for the Amber collector.
    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBAmberVendor());
    }

    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}

// Vendor inventory for the AmberCollector NPC.
public class SBAmberVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBAmberVendor()
    {
        // Note: Prices and quantities are provided as examples.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Amber), 5, 20, 0x0, 0));  // assuming itemID 0x0 for Amber
        m_BuyInfo.Add(new GenericBuyInfo(typeof(PetBondDeed), 100, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(SatanicTable), 250, 5, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(WaterFountain), 150, 5, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(FountainWall), 150, 5, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(HildebrandtFlag), 50, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(MysteryOrb), 500, 2, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BlueberryPie), 10, 20, 0x0, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            // Sell prices (again, as examples)
            Add(typeof(Amber), 3);
            Add(typeof(PetBondDeed), 50);
            Add(typeof(SatanicTable), 125);
            Add(typeof(WaterFountain), 75);
            Add(typeof(FountainWall), 75);
            Add(typeof(HildebrandtFlag), 25);
            Add(typeof(MysteryOrb), 250);
            Add(typeof(BlueberryPie), 5);
        }
    }
}
