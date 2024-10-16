using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zoe Porphyrogenita")]
    public class ZoePorphyrogenita : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ZoePorphyrogenita() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zoe Porphyrogenita";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new FancyDress() { Hue = 1281 }); // Golden dress
            AddItem(new GoldNecklace() { Name = "Zoe Porphyrogenita's Necklace" });
            AddItem(new Boots() { Hue = 1281 }); // Matching boots

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            lastRewardTime = DateTime.MinValue;
            SpeechHue = 0; // Default speech hue
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Zoe Porphyrogenita. How may I assist you?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player =>
                {
                    DialogueModule healthModule = new DialogueModule("I am in good health, thank you for asking. Daily meditations and herbal remedies keep me well.");
                    healthModule.AddOption("What kind of herbal remedies?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I often use Lavender for relaxation and Sage for clarity. They are quite potent.")));
                        });
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("I am a scholar of Byzantium, dedicated to the pursuit of knowledge. My studies encompass history, philosophy, and ancient artifacts.");
                    jobModule.AddOption("What have you discovered?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I have uncovered many secrets, including lost texts that detail the philosophies of ancient scholars.")));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What are virtues?",
                player => true,
                player =>
                {
                    DialogueModule virtuesModule = new DialogueModule("Honor, compassion, and justice are virtues I hold dear. They are essential in guiding one's path.");
                    virtuesModule.AddOption("How do virtues guide you?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Virtues remind me of my duties and responsibilities. They help me remain steadfast even in turbulent times.")));
                        });
                    player.SendGump(new DialogueGump(player, virtuesModule));
                });

            greeting.AddOption("Tell me about your lineage.",
                player => true,
                player =>
                {
                    DialogueModule lineageModule = new DialogueModule("Our family traces its roots back to the very halls of Byzantium, descended from noble scholars.");
                    lineageModule.AddOption("What does Porphyrogenita mean?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("It means 'born into the purple,' signifying a lineage of royalty and nobility.")));
                        });
                    player.SendGump(new DialogueGump(player, lineageModule));
                });

            greeting.AddOption("Do you have any tokens of wisdom?",
                player => CanReceiveToken(player),
                player =>
                {
                    if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                    }
                    else
                    {
                        lastRewardTime = DateTime.UtcNow;
                        player.AddToBackpack(new MaxxiaScroll());
                        player.SendGump(new DialogueGump(player, new DialogueModule("Take this amulet; it’s a replica of an ancient Byzantine artifact. May it aid you on your journey.")));
                    }
                });

            greeting.AddOption("What do you think about Byzantium?",
                player => true,
                player =>
                {
                    DialogueModule byzantiumModule = new DialogueModule("Ah, Byzantium! The heart of an ancient empire, filled with secrets and history. Its beauty is unmatched.");
                    byzantiumModule.AddOption("What is its greatest secret?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("The greatest secret of Byzantium is the knowledge hidden in its ruins. Many scholars seek these truths.")));
                        });
                    player.SendGump(new DialogueGump(player, byzantiumModule));
                });

            greeting.AddOption("Can you teach me about meditations?",
                player => true,
                player =>
                {
                    DialogueModule meditationModule = new DialogueModule("Meditations connect us to the ancient energies of the world. Would you like to learn a basic chant?");
                    meditationModule.AddOption("Yes, please teach me.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("The chant goes like this: 'Abyssus abyssum invocat'. Meditate on these words and feel the energies flow.")));
                        });
                    meditationModule.AddOption("Maybe another time.",
                        pl => true,
                        pl => 
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Of course. Just remember, the path to enlightenment is always open.")));
                        });
                    player.SendGump(new DialogueGump(player, meditationModule));
                });

            greeting.AddOption("What artifacts have you studied?",
                player => true,
                player =>
                {
                    DialogueModule artifactsModule = new DialogueModule("The artifacts from Byzantium hold not just history, but also magic. Each tells a story of its own.");
                    artifactsModule.AddOption("Can you show me an artifact?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I can describe one: the Golden Eagle of Byzantium, a symbol of power and protection.")));
                        });
                    artifactsModule.AddOption("What is the most dangerous artifact?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("The cursed Chalice of Fate is said to bring ruin to those who possess it without wisdom.")));
                        });
                    player.SendGump(new DialogueGump(player, artifactsModule));
                });

            return greeting;
        }

        private bool CanReceiveToken(PlayerMobile player)
        {
            return DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10);
        }

        public ZoePorphyrogenita(Serial serial) : base(serial) { }

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
