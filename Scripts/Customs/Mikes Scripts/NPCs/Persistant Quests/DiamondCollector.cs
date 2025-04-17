using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class DiamondCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required diamonds

    [Constructable]
    public DiamondCollector() : base("the diamond collector")
    {
        Name = "Diamond Dylan";
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

    public DiamondCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in 1000 Diamonds.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        if (!profile.Talents.ContainsKey(TalentID.DiamondCollection))
            profile.Talents[TalentID.DiamondCollection] = new Talent(TalentID.DiamondCollection);

        return (profile.Talents[TalentID.DiamondCollection].Points >= RequiredAmount);
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If the quest is complete, open the vendor gump.
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
            from.SendMessage("I don't sell anything yet. Bring me the diamonds first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect diamonds first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I am in need of 1000 diamonds. Bring me enough and I will open my shop for you.");

        module.AddOption("I have collected the diamonds.", p => true, p =>
        {
            // Process the player's diamonds in a partial turn-in.
            if (ProcessPartialDiamonds(p))
            {
                DialogueModule success = new DialogueModule("Excellent! You've provided all the diamonds I need. My shop is now open for your pleasure.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the diamonds. Your progress has been recorded. Please come back when you have collected 1000 diamonds in total.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What is this quest about?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 diamonds. Once you have provided them, I'll unlock my shop where you can purchase rare items such as Diamond, GlovesOfCommand, BioSamples, OldEmbroideryTool, DistillationFlask, SexWhip, FrostToken, and SoftTowel.");
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

    // Process the partial diamond turn-in.
    // Returns true if total diamond progress reaches or exceeds the required amount.
    private bool ProcessPartialDiamonds(PlayerMobile player)
    {
        // Count diamonds in the player's backpack.
        int available = 0;
        List<Item> diamondItems = new List<Item>();

        foreach (Item item in player.Backpack.Items)
        {
            if (item is Diamond) // Assumes a Diamond item exists.
            {
                available += item.Amount;
                diamondItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();

        if (!profile.Talents.ContainsKey(TalentID.DiamondCollection))
            profile.Talents[TalentID.DiamondCollection] = new Talent(TalentID.DiamondCollection);

        int currentPoints = profile.Talents[TalentID.DiamondCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0 || available <= 0)
            return false;

        int toTurnIn = Math.Min(needed, available);

        // Remove diamonds from the backpack.
        RemoveItems(player.Backpack, diamondItems, toTurnIn);

        // Update the persistent quest progress (cap at RequiredAmount).
        profile.Talents[TalentID.DiamondCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return IsQuestComplete(player);
    }

    // Helper: Remove a specific amount from a list of items in a container.
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
        // You can open your standard vendor gump here.
        // For example: player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBDiamondVendor());
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

// Vendor inventory for the Diamond Collector NPC.
public class SBDiamondVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBDiamondVendor()
    {
        // Add items to sell.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Diamond), 50, 20, 0xF0D, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GlovesOfCommand), 500, 20, 0x1450, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BioSamples), 100, 20, 0x1F0D, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(OldEmbroideryTool), 75, 20, 0x1022, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(DistillationFlask), 150, 20, 0xF09, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(SexWhip), 1000, 20, 0x26B8, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(FrostToken), 200, 20, 0x1F14, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(SoftTowel), 30, 20, 0x0E77, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            // Items the vendor will buy from players (if any).
            Add(typeof(Diamond), 25);
            Add(typeof(GlovesOfCommand), 250);
            Add(typeof(BioSamples), 50);
            Add(typeof(OldEmbroideryTool), 35);
            Add(typeof(DistillationFlask), 75);
            Add(typeof(SexWhip), 500);
            Add(typeof(FrostToken), 100);
            Add(typeof(SoftTowel), 15);
        }
    }
}
