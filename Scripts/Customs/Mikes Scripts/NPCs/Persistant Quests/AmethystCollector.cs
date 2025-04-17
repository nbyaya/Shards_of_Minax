using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class AmethystCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required Amethyst count

    [Constructable]
    public AmethystCollector() : base("the amethyst collector")
    {
        Name = "Gem Hunter Galen";
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

    public AmethystCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in 1000 Amethyst.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.AmethystCollection))
            return false;

        return profile.Talents[TalentID.AmethystCollection].Points >= RequiredAmount;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If quest complete, open vendor gump; otherwise, initiate dialogue.
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

    // Only show buy/sell options if quest is complete.
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
            from.SendMessage("I don't sell anything yet. Bring me the Amethyst first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect Amethyst first.");
            return;
        }
        base.VendorSell(from);
    }

    // Create the dialogue module for branch dialogue.
    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I seek 1000 pieces of Amethyst. Bring me enough and I'll open my shop for you.");
        
        module.AddOption("I have collected the Amethyst.", p => true, p =>
        {
            // Process turn-in and check if the quest is complete.
            if (ProcessPartialAmethyst(p))
            {
                DialogueModule success = new DialogueModule("Outstanding! You have provided all 1000 pieces of Amethyst. My shop is now open to you.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the Amethyst. Your progress has been updated. Come back when you have 1000 pieces.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 pieces of Amethyst. Gather them from the depths and bring them to me.");
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

    // Process the turn-in of Amethyst pieces.
    // Returns true if the total turned in meets the requirement.
    private bool ProcessPartialAmethyst(PlayerMobile player)
    {
        int currentPoints = player.AcquireTalents().Talents[TalentID.AmethystCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0)
            return true;

        int available = 0;
        List<Item> amethystItems = new List<Item>();

        // Look for Amethyst items in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is Amethyst)
            {
                available += item.Amount;
                amethystItems.Add(item);
            }
        }

        if (available <= 0)
            return false;

        int toTurnIn = Math.Min(needed, available);
        RemoveItems(player.Backpack, amethystItems, toTurnIn);

        var profile = player.AcquireTalents();
        profile.Talents[TalentID.AmethystCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);
        return profile.Talents[TalentID.AmethystCollection].Points >= RequiredAmount;
    }

    // Remove a specified number of items from the player's backpack.
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

    // Open the vendor gump (or trigger the vendor window) after quest completion.
    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my selection of rare gems and treasures.");
        // Optionally, open your standard vendor gump here.
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBAmethystVendor());
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

// Vendor inventory for the Amethyst collector NPC (shopkeeper)
public class SBAmethystVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBAmethystVendor()
    {
        // Adjust prices, amounts, and item IDs as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Amethyst), 10, 20, 0x0, 0)); // Amethyst
        m_BuyInfo.Add(new GenericBuyInfo(typeof(MirrorOfKalandra), 500, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GargoyleLamp), 200, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AnimalTopiary), 150, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(TinCowbell), 50, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(WoodAlchohol), 100, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(ChocolateFountain), 250, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(PowerGem), 1000, 20, 0x0, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(Amethyst), 5);
            Add(typeof(MirrorOfKalandra), 250);
            Add(typeof(GargoyleLamp), 100);
            Add(typeof(AnimalTopiary), 75);
            Add(typeof(TinCowbell), 25);
            Add(typeof(WoodAlchohol), 50);
            Add(typeof(ChocolateFountain), 125);
            Add(typeof(PowerGem), 500);
        }
    }
}
