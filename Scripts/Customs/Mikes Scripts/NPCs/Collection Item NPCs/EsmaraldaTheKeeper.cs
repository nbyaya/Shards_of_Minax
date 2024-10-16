using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;
using System.Collections.Generic;

public class EsmaraldaTheKeeper : BaseCreature
{
    private static Dictionary<PlayerMobile, DateTime> LastTradeTime = new Dictionary<PlayerMobile, DateTime>();

    [Constructable]
    public EsmaraldaTheKeeper() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
    {
        Name = "Esmaralda the Keeper of the Lost";
        Body = 0x191; // Human female body
        Hue = Utility.RandomSkinHue();

        SetStr(60);
        SetDex(50);
        SetInt(120);

        SetHits(100);
        SetMana(200);
        SetStam(50);

        Fame = 0;
        Karma = 0;

        // Outfit
        AddItem(new FancyDress(1157)); // A unique purple-hued elegant dress
        AddItem(new Boots(1109)); // Black boots
        AddItem(new Beads()); // Mysterious bead necklace
        AddItem(new Cloak(1265)); // Dark green cloak
        AddItem(new QuarterStaff()); // A mystical staff

        VirtualArmor = 15;
    }

    public EsmaraldaTheKeeper(Serial serial) : base(serial)
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
        DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Esmaralda, Keeper of the Lost. I gather the remnants of forgotten realms and tales. Are you here seeking something beyond the mundane?");

