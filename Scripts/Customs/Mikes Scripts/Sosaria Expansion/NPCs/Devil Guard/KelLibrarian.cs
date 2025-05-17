using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class KelLibrarian : BaseCreature
    {
        [Constructable]
        public KelLibrarian() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kel the Librarian";
            Title = "Keeper of Forbidden Lore";
            Body = 0x190; // humanoid body

            // Basic stats for a scholarly yet shrewd NPC
            SetStr(80);
            SetDex(90);
            SetInt(110);
            SetHits(100);

            // Appearance and gear: scholarly robes mixed with hints of her adventurous past.
            AddItem(new Robe() { Hue = 1770, Name = "Robe of Arcane Reflection" });
            AddItem(new Cap() { Hue = 1150, Name = "Scholarly Cap" });
            AddItem(new Spellbook() { Name = "Compendium of Forgotten Legends" });
            // Randomize skin tone and hair details:
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public KelLibrarian(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule(
                "Greetings, seeker of knowledge. I am Kel, Keeper of Forbidden Lore and steadfast guardian of our ancient archives. " +
                "Within these hallowed halls, I have debated arcane symbols with Arielle of Fawn, discussed lore with Jasper the Scribe, " +
                "and even exchanged dire warnings with Crag of Devil Guard. " +
                "But tell me… what mysteries does your heart seek today?");

            // Option 1: Forbidden Book Incident
            greeting.AddOption("Tell me about the forbidden book incident with the Fawn artist.",
                player => true,
                player =>
                {
                    DialogueModule forbiddenBookModule = new DialogueModule(
                        "Ah, the forbidden book incident—a dark chapter indeed. A daring Fawn artist, intoxicated by ambition, " +
                        "attempted to smuggle a cursed manuscript into our sanctum. Its pages murmured unholy secrets " +
                        "that could undo the veil between order and chaos. I was forced to intervene, and later, Jasper and I " +
                        "spent long nights discussing whether such knowledge should ever be uncovered. " +
                        "Do you seek its origins, or are you curious about our fateful debate?");
                    
                    forbiddenBookModule.AddOption("What are its origins?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule tomeOriginsModule = new DialogueModule(
                                "Legend holds that the tome was birthed in an era when magic was untamed—bound in the leather of a " +
                                "mythic beast and inscribed with symbols older than any mortal tongue. I cross-referenced these glyphs " +
                                "with records in Castle British, and even debated their implications with Jasper. Would you care " +
                                "to hear a recitation of one of its dire passages?");
                            tomeOriginsModule.AddOption("Yes, recite a passage.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule passageModule = new DialogueModule(
                                        "It proclaims: 'When the celestial tears of night embrace the pulse of the earth, " +
                                        "the boundary of creation shatters.' Such words have haunted dreams and spurred heated debates with " +
                                        "Arielle, whose art captures both their beauty and their threat.");
                                    plc.SendGump(new DialogueGump(plc, passageModule));
                                });
                            tomeOriginsModule.AddOption("No, thank you.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, tomeOriginsModule));
                        });
                    
                    forbiddenBookModule.AddOption("What did you and Jasper conclude?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule jaspersModule = new DialogueModule(
                                "Jasper remains a connoisseur of written lore. In our many midnight sessions, we argued that while such " +
                                "forbidden texts might enlighten the curious, they also carry the power to unleash pandemonium. " +
                                "Our conversations, laced with both caution and fervor, have become legend among scholars. " +
                                "Do you wish to know what fate befell the impetuous artist?");
                            jaspersModule.AddOption("Indeed, what became of the artist?",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule artistModule = new DialogueModule(
                                        "Banished from our inner circle, the artist now wanders the fringes of Fawn, their every brushstroke " +
                                        "tainted by the forbidden. Their legacy serves as a stark warning: some knowledge is best left veiled.");
                                    plq.SendGump(new DialogueGump(plq, artistModule));
                                });
                            jaspersModule.AddOption("I understand. Let us return.",
                                plq => true,
                                plq => plq.SendGump(new DialogueGump(plq, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, jaspersModule));
                        });
                    player.SendGump(new DialogueGump(player, forbiddenBookModule));
                });

            // Option 2: Arcane Lore & Esoteric Symbols
            greeting.AddOption("I yearn to learn about arcane lore and esoteric symbols.",
                player => true,
                player =>
                {
                    DialogueModule arcaneLoreModule = new DialogueModule(
                        "Arcane lore flows like a hidden river beneath our world. In intense discussions with Jasper, I've come to " +
                        "believe that these symbols are keys—portals between eras and forces. Arielle’s art brings them to life with " +
                        "a haunting grace. Would you like to scrutinize one in depth or simply learn their broad significance?");
                    
                    arcaneLoreModule.AddOption("Delve into a symbol’s depths.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule symbolsModule = new DialogueModule(
                                "Consider the emblem of the spiraling circle—a mark that some claim unlocks the cycle of birth and decay. " +
                                "I have long debated its meaning with Arielle. Do you desire a comprehensive dissection of its layers or " +
                                "a concise explanation of its role in our mythos?");
                            symbolsModule.AddOption("A comprehensive dissection, please.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule detailedSymbols = new DialogueModule(
                                        "This symbol, called 'The Veil of Ages,' intertwines time and destiny. Its outer circle represents " +
                                        "the endless march of time, while the inner spiral draws one into the depths of forgotten lore. " +
                                        "I have spent countless nights unraveling its secrets alongside Jasper. Does this knowledge inspire awe—or caution?");
                                    detailedSymbols.AddOption("Both. Such knowledge is dangerous.",
                                        plx => true,
                                        plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                                    detailedSymbols.AddOption("I wish to explore further later.",
                                        plx => true,
                                        plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                                    plc.SendGump(new DialogueGump(plc, detailedSymbols));
                                });
                            symbolsModule.AddOption("A concise explanation, then.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule summarySymbols = new DialogueModule(
                                        "Briefly, the symbol connects our reality to that which is beyond—a reminder that even our most " +
                                        "established truths hide undercurrents of mystery. It appears in rituals and ancient relics as a silent warning.");
                                    plc.SendGump(new DialogueGump(plc, summarySymbols));
                                });
                            pl.SendGump(new DialogueGump(pl, symbolsModule));
                        });
                    
                    arcaneLoreModule.AddOption("Tell me about magical traditions in Sosaria.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule traditionsModule = new DialogueModule(
                                "Our land is alive with magical traditions—from the secret rites in Yew's groves to experimental ceremonies " +
                                "in Devil Guard's mines. These practices, though varied, share a reverence for the natural and the arcane. " +
                                "Shall I recount a specific ritual or offer a sweeping overview of our mystical customs?");
                            traditionsModule.AddOption("Recount a specific ritual.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule ritualModule = new DialogueModule(
                                        "During a rare lunar eclipse, a clandestine group gathered for the 'Veil of Shadows' rite. Amid the " +
                                        "darkened sky and flickering candlelight, secrets were exchanged as if the very cosmos whispered truths. " +
                                        "I witnessed this spectacle—a moment of ephemeral power that continues to echo in my studies.");
                                    plc.SendGump(new DialogueGump(plc, ritualModule));
                                });
                            traditionsModule.AddOption("A sweeping overview, please.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule overviewModule = new DialogueModule(
                                        "In essence, the traditions of Sosaria mirror the balance of creation and destruction. " +
                                        "They inspire both reverence and fear, teaching us that magic is as unpredictable as it is essential.");
                                    plc.SendGump(new DialogueGump(plc, overviewModule));
                                });
                            pl.SendGump(new DialogueGump(pl, traditionsModule));
                        });
                    player.SendGump(new DialogueGump(player, arcaneLoreModule));
                });

            // Option 3: Local Legends and Ancient Narratives
            greeting.AddOption("What local legends have you unraveled in your studies?",
                player => true,
                player =>
                {
                    DialogueModule legendsModule = new DialogueModule(
                        "The tapestry of our region is woven with legends. My discourse with Crag of Devil Guard has revealed " +
                        "tales of spectral miners and curses that linger like shadows. Which story intrigues you more—the ghost " +
                        "of a damned miner, or the mystery of an ancient archive rumored to hide untold secrets?");
                    legendsModule.AddOption("Tell me about the spectral miner.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule minerModule = new DialogueModule(
                                "Crag speaks of a miner whose soul was shattered by the secrets buried deep within the earth. " +
                                "His restless spirit now wanders the dark tunnels, a harbinger of doom and a poignant reminder of ambition gone awry.");
                            minerModule.AddOption("How does his story conclude?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule tragicModule = new DialogueModule(
                                        "His tale remains a somber enigma—a narrative left unfinished, as if destiny itself " +
                                        "hesitates to seal his fate. It is a story that warns us: the thirst for forbidden knowledge exacts a steep price.");
                                    plc.SendGump(new DialogueGump(plc, tragicModule));
                                });
                            minerModule.AddOption("I appreciate the sorrow in his tale.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, minerModule));
                        });
                    legendsModule.AddOption("What about the ancient archive?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule otherLegendsModule = new DialogueModule(
                                "Some whisper of a hidden archive buried beneath the ruins—a repository that holds the combined wisdom " +
                                "of fallen civilizations. Its secrets could reshape kingdoms, as debated by Jasper, Arielle, and myself. " +
                                "Do you dare imagine what lies within?");
                            otherLegendsModule.AddOption("Yes, reveal its mysteries.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule archiveModule = new DialogueModule(
                                        "Within that vault, ancient scrolls and arcane devices lie in slumber. " +
                                        "It is said that unlocking its riddles could tip the balance of power in Sosaria. " +
                                        "A tantalizing prospect, yet fraught with peril.");
                                    plc.SendGump(new DialogueGump(plc, archiveModule));
                                });
                            otherLegendsModule.AddOption("Maybe another time.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, otherLegendsModule));
                        });
                    player.SendGump(new DialogueGump(player, legendsModule));
                });

            // Option 4: Personal Journey (Past and Transformation)
            greeting.AddOption("Could you share more about your personal journey as a librarian?",
                player => true,
                player =>
                {
                    DialogueModule personalModule = new DialogueModule(
                        "My path was not always that of a quiet guardian of dusty lore. In my youth, I roamed abandoned ruins and " +
                        "ancient trade routes, deciphering runes and secrets that few dared whisper. It was during those restless days " +
                        "that I made a solemn vow—to shield the world from knowledge that could ignite chaos. " +
                        "Would you hear about the moment destiny found me?");
                    personalModule.AddOption("Yes, reveal that moment.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule momentModule = new DialogueModule(
                                "On a storm-lashed evening, in a forgotten marketplace lit by the flicker of lanterns, I encountered a " +
                                "man whose words could sway kings. His political intrigues and secret deals ignited within me a desire for " +
                                "greater understanding—a turning point that set me upon the path from merchant to keeper. " +
                                "My heart, though heavy with the cost of that choice, has never wavered.");
                            momentModule.AddOption("A life-changing encounter indeed.",
                                plx => true,
                                plx => plx.SendGump(new DialogueGump(plx, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, momentModule));
                        });
                    personalModule.AddOption("Perhaps another time, then.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, personalModule));
                });

            // Option 5: Merchant's Secrets (New Branch)
            greeting.AddOption("I sense there is more to you than a mere librarian… Who were you before these tomes?",
                player => true,
                player =>
                {
                    DialogueModule merchantModule = new DialogueModule(
                        "Ah, you have an astute eye. Beneath this scholarly guise, there once beat the heart of a charismatic traveling merchant. " +
                        "I traversed the realms, weaving charm and wit to uncover political secrets and whispered intrigues from rival courts. " +
                        "Every market stall, every royal banquet, was an opportunity—an artful dance between truth and deception.");
                    
                    // Nested option: Merchant Days
                    merchantModule.AddOption("Tell me about your days on the road.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule merchantDaysModule = new DialogueModule(
                                "I remember the splendor of exotic bazaars and the subtle power games of royal courts. I charmed nobles " +
                                "with a smile, all the while discreetly collecting secrets that could topple empires. Every whispered rumor " +
                                "was currency in my trade. These experiences taught me that knowledge, in the right hands, is the ultimate power.");
                            
                            // Further nested branch: The art of charm
                            merchantDaysModule.AddOption("How did you perfect the art of charm and manipulation?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule charmModule = new DialogueModule(
                                        "Charm, my dear interlocutor, is not merely a smile but a finely honed instrument of influence. " +
                                        "I observed every flicker of hesitation, every word unspoken. With each subtle gesture, I would guide conversations " +
                                        "to reveal the hidden truths. In the courts of rival kingdoms, a well-placed compliment was worth more than a signed decree.");
                                    charmModule.AddOption("Incredible…",
                                        plx => true,
                                        plx => plx.SendGump(new DialogueGump(plx, merchantModule)));
                                    plc.SendGump(new DialogueGump(plc, charmModule));
                                });
                            
                            // Further nested branch: Political Secrets
                            merchantDaysModule.AddOption("What kind of political secrets did you gather?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule secretModule = new DialogueModule(
                                        "I delved into the murmurs of intrigue: secret alliances, betrayals at the highest levels, and whispered pacts " +
                                        "between kingdoms. I learned of covert plots and the hidden motives of rulers—knowledge that could alter the balance " +
                                        "of power. Such secrets, when wielded wisely, could save or shatter nations.");
                                    
                                    // Extra nested option: Trading secrets
                                    secretModule.AddOption("Did you ever trade these secrets?",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule tradeModule = new DialogueModule(
                                                "Indeed. I became a silent broker—a conduit between those who sought power and those desperate to retain it. " +
                                                "I sold truths veiled in half-lies, always careful never to reveal too much, lest I become entangled in the very webs I wove.");
                                            tradeModule.AddOption("That is both fascinating and dangerous.",
                                                ply => true,
                                                ply => ply.SendGump(new DialogueGump(ply, merchantModule)));
                                            plx.SendGump(new DialogueGump(plx, tradeModule));
                                        });
                                    
                                    secretModule.AddOption("I see. And did these secrets burden you?",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule burdenModule = new DialogueModule(
                                                "The weight of every secret is a double-edged sword. While power flowed through my veins, each whispered truth " +
                                                "came with its own price. I learned to balance my ambition with a careful concealment of my true self—a lesson " +
                                                "that still informs my choices today.");
                                            burdenModule.AddOption("Such a burden must leave its mark.",
                                                ply => true,
                                                ply => ply.SendGump(new DialogueGump(ply, merchantModule)));
                                            plx.SendGump(new DialogueGump(plx, burdenModule));
                                        });
                                    plc.SendGump(new DialogueGump(plc, secretModule));
                                });
                            
                            // Option to return
                            merchantDaysModule.AddOption("I appreciate your tales of old.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, merchantDaysModule));
                        });
                    
                    // Nested option: Conflict between past and present
                    merchantModule.AddOption("Did your past ever conflict with your duties here as a librarian?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule conflictModule = new DialogueModule(
                                "The roles of keeper and merchant are seemingly at odds, yet both require vigilance and discretion. " +
                                "While I now preserve lore with reverence, I still retain the cunning and observation honed on the road. " +
                                "That duality is my greatest strength—and my most guarded secret. Would you like to hear of a time " +
                                "when my past nearly upended the present?");
                            
                            conflictModule.AddOption("Yes, reveal that incident.",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule incidentModule = new DialogueModule(
                                        "Once, in the midst of a heated dispute among rival courtiers, I recognized the telltale signs of betrayal. " +
                                        "Using whispered confidences and subtle manipulation, I steered events away from ruin—and in doing so, ensured " +
                                        "that the truth of their schemes remained cloaked. It was a dangerous dance, one that cost me dearly in sleepless nights " +
                                        "and quiet remorse.");
                                    incidentModule.AddOption("A masterful, if perilous, feat.",
                                        plx => true,
                                        plx => plx.SendGump(new DialogueGump(plx, merchantModule)));
                                    incidentModule.AddOption("I wonder how you still sleep at night.",
                                        plx => true,
                                        plx =>
                                        {
                                            DialogueModule sleepModule = new DialogueModule(
                                                "Sleep is a luxury for those who bear too many secrets. Yet, I have learned to reconcile my past with my present—using " +
                                                "my observations not for profit alone, but to guide the lost and the curious toward safer shores.");
                                            plx.SendGump(new DialogueGump(plx, sleepModule));
                                        });
                                    plc.SendGump(new DialogueGump(plc, incidentModule));
                                });
                            conflictModule.AddOption("I see; your world is indeed complex.",
                                plc => true,
                                plc => plc.SendGump(new DialogueGump(plc, merchantModule)));
                            pl.SendGump(new DialogueGump(pl, conflictModule));
                        });
                    
                    // Option to gracefully exit merchant branch
                    merchantModule.AddOption("Thank you for sharing these secrets.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, merchantModule));
                });

            // Option 6: Farewell
            greeting.AddOption("Farewell, Kel. I must take my leave.",
                player => true,
                player =>
                {
                    DialogueModule farewellModule = new DialogueModule(
                        "May the wisdom of forgotten epochs guide your steps and the shadows of truth guard your heart. " +
                        "Remember, the pursuit of knowledge is as perilous as it is wondrous. Farewell, seeker—until our paths " +
                        "intertwine again in these silent halls.");
                    player.SendGump(new DialogueGump(player, farewellModule));
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
