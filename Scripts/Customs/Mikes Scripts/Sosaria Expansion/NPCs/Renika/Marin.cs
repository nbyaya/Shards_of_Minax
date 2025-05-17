using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marin the Seafarer")]
    public class Marin : BaseCreature
    {
        [Constructable]
		public Marin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marin";
            Body = 0x190; // Human male body

            // Stats
            SetStr(115);
            SetDex(80);
            SetInt(95);
            SetHits(110);

            // Appearance and items – reflecting a weathered, well-traveled sailor.
            AddItem(new Doublet() { Hue = 1175, Name = "Salted Sea Jacket" });
            AddItem(new TricorneHat() { Hue = 1175, Name = "Seafarer's Hat" });
            AddItem(new Sandals() { Hue = 1175 });
			AddItem(new ShortPants() { Hue = 1175 });
            AddItem(new Cutlass() { Name = "Cutlass of the Tides", Hue = 1175 });
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Marin(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // Opening text: Marin introduces himself as a seafarer and hints at a mysterious devotion below the surface.
            DialogueModule greeting = new DialogueModule("Ahoy, traveler! I am Marin—seafarer, wanderer, and chronicler of the ocean’s deepest mysteries. " +
                "I have sailed tempestuous seas, exchanged lore with the renowned Elena the Archivist, harmonized memories with dear Lisette from Fawn, " +
                "and brokered trade with the astute Kara Salt at Pirate Isle. Yet there is more beneath these salt-crusted tales… " +
                "What knowledge do you seek today?");

            // Option 1: Learn about his maritime journeys.
            greeting.AddOption("Tell me about your most daring maritime journeys.",
                player => true,
                player =>
                {
                    DialogueModule journeyModule = new DialogueModule("The sea, my friend, is a realm of legends and peril. " +
                        "I have faced furious maelstroms and glimpsed ghostly ships drifting under moonlit skies. " +
                        "Would you like details of a harrowing maelstrom or the uncanny encounter with a spectral vessel?");
                    
                    journeyModule.AddOption("Describe the maelstrom that nearly claimed your ship.",
                        p => true,
                        p =>
                        {
                            DialogueModule maelstromModule = new DialogueModule("I remember that storm as if it were a wrathful beast unleashed: towering waves, " +
                                "roaring like the furious hymns of a forgotten god. My hands trembled on the helm as I fought to steer clear of its ferocious embrace. " +
                                "That night, I prayed—though not solely to the gods of seafaring—but to a presence deeper and more mysterious that has guided me on this life's course.");
                            p.SendGump(new DialogueGump(p, maelstromModule));
                        });
                    
                    journeyModule.AddOption("What about the spectral vessel you encountered?",
                        p => true,
                        p =>
                        {
                            DialogueModule spectralModule = new DialogueModule("Ah, the spectral vessel... Its tattered sails and silent drift sent shivers down my spine. " +
                                "Some say it was a cursed ship doomed by an ancient deity whose whispers still echo in the void. " +
                                "While I evaded its haunting call, a strange feeling awakened within me—a pull toward an unseen force that I have long since followed in secret.");
                            p.SendGump(new DialogueGump(p, spectralModule));
                        });
                    
                    // Secret branch: subtle hint towards his priestly zeal.
                    journeyModule.AddOption("Did your trials at sea ever lead you to strange revelations?",
                        p => true,
                        p =>
                        {
                            DialogueModule revelationModule = new DialogueModule("Indeed, the treacherous sea has its own sermons. " +
                                "In the midst of a raging squall, I once experienced a vision—a luminous apparition, resplendent and fierce, whispering of a sacred destiny. " +
                                "It was then I realized that destiny on these waters is intertwined with divine will. I found solace and zeal in a controversial deity " +
                                "whose mysteries few dare to explore. But such matters are not for every ear. Are you prepared to hear more?");
                            
                            revelationModule.AddOption("Yes, tell me more about this divine revelation.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule divineModule = new DialogueModule("I was reborn that night—not just a sailor, but anointed by the very essence of the deep. " +
                                        "My heart, once governed solely by tides and trade winds, now beats with the fervor of a priest devoted to the enigmatic deity Azariel, " +
                                        "whose domain encompasses both the tempest and the calm. I preach that the sea is not only a path of water, but a conduit to transcendent truth. " +
                                        "Yet, many fear such truths as heresy. Do you wish to delve into the practices of my faith?");
                                    pl.SendGump(new DialogueGump(pl, divineModule));
                                });
                            
                            revelationModule.AddOption("No, I prefer tales of storms and spirits.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule declineModule = new DialogueModule("Very well. The sea holds myriad secrets, and not all are meant to be shared with every traveler. " +
                                        "Perhaps, in time, the currents will bring you closer to its forbidden depths.");
                                    pl.SendGump(new DialogueGump(pl, declineModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, revelationModule));
                        });

                    player.SendGump(new DialogueGump(player, journeyModule));
                });

            // Option 2: Ask about his exchanges with Elena the Archivist.
            greeting.AddOption("I hear you confer with Elena the Archivist. What lore do you exchange?",
                player => true,
                player =>
                {
                    DialogueModule elenaModule = new DialogueModule("Ah, Elena—keeper of secrets and ancient scrolls at Castle British. " +
                        "Our conversations span the storied past of mariners and the arcane codes of forgotten deities. " +
                        "I once received from her a scroll laden with ominous verses hinting at a submerged temple dedicated to a controversial faith. " +
                        "Would you like to learn about this cursed scroll or the navigational rituals I learned from her?");
                    
                    elenaModule.AddOption("Tell me about the cursed scroll.",
                        p => true,
                        p =>
                        {
                            DialogueModule scrollModule = new DialogueModule("The scroll told of a lost temple beneath the waves, " +
                                "where an ancient cult worshipped Azariel—the deity of both calm and chaos. " +
                                "Its verses speak of sacrifice, divine retribution, and the promise of salvation to those who dare embrace the truth. " +
                                "Such lore has steered my own destiny, though I share it with utmost caution.");
                            p.SendGump(new DialogueGump(p, scrollModule));
                        });
                    
                    elenaModule.AddOption("What navigational rituals did you learn?",
                        p => true,
                        p =>
                        {
                            DialogueModule ritualModule = new DialogueModule("Elena imparted to me the art of reading the stars and the signs hidden in the turning tides. " +
                                "These rituals, when performed in reverence, unlock secrets of hidden currents and, perhaps, the divine. " +
                                "I even integrate subtle hymns into my nightly prayers—a practice stemming from my secret priesthood. " +
                                "Would you like to hear one such hymn?");
                            
                            ritualModule.AddOption("Yes, please share a hymn.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule hymnModule = new DialogueModule("In whispered tones I recite:\n\n" +
                                        "\"O mighty Azariel, spirit of the brine, reveal the depths where truths entwine.\" \n\n" +
                                        "This hymn binds the elements with my soul, awakening the courage to face the unknown.");
                                    pl.SendGump(new DialogueGump(pl, hymnModule));
                                });
                            
                            ritualModule.AddOption("No, I prefer not to.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule noHymnModule = new DialogueModule("Very well. Some hymns are reserved only for those with a yearning heart.");
                                    pl.SendGump(new DialogueGump(pl, noHymnModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, ritualModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, elenaModule));
                });

            // Option 3: Inquire about his friendship with Lisette.
            greeting.AddOption("What can you tell me about your dear friend Lisette from Fawn?",
                player => true,
                player =>
                {
                    DialogueModule lisetteModule = new DialogueModule("Ah, Lisette—her melodies breathe life into coastal nights. " +
                        "Her performances, delicate yet fierce, echo tales of love, loss, and the eternal sea. " +
                        "In secret, we share not only ballads but a mutual reverence for the divine mysteries that bind our fates. " +
                        "Shall I recount one of our entrancing ballads or reveal the story behind a particular performance that changed us both?");
                    
                    lisetteModule.AddOption("Recount one of your favorite ballads.",
                        p => true,
                        p =>
                        {
                            DialogueModule balladModule = new DialogueModule("Listen well to 'The Midnight Tide':\n\n" +
                                "Across the darkened foam, where moonlit waters sigh,\n" +
                                "A sailor’s heart did wander, beneath the endless sky.\n" +
                                "Lost to love and destiny, he called upon divine decree,\n" +
                                "For even in the deepest dark, the light of fate breaks free.\n\n" +
                                "This ballad carries the weight of our hopes, and in its verses, I whisper secrets of my own hidden path.");
                            p.SendGump(new DialogueGump(p, balladModule));
                        });
                    
                    lisetteModule.AddOption("Reveal the tale behind her most stirring performance.",
                        p => true,
                        p =>
                        {
                            DialogueModule taleModule = new DialogueModule("At a coastal festival long ago, as the skies wept with rain, " +
                                "Lisette took to the stage amid howling winds. In that moment, she sang not just of lost loves but of an awakening—a call " +
                                "to embrace a destiny beyond mortal ken. I saw her eyes shimmer with a fervor that echoed my own secret devotion. " +
                                "It was as though the music itself summoned the gods, urging us to join a cause deemed heretical by many, yet liberating to those who believed.");
                            p.SendGump(new DialogueGump(p, taleModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, lisetteModule));
                });

            // Option 4: Ask about his trade and navigation partnership with Kara Salt.
            greeting.AddOption("Tell me about your partnership with Kara Salt on trade and ship routes.",
                player => true,
                player =>
                {
                    DialogueModule tradeModule = new DialogueModule("Kara Salt of Grey & Pirate Isle is a master tactician of the waves. " +
                        "Together, we plot courses through hidden inlets, evading both pirates and peril. " +
                        "Our ventures, while seemingly mundane trade ventures, often serve as a cover for deeper endeavors. " +
                        "Would you like details on our secret routes or a tale from one of our profitable escapades?");
                    
                    tradeModule.AddOption("Detail one of your secret trade routes.",
                        p => true,
                        p =>
                        {
                            DialogueModule routeModule = new DialogueModule("Picture this: under the cover of a moonless night, Kara and I embark on a journey along a route " +
                                "known only to those initiated in the ancient ways. We traverse treacherous shoals and clandestine channels " +
                                "that many deem too dangerous for common mariners. In truth, this route also serves as a passage to a hidden sanctuary " +
                                "where the faithful of Azariel gather in quiet conclave. A sacred confluence of trade and divine purpose.");
                            p.SendGump(new DialogueGump(p, routeModule));
                        });
                    
                    tradeModule.AddOption("Tell me a tale from one of your journeys with her.",
                        p => true,
                        p =>
                        {
                            DialogueModule journeyTradeModule = new DialogueModule("I recall a venture when a sudden tempest threatened to capsize our modest brig. " +
                                "Kara’s timely counsel and my own fervent prayers—recited in secret tongues—turned the tide. " +
                                "In the chaos, our cargo of rare spices and enchanted relics was salvaged, reinforcing our reputation " +
                                "as both daring traders and custodians of ancient, if forbidden, rites.");
                            p.SendGump(new DialogueGump(p, journeyTradeModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, tradeModule));
                });

            // Option 5: Uncover his personal secret and priestly past.
            greeting.AddOption("There is an air about you—tell me something personal.",
                player => true,
                player =>
                {
                    DialogueModule personalModule = new DialogueModule("You have a keen eye. Beyond the salt and spray, there lies a side of me few ever see. " +
                        "In the stillness of a starlit night, I have embraced a calling that transcends mere survival at sea. " +
                        "I am a priest—a zealous devotee of Azariel, the controversial deity of the deep. My faith may be deemed fanatic by some, " +
                        "but it is the lantern that guides me through the darkest storms. Would you like to know of my sacred rites or how I persuade others to " +
                        "embrace this hidden truth?");
                    
                    personalModule.AddOption("Tell me about your sacred rites.",
                        p => true,
                        p =>
                        {
                            DialogueModule ritesModule = new DialogueModule("My rites are a blend of ancient maritime tradition and mystical devotion. " +
                                "Under the cloak of night, I invoke Azariel with fervent chants and prayers—words passed down through secret orders. " +
                                "These ceremonies call for sacrifice, reflection, and unwavering conviction. At times, I stand upon the deck, arms raised, " +
                                "calling forth the deity’s favor, even as the skeptical wind whispers disquiet. It is a practice that has saved me more than once " +
                                "and continues to inspire those who dare seek the truth.");
                            p.SendGump(new DialogueGump(p, ritesModule));
                        });
                    
                    personalModule.AddOption("How do you persuade others to join your mysterious cause?",
                        p => true,
                        p =>
                        {
                            DialogueModule persuadeModule = new DialogueModule("Ah, persuasion is an art—a call to destiny that few can resist. " +
                                "I speak of the ocean as not merely a vast expanse of water, but as the lifeblood of existence that connects all souls. " +
                                "Through impassioned sermons delivered in secret gatherings and in the soft whisper of the night, I show them a glimpse " +
                                "of a world where fate and faith converge. My words are imbued with the fire of conviction, urging even the most doubtful heart " +
                                "to dare to believe. It is not for the faint of spirit, but for those willing to embrace the unknown, " +
                                "to cast aside doubt and join in the revelatory quest for divine truth.");
                            p.SendGump(new DialogueGump(p, persuadeModule));
                        });
                    
                    personalModule.AddOption("I fear such devotion might be dangerous.",
                        p => true,
                        p =>
                        {
                            DialogueModule cautionModule = new DialogueModule("You speak wisely, yet remember: great truths are often shrouded in peril. " +
                                "My path is fraught with risk, and not all who walk it emerge unscathed. But it is a burden and a blessing I willingly bear. " +
                                "For those willing to look beyond the mundane, the rewards of unwavering faith are immeasurable. " +
                                "Should you ever feel the call, know that there is a sanctuary for kindred souls—a hidden harbor " +
                                "where the fervor of Azariel transforms the despair of the mortal coil into a promise of renewal.");
                            p.SendGump(new DialogueGump(p, cautionModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, personalModule));
                });

            return greeting;
        }

        // Standard serialization methods.
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