        // Introduce her backstory and knowledge
        greeting.AddOption("Tell me about the forgotten realms you speak of.",
            p => true,
            p =>
            {
                DialogueModule realmsModule = new DialogueModule("The forgotten realms are those that have slipped between the cracks of reality and memory. I collect fragments, artifacts, and knowledge that others have lost or abandoned. These shards tell stories of power, wisdom, and sometimes, despair.");

                realmsModule.AddOption("What kind of artifacts do you seek?",
                    pl => true,
                    pl =>
                    {
                        DialogueModule artifactsModule = new DialogueModule("I am always on the lookout for powerful relics. There is one item in particular that might interest you: the NexusShard. It is a fragment of a shattered world, brimming with forgotten power. Do you, by chance, have one?");

                        artifactsModule.AddOption("Yes, I have a NexusShard.",
                            plaa => HasNexusShard(plaa) && CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                CompleteTrade(plaa);
                            });

                        artifactsModule.AddOption("No, I do not have one.",
                            plaa => !HasNexusShard(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("Come back when you have a NexusShard. I am always interested in them.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });

                        artifactsModule.AddOption("I have traded recently; I'll come back later.",
                            plaa => !CanTradeWithPlayer(plaa),
                            plaa =>
                            {
                                plaa.SendMessage("You can only trade once every 10 minutes. Please return later.");
                                plaa.SendGump(new DialogueGump(plaa, CreateGreetingModule(plaa)));
                            });

                        artifactsModule.AddOption("Why do you want the NexusShard?",
                            plaa => true,
                            plaa =>
                            {
                                DialogueModule shardReasonModule = new DialogueModule("The NexusShard holds secrets of a world long gone. It is a powerful object, yes, but it is also dangerous. I wish to ensure it doesn't fall into the wrong hands. After all, power corrupts, and the weak-minded are often tempted by it.");

                                shardReasonModule.AddOption("You sound like you know about corruption quite well...",
                                    plaab => true,
                                    plaab =>
                                    {
                                        DialogueModule corruptionModule = new DialogueModule("Ah, a perceptive one, aren't you? I do know much about the nature of power, and how easily it can twist those who wield it. Once, I was not unlike you—a seeker of knowledge, driven by a desire to bring change. I was a politician, charming and persuasive. But those days taught me harsh lessons: to survive, one must embrace power, but also keep it hidden beneath layers of charm.");

                                        corruptionModule.AddOption("You used to be a politician? Tell me more.",
                                            plaabc => true,
                                            plaabc =>
                                            {
                                                DialogueModule politicianModule = new DialogueModule("Indeed, I was once a prominent figure, rallying people to my cause. I spoke of prosperity, of survival in trying times, and I gathered many followers who believed in my vision. But the truth is, survival requires sacrifices, and those sacrifices aren't always noble. The weak must be guided—or removed. A leader must not only inspire but also manipulate."
                                                + "");

                                                politicianModule.AddOption("Manipulate? That sounds dangerous.",
                                                    plaabcd => true,
                                                    plaabcd =>
                                                    {
                                                        DialogueModule manipulateModule = new DialogueModule("Dangerous? Perhaps. But isn't survival the most dangerous game of all? One cannot simply survive on the kindness of others. No, one must sometimes be ruthless, make the hard choices others cannot. My charm is but a tool, used to ensure those I guide continue to follow and thrive."
                                                        + "");

                                                        manipulateModule.AddOption("What is your true agenda, Esmaralda?",
                                                            plaabcde => true,
                                                            plaabcde =>
                                                            {
                                                                DialogueModule agendaModule = new DialogueModule("Ah, straight to the heart of it, aren't you? My agenda, dear traveler, is simple: I wish to create a society that understands true strength, a world where only those with the will to endure can survive. I call it survivalism—a new way of life, where no one is burdened by the weak. But this isn't something I share with just anyone, only those who show true potential."
                                                                + "");

                                                                agendaModule.AddOption("And if someone doesn't have potential?",
                                                                    plaabcdef => true,
                                                                    plaabcdef =>
                                                                    {
                                                                        DialogueModule noPotentialModule = new DialogueModule("Then they have no place in the world I envision. Not everyone is destined to thrive. My followers understand this truth, and they are stronger for it. I offer opportunities, but I do not carry the weight of those who cannot carry themselves."
                                                                        + "");

                                                                        noPotentialModule.AddOption("That sounds... harsh.",
                                                                            plaabcdefg => true,
                                                                            plaabcdefg =>
                                                                            {
                                                                                DialogueModule harshModule = new DialogueModule("Harshness is a matter of perspective. I see it as mercy. In times of crisis, those who hesitate are often the first to fall. I am offering a chance for strength, for survival, in a world that demands nothing less. It may seem cruel, but it is ultimately kinder than letting the weak suffer in a world not meant for them."
                                                                                + "");

                                                                                harshModule.AddOption("I think I understand. Thank you for your honesty.",
                                                                                    plaabcdefgh => true,
                                                                                    plaabcdefgh =>
                                                                                    {
                                                                                        plaabcdefgh.SendGump(new DialogueGump(plaabcdefgh, CreateGreetingModule(plaabcdefgh)));
                                                                                    });

                                                                                plaabcdefg.SendGump(new DialogueGump(plaabcdefg, harshModule));
                                                                            });

                                                                        plaabcdef.SendGump(new DialogueGump(plaabcdef, noPotentialModule));
                                                                    });

                                                                plaabcde.SendGump(new DialogueGump(plaabcde, agendaModule));
                                                            });

                                                        plaabcd.SendGump(new DialogueGump(plaabcd, manipulateModule));
                                                    });

                                                politicianModule.AddOption("You must have made enemies, then?",
                                                    plaabcd => true,
                                                    plaabcd =>
                                                    {
                                                        DialogueModule enemiesModule = new DialogueModule("Oh, many enemies. But enemies are simply those who do not understand your vision, or those too weak to see beyond their limited perspective. They opposed me because they feared change. They feared what would happen when they no longer held control. But in the end, they became irrelevant."
                                                        + "");

                                                        enemiesModule.AddOption("And your followers, what did they think?",
                                                            plaabcde => true,
                                                            plaabcde =>
                                                            {
                                                                DialogueModule followersModule = new DialogueModule("My followers saw me for what I was: a beacon of hope and strength in a dark world. They saw the necessity in my words and understood that my charm was more than superficial. It was a call to action, to be more, to endure. Many doubted at first, but when the dust settled, they saw the truth."
                                                                + "");

                                                                followersModule.AddOption("Do you still have followers now?",
                                                                    plaabcdef => true,
                                                                    plaabcdef =>
                                                                    {
                                                                        DialogueModule currentFollowersModule = new DialogueModule("Yes, those who understand my vision are still with me, though we are more secretive now. This world is not yet ready for the change I envision, but in time, it will be. My followers await the right moment, the time when our strength can guide the world out of darkness. Until then, we gather knowledge, artifacts, and power."
                                                                        + "");

                                                                        currentFollowersModule.AddOption("I see. Thank you for sharing, Esmaralda.",
                                                                            plaabcdefgh => true,
                                                                            plaabcdefgh =>
                                                                            {
                                                                                plaabcdefgh.SendGump(new DialogueGump(plaabcdefgh, CreateGreetingModule(plaabcdefgh)));
                                                                            });

                                                                        plaabcdef.SendGump(new DialogueGump(plaabcdef, currentFollowersModule));
                                                                    });

                                                                plaabcde.SendGump(new DialogueGump(plaabcde, followersModule));
                                                            });

                                                        plaabcd.SendGump(new DialogueGump(plaabcd, enemiesModule));
                                                    });

                                                plaabc.SendGump(new DialogueGump(plaabc, politicianModule));
                                            });

                                        plaab.SendGump(new DialogueGump(plaab, corruptionModule));
                                    });

                                plaa.SendGump(new DialogueGump(plaa, shardReasonModule));
                            });

                        pl.SendGump(new DialogueGump(pl, artifactsModule));
                    });

                realmsModule.AddOption("That sounds fascinating, thank you.",
                    pla => true,
                    pla =>
                    {
                        pla.SendGump(new DialogueGump(pla, CreateGreetingModule(pla)));
                    });

                p.SendGump(new DialogueGump(p, realmsModule));
            });

        // Goodbye Option
        greeting.AddOption("Goodbye, Esmaralda.",
            p => true,
            p =>
            {
                p.SendMessage("Esmaralda nods, her eyes seeming to see far beyond the present moment.");
            });

        return greeting;
    }

    private bool HasNexusShard(PlayerMobile player)
    {
        // Check the player's inventory for NexusShard
        return player.Backpack != null && player.Backpack.FindItemByType(typeof(NexusShard)) != null;
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
        // Remove the NexusShard and give the HorrodrickCube and MaxxiaScroll, then set the cooldown timer
        Item nexusShard = player.Backpack.FindItemByType(typeof(NexusShard));
        if (nexusShard != null)
        {
            nexusShard.Delete();
            player.AddToBackpack(new HorrodrickCube());
            player.AddToBackpack(new MaxxiaScroll());
            LastTradeTime[player] = DateTime.UtcNow;
            player.SendMessage("You hand over the NexusShard and receive a HorrodrickCube and a MaxxiaScroll in return.");
        }
        else
        {
            player.SendMessage("It seems you no longer have a NexusShard.");
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