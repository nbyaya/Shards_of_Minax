using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Nellie : BaseCreature
    {
        [Constructable]
        public Nellie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Nellie";
            Body = 0x191; // Human female body
            Hue = Race.RandomSkinHue();
            HairItemID = 0x203B; // Pageboy haircut
            HairHue = 0x47E; // Honey blonde

            // Stats
            SetStr(45);
            SetDex(30);
            SetInt(65);
            SetHits(60);

            // Appearance
            AddItem(new PlainDress() { Hue = 0x3B4 }); // Earth-toned dress
            AddItem(new Sandals() { Hue = 0x743 });
            AddItem(new StrawHat() { Hue = 0x742 });
            AddItem(new FullApron() { Hue = 0x47E });

            // Inventory
            PackItem(new DriedHerbs());
            PackItem(new GardenJournal());
        }

        public Nellie(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            if (InRange(player, 5))
            {
                player.SendGump(new DialogueGump(player, CreateGreetingModule()));
            }
            else
            {
                this.Say("Come closer, dear! These old ears don't carry like they used to.");
            }
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule(
                "Oh! Hello there, petal! *adjusts her sunhat* " +
                "You caught me between my morning pruning and afternoon potting. " +
                "What brings you to my little corner of Yew's embrace?"
            );

            // Main dialogue branches
            greeting.AddOption("I heard you exchange letters with a child in West Montor.",
                player => true,
                player => PenPalDialogue(player));

            greeting.AddOption("Heidi mentioned your herbal remedies.",
                player => true,
                player => HerbalismDialogue(player));

            greeting.AddOption("This forest seems... different.",
                player => true,
                player => ForestLoreDialogue(player));

            greeting.AddOption("What's growing in your garden?",
                player => true,
                player => GardenDialogue(player));

            return greeting;
        }

        private void PenPalDialogue(PlayerMobile player)
        {
            DialogueModule penPalModule = new DialogueModule(
                "*Her eyes light up* Oh yes! Little Lissa - sweet as spring dew! " +
                "We've been pen pals since her family moved to West Montor last harvest. " +
                "Darvin's her uncle, you know. Between you and me, I think she gets lonely...\n\n" +
                "Last letter said she found a nest of glow-moths in their barley field!"
            );

            penPalModule.AddOption("What do you write about?",
                pl => true,
                pl => 
                {
                    DialogueModule contentModule = new DialogueModule(
                        "Oh, everything under the sun! I send pressed flowers from Yew's groves, " +
                        "she sketches the strange mushrooms growing near Montor's old mill. " +
                        "Last moon-cycle, we started comparing cloud shapes!\n\n" +
                        "*She taps her chin* Though... her last drawing looked more like a shadow than a cloud."
                    );
                    contentModule.AddOption("Can I help with your next letter?",
                        p => true,
                        p => LetterQuestHook(p));
                    pl.SendGump(new DialogueGump(pl, contentModule));
                });

            penPalModule.AddOption("Does Darvin know about this?",
                pl => true,
                pl => 
                {
                    DialogueModule darvinModule = new DialogueModule(
                        "*She chuckles* That old stump? He thinks we're exchanging recipes! " +
                        "Men like him see thyme as seasoning, not medicine. " +
                        "But between us girls... *leans in* " +
                        "Lissa's been teaching me Montor's sign-language for talking during silent hunts."
                    );
                    darvinModule.AddOption("Sign language?",
                        p => true,
                        p => p.SendMessage("Nellie demonstrates complex finger movements resembling animal tracks"));
                    pl.SendGump(new DialogueGump(pl, darvinModule));
                });

            player.SendGump(new DialogueGump(player, penPalModule));
        }

        private void HerbalismDialogue(PlayerMobile player)
        {
            DialogueModule herbsModule = new DialogueModule(
                "*She opens her herb satchel with practiced fingers* " +
                "Heidi's the only one in Dawn who appreciates proper moon-cycle harvesting! " +
                "Did you know we've developed a new sleep tonic using:\n" +
                "- Silverleaf (plucked at crescent moon)\n" +
                "- Whisperroot (dug with copper tools)\n" +
                "- And... *glances around* a secret third ingredient?"
            );

            herbsModule.AddOption("Secret ingredient?",
                pl => true,
                pl => 
                {
                    DialogueModule secretModule = new DialogueModule(
                        "*She lowers her voice* It's not in any herbology tome. " +
                        "The old grove guardians called it 'Tears of Yew' - " +
                        "sap from lightning-struck trees mixed with dawn dew. " +
                        "But shhh! If the wrong folks heard... *gestures to dark forest edge*"
                    );
                    secretModule.AddOption("I could help gather some.",
                        p => true,
                        p => 
                        {
                            if (p.Backpack != null && !p.Backpack.Deleted)
                            {
                                p.AddToBackpack(new EmptyVial());
                                p.SendMessage("Nellie gives you a delicate crystal vial");
                            }
                        });
                    pl.SendGump(new DialogueGump(pl, secretModule));
                });

            herbsModule.AddOption("Any advice for a beginner?",
                pl => true,
                pl => 
                {
                    DialogueModule adviceModule = new DialogueModule(
                        "Three rules, petal:\n" +
                        "1. Never harvest wolfsbane without iron gloves\n" +
                        "2. Sing to your rosemary - it grows bitter in silence\n" +
                        "3. And most important... *her voice hardens* " +
                        "If the mushrooms start singing back, RUN."
                    );
                    pl.SendGump(new DialogueGump(pl, adviceModule));
                });

            player.SendGump(new DialogueGump(player, herbsModule));
        }

        private void ForestLoreDialogue(PlayerMobile player)
        {
            DialogueModule forestModule = new DialogueModule(
                "*Her cheerful demeanor falters* The groves remember... things. " +
                "Old magic sleeps in the bark and stones. " +
                "My grandmother used to say Yew's trees grow roots through time itself.\n\n" +
                "Lately... *she touches an oak's trunk* ...the nightmares come more frequent."
            );

            forestModule.AddOption("Nightmares?",
                pl => true,
                pl => 
                {
                    DialogueModule nightmareModule = new DialogueModule(
                        "Not mine - the trees'. In the dark hours, their whispers become... screams. " +
                        "Last week I found an entire patch of foxglove wilted black by dawn! " +
                        "Heidi thinks it's just blight, but I know bloodrot when I smell it."
                    );
                    nightmareModule.AddOption("Bloodrot?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, new DialogueModule(
                            "Ancient corruption. Grows where violence soaked the earth. " +
                            "The old records say... *Her voice trails off* Never mind. " +
                            "Probably just a sick raccoon. Yes. That's all."
                        ))));
                    pl.SendGump(new DialogueGump(pl, nightmareModule));
                });

            player.SendGump(new DialogueGump(player, forestModule));
        }

        private void GardenDialogue(PlayerMobile player)
        {
            DialogueModule gardenModule = new DialogueModule(
                "*She beams with pride* This season's star is my moonpetal hybrid! " +
                "Cross-bred Yew's native nightbloom with seeds from Renika's coral isles. " +
                "They glow faintly blue during rainfall!\n\n" +
                "But between us... *she frowns at a peculiar weed* " +
                "This stubborn thing keeps returning no matter how I pull it."
            );

            gardenModule.AddOption("Can I see the moonpetals?",
                pl => true,
                pl => 
                {
                    pl.SendMessage("Nellie shows you iridescent flowers that hum faintly");
                    if (pl.Backpack != null)
                    {
                        pl.AddToBackpack(new Moonpetal());
                        pl.SendMessage("She gifts you a carefully wrapped bloom");
                    }
                });

            gardenModule.AddOption("What's special about that weed?",
                pl => true,
                pl => 
                {
                    DialogueModule weedModule = new DialogueModule(
                        "Its roots... *she digs up a fibrous mass* " +
                        "See these patterns? Like tiny faces screaming. " +
                        "Mother called them 'soulbinders' - said they grow where... *She suddenly tosses it away* Pah! Old wives' tales."
                    );
                    weedModule.AddOption("Where what?",
                        p => true,
                        p => p.SendGump(new DialogueGump(p, new DialogueModule(
                            "*Her voice drops* Where someone buried their secrets alive. " +
                            "Now enough gloomy talk! Try this lavender honeycake."
                        ))));
                    pl.SendGump(new DialogueGump(pl, weedModule));
                });

            player.SendGump(new DialogueGump(player, gardenModule));
        }

        private void LetterQuestHook(PlayerMobile player)
        {
            DialogueModule questModule = new DialogueModule(
                "How kind! I was planning to send Lissa a pressed glowmoth wing specimen. " +
                "But... *she glances nervously at the forest* " +
                "The best moths gather near the Old Sentinel tree after dark.\n\n" +
                "Would you accompany me tonight? I'll need protection while collecting."
            );

            questModule.AddOption("I'll protect you",
                pl => true,
                pl => 
                {
                    pl.AddToBackpack(new MaxxiaScroll());
                    pl.SendMessage("Nellie gives you her late mother's reinforced net");
                    // Trigger quest flag here
                });

            questModule.AddOption("What's dangerous about moths?",
                pl => true,
                pl => 
                {
                    pl.SendGump(new DialogueGump(pl, new DialogueModule(
                        "Not the moths, petal - what follows them. " +
                        "Glowmoth pheromones attract... other things. " +
                        "Last season, Crag from Devil Guard found teeth marks in his net!"
                    )));
                });

            player.SendGump(new DialogueGump(player, questModule));
        }

        // Serialization
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

    // Custom Items
    public class GardenJournal : Item
    {
        [Constructable]
        public GardenJournal() : base(0x1C11)
        {
            Name = "Nellie's Garden Journal";
            Hue = 0x58B;
        }
        //...serialization
        public GardenJournal(Serial serial)
            : base(serial)
        {
        }        
		public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }		
    }

    public class Moonpetal : Item
    {
        [Constructable]
        public Moonpetal() : base(0x234B)
        {
            Name = "Lunar Hybrid Bloom";
            Hue = 0x4F2;
        }
        //...serialization
        public Moonpetal(Serial serial)
            : base(serial)
        {
        }

		public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }		
    }
}