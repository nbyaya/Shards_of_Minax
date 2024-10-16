using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Galahad")]
    public class SirGalahad : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirGalahad() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Galahad";
            Body = 0x190; // Human male body
            Str = 155;
            Dex = 65;
            Int = 25;
            Hits = 110;

            AddItem(new ChainLegs() { Hue = 1150 });
            AddItem(new ChainChest() { Hue = 1150 });
            AddItem(new PlateHelm() { Hue = 1150 });
            AddItem(new PlateGloves() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1150 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("I am Sir Galahad, the once-great knight! But do you know what I cherish most? Mice!");

            greeting.AddOption("Mice? Why do you love them?",
                player => true,
                player =>
                {
                    DialogueModule miceLoveModule = new DialogueModule("Ah, mice! They are small, clever creatures that embody bravery in their own little ways. Would you like to hear a story about them?");
                    miceLoveModule.AddOption("Yes, tell me a story!",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateMiceStoryModule()));
                        });
                    miceLoveModule.AddOption("No, I want to talk about something else.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateOtherTopicsModule()));
                        });
                    player.SendGump(new DialogueGump(player, miceLoveModule));
                });

            greeting.AddOption("Do you have a favorite type of mouse?",
                player => true,
                player =>
                {
                    DialogueModule favoriteMouseModule = new DialogueModule("Indeed! I have a soft spot for the little field mice. Their tenacity in the face of danger inspires me.");
                    favoriteMouseModule.AddOption("What makes them so inspiring?",
                        p => true,
                        p =>
                        {
                            DialogueModule inspiringMouseModule = new DialogueModule("They scurry about, constantly searching for food, and never give up, even when faced with larger predators. They remind me of my own struggles.");
                            inspiringMouseModule.AddOption("What struggles?",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateStrugglesModule()));
                                });
                            player.SendGump(new DialogueGump(player, inspiringMouseModule));
                        });
                    player.SendGump(new DialogueGump(player, favoriteMouseModule));
                });

            greeting.AddOption("Can I help you with something related to mice?",
                player => true,
                player =>
                {
                    DialogueModule helpWithMiceModule = new DialogueModule("Ah, kind traveler! I have been looking for rare herbs that attract mice. If you find them, I shall reward you handsomely!");
                    helpWithMiceModule.AddOption("What herbs do you need?",
                        p => true,
                        p =>
                        {
                            DialogueModule herbsModule = new DialogueModule("I seek Micebane Flower and Moonlit Grasses. They thrive in the meadows during the full moon. Can you help me?");
                            herbsModule.AddOption("I’ll bring them to you!",
                                pla => true,
                                pla =>
                                {
                                    pla.SendMessage("You set off to gather the herbs for Sir Galahad.");
                                });
                            herbsModule.AddOption("That sounds too risky for me.",
                                pla => true,
                                pla =>
                                {
                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                });
                            p.SendGump(new DialogueGump(p, herbsModule));
                        });
                    player.SendGump(new DialogueGump(player, helpWithMiceModule));
                });

            return greeting;
        }

        private DialogueModule CreateMiceStoryModule()
        {
            DialogueModule storyModule = new DialogueModule("Once, there was a brave little mouse named Squeak. He lived in a vast field, where every day was an adventure.");
            storyModule.AddOption("What happened to Squeak?",
                p => true,
                p =>
                {
                    DialogueModule storyContinuation = new DialogueModule("Squeak faced many challenges, from dodging hungry birds to finding food. One day, he discovered a hidden stash of grain, but it was guarded by a fox!");
                    storyContinuation.AddOption("How did he get past the fox?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule foxEncounterModule = new DialogueModule("Squeak used his wits! He observed the fox and noticed it was always distracted by a shiny object. So, he found a pebble and threw it to create a diversion. He escaped with the grain!");
                            foxEncounterModule.AddOption("What a clever mouse!",
                                pq => true,
                                pq =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, foxEncounterModule));
                        });
                    p.SendGump(new DialogueGump(p, storyContinuation));
                });
            return storyModule;
        }

        private DialogueModule CreateOtherTopicsModule()
        {
            DialogueModule otherTopicsModule = new DialogueModule("Very well! We can talk about my past glories or the lands I’ve traveled.");
            otherTopicsModule.AddOption("Tell me about your past glories.",
                p => true,
                p =>
                {
                    p.SendGump(new DialogueGump(p, CreatePastGloriesModule()));
                });
            otherTopicsModule.AddOption("What lands have you traveled?",
                p => true,
                p =>
                {
                    p.SendGump(new DialogueGump(p, CreateTravelModule()));
                });
            return otherTopicsModule;
        }

        private DialogueModule CreateStrugglesModule()
        {
            DialogueModule strugglesModule = new DialogueModule("As a knight, I have faced many foes, but it is my internal battles that are most difficult. The memory of my fallen comrades weighs heavily on my heart.");
            strugglesModule.AddOption("How do you cope with that?",
                p => true,
                p =>
                {
                    DialogueModule copingModule = new DialogueModule("I find solace in the small things, like the presence of mice. Their simple joys remind me of life's fleeting beauty.");
                    copingModule.AddOption("What else brings you joy?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateJoyModule()));
                        });
                    p.SendGump(new DialogueGump(p, copingModule));
                });
            return strugglesModule;
        }

        private DialogueModule CreatePastGloriesModule()
        {
            DialogueModule pastGloriesModule = new DialogueModule("I was once a revered knight of the realm, known for my bravery in battles and my unyielding spirit. I fought in the Battle of Ravenwood and saved countless lives.");
            pastGloriesModule.AddOption("What was that battle like?",
                p => true,
                p =>
                {
                    DialogueModule battleModule = new DialogueModule("The skies were dark with smoke, and the cries of the wounded filled the air. I led my men against a formidable foe, and we emerged victorious.");
                    battleModule.AddOption("That sounds incredible!",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    p.SendGump(new DialogueGump(p, battleModule));
                });
            return pastGloriesModule;
        }

        private DialogueModule CreateTravelModule()
        {
            DialogueModule travelModule = new DialogueModule("I have traversed many lands, from the enchanted forests of Eldergrove to the bustling markets of Serpent's Reach. Each place has its own stories to tell.");
            travelModule.AddOption("What stories did you find there?",
                p => true,
                p =>
                {
                    DialogueModule storiesModule = new DialogueModule("In Eldergrove, I met a wise old owl who shared secrets of the forest. In Serpent's Reach, I learned about rare potions from a traveling merchant.");
                    storiesModule.AddOption("I would love to hear more about the owl!",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateOwlStoryModule()));
                        });
                    p.SendGump(new DialogueGump(p, storiesModule));
                });
            return travelModule;
        }

        private DialogueModule CreateOwlStoryModule()
        {
            DialogueModule owlModule = new DialogueModule("The owl spoke of the importance of harmony between all creatures. It told me that every being, even the smallest mouse, has a role in the grand tapestry of life.");
            owlModule.AddOption("That’s a profound lesson.",
                p => true,
                p =>
                {
                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                });
            return owlModule;
        }

        private DialogueModule CreateJoyModule()
        {
            DialogueModule joyModule = new DialogueModule("I find joy in helping others, telling stories, and of course, the fleeting moments shared with nature. The laughter of children echoes in my heart.");
            joyModule.AddOption("You have a kind heart, Sir Galahad.",
                pl => true,
                pl =>
                {
                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                });
            return joyModule;
        }

        public SirGalahad(Serial serial) : base(serial) { }

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
