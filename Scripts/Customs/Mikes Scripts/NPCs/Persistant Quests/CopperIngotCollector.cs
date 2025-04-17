using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class CopperIngotCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Total copper ingots required

    [Constructable]
    public CopperIngotCollector() : base("the copper ingot collector")
    {
        Name = "Ingot Ivan";
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

    public CopperIngotCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in 1000 copper ingots
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        // Ensure the talent entry exists
        if (!profile.Talents.ContainsKey(TalentID.CopperIngotQuest))
            profile.Talents[TalentID.CopperIngotQuest] = new Talent(TalentID.CopperIngotQuest);

        return profile.Talents[TalentID.CopperIngotQuest].Points >= RequiredAmount;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If quest is complete, open vendor gump; otherwise show dialogue.
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

    // Only show buy/sell options if the player's quest is complete.
    public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
    {
        base.GetContextMenuEntries(from, list);
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            list.RemoveAll(entry => entry is VendorBuyEntry || entry is VendorSellEntry);
        }
    }

    // Prevent vendor actions unless the player's quest is complete.
    public override void VendorBuy(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I don't sell anything yet. Bring me the copper ingots first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect copper ingots first.");
            return;
        }
        base.VendorSell(from);
    }

    // Creates the branching dialogue for the quest
    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I am in need of 1000 copper ingots. Bring them to me and I shall open my shop for you.");
        
        module.AddOption("I have collected the copper ingots.", p => true, p =>
        {
            // Process ingots in a partial turn-in
            if (ProcessPartialIngots(p))
            {
                DialogueModule success = new DialogueModule("Excellent work! You've delivered all 1000 copper ingots and unlocked my shop. Feel free to browse my wares.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the copper ingots. Your progress has been updated. Return when you've gathered 1000 in total.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What copper ingots do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require a total of 1000 copper ingots. Collect them from the mines and deliver them to me.");
            info.AddOption("Understood.", pla => true, pla =>
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

    // Process partial copper ingot turn-ins.
    // Returns true if the quest is complete.
    private bool ProcessPartialIngots(PlayerMobile player)
    {
        int available = 0;
        List<Item> ingots = new List<Item>();

        // Count copper ingots in the player's backpack
        foreach (Item item in player.Backpack.Items)
        {
            if (item is CopperIngot)
            {
                available += item.Amount;
                ingots.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.CopperIngotQuest))
            profile.Talents[TalentID.CopperIngotQuest] = new Talent(TalentID.CopperIngotQuest);

        int currentPoints = profile.Talents[TalentID.CopperIngotQuest].Points;
        int needed = RequiredAmount - currentPoints;
        int toTurnIn = Math.Min(needed, available);

        if (toTurnIn > 0)
        {
            RemoveItems(player.Backpack, ingots, toTurnIn);
            profile.Talents[TalentID.CopperIngotQuest].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);
        }
        return profile.Talents[TalentID.CopperIngotQuest].Points >= RequiredAmount;
    }

    // Helper method: Remove a specified amount of items from a container.
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

    // Opens the vendor gump (i.e. the shop interface)
    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my fine selection of items.");
        // Optionally, you can open your standard vendor gump here, e.g.:
        // player.SendGump(new VendorGump(player, this));
    }

    // Overrides for vendor inventory.
    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBCopperVendor());
    }

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

// Vendor inventory for the Copper Ingot Collector NPC.
public class SBCopperVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBCopperVendor()
    {
        // Set up the buy list:
        m_BuyInfo.Add(new GenericBuyInfo(typeof(CopperIngot), 10, 20, 0x1BF2, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AnimalLoreAugmentCrystal), 50, 10, 0x1F1C, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ExoticFish), 25, 10, 0xA392, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(HildebrandtShield), 75, 10, 0xAF1B, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GarbageBag), 5, 20, 0x9F85, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(CopperIngot), 5);
            Add(typeof(AnimalLoreAugmentCrystal), 25);
            Add(typeof(ExoticFish), 12);
            Add(typeof(HildebrandtShield), 37);
            Add(typeof(GarbageBag), 2);
        }
    }
}
