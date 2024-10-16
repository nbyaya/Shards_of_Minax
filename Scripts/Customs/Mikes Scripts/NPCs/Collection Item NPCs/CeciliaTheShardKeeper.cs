using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class CeciliaTheShardKeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public CeciliaTheShardKeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Cecilia the Shard Keeper";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(70);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new HoodedShroudOfShadows()); // Hooded robe with a dark mysterious hue
        AddItem(new Sandals(1109)); // Sandals with a deep blue hue
        AddItem(new GoldBracelet()); // Golden bracelet for a touch of mysticism
        AddItem(new GnarledStaff()); // Staff with an ancient look to reflect her knowledge of the shards

        VirtualArmor = 15;
    }

    public CeciliaTheShardKeeper(Serial serial) : base(serial)
    {
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!(from is PlayerMobile player))
            return;

        DialogueModule greetingModule = CreateGreetingModule(player);
        player.SendGump(new DialogueGump(player, greetingModule));
    }

    private DialogueModule CreateGreetingModule(PlayerMobile player)
    {
        DialogueModule greeting = new DialogueModule("Ah, traveler... I sense you carry the weight of many adventures. I am Cecilia, keeper of the shards, protector of forgotten knowledge. What brings you to my humble sanctum?");

        // Start with dialogue about her purpose
        greeting.AddOption("Who are you, and what is a Shard Keeper?",
            p => true,
            p =>
            {
                DialogueModule shardKeeperModule = new DialogueModule("I am one of the few who have studied the ancient WorldShards and their mysteries. These shards are fragments of power, each connected to the threads of reality itself. They hold secrets, ones that only those resourceful enough can hope to uncover. I have lived my life adapting to ever-changing challenges, and now I use my knowledge to guide others.");

                shardKeeperModule.AddOption("Tell me about your life. Why are you called the Shard Keeper?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule backstoryModule = new DialogueModule("I come from a place far away, a flooded land where the waters rose and swallowed everything. I lived on a makeshift raft, gathering those who were lost, those the world had forgotten. Together, we survived, floating from place to place. We became a community of outcasts, resourceful and adaptable, and I taught them what I knew. We shared everything—food, shelter, and secrets. When I found the first shard, I felt its power, and I knew it could help us. That is why I am called the Shard Keeper.");
                        backstoryModule.AddOption("How did you survive on a raft?",
                            pla => true,
                            pla =>
                            {
                                DialogueModule survivalModule = new DialogueModule("It wasn't easy. We had to be inventive. The raft was built from scraps—wood, cloth, anything that floated. I taught my people how to fish, to collect rainwater, and to use the plants that grew on the scattered patches of dry land. We learned to make tools from bones and stones, to find medicine in the marshes, and to always look out for each other. Adapting was the only way we could survive. Resourcefulness kept us alive.");
                                survivalModule.AddOption("It sounds like you were a leader.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule leadershipModule = new DialogueModule("I suppose I was, though I never thought of myself as one. I was simply doing what needed to be done. Leadership isn't about power; it's about helping others find their own strength. I wanted everyone to feel they had a place, that they could contribute. In the end, we were stronger together because we all had a role to play.");
                                        leadershipModule.AddOption("You must be very wise to have guided so many.",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule wisdomModule = new DialogueModule("Wisdom comes from experience, from the mistakes we make and the lessons we learn. I have made many mistakes, but each one taught me something valuable. The shards have their own lessons too—lessons of patience, adaptability, and understanding. That is why I am here now, to share what I have learned with those who seek it.");
                                                wisdomModule.AddOption("Thank you for sharing your story.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, wisdomModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, leadershipModule));
                                    });
                                pla.SendGump(new DialogueGump(pla, survivalModule));
                            });
                        backstoryModule.AddOption("Tell me more about the shards.",
                            pla => true,
                            pla =>
                            {
                                DialogueModule shardDetailModule = new DialogueModule("The shards are fragments of something much greater—a world that once was. They pulse with energy, and each one has a unique power. Some can heal, some can protect, and others can reveal hidden truths. Collecting them is dangerous, as they attract those who would use their power for ill. But to those who are pure of heart and resourceful, they offer great potential.");
                                shardDetailModule.AddOption("Is it dangerous to collect shards?",
                                    plaa => true,
                                    plaa =>
                                    {
                                        DialogueModule dangerModule = new DialogueModule("Yes, very dangerous. The shards are guarded by creatures drawn to their power. The flooded lands were full of such guardians—strange beings that lurked in the depths, ready to strike. Many of my companions were injured, and some did not survive. It is not a task for the faint of heart. But for those who are willing to take the risk, the rewards are immense.");
                                        dangerModule.AddOption("What kind of rewards?",
                                            plaaa => true,
                                            plaaa =>
                                            {
                                                DialogueModule rewardsModule = new DialogueModule("The rewards vary. Some shards can enhance one's abilities, making you faster, stronger, or more resilient. Others have more subtle powers—visions, insights, or even glimpses into possible futures. I once knew a man who touched a shard and was granted the power to breathe underwater. He became our scout, swimming ahead to find safe routes for our raft. Each shard is unique.");
                                                rewardsModule.AddOption("I see. It must take great courage to seek them.",
                                                    plaaaa => true,
                                                    plaaaa =>
                                                    {
                                                        plaaaa.SendGump(new DialogueGump(plaaaa, CreateGreetingModule(plaaaa)));
                                                    });
                                                plaaa.SendGump(new DialogueGump(plaaa, rewardsModule));
                                            });
                                        plaa.SendGump(new DialogueGump(plaa, dangerModule));
                                    });
                                shardDetailModule.AddOption("Thank you for telling me about the shards.",
                                    plaa => true,
                                    plaa =>
                                    {
                                        plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                                    });
                                pla.SendGump(new DialogueGump(pla, shardDetailModule));
                            });
                        pl.SendGump(new DialogueGump(pl, backstoryModule));
                    });

                shardKeeperModule.AddOption("Yes, I have a ShardCrest.",
                    pl => HasShardCrest(pl) && CanTradeWithPlayer(pl),
                    pl =>
                    {
                        CompleteTrade(pl);
                    });
                shardKeeperModule.AddOption("No, I do not have one.",
                    pl => !HasShardCrest(pl),
                    pl =>
                    {
                        pl.SendMessage("Return when you possess a ShardCrest, and I shall reward you.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                shardKeeperModule.AddOption("I have traded recently; I'll come back later.",
                    pl => !CanTradeWithPlayer(pl),
                    pl =>
                    {
                        pl.SendMessage("The exchange of shards is a delicate process. You must wait a while longer before we can trade again.");
                        pl.SendGump(new DialogueGump(pl, CreateGreetingModule(pl)));
                    });
                p.SendGump(new DialogueGump(p, shardKeeperModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye.",
            p => true,
            p =>
            {
                p.SendMessage("Cecilia nods solemnly, her eyes glinting with ancient knowledge.");
            });

        return greeting;
    }

    private bool HasShardCrest(PlayerMobile player)
    {
        // Check the player's inventory for ShardCrest
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(ShardCrest)) != null;
    }

    private bool CanTradeWithPlayer(PlayerMobile player)
    {
        // Check if the player can trade, based on the 10-minute cooldown
        if (LastTradeTime.TryGetValue(player, out DateTime lastTrade))
        {
            return (DateTime.UtcNow - lastTrade).TotalMinutes >= 10;
        }
        return true;
    }

    private void CompleteTrade(PlayerMobile player)
    {
        // Remove the ShardCrest and give the WorldShard and MaxxiaScroll, then set the cooldown timer
        Item shardCrest = player.Backpack.FindItemByType(typeof(ShardCrest));
        if (shardCrest != null)
        {
            shardCrest.Delete();
            player.AddToBackpack(new WorldShard());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the ShardCrest and receive a WorldShard and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a ShardCrest.");
        }
        player.SendGump(new DialogueGump(player, CreateGreetingModule(player)));
    }

    public override void Serialize(GenericWriter writer)
    {
        base.Serialize(writer);
        writer.Write(0); // version
    }

    public override void Deserialize(GenericReader reader)
    {
        base.Deserialize(reader);
        int version = reader.ReadInt();
    }
}