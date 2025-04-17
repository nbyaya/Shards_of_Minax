using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class AgapiteCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required AgapiteIngots

    [Constructable]
    public AgapiteCollector() : base("the ingot collector")
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

    public AgapiteCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player's Agapite Ingot quest is complete.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.AgapiteIngotCollection))
            profile.Talents[TalentID.AgapiteIngotCollection] = new Talent(TalentID.AgapiteIngotCollection);
        return (profile.Talents[TalentID.AgapiteIngotCollection].Points >= RequiredAmount);
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

    // Only show buy/sell context menu options if the player's quest is complete.
    public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
    {
        base.GetContextMenuEntries(from, list);
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            list.RemoveAll(entry => entry is VendorBuyEntry || entry is VendorSellEntry);
        }
    }

    public override void VendorBuy(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I don't sell anything yet. Bring me 1000 Agapite Ingots first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a shopkeeper yet! Help me collect Agapite Ingots first.");
            return;
        }
        base.VendorSell(from);
    }

    // Process the ingot turn-in from the player's backpack.
    private bool ProcessPartialIngots(PlayerMobile player)
    {
        int availableIngots = 0;
        List<Item> ingotItems = new List<Item>();

        // Count Agapite Ingots in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is AgapiteIngot)
            {
                availableIngots += item.Amount;
                ingotItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.AgapiteIngotCollection))
            profile.Talents[TalentID.AgapiteIngotCollection] = new Talent(TalentID.AgapiteIngotCollection);

        int currentPoints = profile.Talents[TalentID.AgapiteIngotCollection].Points;
        int needed = RequiredAmount - currentPoints;
        if (needed <= 0 || availableIngots <= 0)
            return IsQuestComplete(player);

        int toTurnIn = Math.Min(needed, availableIngots);
        RemoveItems(player.Backpack, ingotItems, toTurnIn);
        profile.Talents[TalentID.AgapiteIngotCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return IsQuestComplete(player);
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

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I am in need of 1000 Agapite Ingots. Deliver them to me and I shall open my shop, where you can purchase not only ingots but also rare items.");

        module.AddOption("I have collected the ingots.", p =>
        {
            // Always allow this option.
            return true;
        },
        p =>
        {
            if (ProcessPartialIngots(p))
            {
                DialogueModule success = new DialogueModule("Excellent work! With your help, my shop is now open. Feel free to browse my wares.");
                success.AddOption("Thank you.", pla =>
                {
                    return true;
                },
                pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the ingots. Your progress has been updated. Come back when you have delivered 1000 in total.");
                partial.AddOption("Understood.", pla =>
                {
                    return true;
                },
                pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need the ingots for?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I am gathering Agapite Ingots to fund my new shop. Once I have 1000 ingots, I'll be a shopkeeper selling Agapite Ingots along with ArmsLoreAugmentCrystal, WaterWell, RibbonAward, and RookStone. It’s an opportunity for you to acquire these rare items.");
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

    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my fine selection of goods.");
        // Open your standard vendor gump here.
        // For example:
        // player.SendGump(new VendorGump(player, this));
    }

    // Override to provide the vendor inventory.
    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBAgapiteVendor());
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

// Vendor inventory for the Agapite Collector shop.
public class SBAgapiteVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBAgapiteVendor()
    {
        // Note: Adjust prices, stock amounts, and item IDs as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AgapiteIngot), 10, 20, 0x1BF2, 0)); // AgapiteIngot
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ArmsLoreAugmentCrystal), 50, 10, 0x1F1C, 0)); // ArmsLoreAugmentCrystal
        m_BuyInfo.Add(new GenericBuyInfo(typeof(WaterWell), 100, 5, 0xA300, 0)); // WaterWell
        m_BuyInfo.Add(new GenericBuyInfo(typeof(RibbonAward), 25, 10, 0xA297, 0)); // RibbonAward
        m_BuyInfo.Add(new GenericBuyInfo(typeof(RookStone), 75, 5, 0xA583, 0)); // RookStone
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(AgapiteIngot), 5);
            Add(typeof(ArmsLoreAugmentCrystal), 25);
            Add(typeof(WaterWell), 50);
            Add(typeof(RibbonAward), 10);
            Add(typeof(RookStone), 35);
        }
    }
}
