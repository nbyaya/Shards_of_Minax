using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;
using Server.Targeting;

public class StarSapphireCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required StarSapphire count

    [Constructable]
    public StarSapphireCollector() : base("the gem collector")
    {
        Name = "Gem Collector Gilda";
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

    public StarSapphireCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player's StarSapphire quest is complete.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.StarSapphireCollection))
            return false;

        return profile.Talents[TalentID.StarSapphireCollection].Points >= RequiredAmount;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // Process any partial turn-ins regardless
        if (!IsQuestComplete(player))
        {
            DialogueModule module = CreateDialogueModule();
            player.SendGump(new DialogueGump(player, module));
        }
        else
        {
            OpenVendorGump(player);
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
            from.SendMessage("I don't trade with you until you prove your worth. Bring me enough StarSapphire!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not open for business yet! Bring me enough StarSapphire first.");
            return;
        }
        base.VendorSell(from);
    }

    // Creates the dialogue module for players who haven't completed the quest.
    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, adventurer! I am in desperate need of StarSapphire. Bring me 1000 of these precious gems and I'll open my shop for you.");

        module.AddOption("I have some StarSapphire to turn in.", p =>
        {
            // Always allow this option.
            return true;
        },
        p =>
        {
            if (ProcessPartialSapphires(p))
            {
                DialogueModule success = new DialogueModule("Outstanding! You've turned in enough StarSapphire. My shop is now open to you.");
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
                DialogueModule partial = new DialogueModule("Thank you for the gems. Your progress has been updated. Come back when you've turned in 1000 StarSapphire.");
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

        module.AddOption("What do you need exactly?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 StarSapphire. Once you deliver them, I will transform into a shopkeeper offering StarSapphire along with some rare items.");
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

    // Process partial StarSapphire turn-ins.
    // Returns true if the player has reached the required amount.
    private bool ProcessPartialSapphires(PlayerMobile player)
    {
        int availableCount = 0;
        List<Item> sapphires = new List<Item>();

        // Count StarSapphire items in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            // Assumes your StarSapphire item has a type name of StarSapphire.
            if (item is StarSapphire)
            {
                availableCount += item.Amount;
                sapphires.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.StarSapphireCollection))
            profile.Talents[TalentID.StarSapphireCollection] = new Talent(TalentID.StarSapphireCollection);

        int currentPoints = profile.Talents[TalentID.StarSapphireCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0)
            return true;

        int toTurnIn = Math.Min(needed, availableCount);

        // Remove the turned in sapphires from the backpack.
        RemoveItems(player.Backpack, sapphires, toTurnIn);

        // Update the talent progress (capped at RequiredAmount).
        profile.Talents[TalentID.StarSapphireCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return profile.Talents[TalentID.StarSapphireCollection].Points >= RequiredAmount;
    }

    // Helper method: Remove a specific amount of items from a container.
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

        player.SendMessage("Thank you, friend. My shop is now open. Browse my wares!");
        // Optionally, you can open your standard vendor gump here.
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBStarSapphireVendor());
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

// Vendor inventory for the StarSapphire Collector after quest completion.
public class SBStarSapphireVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBStarSapphireVendor()
    {
        // Prices and stock amounts are examples; adjust as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(StarSapphire), 10, 50, 0x0F0D, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BloodSword), 500, 10, 0x1441, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(DeckOfMagicCards), 250, 10, 0x1F03, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(BrokenBottle), 50, 10, 0x0F08, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(FancyHornOfPlenty), 1000, 10, 0x0E41, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            // Example sell prices (adjust as needed).
            Add(typeof(StarSapphire), 5);
            Add(typeof(BloodSword), 250);
            Add(typeof(DeckOfMagicCards), 125);
            Add(typeof(BrokenBottle), 25);
            Add(typeof(FancyHornOfPlenty), 500);
        }
    }
}
