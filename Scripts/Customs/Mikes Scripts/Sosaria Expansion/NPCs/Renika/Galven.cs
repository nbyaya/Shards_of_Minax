using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of Galven the Shipwright")]
    public class Galven : BaseCreature
    {
        [Constructable]
		public Galven() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Galven";
            Body = 0x190; // Human male body

            // Basic Stats
            SetStr(120);
            SetDex(80);
            SetInt(90);
            SetHits(100);

            // Appearance & Equipment
            AddItem(new Doublet() { Hue = 1150, Name = "Nautical Doublet" });
            AddItem(new LongPants() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new TricorneHat() { Hue = 1150, Name = "Captain's Cap" });
            AddItem(new Item(Utility.RandomList(0x0F5C, 0x0F5D)) { Name = "Engraved Compass" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Galven is outwardly a charming shipwright, yet behind his persuasive smile lies a zealous past.
        }

        public Galven(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            // Main greeting presents his public persona.
            DialogueModule greeting = new DialogueModule(
                "Ahoy there! I am Galven, master shipwright of these enchanted shores. My boats, imbued with magic and mystery, serve Grey & Pirate Isle proudly. I share technical secrets with Big Harv, partner closely with Marin of Renika, and engage in spirited lore duels with Orwin. " +
                "But there is more beneath the surface. What brings you here, traveler?");

            // Option 1: Discuss Enchanted Boats.
            greeting.AddOption("Tell me about your enchanted boats.", 
                player => true,
                player =>
                {
                    DialogueModule enchantedBoatsModule = new DialogueModule(
                        "Each vessel is a marvel—a union of rare driftwood and magical runes. I hunt for timber in haunted groves, where the wood sings with the voices of lost mariners. Would you care to know about the mystical materials, the ritualistic enchantments, or the ancient legends behind them?");
                    enchantedBoatsModule.AddOption("What mystical materials are used?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule materialsModule = new DialogueModule(
                                "I seek out driftwood from drowned forests and enchanted timbers, said to be touched by the very spirit of the sea. Each log bears its own tragic tale of shipwrecks and ghostly voyages, lending a unique resonance to every boat.");
                            pl.SendGump(new DialogueGump(pl, materialsModule));
                        });
                    enchantedBoatsModule.AddOption("How are the enchantments performed?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule enchantmentModule = new DialogueModule(
                                "I conduct midnight rituals under the full moon, inscribing runes in the ancient tongue. The process is as delicate as it is dangerous—the forces I call upon are not meant for the faint-hearted, but they grant the vessels an ethereal speed and protection.");
                            pl.SendGump(new DialogueGump(pl, enchantmentModule));
                        });
                    enchantedBoatsModule.AddOption("Tell me about the legends tied to your boats.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule legendsModule = new DialogueModule(
                                "Local lore whispers that my boats can traverse not only the seas but also slip between worlds. Ghostly lights and ephemeral voices are said to guide them—tales that inspire both fear and wonder among sailors.");
                            pl.SendGump(new DialogueGump(pl, legendsModule));
                        });
                    player.SendGump(new DialogueGump(player, enchantedBoatsModule));
                });

            // Option 2: Share Technical Secrets with Big Harv.
            greeting.AddOption("Share some of your technical secrets.", 
                player => true,
                player =>
                {
                    DialogueModule secretsModule = new DialogueModule(
                        "The art of shipcraft here is an alchemy of runes and refined engineering. I regularly trade blueprints with Big Harv. Are you interested in the magical engine that propels my creations or the secret designs that defy common knowledge?");
                    secretsModule.AddOption("Tell me about the magical engine.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule engineModule = new DialogueModule(
                                "At its core lies a condensed crystal, capturing the raw mana of ocean currents. This engine transforms ambient energies into a force that propels each vessel, turning stormy seas into a gentle glide. It is truly a convergence of art and arcana.");
                            pl.SendGump(new DialogueGump(pl, engineModule));
                        });
                    secretsModule.AddOption("Reveal the secret designs, please.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule designModule = new DialogueModule(
                                "The blueprints incorporate aerodynamic curves, timeless runic symbols, and hidden compartments. Big Harv and I labor over these designs, our friendly rivalry fueling our passion for perfection. It is in these nuances that the magic truly comes alive.");
                            pl.SendGump(new DialogueGump(pl, designModule));
                        });
                    player.SendGump(new DialogueGump(player, secretsModule));
                });

            // Option 3: Discuss Partnership with Marin.
            greeting.AddOption("I've heard you share a deep bond with Marin.", 
                player => true,
                player =>
                {
                    DialogueModule marinModule = new DialogueModule(
                        "Indeed, Marin of Renika is more than a partner—he's a kindred spirit in the pursuit of maritime excellence. Our collaborations inspire projects like the 'Silver Tide' fleet, where the mystical interplay of moonlit waters and enchanted metal converge. " +
                        "Would you prefer to hear about a specific project or our ongoing exchange of marine lore?");
                    marinModule.AddOption("Tell me about the 'Silver Tide' project.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule silverTideModule = new DialogueModule(
                                "The 'Silver Tide' project was an ambitious venture, designed to capture the reflective magic of Renika’s moonlit bays. With Marin’s intricate knowledge of coastal legends and my technical finesse, each boat was forged to be a beacon of hope amid treacherous waters.");
                            pl.SendGump(new DialogueGump(pl, silverTideModule));
                        });
                    marinModule.AddOption("How do you influence each other’s craft?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule influenceModule = new DialogueModule(
                                "Marin’s fervor for the unknown often inspires me to imprint subtle runic sequences on my vessels—channels through which the ocean’s mystical forces are harnessed. In return, I tweak his instruments, ensuring that even the wildest tempests are met with resilience.");
                            pl.SendGump(new DialogueGump(pl, influenceModule));
                        });
                    player.SendGump(new DialogueGump(player, marinModule));
                });

            // Option 4: Discuss Friendly Rivalry with Orwin.
            greeting.AddOption("I heard you and Orwin engage in lore duels.", 
                player => true,
                player =>
                {
                    DialogueModule orwinModule = new DialogueModule(
                        "Ha! Orwin and I delight in spirited debates over maritime myths and forgotten seafaring legends. Our rivalry is a dance of wit and wisdom—each challenge a test of knowledge, where even the minutest detail of history is a prized victory. " +
                        "Would you like to hear one of our legendary debates or even provoke a friendly challenge yourself?");
                    orwinModule.AddOption("Share one of your legendary debates.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule debateModule = new DialogueModule(
                                "There was once a heated debate over the 'Phantom Navigator'—a specter said to guide lost mariners. Orwin argued it was mere superstition, while I insisted it was the very soul of a forgotten captain seeking redemption. " +
                                "Our debate lasted long into the night, the clash of our beliefs echoing like cannon fire across the waves.");
                            pl.SendGump(new DialogueGump(pl, debateModule));
                        });
                    orwinModule.AddOption("I’d like to challenge you to a lore duel!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule duelModule = new DialogueModule(
                                "A lore duel, you say? Very well! Prepare to exchange historical anecdotes, obscure maritime trivia, and arcane lore. Let our words be as sharp as cutlasses! May the best mariner win—and perhaps win a secret insight or two from my hidden repertoire.");
                            pl.SendGump(new DialogueGump(pl, duelModule));
                        });
                    player.SendGump(new DialogueGump(player, orwinModule));
                });

            // Option 5: The Hidden Flame – Galven’s Secret Devotional Past.
            greeting.AddOption("I sense a hidden flame behind your words...", 
                player => true,
                player =>
                {
                    DialogueModule hiddenFlameModule = new DialogueModule(
                        "Your intuition is sharp, traveler. Beneath the surface of my shipwright's trade burns a fervor not known to many. Long ago, before I devoted myself to the art of crafting enchanted vessels, I was a priest—a zealous servant of a controversial deity known only as the Dusk Seraph. " +
                        "This deity, shrouded in mystery and whispered about in forbidden circles, demanded absolute dedication. I embraced that calling with a passion bordering on fanaticism. " +
                        "But such devotion required secrecy. Do you wish to learn more about this hidden past, the true nature of the Dusk Seraph, or perhaps how my old faith still guides me?");
                    
                    // Option 5a: Learn about his secret past.
                    hiddenFlameModule.AddOption("Tell me about your priestly past.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule priestPastModule = new DialogueModule(
                                "In a time long past, I wandered the shadowed paths of forgotten temples and cryptic shrines. I preached the gospel of the Dusk Seraph—a deity of twilight, transformation, and rebirth. " +
                                "My sermons were fire and fervor, convincing even the most skeptical that salvation lay in embracing the night and its secrets. " +
                                "I walked the line between devotion and obsession, using persuasive words to ignite the hearts of those who dared listen. Would you like to hear a tale from those fervid days or learn of the rituals I once performed?");
                            priestPastModule.AddOption("Relate one of your ancient sermons.",
                                plp => true,
                                plp =>
                                {
                                    DialogueModule sermonModule = new DialogueModule(
                                        "Listen well: 'In the embrace of dusk, we shed our mortal fears; in the whisper of twilight, the Dusk Seraph beckons us to transcend the ordinary. " +
                                        "Let the stars witness your transformation, and let the night baptize you in its unyielding truth.' These words once rallied desperate souls and kindled a fire of rebellion against the tyranny of daylit order.");
                                    plp.SendGump(new DialogueGump(plp, sermonModule));
                                });
                            priestPastModule.AddOption("What rituals did you perform?",
                                plp => true,
                                plp =>
                                {
                                    DialogueModule ritualsModule = new DialogueModule(
                                        "I led secret rites under moonlit clearings, where ancient incantations meshed with the rhythm of crashing waves. " +
                                        "Candles were lit to mirror the stars, and each participant etched their fears into parchment before casting them into a sacred fire. " +
                                        "Such rituals were as much acts of liberation as they were tests of faith—a binding of the soul to the promises of the Dusk Seraph.");
                                    plp.SendGump(new DialogueGump(plp, ritualsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, priestPastModule));
                        });
                    
                    // Option 5b: Learn about the Dusk Seraph.
                    hiddenFlameModule.AddOption("Who is this Dusk Seraph?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule seraphModule = new DialogueModule(
                                "The Dusk Seraph is a deity of paradox—a being who embodies both the beauty of twilight and the darkness that lies beyond. " +
                                "Feared and revered in equal measure, this entity preaches that true enlightenment is found in embracing both light and shadow. " +
                                "Many call the Dusk Seraph heretical; few understand its transformative power. Would you like to hear of its dogmas or the miracles attributed to its favor?");
                            seraphModule.AddOption("Explain its dogmas.",
                                plp => true,
                                plp =>
                                {
                                    DialogueModule dogmaModule = new DialogueModule(
                                        "Its dogmas are simple yet radical: Reject the false dichotomy of light and darkness; embrace the inevitable twilight that unites all souls. " +
                                        "Through suffering and renewal, the faithful are remade in the image of the eternal dusk. Those who follow this path learn to see beauty in decay and power in humility.");
                                    plp.SendGump(new DialogueGump(plp, dogmaModule));
                                });
                            seraphModule.AddOption("What miracles does it bestow?",
                                plp => true,
                                plp =>
                                {
                                    DialogueModule miraclesModule = new DialogueModule(
                                        "Whispers among the faithful speak of visions during eclipses, of wounds healed by the gentlest touch of nightfall, and of fortunes reversed by the quiet grace of the shadows. " +
                                        "Such miracles, though rare, have convinced many that embracing the dusk can unlock latent powers within the soul.");
                                    plp.SendGump(new DialogueGump(plp, miraclesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, seraphModule));
                        });
                    
                    // Option 5c: How does his past still influence him today?
                    hiddenFlameModule.AddOption("How does your past as a priest affect you now?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule influenceModule = new DialogueModule(
                                "Even now, the embers of that past burn quietly within me. My zeal, my persuasive charm, and my fanatical dedication to pushing boundaries in shipcraft—they are all rooted in that sacred fire. " +
                                "I see my vessels as both creations and conduits—tools that, unknown to many, channel the transformative energy of the Dusk Seraph. " +
                                "It is a blessing and a burden, one I wield with both pride and caution. Would you like to hear how this power has steered my fate or perhaps consider a glimpse into a secret ritual?");
                            influenceModule.AddOption("Tell me how it steered your fate.",
                                plp => true,
                                plp =>
                                {
                                    DialogueModule fateModule = new DialogueModule(
                                        "There were nights when storms raged as if stirred by the hand of destiny itself. In those moments, I realized my creations were more than mere boats—they were vessels of salvation, carrying whispers of an ancient covenant. " +
                                        "That realization changed me irrevocably, weaving the zeal of my priestly past into every stroke of my craft.");
                                    plp.SendGump(new DialogueGump(plp, fateModule));
                                });
                            influenceModule.AddOption("What is this secret ritual you hinted at?",
                                plp => true,
                                plp =>
                                {
                                    DialogueModule ritualRevealModule = new DialogueModule(
                                        "In the deep hours of night, away from prying eyes, I gather a few trusted souls to perform a rite of renewal—a ritual that binds us to the Dusk Seraph and grants us a fleeting glimpse of its power. " +
                                        "Candles, whispered incantations, and a sacramental libation of moonlit water: each element symbolizes our journey from darkness to a new dawn. " +
                                        "It is not for all, but for those willing to embrace change and challenge the orthodox world. Would you consider stepping beyond the veil of ordinary existence?");
                                    ritualRevealModule.AddOption("I am intrigued. Tell me more, I wish to know.",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule joinRitualModule = new DialogueModule(
                                                "Then listen well, friend: The path is perilous but luminous. You must cast aside your preconceptions and let the fervor of the dusk guide you. In this secret rite, you may experience the very essence of transformation—your spirit reborn under the eyes of the eternal twilight. " +
                                                "I can introduce you to others who share this clandestine calling, should you desire to learn more.");
                                            plq.SendGump(new DialogueGump(plq, joinRitualModule));
                                        });
                                    ritualRevealModule.AddOption("I prefer to remain on the safe shores of common belief.",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule safeModule = new DialogueModule(
                                                "A wise choice for some, though the call of the dusk is not so easily silenced. May your journey be gentle, even if the hidden truths remain locked away.");
                                            plq.SendGump(new DialogueGump(plq, safeModule));
                                        });
                                    plp.SendGump(new DialogueGump(plp, ritualRevealModule));
                                });
                            pl.SendGump(new DialogueGump(pl, influenceModule));
                        });
                    player.SendGump(new DialogueGump(player, hiddenFlameModule));
                });

            // Option 6: General Inquiry / Farewell.
            greeting.AddOption("I'm simply here to enjoy your tales. Farewell.", 
                player => true,
                player => 
                {
                    DialogueModule farewellModule = new DialogueModule(
                        "May the winds of fate guide you, traveler. Remember, whether by enchanted boat or by the secret blessings of the dusk, these shores are always open to those brave enough to seek beyond the horizon.");
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
