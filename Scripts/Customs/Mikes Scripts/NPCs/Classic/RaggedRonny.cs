using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ragged Ronny")]
    public class RaggedRonny : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RaggedRonny() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ragged Ronny";
            Body = 0x190; // Human male body

            // Stats
            Str = 38;
            Dex = 29;
            Int = 20;
            Hits = 43;

            // Appearance
            AddItem(new Kilt() { Hue = 52 });
            AddItem(new Shirt() { Hue = 1102 });
            AddItem(new ThighBoots() { Hue = 1153 });

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
            DialogueModule greeting = new DialogueModule("I am Ragged Ronny, a beggar by trade. Life on the streets, friend, it's a constant battle. How may I help you?");

            greeting.AddOption("What do you think about virtue?",
                player => true,
                player =>
                {
                    DialogueModule virtueModule = new DialogueModule("Compassion, friend, that's the virtue I hold dear. It's the light that guides us through the darkest alleys.");
                    virtueModule.AddOption("Why is compassion important?",
                        p => true,
                        pl =>
                        {
                            DialogueModule compassionModule = new DialogueModule("Compassion is not just a virtue but a way of life. It's the reason I share what little I have with others on the streets.");
                            compassionModule.AddOption("That's insightful.",
                                p => true,
                                pla => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                            pl.SendGump(new DialogueGump(pl, compassionModule));
                        });
                    player.SendGump(new DialogueGump(player, virtueModule));
                });

            greeting.AddOption("Tell me about the streets.",
                player => true,
                player =>
                {
                    DialogueModule streetsModule = new DialogueModule("The streets have taught me more about life than any book. Every cobblestone has a story if you're willing to listen.");
                    streetsModule.AddOption("What kind of stories?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule storiesModule = new DialogueModule("Each cobblestone has felt the weight of both the pauper and the king. They've witnessed acts of kindness and moments of cruelty.");
                            storiesModule.AddOption("It sounds like a tough life.",
                                p => true,
                                plw => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                            pl.SendGump(new DialogueGump(pl, storiesModule));
                        });
                    player.SendGump(new DialogueGump(player, streetsModule));
                });

            greeting.AddOption("Do you have any rewards for me?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        Say("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        Say("For your thoughtful inquiry, please accept this reward.");
                        player.AddToBackpack(new Gold(100)); // Example reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            greeting.AddOption("Tell me about your adventures.",
                player => true,
                player =>
                {
                    DialogueModule adventuresModule = new DialogueModule("Ah, there are tales aplenty! Let me share one of my favorites about the time I found a gold ring in the trash.");
                    adventuresModule.AddOption("What happened with the ring?",
                        p => true,
                        pl =>
                        {
                            DialogueModule ringModule = new DialogueModule("I was rummaging through some refuse near the market when I stumbled upon a glimmer of gold. At first, I thought it was just a scrap of metal, but upon closer inspection, it was a ring!");
                            ringModule.AddOption("Did you keep it?",
                                p => true,
                                ple =>
                                {
                                    DialogueModule keepRingModule = new DialogueModule("Of course! It was beautifully crafted, with a small emerald set in the center. I felt like a king wearing it. But I knew I couldn’t keep it for long. It belonged to someone.");
                                    keepRingModule.AddOption("What did you do with it?",
                                        p => true,
                                        plr =>
                                        {
                                            DialogueModule returnRingModule = new DialogueModule("I decided to return it to the market, hoping its owner would be searching for it. A woman approached me, tears in her eyes. She recognized it immediately! It turned out it was a family heirloom.");
                                            returnRingModule.AddOption("That's kind of you!",
                                                plt => true,
                                                pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                                            pl.SendGump(new DialogueGump(pl, returnRingModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, keepRingModule));
                                });
                            ringModule.AddOption("What did you do afterwards?",
                                p => true,
                                ply =>
                                {
                                    DialogueModule afterRingModule = new DialogueModule("After returning the ring, I felt a sense of fulfillment. To celebrate, I decided to treat myself to a week of fine meats at the inn.");
                                    afterRingModule.AddOption("Fine meats? Tell me more!",
                                        p => true,
                                        plu =>
                                        {
                                            DialogueModule fineMeatsModule = new DialogueModule("Oh, the meats were exquisite! Roast duck, tender beef, and even a leg of lamb. I had never tasted such luxury before!");
                                            fineMeatsModule.AddOption("Did you eat well every day?",
                                                p => true,
                                                pli =>
                                                {
                                                    DialogueModule dailyMealsModule = new DialogueModule("Every day was a feast! I would sit by the fire, savoring each bite, and I could even afford a mug of their finest ale. The innkeeper treated me like a king!");
                                                    dailyMealsModule.AddOption("What was your favorite dish?",
                                                        p => true,
                                                        plo =>
                                                        {
                                                            DialogueModule favoriteDishModule = new DialogueModule("The roast duck was my favorite! It was crispy on the outside and succulent on the inside. They served it with a berry sauce that made my mouth water just thinking about it!");
                                                            favoriteDishModule.AddOption("I'd love to try that!",
                                                                plp => true,
                                                                pla => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                                                            pl.SendGump(new DialogueGump(pl, favoriteDishModule));
                                                        });
                                                    pl.SendGump(new DialogueGump(pl, dailyMealsModule));
                                                });
                                            fineMeatsModule.AddOption("Did you share your meals?",
                                                p => true,
                                                pla =>
                                                {
                                                    DialogueModule sharingModule = new DialogueModule("I did! I shared my meals with fellow travelers and other beggars who stopped by. It felt wonderful to spread the joy.");
                                                    sharingModule.AddOption("That's very generous of you!",
                                                        pls => true,
                                                        plax => { pla.SendGump(new DialogueGump(pla, CreateGreetingModule())); });
                                                    pl.SendGump(new DialogueGump(pl, sharingModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, fineMeatsModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, afterRingModule));
                                });
                            pl.SendGump(new DialogueGump(pl, ringModule));
                        });
                    player.SendGump(new DialogueGump(player, adventuresModule));
                });

            greeting.AddOption("What can you tell me about kindness?",
                player => true,
                player =>
                {
                    DialogueModule kindnessModule = new DialogueModule("Kindness, even a simple act, can change someone's day. A coin, a loaf of bread, or just a listening ear can make a world of difference.");
                    kindnessModule.AddOption("I agree with you.",
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, kindnessModule));
                });

            greeting.AddOption("Tell me more about your life.",
                player => true,
                player =>
                {
                    DialogueModule lifeModule = new DialogueModule("To live is not just to exist. It's to experience, to share, and to give. Even in my situation, I find moments of joy and purpose.");
                    lifeModule.AddOption("That's a good perspective.",
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            return greeting;
        }

        public RaggedRonny(Serial serial) : base(serial) { }

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
