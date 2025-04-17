using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Custom
{
    public class ZanaTheDimensionalCartographer : BaseCreature
    {
        [Constructable]
        public ZanaTheDimensionalCartographer()
            : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zana the Dimensional Cartographer";
            Body = 401; // Human female body
            Hue = Utility.RandomSkinHue();

            // Basic stats
            SetStr(75);
            SetDex(75);
            SetInt(150);

            SetHits(100);
            SetMana(200);
            SetStam(75);

            Fame = 0;
            Karma = 0;

            VirtualArmor = 10;

            // Keep her from wandering away
            Frozen = true;
            CantWalk = true;

            // Dress or equip Zana as you see fit
            AddItem(new LongPants(0x5E2));
            AddItem(new FancyShirt(0x3E4));
            AddItem(new ThighBoots(0x1BB));
        }

        public ZanaTheDimensionalCartographer(Serial serial)
            : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            // Show the main dialogue gump
            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // First module text
            DialogueModule greeting = new DialogueModule(
                "Greetings, traveler. I am Zana, once an exile from Wraeclast, now determined to save all worlds from the threats that spill through interdimensional rifts. " +
                "These lands are besieged by eldritch powers, and I fear even Britannia is not safe. How can I assist you?"
            );

            // Option 1: Ask about her past / exile
            greeting.AddOption(
                "Tell me more about your exile.",
                player => true, 
                player =>
                {
                    DialogueModule exileModule = new DialogueModule(
                        "I was cast out of my homeland during the crisis brought on by The Elder’s corruption. " +
                        "I devoted my life to mapping alternate realms to contain and combat these horrors. But fate brought me here, " +
                        "where Minax’s invasions tear rifts between dimensions. I refuse to sit idle while another realm falls."
                    );
                    exileModule.AddOption("That sounds dire. How can we stop it?",
                        p => true,
                        p =>
                        {
                            DialogueModule direModule = new DialogueModule(
                                "Dimensional threats must be met head-on. I'm researching anomalies, but it’s slow going. " +
                                "If you'd brave these portals, I'd gladly supply you with maps to monster-infested locales. " +
                                "Battling these threats and collecting artifacts might turn the tide."
                            );
                            direModule.AddOption("Tell me more about these maps.",
                                pl => true,
                                pl => 
                                {
                                    DialogueModule mapModule = new DialogueModule(
                                        "Each Magic Map is attuned to a specific region—Felucca, Trammel, Ilshenar, Tokuno, even Ter Mur. " +
                                        "They’ll transport you to places overrun with monsters, but also laden with treasure. If you survive, " +
                                        "you’ll be stronger for the battles ahead."
                                    );
                                    mapModule.AddOption("I'm interested. Show me the maps.",
                                        pla => true,
                                        pla =>
                                        {
                                            // Show the vendor gump for maps
                                            pla.CloseGump(typeof(ZanaMapVendorGump));
                                            pla.SendGump(new ZanaMapVendorGump(pla));
                                        }
                                    );
                                    mapModule.AddOption("I need more time to prepare.",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                        }
                                    );
                                    pl.SendGump(new DialogueGump(pl, mapModule));
                                }
                            );
                            direModule.AddOption("I’ll fight in my own way. Farewell.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                }
                            );
                            p.SendGump(new DialogueGump(p, direModule));
                        }
                    );

                    exileModule.AddOption("I see. Let’s talk about something else.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        }
                    );

                    player.SendGump(new DialogueGump(player, exileModule));
                }
            );

            // Option 2: Buy maps directly
            greeting.AddOption(
                "Show me your Magic Maps.",
                player => true,
                player =>
                {
                    // Open the custom map vendor gump
                    player.CloseGump(typeof(ZanaMapVendorGump));
                    player.SendGump(new ZanaMapVendorGump(player));
                }
            );

            // Option 3: Goodbye
            greeting.AddOption(
                "Farewell, Zana. I have other matters to attend to.",
                player => true,
                player =>
                {
                    player.SendMessage("You take your leave from Zana.");
                }
            );

            return greeting;
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
}
