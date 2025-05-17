using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Rolan : BaseCreature
    {
        [Constructable]
		public Rolan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rolan";
            Body = 0x190; // Human male body

            // Basic Stats
            SetStr(90);
            SetDex(80);
            SetInt(100);
            SetHits(110);

            // Appearance
            AddItem(new Tunic() { Hue = 150 });
            AddItem(new Boots() { Hue = 150 });
            AddItem(new TricorneHat() { Hue = 150 });
            // Optionally, you can add an item that hints at his quest (e.g., a carved amulet)
            AddItem(new GoldRing() { Name = "Ring of Lost Kin", Hue = 1175 });
        }

        public Rolan(Serial serial) : base(serial) { }

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
                "Greetings, traveler. I am Rolan, a wanderer haunted by the echoes of a lost past. " +
                "Ever since my dearest sibling vanished under mysterious circumstances, I have been " +
                "tormented by enigmatic carvings scattered throughout the land. These symbols, I believe, " +
                "are the key to unearthing the secrets of my family’s fate. I have sought counsel from " +
                "Toma of Fawn—whose art breathes life into ancient lore—and from Jasper the Scribe at " +
                "Castle British, who records every forgotten legend. How might you aid me in this quest?");

            greeting.AddOption("Tell me about these mysterious carvings.",
                player => true,
                player =>
                {
                    DialogueModule carvingsModule = new DialogueModule(
                        "The carvings are not mere scratches upon stone; they are the voice of our ancestors. " +
                        "Etched in hidden groves and ruined temples, these symbols speak of loss, hope, and destiny. " +
                        "I first noticed a recurring spiral entwined with a crescent moon—a symbol that now haunts my every step. " +
                        "Toma once confided that these markings might point the way to a secret sanctuary, one where the " +
                        "boundary between the living and the dead is thin. Would you like to learn about the origins of these carvings " +
                        "or hear what Toma has revealed to me?");
                    
                    carvingsModule.AddOption("Explain the origins of the carvings.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule originModule = new DialogueModule(
                                "Legends tell that these carvings were born in an age of great turmoil. " +
                                "Our ancestors carved them as both a prayer and a guide—a desperate plea for divine protection " +
                                "in a time when families were torn apart by calamity. Every line and curve is steeped in meaning, " +
                                "and some even say that the carvings are a map, leading to relics imbued with lost magic. " +
                                "Would you like me to detail the symbols themselves or discuss their historical context?");
                            
                            originModule.AddOption("Detail the specific symbols.",
                                ply => true,
                                ply =>
                                {
                                    DialogueModule symbolsModule = new DialogueModule(
                                        "The most striking symbol is a spiral intertwined with a crescent moon. " +
                                        "It represents the endless cycle of life, death, and rebirth—a cycle that my own family " +
                                        "seems doomed to repeat. Toma believes this symbol could unlock the secrets to reuniting lost kin. " +
                                        "Do you wish to hear Toma’s interpretation or learn more about the symbol’s ancient roots?");
                                    
                                    symbolsModule.AddOption("What does Toma say about it?",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule tomaInsightModule = new DialogueModule(
                                                "Toma, whose art captures the very soul of our legends, told me that the spiral and moon " +
                                                "together may indicate a hidden passage—a gateway to a realm where our ancestors still watch over us. " +
                                                "He insists that during the twilight hours, the carvings nearly shimmer with otherworldly light. " +
                                                "Does this theory resonate with you?");
                                            
                                            tomaInsightModule.AddOption("Yes, it is both eerie and inspiring.",
                                                plyxx => true,
                                                plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                            tomaInsightModule.AddOption("I remain skeptical, but intrigued.",
                                                plyxx => true,
                                                plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                            plyx.SendGump(new DialogueGump(plyx, tomaInsightModule));
                                        });
                                    
                                    symbolsModule.AddOption("Tell me more about the symbol's ancient roots.",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule historyModule = new DialogueModule(
                                                "In times long past, these symbols adorned sacred sites where elders and mystics " +
                                                "gathered to invoke the blessings of the divine. They were as much a part of ritual as " +
                                                "they were a means to record history. Many believe that the secret behind these carvings " +
                                                "could reunite families severed by fate. What do you think—can ancient art guide modern hearts?");
                                            
                                            historyModule.AddOption("It surely can, if one listens closely.",
                                                plyxx => true,
                                                plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                            historyModule.AddOption("I wonder if it’s merely coincidence.",
                                                plyxx => true,
                                                plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                            plyx.SendGump(new DialogueGump(plyx, historyModule));
                                        });
                                    
                                    ply.SendGump(new DialogueGump(ply, symbolsModule));
                                });
                            
                            originModule.AddOption("Discuss the historical context.",
                                ply => true,
                                ply =>
                                {
                                    DialogueModule contextModule = new DialogueModule(
                                        "The carvings date back to a period of upheaval, when natural disasters and wars " +
                                        "forced communities to cling to ancient rites. They were carved during ceremonies meant " +
                                        "to honor both the living and the dead, serving as a bridge between worlds. Even Jasper " +
                                        "the Scribe has recorded how such relics once united families and restored hope in dire times. " +
                                        "Would you like to explore how these ceremonies were held or learn about the lost relics that " +
                                        "accompanied them?");
                                    
                                    contextModule.AddOption("Describe the ancient ceremonies.",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule ceremoniesModule = new DialogueModule(
                                                "In those long-forgotten days, communities would gather under full moons, " +
                                                "chanting and dancing as they traced the carvings on stone. The rituals were a blend " +
                                                "of art and prayer—an effort to invoke the protection of the divine. It is said that, " +
                                                "for a fleeting moment, the air would vibrate with the voices of the ancestors. " +
                                                "Does this not stir something deep within you?");
                                            
                                            ceremoniesModule.AddOption("It stirs a profound curiosity within me.",
                                                plyxx => true,
                                                plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                            ceremoniesModule.AddOption("I find the idea beautiful yet mysterious.",
                                                plyxx => true,
                                                plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                            plyx.SendGump(new DialogueGump(plyx, ceremoniesModule));
                                        });
                                    
                                    contextModule.AddOption("Tell me about the lost relics.",
                                        plyx => true,
                                        plyx =>
                                        {
                                            DialogueModule relicsModule = new DialogueModule(
                                                "Jasper’s chronicles hint at relics—amulets, pendants, and scrolls—that " +
                                                "once symbolized unity and remembrance. One such relic, the Moonlit Pendant, is " +
                                                "rumored to glow when destiny calls for a reunion of lost kin. I cannot help but " +
                                                "wonder if such an artifact might lead me back to my sibling. Would you join me in " +
                                                "searching for clues about these relics?");
                                            
                                            relicsModule.AddOption("I will help you search for the pendant.",
                                                plyxx => true,
                                                plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                            relicsModule.AddOption("I need to learn more before I decide.",
                                                plyxx => true,
                                                plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                            plyx.SendGump(new DialogueGump(plyx, relicsModule));
                                        });
                                    
                                    ply.SendGump(new DialogueGump(ply, contextModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, originModule));
                        });
                    
                    carvingsModule.AddOption("Tell me about Toma’s interpretations.",
                        ply => true,
                        ply =>
                        {
                            DialogueModule tomaModule = new DialogueModule(
                                "Toma is an enigmatic artist from Fawn whose paintings capture the very soul " +
                                "of our ancestral lore. He sees in these carvings a message—a hidden map leading to " +
                                "a sanctuary where our past might be reclaimed. He once told me that, under the glow " +
                                "of the Moon Portals, the carvings appear almost alive, whispering secrets of a reunion " +
                                "lost in time. Would you like to hear more about his latest theory or his personal experiences?");
                            
                            tomaModule.AddOption("Share his latest theory.",
                                plyx => true,
                                plyx =>
                                {
                                    DialogueModule theoryModule = new DialogueModule(
                                        "According to Toma, the recurring spiral and crescent may indicate a secret passage " +
                                        "to an ancient burial ground—a site where our ancestors rest. He theorizes that " +
                                        "this place, shrouded in natural magic, might reveal the path to reconnect families " +
                                        "divided by tragedy. Does this notion kindle a spark of hope in you?");
                                    
                                    theoryModule.AddOption("Yes, that hope is what drives me.",
                                        plyxx => true,
                                        plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                    theoryModule.AddOption("I’m intrigued, though I need proof.",
                                        plyxx => true,
                                        plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                    plyx.SendGump(new DialogueGump(plyx, theoryModule));
                                });
                            
                            tomaModule.AddOption("Tell me about his personal experiences.",
                                plyx => true,
                                plyx =>
                                {
                                    DialogueModule experiencesModule = new DialogueModule(
                                        "Toma once described a night when the carvings seemed to pulse with life under a rare celestial alignment. " +
                                        "He felt as if the symbols were reaching out, urging him to remember a promise made long ago. " +
                                        "That night, his art transformed into a window to the past, a moment that has forever altered his view of fate. " +
                                        "Does his tale not stir a longing for answers within you?");
                                    
                                    experiencesModule.AddOption("Yes, his words resonate with me.",
                                        plyxx => true,
                                        plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                    experiencesModule.AddOption("I remain cautious about such mysteries.",
                                        plyxx => true,
                                        plyxx => plyxx.SendGump(new DialogueGump(plyxx, CreateGreetingModule())));
                                    plyx.SendGump(new DialogueGump(plyx, experiencesModule));
                                });
                            
                            ply.SendGump(new DialogueGump(ply, tomaModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, carvingsModule));
                });
            
            greeting.AddOption("What else troubles your heart?",
                player => true,
                player =>
                {
                    DialogueModule troublesModule = new DialogueModule(
                        "Beyond the cryptic carvings, my soul is burdened by memories of a once-happy family " +
                        "now torn apart by fate. I still recall the laughter of my sibling and the shared dreams " +
                        "of our future—dreams now replaced by a yearning to uncover the truth. " +
                        "I have leaned on the wisdom of Jasper the Scribe, whose chronicles keep our lost stories alive. " +
                        "Would you like to hear about my past journeys or the friends who have guided me?");
                    
                    troublesModule.AddOption("Tell me about your past journeys.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule journeysModule = new DialogueModule(
                                "I have wandered through ancient ruins and bustling market streets alike, " +
                                "searching for clues about my lost kin. From the mystical groves of Yew to the storied halls " +
                                "of Castle British, every step has revealed both wonder and sorrow. One encounter in particular " +
                                "still lingers in my memory—a moment of unexpected solace amid despair. Would you like to hear " +
                                "about that encounter or perhaps learn more about those who have stood by me on this arduous path?");
                            
                            journeysModule.AddOption("Share the memorable encounter.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule encounterModule = new DialogueModule(
                                        "In the shadow of a ruined temple near Fawn, I stumbled upon a weathered stone " +
                                        "bearing the same mysterious symbols that now torment me. In that silent moment, " +
                                        "I felt an inexplicable connection—as if my sibling’s spirit reached out through time. " +
                                        "It was both heart-wrenching and hopeful, a reminder that the past may yet guide us home. " +
                                        "Do you understand the power of such moments?");
                                    
                                    encounterModule.AddOption("Yes, I empathize deeply.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    encounterModule.AddOption("I have my own burdens to bear.",
                                        ply => true,
                                        ply => ply.SendGump(new DialogueGump(ply, CreateGreetingModule())));
                                    plx.SendGump(new DialogueGump(plx, encounterModule));
                                });
                            
                            journeysModule.AddOption("Tell me about those who guided you.",
                                plx => true,
                                plx =>
                                {
                                    DialogueModule peopleModule = new DialogueModule(
                                        "The path has been illuminated by the unwavering support of friends like Toma and Jasper. " +
                                        "Toma’s gentle artistry and soulful interpretations have provided comfort, while Jasper’s " +
                                        "meticulous records of our shared history have kindled hope. Their wisdom has been a beacon in " +
                                        "the darkest nights. Would you like to learn more about Toma or hear about Jasper’s enduring tales?");
                                    
                                    peopleModule.AddOption("Tell me about Toma.",
                                        ply => true,
                                        ply =>
                                        {
                                            DialogueModule tomaBioModule = new DialogueModule(
                                                "Toma is a reserved yet passionate artist from Fawn. His work, a fusion of intuition and ancient lore, " +
                                                "captures the ephemeral beauty of our heritage. He believes every brushstroke is a prayer, every " +
                                                "sketch a step closer to understanding our fate. His quiet determination has been a great comfort to me.");
                                            
                                            tomaBioModule.AddOption("His passion is truly admirable.",
                                                plyx => true,
                                                plyx => plyx.SendGump(new DialogueGump(plyx, CreateGreetingModule())));
                                            tomaBioModule.AddOption("I would love to hear more of his story.",
                                                plyx => true,
                                                plyx => plyx.SendGump(new DialogueGump(plyx, CreateGreetingModule())));
                                            ply.SendGump(new DialogueGump(ply, tomaBioModule));
                                        });
                                    
                                    peopleModule.AddOption("Tell me about Jasper the Scribe.",
                                        ply => true,
                                        ply =>
                                        {
                                            DialogueModule jasperBioModule = new DialogueModule(
                                                "Jasper, the venerable chronicler of Castle British, has devoted his life to recording our legends. " +
                                                "His careful pen and unwavering dedication have preserved tales of triumph and tragedy alike. " +
                                                "He insists that every lost relic and every faded symbol tells a story—and it is through these stories " +
                                                "that we may one day find our way back to what was lost.");
                                            
                                            jasperBioModule.AddOption("His chronicles are inspiring.",
                                                plyx => true,
                                                plyx => plyx.SendGump(new DialogueGump(plyx, CreateGreetingModule())));
                                            jasperBioModule.AddOption("I would like to hear more of his methods.",
                                                plyx => true,
                                                plyx => plyx.SendGump(new DialogueGump(plyx, CreateGreetingModule())));
                                            ply.SendGump(new DialogueGump(ply, jasperBioModule));
                                        });
                                    
                                    plx.SendGump(new DialogueGump(plx, peopleModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, journeysModule));
                        });
                    
                    troublesModule.AddOption("I have no further questions.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, troublesModule));
                });
            
            greeting.AddOption("Farewell.",
                player => true,
                player =>
                {
                    player.SendMessage("May the light of forgotten memories guide you on your journey.");
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
