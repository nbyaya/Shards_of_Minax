using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Gumps;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Remy the Rogue")]
    public class RemyTheRogue : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RemyTheRogue() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Remy the Rogue";
            Body = 0x190; // Human male body

            // Stats
            SetStr(60);
            SetDex(60);
            SetInt(60);
            SetHits(70);

            // Appearance
            AddItem(new LeatherArms() { Name = "Remy's Armguards" });
            AddItem(new LeatherLegs() { Name = "Remy's Leggings" });
            AddItem(new LeatherChest() { Name = "Remy's Vest" });
            AddItem(new LeatherGorget() { Name = "Remy's Collar" });
            AddItem(new LeatherCap() { Name = "Remy's Cap" });
            AddItem(new Boots() { Hue = 1904 });
            AddItem(new Dagger() { Name = "Remy's Dagger" });

            // Random hair and facial hair settings
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. They call me Remy the Rogue. I have a penchant for silver, you see. It's often less well guarded than gold. How may I assist you?");

            greeting.AddOption("What do you have against gold?",
                player => true,
                player =>
                {
                    DialogueModule goldModule = new DialogueModule("Gold is flashy and draws attention. Silver, on the other hand, is subtle and often overlooked. It allows a rogue like me to blend in more easily.");
                    goldModule.AddOption("So you prefer silver?",
                        p => true,
                        p =>
                        {
                            DialogueModule silverPreferenceModule = new DialogueModule("Indeed! Silver is not only valuable but also carries a certain charm. There's a beauty in its simplicity, don't you think?");
                            silverPreferenceModule.AddOption("What about its value?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule valueModule = new DialogueModule("Ah, while silver may not fetch as high a price as gold, it's still worth a fair amount. Besides, there are places where it can be found more abundantly, making it easier to acquire.");
                                    pl.SendGump(new DialogueGump(pl, valueModule));
                                });
                            silverPreferenceModule.AddOption("Where do you find silver?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule findModule = new DialogueModule("You can find silver in the most unexpected places. Hidden caches in merchant stalls, abandoned mines, or even beneath the floorboards of old taverns. The trick is knowing where to look!");
                                    findModule.AddOption("Have you found anything valuable recently?",
                                        pq => true,
                                        pq =>
                                        {
                                            DialogueModule recentFindsModule = new DialogueModule("Just the other night, I lifted a silver goblet from an unsuspecting noble's feast. He was too busy bragging about his gold to notice the glint of silver slipping away.");
                                            p.SendGump(new DialogueGump(p, recentFindsModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, findModule));
                                });
                            player.SendGump(new DialogueGump(player, silverPreferenceModule));
                        });
                    greeting.AddOption("What about other types of treasure?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule otherTreasuresModule = new DialogueModule("Ah, there are treasures beyond silver and gold. Gems can fetch a high price, but they require careful handling. If one is not cautious, they might attract unwanted attention!");
                            otherTreasuresModule.AddOption("What gems do you seek?",
                                p => true,
                                p =>
                                {
                                    DialogueModule gemsModule = new DialogueModule("I particularly favor sapphires and emeralds. Their colors catch the eye, and they're always in demand among collectors.");
                                    gemsModule.AddOption("Have you ever stolen a gem?",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule gemTheftModule = new DialogueModule("Once, I pilfered a rare sapphire from a merchant’s showcase. The look on his face was priceless! But I must admit, I had to escape quickly.");
                                            pl.SendGump(new DialogueGump(pl, gemTheftModule));
                                        });
                                    p.SendGump(new DialogueGump(p, gemsModule));
                                });
                            player.SendGump(new DialogueGump(player, otherTreasuresModule));
                        });
                    player.SendGump(new DialogueGump(player, goldModule));
                });

            greeting.AddOption("How did you become a rogue?",
                player => true,
                player =>
                {
                    DialogueModule backstoryModule = new DialogueModule("Ah, my life as a rogue began in the back alleys of the city. I was a street urchin, and survival often meant learning the art of stealth and thievery.");
                    backstoryModule.AddOption("What was your first theft?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule firstTheftModule = new DialogueModule("My first theft was a loaf of bread from a baker’s window. It was a desperate act, but it taught me valuable lessons about risk and reward.");
                            pl.SendGump(new DialogueGump(pl, firstTheftModule));
                        });
                    backstoryModule.AddOption("Did you have a mentor?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule mentorModule = new DialogueModule("Yes, an old thief named Garret took me under his wing. He taught me the tricks of the trade, like how to avoid detection and the importance of patience.");
                            mentorModule.AddOption("What did Garret teach you?",
                                p => true,
                                p =>
                                {
                                    DialogueModule teachingsModule = new DialogueModule("Garret emphasized the value of observation. A good thief knows their surroundings and their targets inside and out. Timing is everything.");
                                    p.SendGump(new DialogueGump(p, teachingsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, mentorModule));
                        });
                    player.SendGump(new DialogueGump(player, backstoryModule));
                });

            greeting.AddOption("Do you have any tales of your adventures?",
                player => true,
                player =>
                {
                    DialogueModule talesModule = new DialogueModule("Oh, I've had many thrilling adventures! There was one time I infiltrated a lord's mansion during a grand ball. The silverware was my target, but the lady of the house had my eye.");
                    talesModule.AddOption("What happened at the ball?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule ballModule = new DialogueModule("I managed to charm my way past the guards and blend in with the guests. I lifted a beautiful silver platter right off the table while everyone was distracted by the music.");
                            ballModule.AddOption("Did you escape without being caught?",
                                p => true,
                                p =>
                                {
                                    DialogueModule escapeModule = new DialogueModule("Just barely! As I was leaving, the lady noticed the missing platter and raised the alarm. I slipped into the shadows and made my way to safety, silver in hand!");
                                    p.SendGump(new DialogueGump(p, escapeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, ballModule));
                        });
                    player.SendGump(new DialogueGump(player, talesModule));
                });

            greeting.AddOption("Do you have any secrets to share?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendMessage("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        player.SendMessage("Not all secrets should be shared openly. But for you, I'll make an exception. Here's a small token for your journey.");
                        player.AddToBackpack(new MaxxiaScroll()); // Replace with the correct item class
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                });

            return greeting;
        }

        public RemyTheRogue(Serial serial) : base(serial) { }

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
