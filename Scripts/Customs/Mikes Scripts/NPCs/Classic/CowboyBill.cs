using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cowboy Bill")]
    public class CowboyBill : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CowboyBill() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cowboy Bill";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 95;
            Int = 55;
            Hits = 80;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1175 });
            AddItem(new LeatherChest() { Hue = 1175 });
            AddItem(new WideBrimHat() { Hue = 1173 });
            AddItem(new Boots() { Hue = 1150 });
            AddItem(new FireballWand() { Name = "Bill's Revolver" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue
        }

        public CowboyBill(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule("Howdy, partner! I'm Cowboy Bill, the quickest draw in these parts. What brings you here?");

            greeting.AddOption("Tell me about your job.",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("I wrangle up them pesky varmints and keep this town safe! It's a tough job, but someone's gotta do it.");
                    jobModule.AddOption("What kind of varmints?",
                        p => true,
                        p =>
                        {
                            DialogueModule varmintsModule = new DialogueModule("Them varmints can be real trouble, especially the coyotes that've been causin' a ruckus at the Johnson farm.");
                            varmintsModule.AddOption("How can I help?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule helpModule = new DialogueModule("If you help round 'em up, there might be a reward in it for ya. What do ya say?");
                                    helpModule.AddOption("I'm in!",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendMessage("You agree to help Cowboy Bill with the coyotes.");
                                            // Additional logic for quest initiation can go here
                                        });
                                    helpModule.AddOption("Maybe another time.",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                        });
                                    pl.SendGump(new DialogueGump(pl, helpModule));
                                });
                            varmintsModule.AddOption("Sounds dangerous.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, varmintsModule));
                        });
                    jobModule.AddOption("I wish you luck out there.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("Do you sell anything?",
                player => true,
                player =>
                {
                    DialogueModule shopModule = new DialogueModule("I do have some spare supplies I've gathered. Feel free to browse my collection.");
                    shopModule.AddOption("What do you have?",
                        p => true,
                        p =>
                        {
                            // Logic to show the player's buy/sell gump or shop interface
                            p.SendMessage("Cowboy Bill shows you his collection of supplies.");
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    shopModule.AddOption("Maybe later.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, shopModule));
                });

            greeting.AddOption("Tell me a story.",
                player => true,
                player =>
                {
                    DialogueModule storyModule = new DialogueModule("Ah, stories are what make a cowboy's heart beat! Let me tell ya about the time I faced down a rattlesnake...");
                    storyModule.AddOption("What happened?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule snakeStoryModule = new DialogueModule("Well, I reckon it was a fierce encounter! That rattlesnake bite was somethin' fierce. But if you ever get bitten, there's an old remedy my grandma used to swear by. You interested?");
                            snakeStoryModule.AddOption("Yes, tell me about it.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); // Add remedy info if needed
                                });
                            snakeStoryModule.AddOption("Maybe another time.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, snakeStoryModule));
                        });
                    storyModule.AddOption("Tell me about your time ranching blue oxen.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule ranchingModule = new DialogueModule("Oh, the enchanted blue oxen! Now those were some mighty creatures! I had a whole herd of 'em at one point. Let me tell ya, they were as strong as an ox and twice as stubborn!");
                            ranchingModule.AddOption("What made them enchanted?",
                                p => true,
                                p =>
                                {
                                    DialogueModule enchantmentModule = new DialogueModule("Well, it all started when I stumbled upon a hidden glade deep in the Whispering Forest. There was a shimmering spring that glowed with a strange light. I had a feeling that if I let the oxen drink from it, they'd become something special.");
                                    enchantmentModule.AddOption("Did they really get stronger?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule strengthModule = new DialogueModule("You bet! After they drank from that spring, they could pull twice the load of a normal ox! I once saw one of 'em lift a fallen tree to clear a path for my wagon. It was a sight to behold!");
                                            strengthModule.AddOption("What else did they do?",
                                                pw => true,
                                                pw =>
                                                {
                                                    DialogueModule powersModule = new DialogueModule("Well, they had this glow about 'em. At night, they illuminated the ranch like living lanterns! And they had a knack for finding herbs and roots that could heal the sick.");
                                                    powersModule.AddOption("Did they help you with anything specific?",
                                                        ple => true,
                                                        ple =>
                                                        {
                                                            DialogueModule healingModule = new DialogueModule("One time, I had a neighbor whose cattle were sick. The blue oxen led me to this rare flower called the Silver Bloom, which cured them right up. I swear they knew what they were doing!");
                                                            healingModule.AddOption("That's incredible! Did you ever have trouble with them?",
                                                                pr => true,
                                                                pr =>
                                                                {
                                                                    DialogueModule troubleModule = new DialogueModule("Oh, you better believe it! One time, they got spooked by a coyote howling at the moon and charged right through my fence! I had to round them up for hours.");
                                                                    troubleModule.AddOption("What did you do to calm them down?",
                                                                        plt => true,
                                                                        plt =>
                                                                        {
                                                                            DialogueModule calmModule = new DialogueModule("I had to sing them a lullaby! Sounds silly, but they calmed right down when they heard my voice. It was my old mama’s tune; she always said it could soothe even the wildest beasts.");
                                                                            calmModule.AddOption("Your mama sounds wise.",
                                                                                py => true,
                                                                                py =>
                                                                                {
                                                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                                                });
                                                                            pl.SendGump(new DialogueGump(pl, calmModule));
                                                                        });
                                                                    troubleModule.AddOption("What happened next?",
                                                                        plu => true,
                                                                        plu =>
                                                                        {
                                                                            DialogueModule nextModule = new DialogueModule("Well, after I calmed 'em down, I had to fix the fence. But those blue oxen stuck around to help, using their strength to lift the broken pieces back in place.");
                                                                            nextModule.AddOption("They sound like great companions.",
                                                                                pi => true,
                                                                                pi =>
                                                                                {
                                                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                                                });
                                                                            pl.SendGump(new DialogueGump(pl, nextModule));
                                                                        });
                                                                    p.SendGump(new DialogueGump(p, troubleModule));
                                                                });
                                                            healingModule.AddOption("What happened to the blue oxen?",
                                                                po => true,
                                                                po =>
                                                                {
                                                                    DialogueModule fateModule = new DialogueModule("Eventually, I had to sell them to a traveling merchant. He saw their glow and knew their worth. I reckon they’re still out there, bringing luck to whomever owns them now.");
                                                                    fateModule.AddOption("I hope they found a good home.",
                                                                        plp => true,
                                                                        plp =>
                                                                        {
                                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                        });
                                                                    p.SendGump(new DialogueGump(p, fateModule));
                                                                });
                                                            pl.SendGump(new DialogueGump(pl, healingModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, powersModule));
                                                });
                                            p.SendGump(new DialogueGump(p, strengthModule));
                                        });
                                    p.SendGump(new DialogueGump(p, enchantmentModule));
                                });
                            pl.SendGump(new DialogueGump(pl, ranchingModule));
                        });
                    storyModule.AddOption("What else have you seen in your travels?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule travelsModule = new DialogueModule("Ah, I've seen my fair share! From wild storms that rolled through the plains to mysterious lights flickering in the night sky. But nothing quite tops my time with those blue oxen.");
                            travelsModule.AddOption("Tell me about a storm.",
                                p => true,
                                p =>
                                {
                                    DialogueModule stormModule = new DialogueModule("Oh boy! One time, a storm rolled in so fast, I barely had time to secure the cattle. The winds howled like a banshee! I had to hunker down and wait it out in the barn.");
                                    stormModule.AddOption("That sounds terrifying!",
                                        pla => true,
                                        pla =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, stormModule));
                                });
                            travelsModule.AddOption("What about the lights in the sky?",
                                p => true,
                                p =>
                                {
                                    DialogueModule lightsModule = new DialogueModule("Now, that’s a tale! I once saw lights dancing over the hills. Folks say they’re spirits of lost cattle, still roaming the plains. I was too curious to run away and ended up following them for a bit.");
                                    lightsModule.AddOption("Did you find anything?",
                                        pls => true,
                                        pls =>
                                        {
                                            DialogueModule findModule = new DialogueModule("Actually, I stumbled upon an old homestead that was long abandoned. Inside, I found some old relics and even a map leading to hidden treasures in the area! But that’s a story for another time.");
                                            findModule.AddOption("That sounds exciting! I want to hear more!",
                                                pd => true,
                                                pd =>
                                                {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule())); // Add more details if needed
                                                });
                                            pl.SendGump(new DialogueGump(pl, findModule));
                                        });
                                    p.SendGump(new DialogueGump(p, lightsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, travelsModule));
                        });
                    player.SendGump(new DialogueGump(player, storyModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendMessage("Take care, partner! Stay safe out there!");
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
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
