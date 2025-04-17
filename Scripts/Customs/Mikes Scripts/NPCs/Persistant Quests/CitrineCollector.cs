using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;
using Server.Targeting;

public class CitrineCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 1000; // Required Citrine

    [Constructable]
    public CitrineCollector() : base("the citrine collector")
    {
        Name = "Gavin the Gem-Hunter";
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

    public CitrineCollector(Serial serial) : base(serial)
    {
    }

    // Check if the player has turned in enough Citrine.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();
        if (!profile.Talents.ContainsKey(TalentID.CitrineCollection))
            return false;
        return (profile.Talents[TalentID.CitrineCollection].Points >= RequiredAmount);
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        if (IsQuestComplete(player))
        {
            // Quest complete; open vendor shop.
            OpenVendorGump(player);
        }
        else
        {
            // Quest not complete; open dialogue.
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
            from.SendMessage("I am not ready to trade until you gather enough citrine!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I am not trading yet! Please bring me enough citrine first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I am in need of 1000 pieces of citrine. Bring me the citrine and I will open my shop to you.");
        
        module.AddOption("I have collected the citrine.", p => true, p =>
        {
            if (ProcessPartialCitrine(p))
            {
                DialogueModule success = new DialogueModule("Excellent work! You have delivered all 1000 pieces of citrine. My shop is now open for your trading pleasure.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                int current = p.AcquireTalents().Talents[TalentID.CitrineCollection].Points;
                int needed = RequiredAmount - current;
                DialogueModule partial = new DialogueModule($"Thank you for the citrine. You have turned in {current} pieces. Bring me {needed} more to unlock my shop.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What do you need exactly?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 1000 pieces of citrine. Once you have gathered them all, my shop will open. I sell fine citrine along with unique items such as a JesterHatOfCommand, JudasCradle, GlassFurnace, CupOfSlime, GingerbreadCookie, CarvingMachine, and LovelyLilies.");
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

    // Process partial citrine turn-ins.
    // Returns true if the player has reached the required amount.
    private bool ProcessPartialCitrine(PlayerMobile player)
    {
        // Count the citrine in the player's backpack.
        int available = 0;
        List<Item> citrineItems = new List<Item>();
        foreach (Item item in player.Backpack.Items)
        {
            if (item is Citrine)
            {
                available += item.Amount;
                citrineItems.Add(item);
            }
        }

        var profile = player.AcquireTalents();
        // Ensure the talent exists (it should from the acquire call)
        if (!profile.Talents.ContainsKey(TalentID.CitrineCollection))
            profile.Talents[TalentID.CitrineCollection] = new Talent(TalentID.CitrineCollection);

        int currentPoints = profile.Talents[TalentID.CitrineCollection].Points;
        int needed = RequiredAmount - currentPoints;

        if (needed <= 0 || available <= 0)
            return currentPoints >= RequiredAmount;

        int toTurnIn = Math.Min(needed, available);
        RemoveItems(player.Backpack, citrineItems, toTurnIn);

        profile.Talents[TalentID.CitrineCollection].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);

        return profile.Talents[TalentID.CitrineCollection].Points >= RequiredAmount;
    }

    // Remove a specified amount from a list of items in a container.
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

        player.SendMessage("Welcome to my shop! Browse my selection of fine goods.");
        // Here you can open your standard vendor gump.
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBCitrineVendor());
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

// Vendor inventory for the CitrineCollector NPC.
public class SBCitrineVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBCitrineVendor()
    {
        // BuyInfo: adjust prices and stock as needed.
        m_BuyInfo.Add(new GenericBuyInfo(typeof(Citrine), 10, 50, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(JesterHatOfCommand), 1500, 10, 0x1711, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(JudasCradle), 2000, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GlassFurnace), 2500, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(CupOfSlime), 3000, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(GingerbreadCookie), 50, 20, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(CarvingMachine), 4000, 10, 0x0, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(LovelyLilies), 1000, 10, 0x0, 0));
    }

    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(Citrine), 5);
            Add(typeof(JesterHatOfCommand), 750);
            Add(typeof(JudasCradle), 1000);
            Add(typeof(GlassFurnace), 1250);
            Add(typeof(CupOfSlime), 1500);
            Add(typeof(GingerbreadCookie), 25);
            Add(typeof(CarvingMachine), 2000);
            Add(typeof(LovelyLilies), 500);
        }
    }
}
