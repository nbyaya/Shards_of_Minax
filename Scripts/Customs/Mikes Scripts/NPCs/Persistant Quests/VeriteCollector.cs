using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class VeriteCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required number of Verite Ingots

    [Constructable]
    public VeriteCollector() : base("the verite collector")
    {
        Name = "Verite Collector Vance";
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

    public VeriteCollector(Serial serial) : base(serial)
    {
    }

    // Checks if the player has turned in enough Verite Ingots.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        if (!profile.Talents.ContainsKey(TalentID.VeriteCollection))
            return false;

        return (profile.Talents[TalentID.VeriteCollection].Points >= RequiredAmount);
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
            from.SendMessage("I don't sell anything yet. Bring me the Verite Ingots first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect Verite Ingots first.");
            return;
        }
        base.VendorSell(from);
    }

    // Create the branching dialogue for the quest.
    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I'm in dire need of 1000 Verite Ingots. Bring them to me and I'll open my shop for you.");
        
        module.AddOption("I have collected the ingots.", p => true, p =>
        {
            // Process the ingot turn-in and check progress.
            if (ProcessPartialIngots(p))
            {
                DialogueModule success = new DialogueModule("Outstanding work! You've turned in all required ingots. My shop is now open for you.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the ingots. Your progress has been updated. Come back when you have turned in 1000 Verite Ingots.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });
        
        module.AddOption("What do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 Verite Ingots. Once you bring them to me, my shop will be at your disposal.");
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

    // Processes partial turn-ins for Verite Ingots.
    // Returns true if the player has now turned in all required ingots.
    private bool ProcessPartialIngots(PlayerMobile player)
    {
        int availableIngots = 0;
        List<Item> ingotItems = new List<Item>();

        // Count all VeriteIngot items in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is VeriteIngot)
            {
                availableIngots += item.Amount;
                ingotItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.VeriteCollection))
            profile.Talents[TalentID.VeriteCollection] = new Talent(TalentID.VeriteCollection);

        int currentPoints = profile.Talents[TalentID.VeriteCollection].Points;
        int needed = RequiredAmount - currentPoints;

        // If no ingots available or already complete, return false.
        if (needed <= 0 || availableIngots <= 0)
            return false;

        int toTurnIn = Math.Min(needed, availableIngots);
        RemoveItems(player.Backpack, ingotItems, toTurnIn);
        profile.Talents[TalentID.VeriteCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return IsQuestComplete(player);
    }

    // Removes a specified amount from a list of items in a container.
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

    // Opens the vendor shop for players who have completed the quest.
    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my fine selection of wares.");
        // Here you might open your standard vendor gump.
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    // Expose the SBInfo list for the vendor.
    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBVeriteVendor());
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

// Vendor inventory for the Verite Collector.
public class SBVeriteVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBVeriteVendor()
    {
        // Define items to sell. Adjust prices, stock, and item IDs as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(VeriteIngot), 10, 20, 0x1BF2, 0)); 
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ArmSlotChangeDeed), 500, 10, 0x14F0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AlchemistBookcase), 800, 5, 0xA5E8, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ElementRegular), 150, 10, 0x183E, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Satchel), 50, 10, 0xAD77, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(VeriteIngot), 5);
            Add(typeof(ArmSlotChangeDeed), 250);
            Add(typeof(AlchemistBookcase), 400);
            Add(typeof(ElementRegular), 75);
            Add(typeof(Satchel), 25);
        }
    }
}
