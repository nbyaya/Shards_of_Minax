using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Lisette : BaseCreature
    {
        [Constructable]
        public Lisette() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lisette";
            Body = 0x191; // Standard human body

            // Basic stats and appearance
            SetStr(80);
            SetDex(90);
            SetInt(100);
            SetHits(100);

            // Dress her in a flowing minstrel outfit and give her a signature instrument
            AddItem(new FancyDress() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new Lute() { Name = "Lisette's Lute", Hue = 1150 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Lisette(Serial serial) : base(serial) { }

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
                "Ahoy, traveler! I am Lisette, a wandering minstrel of these coastal lands. " +
                "My songs echo with the laughter of festivals, the wisdom of the deep, and a burden I bear in secret. " +
                "I've performed alongside Marin at the grand Coastal Festival in Renika, exchanged maritime legends " +
                "with the formidable Kara Salt of Pirate Isle, and shared stirring sea shanties with the erudite Kess from Dawn. " +
                "But beneath these lively tales lies a stoic heart, dutiful and wise, for I am bound by an ancient oath " +
                "to guard a hidden shrine until a worthy successor emerges. What stories do you wish to hear?"
            );

            // Option 1: Coastal Festival with Marin
            greeting.AddOption("Tell me about the Coastal Festival with Marin.", 
                player => true,
                player =>
                {
                    DialogueModule festivalModule = new DialogueModule(
                        "The Coastal Festival is a dazzling celebration of life at sea! Marin and I once graced the sandy stage " +
                        "beneath a tapestry of stars, our voices mingling with the crashing waves. Would you like to know how we first met " +
                        "or learn more about the festival's vibrant traditions?"
                    );
                    festivalModule.AddOption("How did you first meet Marin?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule meetModule = new DialogueModule(
                                "It was a moonlit evening at the bustling Renikan docks. Amid the hum of conversation and clatter of the tavern, " +
                                "I heard a soulful melody drifting across the pier. Drawn by its pull, I found Marin recounting his daring escapades. " +
                                "Our conversation flowed as naturally as the tide. Would you care to know more about his adventurous voyages " +
                                "or our unforgettable first duet?"
                            );
                            meetModule.AddOption("Tell me about his adventurous voyages.", 
                                p => true,
                                p =>
                                {
                                    DialogueModule escapadeModule = new DialogueModule(
                                        "Marin has traversed treacherous whirlpools, braved furious tempests, and even encountered phantom vessels " +
                                        "lost to time. His tales speak of narrow escapes and miraculous encounters—each a vivid brushstroke in the grand canvas " +
                                        "of the sea's eternal mystery."
                                    );
                                    p.SendGump(new DialogueGump(p, escapadeModule));
                                });
                            meetModule.AddOption("Describe your first duet together.", 
                                p => true,
                                p =>
                                {
                                    DialogueModule duetModule = new DialogueModule(
                                        "Our first duet was pure enchantment. As our voices intertwined under the moon’s gentle gaze, it was as if " +
                                        "the sea itself paused to listen. That moment of harmonic convergence still resonates within me—a memory of magic " +
                                        "etched in starlight and salt."
                                    );
                                    p.SendGump(new DialogueGump(p, duetModule));
                                });
                            pl.SendGump(new DialogueGump(pl, meetModule));
                        });
                    festivalModule.AddOption("What are the festival's traditions?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule traditionsModule = new DialogueModule(
                                "The festival is a riot of color and sound—lanterns drifting on the tide, impromptu shanty contests, and feasts " +
                                "where the aroma of freshly caught seafood mingles with exotic spices. Each tradition is a tribute to the sea's " +
                                "capricious beauty and the unbreakable bonds it forges among kindred souls."
                            );
                            traditionsModule.AddOption("Tell me more about the shanty contests.", 
                                p => true,
                                p =>
                                {
                                    DialogueModule shantyContestModule = new DialogueModule(
                                        "Minstrels from every corner compete with verses born of both sorrow and joy. Their songs recount lost loves, " +
                                        "heroic rescues, and the ceaseless march of time. Victory in a shanty contest is rewarded not just with praise, " +
                                        "but with relics and whispers of forgotten lore."
                                    );
                                    p.SendGump(new DialogueGump(p, shantyContestModule));
                                });
                            traditionsModule.AddOption("What of the lantern processions?", 
                                p => true,
                                p =>
                                {
                                    DialogueModule lanternModule = new DialogueModule(
                                        "During the lantern processions, the shoreline transforms into a corridor of flickering light—each lantern " +
                                        "a symbol of a lost soul or a hope rekindled. The procession is a moving tribute to the sea, a prayer for safe passage " +
                                        "and bountiful fortune."
                                    );
                                    p.SendGump(new DialogueGump(p, lanternModule));
                                });
                            pl.SendGump(new DialogueGump(pl, traditionsModule));
                        });
                    player.SendGump(new DialogueGump(player, festivalModule));
                });

            // Option 2: Maritime Legends with Kara Salt
            greeting.AddOption("Share with me the maritime legends you discussed with Kara Salt.", 
                player => true,
                player =>
                {
                    DialogueModule legendsModule = new DialogueModule(
                        "Maritime legends are as deep and shifting as the ocean itself. Kara Salt and I have spent countless nights " +
                        "exchanging stories—tales of ghost ships adrift in eerie mists and sacred deities who watch over lost sailors. " +
                        "Which legend calls to you: the sorrowful ghost ship or the benevolent spirit Thalassa?"
                    );
                    legendsModule.AddOption("Tell me about the ghost ship.", 
                        p => true,
                        p =>
                        {
                            DialogueModule ghostShipModule = new DialogueModule(
                                "The ghost ship drifts through fog-cloaked nights, its tattered sails aglow with an unearthly luminescence. " +
                                "It is said that her crew is cursed to wander the brine, forever lamenting their shattered dreams. " +
                                "Would you like to know about the curse that binds them or the eerie encounters of those who have seen her?"
                            );
                            ghostShipModule.AddOption("What curse binds the crew?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule curseModule = new DialogueModule(
                                        "Long ago, betrayal and hubris doomed the ship’s master. In retribution, a vengeful spirit cursed the vessel, " +
                                        "condemning its crew to an eternal voyage through shadow and mist—a fate that serves as a dire warning to all who dare defy the sea."
                                    );
                                    pp.SendGump(new DialogueGump(pp, curseModule));
                                });
                            ghostShipModule.AddOption("Describe the eerie encounters.", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule encountersModule = new DialogueModule(
                                        "Many sailors recount a sudden hush amid roaring storms, as if time itself stilled. " +
                                        "They speak of ghostly figures on deck, of soft, mournful tunes rising from the deep, and of a chill that seeps into the soul."
                                    );
                                    pp.SendGump(new DialogueGump(pp, encountersModule));
                                });
                            p.SendGump(new DialogueGump(p, ghostShipModule));
                        });
                    legendsModule.AddOption("Tell me about Thalassa, the sea deity.", 
                        p => true,
                        p =>
                        {
                            DialogueModule deityModule = new DialogueModule(
                                "Thalassa is revered as the nurturing spirit of the ocean—both merciful and formidable. " +
                                "Kara Salt tells tales of her miraculous interventions and the sacred rituals performed in her honor. " +
                                "Would you like to learn about the ancient ceremonies or the miracles attributed to her grace?"
                            );
                            deityModule.AddOption("Describe the sacred ceremonies.", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule ritualsModule = new DialogueModule(
                                        "Ceremonies in Thalassa's name involve casting luminous offerings into the deep, reciting chants " +
                                        "passed down through generations, and lighting candles that flicker like the pulse of the ocean. " +
                                        "These rituals are believed to invoke her benevolence and protect seafarers from the wrath of the deep."
                                    );
                                    pp.SendGump(new DialogueGump(pp, ritualsModule));
                                });
                            deityModule.AddOption("What miracles are ascribed to her?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule miraclesModule = new DialogueModule(
                                        "Legends claim that Thalassa once quelled a tempest that threatened an entire fleet, guiding lost ships " +
                                        "to safety against all odds. Her interventions are shrouded in mystery, whispered as both warnings and blessings."
                                    );
                                    pp.SendGump(new DialogueGump(pp, miraclesModule));
                                });
                            p.SendGump(new DialogueGump(p, deityModule));
                        });
                    player.SendGump(new DialogueGump(player, legendsModule));
                });

            // Option 3: Sea Shanties and Lore with Kess
            greeting.AddOption("I’d love to hear a sea shanty and learn about your lore exchanges with Kess.", 
                player => true,
                player =>
                {
                    DialogueModule shantyModule = new DialogueModule(
                        "Ah, the sea shanties! Kess and I have composed verses that echo the trials and triumphs of our maritime journeys. " +
                        "Would you like to hear one of our stirring shanties or delve into the lore behind these timeless melodies?"
                    );
                    shantyModule.AddOption("Sing me one of your shanties.", 
                        p => true,
                        p =>
                        {
                            DialogueModule verseModule = new DialogueModule(
                                "Listen well:\n\n" +
                                "'The storm may rage and the night be long,\n" +
                                "Yet in our hearts burns a steadfast song.\n" +
                                "Through waves that crash and winds that cry,\n" +
                                "Our spirits soar, for hope will never die.'\n\n" +
                                "Each note is imbued with our shared memories and the wisdom of the deep."
                            );
                            p.SendGump(new DialogueGump(p, verseModule));
                        });
                    shantyModule.AddOption("Explain the lore behind these shanties.", 
                        p => true,
                        p =>
                        {
                            DialogueModule loreModule = new DialogueModule(
                                "Our shanties are born from the trials of the sea—epic journeys, fierce battles against nature, and quiet moments of introspection. " +
                                "Kess often reminds me that every verse carries lessons of resilience, honor, and the enduring power of hope."
                            );
                            loreModule.AddOption("What lessons do they impart?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule lessonsModule = new DialogueModule(
                                        "They speak of enduring hardship with grace, of finding light in the darkest tempests, " +
                                        "and of the timeless strength that lies in unity and trust. Each melody is a lesson in perseverance."
                                    );
                                    pp.SendGump(new DialogueGump(pp, lessonsModule));
                                });
                            loreModule.AddOption("How do you craft such verses?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule craftingModule = new DialogueModule(
                                        "The crafting of a shanty is both art and instinct. We wander the shores, listen to the whispers " +
                                        "of the ocean, and let our hearts dictate the rhythm. It is a dialogue between our souls and the ever-changing sea."
                                    );
                                    pp.SendGump(new DialogueGump(pp, craftingModule));
                                });
                            p.SendGump(new DialogueGump(p, loreModule));
                        });
                    player.SendGump(new DialogueGump(player, shantyModule));
                });

            // Option 4: Adventures as a Coastal Minstrel
            greeting.AddOption("Tell me about your adventures as a coastal minstrel.", 
                player => true,
                player =>
                {
                    DialogueModule adventuresModule = new DialogueModule(
                        "My life is a ceaseless voyage along rugged coasts and serene bays. Every port, every storm, " +
                        "carries its own tale. Would you like to hear about a particularly memorable voyage or understand the philosophy that guides my every step?"
                    );
                    adventuresModule.AddOption("Share a memorable voyage.", 
                        p => true,
                        p =>
                        {
                            DialogueModule voyageModule = new DialogueModule(
                                "I recall a night when a furious storm swept us into unknown waters. " +
                                "Amid the chaos, I discovered a hidden cove bathed in an ethereal glow, " +
                                "where the ocean whispered its deepest secrets. That night, I learned that beauty often lies " +
                                "in the heart of turmoil."
                            );
                            p.SendGump(new DialogueGump(p, voyageModule));
                        });
                    adventuresModule.AddOption("What philosophy guides you?", 
                        p => true,
                        p =>
                        {
                            DialogueModule philosophyModule = new DialogueModule(
                                "I believe the sea is a relentless teacher. It teaches us to be resilient, " +
                                "to embrace our duty even when the path is shrouded in darkness, and to seek wisdom " +
                                "in every fleeting moment of calm. This philosophy is the compass that guides my journey."
                            );
                            p.SendGump(new DialogueGump(p, philosophyModule));
                        });
                    player.SendGump(new DialogueGump(player, adventuresModule));
                });

            // Option 5: The Secret Duty
            greeting.AddOption("I sense a hidden burden in you. Tell me about your secret duty.", 
                player => true,
                player =>
                {
                    DialogueModule secretModule = new DialogueModule(
                        "Your intuition serves you well. Beneath these melodies and tales lies a sacred duty I have carried in silence. " +
                        "Long ago, I was chosen as the guardian of an ancient shrine—a sanctum steeped in forgotten lore and mystical power. " +
                        "Bound by an oath under a blood-red moon, I am tasked with protecting this shrine until a successor, pure of heart and " +
                        "wise beyond measure, emerges. What would you like to know about this hidden charge?"
                    );
                    // Sub-option: The Ancient Shrine
                    secretModule.AddOption("Tell me about the ancient shrine.", 
                        p => true,
                        p =>
                        {
                            DialogueModule shrineModule = new DialogueModule(
                                "The shrine lies secluded in a rugged cove, its weathered stones etched with cryptic runes that hum with ancient power. " +
                                "It is a place where time stands still, a sanctuary where the wisdom of ages is preserved. " +
                                "Many seek its secrets, yet few are deemed worthy of its truth."
                            );
                            shrineModule.AddOption("What do the runes reveal?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule runesModule = new DialogueModule(
                                        "The runes speak in whispers of creation and decay, of cycles that bind all life to the eternal rhythm of the earth and sea. " +
                                        "They hint at prophecies and hidden truths, meant only for those who possess true insight."
                                    );
                                    pp.SendGump(new DialogueGump(pp, runesModule));
                                });
                            shrineModule.AddOption("Can anyone access the shrine?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule accessModule = new DialogueModule(
                                        "Access is granted only to those who embody wisdom, courage, and selflessness. " +
                                        "The shrine tests the spirit, ensuring that only a pure heart may step into its sanctified embrace."
                                    );
                                    pp.SendGump(new DialogueGump(pp, accessModule));
                                });
                            p.SendGump(new DialogueGump(p, shrineModule));
                        });
                    // Sub-option: The Oath
                    secretModule.AddOption("What oath binds you to this duty?", 
                        p => true,
                        p =>
                        {
                            DialogueModule oathModule = new DialogueModule(
                                "I swore my oath beneath a blood-red moon, in a ritual as old as the sea itself. " +
                                "The oath demands that I remain ever-vigilant, stoic in my resolve and dutiful in my actions, " +
                                "protecting the shrine against those who would defile its sacred legacy. " +
                                "It is a bond of honor that weighs heavy upon my soul."
                            );
                            oathModule.AddOption("Do you ever long to be free of it?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule freedomModule = new DialogueModule(
                                        "There are moments, in the quiet solitude of night, when the weight of the oath seems almost unbearable. " +
                                        "Yet, I find strength in knowing that every sacrifice brings us closer to the day when a worthy successor appears— " +
                                        "someone to carry this torch of ancient wisdom forward."
                                    );
                                    pp.SendGump(new DialogueGump(pp, freedomModule));
                                });
                            oathModule.AddOption("How was this oath bestowed upon you?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule bestowModule = new DialogueModule(
                                        "Legend tells that during a violent tempest, as the heavens roared and the sea unleashed its fury, " +
                                        "a radiant light engulfed me before the shrine. In that transcendent moment, I was chosen—my fate sealed " +
                                        "by the sacred covenant. The experience was both humbling and awe-inspiring, forever altering the course of my life."
                                    );
                                    bestowModule.AddOption("What emotions did that moment stir in you?", 
                                        ppp => true,
                                        ppp =>
                                        {
                                            DialogueModule emotionModule = new DialogueModule(
                                                "In that moment, I felt a tempest of emotions—fear, reverence, and an overwhelming sense of duty. " +
                                                "It was as if the very essence of the sea had imprinted its ancient wisdom upon my soul, " +
                                                "guiding me on a path of unyielding vigilance."
                                            );
                                            ppp.SendGump(new DialogueGump(ppp, emotionModule));
                                        });
                                    pp.SendGump(new DialogueGump(pp, bestowModule));
                                });
                            p.SendGump(new DialogueGump(p, oathModule));
                        });
                    // Sub-option: The Trials of Duty
                    secretModule.AddOption("What trials test your resolve as guardian?", 
                        p => true,
                        p =>
                        {
                            DialogueModule trialsModule = new DialogueModule(
                                "Every day brings its own trial—a battle against encroaching shadows, the relentless solitude of endless nights, " +
                                "and the ever-present threat of those who would seek to claim the shrine’s power. " +
                                "These trials are both physical and spiritual, each one a lesson in fortitude and honor. " +
                                "Would you like to hear of a recent trial or understand the nature of these perpetual tests?"
                            );
                            trialsModule.AddOption("Describe a recent trial.", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule recentTrialModule = new DialogueModule(
                                        "Not long ago, a band of rogue mystics attempted to breach the shrine’s sanctum, seeking to harness its forbidden power. " +
                                        "In the ensuing clash, I stood resolute, drawing upon ancient techniques and the wisdom of the ages to repel the invaders. " +
                                        "The confrontation was fierce, leaving both scars and renewed determination in its wake."
                                    );
                                    pp.SendGump(new DialogueGump(pp, recentTrialModule));
                                });
                            trialsModule.AddOption("What is the essence of these tests?", 
                                pp => true,
                                pp =>
                                {
                                    DialogueModule natureTrialModule = new DialogueModule(
                                        "The trials challenge me to remain stoic in the face of overwhelming odds, " +
                                        "to act with unwavering duty, and to seek wisdom even when darkness surrounds me. " +
                                        "Each challenge is a crucible, refining my spirit and ensuring I remain a worthy guardian of the sacred trust."
                                    );
                                    pp.SendGump(new DialogueGump(pp, natureTrialModule));
                                });
                            p.SendGump(new DialogueGump(p, trialsModule));
                        });
                    player.SendGump(new DialogueGump(player, secretModule));
                });

            // Option 6: Farewell
            greeting.AddOption("Farewell.", 
                player => true,
                player =>
                {
                    DialogueModule farewellModule = new DialogueModule(
                        "May the tides guide you safely, traveler, and may your journey be enriched with both song and wisdom. " +
                        "Until our paths cross again, remember that every burden carried with honor becomes a beacon for those who follow."
                    );
                    player.SendGump(new DialogueGump(player, farewellModule));
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version number
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
