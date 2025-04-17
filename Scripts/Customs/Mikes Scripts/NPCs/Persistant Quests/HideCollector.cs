using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class HideCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required hides

    [Constructable]
    public HideCollector() : base("the hide collector")
    {
        Name = "Hide Collector Hank";
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

    public HideCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has completed the hide collection quest
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.HideCollection))
            return false;
        return (profile.Talents[TalentID.HideCollection].Points >= RequiredAmount);
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
            from.SendMessage("I'm not a vendor yet! Help me collect hides first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I am in need of 1000 hides. Bring them to me and I'll open my shop for you.");

        module.AddOption("I have collected the hides.", p => true, p =>
        {
            // Process hide turn-in.
            if (ProcessPartialHides(p))
            {
                DialogueModule success = new DialogueModule("Excellent work! You've gathered all the hides. My shop is now open for business. Please, have a look at my wares.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thanks for the hides. Your progress has been updated. Return when you have collected 1000 hides in total.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What hides are you looking for?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 hides. Hides are typically obtained from slain beasts. Collect enough and you'll unlock my shop.");
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

    // Process partial hide turn-ins.
    // Returns true if the player has now turned in 1000 hides.
    private bool ProcessPartialHides(PlayerMobile player)
    {
        int availableHides = 0;
        List<Item> hideItems = new List<Item>();

        // Count hides in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is Hides) // Assuming Hide is your hide item type
            {
                availableHides += item.Amount;
                hideItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.HideCollection))
            profile.Talents[TalentID.HideCollection] = new Talent(TalentID.HideCollection);

        int currentPoints = profile.Talents[TalentID.HideCollection].Points;
        int needed = RequiredAmount - currentPoints;
        if (needed <= 0)
            return true;

        int toTurnIn = Math.Min(needed, availableHides);
        RemoveItems(player.Backpack, hideItems, toTurnIn);
        profile.Talents[TalentID.HideCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return IsQuestComplete(player);
    }

    // Helper: remove a specific number of hide items from the container.
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

        player.SendMessage("Welcome to my shop! Browse my fine selection of wares.");
        // Optionally, you could open your standard vendor gump here.
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBHideVendor());
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

// Vendor inventory for the Hide Collector NPC.
public class SBHideVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBHideVendor()
    {
        // Add items to be sold:
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Hides), 10, 20, 0x1079, 0)); // Hides
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BagOfHealth), 50, 20, 0x0E80, 0)); // BagOfHealth
        m_BuyInfo.Add(new GenericBuyInfo(typeof(TwentyfiveShield), 50, 20, 0x1BF2, 0)); // TwentyfiveShield
        m_BuyInfo.Add(new GenericBuyInfo(typeof(DaggerSign), 25, 20, 0x1F5D, 0)); // DaggerSign
        m_BuyInfo.Add(new GenericBuyInfo(typeof(MultiPump), 75, 20, 0x1EAD, 0)); // MultiPump
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(Hides), 5);
            Add(typeof(BagOfHealth), 25);
            Add(typeof(TwentyfiveShield), 25);
            Add(typeof(DaggerSign), 12);
            Add(typeof(MultiPump), 35);
        }
    }
}
