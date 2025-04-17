using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.ContextMenus;
using Server.Items;

public class LogCollector : BaseVendor
{
    private readonly List<SBInfo> m_SBInfos = new List<SBInfo>();
    private const int RequiredAmount = 100; // Required logs per type

    [Constructable]
    public LogCollector() : base("the log collector")
    {
        Name = "Lumberjack Lyle";
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

    public LogCollector(Serial serial) : base(serial)
    {
    }

    // Helper: Check if the player has turned in 100 of each log type.
    private bool IsQuestComplete(PlayerMobile player)
    {
        var profile = player.AcquireTalents();

        // Ensure all talents exist first; if not, quest isn't complete.
        if (!profile.Talents.ContainsKey(TalentID.LogCollectionRegular) ||
            !profile.Talents.ContainsKey(TalentID.LogCollectionHeartwood) ||
            !profile.Talents.ContainsKey(TalentID.LogCollectionBloodwood) ||
            !profile.Talents.ContainsKey(TalentID.LogCollectionFrostwood) ||
            !profile.Talents.ContainsKey(TalentID.LogCollectionOak) ||
            !profile.Talents.ContainsKey(TalentID.LogCollectionAsh) ||
            !profile.Talents.ContainsKey(TalentID.LogCollectionYew))
        {
            return false;
        }

        return (profile.Talents[TalentID.LogCollectionRegular].Points >= RequiredAmount &&
                profile.Talents[TalentID.LogCollectionHeartwood].Points >= RequiredAmount &&
                profile.Talents[TalentID.LogCollectionBloodwood].Points >= RequiredAmount &&
                profile.Talents[TalentID.LogCollectionFrostwood].Points >= RequiredAmount &&
                profile.Talents[TalentID.LogCollectionOak].Points >= RequiredAmount &&
                profile.Talents[TalentID.LogCollectionAsh].Points >= RequiredAmount &&
                profile.Talents[TalentID.LogCollectionYew].Points >= RequiredAmount);
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
            from.SendMessage("I don't sell anything yet. Bring me the logs first!");
            return;
        }
        base.VendorBuy(from);
    }

    public override void VendorSell(Mobile from)
    {
        if (!(from is PlayerMobile player) || !IsQuestComplete(player))
        {
            from.SendMessage("I'm not a vendor yet! Help me collect logs first.");
            return;
        }
        base.VendorSell(from);
    }

