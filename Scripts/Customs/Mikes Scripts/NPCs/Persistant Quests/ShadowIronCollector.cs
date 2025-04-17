using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class ShadowIronCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required ingots

    [Constructable]
    public ShadowIronCollector() : base("the ingot inspector")
    {
        Name = "Ingot Isaac";
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

    public ShadowIronCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has collected 1000 ShadowIronIngots.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.ShadowIronIngotCollection))
            return false;

        return (profile.Talents[TalentID.ShadowIronIngotCollection].Points >= RequiredAmount);
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

    // Only show buy/sell options in the context menu if the player's quest is complete.
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
            from.SendMessage("I don't sell anything yet. Bring me 1000 Shadow Iron Ingots first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect the ingots first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I am in dire need of 1000 Shadow Iron Ingots. If you can bring them to me, I'll reward you by opening my shop.");
        
        module.AddOption("I have collected the ingots.", p => true, p =>
        {
            if (ProcessIngotTurnIn(p))
            {
                DialogueModule success = new DialogueModule("Outstanding! You have delivered all 1000 ingots. My shop is now open for you to browse.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the ingots. Your progress has been updated. Come back when you have delivered 1000 Shadow Iron Ingots.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What are these ingots for?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 Shadow Iron Ingots. Once delivered, I'll open my shop where you can buy ingots along with AnatomyAugmentCrystal, BottledPlague, NixieStatue, and GrandmasSpecialRolls.");
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

    // Process ingot turn-ins from the player's backpack.
    // Returns true if the total reaches the required amount.
    private bool ProcessIngotTurnIn(PlayerMobile player)
    {
        int ingotsFound = 0;
        List<Item> ingotItems = new List<Item>();

        // Count ingots in player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is ShadowIronIngot)
            {
                ingotsFound += item.Amount;
                ingotItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.ShadowIronIngotCollection))
            profile.Talents[TalentID.ShadowIronIngotCollection] = new Talent(TalentID.ShadowIronIngotCollection);

        int currentPoints = profile.Talents[TalentID.ShadowIronIngotCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0)
            return true; // Already complete

        int toTurnIn = Math.Min(needed, ingotsFound);

        // Remove ingots from the backpack.
        RemoveItems(player.Backpack, ingotItems, toTurnIn);

        // Update the talent progress.
        profile.Talents[TalentID.ShadowIronIngotCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);
        return profile.Talents[TalentID.ShadowIronIngotCollection].Points >= RequiredAmount;
    }

    // Helper: Remove a specified amount of ingots from the list of items in the container.
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

        player.SendMessage("Welcome to my shop! Browse my fine selection of items.");
        // Here you might open your standard vendor gump:
        // player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBShadowIronVendor());
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

// Vendor inventory for the ShadowIronCollector once the quest is complete.
public class SBShadowIronVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBShadowIronVendor()
    {
        // Adjust prices, amounts, and item IDs as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ShadowIronIngot), 20, 50, 0x1BF2, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AnatomyAugmentCrystal), 50, 20, 0x1F1C, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BottledPlague), 30, 20, 0xE27, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(NixieStatue), 100, 10, 0xA2C7, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GrandmasSpecialRolls), 15, 30, 0x4BAB, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(ShadowIronIngot), 10);
            Add(typeof(AnatomyAugmentCrystal), 25);
            Add(typeof(BottledPlague), 15);
            Add(typeof(NixieStatue), 50);
            Add(typeof(GrandmasSpecialRolls), 8);
        }
    }
}
