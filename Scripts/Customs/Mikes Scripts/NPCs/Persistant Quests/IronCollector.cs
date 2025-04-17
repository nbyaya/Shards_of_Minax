using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class IronCollector : BaseVendor
{
    private List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // 1000 IronIngots required

    [Constructable]
    public IronCollector() : base("the ingot collector")
    {
        Name = "Tinkerer Troy";
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

    public IronCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in enough IronIngots.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.IronIngotCollection))
            profile.Talents[TalentID.IronIngotCollection] = new Talent(TalentID.IronIngotCollection);

        return profile.Talents[TalentID.IronIngotCollection].Points >= RequiredAmount;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        // If quest is complete, open vendor gump
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

    // Only show buy/sell context menu options if the quest is complete.
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
            from.SendMessage("I cannot deal with you until you provide the ingots!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I am not trading yet. Bring me 1000 Iron Ingots first.");
            return;
        }
        base.VendorSell(from);
    }

    // Create the dialogue module for the NPC
    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, adventurer! I am in need of 1000 Iron Ingots. " +
            "If you can bring me that many, I'll convert myself into a merchant and sell you rare items.");
        
        module.AddOption("I have collected the ingots.", p =>
        {
            return true;
        }, p =>
        {
            if (ProcessPartialIngots(p))
            {
                DialogueModule complete = new DialogueModule("Excellent work! You have delivered all 1000 Iron Ingots. " +
                    "Now, I am a vendor. Browse my wares at your leisure.");
                complete.AddOption("Thank you.", pla =>
                {
                    return true;
                }, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, complete));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the ingots. Your progress has been updated. " +
                    "Return when you have turned in a total of 1000 Iron Ingots.");
                partial.AddOption("Understood.", pla =>
                {
                    return true;
                }, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("Tell me more about this quest.", p =>
        {
            return true;
        }, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 Iron Ingots. " +
                "Simply gather the ingots and hand them over to me. " +
                "Once you’ve done so, I will transform into a vendor that sells not only Iron Ingots " +
                "but also rare items like Random Magic Weapons, Mah Jong Tiles, Alchemy Talismans, and Cartography Tables.");
            info.AddOption("Understood.", pla =>
            {
                return true;
            }, pla =>
            {
                pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
            });
            p.SendGump(new DialogueGump(p, info));
        });

        module.AddOption("I must be going.", p =>
        {
            return true;
        }, p =>
        {
            p.SendMessage("Safe travels, adventurer.");
        });

        return module;
    }

    // Process ingot turn-ins from the player's backpack.
    // Returns true if the player has turned in a total of 1000 ingots.
    private bool ProcessPartialIngots(PlayerMobile player)
    {
        int availableIngots = 0;
        List<Item> ingotItems = new List<Item>();

        // Count IronIngots in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            if (item is IronIngot)
            {
                availableIngots += item.Amount;
                ingotItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.IronIngotCollection))
            profile.Talents[TalentID.IronIngotCollection] = new Talent(TalentID.IronIngotCollection);

        int currentPoints = profile.Talents[TalentID.IronIngotCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0 || availableIngots <= 0)
            return currentPoints >= RequiredAmount;

        int toTurnIn = Math.Min(needed, availableIngots);
        RemoveItems(player.Backpack, ingotItems, toTurnIn);
        profile.Talents[TalentID.IronIngotCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return profile.Talents[TalentID.IronIngotCollection].Points >= RequiredAmount;
    }

    // Remove a specific amount of ingots from the provided list of items.
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

    // Open the vendor gump – and initialize the vendor if necessary.
    private void OpenVendorGump(PlayerMobile player)
    {
        if (m_SBInfos.Count == 0)
            InitSBInfo();

        player.SendMessage("Welcome to my shop! Browse my fine selection of wares.");
        // You can also open your standard vendor gump here, for example:
        // player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos { get { return m_SBInfos; } }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBIngotVendor());
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


public class SBIngotVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBIngotVendor()
    {
        // The vendor will now sell:
        // IronIngot, RandomMagicWeapon, MahJongTile, AlchemyTalisman, and CartographyTable.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(IronIngot), 10, 20, 0x1BF2, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(MahJongTile), 50, 20, 0x1422, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AlchemyTalisman), 150, 10, 0x9E26, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(CartographyTable), 200, 5, 0xA849, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            // Set sell prices for the items.
            Add(typeof(IronIngot), 5);
            Add(typeof(MahJongTile), 25);
            Add(typeof(AlchemyTalisman), 75);
            Add(typeof(CartographyTable), 100);
        }
    }
}