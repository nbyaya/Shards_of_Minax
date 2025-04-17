using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class ValoriteCollector : BaseVendor
{
    private const int RequiredAmount = 1000; // Number of ValoriteIngots required for quest completion
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();

    [Constructable]
    public ValoriteCollector() : base("the valorite collector")
    {
        Name = "Valorite Victor";
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

    public ValoriteCollector(Serial serial) : base(serial)
    {
    }

    // Helper: Check if the player has turned in 1000 ValoriteIngots.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        if (!profile.Talents.ContainsKey(TalentID.ValoriteCollection))
            profile.Talents[TalentID.ValoriteCollection] = new Talent(TalentID.ValoriteCollection);

        return (profile.Talents[TalentID.ValoriteCollection].Points >= RequiredAmount);
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If quest is complete, open vendor gump.
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
            from.SendMessage("I don't trade until I have received 1000 Valorite Ingots!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I am not trading until my quest is complete.");
            return;
        }
        base.VendorSell(from);
    }

    // Create branching dialogue for the Valorite quest.
    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, adventurer! I am collecting Valorite Ingots. Bring me 1000 of them, and my trading post will open to you.");
        
        module.AddOption("I have the valorite ingots.", p => true, p =>
        {
            // Process the player's ingots.
            if (ProcessPartialValorite(p))
            {
                DialogueModule success = new DialogueModule("Excellent work! You've delivered all 1000 Valorite Ingots. My shop is now open to you.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the ingots. Your progress has been updated. Return when you have gathered 1000 in total.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 Valorite Ingots. Gather them and I will become a shop where you can purchase rare items.");
            info.AddOption("Got it.", pla => true, pla =>
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

    // Process partial Valorite ingot turn-in.
    // Returns true if the total reaches the required amount.
    private bool ProcessPartialValorite(PlayerMobile player)
    {
        int availableIngots = 0;
        List<Item> valoriteItems = new List<Item>();

        // Count ValoriteIngots in player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is ValoriteIngot)
            {
                availableIngots += item.Amount;
                valoriteItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.ValoriteCollection))
            profile.Talents[TalentID.ValoriteCollection] = new Talent(TalentID.ValoriteCollection);

        int currentPoints = profile.Talents[TalentID.ValoriteCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0)
            return true;

        int toTurnIn = Math.Min(needed, availableIngots);

        // Remove ValoriteIngots from the backpack.
        RemoveItems(player.Backpack, valoriteItems, toTurnIn);

        // Update the player's persistent quest progress.
        profile.Talents[TalentID.ValoriteCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return profile.Talents[TalentID.ValoriteCollection].Points >= RequiredAmount;
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

        player.SendMessage("Welcome to my trading post! Browse my fine selection of rare items.");
        // Optionally, open your standard vendor gump here.
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBValoriteVendor());
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


public class SBValoriteVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBValoriteVendor()
    {
        // Adjust item IDs, prices, and stock amounts as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ValoriteIngot), 10, 20, 0x1BF2, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BagOfBombs), 50, 10, 0xE76, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(LayingChicken), 25, 10, 0xA514, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(EssenceOfToad), 75, 10, 0xE2A, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(SalvageMachine), 100, 5, 0xAC5A, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(ValoriteIngot), 5);
            Add(typeof(BagOfBombs), 25);
            Add(typeof(LayingChicken), 12);
            Add(typeof(EssenceOfToad), 35);
            Add(typeof(SalvageMachine), 50);
        }
    }
}
