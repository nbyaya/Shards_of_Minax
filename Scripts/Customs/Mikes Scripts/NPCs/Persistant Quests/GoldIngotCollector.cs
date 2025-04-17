using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class GoldIngotCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required number of GoldIngots

    [Constructable]
    public GoldIngotCollector() : base("the gold ingot collector")
    {
        Name = "Gold Collector Greg";
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

    public GoldIngotCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in 1000 GoldIngots.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.GoldIngotCollection))
            return false;

        return profile.Talents[TalentID.GoldIngotCollection].Points >= RequiredAmount;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If quest is complete, open the vendor shop.
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

    public override void VendorBuy(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I don't sell anything yet. Bring me the gold ingots first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Bring me the gold ingots first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings! I need 1000 Gold Ingots. Bring them to me and I will open my shop for you.");

        module.AddOption("I have collected the gold ingots.", p =>
        {
            // Option always available
            return true;
        },
        p =>
        {
            // Process turn-in of GoldIngots.
            if (ProcessPartialGoldIngots(p))
            {
                DialogueModule success = new DialogueModule("Excellent! You've brought all the gold ingots. My shop is now open for you.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the gold ingots. Keep going, you haven't reached 1000 yet.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need exactly?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 Gold Ingots. Once you have them, I will set up my shop where you can buy some rare items.");
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

    // Process partial GoldIngot turn-ins.
    // Returns true if the player's GoldIngotCollection reaches or exceeds the required amount.
    private bool ProcessPartialGoldIngots(PlayerMobile player)
    {
        int ingotsCount = 0;
        List<Item> ingotItems = new List<Item>();
        Container pack = player.Backpack;
        if (pack == null)
            return false;

        foreach (Item item in pack.Items)
        {
            if (item is GoldIngot)
            {
                ingotsCount += item.Amount;
                ingotItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.GoldIngotCollection))
            profile.Talents[TalentID.GoldIngotCollection] = new Talent(TalentID.GoldIngotCollection);

        int currentPoints = profile.Talents[TalentID.GoldIngotCollection].Points;
        int needed = RequiredAmount - currentPoints;
        if (needed <= 0)
            return true;

        int toTurnIn = Math.Min(needed, ingotsCount);
        RemoveItems(pack, ingotItems, toTurnIn);
        profile.Talents[TalentID.GoldIngotCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return IsQuestComplete(player);
    }

    // Helper method: Remove a specific amount from a list of items in a container.
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

    // Open the vendor gump; this also initializes the vendor inventory if needed.
    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my fine selection of items.");
        // You can open your standard vendor gump here if desired:
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBGoldVendor());
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



public class SBGoldVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBGoldVendor()
    {
        // The parameters for GenericBuyInfo: (Type, price, amount, itemID, hue)
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GoldIngot), 10, 20, 0x1BF2, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ArcheryAugmentCrystal), 50, 10, 0x1F1D, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(PlatinumChip), 75, 10, 0x14BC6, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(TribalHelm), 100, 5, 0xA1C7, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GlassTable), 200, 5, 0xA52E, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(GoldIngot), 5);
            Add(typeof(ArcheryAugmentCrystal), 25);
            Add(typeof(PlatinumChip), 35);
            Add(typeof(TribalHelm), 50);
            Add(typeof(GlassTable), 100);
        }
    }
}
