using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gralk the Stonehide")]
    public class GralkTheStonehide : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GralkTheStonehide() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gralk the Stonehide";
            Body = 0x190; // Human male body

            // Stats
            Str = 160;
            Dex = 70;
            Int = 40;
            Hits = 120;

            // Appearance
            AddItem(new BoneHelm() { Hue = 1167 });
            AddItem(new RingmailLegs() { Hue = 1166 });
            AddItem(new HammerPick() { Name = "Gralk's Maul" });

            // Set up NPC appearance
            Hue = 1165;
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Gralk the Stonehide, born of the earth itself! What wisdom do you seek, traveler?");

            greeting.AddOption("Tell me about the Stone Kingdom.",
                player => true,
                player =>
                {
                    DialogueModule stoneKingdomModule = new DialogueModule("Ah, the Stone Kingdom! A vast realm beneath our feet where living rocks thrive and ancient secrets are preserved. Would you like to hear about its history or its inhabitants?");
                    
                    stoneKingdomModule.AddOption("Tell me about its history.",
                        p => true,
                        p =>
                        {
                            DialogueModule historyModule = new DialogueModule("The Stone Kingdom was forged eons ago when the first rocks gained sentience, guided by the Earth Spirit. Over time, they built vast cities of shimmering crystals and impenetrable stone. These cities resonate with the heartbeat of the earth. Do you wish to know about the Earth Spirit?");
                            
                            historyModule.AddOption("Who is the Earth Spirit?",
                                pq => true,
                                pq =>
                                {
                                    DialogueModule earthSpiritModule = new DialogueModule("The Earth Spirit is a primordial force that nurtured the living rocks, granting them life and wisdom. It is said that the Spirit speaks through the rumblings of the earth, guiding us in our quest to protect the land.");
                                    earthSpiritModule.AddOption("How do the living rocks interact with the Earth Spirit?",
                                        pw => true,
                                        pw =>
                                        {
                                            DialogueModule interactionModule = new DialogueModule("Living rocks commune with the Earth Spirit through deep meditation, absorbing its ancient wisdom. They share tales of the surface world and harmonize their energies to maintain the balance of nature.");
                                            p.SendGump(new DialogueGump(p, interactionModule));
                                        });
                                    p.SendGump(new DialogueGump(p, earthSpiritModule));
                                });
                            historyModule.AddOption("What are the cities like?",
                                pe => true,
                                pe =>
                                {
                                    DialogueModule citiesModule = new DialogueModule("The cities are magnificent, carved from the very heart of the earth. Crystal spires reach towards the surface, while expansive caverns echo with the songs of the stones. Each city has its own guardian, a powerful entity that ensures its safety.");
                                    p.SendGump(new DialogueGump(p, citiesModule));
                                });
                            historyModule.AddOption("What else can you tell me?",
                                pr => true,
                                pr =>
                                {
                                    DialogueModule moreHistoryModule = new DialogueModule("The Stone Kingdom is filled with ancient lore. Many rocks are imbued with magical properties, granting them unique abilities. Tales of brave adventurers who ventured into the depths for treasure or knowledge are common.");
                                    p.SendGump(new DialogueGump(p, moreHistoryModule));
                                });
                            p.SendGump(new DialogueGump(p, historyModule));
                        });

                    stoneKingdomModule.AddOption("What about its inhabitants?",
                        p => true,
                        p =>
                        {
                            DialogueModule inhabitantsModule = new DialogueModule("The inhabitants of the Stone Kingdom include the noble Golemkin, wise Crystal Sages, and the mischievous Pebblefolk. Each has its own role in maintaining the harmony of our realm. Would you like to know about a specific type?");
                            
                            inhabitantsModule.AddOption("Tell me about the Golemkin.",
                                pt => true,
                                pt =>
                                {
                                    DialogueModule golemkinModule = new DialogueModule("The Golemkin are protectors of the kingdom, created from the strongest rocks. They are fierce warriors, tasked with defending our land against threats. Their hearts are infused with the essence of the earth.");
                                    golemkinModule.AddOption("What is their purpose?",
                                        pl => true,
                                        pl =>
                                        {
                                            DialogueModule purposeModule = new DialogueModule("They guard the gates of our cities and ensure that no harm befalls the living rocks. They have the ability to channel the earth’s energy, making them formidable foes in battle.");
                                            pl.SendGump(new DialogueGump(pl, purposeModule));
                                        });
                                    golemkinModule.AddOption("Are they friendly?",
                                        pl => true,
                                        pl =>
                                        {
                                            DialogueModule friendlinessModule = new DialogueModule("While they can be intimidating, the Golemkin are loyal to those who respect the Stone Kingdom. Prove yourself worthy, and they may even aid you in your quests.");
                                            pl.SendGump(new DialogueGump(pl, friendlinessModule));
                                        });
                                    p.SendGump(new DialogueGump(p, golemkinModule));
                                });

                            inhabitantsModule.AddOption("Tell me about the Crystal Sages.",
                                py => true,
                                py =>
                                {
                                    DialogueModule crystalSagesModule = new DialogueModule("The Crystal Sages are ancient beings of immense knowledge. They are the keepers of our history and serve as advisors to the rulers of the Stone Kingdom. Their wisdom is unparalleled.");
                                    crystalSagesModule.AddOption("What kind of knowledge do they possess?",
                                        pl => true,
                                        pl =>
                                        {
                                            DialogueModule sageKnowledgeModule = new DialogueModule("The Sages know the secrets of alchemy, earth magic, and the ancient prophecies that guide our realm. Many seek their counsel in times of need.");
                                            pl.SendGump(new DialogueGump(pl, sageKnowledgeModule));
                                        });
                                    crystalSagesModule.AddOption("Can I meet one?",
                                        pl => true,
                                        pl =>
                                        {
                                            DialogueModule meetSageModule = new DialogueModule("It is a rare honor to meet a Crystal Sage. They are elusive and often reside in the heart of the kingdom, surrounded by ancient crystals that resonate with their energy.");
                                            pl.SendGump(new DialogueGump(pl, meetSageModule));
                                        });
                                    p.SendGump(new DialogueGump(p, crystalSagesModule));
                                });

                            inhabitantsModule.AddOption("What about the Pebblefolk?",
                                pu => true,
                                pu =>
                                {
                                    DialogueModule pebblefolkModule = new DialogueModule("The Pebblefolk are small and whimsical creatures. They love to play tricks but are fiercely loyal to their friends. They can often be found in the grassy areas near the entrances to the Stone Kingdom.");
                                    pebblefolkModule.AddOption("What kind of tricks do they play?",
                                        pl => true,
                                        pl =>
                                        {
                                            DialogueModule tricksModule = new DialogueModule("Their tricks range from harmless pranks to clever puzzles. They love to challenge visitors and often hide treasures as part of their games.");
                                            pl.SendGump(new DialogueGump(pl, tricksModule));
                                        });
                                    pebblefolkModule.AddOption("Are they dangerous?",
                                        pl => true,
                                        pl =>
                                        {
                                            DialogueModule dangerModule = new DialogueModule("Not at all! The Pebblefolk are playful and enjoy fun more than mischief. However, they may lead you astray if you're not careful.");
                                            pl.SendGump(new DialogueGump(pl, dangerModule));
                                        });
                                    p.SendGump(new DialogueGump(p, pebblefolkModule));
                                });

                            inhabitantsModule.AddOption("Tell me more about the kingdom.",
                                pi => true,
                                pi =>
                                {
                                    DialogueModule moreInhabitantsModule = new DialogueModule("There are many more creatures in the Stone Kingdom. Some are allies, while others can be hostile, depending on their past interactions with surface dwellers.");
                                    p.SendGump(new DialogueGump(p, moreInhabitantsModule));
                                });

                            p.SendGump(new DialogueGump(p, inhabitantsModule));
                        });

                    player.SendGump(new DialogueGump(player, stoneKingdomModule));
                });

            greeting.AddOption("What is your role in the Stone Kingdom?",
                player => true,
                player =>
                {
                    DialogueModule roleModule = new DialogueModule("I am a guardian and a storyteller, preserving the lore of our people. I ensure that the knowledge of the past flows into the minds of the young and wise.");
                    roleModule.AddOption("What stories do you tell?",
                        p => true,
                        p =>
                        {
                            DialogueModule storiesModule = new DialogueModule("I recount tales of bravery, of the rise and fall of our great cities, and the eternal battle against those who wish to exploit our land. Would you like to hear one of these tales?");
                            storiesModule.AddOption("Yes, tell me a tale.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule taleModule = new DialogueModule("Once, there was a brave Golemkin named Kharak. He fought valiantly against an invading force that sought to steal the Heartstone, the source of our kingdom's power. With his strength and wisdom, he protected our home and became a legend among the stones.");
                                    taleModule.AddOption("What happened to Kharak?",
                                        pz => true,
                                        pz =>
                                        {
                                            DialogueModule khrakFateModule = new DialogueModule("Kharak disappeared after the battle, believed to have ventured deep into the earth to seek the Earth Spirit's guidance. His legacy lives on, inspiring generations of guardians.");
                                            p.SendGump(new DialogueGump(p, khrakFateModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, taleModule));
                                });
                            storiesModule.AddOption("I want to hear another tale.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule anotherTaleModule = new DialogueModule("Very well! There was once a Crystal Sage named Elthar. He discovered an ancient artifact that could amplify the Earth Spirit's power but fell into the hands of a dark sorcerer. The tale of his courage and cunning is one I hold dear.");
                                    anotherTaleModule.AddOption("How did Elthar defeat the sorcerer?",
                                        px => true,
                                        px =>
                                        {
                                            DialogueModule eltharVictoryModule = new DialogueModule("With the help of the Golemkin and the Pebblefolk, Elthar devised a clever plan to reclaim the artifact. They tricked the sorcerer into a trap using illusions crafted from light and stone.");
                                            p.SendGump(new DialogueGump(p, eltharVictoryModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, anotherTaleModule));
                                });
                            p.SendGump(new DialogueGump(p, storiesModule));
                        });

                    roleModule.AddOption("How can I help the Stone Kingdom?",
                        p => true,
                        p =>
                        {
                            DialogueModule helpModule = new DialogueModule("If you seek to aid us, you could gather lost artifacts, assist in repairs of our sacred sites, or simply spread the wisdom of the stones to those who dwell above.");
                            helpModule.AddOption("What artifacts do you need?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule artifactsModule = new DialogueModule("There are several relics lost to time. The Crystal Heart of Eldora and the Echo Stone of Resilience are among the most sought. Their recovery would bring great honor.");
                                    pl.SendGump(new DialogueGump(pl, artifactsModule));
                                });
                            helpModule.AddOption("What about the sacred sites?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule sitesModule = new DialogueModule("Our sacred sites, where the energies of the earth converge, are in need of restoration. You could help by gathering rare minerals and assisting our builders.");
                                    pl.SendGump(new DialogueGump(pl, sitesModule));
                                });
                            helpModule.AddOption("I will spread the wisdom of the stones.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule wisdomModule = new DialogueModule("Your journey will be filled with opportunities to share our tales and knowledge. Carry our stories to the surface, and let them echo in the hearts of those who listen.");
                                    pl.SendGump(new DialogueGump(pl, wisdomModule));
                                });
                            p.SendGump(new DialogueGump(p, helpModule));
                        });

                    player.SendGump(new DialogueGump(player, roleModule));
                });

            greeting.AddOption("Do you offer a boon?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                        player.SendGump(new DialogueGump(player, noRewardModule));
                    }
                    else
                    {
                        DialogueModule boonModule = new DialogueModule("Very well, you have shown a deep interest in the wisdom of the stones. As a token of my appreciation, accept this gift from the earth itself.");
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        player.SendGump(new DialogueGump(player, boonModule));
                    }
                });

            greeting.AddOption("What do you know about the cycles of the earth?",
                player => true,
                player =>
                {
                    DialogueModule cyclesModule = new DialogueModule("The cycles of the earth are eternal, teaching us about change, resilience, and growth. Each season brings forth new lessons, shaping the lives of the living rocks.");
                    cyclesModule.AddOption("How do these cycles affect the Stone Kingdom?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule effectModule = new DialogueModule("The cycles govern the flow of energy and resources. During the season of renewal, we find abundance, while the season of dormancy teaches patience and reflection.");
                            pl.SendGump(new DialogueGump(pl, effectModule));
                        });
                    cyclesModule.AddOption("What can I learn from them?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule learnModule = new DialogueModule("Patience, understanding, and adaptability. The stones remind us that we too must grow and change with the seasons of our lives.");
                            pl.SendGump(new DialogueGump(pl, learnModule));
                        });
                    player.SendGump(new DialogueGump(player, cyclesModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }

        public GralkTheStonehide(Serial serial) : base(serial) { }
    }
}
