using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Wirt")]
    public class Wirt : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Wirt() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Wirt";
            Body = 0x190; // Human male body
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize stats and appearance
            SetStr(40);
            SetDex(60);
            SetInt(50);
            SetHits(30);

            AddItem(new ShortPants() { Hue = 1110 });
            AddItem(new Tunic() { Hue = 1125 });
            AddItem(new Boots() { Hue = 0 });
            AddItem(new Club() { Name = "Wirt's Peg Leg" });

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
            DialogueModule greeting = new DialogueModule("I'm Wirt, the one-legged wonder. What do you want?");

            greeting.AddOption("Tell me about your time in Tristram.",
                player => true,
                player => 
                {
                    DialogueModule tristramModule = new DialogueModule("Ah, Tristram! A town bustling with adventurers and magic. I sold everything from enchanted swords to mysterious potions there.");
                    tristramModule.AddOption("What was the most magical item you sold?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule itemModule = new DialogueModule("One time, I managed to sell a staff said to be imbued with the essence of a fallen star! The price was steep, but the buyer was desperate.");
                            itemModule.AddOption("What happened to the staff?",
                                p => true,
                                p => 
                                {
                                    DialogueModule fateModule = new DialogueModule("The buyer claimed it was stolen shortly after. I often wonder if it was truly magical or just a pretty stick. Either way, it was a memorable sale!");
                                    p.SendGump(new DialogueGump(p, fateModule));
                                });
                            pl.SendGump(new DialogueGump(pl, itemModule));
                        });
                    tristramModule.AddOption("Did you have any interesting customers?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule customersModule = new DialogueModule("Oh, plenty! There was a wizard who kept insisting that my potions were cursed. I had to convince him otherwise. In the end, he bought a whole crate!");
                            customersModule.AddOption("What did he want the potions for?",
                                p => true,
                                p => 
                                {
                                    DialogueModule reasonModule = new DialogueModule("He claimed they were for an experiment to enhance his spells. I never did find out how that turned out.");
                                    p.SendGump(new DialogueGump(p, reasonModule));
                                });
                            pl.SendGump(new DialogueGump(pl, customersModule));
                        });
                    greeting.AddOption("What else do you remember about Tristram?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule memoriesModule = new DialogueModule("The festival of magic was a highlight! People showcased their magical talents, and I had a stall selling my wares. The atmosphere was electrifying.");
                            memoriesModule.AddOption("Did anything go wrong during the festival?",
                                p => true,
                                p => 
                                {
                                    DialogueModule mishapModule = new DialogueModule("Oh, you wouldn't believe it! One magician accidentally set his robes on fire while performing a fire spell! It turned into a hilarious chaos.");
                                    p.SendGump(new DialogueGump(p, mishapModule));
                                });
                            pl.SendGump(new DialogueGump(pl, memoriesModule));
                        });
                    player.SendGump(new DialogueGump(player, tristramModule));
                });

            greeting.AddOption("What happened to your leg?",
                player => true,
                player => 
                {
                    DialogueModule legModule = new DialogueModule("Ah, a painful memory. I lost my leg in a cave while chasing a particularly elusive artifact.");
                    legModule.AddOption("What artifact were you chasing?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule artifactModule = new DialogueModule("It was a gemstone, said to hold the power of healing. I thought it could be a fortune in Tristram. Alas, greed led me to danger.");
                            artifactModule.AddOption("What kind of danger?",
                                p => true,
                                p => 
                                {
                                    DialogueModule dangerModule = new DialogueModule("There were traps set by the ancient guardians of the cave. I misstepped and triggered one. It was a painful lesson about greed!");
                                    dangerModule.AddOption("Did you manage to get the gemstone?",
                                        pla => true,
                                        pla => 
                                        {
                                            DialogueModule outcomeModule = new DialogueModule("I did, but it cost me dearly. I barely escaped, and my leg paid the price. The gemstone? Lost to time.");
                                            pl.SendGump(new DialogueGump(pl, outcomeModule));
                                        });
                                    p.SendGump(new DialogueGump(p, dangerModule));
                                });
                            pl.SendGump(new DialogueGump(pl, artifactModule));
                        });
                    player.SendGump(new DialogueGump(player, legModule));
                });

            greeting.AddOption("Do you sell magic items now?",
                player => true,
                player => 
                {
                    DialogueModule shopModule = new DialogueModule("I still have a few rare items left! You won't find better prices anywhere else.");
                    shopModule.AddOption("Let me see what you have.",
                        p => true,
                        p => 
                        {
                            // Open the player's buy/sell gump or shop interface
                            p.SendMessage("Wirt shows you his collection of magic items.");
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    shopModule.AddOption("Maybe later.",
                        p => true,
                        p => 
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, shopModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player => 
                {
                    player.SendMessage("Wirt waves you off.");
                });

            return greeting;
        }

        public Wirt(Serial serial) : base(serial) { }

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
