using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class TourmalineCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required Tourmaline

    [Constructable]
    public TourmalineCollector() : base("the tourmaline collector")
    {
        Name = "Trader Toby";
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

    public TourmalineCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in 1000 Tourmaline.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        // Ensure the Tourmaline talent exists first; if not, quest isn't complete.
        if (!profile.Talents.ContainsKey(TalentID.TourmalineCollection))
            return false;

        return (profile.Talents[TalentID.TourmalineCollection].Points >= RequiredAmount);
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
            from.SendMessage("I don't sell anything yet. Bring me the Tourmaline first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect Tourmaline first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, adventurer! I'm in desperate need of Tourmaline. Bring me 1000 pieces, and I'll open my shop for you.");

        module.AddOption("I have collected the Tourmaline.", p =>
        {
            // This option is available regardless of progress.
            return true;
        },
        p =>
        {
            // Process the Tourmaline turn-in.
            if (ProcessPartialTourmaline(p))
            {
                DialogueModule success = new DialogueModule("Excellent work! You have provided all 1000 Tourmaline. My shop is now open for your browsing.");
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
                DialogueModule partial = new DialogueModule("Thank you for the Tourmaline. Your progress has been updated. Come back once you have turned in a total of 1000 pieces.");
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

        module.AddOption("What are you collecting?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 pieces of Tourmaline. The more you bring, the closer you get to unlocking my shop.");
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
            p.SendMessage("Safe travels, friend.");
        });

        return module;
    }

    // Process partial Tourmaline turn-ins.
    // Returns true if the player has reached 1000 Tourmaline.
    private bool ProcessPartialTourmaline(PlayerMobile player)
    {
        int available = 0;
        List<Item> tourmalineItems = new List<Item>();

        // Count Tourmaline in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is Tourmaline) // Assumes Tourmaline is an Item subclass.
            {
                available += item.Amount;
                tourmalineItems.Add(item);
            }
        }

        // Acquire the player's talent profile.
        var profile = player.AcquireTalents();

        // Ensure the Tourmaline talent exists.
        if (!profile.Talents.ContainsKey(TalentID.TourmalineCollection))
            profile.Talents[TalentID.TourmalineCollection] = new Talent(TalentID.TourmalineCollection);

        int currentPoints = profile.Talents[TalentID.TourmalineCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0 || available <= 0)
            return false;

        int toTurnIn = Math.Min(needed, available);

        // Remove items from the backpack.
        RemoveItems(player.Backpack, tourmalineItems, toTurnIn);

        // Update progress, capping at RequiredAmount.
        profile.Talents[TalentID.TourmalineCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return profile.Talents[TalentID.TourmalineCollection].Points >= RequiredAmount;
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

    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my wares.");
        // Optionally, open your standard vendor gump here.
        // For example: player.SendGump(new VendorGump(player, this));
    }

    // Override to match BaseVendor's expected access.
    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBTourmalineVendor());
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

// Vendor inventory for the Tourmaline NPC.
public class SBTourmalineVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBTourmalineVendor()
    {
        // Add items to sell. Assumes that the classes below exist.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Tourmaline), 10, 20, 0x0F0D, 0)); // Tourmaline
        m_BuyInfo.Add(new GenericBuyInfo(typeof(PlateLeggingsOfCommand), 500, 10, 0x141E, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AtomicRegulator), 750, 10, 0x1F2A, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(JesterSkull), 300, 10, 0x1F0B, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GamerGirlFeet), 300, 10, 0x170C, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(HildebrandtTapestry), 1000, 10, 0x10EA, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AnimalBox), 150, 10, 0x9A3, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BakingBoard), 200, 10, 0x1EBA, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(Tourmaline), 5);
            Add(typeof(PlateLeggingsOfCommand), 250);
            Add(typeof(AtomicRegulator), 375);
            Add(typeof(JesterSkull), 150);
            Add(typeof(GamerGirlFeet), 150);
            Add(typeof(HildebrandtTapestry), 500);
            Add(typeof(AnimalBox), 75);
            Add(typeof(BakingBoard), 100);
        }
    }
}
