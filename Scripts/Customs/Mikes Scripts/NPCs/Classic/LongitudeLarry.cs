using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Longitude Larry")]
    public class LongitudeLarry : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LongitudeLarry() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Longitude Larry";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 55;
            Int = 115;
            Hits = 75;

            // Appearance
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Tunic() { Hue = 1103 });
            AddItem(new Shoes() { Hue = 45 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
            DialogueModule greeting = new DialogueModule("I am Longitude Larry, the world's most renowned cartographer. But what do you care?");

            greeting.AddOption("Tell me about your job.",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My job? I'm a cartographer, mapping this wretched world for fools like you who can't find their way!");
                    jobModule.AddOption("What does a cartographer do?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule cartographyModule = new DialogueModule("A cartographer collects data from explorations and creates detailed maps to guide adventurers. It's more art than science, you know.");
                            cartographyModule.AddOption("What kind of maps do you create?",
                                p => true,
                                p =>
                                {
                                    DialogueModule mapTypesModule = new DialogueModule("I specialize in various maps: topographical, political, and even treasure maps! Each serves a unique purpose for those brave enough to venture into the unknown.");
                                    mapTypesModule.AddOption("Do you have a treasure map?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule treasureMapModule = new DialogueModule("Ah, treasure maps! They hold the secrets to hidden riches. Unfortunately, I don't have any at the moment, but keep an eye out for them!");
                                            pl.SendGump(new DialogueGump(pl, treasureMapModule));
                                        });
                                    p.SendGump(new DialogueGump(p, mapTypesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, cartographyModule));
                        });
                    jobModule.AddOption("What about battles?",
                        p => true,
                        p =>
                        {
                            DialogueModule battlesModule = new DialogueModule("Oh, you think you're valiant, do you? Let's see if you can even comprehend what true valor means.");
                            battlesModule.AddOption("Tell me about valor.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule valorModule = new DialogueModule("Valor is not simply the absence of fear; it's the courage to face the unknown, even when the odds are against you.");
                                    valorModule.AddOption("Have you shown valor?",
                                        p2 => true,
                                        p2 =>
                                        {
                                            DialogueModule myValorModule = new DialogueModule("Indeed! I once braved the treacherous Ice Peaks to recover lost maps from a ruined outpost. Many perished there, but I returned with knowledge that changed the world!");
                                            p2.SendGump(new DialogueGump(p2, myValorModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, valorModule));
                                });
                            p.SendGump(new DialogueGump(p, battlesModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What hardships have you faced?",
                player => true,
                player =>
                {
                    DialogueModule hardshipsModule = new DialogueModule("Ha! I've traversed treacherous terrains, crossed mountains, and braved deserts. All to create the perfect map! You think it's easy? Have you faced any battles yourself?");
                    hardshipsModule.AddOption("What was the hardest journey?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule hardestJourneyModule = new DialogueModule("The hardest was undoubtedly my expedition to the Abyssal Depths. The darkness is palpable, and the creatures lurking there are more terrifying than any beast in this realm.");
                            hardestJourneyModule.AddOption("Did you encounter any monsters?",
                                p => true,
                                p =>
                                {
                                    DialogueModule monsterModule = new DialogueModule("Indeed! I encountered shadow wraiths and cave trolls. They are vicious and relentless, but I managed to escape with my life and some valuable knowledge.");
                                    p.SendGump(new DialogueGump(p, monsterModule));
                                });
                            pl.SendGump(new DialogueGump(pl, hardestJourneyModule));
                        });
                    hardshipsModule.AddOption("Tell me about mapping.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule mappingModule = new DialogueModule("Mapping isn't just about charting unknown lands. It's about giving adventurers a beacon of hope, a guide through darkness. But I doubt you understand the intricacies of my work. Ever tried your hand at mapmaking?");
                            mappingModule.AddOption("No, but I’d like to learn.",
                                p => true,
                                p =>
                                {
                                    DialogueModule learnModule = new DialogueModule("Mapmaking requires precision and an eye for detail. You must understand the geography and the stories behind each landmark. Would you like to hear about specific techniques?");
                                    learnModule.AddOption("Yes, please tell me!",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule techniquesModule = new DialogueModule("You must master triangulation for accurate positioning, and understanding weather patterns is vital to predict changes in terrain. Patience is key in this art.");
                                            pl.SendGump(new DialogueGump(pl, techniquesModule));
                                        });
                                    p.SendGump(new DialogueGump(p, learnModule));
                                });
                            pl.SendGump(new DialogueGump(pl, mappingModule));
                        });
                    player.SendGump(new DialogueGump(player, hardshipsModule));
                });

            greeting.AddOption("Do you have a reward for adventurers?",
                player => true,
                player =>
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        SayToPlayer(player, "I have no reward right now. Please return later.");
                    }
                    else
                    {
                        SayToPlayer(player, "Ah, the spirit of an adventurer! Always seeking, always exploring. Here, take this map. It might help you on your journey.");
                        player.AddToBackpack(new FocusAugmentCrystal()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            return greeting;
        }

        private void SayToPlayer(Mobile player, string message)
        {
            player.SendMessage(message);
        }

        private bool HasPreviousSpeech(int entryNumber)
        {
            // Placeholder for previous speech logic; implement as needed
            return true;
        }

        public LongitudeLarry(Serial serial) : base(serial) { }

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
