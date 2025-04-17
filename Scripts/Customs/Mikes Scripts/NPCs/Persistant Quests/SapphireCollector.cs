using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class SapphireCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000;

    [Constructable]
    public SapphireCollector() : base("the gem collector")
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

    public SapphireCollector(Serial serial) : base(serial)
    {
    }

    // Checks if the player has turned in 1000 Sapphire.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.SapphireCollection))
            return false;
        return profile.Talents[TalentID.SapphireCollection].Points >= RequiredAmount;
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
            from.SendMessage("I don't sell anything yet. Bring me the sapphire first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect the sapphire first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I am in dire need of 1000 Sapphire. Bring them to me and I shall open my shop for you.");

        module.AddOption("I have collected the sapphire.", p => true, p =>
        {
            // Process sapphire turn-ins from the player's backpack.
            if (ProcessPartialSapphire(p))
            {
                DialogueModule success = new DialogueModule("Outstanding work! You have turned in all 1000 Sapphire and unlocked my shop. Browse my wares at your leisure.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the sapphire. Your progress has been updated. Come back when you have turned in 1000 Sapphire.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 Sapphire. These precious gems are essential for my collection. Once complete, my shop will be at your service.");
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

    // Processes sapphire turn-ins from the player's backpack.
    private bool ProcessPartialSapphire(PlayerMobile player)
    {
        int available = 0;
        List<Item> sapphireItems = new List<Item>();

        // Count Sapphire in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is Sapphire)
            {
                available += item.Amount;
                sapphireItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();

        // Ensure the talent exists.
        if (!profile.Talents.ContainsKey(TalentID.SapphireCollection))
            profile.Talents[TalentID.SapphireCollection] = new Talent(TalentID.SapphireCollection);

        int currentPoints = profile.Talents[TalentID.SapphireCollection].Points;
        int needed = RequiredAmount - currentPoints;

        // Nothing to process.
        if (needed <= 0 || available <= 0)
            return false;

        int toTurnIn = Math.Min(needed, available);
        RemoveItems(player.Backpack, sapphireItems, toTurnIn);
        profile.Talents[TalentID.SapphireCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);
        return IsQuestComplete(player);
    }

    // Helper method to remove a set number of items from the player's backpack.
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

        player.SendMessage("Welcome to my shop! Browse my fine selection of gems and treasures.");
        // Optionally, you can open your standard vendor gump:
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    // Required by BaseVendor.
    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBGemVendor());
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

// Vendor inventory for the Sapphire Collector once the quest is complete.
public class SBGemVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBGemVendor()
    {
        // Note: Adjust the prices, stock amount, and item IDs as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Sapphire), 10, 20, 0x0F0E, 0)); // Sapphire item (itemID is a placeholder)
        m_BuyInfo.Add(new GenericBuyInfo(typeof(CapacityIncreaseDeed), 100, 20, 0x14F0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(LampPostC), 50, 20, 0x1C06, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BlueSand), 5, 20, 0x1D9B, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(FunMushroom), 20, 20, 0x1F0D, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(Sapphire), 5);
            Add(typeof(CapacityIncreaseDeed), 50);
            Add(typeof(LampPostC), 25);
            Add(typeof(BlueSand), 2);
            Add(typeof(FunMushroom), 10);
        }
    }
}
