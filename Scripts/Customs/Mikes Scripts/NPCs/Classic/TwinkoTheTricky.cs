using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Twinko the Tricky")]
    public class TwinkoTheTricky : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TwinkoTheTricky() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Twinko the Tricky";
            Body = 0x190; // Human male body
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Stats
            Str = 85;
            Dex = 70;
            Int = 90;
            Hits = 88;

            // Appearance
            AddItem(new JesterHat() { Hue = 1157 });
            AddItem(new JesterSuit() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1167 });

            SpeechHue = 0; // Default speech hue

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
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Twinko the Tricky, the jester of these lands! Would you like to hear a story?");

            greeting.AddOption("Tell me a story.",
                player => true,
                player => {
                    DialogueModule storyModule = new DialogueModule("Ah, let me regale you with the tale of how I tricked a notorious bandit into giving up all his gold to charity! Would you like to hear it?");
                    storyModule.AddOption("Yes, please!",
                        pl => true,
                        pl => {
                            DialogueModule banditStory = new DialogueModule("It all began one fateful day when I encountered a bandit named Rocco, known for his ruthlessness. He was terrorizing the villagers and taking their hard-earned coin.");
                            banditStory.AddOption("What did you do?",
                                p => true,
                                p => {
                                    DialogueModule actionModule = new DialogueModule("I approached Rocco while he was counting his ill-gotten gains in a dark alley. I donned my best jester outfit and began to juggle! He was intrigued.");
                                    actionModule.AddOption("Did he laugh?",
                                        plq => true,
                                        plq => {
                                            DialogueModule laughModule = new DialogueModule("At first, he frowned, but I made a few clever jokes about his appearance. Eventually, he chuckled, and I saw an opportunity!");
                                            laughModule.AddOption("What did you say?",
                                                pw => true,
                                                pw => {
                                                    player.SendGump(new DialogueGump(player, new DialogueModule("I told him, 'With a face like that, you could scare off a dragon! Perhaps you should consider a mask!' This caught his attention!")));
                                                });
                                            actionModule.AddOption("What was your next move?",
                                                ple => true,
                                                ple => {
                                                    DialogueModule moveModule = new DialogueModule("I then challenged him to a game of wits. If I could make him laugh three times, he would have to donate a portion of his loot to the local charity.");
                                                    moveModule.AddOption("And did he accept the challenge?",
                                                        pr => true,
                                                        pr => {
                                                            player.SendGump(new DialogueGump(player, new DialogueModule("To my surprise, he agreed! He boasted about his cleverness, and I sensed his pride would be his downfall.")));
                                                        });
                                                    player.SendGump(new DialogueGump(player, moveModule));
                                                });
                                            player.SendGump(new DialogueGump(player, laughModule));
                                        });
                                    player.SendGump(new DialogueGump(player, actionModule));
                                });

                            storyModule.AddOption("What happened next?",
                                plt => true,
                                plt => {
                                    DialogueModule nextModule = new DialogueModule("As we played our game, I made jokes about bandits in general. 'Why don't bandits ever play hide and seek? Because good luck hiding when everyone knows you're a thief!' He roared with laughter!");
                                    nextModule.AddOption("Did he lose control?",
                                        p => true,
                                        p => {
                                            player.SendGump(new DialogueGump(player, new DialogueModule("Yes! Rocco laughed so hard that he lost track of our wager! I reminded him about the charity, and he was too distracted to back out!")));
                                        });
                                    player.SendGump(new DialogueGump(player, nextModule));
                                });

                            player.SendGump(new DialogueGump(player, banditStory));
                        });
                    storyModule.AddOption("Tell me more about the bandit.",
                        pl => true,
                        pl => {
                            player.SendGump(new DialogueGump(player, new DialogueModule("Rocco was a notorious figure in the region, feared by many. They say he once stole the crown jewels and hid them in a secret lair!")));
                        });
                    player.SendGump(new DialogueGump(player, storyModule));
                });

            greeting.AddOption("Do you have any other stories?",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I have countless tales! But tell me, what type of story would you like to hear next? A tale of adventure, mischief, or perhaps romance?")));
                });

            greeting.AddOption("What else do you do?",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Apart from jesting, I travel, perform, and sometimes lend a hand to those in need! Laughter is a powerful tool, don't you think?")));
                });

            greeting.AddOption("What about that jester's hat?",
                player => true,
                player => {
                    if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                    }
                    else
                    {
                        player.AddToBackpack(new JesterHatOfCommand());
                        lastRewardTime = DateTime.UtcNow;
                        player.SendGump(new DialogueGump(player, new DialogueModule("Ah! You’re interested in the jester's hat? Here, take this special one as a token of our conversation!")));
                    }
                });

            return greeting;
        }

        public TwinkoTheTricky(Serial serial) : base(serial) { }

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
