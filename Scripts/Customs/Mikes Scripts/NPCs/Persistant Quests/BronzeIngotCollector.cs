using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class BronzeIngotCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required Bronze Ingots

    [Constructable]
    public BronzeIngotCollector() : base("the ingot collector")
    {
        Name = "Ingot Collector Ivan";
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

    public BronzeIngotCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player's Bronze Ingot quest is complete.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.BronzeIngotCollection))
            return false;
        return (profile.Talents[TalentID.BronzeIngotCollection].Points >= RequiredAmount);
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If quest is complete, open the vendor gump.
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

    // Only show buy/sell options in the context menu if the quest is complete.
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
            from.SendMessage("I don’t trade until I have all my Bronze Ingots. Bring me 1000 of them first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I’m not a shopkeeper yet! Please help me collect 1000 Bronze Ingots.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I’m in need of 1000 Bronze Ingots. Deliver them to me and I’ll open my shop for you.");

        module.AddOption("I have collected the Bronze Ingots.", p => true, p =>
        {
            // Process any ingots the player has turned in.
            if (ProcessPartialIngots(p))
            {
                DialogueModule success = new DialogueModule("Marvelous! You have turned in all 1000 Bronze Ingots. My shop is now open to you.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the ingots. Your progress has been updated. Please return once you have turned in all 1000 Bronze Ingots.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need the ingots for?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I’m collecting Bronze Ingots to fund my business. Once I have 1000 ingots, I’ll become a shopkeeper offering fine items like ingots, Animal Taming Augment Crystals, Glass Of Bubbly, Fish Baskets, and Fine Silver Wire.");
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

    // Process partial Bronze Ingot turn-ins.
    // Returns true if the player has turned in all required ingots.
    private bool ProcessPartialIngots(PlayerMobile player)
    {
        Container pack = player.Backpack;
        if (pack == null)
            return false;

        int availableIngots = 0;
        List<Item> ingotItems = new List<Item>();

        // Count BronzeIngot items in the backpack.
        foreach (Item item in pack.Items)
        {
            if (item is BronzeIngot)
            {
                availableIngots += item.Amount;
                ingotItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();

        // Ensure the BronzeIngotCollection talent exists.
        if (!profile.Talents.ContainsKey(TalentID.BronzeIngotCollection))
            profile.Talents[TalentID.BronzeIngotCollection] = new Talent(TalentID.BronzeIngotCollection);

        int currentPoints = profile.Talents[TalentID.BronzeIngotCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0 || availableIngots <= 0)
            return currentPoints >= RequiredAmount;

        int toTurnIn = Math.Min(needed, availableIngots);
        RemoveItems(pack, ingotItems, toTurnIn);
        profile.Talents[TalentID.BronzeIngotCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return profile.Talents[TalentID.BronzeIngotCollection].Points >= RequiredAmount;
    }

    // Remove a specific amount of items from a container.
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

        player.SendMessage("Welcome to my shop! Browse my fine selection of goods.");
        // You can open your vendor gump here if desired.
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    // Vendor inventory properties.
    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBBronzeVendor());
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

// Vendor inventory for the Ingot Collector NPC.
public class SBBronzeVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBBronzeVendor()
    {
        // Items sold: BronzeIngot, AnimalTamingAugmentCrystal, GlassOfBubbly, FishBasket, FineSilverWire.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BronzeIngot), 5, 20, 0x1BF2, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AnimalTamingAugmentCrystal), 50, 10, 0x1F1C, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GlassOfBubbly), 10, 10, 0x9AAA, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(FishBasket), 15, 10, 0xA7AC, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(FineSilverWire), 20, 10, 0x1877, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(BronzeIngot), 2);
            Add(typeof(AnimalTamingAugmentCrystal), 25);
            Add(typeof(GlassOfBubbly), 5);
            Add(typeof(FishBasket), 7);
            Add(typeof(FineSilverWire), 10);
        }
    }
}
