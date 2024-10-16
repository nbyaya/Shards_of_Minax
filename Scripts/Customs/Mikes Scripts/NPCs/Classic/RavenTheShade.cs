using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Raven the Shade")]
    public class RavenTheShade : BaseCreature
    {
        [Constructable]
        public RavenTheShade() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Raven the Shade";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 120;
            Int = 50;
            Hits = 80;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1107 });
            AddItem(new LeatherChest() { Hue = 1107 });
            AddItem(new LeatherGloves() { Hue = 1107 });
            AddItem(new HoodedShroudOfShadows() { Hue = 1107 });
            AddItem(new Boots() { Hue = 1107 });
            AddItem(new Kryss() { Name = "Raven's Kryss" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
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
            DialogueModule greeting = new DialogueModule("I am Raven the Shade, the night's silent blade. Raised among ravens, I am both shadow and secret. What do you wish to know?");

            greeting.AddOption("Tell me about your childhood.",
                player => true,
                player => 
                {
                    DialogueModule childhoodModule = new DialogueModule("I was not raised by mere mortals but by the enigmatic ravens of the Whispering Woods. They found me as a child, abandoned and alone, and took me into their fold.");
                    childhoodModule.AddOption("What was it like living with ravens?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule ravensLifeModule = new DialogueModule("Life with ravens is unlike any other. They are clever, cunning, and fiercely protective. They taught me to see the world through their eyes, to embrace the shadows and find wisdom in darkness.");
                            ravensLifeModule.AddOption("Did you learn their ways?",
                                p => true,
                                p => 
                                {
                                    DialogueModule learnedWaysModule = new DialogueModule("Indeed! I learned their calls, their dances in the air, and their secrets whispered on the wind. I became one with the shadows, understanding the balance of light and darkness.");
                                    learnedWaysModule.AddOption("What secrets did they share?",
                                        plq => true,
                                        plq => 
                                        {
                                            DialogueModule secretsModule = new DialogueModule("Ravens are the keepers of many secrets. They showed me hidden paths in the forest and taught me to listen for the truths buried beneath the lies of the world.");
                                            secretsModule.AddOption("How do you listen to the world?",
                                                pw => true,
                                                pw => 
                                                {
                                                    DialogueModule listeningModule = new DialogueModule("Listening is an art. It requires patience and an open heart. The whispers of the night carry tales of the past, and if you attune your senses, you can hear them.");
                                                    listeningModule.AddOption("Can you teach me to listen?",
                                                        ple => true,
                                                        ple => 
                                                        {
                                                            DialogueModule teachListeningModule = new DialogueModule("To listen is to observe. Find a quiet place, close your eyes, and breathe. Let the world around you fade, and you shall hear the echoes of existence.");
                                                            teachListeningModule.AddOption("I will try to listen.",
                                                                pla => true,
                                                                pla => pla.SendMessage("You resolve to practice the art of listening."));
                                                            teachListeningModule.AddOption("Maybe another time.",
                                                                pla => true,
                                                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                                            p.SendGump(new DialogueGump(p, teachListeningModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, listeningModule));
                                                });
                                            secretsModule.AddOption("What else did you learn?",
                                                plr => true,
                                                plr => 
                                                {
                                                    DialogueModule moreLearningModule = new DialogueModule("The ravens taught me to navigate through life's complexities. They showed me that survival is about cunning and strategy, much like a game of shadows.");
                                                    moreLearningModule.AddOption("What strategies did you learn?",
                                                        pt => true,
                                                        pt => 
                                                        {
                                                            DialogueModule strategiesModule = new DialogueModule("I learned to adapt, to blend in with my surroundings, and to strike when the moment is right. In the world of shadows, timing is everything.");
                                                            strategiesModule.AddOption("That sounds useful.",
                                                                ply => true,
                                                                ply => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                            p.SendGump(new DialogueGump(p, strategiesModule));
                                                        });
                                                    p.SendGump(new DialogueGump(pl, moreLearningModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, secretsModule));
                                        });
                                    p.SendGump(new DialogueGump(p, learnedWaysModule));
                                });
                            ravensLifeModule.AddOption("Did you have any friends?",
                                p => true,
                                p => 
                                {
                                    DialogueModule friendsModule = new DialogueModule("The ravens were my family, my friends. They understood my heart, and in return, I learned their language. Together, we soared through the night.");
                                    friendsModule.AddOption("What adventures did you have?",
                                        plu => true,
                                        plu => 
                                        {
                                            DialogueModule adventuresModule = new DialogueModule("We would explore hidden glades, soar above the treetops, and discover ancient ruins shrouded in mist. Each night was a new adventure waiting to unfold.");
                                            adventuresModule.AddOption("What were these ruins like?",
                                                pi => true,
                                                pi => 
                                                {
                                                    DialogueModule ruinsModule = new DialogueModule("The ruins whispered of a forgotten time. Crumbling stones bore the marks of ancient rituals, and the air was thick with the energy of past lives.");
                                                    ruinsModule.AddOption("Did you find anything valuable?",
                                                        plo => true,
                                                        plo => 
                                                        {
                                                            DialogueModule treasuresModule = new DialogueModule("I found relics of power and knowledge, remnants of an era long lost. The ravens showed me how to harness this knowledge for my own understanding.");
                                                            treasuresModule.AddOption("What kind of relics?",
                                                                pp => true,
                                                                pp => 
                                                                {
                                                                    DialogueModule relicsModule = new DialogueModule("Some were trinkets, others were tomes filled with ancient lore. Each had a story, a connection to the past that shaped my understanding of the world.");
                                                                    relicsModule.AddOption("Can you share a story from one of these relics?",
                                                                        pla => true,
                                                                        pla => 
                                                                        {
                                                                            DialogueModule storyModule = new DialogueModule("One relic spoke of a great battle between light and dark, where heroes fell and legends were born. It taught me that every story holds a lesson, and wisdom lies in the shadows of our past.");
                                                                            storyModule.AddOption("That's fascinating!",
                                                                                plas => true,
                                                                                plas => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                                                            p.SendGump(new DialogueGump(p, storyModule));
                                                                        });
                                                                    p.SendGump(new DialogueGump(pl, relicsModule));
                                                                });
                                                            p.SendGump(new DialogueGump(pl, treasuresModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, ruinsModule));
                                                });
                                            p.SendGump(new DialogueGump(pl, adventuresModule));
                                        });
                                    p.SendGump(new DialogueGump(pl, friendsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, ravensLifeModule));
                        });
                    player.SendGump(new DialogueGump(player, childhoodModule));
                });

            greeting.AddOption("What about your abilities?",
                player => true,
                player =>
                {
                    DialogueModule abilitiesModule = new DialogueModule("The ravens granted me unique abilities, a connection to the shadows. I can slip between worlds and harness the night to conceal myself.");
                    abilitiesModule.AddOption("How do you use these abilities?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule usingAbilitiesModule = new DialogueModule("In the depths of night, I blend with the shadows, becoming one with them. I can move unnoticed, gathering secrets and observing the world without being seen.");
                            usingAbilitiesModule.AddOption("What do you observe?",
                                p => true,
                                p => 
                                {
                                    DialogueModule observationModule = new DialogueModule("I watch the struggles of mortals, the dances of the celestial bodies, and the ebb and flow of life. Each moment is a lesson, a chance to learn.");
                                    observationModule.AddOption("Do you share what you learn?",
                                        pld => true,
                                        pld => 
                                        {
                                            DialogueModule shareModule = new DialogueModule("I share only what is necessary, for knowledge is a double-edged sword. Some truths are burdensome, while others enlighten the soul.");
                                            shareModule.AddOption("What truths have you shared?",
                                                pf => true,
                                                pf => 
                                                {
                                                    DialogueModule truthsModule = new DialogueModule("I have shared the importance of balance, the need for light and dark to coexist. The world thrives in duality, and understanding this is key to harmony.");
                                                    truthsModule.AddOption("That's profound.",
                                                        pla => true,
                                                        pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                                    p.SendGump(new DialogueGump(p, truthsModule));
                                                });
                                            p.SendGump(new DialogueGump(pl, shareModule));
                                        });
                                    p.SendGump(new DialogueGump(p, observationModule));
                                });
                            player.SendGump(new DialogueGump(player, usingAbilitiesModule));
                        });
                    player.SendGump(new DialogueGump(player, abilitiesModule));
                });

            greeting.AddOption("Do you have any regrets?",
                player => true,
                player =>
                {
                    DialogueModule regretsModule = new DialogueModule("Regrets are shadows that cling to the heart. I sometimes wonder what life would have been like had I not been raised by ravens.");
                    regretsModule.AddOption("What do you mean?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule meaningModule = new DialogueModule("While the ravens taught me much, there are experiences of humanity I missed. Friendships, love, and the warmth of family that I have only glimpsed from afar.");
                            meaningModule.AddOption("Is it too late to experience that?",
                                p => true,
                                p => 
                                {
                                    DialogueModule lateModule = new DialogueModule("Time is but an illusion. Every day brings a new chance to embrace the warmth of connection. It is never too late to reach out.");
                                    lateModule.AddOption("How do I start?",
                                        plg => true,
                                        plg => 
                                        {
                                            DialogueModule startModule = new DialogueModule("Begin by opening your heart. Listen, share, and be vulnerable. Only then can you truly connect with others.");
                                            startModule.AddOption("I will try to be more open.",
                                                pla => true,
                                                pla => pla.SendMessage("You resolve to be more open with others."));
                                            startModule.AddOption("Maybe I should think about it.",
                                                pla => true,
                                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                                            p.SendGump(new DialogueGump(p, startModule));
                                        });
                                    p.SendGump(new DialogueGump(p, lateModule));
                                });
                            pl.SendGump(new DialogueGump(pl, meaningModule));
                        });
                    player.SendGump(new DialogueGump(player, regretsModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendMessage("Raven nods silently as you depart.");
                });

            return greeting;
        }

        public RavenTheShade(Serial serial) : base(serial) { }

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
