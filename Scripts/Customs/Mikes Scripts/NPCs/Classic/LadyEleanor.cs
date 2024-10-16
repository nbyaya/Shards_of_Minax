using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Eleanor")]
    public class LadyEleanor : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyEleanor() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Eleanor";
            Body = 0x191; // Human female body

            // Stats
            Str = 150;
            Dex = 62;
            Int = 23;
            Hits = 105;

            // Appearance
            AddItem(new Kilt() { Hue = 1200 });
            AddItem(new ChainChest() { Hue = 1200 });
            AddItem(new PlateHelm() { Hue = 1200 });
            AddItem(new PlateGloves() { Hue = 1200 });
            AddItem(new Boots() { Hue = 1200 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Eleanor of Aquitaine, once a queen, now a prisoner of this wretched realm. How may I assist you?");
            
            greeting.AddOption("Tell me about your health.",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Society has cast me aside, treating me like a relic of the past. My health? It withers just as my spirit does.")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My 'job'? It was once to rule a kingdom, to lead armies, to shape history. Now, it is to languish in this forsaken place.")));
                });

            greeting.AddOption("Tell me about your past.",
                player => true,
                player =>
                {
                    DialogueModule pastModule = new DialogueModule("Once, I stood by kings and influenced the course of nations. A queen's crown is heavy, laden with responsibilities and expectations.");
                    pastModule.AddOption("What was your kingdom like?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule kingdomModule = new DialogueModule("My kingdom was a tapestry of culture and power. The streets bustled with merchants, scholars debated in the halls, and the fields were rich with harvests. But politics is a treacherous game.");
                            kingdomModule.AddOption("What were your responsibilities?",
                                pl2 => true,
                                pl2 => 
                                {
                                    pl2.SendGump(new DialogueGump(pl2, new DialogueModule("As a queen, I was tasked with ensuring the prosperity of my realm. From diplomatic negotiations to overseeing the well-being of my subjects, every decision carried weight.")));
                                });
                            kingdomModule.AddOption("Did you face many challenges?",
                                pl2 => true,
                                pl2 =>
                                {
                                    pl2.SendGump(new DialogueGump(pl2, new DialogueModule("Ah, challenges were aplenty! Intrigue lurked in every corner. I had to navigate betrayals, manage alliances, and maintain the delicate balance of power.")));
                                });
                            pl.SendGump(new DialogueGump(pl, kingdomModule));
                        });
                    player.SendGump(new DialogueGump(player, pastModule));
                });

            greeting.AddOption("Can you tell me about your family?",
                player => true,
                player =>
                {
                    DialogueModule familyModule = new DialogueModule("My family was both my strength and my sorrow. I had children, but the crown weighed heavy on us all. Their futures became entangled in the web of politics.");
                    familyModule.AddOption("What happened to them?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Alas, the fates were unkind. Some were lost in battles, others fell victim to plots. Their absence is a wound that will never heal.")));
                        });
                    familyModule.AddOption("Did you have a husband?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("I was wed to a powerful king, whose ambition sometimes overshadowed my own. Love was complicated by duty, and we often found ourselves at odds.")));
                        });
                    player.SendGump(new DialogueGump(player, familyModule));
                });

            greeting.AddOption("Can you reward me?",
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
                        player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                        player.SendGump(new DialogueGump(player, new DialogueModule("My treasures were many. Aid me, and one shall be yours. A sample for you.")));
                    }
                });

            greeting.AddOption("What are your thoughts on society?",
                player => true,
                player =>
                {
                    DialogueModule societyModule = new DialogueModule("Ah, society. A double-edged sword. It offers companionship but often chains us with expectations and norms. Are you a prisoner of your own society?");
                    societyModule.AddOption("Perhaps I am.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Many are trapped by the whims of fate or society's chains. Yet, I wonder, is there a way to break free?")));
                        });
                    societyModule.AddOption("I strive to be free.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Bravery is commendable. To seek one's path amidst the chaos is a noble endeavor. Perhaps you can teach others the same.")));
                        });
                    player.SendGump(new DialogueGump(player, societyModule));
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

        public LadyEleanor(Serial serial) : base(serial) { }
    }
}
