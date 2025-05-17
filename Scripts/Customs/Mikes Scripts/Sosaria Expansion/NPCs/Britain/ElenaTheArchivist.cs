using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class ElenaTheArchivist : BaseCreature
    {
        [Constructable]
        public ElenaTheArchivist() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Elena";
            Title = "the Keeper of Forgotten Pages";
            Body = 0x191; // Human female body
            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Long hair
            HairHue = 0x461;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new GoldNecklace());
            AddItem(new HalfApron() { Hue = 1355 });

            // Stats
            SetStr(50);
            SetDex(60);
            SetInt(125);
            SetHits(80);
        }

        public ElenaTheArchivist(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule(
                "Ah, a curious soul! I'm Elena, chronicler of Sosaria's hidden histories. " +
                "These shelves hold more than books - they're windows to our world's secret heart. " +
                "What knowledge calls to you today?\n\n" +
                "(Her fingers trail across a massive tome titled 'Maritime Omens and Other Coastal Curiosities')"
            );

            // Main dialogue options
            greeting.AddOption("Your work with Marin in Renika...",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateMarinModule())))
            ;

            greeting.AddOption("Arielle's paintings in Fawn...",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateArielleModule()))
            );

            greeting.AddOption("Research with Iris in Devil Guard...",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateIrisModule()))
            );

            greeting.AddOption("What's your current project?",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateProjectModule()))
            );

            greeting.AddOption("Just browsing your collection...",
                player => true,
                player => 
                {
                    DialogueModule browseModule = new DialogueModule(
                        "Her eyes light up as she gestures at the shelves:\n\n" +
                        "'The Complete Bestiary of Cursed Caverns' glows faintly. " +
                        "A ship's log labeled 'The Crimson Tide's Last Voyage' smells of brine. " +
                        "The 'Compendium of Forgotten Moon Glyphs' hums with residual magic..."
                    );
                    browseModule.AddOption("Go back", p => true, p => p.SendGump(new DialogueGump(p, greeting)));
                    player.SendGump(new DialogueGump(player, browseModule));
                }
            );

            return greeting;
        }

        private DialogueModule CreateMarinModule()
        {
            DialogueModule marinModule = new DialogueModule(
                "Marin's sea-stained letters arrive weekly, each containing some new maritime mystery. " +
                "Just yesterday she sent this...\n\n" +
                "(She unfolds a map marked with strange tidal patterns)\n" +
                "'The Tidecaller's Dirge' - a ghost ship said to sail when the moon bleeds crimson. " +
                "We've tracked its appearances to locations where... well, where sailors report hearing *laughter* from the depths."
            );

            marinModule.AddOption("Why study ghost ships?",
                player => true,
                player => 
                {
                    DialogueModule whyModule = new DialogueModule(
                        "Because patterns emerge. Last year, three 'phantom ship' sightings preceded: " +
                        "1) The Greyport vanishing\n2) That fungal bloom near Pirate Isle\n3) The mass beaching of luminous jellyfish\n" +
                        "The sea remembers what land forgets. Marin believes...\n\n" +
                        "(She lowers her voice) ...they're not omens, but warnings from the deep."
                    );
                    whyModule.AddOption("Continue", p => true, p => p.SendGump(new DialogueGump(p, marinModule)));
                    player.SendGump(new DialogueGump(player, whyModule));
                }
            );

            marinModule.AddOption("Any connection to Arielle's art?",
                player => true,
                player => 
                {
                    DialogueModule connectionModule = new DialogueModule(
                        "Ah, you've noticed! Compare this...\n\n" +
                        "(She places Arielle's sketch of 'The Drowning Ballerina' next to Marin's kraken drawings)\n" +
                        "The same spiral patterns. We suspect they represent the 'Churn' - " +
                        "an ancient aquatic magic mentioned in both mining songs and pirate shanties."
                    );
                    connectionModule.AddOption("Fascinating", p => true, p => p.SendGump(new DialogueGump(p, marinModule)));
                    player.SendGump(new DialogueGump(player, connectionModule));
                }
            );

            marinModule.AddOption("How can I help?",
                player => true,
                player => 
                {
                    DialogueModule helpModule = new DialogueModule(
                        "Marin needs brave souls to collect bioluminescent algae from the Caverns of Echoing Tides. " +
                        "But beware the 'Songstress' - she who...\n\n" +
                        "(Elena's quill suddenly snaps) Let's just say, don't hum along with the cave's melodies."
                    );
                    helpModule.AddOption("I'll investigate", p => true, p => 
                    {
                        // Trigger quest here
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                    helpModule.AddOption("Too dangerous", p => true, p => p.SendGump(new DialogueGump(p, marinModule)));
                    player.SendGump(new DialogueGump(player, helpModule));
                }
            );

            return marinModule;
        }

        private DialogueModule CreateArielleModule()
        {
            DialogueModule arielleModule = new DialogueModule(
                "Arielle's latest piece - 'The Weeping Canvas' - shows a figure dissolving into brushstrokes. " +
                "Notice the eyes in the paint swirls? Identical to those described in...\n\n" +
                "(She produces a crumbling bestiary) ...the 'Pigment Horrors' section! " +
                "1883 reports from Fawn mention artists consumed by their own creations."
            );

            arielleModule.AddOption("Literal consumption?",
                player => true,
                player => 
                {
                    DialogueModule literalModule = new DialogueModule(
                        "In the physical sense? Perhaps. Old Fawn census records show: " +
                        "- 12 missing painters\n- 3 sculptors gone blind\n- 1 potter who...\n\n" +
                        "(She shows a charcoal rubbing of a tombstone)\n" +
                        "'Here lies Marla Clayborn, who became one with her final vase.'\n" +
                        "The clay still whispers on moonless nights."
                    );
                    literalModule.AddOption("Chilling", p => true, p => p.SendGump(new DialogueGump(p, arielleModule)));
                    player.SendGump(new DialogueGump(player, literalModule));
                }
            );

            arielleModule.AddOption("Any protective measures?",
                player => true,
                player => 
                {
                    DialogueModule protectionModule = new DialogueModule(
                        "Iris developed this...\n\n" +
                        "(She holds up a crystal vial) ...'Chromatic Neutralizer' made from: " +
                        "- Devil Guard's prismatic ore\n- Yew's twilight sap\n- Ground moonstone\n" +
                        "Sprinkle it on canvases showing the 'Triple Spiral' motif. " +
                        "But handle carefully - it reacts violently to lies."
                    );
                    protectionModule.AddOption("Can I have some?", p => true, p => 
                    {
                        // Give item
                        p.SendGump(new DialogueGump(p, arielleModule));
                    });
                    protectionModule.AddOption("Fascinating process", p => true, p => p.SendGump(new DialogueGump(p, arielleModule)));
                    player.SendGump(new DialogueGump(player, protectionModule));
                }
            );

            return arielleModule;
        }

        private DialogueModule CreateIrisModule()
        {
            DialogueModule irisModule = new DialogueModule(
                "Our latest breakthrough! Cross-referencing mining songs with ancient star charts, " +
                "we discovered the 'Lode Code' - a musical notation system in pickaxe strikes. " +
                "It corresponds to...\n\n" +
                "(She plays a strange melody on a mineralophone) ...the resonant frequency of Shadow Quartz!"
            );

            irisModule.AddOption("Practical applications?",
                player => true,
                player => 
                {
                    DialogueModule appsModule = new DialogueModule(
                        "1. Detecting unstable mine shafts\n2. Communicating with crystal entities\n3. Neutralizing...\n\n" +
                        "(The ground trembles slightly) ...certain 'geological tensions'. " +
                        "Just last week we averted a cave-in by singing counter-harmonies!"
                    );
                    appsModule.AddOption("Incredible!", p => true, p => p.SendGump(new DialogueGump(p, irisModule)));
                    player.SendGump(new DialogueGump(player, appsModule));
                }
            );

            irisModule.AddOption("Dangers?",
                player => true,
                player => 
                {
                    DialogueModule dangersModule = new DialogueModule(
                        "The wrong note can:\n- Shatter precious gems\n- Awaken dormant elementals\n- Cause temporary...\n\n" +
                        "(She shows a photo of a glowing miner) ...transmutation of flesh to mineral. " +
                        "Poor Jorik spent three days as a living statue before we found the antidote!"
                    );
                    dangersModule.AddOption("Recipe for disaster", p => true, p => p.SendGump(new DialogueGump(p, irisModule)));
                    player.SendGump(new DialogueGump(player, dangersModule));
                }
            );

            return irisModule;
        }

        private DialogueModule CreateProjectModule()
        {
            DialogueModule projectModule = new DialogueModule(
                "My magnum opus - 'The Lexicon of Living History'! Each page is: " +
                "- Impregnated with memory moss\n- Bound in shapeshifter hide\n- Inked with...\n\n" +
                "(She glances around nervously) ...well, let's say the 'pigment' from certain Fawn paintings. " +
                "It's been... communicating lately. Last night it rearranged itself to show this symbol..."
            );

            projectModule.AddOption("What symbol?",
                player => true,
                player => 
                {
                    DialogueModule symbolModule = new DialogueModule(
                        "(She quickly sketches an eye within a spiral within a crescent)\n" +
                        "Appears in:\n- Pirate Isle tide carvings\n- Exodus Castle's foundation stones\n" +
                        "- The birthmarks of every third child in Dawn this year\n" +
                        "I believe it's a...\n\n" +
                        "(The sketch suddenly combusts) ...self-preserving concept. Handle with care."
                    );
                    symbolModule.AddOption("Alarming", p => true, p => p.SendGump(new DialogueGump(p, projectModule)));
                    player.SendGump(new DialogueGump(player, symbolModule));
                }
            );

            projectModule.AddOption("Need assistance?",
                player => true,
                player => 
                {
                    DialogueModule helpModule = new DialogueModule(
                        "Three crucial items needed:\n\n" +
                        "1. A tear from the Moon Portal's guardian\n2. The ink used in Arielle's first painting\n" +
                        "3. A page from the 'Book of Unwritten Moments'\n\n" +
                        "But be warned - retrieving these may alter your... let's call it 'narrative cohesion'."
                    );
                    helpModule.AddOption("I'll help", p => true, p => 
                    {
                        // Start quest
                        p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                    });
                    helpModule.AddOption("Too abstract", p => true, p => p.SendGump(new DialogueGump(p, projectModule)));
                    player.SendGump(new DialogueGump(player, helpModule));
                }
            );

            return projectModule;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}