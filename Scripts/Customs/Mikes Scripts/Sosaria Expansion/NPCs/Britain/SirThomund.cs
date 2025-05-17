using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Thomund")]
    public class SirThomund : BaseCreature
    {
        [Constructable]
        public SirThomund() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Thomund";
            Body = 0x190; // Human male body

            // Stats
            SetStr(150);
            SetDex(80);
            SetInt(90);
            SetHits(120);

            // Appearance
            AddItem(new PlateChest() { Hue = 1175 });
            AddItem(new Helmet() { Hue = 1175 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new Longsword() { Name = "Defender of the Realm" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public SirThomund(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Sir Thomund, once a proud knight of the realm—courageous, loyal, and, I must confess, a hot-headed soul. My past is marred by exile, for I defied corrupt orders that sought to oppress the weak. Now, I roam anonymously, seeking redemption by protecting those who cannot defend themselves. How may I share my tale with you today?");

            // Option 1: My Past and Exile
            greeting.AddOption("Tell me about your past and exile.", 
                player => true,
                player => 
                {
                    DialogueModule pastModule = new DialogueModule("My past is a tapestry woven with honor and regret. I once served as a knight under a banner that promised justice. Yet, when orders turned cruel and corrupt, I dared to defy them. I stood up for a small village, sacrificing my rank and favor with the crown. As a result, I was exiled—a bitter fate for one who had fought for righteousness.");
                    
                    // Nested: The Moment of Defiance
                    pastModule.AddOption("What exactly happened during your defiance?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule defianceModule = new DialogueModule("In a fateful winter, news came that the crown planned to levy a cruel tax on the starving peasants. I could not stand by as innocent blood was about to be spilled. I led a small band of loyal knights to protect the village, defying orders. The battle was fierce, and though we triumphed, the cost was my honor. I was branded a traitor by those blinded by greed.");
                            
                            // Nested further: Feelings on the Betrayal
                            defianceModule.AddOption("That must have been heart-wrenching. How did it affect you?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule affectModule = new DialogueModule("It was a crucible of fire and sorrow. I felt both the weight of betrayal and the burning fire of righteous anger. My loyalty to the people burned brighter than the loyalty owed to a corrupt crown. Even now, I carry that scar—a constant reminder of the price of integrity.");
                                    pl2.SendGump(new DialogueGump(pl2, affectModule));
                                });
                            pl.SendGump(new DialogueGump(pl, defianceModule));
                        });
                    
                    // Nested: Redemption and Anonymous Protection
                    pastModule.AddOption("How do you seek redemption now?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule redemptionModule = new DialogueModule("Redemption, to me, is found in each act of kindness and valor. I wander the lands under a veil of anonymity, offering my sword and counsel to those in need. Whether it is fortifying a crumbling castle wall or shielding a humble miner from lurking shadows, every deed is a step toward atoning for my past.");
                            
                            redemptionModule.AddOption("Tell me about your work with the miners.",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule minerModule = new DialogueModule("At Devil Guard, I work alongside Crag—a grizzled miner with a wisdom born of hardship—and Yorn, a caretaker whose keen eyes never miss a detail. They share with me the secrets of the deep mines, and together, we secure the passages against dark forces that lurk below. Our bond is forged in the fires of adversity and tempered by shared purpose.");
                                    pl2.SendGump(new DialogueGump(pl2, minerModule));
                                });
                            redemptionModule.AddOption("And your alliance with Torren?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule torrenModule = new DialogueModule("Torren, a master builder from East Montor, is both a comrade and a mentor in the art of fortification. We spend endless nights poring over ancient blueprints and modern techniques to ensure our defenses can withstand any assault. His innovative spirit inspires me to rebuild not just walls, but the honor I lost.");
                                    pl2.SendGump(new DialogueGump(pl2, torrenModule));
                                });
                            pl.SendGump(new DialogueGump(pl, redemptionModule));
                        });
                    
                    pastModule.AddOption("Do you ever regret defying those orders?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule regretModule = new DialogueModule("Regret, like a shadow, follows every man. I have moments of sorrow when the weight of my choices bears down on me. Yet, my courageous heart tells me that sometimes, defiance is the only path to true justice. My loyalty to the people—and to the ideals I hold dear—remains unshaken, even as I bear the scars of my past.");
                            pl.SendGump(new DialogueGump(pl, regretModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, pastModule));
                });

            // Option 2: Your Traits and Inner Nature
            greeting.AddOption("What would you say defines you as a knight?",
                player => true,
                player => 
                {
                    DialogueModule traitsModule = new DialogueModule("I am defined by a fierce courage, unwavering loyalty, and yes—an occasionally hot-headed temper. My courage is the fire that propels me into battle, my loyalty is the bond I share with those who stand for justice, and my temper, though a flaw, fuels my passion to right wrongs.");
                    
                    // Nested: Courage
                    traitsModule.AddOption("Tell me more about your courage.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule courageModule = new DialogueModule("Courage, to me, is not the absence of fear but the resolve to face it head-on. I have stared into the abyss of despair on countless battlefields, and each time, I chose to stand firm. My courage was forged on the fields of strife and tempered by the countless lives I vowed to protect.");
                            pl.SendGump(new DialogueGump(pl, courageModule));
                        });
                    
                    // Nested: Loyalty
                    traitsModule.AddOption("And your loyalty?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule loyaltyModule = new DialogueModule("Loyalty is the compass that guides my every action. Even in exile, my loyalty to the innocent remains unbroken. I keep close ties with those like Crag, Yorn, and Torren—each a beacon of hope in a dark world. I would risk everything for a friend, and my oaths to protect the weak are sacrosanct.");
                            loyaltyModule.AddOption("Who are these friends you speak of?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule friendsModule = new DialogueModule("Crag, the steadfast miner who listens to the whispers of the deep; Yorn, whose careful observations save lives; and Torren, whose brilliant mind rebuilds what was lost. They are more than colleagues—they are the family I chose in a world that once cast me out.");
                                    pl2.SendGump(new DialogueGump(pl2, friendsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, loyaltyModule));
                        });
                    
                    // Nested: Hot-Headed Temperament
                    traitsModule.AddOption("What about your hot-headed side?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule hotHeadedModule = new DialogueModule("My temper has been both a curse and a blessing. There are times when the fury of injustice makes my blood boil, and in those moments, I act before I think. Though it has led to bitter regrets in the past, I have learned to channel that fire into protecting others. It is a volatile spark that reminds me of my humanity.");
                            
                            hotHeadedModule.AddOption("Has your temper ever caused trouble?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule troubleModule = new DialogueModule("Oh, many times. There were battles where my impulsive strikes nearly cost us dearly. But each misstep has been a lesson, forging me into a knight who now wields both passion and caution in equal measure.");
                                    pl2.SendGump(new DialogueGump(pl2, troubleModule));
                                });
                            pl.SendGump(new DialogueGump(pl, hotHeadedModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, traitsModule));
                });

            // Option 3: Your Current Duties and Collaborations
            greeting.AddOption("What are your duties now?",
                player => true,
                player =>
                {
                    DialogueModule dutyModule = new DialogueModule("My current duties are manifold. I patrol the ancient ramparts of Castle British, advising on fortifications with the keen mind of Torren from East Montor. At Devil Guard, I join forces with Crag and Yorn to secure the perilous mines. Each day presents new challenges, yet every task is a step toward the redemption I seek.");
                    
                    // Nested: Castle Duties
                    dutyModule.AddOption("Tell me about your work at the castle.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule castleModule = new DialogueModule("At the castle, I am not merely a sentinel but a steward of history. I oversee repairs, study old blueprints, and ensure that every stone tells a story of valor. My days are filled with the echoes of ancient battles and the quiet determination of those who believe in justice.");
                            
                            castleModule.AddOption("Do you ever hear the voices of the past?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule voicesModule = new DialogueModule("Sometimes, in the silence of night, the castle itself seems to whisper tales of glory and sacrifice. I listen, and I learn—each whisper a reminder of why I must keep fighting.");
                                    pl2.SendGump(new DialogueGump(pl2, voicesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, castleModule));
                        });
                    
                    // Nested: Mine Duties
                    dutyModule.AddOption("What dangers do you face in the mines?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule mineModule = new DialogueModule("The mines are fraught with darkness and hidden terrors. Alongside Crag—a miner whose weathered face speaks of countless battles—and Yorn, whose vigilance is unmatched, I confront the twisted echoes of ancient curses and restless spirits. We fortify passages, set enchanted wards, and ensure that these depths do not overwhelm the light above.");
                            
                            mineModule.AddOption("How do you prepare for these encounters?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule prepModule = new DialogueModule("Preparation is both art and science. I consult with Torren for structural reinforcements, study the old tomes preserved by Elena the Archivist, and listen intently to Crag's accounts of subterranean whispers. Our collaboration is the key to turning the tide against the darkness.");
                                    pl2.SendGump(new DialogueGump(pl2, prepModule));
                                });
                            pl.SendGump(new DialogueGump(pl, mineModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, dutyModule));
                });

            // Option 4: Advice for Aspiring Knights and Redeemers
            greeting.AddOption("What advice do you have for an aspiring knight?",
                player => true,
                player =>
                {
                    DialogueModule adviceModule = new DialogueModule("An aspiring knight must be prepared to face both external foes and internal demons. Learn from my mistakes and triumphs. Honor, courage, and loyalty are your most potent weapons—tempered by wisdom and the willingness to admit when you are wrong.");
                    
                    adviceModule.AddOption("How can I prove my valor?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule valorModule = new DialogueModule("Valor is proven in action. Do not hesitate to step forward when the weak cry out for help. Seek challenges, stand up against injustice, and know that every scar you earn is a lesson that molds you into a protector of the realm.");
                            valorModule.AddOption("What challenges should I face?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule challengeModule = new DialogueModule("Face the perils in the mines, assist the downtrodden in the villages, and learn the ancient arts of defense and repair. Every challenge, whether a skirmish with bandits or a dark secret lurking in the ruins, is a test of your spirit.");
                                    pl2.SendGump(new DialogueGump(pl2, challengeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, valorModule));
                        });
                    
                    adviceModule.AddOption("What if I stumble or falter?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule stumbleModule = new DialogueModule("Even the greatest knights have stumbled. Embrace your failures as stepping stones toward greatness. Seek counsel from those who have walked the path before you—listen to the wisdom of Crag, Yorn, and even Torren—and remember that redemption is found in rising after every fall.");
                            pl.SendGump(new DialogueGump(pl, stumbleModule));
                        });
                    
                    adviceModule.AddOption("How do I balance passion with wisdom?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule balanceModule = new DialogueModule("Passion fuels your desire to protect, but wisdom guides your hand. Temper your hot-headed impulses with careful thought. Reflect on my own journey: the fire in my heart once led me astray, but through hardship, I learned that true strength lies in controlled resolve and a compassionate soul.");
                            pl.SendGump(new DialogueGump(pl, balanceModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, adviceModule));
                });

            // Option 5: Farewell
            greeting.AddOption("Farewell.",
                player => true,
                player =>
                {
                    DialogueModule farewellModule = new DialogueModule("May honor and valor light your path, traveler. Remember, redemption is not a destination but a journey—one you walk with every brave deed. Until we meet again.");
                    player.SendGump(new DialogueGump(player, farewellModule));
                });

            return greeting;
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
}
