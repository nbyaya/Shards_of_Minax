using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Shepherdess Anna")]
    public class ShepherdessAnna : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ShepherdessAnna() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Shepherdess Anna";
            Body = 0x191; // Human female body

            // Stats
            Str = 78;
            Dex = 58;
            Int = 52;
            Hits = 68;

            // Appearance
            AddItem(new PlainDress() { Hue = 1133 });
            AddItem(new Boots() { Hue = 1152 });
            AddItem(new ShepherdsCrook() { Name = "Shepherdess Anna's Crook" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Shepherdess Anna. Lately, I've been pondering many things, especially about my role in this land and the choices I make.");

            greeting.AddOption("What do you mean?",
                player => true,
                player => 
                {
                    DialogueModule detailModule = new DialogueModule("I've been struggling with the idea of becoming a vegan. You see, I care for my flock deeply, yet I also feel a connection to the creatures around me.");
                    detailModule.AddOption("Why do you want to become a vegan?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule reasonModule = new DialogueModule("I've been learning about the impact of animal agriculture on the land and the environment. It troubles me to think about the suffering it causes. I want to do what's right.");
                            reasonModule.AddOption("That's understandable. Have you considered the challenges?",
                                p => true,
                                p => 
                                {
                                    DialogueModule challengeModule = new DialogueModule("Yes, there are many challenges. What would happen to my sheep? They rely on me. The thought of losing them is unbearable.");
                                    challengeModule.AddOption("Could you find a way to keep them?",
                                        plq => true,
                                        plq => 
                                        {
                                            DialogueModule keepModule = new DialogueModule("Perhaps. If I could transition to a lifestyle that focuses on plants and perhaps find other ways to care for my sheep, it might work.");
                                            keepModule.AddOption("That sounds like a plan. Have you spoken to anyone about it?",
                                                pw => true,
                                                pw => 
                                                {
                                                    DialogueModule talkModule = new DialogueModule("Not yet. I'm afraid of what others might think. They might see me as foolish for wanting to change something that has been my way of life for so long.");
                                                    talkModule.AddOption("Your feelings are valid. Change can be hard.",
                                                        ple => true,
                                                        ple => 
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, detailModule));
                                                        });
                                                    talkModule.AddOption("Maybe they will understand if you explain your thoughts.",
                                                        plr => true,
                                                        plr => 
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, keepModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, talkModule));
                                                });
                                            p.SendGump(new DialogueGump(p, challengeModule));
                                        });
                                    challengeModule.AddOption("Perhaps you should seek help from others.",
                                        plt => true,
                                        plt => 
                                        {
                                            DialogueModule helpModule = new DialogueModule("That's a good idea. There are many who have taken similar journeys. Perhaps they could share their experiences.");
                                            helpModule.AddOption("It’s important to have support.",
                                                py => true,
                                                py => 
                                                {
                                                    p.SendGump(new DialogueGump(p, detailModule));
                                                });
                                            p.SendGump(new DialogueGump(p, helpModule));
                                        });
                                    p.SendGump(new DialogueGump(p, reasonModule));
                                });
                            reasonModule.AddOption("How do you feel about your sheep?",
                                plu => true,
                                plu => 
                                {
                                    DialogueModule sheepModule = new DialogueModule("I adore them. They are not just livestock; they are companions. I often wonder if they sense my conflict.");
                                    sheepModule.AddOption("They might. Animals are perceptive.",
                                        p => true,
                                        p => 
                                        {
                                            sheepModule.AddOption("It's true. I’ve seen them react when I’m upset. It makes me feel even more responsible for their well-being.",
                                                pli => true,
                                                pli => 
                                                {
                                                    pl.SendGump(new DialogueGump(pl, sheepModule));
                                                });
                                            p.SendGump(new DialogueGump(p, sheepModule));
                                        });
                                    plu.SendGump(new DialogueGump(plu, sheepModule));
                                });
                            pl.SendGump(new DialogueGump(pl, reasonModule));
                        });
                    player.SendGump(new DialogueGump(player, detailModule));
                });

            greeting.AddOption("Have you thought about your diet?",
                player => true,
                player => 
                {
                    DialogueModule dietModule = new DialogueModule("Yes! I know I can grow many vegetables and fruits, but I worry about the nutrients I might miss. What if I can't get enough protein?");
                    dietModule.AddOption("There are many plant-based sources of protein.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule sourcesModule = new DialogueModule("That's true! Lentils, beans, and even nuts can provide what I need. It’s a whole new world to explore.");
                            sourcesModule.AddOption("Exactly! It could be a wonderful journey of discovery.",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, dietModule));
                                });
                            dietModule.AddOption("What about the taste of meat?",
                                plz => true,
                                plz => 
                                {
                                    DialogueModule tasteModule = new DialogueModule("I do enjoy the flavors of meat, but I wonder if I can find substitutes that could satisfy my cravings.");
                                    tasteModule.AddOption("There are many delicious plant-based recipes out there.",
                                        p => true,
                                        p => 
                                        {
                                            tasteModule.AddOption("I’d love to explore them. Maybe cooking could become a new hobby for me!",
                                                plx => true,
                                                plx => 
                                                {
                                                    pl.SendGump(new DialogueGump(pl, tasteModule));
                                                });
                                            p.SendGump(new DialogueGump(p, tasteModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, tasteModule));
                                });
                            pl.SendGump(new DialogueGump(pl, sourcesModule));
                        });
                    player.SendGump(new DialogueGump(player, dietModule));
                });

            greeting.AddOption("What do you think about the ethics of eating meat?",
                player => true,
                player => 
                {
                    DialogueModule ethicsModule = new DialogueModule("It’s a heavy topic for me. I want to honor the lives of the animals I care for, yet survival often feels like a primal instinct.");
                    ethicsModule.AddOption("That’s a common struggle for many.",
                        pl => true,
                        pl => 
                        {
                            DialogueModule commonStruggleModule = new DialogueModule("Yes, I see that now. I must reconcile my needs with my values.");
                            commonStruggleModule.AddOption("Finding balance is key.",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, commonStruggleModule));
                                });
                            ethicsModule.AddOption("Maybe you could find a middle ground.",
                                plb => true,
                                plb => 
                                {
                                    pl.SendGump(new DialogueGump(pl, commonStruggleModule));
                                });
                            player.SendGump(new DialogueGump(player, ethicsModule));
                        });
                    player.SendGump(new DialogueGump(player, ethicsModule));
                });

            return greeting;
        }

        public ShepherdessAnna(Serial serial) : base(serial) { }

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
