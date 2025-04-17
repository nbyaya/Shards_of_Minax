using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class SpinedHideCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // 1000 SpinedHides required

    [Constructable]
    public SpinedHideCollector() : base("the hide collector")
    {
        Name = "Hide Handler Horace";
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

    public SpinedHideCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has collected enough SpinedHides
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        if (!profile.Talents.ContainsKey(TalentID.SpinedHideCollection))
            return false;

        return (profile.Talents[TalentID.SpinedHideCollection].Points >= RequiredAmount);
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

    // Only show vendor context menu entries if the quest is complete.
    public override void GetContextMenuEntries(Mobile from, List<ContextMenuEntry> list)
    {
        base.GetContextMenuEntries(from, list);
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            list.RemoveAll(entry => entry is VendorBuyEntry || entry is VendorSellEntry);
        }
    }

    // Prevent vendor actions unless quest is complete.
    public override void VendorBuy(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I don't sell hides yet. Bring me the hides first!");
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

    // Create the branching dialogue using your DialogueModule
    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I'm in need of 1000 Spined Hides. Bring me the hides and I'll open my shop for you.");

        module.AddOption("I have collected the hides.", p => true, p =>
        {
            // Process turn-in
            if (ProcessPartialHides(p))
            {
                DialogueModule success = new DialogueModule("Excellent! You've provided all 1000 Spined Hides. My shop is now open for you to browse my wares.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the hides. Your progress has been updated. Return when you have collected 1000 Spined Hides.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What are Spined Hides?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("Spined Hides are tough animal hides prized for crafting. Bring me 1000 and my shop will open for you.");
            info.AddOption("Understood.", pla => true, pla =>
            {
                pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
            });
            p.SendGump(new DialogueGump(p, info));
        });

        module.AddOption("I must be going.", p => true, p =>
        {
            p.SendMessage("Farewell, traveler.");
        });

        return module;
    }

    // Process the player's SpinedHide turn-in
    private bool ProcessPartialHides(PlayerMobile player)
    {
        Dictionary<Type, int> availableHides = new Dictionary<Type, int>()
        {
            { typeof(SpinedHides), 0 }
        };

        Dictionary<Type, List<Item>> hideItems = new Dictionary<Type, List<Item>>()
        {
            { typeof(SpinedHides), new List<Item>() }
        };

        // Count SpinedHides in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            Type t = item.GetType();
            if (availableHides.ContainsKey(t))
            {
                availableHides[t] += item.Amount;
                hideItems[t].Add(item);
            }
        }

        var profile = player.AcquireTalents();

        if (!profile.Talents.ContainsKey(TalentID.SpinedHideCollection))
            profile.Talents[TalentID.SpinedHideCollection] = new Talent(TalentID.SpinedHideCollection);

        ProcessHideType<SpinedHides>(player, hideItems, availableHides, profile, TalentID.SpinedHideCollection);

        return IsQuestComplete(player);
    }

    // Helper: Process a specific hide type turn-in
    private void ProcessHideType<T>(PlayerMobile player, Dictionary<Type, List<Item>> hideItems, Dictionary<Type, int> availableHides, TalentProfile profile, TalentID talent)
        where T : Item
    {
        int currentPoints = profile.Talents[talent].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0 || !availableHides.ContainsKey(typeof(T)) || availableHides[typeof(T)] <= 0)
            return;

        int toTurnIn = Math.Min(needed, availableHides[typeof(T)]);
        RemoveItems(player.Backpack, hideItems[typeof(T)], toTurnIn);
        profile.Talents[talent].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);
    }

    // Helper: Remove a specified amount of items from a container.
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

    // Open the vendor gump once the quest is complete.
    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my selection of hides and special items.");
        // Optionally, you could open your standard vendor gump here:
        // player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBSpinedHideVendor());
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

// Vendor inventory for the new NPC.
public class SBSpinedHideVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBSpinedHideVendor()
    {
        // Adjust item IDs and prices as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(SpinedHides), 10, 20, 0x1BF1, 0)); // SpinedHide
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BagOfJuice), 25, 20, 0x9C1, 0));    // BagOfJuice
        m_BuyInfo.Add(new GenericBuyInfo(typeof(HeartPillow), 30, 20, 0x1B0C, 0));   // HeartPillow
        m_BuyInfo.Add(new GenericBuyInfo(typeof(HydroxFluid), 50, 20, 0x1B0D, 0));   // HydroxFluid
        m_BuyInfo.Add(new GenericBuyInfo(typeof(MasterCello), 100, 20, 0x1B0E, 0));  // MasterCello
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(SpinedHides), 5);
            Add(typeof(BagOfJuice), 10);
            Add(typeof(HeartPillow), 15);
            Add(typeof(HydroxFluid), 25);
            Add(typeof(MasterCello), 50);
        }
    }
}
