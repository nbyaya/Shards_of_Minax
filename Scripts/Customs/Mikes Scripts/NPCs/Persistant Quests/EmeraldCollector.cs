using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class EmeraldCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required emerald count

    [Constructable]
    public EmeraldCollector() : base("the emerald collector")
    {
        Name = "Gem Collector Greg";
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

    public EmeraldCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in 1000 emeralds.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.EmeraldCollection))
            return false;
        return profile.Talents[TalentID.EmeraldCollection].Points >= RequiredAmount;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If quest complete, open vendor gump
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

    // Only show buy/sell options if the quest is complete.
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
            from.SendMessage("I don't sell anything yet. Bring me the emeralds first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect emeralds first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I'm in dire need of emeralds. Bring me 1000 emeralds, and I'll open my shop for you.");

        module.AddOption("I have collected the emeralds.", p =>
        {
            return true;
        }, p =>
        {
            if (ProcessEmeralds(p))
            {
                DialogueModule success = new DialogueModule("Outstanding work! You have turned in all required emeralds and unlocked my shop. Browse my wares at your leisure.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the emeralds. Your progress has been updated. Come back when you have turned in 1000 emeralds.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What kind of emeralds do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 emeralds. Simply gather them from the world and bring them to me.");
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

    // Process emerald turn-ins from the player's backpack.
    // Returns true if the quest is complete.
    private bool ProcessEmeralds(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        // Ensure the emerald talent exists.
        if (!profile.Talents.ContainsKey(TalentID.EmeraldCollection))
            profile.Talents[TalentID.EmeraldCollection] = new Talent(TalentID.EmeraldCollection);

        int currentPoints = profile.Talents[TalentID.EmeraldCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0)
            return true;

        int available = 0;
        List<Item> emeraldItems = new List<Item>();

        // Look for emeralds in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is Emerald)
            {
                available += item.Amount;
                emeraldItems.Add(item);
            }
        }

        if (available <= 0)
            return false;

        int toTurnIn = Math.Min(needed, available);
        RemoveItems(player.Backpack, emeraldItems, toTurnIn);

        profile.Talents[TalentID.EmeraldCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);
        return profile.Talents[TalentID.EmeraldCollection].Points >= RequiredAmount;
    }

    // Remove a given number of items from a container.
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

    // Open the vendor gump (or vendor interface) when quest is complete.
    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my fine selection of emerald-related items.");
        // Optionally, you can open your standard vendor gump here.
        // For example: player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBEmeraldVendor());
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

// Vendor inventory for the Emerald NPC.
public class SBEmeraldVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBEmeraldVendor()
    {
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Emerald), 50, 20, 0x0F0D, 0)); // Adjust itemID as needed
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BraceletSlotChangeDeed), 100, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(FineGoldWire), 75, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(WaterRelic), 200, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(EnchantedAnnealer), 150, 10, 0x0, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(Emerald), 25);
            Add(typeof(BraceletSlotChangeDeed), 50);
            Add(typeof(FineGoldWire), 35);
            Add(typeof(WaterRelic), 100);
            Add(typeof(EnchantedAnnealer), 75);
        }
    }
}
