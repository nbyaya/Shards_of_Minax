using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Gillian")]
    public class Gillian : BaseCreature
    {
        [Constructable]
        public Gillian() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Gillian";
            Body = 0x191; // Human female body

            // Stats
            SetStr(50);
            SetDex(70);
            SetInt(90);
            SetHits(50);

            // Appearance
            AddItem(new Skirt() { Hue = 1116 });
            AddItem(new FancyShirt() { Hue = 1114 });
            AddItem(new Boots() { Hue = 1157 });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x203C); // Random female hair
            HairHue = Utility.RandomHairHue();
        }

        public Gillian(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Gillian, a purveyor of mystical knowledge and rare artifacts. How may I assist you today?");

            greeting.AddOption("Tell me about your past.",
                player => true,
                player =>
                {
                    DialogueModule pastModule = new DialogueModule("Ah, my past is a tale of darkness and escape. I once lived in the cursed town of Tristram, which fell victim to the demon Diablo's rise. It was a harrowing time...");
                    pastModule.AddOption("What happened in Tristram?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule tristramModule = new DialogueModule("As Diablo emerged from the depths, chaos erupted. Shadows filled the streets, and the once vibrant town became a nightmarish realm of despair. I knew I had to escape.");
                            tristramModule.AddOption("How did you manage to escape?",
                                p => true,
                                p =>
                                {
                                    DialogueModule escapeModule = new DialogueModule("In my desperation, I stumbled upon a corrupted Town Portal scroll. It was tainted, filled with dark energies, but it was my only hope. I cast it without a second thought.");
                                    escapeModule.AddOption("What happened when you used it?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule portalModule = new DialogueModule("The moment I activated the scroll, a surge of energy enveloped me. I felt the pull of the arcane as reality twisted around me. It was unlike anything I've ever experienced.");
                                            portalModule.AddOption("Where did it take you?",
                                                pw => true,
                                                pw =>
                                                {
                                                    DialogueModule newWorldModule = new DialogueModule("Instead of returning to safety, I found myself in this strange world. It was both wondrous and frightening, filled with creatures and magic I had never known.");
                                                    newWorldModule.AddOption("What was your first impression of this world?",
                                                        pla => true,
                                                        pla =>
                                                        {
                                                            DialogueModule impressionModule = new DialogueModule("I was awestruck by the beauty of the landscapes, but it quickly faded when I encountered the dangers that lurked here. Every day is a struggle, but I am determined to survive.");
                                                            impressionModule.AddOption("What do you do to survive?",
                                                                ple => true,
                                                                ple =>
                                                                {
                                                                    DialogueModule survivalModule = new DialogueModule("I have taken to gathering herbs and crafting potions. Knowledge is my greatest weapon against the perils I face. I trade with travelers and learn about their worlds.");
                                                                    survivalModule.AddOption("What kind of potions do you make?",
                                                                        pr => true,
                                                                        pr =>
                                                                        {
                                                                            DialogueModule potionModule = new DialogueModule("I create potions that heal wounds, enhance strength, and even grant temporary agility. The ingredients are often hard to find, but they can be the difference between life and death.");
                                                                            potionModule.AddOption("Can you teach me about potion making?",
                                                                                plat => true,
                                                                                plat =>
                                                                                {
                                                                                    DialogueModule teachingModule = new DialogueModule("Of course! Alchemy is both art and science. Understanding the properties of each ingredient is crucial. What would you like to learn first?");
                                                                                    teachingModule.AddOption("How do I identify rare ingredients?",
                                                                                        py => true,
                                                                                        py =>
                                                                                        {
                                                                                            DialogueModule rareIngredientsModule = new DialogueModule("Rare ingredients often glow with a faint light or have unusual shapes. They are typically found in remote locations, guarded by powerful creatures or hidden within ancient ruins.");
                                                                                            rareIngredientsModule.AddOption("That sounds challenging.",
                                                                                                plu => true,
                                                                                                plu =>
                                                                                                {
                                                                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                                                });
                                                                                            p.SendGump(new DialogueGump(p, rareIngredientsModule));
                                                                                        });
                                                                                    teachingModule.AddOption("Tell me about potion effects.",
                                                                                        pi => true,
                                                                                        pi =>
                                                                                        {
                                                                                            DialogueModule effectsModule = new DialogueModule("Each potion has unique effects based on the ingredients used. For example, combining Nightshade with Crystal Dew can create a potion of invisibility.");
                                                                                            effectsModule.AddOption("What about dangerous potions?",
                                                                                                plo => true,
                                                                                                plo =>
                                                                                                {
                                                                                                    DialogueModule dangerousModule = new DialogueModule("Some potions can have side effects or even harmful reactions if not crafted correctly. It's essential to always test small quantities before using them in critical situations.");
                                                                                                    dangerousModule.AddOption("I'll be careful.",
                                                                                                        plap => true,
                                                                                                        plap =>
                                                                                                        {
                                                                                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                                                                        });
                                                                                                    p.SendGump(new DialogueGump(p, dangerousModule));
                                                                                                });
                                                                                            p.SendGump(new DialogueGump(p, effectsModule));
                                                                                        });
                                                                                    player.SendGump(new DialogueGump(player, teachingModule));
                                                                                });
                                                                            survivalModule.AddOption("I wish you luck in your crafting.",
                                                                                plaz => true,
                                                                                plaz =>
                                                                                {
                                                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                                });
                                                                            p.SendGump(new DialogueGump(p, potionModule));
                                                                        });
                                                                    survivalModule.AddOption("What dangers do you face?",
                                                                        pls => true,
                                                                        pls =>
                                                                        {
                                                                            DialogueModule dangerModule = new DialogueModule("Every day, I face wild creatures, bandits, and the remnants of Diablo's influence. It is a harsh world, but I must keep moving forward.");
                                                                            dangerModule.AddOption("What can I do to help?",
                                                                                pd => true,
                                                                                pd =>
                                                                                {
                                                                                    DialogueModule helpModule = new DialogueModule("Your willingness to help is commendable! Perhaps you could gather herbs or assist me in defending my little shop from marauding beasts.");
                                                                                    helpModule.AddOption("I can gather herbs.",
                                                                                        plaf => true,
                                                                                        plaf =>
                                                                                        {
                                                                                            pla.SendMessage("You have taken on a quest to gather herbs.");
                                                                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                                                        });
                                                                                    helpModule.AddOption("I can protect your shop.",
                                                                                        plag => true,
                                                                                        plag =>
                                                                                        {
                                                                                            pla.SendMessage("You have pledged to protect Gillian's shop.");
                                                                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                                                        });
                                                                                    p.SendGump(new DialogueGump(p, helpModule));
                                                                                });
                                                                            player.SendGump(new DialogueGump(player, dangerModule));
                                                                        });
                                                                    player.SendGump(new DialogueGump(player, survivalModule));
                                                                });
                                                            player.SendGump(new DialogueGump(player, impressionModule));
                                                        });
                                                    player.SendGump(new DialogueGump(player, newWorldModule));
                                                });
                                            player.SendGump(new DialogueGump(player, portalModule));
                                        });
                                    player.SendGump(new DialogueGump(player, escapeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, tristramModule));
                        });
                    player.SendGump(new DialogueGump(player, pastModule));
                });

            greeting.AddOption("What do you know of Diablo?",
                player => true,
                player =>
                {
                    DialogueModule diabloModule = new DialogueModule("Diablo, the Lord of Terror, is a formidable being. His return to Tristram brought suffering and chaos. The very earth trembled at his awakening.");
                    diabloModule.AddOption("How did he affect the town?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule effectModule = new DialogueModule("His influence twisted the hearts of many, turning friends against each other. Darkness seeped into the town, corrupting everything it touched.");
                            effectModule.AddOption("That's horrifying! How did you survive?",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, effectModule));
                        });
                    player.SendGump(new DialogueGump(player, diabloModule));
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
