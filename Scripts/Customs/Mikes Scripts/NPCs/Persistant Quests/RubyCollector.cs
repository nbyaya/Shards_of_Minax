using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class RubyCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required number of Ruby

    [Constructable]
    public RubyCollector() : base("the ruby collector")
    {
        Name = "Ruby Collector Rocco";
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

    public RubyCollector(Serial serial) : base(serial)
    {
    }

    // Helper: Check if the player has turned in 1000 Ruby.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        // Ensure RubyCollection talent exists.
        if (!profile.Talents.ContainsKey(TalentID.RubyCollection))
            profile.Talents[TalentID.RubyCollection] = new Talent(TalentID.RubyCollection);

        return profile.Talents[TalentID.RubyCollection].Points >= RequiredAmount;
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
            from.SendMessage("I don't sell anything yet. Bring me the rubies first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect rubies first.");
            return;
        }
        base.VendorSell(from);
    }

    // Process partial Ruby turn-ins.
    // Returns true if the player now has reached the required amount.
    private bool ProcessPartialRubies(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        int currentPoints = profile.Talents[TalentID.RubyCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0)
            return true;

        int available = 0;
        List<Item> rubyItems = new List<Item>();

        // Count Ruby items in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is Ruby)
            {
                available += item.Amount;
                rubyItems.Add(item);
            }
        }

        int toTurnIn = Math.Min(needed, available);
        RemoveItems(player.Backpack, rubyItems, toTurnIn);

        profile.Talents[TalentID.RubyCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);
        return IsQuestComplete(player);
    }

    // Remove a specific amount of items from the container.
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

    // Create the branching dialogue for the Ruby quest.
    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, adventurer! I am in dire need of 1000 Rubies. Bring me enough and I will transform into a shopkeeper with exclusive wares.");
        
        module.AddOption("I have collected the rubies.", p =>
        {
            return true;
        },
        p =>
        {
            if (ProcessPartialRubies(p))
            {
                DialogueModule success = new DialogueModule("Marvelous! You have provided all the Rubies I need. Now, feel free to browse my shop.");
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
                DialogueModule partial = new DialogueModule("Thank you for the rubies. Your progress has been updated. Return when you've provided a total of 1000 Rubies.");
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

        module.AddOption("What are these rubies for?", p =>
        {
            return true;
        },
        p =>
        {
            DialogueModule info = new DialogueModule("I collect Rubies to fund my secret shop. Once you help me gather 1000 Rubies, I'll be able to offer not only Rubies but also GlovesOfCommand, ReactiveHormones, CandleStick, and LuckyDice.");
            info.AddOption("Understood.", pla =>
            {
                return true;
            },
            pla =>
            {
                pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
            });
            p.SendGump(new DialogueGump(p, info));
        });

        module.AddOption("I must be going.", p => true, p =>
        {
            p.SendMessage("Safe travels, adventurer.");
        });

        return module;
    }

    // Open the vendor shop for players who have completed the quest.
    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my selection of treasures.");
        // Optionally, open your standard vendor gump here.
        // For example: player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBRubyVendor());
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

// Vendor inventory for the Ruby Collector when in shopkeeper mode.
public class SBRubyVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBRubyVendor()
    {
        // Adjust prices, amounts, and item IDs to suit your shard.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Ruby), 10, 20, 0x0F13, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GlovesOfCommand), 5000, 20, 0x13B5, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ReactiveHormones), 2000, 20, 0x0F0C, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(CandleStick), 1500, 20, 0xA0C, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(LuckyDice), 1000, 20, 0x0E39, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(Ruby), 5);
            Add(typeof(GlovesOfCommand), 2500);
            Add(typeof(ReactiveHormones), 1000);
            Add(typeof(CandleStick), 750);
            Add(typeof(LuckyDice), 500);
        }
    }
}
