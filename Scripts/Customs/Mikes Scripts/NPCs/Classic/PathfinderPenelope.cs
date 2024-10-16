using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Pathfinder Penelope")]
    public class PathfinderPenelope : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PathfinderPenelope() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pathfinder Penelope";
            Body = 0x191; // Human female body

            // Stats
            Str = 103;
            Dex = 54;
            Int = 119;
            Hits = 71;

            // Appearance
            AddItem(new FancyDress() { Hue = 44 }); // Clothing item with hue 44
            AddItem(new Boots() { Hue = 1122 }); // Boots with hue 1122

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Pathfinder Penelope, the cartographer. Have you heard of the legendary lost city of Wind?");

            greeting.AddOption("Tell me about the lost city of Wind.",
                player => true,
                player =>
                {
                    DialogueModule windCityModule = new DialogueModule("Ah, the city of Wind! It is said to be a place of unparalleled beauty, filled with ancient knowledge and magic. Many have searched for it, but few have returned with stories to tell. Do you seek to find it?");
                    windCityModule.AddOption("Yes, I wish to find it!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule searchModule = new DialogueModule("Your spirit is admirable! The journey is perilous, but I can share what I know. The path to the city appears during certain celestial events. Legends speak of shimmering lights in the sky guiding the way.");
                            searchModule.AddOption("What celestial events are you talking about?",
                                p => true,
                                p =>
                                {
                                    DialogueModule celestialModule = new DialogueModule("There are tales of two moons aligning and a comet streaking across the sky. During those nights, one might find a glimmering path leading towards the city. However, timing is everything!");
                                    celestialModule.AddOption("How can I prepare for such an event?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule preparationModule = new DialogueModule("Preparation is key. Bring with you enchanted maps, rare crystals, and a heart open to the wonders of the world. A spirit willing to listen to the winds is essential. Would you like guidance on gathering these items?");
                                            preparationModule.AddOption("Yes, please tell me more.",
                                                pw => true,
                                                pw =>
                                                {
                                                    DialogueModule itemsModule = new DialogueModule("You'll need a map infused with the essence of the stars, a vial of Essence of Wind, and a Heartstone. These items resonate with the city's magic. I've seen such items in the ruins of the Windwalkers.");
                                                    itemsModule.AddOption("Where can I find the Windwalkers' ruins?",
                                                        ple => true,
                                                        ple =>
                                                        {
                                                            DialogueModule ruinsModule = new DialogueModule("The ruins lie in the Whispering Hills, surrounded by ancient trees and mystical barriers. Be wary, for the guardians of the ruins are not easily persuaded. But fear not! Their challenges are but tests for the worthy.");
                                                            ruinsModule.AddOption("I will seek the ruins.",
                                                                pr => true,
                                                                pr =>
                                                                {
                                                                    p.SendMessage("You set off toward the Whispering Hills, determined to uncover the secrets of the Windwalkers.");
                                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                                });
                                                            itemsModule.AddOption("What challenges should I expect?",
                                                                plt => true,
                                                                plt =>
                                                                {
                                                                    DialogueModule challengesModule = new DialogueModule("You may encounter illusions that test your resolve and riddles that challenge your mind. Remember, not all is as it seems. Trust your instincts and remain steadfast in your purpose.");
                                                                    challengesModule.AddOption("I will remember your advice.",
                                                                        pla => true,
                                                                        pla =>
                                                                        {
                                                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                                        });
                                                                    pl.SendGump(new DialogueGump(pl, challengesModule));
                                                                });
                                                            player.SendGump(new DialogueGump(player, ruinsModule));
                                                        });
                                                    itemsModule.AddOption("What about the Essence of Wind?",
                                                        ply => true,
                                                        ply =>
                                                        {
                                                            DialogueModule essenceModule = new DialogueModule("The Essence of Wind can be harvested from the wisps that roam the Gale Forest during twilight. Approach them with respect, and they may share their essence with you.");
                                                            essenceModule.AddOption("Sounds like an adventure!",
                                                                pu => true,
                                                                pu =>
                                                                {
                                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                                });
                                                            player.SendGump(new DialogueGump(player, essenceModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, preparationModule));
                                                });
                                            preparationModule.AddOption("Maybe another time.",
                                                pli => true,
                                                pli =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            player.SendGump(new DialogueGump(player, preparationModule));
                                        });
                                    p.SendGump(new DialogueGump(p, celestialModule));
                                });
                            windCityModule.AddOption("What if I can't find it?",
                                plo => true,
                                plo =>
                                {
                                    DialogueModule doubtModule = new DialogueModule("Many have tried and failed. The city is elusive, appearing only to those deemed worthy. But remember, the journey itself holds valuable lessons, even if the destination remains a dream.");
                                    doubtModule.AddOption("What lessons can I learn?",
                                        p => true,
                                        p =>
                                        {
                                            DialogueModule lessonsModule = new DialogueModule("Every journey teaches patience, resilience, and the importance of hope. Often, those who seek do not find but instead discover themselves along the way.");
                                            lessonsModule.AddOption("I understand. Thank you.",
                                                pla => true,
                                                pla =>
                                                {
                                                    pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                                });
                                            p.SendGump(new DialogueGump(p, lessonsModule));
                                        });
                                    player.SendGump(new DialogueGump(player, doubtModule));
                                });
                            player.SendGump(new DialogueGump(player, searchModule));
                        });
                    windCityModule.AddOption("No, I have other matters to attend to.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, windCityModule));
                });

            greeting.AddOption("What is your role as a cartographer?",
                player => true,
                player =>
                {
                    DialogueModule roleModule = new DialogueModule("As a cartographer, I not only draw maps but also study the lands, their secrets, and their stories. Each expedition brings me closer to understanding the world’s mysteries.");
                    roleModule.AddOption("What is the most interesting place you've mapped?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule interestingModule = new DialogueModule("The most fascinating place? That would be the Ruins of Eldoria. Ancient structures built by a forgotten civilization, infused with magic. I often feel their spirits guiding my hand as I sketch.");
                            interestingModule.AddOption("I would love to see Eldoria!",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, interestingModule));
                        });
                    roleModule.AddOption("Do you have any mapping techniques?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule techniquesModule = new DialogueModule("Yes! I use a combination of traditional methods and magical tools. The most useful is the Compass of True North, which always points to places of great significance.");
                            techniquesModule.AddOption("Where can I find this compass?",
                                p => true,
                                p =>
                                {
                                    DialogueModule compassModule = new DialogueModule("The Compass of True North is said to be hidden in the Caves of Whispers, guarded by a spirit that tests the hearts of those who seek it. Only the pure of intention can claim it.");
                                    compassModule.AddOption("I will seek out this compass.",
                                        plp => true,
                                        plp =>
                                        {
                                            pl.SendMessage("You set your sights on the Caves of Whispers, your heart full of determination.");
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    player.SendGump(new DialogueGump(player, compassModule));
                                });
                            player.SendGump(new DialogueGump(player, techniquesModule));
                        });
                    player.SendGump(new DialogueGump(player, roleModule));
                });

            greeting.AddOption("Tell me about the challenges of being a cartographer.",
                player => true,
                player =>
                {
                    DialogueModule challengesModule = new DialogueModule("The greatest challenge is often not the physical journey but the mental one. Mapping uncharted lands can be disheartening when faced with obstacles, and the elusive city of Wind is a perfect example of this.");
                    challengesModule.AddOption("What makes Wind so elusive?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule elusiveModule = new DialogueModule("Wind seems to hide from those who pursue it too eagerly. It requires patience and an understanding of its whispers. Some say it's a test of the seeker’s heart and mind.");
                            elusiveModule.AddOption("How can I prepare myself for such a journey?",
                                p => true,
                                p =>
                                {
                                    DialogueModule preparationModule = new DialogueModule("Prepare yourself by being open to the unknown. Trust your instincts, and learn to read the signs the world gives you. Bring along companions who share your curiosity.");
                                    preparationModule.AddOption("I will remember your words.",
                                        pla => true,
                                        pla =>
                                        {
                                            pla.SendGump(new DialogueGump(pla, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, preparationModule));
                                });
                            player.SendGump(new DialogueGump(player, elusiveModule));
                        });
                    challengesModule.AddOption("What do you find most rewarding?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule rewardModule = new DialogueModule("The most rewarding moments come from discovering something new and sharing that knowledge with others. Every map tells a story, and I am honored to be the storyteller.");
                            rewardModule.AddOption("I admire your dedication.",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            player.SendGump(new DialogueGump(player, rewardModule));
                        });
                    player.SendGump(new DialogueGump(player, challengesModule));
                });

            return greeting;
        }

        public PathfinderPenelope(Serial serial) : base(serial) { }

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