    private DialogueModule CreateDialogueModule()
    {
        DialogueModule module = new DialogueModule("Greetings, traveler! I need 100 of each kind of log. If you bring them, you'll unlock my shop for your use.");

        module.AddOption("I have collected the logs.", p => true, p =>
        {
            // Process whatever logs the player has in a partial turn-in.
            if (ProcessPartialLogs(p))
            {
                DialogueModule success = new DialogueModule("Outstanding work! You have turned in all required logs and unlocked my shop. Browse my wares at your leisure.");
                success.AddOption("Thank you.", pla => true, pla =>
                {
                    OpenVendorGump(pla);
                });
                p.SendGump(new DialogueGump(p, success));
            }
            else
            {
                DialogueModule partial = new DialogueModule("Thank you for the logs. Your progress has been updated. Come back when you have turned in 100 of each log type.");
                partial.AddOption("Understood.", pla => true, pla =>
                {
                    pla.SendGump(new DialogueGump(pla, CreateDialogueModule()));
                });
                p.SendGump(new DialogueGump(p, partial));
            }
        });

        module.AddOption("What kinds of logs do you need?", p => true, p =>
        {
            DialogueModule info = new DialogueModule("I require 100 each of these logs:\n- Regular Log\n- HeartwoodLog\n- BloodwoodLog\n- FrostwoodLog\n- OakLog\n- AshLog\n- YewLog");
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

    // Process partial log turn-ins.
    // Returns true if all log types have reached the required amount.
    private bool ProcessPartialLogs(PlayerMobile player)
    {
        // Dictionaries to hold available log counts and matching item lists.
        Dictionary<Type, int> availableLogs = new Dictionary<Type, int>()
        {
            { typeof(Log), 0 },
            { typeof(HeartwoodLog), 0 },
            { typeof(BloodwoodLog), 0 },
            { typeof(FrostwoodLog), 0 },
            { typeof(OakLog), 0 },
            { typeof(AshLog), 0 },
            { typeof(YewLog), 0 }
        };

        Dictionary<Type, List<Item>> logItems = new Dictionary<Type, List<Item>>()
        {
            { typeof(Log), new List<Item>() },
            { typeof(HeartwoodLog), new List<Item>() },
            { typeof(BloodwoodLog), new List<Item>() },
            { typeof(FrostwoodLog), new List<Item>() },
            { typeof(OakLog), new List<Item>() },
            { typeof(AshLog), new List<Item>() },
            { typeof(YewLog), new List<Item>() }
        };

        // Count logs in the player's backpack.
        foreach (Item item in player.Backpack.Items)
        {
            Type t = item.GetType();
            if (availableLogs.ContainsKey(t))
            {
                availableLogs[t] += item.Amount;
                logItems[t].Add(item);
            }
        }

        // Acquire the player's talent profile.
        var profile = player.AcquireTalents();

        // Ensure all talent entries exist.
        if (!profile.Talents.ContainsKey(TalentID.LogCollectionRegular))
            profile.Talents[TalentID.LogCollectionRegular] = new Talent(TalentID.LogCollectionRegular);
        if (!profile.Talents.ContainsKey(TalentID.LogCollectionHeartwood))
            profile.Talents[TalentID.LogCollectionHeartwood] = new Talent(TalentID.LogCollectionHeartwood);
        if (!profile.Talents.ContainsKey(TalentID.LogCollectionBloodwood))
            profile.Talents[TalentID.LogCollectionBloodwood] = new Talent(TalentID.LogCollectionBloodwood);
        if (!profile.Talents.ContainsKey(TalentID.LogCollectionFrostwood))
            profile.Talents[TalentID.LogCollectionFrostwood] = new Talent(TalentID.LogCollectionFrostwood);
        if (!profile.Talents.ContainsKey(TalentID.LogCollectionOak))
            profile.Talents[TalentID.LogCollectionOak] = new Talent(TalentID.LogCollectionOak);
        if (!profile.Talents.ContainsKey(TalentID.LogCollectionAsh))
            profile.Talents[TalentID.LogCollectionAsh] = new Talent(TalentID.LogCollectionAsh);
        if (!profile.Talents.ContainsKey(TalentID.LogCollectionYew))
            profile.Talents[TalentID.LogCollectionYew] = new Talent(TalentID.LogCollectionYew);

        // Process each log type.
        ProcessLogType<Log>(player, logItems, availableLogs, profile, TalentID.LogCollectionRegular);
        ProcessLogType<HeartwoodLog>(player, logItems, availableLogs, profile, TalentID.LogCollectionHeartwood);
        ProcessLogType<BloodwoodLog>(player, logItems, availableLogs, profile, TalentID.LogCollectionBloodwood);
        ProcessLogType<FrostwoodLog>(player, logItems, availableLogs, profile, TalentID.LogCollectionFrostwood);
        ProcessLogType<OakLog>(player, logItems, availableLogs, profile, TalentID.LogCollectionOak);
        ProcessLogType<AshLog>(player, logItems, availableLogs, profile, TalentID.LogCollectionAsh);
        ProcessLogType<YewLog>(player, logItems, availableLogs, profile, TalentID.LogCollectionYew);

        // Return true if all log types have reached the required amount.
        return IsQuestComplete(player);
    }

    // Helper method: For a given log type T, remove logs from the backpack and update talent progress.
    private void ProcessLogType<T>(PlayerMobile player, Dictionary<Type, List<Item>> logItems, Dictionary<Type, int> availableLogs, TalentProfile profile, TalentID talent)
        where T : Item
    {
        int currentPoints = profile.Talents[talent].Points;
        int needed = RequiredAmount - currentPoints;

        // If already complete or no logs available, do nothing.
        if (needed <= 0 || !availableLogs.ContainsKey(typeof(T)) || availableLogs[typeof(T)] <= 0)
            return;

        int toTurnIn = Math.Min(needed, availableLogs[typeof(T)]);
        // Remove logs from the backpack.
        RemoveItems(player.Backpack, logItems[typeof(T)], toTurnIn);
        // Update progress, capping at RequiredAmount.
        profile.Talents[talent].Points = Math.Min(currentPoints + toTurnIn, RequiredAmount);
    }

    // Remove a specific amount from a list of items in a container.
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

        player.SendMessage("Welcome to my shop! Browse my fine selection of wood.");
        // Optionally, open your standard vendor gump here.
        // e.g., player.SendGump(new VendorGump(player, this));
    }

    // Override to match BaseVendor's expected access.
    protected override List<SBInfo> SBInfos
    {
        get { return m_SBInfos; }
    }

    public override void InitSBInfo()
    {
        m_SBInfos.Clear();
        m_SBInfos.Add(new SBWoodVendor());
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write((int)0); // version
        // No global vendor flag is stored.
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
        // No global vendor flag to load.
    }
}



public class SBWoodVendor : SBInfo
{
    private List<GenericBuyInfo> m_BuyInfo = new List<GenericBuyInfo>();
    private IShopSellInfo m_SellInfo = new InternalSellInfo();

    public SBWoodVendor()
    {
        m_BuyInfo.Add(new GenericBuyInfo(typeof(OakLog), 5, 20, 0x1BDD, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(AshLog), 5, 20, 0x1BDD, 0));
        m_BuyInfo.Add(new GenericBuyInfo(typeof(YewLog), 5, 20, 0x1BDD, 0));
		m_BuyInfo.Add(new GenericBuyInfo(typeof(Log), 5, 20, 0x1BDD, 0));
		m_BuyInfo.Add(new GenericBuyInfo(typeof(HeartwoodLog), 5, 20, 0x1BDD, 0));
		m_BuyInfo.Add(new GenericBuyInfo(typeof(BloodwoodLog), 5, 20, 0x1BDD, 0));
		m_BuyInfo.Add(new GenericBuyInfo(typeof(FrostwoodLog), 5, 20, 0x1BDD, 0));
    }

    // If the base class does not define these as virtual properties,
    // you might simply expose them as new properties instead of overriding.
    public override List<GenericBuyInfo> BuyInfo { get { return m_BuyInfo; } }
    public override IShopSellInfo SellInfo { get { return m_SellInfo; } }

    public class InternalSellInfo : GenericSellInfo
    {
        public InternalSellInfo()
        {
            Add(typeof(OakLog), 2);
            Add(typeof(AshLog), 2);
            Add(typeof(YewLog), 2);
			Add(typeof(Log), 2);
			Add(typeof(HeartwoodLog), 2);
			Add(typeof(BloodwoodLog), 2);
			Add(typeof(FrostwoodLog), 2);
        }
    }
}