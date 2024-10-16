using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of the Red Ranger")]
    public class RedRanger : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RedRanger() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Red Ranger";
            Body = 0x190; // Human male body

            // Stats
            SetStr(110);
            SetDex(110);
            SetInt(50);
            SetHits(85);

            // Appearance
            AddItem(new PlateLegs() { Hue = 37 });
            AddItem(new PlateChest() { Hue = 37 });
            AddItem(new PlateHelm() { Hue = 37 });
            AddItem(new PlateGloves() { Hue = 37 });
            AddItem(new Boots() { Hue = 37 });
            AddItem(new Broadsword() { Name = "Red Ranger's Sword" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am the Red Ranger, a guardian of the land. My journey has been filled with battles and lessons from my mentor, Zordon. How may I assist you today?");

            greeting.AddOption("Tell me about your time as leader of the Power Rangers.",
                player => true,
                player =>
                {
                    DialogueModule leaderModule = new DialogueModule("Leading the Power Rangers was both an honor and a challenge. Together, we faced many formidable foes, including Rita Repulsa and Lord Zedd. Each battle tested our strength and unity.");
                    leaderModule.AddOption("What was it like fighting alongside your team?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule teamModule = new DialogueModule("My team was like family. We supported one another through thick and thin. Each Ranger brought unique skills and perspectives, which made us stronger together. We had our share of disagreements, but our bond always prevailed.");
                            teamModule.AddOption("Can you share a specific battle story?",
                                p => true,
                                p =>
                                {
                                    DialogueModule storyModule = new DialogueModule("One of the most memorable battles was against the Green Ranger, who was initially our enemy. After a fierce fight, we learned that he was under a spell cast by Rita. With teamwork and courage, we helped him break free and join our cause.");
                                    storyModule.AddOption("That sounds intense! How did you feel during that battle?",
                                        plq => true,
                                        plq =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, storyModule));
                                });
                            teamModule.AddOption("What happened when you faced Lord Zedd?",
                                p => true,
                                p =>
                                {
                                    DialogueModule zeddModule = new DialogueModule("Lord Zedd was a fierce opponent. His dark magic was a constant threat. We had to strategize carefully to counter his power. I remember the day he unleashed the Zord-slaying monster. It took all of our teamwork to bring him down.");
                                    zeddModule.AddOption("How did you feel leading your team during that fight?",
                                        plw => true,
                                        plw =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, zeddModule));
                                });
                            pl.SendGump(new DialogueGump(pl, teamModule));
                        });
                    leaderModule.AddOption("What were your greatest challenges as a leader?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule challengesModule = new DialogueModule("The greatest challenge was always making decisions that affected my team. I had to ensure everyone was safe while also making the right tactical choices in battle. It was a heavy burden, but I learned to trust my instincts and my team.");
                            challengesModule.AddOption("Did you ever doubt your leadership?",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("Yes, there were moments of doubt, especially when the stakes were high. But I drew strength from my team and Zordon's teachings.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, challengesModule));
                        });
                    player.SendGump(new DialogueGump(player, leaderModule));
                });

            greeting.AddOption("What was it like learning under Zordon?",
                player => true,
                player =>
                {
                    DialogueModule zordonModule = new DialogueModule("Zordon was a wise and powerful mentor. He taught me the importance of leadership, courage, and compassion. Under his guidance, I learned that being a Ranger was not just about fighting; it was about protecting the innocent and being a beacon of hope.");
                    zordonModule.AddOption("Can you share some of Zordon's teachings?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule teachingsModule = new DialogueModule("Zordon often said, 'The power is not in the morphing grid but in the heart of a true leader.' He emphasized that strength must be balanced with wisdom. I carry his lessons with me to this day.");
                            teachingsModule.AddOption("What did you learn about teamwork?",
                                p => true,
                                p =>
                                {
                                    DialogueModule teamworkModule = new DialogueModule("Zordon taught me that a united team can overcome any obstacle. He encouraged us to rely on one another and to communicate openly. This bond is what allowed us to prevail in even the darkest times.");
                                    teamworkModule.AddOption("What was your favorite lesson?",
                                        ple => true,
                                        ple =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, teamworkModule));
                                });
                            pl.SendGump(new DialogueGump(pl, teachingsModule));
                        });
                    zordonModule.AddOption("What was Zordon like in person?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule zordonPersonalityModule = new DialogueModule("Zordon was a towering figure, both physically and metaphorically. His presence commanded respect, and his calm demeanor brought peace in chaos. He was patient and always willing to listen to our concerns.");
                            zordonPersonalityModule.AddOption("What did he think of your leadership?",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("Zordon believed in me, even when I doubted myself. He reminded me that true leaders grow through experience.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, zordonPersonalityModule));
                        });
                    player.SendGump(new DialogueGump(player, zordonModule));
                });

            greeting.AddOption("Do you have any rewards for brave souls?",
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
                        player.SendMessage("Ah, a brave soul! Here, take this. It is a small token of the forest's magic. Use it wisely.");
                        player.AddToBackpack(new MaxxiaScroll()); // Replace with the correct item
                        lastRewardTime = DateTime.UtcNow;
                    }
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            return greeting;
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

        public RedRanger(Serial serial) : base(serial) { }
    }
}
