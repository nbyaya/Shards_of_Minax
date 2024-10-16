using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mr. Lancealot")]
    public class MrLancealot : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MrLancealot() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mr. Lancealot";
            Body = 0x190; // Human male body

            // Stats
            Str = 60;
            Dex = 80;
            Int = 80;
            Hits = 30;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1153 });
            AddItem(new LeatherChest() { Hue = 1153 });
            AddItem(new LeatherGloves() { Hue = 1153 });
            AddItem(new LeatherCap() { Hue = 1153 });
            AddItem(new Boots() { Hue = 1153 });
            AddItem(new Lance() { Name = "Mr. Lancealot's Lance" });

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
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Mr. Lancealot, a scientist dedicated to unraveling the mysteries of the cosmos. How may I assist you today?");

            greeting.AddOption("Tell me more about yourself.",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Ah, where to begin? My passion lies in the study of celestial phenomena. I spend my days observing the stars and pondering the vastness of the universe. My research has led me to fascinating discoveries and peculiar anomalies.")));
                });

            greeting.AddOption("What is the greatest question you ponder?",
                player => true,
                player =>
                {
                    DialogueModule questionModule = new DialogueModule("Ah, a curious mind! The greatest question I ponder is: 'What lies beyond the known universe?' It's a thought that ignites my imagination. What do you think?");
                    questionModule.AddOption("I believe there are infinite possibilities.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! The idea of infinite possibilities is both exhilarating and daunting. Each choice we make could lead us to different realms of existence.")));
                        });
                    questionModule.AddOption("Perhaps it's all a dream.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("A dream, you say? Perhaps reality is merely a veil obscuring deeper truths. What do you dream about?")));
                        });
                    questionModule.AddOption("Why bother pondering at all?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, but the act of pondering is what sets us apart! It is through inquiry that we grow, learn, and evolve. Every question leads to a new path.")));
                        });
                    player.SendGump(new DialogueGump(player, questionModule));
                });

            greeting.AddOption("What do you know about the universe?",
                player => true,
                player =>
                {
                    DialogueModule universeModule = new DialogueModule("The universe is an unfathomable expanse, teeming with mysteries. I've observed distant stars and the peculiar behavior of celestial bodies. Would you like to know about a specific phenomenon?");
                    universeModule.AddOption("Tell me about black holes.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, black holes! They are regions of space where gravity is so strong that nothing can escape their pull. Their presence warps time and space. Fascinating, isn't it?")));
                        });
                    universeModule.AddOption("What about stars?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Stars are the lifeblood of the universe. They forge elements in their cores and scatter them across the cosmos when they explode. Each star has a story to tell, and I seek to uncover them all.")));
                        });
                    universeModule.AddOption("Do you study planets?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! Each planet holds secrets of its own. I study their atmospheres, climates, and potential for life. It is my hope to find worlds similar to our own.")));
                        });
                    player.SendGump(new DialogueGump(player, universeModule));
                });

            greeting.AddOption("What is this anomaly you mentioned?",
                player => true,
                player =>
                {
                    DialogueModule anomalyModule = new DialogueModule("The anomaly is a rift in the fabric of space, leading to an alternate dimension. I believe it holds hidden truths and treasures waiting to be uncovered. Would you care to explore this rift with me?");
                    anomalyModule.AddOption("Yes, I would love to explore!",
                        pl => true,
                        pl => {
                            pl.SendMessage("Fantastic! But to explore, I need a rare crystal found only in the treacherous caves of Gloomshade. Will you retrieve it for me?");
                            pl.SendGump(new DialogueGump(pl, CreateQuestModule()));
                        });
                    anomalyModule.AddOption("No, that sounds too dangerous.",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I understand. The unknown can be frightening. But the rewards of discovery are worth the risk. Let me know if you change your mind.")));
                        });
                    player.SendGump(new DialogueGump(player, anomalyModule));
                });

            greeting.AddOption("Tell me about Gloomshade.",
                player => true,
                player =>
                {
                    DialogueModule gloomshadeModule = new DialogueModule("Gloomshade is a cave system to the east, infamous for the dangers lurking within. Many brave adventurers have entered, but few return. However, the crystals inside are said to possess powerful energy.");
                    gloomshadeModule.AddOption("What kind of dangers?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("The caves are home to treacherous beasts and perilous traps. It is said that only the bravest and most cunning can navigate its depths. You must tread carefully!")));
                        });
                    gloomshadeModule.AddOption("What is the crystal's power?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("The crystal has the potential to enhance our understanding of dimensional travel. It could unlock the secrets of the rift and even grant us access to realms beyond imagination.")));
                        });
                    player.SendGump(new DialogueGump(player, gloomshadeModule));
                });

            greeting.AddOption("What can you tell me about rewards?",
                player => true,
                player => {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendMessage("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        player.SendMessage("Ah, rewards! If you bring me the crystal, I shall grant you knowledge that has been lost to time and perhaps even something to aid you on your journey.");
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            greeting.AddOption("What is knowledge to you?",
                player => true,
                player =>
                {
                    DialogueModule knowledgeModule = new DialogueModule("Knowledge is the greatest treasure of all. It is a light that guides us through the darkness. With knowledge, we can unlock the secrets of the universe and change our destinies.");
                    knowledgeModule.AddOption("How can I gain knowledge?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("To gain knowledge, one must seek it out. Read, explore, ask questions, and never cease to be curious. The world is filled with teachers if you know where to look.")));
                        });
                    knowledgeModule.AddOption("What if I forget what I learn?",
                        pl => true,
                        pl => {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, memory can be fickle. Keeping a journal can help. Write down your discoveries and thoughts. It will be a treasure for your future self.")));
                        });
                    player.SendGump(new DialogueGump(player, knowledgeModule));
                });

            return greeting;
        }

        private DialogueModule CreateQuestModule()
        {
            DialogueModule questModule = new DialogueModule("To explore the rift, I require a rare crystal from Gloomshade. Bring it to me, and the rewards will be great!");

            questModule.AddOption("I accept the quest!",
                player => true,
                player => {
                    player.SendMessage("Brave adventurer! Go forth and return with the crystal. Your name shall be remembered in the annals of science.");
                });

            questModule.AddOption("I need more time to think.",
                player => true,
                player => {
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            return questModule;
        }

        public MrLancealot(Serial serial) : base(serial) { }

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
