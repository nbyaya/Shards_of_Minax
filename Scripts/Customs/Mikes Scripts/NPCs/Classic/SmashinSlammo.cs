using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Smashin' Slammo")]
    public class SmashinSlammo : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SmashinSlammo() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Smashin' Slammo";
            Body = 0x190; // Human male body

            // Stats
            Str = 150;
            Hits = 100;

            // Appearance
            AddItem(new Cloak() { Hue = 1153 });
            AddItem(new ThighBoots() { Hue = 1153 });
            AddItem(new LeatherGloves() { Hue = 1153 });
            AddItem(new TribalMask() { Hue = 1153 });
            AddItem(new LongPants() { Hue = 1153 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public SmashinSlammo(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I'm Smashin' Slammo, the wrestling sensation! Have you heard about the Interplanar Wrestling Championship at the Planar metropolis of Lim?");

            greeting.AddOption("Tell me more about the championship.",
                player => true,
                player => CreateChampionshipModule());

            greeting.AddOption("What is your health like?",
                player => true,
                player => Say("I'm in top shape, ready for action!"));

            greeting.AddOption("What is your job?",
                player => true,
                player => Say("My job is to entertain the crowd with my wrestling moves!"));

            greeting.AddOption("What about your battles?",
                player => true,
                player => Say("Do you have what it takes to step into the ring with me?"));

            greeting.AddOption("Do you have any rewards for me?",
                player => true,
                player => HandleReward(player));

            return greeting;
        }

        private DialogueModule CreateChampionshipModule()
        {
            DialogueModule championship = new DialogueModule("The Interplanar Wrestling Championship is the ultimate test of strength and skill, where wrestlers from different realms compete for glory. It takes place in Lim, a vibrant city full of challengers!");

            championship.AddOption("What makes Lim special?",
                player => true,
                player => 
                {
                    Say("Lim is a metropolis that exists between dimensions, known for its floating arenas and magical energy that enhances physical prowess.");
                    player.SendGump(new DialogueGump(player, championship));
                });

            championship.AddOption("Who are the notable competitors?",
                player => true,
                player => 
                {
                    DialogueModule competitorsModule = new DialogueModule("Competitors like Raging Rick, the Iron Beast, and the Shadow Matron are infamous for their techniques. Each brings unique skills to the ring!");

                    competitorsModule.AddOption("Tell me about Raging Rick.",
                        pl => true,
                        pl => 
                        {
                            Say("Raging Rick is a fierce competitor known for his brute strength and unpredictable moves. He’s a two-time champion!");
                            pl.SendGump(new DialogueGump(pl, competitorsModule));
                        });

                    competitorsModule.AddOption("What about the Iron Beast?",
                        pl => true,
                        pl => 
                        {
                            Say("The Iron Beast is a massive creature that relies on sheer power and toughness. Many challengers have crumbled under his weight!");
                            pl.SendGump(new DialogueGump(pl, competitorsModule));
                        });

                    competitorsModule.AddOption("And the Shadow Matron?",
                        pl => true,
                        pl => 
                        {
                            Say("The Shadow Matron is a mysterious figure who uses illusion and deception to outwit her opponents. Facing her is a test of wits as much as strength!");
                            pl.SendGump(new DialogueGump(pl, competitorsModule));
                        });

                    player.SendGump(new DialogueGump(player, competitorsModule));
                });

            championship.AddOption("What do I need to compete?",
                player => true,
                player => 
                {
                    DialogueModule requirementsModule = new DialogueModule("To compete, you must be strong and skilled. You'll also need a sponsor from Lim, and a special amulet to channel the arena's magic.");

                    requirementsModule.AddOption("How do I get a sponsor?",
                        pl => true,
                        pl => 
                        {
                            Say("You can impress a local champion or merchant with your skills. Show them you have what it takes to compete at the highest level!");
                            pl.SendGump(new DialogueGump(pl, requirementsModule));
                        });

                    requirementsModule.AddOption("What about the amulet?",
                        pl => true,
                        pl => 
                        {
                            Say("The amulet is crafted from rare materials found only in Lim. You might need to venture into the nearby realms to gather them!");
                            pl.SendGump(new DialogueGump(pl, requirementsModule));
                        });

                    player.SendGump(new DialogueGump(player, requirementsModule));
                });

            championship.AddOption("What happens if I win?",
                player => true,
                player => 
                {
                    Say("Winning grants you the title of Champion and a chance to compete in the Grand Tournament of the Planes, plus a hefty reward!");
                    player.SendGump(new DialogueGump(player, championship));
                });

            return championship;
        }

        private void HandleReward(Mobile from)
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                Say("I have no reward right now. Please return later.");
            }
            else
            {
                Say("Alright, show me your best wrestling stance. Ah, not bad! Here, take this as a token of my appreciation!");
                from.AddToBackpack(new MaxxiaScroll()); // Replace with the actual item type and name
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }
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
