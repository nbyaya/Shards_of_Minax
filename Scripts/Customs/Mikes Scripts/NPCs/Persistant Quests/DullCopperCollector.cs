using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class DullCopperCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Number of DullCopperIngots required

    [Constructable]
    public DullCopperCollector() : base("the ingot collector")
    {
        Name = "Ingot Collector Iggy";
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

    public DullCopperCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in enough ingots.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        if (!profile.Talents.ContainsKey(TalentID.DullCopperIngotCollection))
            return false;

        return (profile.Talents[TalentID.DullCopperIngotCollection].Points >= RequiredAmount);
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If the quest is complete, open vendor gump (shop)
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

    // Only show buy/sell options in the context menu if the player's quest is complete.
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
            from.SendMessage("I am not trading yet. Please bring me 1000 Dull Copper Ingots first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I can't trade with you until you've brought me the ingots.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I am in dire need of 1000 Dull Copper Ingots. Bring them to me and I shall open my shop for you.");

        module.AddOption("I have collected the ingots.", p => true, p =>
        {
            // Process the ingots in a partial turn-in.
            if (ProcessPartialIngots(p))
            {
                DialogueModule success = new DialogueModule("Excellent work! With all 1000 ingots, my shop is now open. Browse my wares freely.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the ingots. Your progress has been recorded. Return when you have collected a total of 1000.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("Tell me more about this task.", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 Dull Copper Ingots. These ingots are a rare find and are used in the finest crafting. Return when you have gathered them.");
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

    // Process partial ingot turn-ins.
    // Returns true if the player’s cumulative ingot count reaches the required amount.
    private bool ProcessPartialIngots(PlayerMobile player)
    {
        // Count available ingots in the player's backpack.
        int availableIngots = 0;
        List<Item> ingotItems = new List<Item>();

        foreach (Item item in player.Backpack.Items)
        {
            // Assuming your shard defines a DullCopperIngot item type.
            if (item is DullCopperIngot)
            {
                availableIngots += item.Amount;
                ingotItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();

        // Ensure the talent entry exists.
        if (!profile.Talents.ContainsKey(TalentID.DullCopperIngotCollection))
            profile.Talents[TalentID.DullCopperIngotCollection] = new Talent(TalentID.DullCopperIngotCollection);

        int currentPoints = profile.Talents[TalentID.DullCopperIngotCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0 || availableIngots <= 0)
            return false;

        int toTurnIn = Math.Min(needed, availableIngots);
        RemoveItems(player.Backpack, ingotItems, toTurnIn);
        profile.Talents[TalentID.DullCopperIngotCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return (profile.Talents[TalentID.DullCopperIngotCollection].Points >= RequiredAmount);
    }

    // Remove a specific number of ingots from the player's backpack.
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

        player.SendMessage("My shop is now open! Browse my wares.");
        // Optionally, open your standard vendor gump:
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    // Override to supply our vendor buy/sell information.
    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBDullCopperVendor());
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

// Vendor inventory for the DullCopperCollector NPC.
public class SBDullCopperVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBDullCopperVendor()
    {
        // The vendor sells the DullCopperIngot along with the following items.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(DullCopperIngot), 5, 20, 0x1BF2, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AlchemyAugmentCrystal), 10, 20, 0x1F1D, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(StrappedBooks), 15, 20, 0xA721, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(CarpentryTalisman), 20, 20, 0x9E2C, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(CharcuterieBoard), 25, 20, 0xA857, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            // Set prices for items the vendor will buy from players.
            Add(typeof(DullCopperIngot), 2);
            Add(typeof(AlchemyAugmentCrystal), 5);
            Add(typeof(StrappedBooks), 7);
            Add(typeof(CarpentryTalisman), 10);
            Add(typeof(CharcuterieBoard), 12);
        }
    }
}
