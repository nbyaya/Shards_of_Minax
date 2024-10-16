using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Junny")]
    public class Junny : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Junny() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jun";
            Body = 0x191; // Human male body

            // Stats
            SetStr(100);
            SetDex(90);
            SetInt(60);
            SetHits(80);

            // Appearance
            AddItem(new FemaleLeatherChest() { Hue = 1337 });
            AddItem(new Cloak() { Hue = 1337 });
            AddItem(new ThighBoots() { Hue = 1337 });
            AddItem(new Dagger() { Name = "Junny's Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            lastRewardTime = DateTime.MinValue; // Initialize lastRewardTime
        }

        public Junny(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Junny, the Exile. What would you like to discuss?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I've faced countless battles in Wraeclast, and while my body is scarred, my spirit remains unbroken. What else do you wish to know?")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My life is a never-ending battle for survival. I fend for myself in the wilds, often crafting weapons and potions from the materials I find. What intrigues you about my life?");
                    jobModule.AddOption("How do you survive?",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Survival in Wraeclast requires cunning and resourcefulness. I hunt, gather, and trade with others to sustain myself. It’s a constant struggle, but it keeps me sharp.")));
                        });
                    jobModule.AddOption("Do you have any allies?",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Allies are scarce in Wraeclast, but I have a few trusted companions. We look out for each other, though trust is a rare commodity here.")));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What can you tell me about battles?",
                player => true,
                player =>
                {
                    DialogueModule battlesModule = new DialogueModule("Survival in Wraeclast requires many virtues. What virtues guide you?");
                    battlesModule.AddOption("Tell me about your virtues.",
                        p => true,
                        p =>
                        {
                            DialogueModule virtuesModule = new DialogueModule("Virtues like honor, sacrifice, and courage are my guiding lights. They remind me to fight for something greater than myself. Which virtue resonates with you?");
                            virtuesModule.AddOption("I value honor.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Honor is paramount. It gives meaning to our struggles and guides our actions in the heat of battle.")));
                                });
                            virtuesModule.AddOption("Courage is what I cherish.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Courage is essential, especially when facing the unknown. It empowers us to take risks.")));
                                });
                            virtuesModule.AddOption("Sacrifice is the path I choose.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Sacrifice can lead to greater rewards. Sometimes, one must give up something valuable for the greater good.")));
                                });
                            p.SendGump(new DialogueGump(p, virtuesModule));
                        });
                    battlesModule.AddOption("What do you think of Wraeclast?",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, new DialogueModule("Wraeclast is a land of dark magic and dangerous creatures. Every day is a test of one's will and might. What stories have you heard about it?")));
                        });
                    player.SendGump(new DialogueGump(player, battlesModule));
                });

            greeting.AddOption("Do you have any crafting secrets?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendGump(new DialogueGump(player, new DialogueModule("I have no reward right now. Please return later.")));
                    }
                    else
                    {
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                        player.AddToBackpack(new MaxxiaScroll()); // Replace with actual reward item
                        player.SendGump(new DialogueGump(player, new DialogueModule("Crafting is not just a skill but an art. I’ve given you a crafted item as a reward.")));
                    }
                });

            greeting.AddOption("What about courage?",
                player => true,
                player =>
                {
                    DialogueModule courageModule = new DialogueModule("Courage is not just about facing danger; it's also about standing up for what is right. In Wraeclast, trust is a rare commodity. Do you have someone you trust?");
                    courageModule.AddOption("I have a trusted companion.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("That's good to hear. In this land, having someone at your back can mean the difference between life and death.")));
                        });
                    courageModule.AddOption("I stand alone.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Standing alone can be a heavy burden. Remember, even the strongest need allies sometimes.")));
                        });
                    player.SendGump(new DialogueGump(player, courageModule));
                });

            greeting.AddOption("What about magic in Wraeclast?",
                player => true,
                player =>
                {
                    DialogueModule magicModule = new DialogueModule("The magic in Wraeclast is ancient and powerful. It's not just about casting spells, but understanding the very fabric of reality. What do you think of magic?");
                    magicModule.AddOption("Magic is a double-edged sword.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed, those who misuse it often pay a heavy price. Balance is key.")));
                        });
                    magicModule.AddOption("I seek magical knowledge.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Knowledge is the greatest treasure. Seek out wise sages and ancient tomes to deepen your understanding.")));
                        });
                    player.SendGump(new DialogueGump(player, magicModule));
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
    }
}
