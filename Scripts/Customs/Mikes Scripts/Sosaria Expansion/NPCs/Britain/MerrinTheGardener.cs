using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class MerrinTheGardener : BaseCreature
    {
        [Constructable]
        public MerrinTheGardener() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Merrin";
            Title = "the Verdant Gardener";
            Body = 0x191; // Human female body
            Female = true;

            SetStr(80);
            SetDex(90);
            SetInt(85);

            AddItem(new StrawHat() { Hue = 746 });
            AddItem(new LeafTonlet() { Hue = 746 });
            AddItem(new FullApron() { Hue = 2101 });
            AddItem(new Sandals() { Hue = 1810 });

            HairItemID = 0x203B; // Short bob
            HairHue = 1149; // Auburn
            Hue = Race.RandomSkinHue();
        }

        public MerrinTheGardener(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            player.SendGump(new DialogueGump(player, CreateGreetingModule()));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule(
                "Well hello there! *brushes soil from her gloves* I'm Merrin, tender of Castle British's gardens and " +
                "keeper of Sosaria's sweetest strawberries. Are you here to talk plants, or perhaps sample this season's " +
                "moon-kissed melons? They're particularly juicy since the last blue phase..."
            );

            // Main dialogue options
            greeting.AddOption("Your gardens are beautiful! Any growing secrets?",
                player => true,
                player => SendGardeningTips(player));

            greeting.AddOption("I heard you supply Bryn the Baker",
                player => true,
                player => SendBrynDialogue(player));

            greeting.AddOption("Old Bramble mentioned your collaborations",
                player => true,
                player => SendBrambleDialogue(player));

            greeting.AddOption("What's that unusual flower you're tending?",
                player => true,
                player => SendSecretProjectDialogue(player));

            greeting.AddOption("Just enjoying the scenery",
                player => true,
                player => 
                    player.SendMessage("Merrin winks and hands you a perfect strawberry. \"The earth provides, if we listen.\""));

            return greeting;
        }

        private void SendGardeningTips(PlayerMobile player)
        {
            DialogueModule tipsModule = new DialogueModule(
                "Secrets? Oh, it's all in the listening! *she kneads rich soil between her fingers* " +
                "The blacksoil from Devil Guard's hot springs makes roses bloom crimson, but you must " +
                "mix in Yew's oakleaf mold to balance the pH. Now, what specifically interests you?"
            );

            tipsModule.AddOption("Rare plants in Sosaria",
                p => true,
                p => 
                {
                    DialogueModule rarePlants = new DialogueModule(
                        "Ah! The Frostbloom only grows near Ice Cavern entrances during waning moons. " +
                        "But beware - its nectar attracts Phase Spiders. Old Bramble and I nearly lost " +
                        "our eyebrows collecting some for Bryn's Winterveil tarts last year!"
                    );
                    p.SendGump(new DialogueGump(p, rarePlants));
                });

            tipsModule.AddOption("Dealing with pests",
                p => true,
                p => 
                {
                    DialogueModule pests = new DialogueModule(
                        "For root borers, plant Marigold of Mourning - they hate the scent. " +
                        "But the real trouble comes from... *she lowers her voice* ...Spirit Slimes. " +
                        "They emerge from cursed soil. If you find any, tell Iris at Devil Guard immediately!"
                    );
                    pests.AddOption("Spirit Slimes?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule slimes = new DialogueModule(
                                "They're not natural creatures - manifestations of unresolved grief, " +
                                "according to Cassian from Fawn. Last summer, an infestation in West Montor's " +
                                "graveyard turned pumpkins into wailing... things. We had to burn the whole crop."
                            );
                            pl.SendGump(new DialogueGump(pl, slimes));
                        });
                    p.SendGump(new DialogueGump(p, pests));
                });

            tipsModule.AddOption("Moon's effect on crops",
                p => true,
                p => 
                {
                    DialogueModule moonEffects = new DialogueModule(
                        "Ah! During the Crimson Moon phase, tomatoes grow as big as your head but become " +
                        "slightly... sentient. Bryn uses them for 'surprise stews'. When the moon enters " +
                        "Umbra Veil alignment... *she shudders* ...we don't speak of those harvests."
                    );
                    moonEffects.AddOption("Umbra Veil harvests?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule forbidden = new DialogueModule(
                                "Three years ago, I planted turnips under that accursed light. They grew eyes " +
                                "and started reciting lost poetry. Lady Isobel had the whole batch destroyed. " +
                                "Though... *she glances around* ...I kept one bulb. For research."
                            );
                            pl.SendGump(new DialogueGump(pl, forbidden));
                        });
                    p.SendGump(new DialogueGump(p, moonEffects));
                });

            player.SendGump(new DialogueGump(player, tipsModule));
        }

        private void SendBrynDialogue(PlayerMobile player)
        {
            DialogueModule brynModule = new DialogueModule(
                "Sweet Bryn! Our flour-and-flower exchange keeps both castle and bakery blooming. " +
                "Just yesterday I delivered a batch of Shadow Peppers - gives her famous meat pies that " +
                "extra 'dimensional kick'. Though we did have that incident with the floating crusts..."
            );

            brynModule.AddOption("Floating crusts?",
                p => true,
                p => 
                {
                    DialogueModule incident = new DialogueModule(
                        "When Bryn used my Levitation Honey without proper balancing herbs! " +
                        "Twenty meat pies ascended into Castle British's rafters during the Autumn Feast. " +
                        "Sir Thomund still finds occasional gravy stains on his armor."
                    );
                    p.SendGump(new DialogueGump(p, incident));
                });

            brynModule.AddOption("What's your favorite creation?",
                p => true,
                p => 
                {
                    DialogueModule favorite = new DialogueModule(
                        "Her Stardew Sourdough, made with my Eclipse Wheat! The starter culture is " +
                        "over 200 years old, passed down from Bramble's grandmother. It has... " +
                        "*she whispers* ...a faint magical glow. Perfect for midnight picnics."
                    );
                    p.SendGump(new DialogueGump(p, favorite));
                });

            brynModule.AddOption("Any new collaborations?",
                p => true,
                p => 
                {
                    DialogueModule collaboration = new DialogueModule(
                        "We're experimenting with Crystal Berries from Devil Guard's mines! " +
                        "They make jam that... well, let's just say it helps Jonas from West Montor " +
                        "with his 'dream research'. Though Bryn insists on calling it 'Mystic Marmalade'."
                    );
                    collaboration.AddOption("Dream research?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule dreams = new DialogueModule(
                                "Jonas believes certain plants can bridge waking and dreaming. " +
                                "Last week he ate a whole jar and claimed to converse with a turnip " +
                                "that knew three dead languages. We're... monitoring the dosage."
                            );
                            pl.SendGump(new DialogueGump(pl, dreams));
                        });
                    p.SendGump(new DialogueGump(p, collaboration));
                });

            player.SendGump(new DialogueGump(player, brynModule));
        }

        private void SendBrambleDialogue(PlayerMobile player)
        {
            DialogueModule brambleModule = new DialogueModule(
                "Old Bramble is a treasure! He taught me the Oak Whisper technique - lets you " +
                "diagnose tree ailments by listening to sap flow. Though his 'remedies' often involve " +
                "singing to saplings at dawn... which actually works, curse him!"
            );

            brambleModule.AddOption("Oak Whisper technique?",
                p => true,
                p => 
                {
                    DialogueModule technique = new DialogueModule(
                        "Press your ear to the bark during high sun, and you'll hear the tree's song. " +
                        "A healthy oak hums in F-sharp. The Yew Grandfather Tree? A full choir! " +
                        "But the blighted ones... *she shudders* ...they scream in silence."
                    );
                    p.SendGump(new DialogueGump(p, technique));
                });

            brambleModule.AddOption("Any joint projects?",
                p => true,
                p => 
                {
                    DialogueModule projects = new DialogueModule(
                        "We're reviving the Lost Orchard of Moonlight! Legend says its fruits " +
                        "could cure any ailment. So far we've grown apples that make you speak " +
                        "in rhymes... Progress! *she holds up a glowing sapling* Want to help?"
                    );
                    projects.AddOption("How can I help?",
                        pl => true,
                        pl => 
                        {
                            pl.AddToBackpack(new MysticSapling());
                            DialogueModule questStart = new DialogueModule(
                                "Take this sapling to the Temple Ruins' sacred spring! " +
                                "But BEWARE - the water must be collected during a crescent moon. " +
                                "Return to me with Blessed Water, and I'll share Bramble's Ancient Cider recipe!"
                            );
                            pl.SendGump(new DialogueGump(pl, questStart));
                        });
                    projects.AddOption("Maybe later",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    p.SendGump(new DialogueGump(p, projects));
                });

            player.SendGump(new DialogueGump(player, brambleModule));
        }

        private void SendSecretProjectDialogue(PlayerMobile player)
        {
            DialogueModule secretModule = new DialogueModule(
                "*she quickly steps between you and a peculiar violet-blossomed plant* That? " +
                "Just a... hybrid. Nothing to see! Though... *leans closer* ...are you familiar " +
                "with Fungal Caves? I need someone discreet to fetch Sporecap Pollen..."
            );

            secretModule.AddOption("Fungal Caves? Dangerous!",
                p => true,
                p => 
                {
                    DialogueModule warning = new DialogueModule(
                        "Yes, but this could revolutionize agriculture! The pollen allows plants " +
                        "to grow without sunlight. Imagine crops during eternal night! Though... " +
                        "*she glances at a mutated zucchini* ...size control needs work."
                    );
                    warning.AddOption("I'll help (Start Quest)",
                        pl => true,
                        pl => 
                        {
                            pl.AddToBackpack(new GlassVialSet());
                            DialogueModule quest = new DialogueModule(
                                "Take these crystal vials - only they can preserve the pollen! " +
                                "Navigate Catastrophe's bioluminescent depths, but DON'T inhale " +
                                "the spores! Return safely, and I'll name this discovery after you!"
                            );
                            pl.SendGump(new DialogueGump(pl, quest));
                        });
                    p.SendGump(new DialogueGump(p, warning));
                });

            secretModule.AddOption("Not my business",
                p => true,
                p => 
                {
                    DialogueModule retreat = new DialogueModule(
                        "Wise choice. *she waters the plant which squeaks happily* " +
                        "Some mysteries should grow undisturbed... for now."
                    );
                    p.SendGump(new DialogueGump(p, retreat));
                });

            player.SendGump(new DialogueGump(player, secretModule));
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

    // Quest items
    public class MysticSapling : Item
    {
        [Constructable]
        public MysticSapling() : base(0x0CE9)
        {
            Name = "Mystic Sapling";
            Hue = 1153;
        }
        //... serialization methods
        public MysticSapling(Serial serial)
            : base(serial)
        {
        }        
        public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }		
    }

    public class GlassVialSet : Item
    {
        [Constructable]
        public GlassVialSet() : base(0x0E24)
        {
            Name = "Crystal Pollen Vials";
            Hue = 1175;
        }
        //... serialization methods
        public GlassVialSet(Serial serial)
            : base(serial)
        {
        }        
		public override void Serialize(GenericWriter writer) { base.Serialize(writer); writer.Write(0); }
        public override void Deserialize(GenericReader reader) { base.Deserialize(reader); int version = reader.ReadInt(); }
    }
}