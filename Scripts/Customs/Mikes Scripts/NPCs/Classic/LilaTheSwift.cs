using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lila the Swift")]
    public class LilaTheSwift : BaseCreature
    {
        [Constructable]
        public LilaTheSwift() : base(AIType.AI_Archer, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lila the Swift";
            Body = 0x191; // Human female body
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Stats
            SetStr(100);
            SetDex(80);
            SetInt(80);
            SetHits(80);

            // Appearance
            AddItem(new LeatherChest() { Name = "Lila's Leather Tunic" });
            AddItem(new LeatherLegs() { Name = "Lila's Leather Leggings" });
            AddItem(new LeatherGloves() { Name = "Lila's Leather Gloves" });
            AddItem(new LeatherCap() { Name = "Lila's Leather Cap" });
            AddItem(new Boots() { Name = "Lila's Boots" });
            AddItem(new Bow() { Name = "Lila's Bow" });
        }

        public LilaTheSwift(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lila the Swift. What knowledge do you seek today?");
            
            greeting.AddOption("Tell me about yourself.",
                player => true,
                player =>
                {
                    DialogueModule aboutModule = new DialogueModule("I am an archer, skilled in the art of precision and swiftness. My life is dedicated to the mastery of the bow. What would you like to know?");
                    
                    aboutModule.AddOption("What is your history?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule historyModule = new DialogueModule("I hail from the distant forests of Yew, where the trees whisper ancient secrets. My father taught me the ways of the bow, and my mother, the wisdom of nature. Every arrow I shoot carries a piece of my family's legacy.");
                            historyModule.AddOption("What about your father?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("My father was a renowned archer, feared by foes and respected by allies. He taught me the importance of focus and the rhythm of the wind. I strive to honor his teachings with every shot.")));
                                });
                            historyModule.AddOption("Tell me about your mother.",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("My mother was a healer, using the herbs of the forest to mend wounds and soothe pain. Her wisdom taught me that strength lies not only in power but in compassion.")));
                                });
                            pl.SendGump(new DialogueGump(pl, historyModule));
                        });

                    aboutModule.AddOption("What drives your passion for archery?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule passionModule = new DialogueModule("Archery is an extension of my spirit. It requires patience, precision, and an understanding of the world around me. Each arrow is a connection between myself and the target, a dance of focus and skill.");
                            passionModule.AddOption("How do you improve your skills?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Practice, practice, practice! I often spend hours in the forest, honing my aim and learning to read the wind. The more I shoot, the better I understand my craft.")));
                                });
                            passionModule.AddOption("What techniques do you use?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("I utilize a technique known as 'breath control.' By regulating my breathing, I steady my aim and align my focus. It’s a skill that takes time to master but pays off with every shot.")));
                                });
                            pl.SendGump(new DialogueGump(pl, passionModule));
                        });

                    player.SendGump(new DialogueGump(player, aboutModule));
                });

            greeting.AddOption("What is valor?",
                player => true,
                player =>
                {
                    DialogueModule valorModule = new DialogueModule("Valor is not merely the absence of fear; it is the strength to face adversity despite that fear. It is about standing firm when the world tries to push you down. Do you consider yourself valorous?");
                    valorModule.AddOption("Yes, I stand for what is right.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("Then you understand the essence of valor. It is the heartbeat of every true hero.");
                        });
                    valorModule.AddOption("No, I often hesitate.",
                        pl => true,
                        pl =>
                        {
                            pl.SendMessage("Hesitation is natural, but remember that courage can be cultivated. Each step taken in the face of fear is a step toward valor.");
                        });
                    player.SendGump(new DialogueGump(player, valorModule));
                });

            greeting.AddOption("What do you know about archery?",
                player => true,
                player =>
                {
                    DialogueModule archeryModule = new DialogueModule("Being an archer is about more than just releasing the arrow. It’s about understanding the journey it takes, the wind’s whispers, and the target’s heart. Would you like to learn some techniques?");
                    archeryModule.AddOption("Yes, please teach me.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule techniqueModule = new DialogueModule("To become a skilled archer, one must learn to focus. Find a quiet spot, draw your bow, and aim at a stationary target. Focus on your breath, and let the arrow fly when you're ready.");
                            techniqueModule.AddOption("What else should I know?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Patience is key. The more you practice, the more natural it becomes. Learn to read the wind; it can change in an instant and affect your shot.")));
                                });
                            pl.SendGump(new DialogueGump(pl, techniqueModule));
                        });
                    archeryModule.AddOption("Not right now.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, archeryModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendMessage("Farewell, traveler. May the winds guide your path and the arrows find their mark.");
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
