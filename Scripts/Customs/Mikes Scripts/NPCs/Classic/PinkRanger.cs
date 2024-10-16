using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Pink Ranger")]
    public class PinkRanger : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PinkRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pink Ranger";
            Body = 0x191; // Human female body

            // Stats
            SetStr(100);
            SetDex(110);
            SetInt(65);
            SetHits(75);

            // Appearance
            AddItem(new StuddedLegs() { Hue = 24 });
            AddItem(new StuddedChest() { Hue = 24 });
            AddItem(new ChainCoif() { Hue = 24 });
            AddItem(new StuddedGloves() { Hue = 24 });
            AddItem(new Boots() { Hue = 24 });
            AddItem(new Bow() { Name = "Pink Ranger's Bow" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public PinkRanger(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am the Pink Ranger, a guardian of Compassion! How can I assist you today?");

            greeting.AddOption("Tell me about your time as a Power Ranger.",
                player => true,
                player =>
                {
                    DialogueModule rangerModule = new DialogueModule("Ah, my time as a Power Ranger was filled with adventure and friendship! We faced countless villains and learned the true meaning of teamwork. Would you like to hear about a specific mission or perhaps a memorable moment?");
                    rangerModule.AddOption("Tell me about your first battle.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateResponseModule("My first battle was against Rita Repulsa! I was so nervous, but my teammates supported me. Together, we fought valiantly and learned to trust one another.")));
                        });
                    rangerModule.AddOption("What was the most difficult challenge you faced?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateResponseModule("The most difficult challenge was when Lord Zedd captured my friends. I had to summon all my strength and courage to save them, and it was during that time I truly understood the power of friendship.")));
                        });
                    rangerModule.AddOption("Did you ever face the Green Ranger?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule greenRangerModule = new DialogueModule("Ah, Tommy. He started as our enemy, but I always felt there was more to him. Even when he was evil, I sensed a kind heart deep inside. Would you like to hear more about that time?");
                            greenRangerModule.AddOption("Yes, tell me about Tommy.",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule tommyModule = new DialogueModule("Tommy was originally the Green Ranger, sent to defeat us. But even while he was under Rita's control, I felt an inexplicable connection to him. It was complicated, to say the least.");
                                    tommyModule.AddOption("Did you have a crush on him?",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule crushModule = new DialogueModule("Yes, I did! It was so confusing because he was our enemy. But I couldn’t help but admire his strength and determination. When he turned good, it felt like a dream come true!");
                                            crushModule.AddOption("What was it like when he became good?",
                                                ple => true,
                                                ple =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, CreateResponseModule("When Tommy broke free from Rita’s spell, it was like a weight lifted off my heart. We became close friends, and I admired him even more for choosing the path of righteousness.")));
                                                });
                                            crushModule.AddOption("Did you ever tell him?",
                                                plr => true,
                                                plr =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, CreateResponseModule("I wanted to! But with all the battles and challenges, it was hard to find the right moment. Sometimes I think he knew, though. Our bond grew stronger as teammates.")));
                                                });
                                            pl.SendGump(new DialogueGump(pl, crushModule));
                                        });
                                    tommyModule.AddOption("How did the other Rangers feel about him?",
                                        plt => true,
                                        plt =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateResponseModule("At first, they were cautious. But as Tommy proved himself, they welcomed him into the team. He quickly became an invaluable member of our family.")));
                                        });
                                    pl.SendGump(new DialogueGump(pl, tommyModule));
                                });
                            greenRangerModule.AddOption("What was the turning point for Tommy?",
                                ply => true,
                                ply =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateResponseModule("The turning point was when he chose to help us against Rita, risking his own safety. It was a brave act, and from that moment, he truly became one of us.")));
                                });
                            pl.SendGump(new DialogueGump(pl, greenRangerModule));
                        });
                    player.SendGump(new DialogueGump(player, rangerModule));
                });

            greeting.AddOption("How are you?",
                player => true,
                player =>
            {
                player.SendGump(new DialogueGump(player, CreateResponseModule("I am in perfect health, as Compassion heals all wounds.")));
            });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, CreateResponseModule("I protect the innocent and spread Compassion throughout the land. I also help others discover their inner strength.")));
                });

            greeting.AddOption("What was your favorite mission?",
                player => true,
                player =>
                {
                    DialogueModule missionModule = new DialogueModule("Oh, there are so many! But one of my favorites was when we saved a town from a giant monster. We worked together seamlessly, each using our unique abilities. It showed me the power of teamwork.");
                    missionModule.AddOption("What was the monster like?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateResponseModule("The monster was huge and terrifying, but we stood our ground. With strategy and teamwork, we managed to bring it down and save the day!")));
                        });
                    missionModule.AddOption("Did you get injured?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateResponseModule("I took a few hits, but nothing too serious. We always looked out for each other, and our bond kept us strong.")));
                        });
                    player.SendGump(new DialogueGump(player, missionModule));
                });

            greeting.AddOption("Can you give me a reward?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendGump(new DialogueGump(player, CreateResponseModule("I have no reward right now. Please return later.")));
                    }
                    else
                    {
                        player.SendGump(new DialogueGump(player, CreateResponseModule("Feelings are a guiding force. Here's a reward to help you on your way.")));
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                });

            return greeting;
        }

        private DialogueModule CreateResponseModule(string response)
        {
            return new DialogueModule(response);
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
