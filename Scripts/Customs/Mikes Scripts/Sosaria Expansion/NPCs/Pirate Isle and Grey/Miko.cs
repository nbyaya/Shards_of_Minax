using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remnants of cartographic relics")]
    public class Miko : BaseCreature
    {
        private DateTime lastMapUpdateTime;

        [Constructable]
        public Miko() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Miko, the Mapmaker";
            Body = 0x190; // Human male body

            // Stats
            SetStr(90);
            SetDex(100);
            SetInt(130);
            SetHits(100);

            // Appearance and Equipment
            AddItem(new Shirt() { Hue = 1005, Name = "Intricate Cartographer's Vest" });
            AddItem(new ShortPants() { Hue = 2150 });
            AddItem(new Sandals() { Hue = 1650 });
            AddItem(new MapmakersPen() { Name = "Master's Quill" });
            AddItem(new BlankMap() { Name = "Unfinished Map Scroll" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            lastMapUpdateTime = DateTime.MinValue;
        }

        public Miko(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Miko, the renowned mapmaker of Sosaria. My days are spent charting secret relic sites, treacherous dungeons, and hidden coastal pathways. Many know me for my collaborations with Jasper the Scribe and Captain Waylon. Yet, if you listen closely, you might sense that there is more to me—a secret past of magic and solitude. What would you like to discuss?");
            
            // Option 1: Working with Jasper the Scribe
            greeting.AddOption("Tell me about your work with Jasper the Scribe.", 
                player => true, 
                player => 
                {
                    DialogueModule jasperModule = new DialogueModule("Ah, Jasper—the diligent scribe whose records breathe life into my maps. Together, we've deciphered the mysteries of ancient ruins and the cryptic inscriptions of lost temples. Would you like to hear about a recent discovery or our unique methods?");
                    
                    jasperModule.AddOption("Tell me about a recent discovery.", 
                        p => true, 
                        p => 
                        {
                            DialogueModule recentDiscovery = new DialogueModule("Not long ago, Jasper deciphered inscriptions near Castle British that hinted at forgotten vaults beneath its grand halls. I meticulously charted every hidden passage and trap, ensuring that adventurers are warned of lurking dangers. Fascinating, isn’t it?");
                            recentDiscovery.AddOption("It truly is remarkable!", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            recentDiscovery.AddOption("Can you reveal more map details?", 
                                pl => true, 
                                pl =>
                                {
                                    DialogueModule mapDetails = new DialogueModule("Every line on my maps is a blend of precision and lore. Jasper provides historical context while I overlay cartographic symbols—each marking relics, secret passages, and even the faint whispers of ancient magic. Would you like to delve deeper into our process?");
                                    mapDetails.AddOption("Yes, please explain the process.", 
                                        plx => true, 
                                        plx => 
                                        {
                                            DialogueModule processModule = new DialogueModule("Our process is a delicate dance: Jasper’s manuscripts, replete with obscure symbols, meet my modern surveying techniques. Together, we capture not just geography but the very spirit of Sosaria—a spirit imbued with magic and mystery.");
                                            plx.SendGump(new DialogueGump(plx, processModule));
                                        });
                                    mapDetails.AddOption("I think I've had enough details.", 
                                        plx => true, 
                                        plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, mapDetails));
                                });
                            p.SendGump(new DialogueGump(p, recentDiscovery));
                        });
                    
                    jasperModule.AddOption("What methods do you use together?", 
                        p => true, 
                        p => 
                        {
                            DialogueModule methodsModule = new DialogueModule("Our collaboration is an art form: Jasper’s careful records of lore mix with my instinct for geography. Countless nights spent poring over dusty tomes and quiet debates over the meaning of ancient symbols—they all go into creating maps that serve as both guides and historical accounts.");
                            methodsModule.AddOption("That sounds intense.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            methodsModule.AddOption("Do you ever have conflicts?", 
                                pl => true, 
                                pl =>
                                {
                                    DialogueModule debateModule = new DialogueModule("Indeed, there have been moments of discord. Jasper is unwavering in his interpretation of lore, while I sometimes see a hidden pattern where none is obvious. Yet, our debates only refine the maps further. Would you like an example of our most memorable disagreement?");
                                    debateModule.AddOption("Yes, share an example.", 
                                        plx => true, 
                                        plx => 
                                        {
                                            DialogueModule exampleModule = new DialogueModule("In the depths of the Whispering Woods, Jasper insisted there was a single hidden grove, while I charted multiple secret clearings. Our debate led to a compromise that not only revealed the grove but unveiled an intricate network of mystic pathways. A triumph of collaboration!");
                                            plx.SendGump(new DialogueGump(plx, exampleModule));
                                        });
                                    debateModule.AddOption("That's enough detail for me.", 
                                        plx => true, 
                                        plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, debateModule));
                                });
                            p.SendGump(new DialogueGump(p, methodsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, jasperModule));
                });

            // Option 2: Adventures with Captain Waylon
            greeting.AddOption("What adventures have you shared with Captain Waylon?", 
                player => true, 
                player => 
                {
                    DialogueModule waylonModule = new DialogueModule("Captain Waylon is a rugged seafarer with a wealth of stories from dangerous waters and ghostly encounters. With him, I've mapped eerie shipwrecks and navigated treacherous coastal reefs. What tale would you prefer to hear?");
                    
                    waylonModule.AddOption("Tell me about the ghostly shipwreck.", 
                        p => true, 
                        p =>
                        {
                            DialogueModule shipwreckModule = new DialogueModule("Once, off the coast of Grey & Pirate Isle, we investigated a shipwreck shrouded in spectral mist. Amid shifting shadows, Captain Waylon recounted legends of lost souls and ancient curses. I marked every dangerous inlet and hidden cove, preserving the tale on my maps.");
                            shipwreckModule.AddOption("What did you learn from that journey?", 
                                pl => true, 
                                pl =>
                                {
                                    DialogueModule learnModule = new DialogueModule("We uncovered that the wreck was part of a forgotten smuggling route. Every detail—a torn sail, a broken figurehead—was a clue. Our maps now warn future navigators of these spectral dangers.");
                                    pl.SendGump(new DialogueGump(pl, learnModule));
                                });
                            shipwreckModule.AddOption("I'd like another tale.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, shipwreckModule));
                        });
                    
                    waylonModule.AddOption("What about those perilous coastal traps?", 
                        p => true, 
                        p =>
                        {
                            DialogueModule trapsModule = new DialogueModule("Captain Waylon once confided in me about coastal regions where nature itself conspires against the unwary: hidden reefs, sudden tidal surges, and treacherous rock formations that resemble ancient ruins. We chart these with care to protect adventurers.");
                            trapsModule.AddOption("How do you mark these dangers?", 
                                pl => true, 
                                pl =>
                                {
                                    DialogueModule chartModule = new DialogueModule("Every hazard is recorded with symbols and lore-derived annotations. Captain Waylon's firsthand experiences and my cartographic precision ensure that no danger goes unnoticed.");
                                    pl.SendGump(new DialogueGump(pl, chartModule));
                                });
                            trapsModule.AddOption("Your teamwork is remarkable.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, trapsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, waylonModule));
                });

            // Option 3: Relic Discoveries
            greeting.AddOption("What new relic sites have you uncovered recently?", 
                player => true, 
                player =>
                {
                    DialogueModule relicModule = new DialogueModule("My recent expeditions have led me to relics that defy belief. In a secluded cavern beneath the Whispering Woods, I discovered runes that pulse with hidden power. Would you like to learn about this enigmatic cavern or another mysterious find?");
                    
                    relicModule.AddOption("Tell me about the hidden cavern.", 
                        p => true, 
                        p =>
                        {
                            DialogueModule cavernModule = new DialogueModule("Deep within the Whispering Woods, I found a cavern whose walls are inscribed with runes of an ancient language. The very air there shimmers as if alive with magic. I spent days charting its maze-like passages, each turn hinting at secret chambers and forgotten rituals.");
                            cavernModule.AddOption("What secrets might it hold?", 
                                pl => true, 
                                pl =>
                                {
                                    DialogueModule secretsModule = new DialogueModule("Legends whisper that the cavern conceals an altar to a forgotten deity. Others claim it is a reservoir of raw magical energy, waiting to be tapped by those brave enough to understand its language.");
                                    pl.SendGump(new DialogueGump(pl, secretsModule));
                                });
                            cavernModule.AddOption("I appreciate the mystery.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, cavernModule));
                        });
                    
                    relicModule.AddOption("Tell me of another discovery.", 
                        p => true, 
                        p =>
                        {
                            DialogueModule anotherRelic = new DialogueModule("On the outskirts of West Montor, amidst the ruins of an old fort, I uncovered tattered banners, crumbled statues, and carvings hinting at a long-forgotten battle. This site has added a new layer of history to my maps and whispered promises of deeper lore.");
                            anotherRelic.AddOption("Truly inspiring work.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, anotherRelic));
                        });
                    
                    player.SendGump(new DialogueGump(player, relicModule));
                });

            // Option 4: Life as a Mapmaker (and hints of magic)
            greeting.AddOption("Can you tell me more about your life as a mapmaker?", 
                player => true, 
                player =>
                {
                    DialogueModule lifeModule = new DialogueModule("My journey as a mapmaker began in the winding alleys of Grey, but the truth is far more complex. I once wandered in solitude, deep within enchanted forests, where I learned the arcane arts. Every map I create now is not only a record of geography but also a record of the subtle magic of this land.");
                    
                    lifeModule.AddOption("How did you become a mapmaker?", 
                        p => true, 
                        p =>
                        {
                            DialogueModule originModule = new DialogueModule("It began with a chance discovery—a hidden scroll in a moss-covered ruin that spoke of ancient ley lines and mystical forces. Drawn by an inexplicable pull, I abandoned my secluded existence to decipher these secrets and chart the unseen energies of Sosaria.");
                            originModule.AddOption("That sounds utterly magical.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            p.SendGump(new DialogueGump(p, originModule));
                        });
                    
                    lifeModule.AddOption("What challenges do you face in your work?", 
                        p => true, 
                        p =>
                        {
                            DialogueModule challengesModule = new DialogueModule("The challenge is twofold: navigating treacherous terrain and taming the unpredictable whispers of magic that linger in the land. There are times when the forces I once harnessed in solitude return unexpectedly, reminding me of a past I dare not speak of openly.");
                            challengesModule.AddOption("I admire your resolve.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            challengesModule.AddOption("What do you mean by a 'past you dare not speak of'?", 
                                pl => true, 
                                pl =>
                                {
                                    DialogueModule hintedPastModule = new DialogueModule("There are nights, in the quiet solitude of my study, when the memories of my former life as a hermit sorcerer in an ancient, enchanted forest come flooding back. I was once labeled a monster—feared for wielding magic that I hardly understood. But behind that fear lay my kind heart and my gentle nature. Would you like to learn more about that hidden chapter of my life?");
                                    hintedPastModule.AddOption("Yes, please tell me about your secret past.", 
                                        plx => true, 
                                        plx =>
                                        {
                                            DialogueModule secretPastModule = new DialogueModule("I was born with an innate connection to magic, but instead of embracing it among society, I retreated deep into the forest. There, amid whispering trees and shimmering glades, I practiced the arcane arts in solitude. The locals, frightened by my powers and my isolation, dubbed me a monster. Yet, in truth, I was nothing more than a shy, kind-hearted soul yearning for acceptance.");
                                            secretPastModule.AddOption("That is so tragic—how did you cope with their fear?", 
                                                plyy => true, 
                                                plyy =>
                                                {
                                                    DialogueModule copeModule = new DialogueModule("Through endless nights of study and quiet reflection, I discovered that true magic lies in compassion. I learned to channel my abilities to heal and protect. Over time, I integrated that wisdom into my work as a mapmaker, using magic to reveal hidden secrets on the land. It is a burden and a gift—a reminder of both solitude and hope.");
                                                    secretPastModule.AddOption("Your resilience is inspiring.", 
                                                        plz => true, 
                                                        plz => plz.SendGump(new DialogueGump(plz, CreateGreetingModule())));
                                                    secretPastModule.AddOption("I wish more people understood you.", 
                                                        plz => true, 
                                                        plz => plz.SendGump(new DialogueGump(plz, CreateGreetingModule())));
                                                    plyy.SendGump(new DialogueGump(plyy, copeModule));
                                                });
                                            secretPastModule.AddOption("Do you ever use your magic openly?", 
                                                plyy => true, 
                                                plyy =>
                                                {
                                                    DialogueModule magicUsageModule = new DialogueModule("I rarely do—my magic is subtle, woven into the very lines of my maps. When I touch parchment, ancient energies stir, revealing truths that elude ordinary sight. Though I am shy about it, I know that this power connects me to the heart of Sosaria. It remains my secret, shared only with those who truly seek understanding.");
                                                    plyy.SendGump(new DialogueGump(plyy, magicUsageModule));
                                                });
                                            secretPastModule.AddOption("I understand; some secrets are best kept hidden.", 
                                                plyy => true, 
                                                plyy => plyy.SendGump(new DialogueGump(plyy, CreateGreetingModule())));
                                            plx.SendGump(new DialogueGump(plx, secretPastModule));
                                        });
                                    hintedPastModule.AddOption("I respect your privacy. Let us return to your maps.", 
                                        plx => true, 
                                        plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, hintedPastModule));
                                });
                            p.SendGump(new DialogueGump(p, challengesModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            // Option 5: Reveal Your Secret Past (Dedicated Branch)
            greeting.AddOption("Reveal your secret past.", 
                player => true, 
                player =>
                {
                    DialogueModule secretModule = new DialogueModule("There are moments when I feel compelled to share the truth behind my eyes—a past hidden in shadow and magic. Beneath my role as a mapmaker lies a tale of a hermit sorcerer, a gentle spirit who once dwelled in an enchanted forest. Would you like to know how I discovered my magical nature, or how isolation shaped my destiny?");
                    
                    secretModule.AddOption("How did you discover your magical nature?", 
                        p => true, 
                        p =>
                        {
                            DialogueModule discoverModule = new DialogueModule("I recall a time, long ago, when the forest itself whispered secrets to me. Wandering among ancient trees, I discovered a glade lit by shimmering moonlight. There, a surge of power awoke me—magic flowed through my veins like a gentle river. I realized then that I was different from those around me, destined for a solitary journey.");
                            discoverModule.AddOption("That sounds wondrous.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            discoverModule.AddOption("Were you scared of this power?", 
                                pl => true, 
                                pl =>
                                {
                                    DialogueModule fearModule = new DialogueModule("In my youthful solitude, fear and wonder mingled. I was both terrified and enchanted by the power I could scarcely control. The forest became my sanctuary, a place where I could practice in secrecy while the outside world misunderstood me. It was a bittersweet awakening that would shape the rest of my life.");
                                    fearModule.AddOption("It must have been lonely.", 
                                        plx => true, 
                                        plx =>
                                        {
                                            DialogueModule lonelyModule = new DialogueModule("Yes, the solitude was profound. But in the quiet, I discovered that my kindness and gentle nature were gifts—an inner light that could heal as much as harm. This revelation became the foundation of my quiet strength, even as rumors painted me as a monster.");
                                            plx.SendGump(new DialogueGump(plx, lonelyModule));
                                        });
                                    fearModule.AddOption("I admire your resilience.", 
                                        plx => true, 
                                        plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, fearModule));
                                });
                            p.SendGump(new DialogueGump(p, discoverModule));
                        });
                    
                    secretModule.AddOption("How did isolation shape your destiny?", 
                        p => true, 
                        p =>
                        {
                            DialogueModule isolationModule = new DialogueModule("Living hidden in the forest, away from society’s harsh judgment, I honed my magical craft. The creatures of the wood became my companions, and the ancient spirits my teachers. Even though the world outside feared what they did not understand, I learned to use my magic for healing, protection, and the recording of nature’s secrets on my maps.");
                            isolationModule.AddOption("That is both sad and beautiful.", 
                                pl => true, 
                                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            isolationModule.AddOption("Do you still feel isolated?", 
                                pl => true, 
                                pl =>
                                {
                                    DialogueModule modernModule = new DialogueModule("Sometimes, yes. I carry the loneliness of those quiet, enchanted nights with me. Yet every map I create, every relic I uncover, connects me to the world. My magic, once a source of solitude, is now a bridge that unites the past and present. It is a secret I share only with those whose hearts seek truth.");
                                    pl.SendGump(new DialogueGump(pl, modernModule));
                                });
                            p.SendGump(new DialogueGump(p, isolationModule));
                        });
                    
                    secretModule.AddOption("I appreciate your trust in sharing this.", 
                        p => true, 
                        p => p.SendGump(new DialogueGump(p, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, secretModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastMapUpdateTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastMapUpdateTime = reader.ReadDateTime();
        }
    }
}
