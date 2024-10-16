using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherd Luke")]
    public class ShepherdLuke : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdLuke() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherd Luke";
            Body = 0x190; // Human male body

            // Stats
            Str = 82;
            Dex = 62;
            Int = 48;
            Hits = 73;

            // Appearance
            AddItem(new ShortPants() { Hue = 1132 });
            AddItem(new Shirt() { Hue = 1131 });
            AddItem(new Sandals() { Hue = 0 });
            AddItem(new ShepherdsCrook() { Name = "Shepherd Luke's Crook" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

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
            DialogueModule greeting = new DialogueModule("I'm Shepherd Luke, the miserable shepherd. My sheep are my only companions. Would you like to talk about them?");

            greeting.AddOption("Tell me about your sheep.",
                player => true,
                player => 
                {
                    DialogueModule sheepModule = new DialogueModule("Ah, my sheep! They're soft, woolly wonders. Their fleece is like a cloud, comforting and warm. I often find myself lost in their embrace. Would you like to know more about their beauty?");
                    sheepModule.AddOption("What makes their fleece so special?",
                        p => true,
                        p => 
                        {
                            DialogueModule fleeceModule = new DialogueModule("Their wool is a treasure, a soft tapestry woven by nature. Each strand holds secrets of the land and warmth of the sun. I could spend hours caressing their wool, feeling every curl and twist.");
                            fleeceModule.AddOption("That sounds… intimate.",
                                pl => true,
                                pl => 
                                {
                                    DialogueModule intimateModule = new DialogueModule("Intimacy is what I feel with them. It's a bond beyond words. You can't understand the joy of wrapping your fingers in their fluffy wool, can you?");
                                    intimateModule.AddOption("Maybe not.",
                                        pq => true,
                                        pq => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                                    fleeceModule.AddOption("What do you do with the wool?",
                                        plw => true,
                                        plw => 
                                        {
                                            DialogueModule useWoolModule = new DialogueModule("Oh, the uses are endless! I spin it into threads and weave it into soft blankets. Each piece carries a piece of my heart, wrapped in warmth.");
                                            useWoolModule.AddOption("Can I see a blanket?",
                                                pe => true,
                                                pe => { SendMessage("Here, feel the softness! It’s like a warm hug from a thousand sheep!"); });
                                            useWoolModule.AddOption("I think you've lost your mind.",
                                                pr => true,
                                                pr => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                                            fleeceModule.AddOption("Show me your weaving.",
                                                plt => true,
                                                plt => { SendMessage("I’ll gladly show you my creations, but be warned—the softness is enchanting!"); });
                                            pl.SendGump(new DialogueGump(pl, useWoolModule));
                                        });
                                    p.SendGump(new DialogueGump(p, intimateModule));
                                });
                            p.SendGump(new DialogueGump(p, fleeceModule));
                        });
                    sheepModule.AddOption("How do you care for them?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule careModule = new DialogueModule("Caring for them is a labor of love. I groom their wool, making sure it's fluffy and beautiful. Sometimes, I sing to them; they respond with soft bleats of affection.");
                            careModule.AddOption("Do they really respond to singing?",
                                p => true,
                                p => 
                                {
                                    DialogueModule singModule = new DialogueModule("Oh yes! When I sing, they gather around me, their soft wool brushing against my skin. It's a feeling of pure bliss, like being enveloped in a warm embrace.");
                                    singModule.AddOption("That sounds beautiful.",
                                        ply => true,
                                        ply => { p.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                                    singModule.AddOption("It's a bit creepy, don't you think?",
                                        plu => true,
                                        plu => 
                                        {
                                            DialogueModule creepyModule = new DialogueModule("Creepy? Perhaps. But the bond I share with them is unmatched. There's a comfort in their presence that no human can provide.");
                                            creepyModule.AddOption("I guess that’s true.",
                                                pi => true,
                                                pi => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                                            p.SendGump(new DialogueGump(p, creepyModule));
                                        });
                                    p.SendGump(new DialogueGump(p, singModule));
                                });
                            pl.SendGump(new DialogueGump(pl, careModule));
                        });
                    player.SendGump(new DialogueGump(player, sheepModule));
                });

            greeting.AddOption("What happened to you?",
                player => true,
                player =>
                {
                    DialogueModule storyModule = new DialogueModule("Ah, my story is long and sorrowful. It begins with a betrayal and ends with my solace found in these woolly companions.");
                    storyModule.AddOption("Tell me about the betrayal.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule betrayalModule = new DialogueModule("It was my brother, Jacob. He took everything from me, even the woman I loved. Now I find comfort in my sheep, who will never betray me.");
                            betrayalModule.AddOption("How did you cope with the loss?",
                                p => true,
                                p => 
                                {
                                    DialogueModule copeModule = new DialogueModule("At first, I was consumed by despair. But the sheep’s gentle presence healed my wounds. They became my family, and their wool is a reminder of my newfound joy.");
                                    copeModule.AddOption("It sounds like you really love them.",
                                        plo => true,
                                        plo => { SendMessage("Love is an understatement! Their woolly warmth is the only thing that keeps my heart from freezing."); });
                                    copeModule.AddOption("Maybe you should see a healer.",
                                        pla => true,
                                        pla => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                                    p.SendGump(new DialogueGump(p, copeModule));
                                });
                            player.SendGump(new DialogueGump(player, betrayalModule));
                        });
                    storyModule.AddOption("What do you do to forget?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule forgetModule = new DialogueModule("I lose myself in the softness of their wool. Each time I touch it, I forget my sorrows, if only for a moment.");
                            forgetModule.AddOption("That's quite poetic.",
                                p => true,
                                p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                            forgetModule.AddOption("You might want to find a hobby.",
                                p => true,
                                p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                            player.SendGump(new DialogueGump(player, forgetModule));
                        });
                    player.SendGump(new DialogueGump(player, storyModule));
                });

            greeting.AddOption("Do you ever get lonely?",
                player => true,
                player =>
                {
                    DialogueModule lonelinessModule = new DialogueModule("Lonely? Only when the sun sets and I can no longer see their soft, woolly forms. But as long as they’re here, I feel complete.");
                    lonelinessModule.AddOption("It must be nice to have them.",
                        p => true,
                        p => { SendMessage("Nice? It's everything! Every moment spent with them is a treasure, filled with warmth and love."); });
                    lonelinessModule.AddOption("You should try talking to people.",
                        p => true,
                        p => { p.SendGump(new DialogueGump(p, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, lonelinessModule));
                });

            return greeting;
        }

        public ShepherdLuke(Serial serial) : base(serial) { }

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
