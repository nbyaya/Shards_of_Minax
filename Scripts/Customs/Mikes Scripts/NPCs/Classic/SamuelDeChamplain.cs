using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Samuel de Champlain")]
    public class SamuelDeChamplain : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SamuelDeChamplain() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Samuel de Champlain";
            Body = 0x190; // Human male body

            // Initialize appearance
            AddItem(new LongPants() { Hue = 1153 });
            AddItem(new FancyShirt() { Hue = 1100 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new WideBrimHat() { Hue = 1100 });
            AddItem(new Cutlass() { Name = "Champlain's Blade" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize last reward time
            lastRewardTime = DateTime.MinValue;
        }

		public SamuelDeChamplain(Serial serial) : base(serial)
		{
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
            DialogueModule greeting = new DialogueModule("Ah, greetings, traveler! I am Samuel de Champlain, a traveler from the distant land of France. How may I assist you today?");

            greeting.AddOption("What do you think of magic?",
                player => true,
                player =>
                {
                    DialogueModule magicWonderModule = new DialogueModule("Magic! It fascinates me to no end! I once thought it merely a figment of tales and myths, but to discover it is real... it's breathtaking!");
                    magicWonderModule.AddOption("How did you discover it?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule discoveryModule = new DialogueModule("Ah, it was during my travels in the enchanted forests of Eldoria. I stumbled upon an ancient grove, where the air shimmered with energy. A fairy revealed herself, and I could hardly believe my eyes!");
                            discoveryModule.AddOption("What was the fairy like?",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule fairyDescriptionModule = new DialogueModule("She was a vision of beauty, with delicate wings that sparkled like morning dew. Her laughter was like music, and her knowledge of the magical world was vast. She spoke of wonders I had never imagined!");
                                    fairyDescriptionModule.AddOption("What did she teach you?",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule fairyTeachingsModule = new DialogueModule("She revealed the secrets of the elements—how fire dances, water flows, earth nurtures, and air whispers. I learned that magic flows through everything, binding us all!");
                                            fairyTeachingsModule.AddOption("Is it dangerous to know these secrets?",
                                                ple => true,
                                                ple =>
                                                {
                                                    DialogueModule dangerModule = new DialogueModule("Ah, knowledge always carries risk. The power of magic can be intoxicating and, if misused, dangerous. One must tread carefully and respect its nature.");
                                                    dangerModule.AddOption("I would like to learn more about magic.",
                                                        plr => true,
                                                        plr =>
                                                        {
                                                            DialogueModule learnMoreModule = new DialogueModule("To truly understand magic, one must observe and practice. There are tomes in hidden libraries, spells whispered in the night... Would you seek them out?");
                                                            learnMoreModule.AddOption("Yes, I will seek them!",
                                                                plt => true,
                                                                plt =>
                                                                {
                                                                    pl.SendMessage("Samuel nods approvingly, encouraging your pursuit of magical knowledge.");
                                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                });
                                                            learnMoreModule.AddOption("Perhaps another time.",
                                                                ply => true,
                                                                ply =>
                                                                {
                                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                });
                                                            pl.SendGump(new DialogueGump(pl, learnMoreModule));
                                                        });
                                                    pl.SendGump(new DialogueGump(pl, dangerModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, fairyTeachingsModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, fairyDescriptionModule));
                                });
                            pl.SendGump(new DialogueGump(pl, discoveryModule));
                        });
                    magicWonderModule.AddOption("What else have you learned about magic?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule moreMagicModule = new DialogueModule("I learned of magical creatures, enchanted artifacts, and the delicate balance between our world and the realm of magic. There are whispers of a hidden artifact that could amplify one's magical prowess...");
                            moreMagicModule.AddOption("Tell me more about the artifact.",
                                plu => true,
                                plu =>
                                {
                                    DialogueModule artifactModule = new DialogueModule("The artifact is said to be the Heart of the Elemental, a crystal that pulses with the energy of all four elements. Legends claim it can grant the wielder immense power!");
                                    artifactModule.AddOption("Where can I find it?",
                                        pli => true,
                                        pli =>
                                        {
                                            DialogueModule locationModule = new DialogueModule("Its location is lost to time, but some say it rests in the ruins of an ancient temple, guarded by powerful creatures. A brave soul would need to venture there!");
                                            locationModule.AddOption("I will search for it!",
                                                plo => true,
                                                plo =>
                                                {
                                                    pl.SendMessage("Samuel's eyes sparkle with excitement. 'May fortune favor your quest!'");
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            locationModule.AddOption("That sounds too perilous.",
                                                plp => true,
                                                plp =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            pl.SendGump(new DialogueGump(pl, locationModule));
                                        });
                                    moreMagicModule.AddOption("What if I don't succeed?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule failureModule = new DialogueModule("Every journey is fraught with challenges, but even in failure, one learns. It's the pursuit of knowledge that enriches us. Do not fear the unknown!");
                                            failureModule.AddOption("Your words inspire me.",
                                                pls => true,
                                                pls =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                });
                                            pl.SendGump(new DialogueGump(pl, failureModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, artifactModule));
                                });
                            moreMagicModule.AddOption("I think I need more time to ponder.",
                                pld => true,
                                pld =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, moreMagicModule));
                        });
                    player.SendGump(new DialogueGump(player, magicWonderModule));
                });

            greeting.AddOption("What do you know about the unknown?",
                player => true,
                player =>
                {
                    DialogueModule unknownModule = new DialogueModule("The unknown is a vast sea of possibilities! It beckons us to explore, to question, to discover what lies beyond the horizon.");
                    unknownModule.AddOption("What drives you to explore the unknown?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule driveModule = new DialogueModule("Curiosity fuels my soul! Every new sight, every tale of wonder draws me deeper into the realms of adventure. It is in exploration that we find our true selves.");
                            driveModule.AddOption("What have you found on your adventures?",
                                plf => true,
                                plf =>
                                {
                                    DialogueModule adventuresModule = new DialogueModule("I've encountered breathtaking landscapes, met extraordinary beings, and unveiled secrets that lie hidden in the shadows. Each adventure enriches my spirit.");
                                    adventuresModule.AddOption("Can you tell me about a specific adventure?",
                                        plg => true,
                                        plg =>
                                        {
                                            DialogueModule specificAdventureModule = new DialogueModule("Ah, let me tell you of my journey to the Crystal Caves of Mystral! The walls shimmered with vibrant colors, and echoes of ancient songs filled the air. It was a sight to behold!");
                                            specificAdventureModule.AddOption("What was the most remarkable thing you saw?",
                                                plh => true,
                                                plh =>
                                                {
                                                    DialogueModule remarkableModule = new DialogueModule("In the heart of the caves, I discovered a crystal that sang! It resonated with the essence of magic itself, and I could feel the energy coursing through my veins.");
                                                    remarkableModule.AddOption("Could magic be tied to such wonders?",
                                                        plj => true,
                                                        plj =>
                                                        {
                                                            DialogueModule magicConnectionModule = new DialogueModule("Absolutely! Magic flows through all things—every sound, every sight, and every emotion is a thread in the tapestry of existence. We are all connected by it.");
                                                            magicConnectionModule.AddOption("How can I feel this connection?",
                                                                plk => true,
                                                                plk =>
                                                                {
                                                                    DialogueModule connectionModule = new DialogueModule("To feel this connection, one must quiet the mind and open the heart. Meditation, observation, and a thirst for understanding will guide you.");
                                                                    connectionModule.AddOption("I will try that.",
                                                                        pll => true,
                                                                        pll =>
                                                                        {
                                                                            pl.SendMessage("Samuel nods encouragingly. 'May your journey lead you to enlightenment!'");
                                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                        });
                                                                    connectionModule.AddOption("Perhaps I need more guidance.",
                                                                        plz => true,
                                                                        plz =>
                                                                        {
                                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                        });
                                                                    pl.SendGump(new DialogueGump(pl, connectionModule));
                                                                });
                                                            pl.SendGump(new DialogueGump(pl, magicConnectionModule));
                                                        });
                                                    pl.SendGump(new DialogueGump(pl, remarkableModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, specificAdventureModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, adventuresModule));
                                });
                            pl.SendGump(new DialogueGump(pl, driveModule));
                        });
                    player.SendGump(new DialogueGump(player, unknownModule));
                });

            greeting.AddOption("What else do you offer?",
                player => true,
                player =>
                {
                    DialogueModule offeringsModule = new DialogueModule("I have tales of wonder, maps of uncharted lands, and perhaps a few magical trinkets. Would you care to see what I have?");
                    offeringsModule.AddOption("Yes, show me your wares!",
                        pl => true,
                        pl =>
                        {
                            // Here you would implement the shop interface
                            pl.SendMessage("Samuel shows you his collection of intriguing items and maps.");
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    offeringsModule.AddOption("Not right now, thank you.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, offeringsModule));
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
