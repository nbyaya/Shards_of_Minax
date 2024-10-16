using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Lethal")]
    public class LadyLethal : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyLethal() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Lethal";
            Body = 0x191; // Human female body

            // Stats
            Str = 110;
            Dex = 95;
            Int = 55;
            Hits = 75;

            // Appearance
            AddItem(new BoneLegs() { Hue = 2118 });
            AddItem(new BoneChest() { Hue = 2118 });
            AddItem(new BoneArms() { Hue = 2118 });
            AddItem(new BoneHelm() { Hue = 2118 });
            AddItem(new Boots() { Hue = 2118 });
            AddItem(new DoubleAxe() { Name = "Lady Lethal's Axe" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("I am Lady Lethal, the assassin extraordinaire! What brings you to my domain?");

            greeting.AddOption("Tell me about your job.",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My job involves eliminating those who annoy me with the utmost discretion. It's a craft, an art form, you could say.");
                    jobModule.AddOption("What makes it an art?",
                        p => true,
                        p =>
                        {
                            DialogueModule artModule = new DialogueModule("Every strike is a brushstroke, every stealthy approach is a dance. It's all about elegance and precision.");
                            artModule.AddOption("Can anyone learn this art?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule learnModule = new DialogueModule("Not everyone has the temperament for it. You must be cold-hearted, patient, and skilled in deception.");
                                    learnModule.AddOption("I believe I have those qualities.",
                                        pq => true,
                                        pq => 
                                        {
                                            p.SendGump(new DialogueGump(p, CreateJoinModule()));
                                        });
                                    learnModule.AddOption("I’m not sure I could be that ruthless.",
                                        pw => true,
                                        pw => 
                                        {
                                            p.SendGump(new DialogueGump(p, CreateHesitationModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, learnModule));
                                });
                            p.SendGump(new DialogueGump(p, artModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What do you think of your profession?",
                player => true,
                player =>
                {
                    DialogueModule professionModule = new DialogueModule("It’s a dangerous life, filled with risks and rewards. It takes a keen mind and sharper instincts.");
                    professionModule.AddOption("What risks do you face?",
                        p => true,
                        p =>
                        {
                            DialogueModule riskModule = new DialogueModule("There are many: rivals, the law, and sometimes even those you thought were allies.");
                            riskModule.AddOption("Have you faced many rivals?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule rivalsModule = new DialogueModule("Oh, many. Some have been crafty, others surprisingly foolish. Each encounter teaches a lesson.");
                                    rivalsModule.AddOption("What lesson did you learn from your biggest rival?",
                                        pe => true,
                                        pe => 
                                        {
                                            p.SendGump(new DialogueGump(p, CreateRivalLessonModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, rivalsModule));
                                });
                            p.SendGump(new DialogueGump(p, riskModule));
                        });
                    player.SendGump(new DialogueGump(player, professionModule));
                });

            greeting.AddOption("What is the medallion of Lord Valorian?",
                player => true,
                player =>
                {
                    DialogueModule medallionModule = new DialogueModule("The medallion symbolizes power and legacy, held by Lord Valorian himself. It will not be easy to acquire.");
                    medallionModule.AddOption("What can you tell me about Lord Valorian?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule lordModule = new DialogueModule("He is a formidable adversary, known for his cunning and strength. Many have tried to take the medallion from him, but few have succeeded.");
                            lordModule.AddOption("What is his weakness?",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, CreateValorianWeaknessModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, lordModule));
                        });
                    player.SendGump(new DialogueGump(player, medallionModule));
                });

            greeting.AddOption("Do you have any rewards?",
                player => true,
                player =>
                {
                    if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                    {
                        player.SendMessage("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        player.AddToBackpack(new NinjitsuAugmentCrystal());
                        lastRewardTime = DateTime.UtcNow;
                        player.SendMessage("You received a Ninjitsu Augment Crystal as a reward. Use it wisely.");
                    }
                });

            return greeting;
        }

        private DialogueModule CreateJoinModule()
        {
            DialogueModule joinModule = new DialogueModule("If you wish to prove yourself, you must retrieve the medallion of Lord Valorian. Do you accept this challenge?");
            joinModule.AddOption("I accept the challenge.",
                pl => true,
                pl => 
                {
                    pl.SendMessage("You embark on the quest to retrieve the medallion. Be cautious, for he is not an easy target.");
                });
            joinModule.AddOption("I need more time to think.",
                pl => true,
                pl => 
                {
                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                });
            return joinModule;
        }

        private DialogueModule CreateHesitationModule()
        {
            DialogueModule hesitationModule = new DialogueModule("It's wise to be cautious. The path of an assassin is not for the faint-hearted. Consider your choices carefully.");
            hesitationModule.AddOption("Perhaps I should learn more before deciding.",
                pl => true,
                pl => 
                {
                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                });
            return hesitationModule;
        }

        private DialogueModule CreateRivalLessonModule()
        {
            DialogueModule lessonModule = new DialogueModule("From my biggest rival, I learned the importance of never underestimating your opponent. Always expect the unexpected.");
            lessonModule.AddOption("That’s valuable advice. I’ll remember it.",
                pl => true,
                pl => 
                {
                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                });
            return lessonModule;
        }

        private DialogueModule CreateValorianWeaknessModule()
        {
            DialogueModule weaknessModule = new DialogueModule("Lord Valorian has a weakness for his family. Use that to your advantage, but be careful; he’s unpredictable.");
            weaknessModule.AddOption("Thank you for the insight. I’ll be cautious.",
                pl => true,
                pl => 
                {
                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                });
            return weaknessModule;
        }

        public LadyLethal(Serial serial) : base(serial) { }

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
