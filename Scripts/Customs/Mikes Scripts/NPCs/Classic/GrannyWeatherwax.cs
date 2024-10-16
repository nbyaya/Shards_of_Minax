using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Granny Weatherwax")]
    public class GrannyWeatherwax : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GrannyWeatherwax() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Granny Weatherwax";
            Body = 0x191; // Human female body

            // Stats
            SetStr(75);
            SetDex(60);
            SetInt(120);
            SetHits(70);

            // Appearance
            AddItem(new Robe() { Hue = 1107 });
            AddItem(new Boots() { Hue = 1107 });
            AddItem(new WizardsHat() { Hue = 1107 });
            AddItem(new Spellbook() { Name = "Weatherwax's Manual" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

		public GrannyWeatherwax(Serial serial) : base(serial)
		{
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Granny Weatherwax, a keeper of ancient wisdom. How may I assist you?");
            
            greeting.AddOption("What can you tell me about geomancy?",
                player => true,
                player => {
                    DialogueModule geomancyModule = new DialogueModule("Ah, geomancy! The art of divining the earth's energies. It’s a powerful practice that connects us to the land. Would you like to learn about its history, techniques, or its relationship with nature?");
                    geomancyModule.AddOption("Tell me about its history.",
                        p => true,
                        p => {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Geomancy dates back to ancient civilizations, where it was used as a method of divination. Practitioners would interpret the patterns made by casting sand or soil, seeking guidance from the earth. Its roots can be found in various cultures across the globe.")));
                        });
                    geomancyModule.AddOption("What techniques are involved?",
                        p => true,
                        p => {
                            DialogueModule techniquesModule = new DialogueModule("The primary technique involves casting geomantic figures, which are formed by randomly marking the ground. Each figure represents different elements and energies. These figures are then interpreted to provide insights about the future or the state of one’s environment.");
                            techniquesModule.AddOption("What figures do you use?",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("There are several geomantic figures, each with its own meaning. For example, the figure of 'Fortuna Major' symbolizes great fortune, while 'Fortuna Minor' suggests minor luck. The key is to understand their interconnectedness and the energies they represent.")));
                                });
                            techniquesModule.AddOption("Can I learn to practice geomancy?",
                                pl => true,
                                pl => {
                                    DialogueModule learnModule = new DialogueModule("Of course! To begin, one must develop a strong connection to the earth. You can start by spending time in nature, meditating, and learning to listen to the whispers of the wind and the rustle of leaves. Practice drawing the figures in your mind before moving to the ground.");
                                    learnModule.AddOption("What tools do I need?",
                                        plq => true,
                                        plq => {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("You don't need much. A simple stick for marking the ground and your intuition are enough to start. As you advance, you might find it beneficial to use crystals or other natural objects to enhance your focus.")));
                                        });
                                    learnModule.AddOption("I'm not sure I believe in geomancy.",
                                        plw => true,
                                        plw => {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Skepticism is natural, but keep an open mind. The earth has its secrets, and understanding them can bring profound insights. Sometimes, belief comes from personal experience.")));
                                        });
                                    pl.SendGump(new DialogueGump(pl, learnModule));
                                });
                            p.SendGump(new DialogueGump(p, techniquesModule));
                        });
                    geomancyModule.AddOption("How does it relate to nature?",
                        p => true,
                        p => {
                            DialogueModule natureModule = new DialogueModule("Geomancy is deeply connected to the natural world. Each element of nature, from rocks to rivers, carries its own energy and meaning. By understanding these energies, one can harmonize with nature and harness its power for healing and protection.");
                            natureModule.AddOption("Can geomancy aid in healing?",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! Geomancy can be used for healing by aligning one’s energies with the earth's vibrations. Many practitioners believe that specific geomantic figures can enhance healing spells and rituals.")));
                                });
                            natureModule.AddOption("What dangers should I be aware of?",
                                pl => true,
                                pl => {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("As with all forms of magic, geomancy requires respect. Disturbing natural energies can lead to unforeseen consequences. Always approach with caution and humility, and listen to the earth’s warnings.")));
                                });
                            p.SendGump(new DialogueGump(p, natureModule));
                        });
                    player.SendGump(new DialogueGump(player, geomancyModule));
                });

            greeting.AddOption("What can you tell me about numerology?",
                player => true,
                player => {
                    DialogueModule numerologyModule = new DialogueModule("Numerology is the study of numbers and their mystical significance. Each number vibrates with its own unique energy, and by understanding these vibrations, we can gain insights into our lives and destinies. Would you like to explore its principles or learn about your own numbers?");
                    numerologyModule.AddOption("What are the principles of numerology?",
                        pl => true,
                        pl => {
                            DialogueModule principlesModule = new DialogueModule("At its core, numerology is based on the idea that everything in the universe can be reduced to numbers. The most important numbers are your Life Path Number, Destiny Number, and Soul Urge Number. Each reveals different aspects of your personality and life purpose.");
                            principlesModule.AddOption("How do I calculate my Life Path Number?",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("To calculate your Life Path Number, you sum the digits of your birth date until you arrive at a single digit. For example, if you were born on July 12, 1985, you would add 7 + 1 + 2 + 1 + 9 + 8 + 5 = 33, then 3 + 3 = 6. Thus, your Life Path Number would be 6.")));
                                });
                            principlesModule.AddOption("What does my Destiny Number tell me?",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Your Destiny Number is calculated from the letters in your full name, each corresponding to a number. This number reveals your life purpose and what you are meant to achieve. It often guides your choices and experiences.")));
                                });
                            principlesModule.AddOption("Can numerology help me make decisions?",
                                p => true,
                                p => {
                                    DialogueModule decisionsModule = new DialogueModule("Yes! By understanding your numbers, you can align your decisions with your life's purpose. Numerology can provide insights into timing for important actions and help you make more informed choices.");
                                    decisionsModule.AddOption("What about compatibility with others?",
                                        ply => true,
                                        ply => {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Numerology can also be used to assess compatibility between individuals. By comparing Life Path and Destiny Numbers, one can gain insights into potential harmonies and conflicts in relationships.")));
                                        });
                                    decisionsModule.AddOption("I want to learn more about my own numbers.",
                                        plu => true,
                                        plu => {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("If you wish, I can help you analyze your numbers. All I need is your full name and birth date, and we can discover the secrets they hold for you.")));
                                        });
                                    p.SendGump(new DialogueGump(p, decisionsModule));
                                });
                            player.SendGump(new DialogueGump(player, principlesModule));
                        });
                    numerologyModule.AddOption("Can you share a famous example of numerology?",
                        p => true,
                        p => {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Many famous figures have used numerology, including Pythagoras, who is often credited with its origins. His teachings emphasized the significance of numbers and their relationship to the universe, a foundation that has influenced countless practices of numerology today.")));
                        });
                    player.SendGump(new DialogueGump(player, numerologyModule));
                });

            greeting.AddOption("Tell me more about the Eldertree.",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("The Eldertree is a powerful symbol of wisdom and endurance. It stands tall, its roots deeply embedded in the earth, drawing energy from the ancient soil. Many believe that it holds the knowledge of all who have come before.")));
                });

            greeting.AddOption("What is the forbidden spell of Netherbinding?",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Ah, the Netherbinding spell! It is a dangerous invocation that binds one’s spirit to the netherworld. While it grants immense power, the cost is steep. Many who attempt it lose their way and never return to the realm of the living.")));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("May the winds carry you safely, traveler. Return to me when you seek knowledge anew.")));
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
    }
}
