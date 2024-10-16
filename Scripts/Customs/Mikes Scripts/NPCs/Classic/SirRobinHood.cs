using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Robin Hood")]
    public class SirRobinHood : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirRobinHood() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Robin Hood";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 120;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 2126 });
            AddItem(new StuddedChest() { Hue = 2126 });
            AddItem(new StuddedGloves() { Hue = 2126 });
            AddItem(new StuddedArms() { Hue = 2126 });
            AddItem(new StuddedGorget() { Hue = 2126 });
            AddItem(new Boots() { Hue = 2126 });
            AddItem(new Bow() { Name = "Sir Robin Hood's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public SirRobinHood(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Ah, greetings! I am Sir Robin Hood, the 'hero' of the poor. Ha! Care to hear about my most daring exploits?");

            greeting.AddOption("Tell me about your best robbery.",
                player => true,
                player => 
                {
                    DialogueModule robberyModule = new DialogueModule("Ah, the thrill of the chase! One of my finest was when I raided the Sheriff of Nottingham's treasury. It was a night to remember!");
                    robberyModule.AddOption("What happened during the raid?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule raidDetailsModule = new DialogueModule("The night was dark, and the moon was my only witness. I crept in with my merry band, avoiding guards with the grace of shadows.");
                            raidDetailsModule.AddOption("Were you caught?",
                                p => true,
                                p =>
                                {
                                    DialogueModule caughtModule = new DialogueModule("Not this time! Just as we reached the treasury, I heard footsteps approaching. We quickly disguised ourselves as guards and fooled them.");
                                    caughtModule.AddOption("Impressive! Did you escape with treasure?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule treasureModule = new DialogueModule("Indeed! We filled our bags with gold and jewels. The look on the Sheriff’s face when he found his vault empty was priceless!");
                                            player.SendGump(new DialogueGump(player, treasureModule));
                                        });
                                    player.SendGump(new DialogueGump(player, caughtModule));
                                });
                            raidDetailsModule.AddOption("What did you do next?",
                                p => true,
                                p =>
                                {
                                    DialogueModule escapeModule = new DialogueModule("We sprinted through the forest, laughter echoing in the night. Our merry band celebrated with a feast under the stars.");
                                    escapeModule.AddOption("That sounds incredible!",
                                        plw => true,
                                        plw => 
                                        {
                                            DialogueModule celebrationModule = new DialogueModule("Ah, it was a night of joy! But what made it truly special was sharing it with those in need.");
                                            player.SendGump(new DialogueGump(player, celebrationModule));
                                        });
                                    player.SendGump(new DialogueGump(player, escapeModule));
                                });
                            player.SendGump(new DialogueGump(player, raidDetailsModule));
                        });
                    player.SendGump(new DialogueGump(player, robberyModule));
                });

            greeting.AddOption("Do you have any other memorable stories?",
                player => true,
                player =>
                {
                    DialogueModule memorableStoriesModule = new DialogueModule("Oh, countless tales! One time, I intercepted a tax collector on his way to the castle, heavy with gold!");
                    memorableStoriesModule.AddOption("How did you manage that?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule collectorModule = new DialogueModule("I disguised myself as a peasant, blending in with the villagers. When he stopped for a break, I struck!");
                            collectorModule.AddOption("What did you do next?",
                                p => true,
                                p =>
                                {
                                    DialogueModule aftermathModule = new DialogueModule("I filled my pack with gold, then scattered his papers in the wind. The look on his face when he woke up—priceless!");
                                    player.SendGump(new DialogueGump(player, aftermathModule));
                                });
                            player.SendGump(new DialogueGump(player, collectorModule));
                        });
                    memorableStoriesModule.AddOption("Any other daring deeds?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule otherDeedsModule = new DialogueModule("There was the time I rescued a group of villagers from a band of mercenaries. They were going to be sold as slaves!");
                            otherDeedsModule.AddOption("What happened then?",
                                p => true,
                                p =>
                                {
                                    DialogueModule rescueModule = new DialogueModule("With quick thinking, I staged a false attack, distracting the mercenaries long enough for the villagers to escape. They owe me their freedom!");
                                    player.SendGump(new DialogueGump(player, rescueModule));
                                });
                            player.SendGump(new DialogueGump(player, otherDeedsModule));
                        });
                    player.SendGump(new DialogueGump(player, memorableStoriesModule));
                });

            greeting.AddOption("Do you ever regret your actions?",
                player => true,
                player =>
                {
                    DialogueModule regretModule = new DialogueModule("Regret? Sometimes I wonder if I could have helped even more, but the need is great, and I can only do so much.");
                    regretModule.AddOption("Your heart is in the right place.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule heartModule = new DialogueModule("Thank you! I believe that true heroism lies in the intent to help others, regardless of the path we take.");
                            player.SendGump(new DialogueGump(player, heartModule));
                        });
                    player.SendGump(new DialogueGump(player, regretModule));
                });

            greeting.AddOption("What is your stance on justice?",
                player => true,
                player =>
                {
                    DialogueModule justiceModule = new DialogueModule("Justice is often skewed in favor of the wealthy. My actions are a form of reclaiming that balance for the poor.");
                    justiceModule.AddOption("Do you think it's effective?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule effectivenessModule = new DialogueModule("It brings hope to those in despair and fear to those who exploit. Change takes time, but I believe in our cause.");
                            player.SendGump(new DialogueGump(player, effectivenessModule));
                        });
                    player.SendGump(new DialogueGump(player, justiceModule));
                });

            return greeting;
        }

        private bool HasAlreadySpoken(int entryNumber)
        {
            // Implement logic to check if NPC has already spoken the given entry number
            return false; // Placeholder logic
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
