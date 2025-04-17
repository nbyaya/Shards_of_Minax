using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class HornedHideCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required HornedHides

    [Constructable]
    public HornedHideCollector() : base("the hide handler")
    {
        Name = "Hide Handler Hank";
        Body = 0x190; // Human body type
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(60);
        SetInt(60);

        SetHits(100);
        SetMana(0);
        SetStam(100);

        Fame = 1500;
        Karma = 1500;

        VirtualArmor = 10;

        Frozen = true; // Prevent movement
        CantWalk = true;
    }

    public HornedHideCollector(Serial serial) : base(serial)
    {
    }

    // Helper: Check if the player has turned in 1000 HornedHides.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.HornedHideCollection))
            return false;

        return profile.Talents[TalentID.HornedHideCollection].Points >= RequiredAmount;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If the player's quest is complete, open the vendor gump.
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

    // Prevent vendor actions unless the quest is complete.
    public override void VendorBuy(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I don't sell anything yet. Bring me the hides first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect HornedHides first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I’m in desperate need of HornedHides. Bring me 1000 and I’ll open my shop for you.");

        module.AddOption("I have collected the hides.", p => true, p =>
        {
            if (ProcessPartialHides(p))
            {
                DialogueModule success = new DialogueModule("Outstanding work! You have turned in all required HornedHides. My shop is now open for you.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the hides. Your progress has been updated. Come back when you have turned in 1000 HornedHides.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 HornedHides. Simply hand them over and I’ll reward you with access to my shop, stocked with some rare items.");
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

    // Process partial HornedHide turn-ins.
    // Returns true if the required amount is reached.
    private bool ProcessPartialHides(PlayerMobile player)
    {
        // Dictionary to track available HornedHides.
        int availableHides = 0;
        List<Item> hideItems = new List<Item>();

        foreach (Item item in player.Backpack.Items)
        {
            // Assume HornedHide is the type for the hide item.
            if (item is HornedHides)
            {
                availableHides += item.Amount;
                hideItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        int currentPoints = profile.Talents[TalentID.HornedHideCollection].Points;
        int needed = RequiredAmount - currentPoints;
        if (needed <= 0)
            return true;

        int toTurnIn = Math.Min(needed, availableHides);
        RemoveItems(player.Backpack, hideItems, toTurnIn);
        profile.Talents[TalentID.HornedHideCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return profile.Talents[TalentID.HornedHideCollection].Points >= RequiredAmount;
    }

    // Helper: Remove a specific amount of items from the container.
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

        player.SendMessage("Welcome to my shop! Browse my fine selection of rare items.");
        // Open your standard vendor gump here.
        // For example: player.SendGump(new VendorGump(player, this));
    }

    // Override to return vendor inventory.
    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBHornedHideVendor());
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

// Vendor inventory for the HornedHideCollector NPC.
public class SBHornedHideVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBHornedHideVendor()
    {
        // Items for sale:
        m_BuyInfo.Add(new GenericBuyInfo(typeof(HornedHides), 10, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BootsOfCommand), 250, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(DressForm), 150, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(LargeWeatheredBook), 100, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(WeddingCandelabra), 100, 10, 0x0, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(HornedHides), 5);
            Add(typeof(BootsOfCommand), 125);
            Add(typeof(DressForm), 75);
            Add(typeof(LargeWeatheredBook), 50);
            Add(typeof(WeddingCandelabra), 50);
        }
    }
}
