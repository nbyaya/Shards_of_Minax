using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Mino : BaseCreature
    {
        [Constructable]
        public Mino() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mino, the Alchemist";
            Body = 0x190; // Human body

            // Stats
            SetStr(100);
            SetDex(70);
            SetInt(120);
            SetHits(100);

            // Appearance & Equipment – styled as a scholarly alchemist with hints of his magical past
            AddItem(new Robe() { Hue = 1750 });
            AddItem(new Sandals() { Hue = 1150 });
            // A cherished item from his early days
            Item batteredSpellbook = new Item(0x1F2D) { Name = "A Battered Spellbook of Apprentice Days" };
            AddItem(batteredSpellbook);

            // Randomized skin and hair for a natural look
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Mino(Serial serial) : base(serial)
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
            DialogueModule greeting = new DialogueModule(
                "Greetings, seeker of hidden truths! I am Mino, the alchemist who dares to decipher forbidden lore from the mystical texts of Fawn. " +
                "My days are filled with fervent research alongside Cassian’s insightful guidance and covert recipe exchanges with Thistle of Devil Guard. " +
                "Not to mention, my impassioned debates with Jonas of West Montor—each conversation sparking new theories that electrify my experiments. " +
                "But behind these pursuits lies a secret: in my youth, I was an apprentice mage, brimming with enthusiasm and curiosity, " +
                "often stumbling into magical mishaps with my clumsy spells. What would you like to explore today?"
            );

            // Option 1: Forbidden Texts
            greeting.AddOption("Tell me about your forbidden texts from Fawn.",
                player => true,
                player =>
                {
                    DialogueModule textsModule = new DialogueModule(
                        "Ah, the forbidden texts! These ancient scrolls and grimoires are more than relics—they are living memories of a time when magic danced freely. " +
                        "I acquired them through daring escapades in Fawn’s hidden archives, often aided by Cassian’s skill in deciphering esoteric runes. " +
                        "Would you like to know how I risked life and limb to recover them, or shall I share the secrets they whisper?"
                    );
                    textsModule.AddOption("How did you risk your life to acquire them?",
                        p => true,
                        p =>
                        {
                            DialogueModule acquisitionModule = new DialogueModule(
                                "It was a perilous night when, guided only by moonlight and my burning curiosity, I ventured into a long-forgotten library beneath Fawn. " +
                                "Within those damp, crumbling walls, ancient wards and spectral custodians tested my resolve. I narrowly escaped a trap that nearly ensnared my very soul. " +
                                "Cassian’s timely advice saved me when deciphering the protective sigils—each symbol a promise of forbidden power."
                            );
                            p.SendGump(new DialogueGump(p, acquisitionModule));
                        });
                    textsModule.AddOption("What secrets do these texts reveal?",
                        p => true,
                        p =>
                        {
                            DialogueModule secretModule = new DialogueModule(
                                "Each parchment hums with tales of transmutation, of elixirs capable of mending or cursing fate itself. " +
                                "They reveal long-lost recipes for potions that can grant temporary invulnerability or expose hidden realms. " +
                                "Cassian and I still argue over the true meaning behind these cryptic inscriptions, convinced that each misinterpretation unveils a new mystery."
                            );
                            // Additional nested branch: deeper inquiry on potion secrets.
                            secretModule.AddOption("Tell me more about these mysterious elixirs.",
                                pp => true,
                                pp =>
                                {
                                    DialogueModule elixirModule = new DialogueModule(
                                        "Picture a potion so volatile that a single misstep during its creation sends it boiling over in a burst of unpredictable magic. " +
                                        "I once nearly turned an entire workshop into an impromptu fireworks display! But in those moments of chaotic brilliance, true breakthroughs are made."
                                    );
                                    pp.SendGump(new DialogueGump(pp, elixirModule));
                                });
                            p.SendGump(new DialogueGump(p, secretModule));
                        });
                    player.SendGump(new DialogueGump(player, textsModule));
                });

            // Option 2: Secret Recipes with Thistle
            greeting.AddOption("Tell me about your secret recipes with Thistle.",
                player => true,
                player =>
                {
                    DialogueModule recipesModule = new DialogueModule(
                        "Ah, the clandestine art of recipe exchange! In the depths of Devil Guard, amidst haunted mines and hot springs infused with elemental energies, Thistle procures ingredients that defy mortal understanding. " +
                        "Our secret concoctions are born of hazardous experiments—each a symphony of volatile reactions and precise timing. " +
                        "Shall I reveal the rare ingredients we dare to blend, or narrate the elaborate process that turns them into transformative elixirs?"
                    );
                    recipesModule.AddOption("What rare ingredients do you combine?",
                        p => true,
                        p =>
                        {
                            DialogueModule ingredientsModule = new DialogueModule(
                                "Our ingredients range from luminescent fungi harvested in the eerie darkness of the Mines of Minax, " +
                                "to crystalline shards from Devil Guard’s spectral caverns, even dew distilled under the first light in Yew’s enchanted groves. " +
                                "Each component is as capricious as it is potent—commands respect and a steady hand, lest they wreak untold havoc."
                            );
                            // Further nested detail on individual ingredient mishaps.
                            ingredientsModule.AddOption("Have these ingredients ever backfired on you?",
                                pp => true,
                                pp =>
                                {
                                    DialogueModule mishapModule = new DialogueModule(
                                        "Oh, indeed! Once, an overenthusiastic attempt to mix a rare moonlit nectar with glacial crystals resulted in an explosion that singed my eyebrows off. " +
                                        "Thankfully, Thistle’s quick thinking (and a dash of his own mystical elixir) saved not only my visage but also the integrity of our research."
                                    );
                                    pp.SendGump(new DialogueGump(pp, mishapModule));
                                });
                            p.SendGump(new DialogueGump(p, ingredientsModule));
                        });
                    recipesModule.AddOption("What is your secret process?",
                        p => true,
                        p =>
                        {
                            DialogueModule processModule = new DialogueModule(
                                "Imagine an orchestra where each instrument plays in perfect disarray—that is my alchemical process. " +
                                "I meticulously blend volatile reagents, all while reading the subtle cues of their reactions. " +
                                "Thistle ensures that the catalyst is added at the precise moment of kinetic equilibrium. " +
                                "There have been moments when my clumsiness almost threw off the entire symphony, but those very missteps have taught me invaluable lessons in balancing chaos with order."
                            );
                            // Nested branch exploring lessons learned.
                            processModule.AddOption("What lessons have you learned from your mistakes?",
                                pp => true,
                                pp =>
                                {
                                    DialogueModule lessonsModule = new DialogueModule(
                                        "Every mishap is a lesson carved into the annals of my memory. " +
                                        "I learned that precision and patience are the twin pillars of mastery, and that even in failure, there lies the spark of innovation. " +
                                        "It is this blend of enthusiasm and humility that now guides my every experiment."
                                    );
                                    pp.SendGump(new DialogueGump(pp, lessonsModule));
                                });
                            p.SendGump(new DialogueGump(p, processModule));
                        });
                    player.SendGump(new DialogueGump(player, recipesModule));
                });

            // Option 3: Debates and Theories with Jonas
            greeting.AddOption("I heard you often compete with Jonas on magical theories. How does that rivalry work?",
                player => true,
                player =>
                {
                    DialogueModule competitionModule = new DialogueModule(
                        "Ah, Jonas of West Montor! Our debates are veritable duels of intellect and passion. " +
                        "While I delve into the immutable secrets hidden in ancient texts, Jonas proposes revolutionary ideas that upend conventional alchemy. " +
                        "Our discussions are a delightful clash of tradition versus innovation. " +
                        "Would you care to hear about one of our most feverish debates, or perhaps the latest theory stirring our experimental cauldrons?"
                    );
                    competitionModule.AddOption("Tell me about one of your fiercest debates with Jonas.",
                        p => true,
                        p =>
                        {
                            DialogueModule debateModule = new DialogueModule(
                                "One memorable evening, under a sky lit by a rare comet, Jonas argued that magic is an ever-evolving force—a living, breathing entity. " +
                                "I, however, maintained that a primordial, unchanging essence underpinned all alchemy. " +
                                "Our voices, raised in passionate discourse, resonated through the halls of our makeshift laboratory, drawing a small crowd of fellow mages and scholars. " +
                                "Though no victor emerged, that night deepened our mutual respect and pushed both our theories to new heights."
                            );
                            // Additional nested branch about the comet incident.
                            debateModule.AddOption("What about the comet incident? That sounds dramatic.",
                                pp => true,
                                pp =>
                                {
                                    DialogueModule cometModule = new DialogueModule(
                                        "Oh, the comet! It bathed our workshop in an ethereal glow, amplifying the energies of our failed experiments. " +
                                        "In the frenzy, I accidentally unleashed a burst of chaotic magic that sent my notes flying—and nearly set the floor alight! " +
                                        "It was a chaotic, yet unforgettable testament to the raw, unpredictable power of magic."
                                    );
                                    pp.SendGump(new DialogueGump(pp, cometModule));
                                });
                            p.SendGump(new DialogueGump(p, debateModule));
                        });
                    competitionModule.AddOption("What is the latest theory you are both exploring?",
                        p => true,
                        p =>
                        {
                            DialogueModule theoryModule = new DialogueModule(
                                "Recently, we have been toying with the concept of 'Elemental Resonance'—the idea that every element vibrates with a unique frequency. " +
                                "Jonas dreams of harnessing these vibrations to manifest miraculous transformations, while I argue for a delicate synthesis between ancient ritual and modern experimentation. " +
                                "This ongoing debate fuels our every experiment, challenging us to break the boundaries of known alchemy."
                            );
                            p.SendGump(new DialogueGump(p, theoryModule));
                        });
                    player.SendGump(new DialogueGump(player, competitionModule));
                });

            // Option 4: His Personal Journey & Secret Past as an Apprentice Mage
            greeting.AddOption("Tell me about your personal journey and secret past as an apprentice mage.",
                player => true,
                player =>
                {
                    DialogueModule journeyModule = new DialogueModule(
                        "My journey is one woven with both triumph and calamity. In my earliest days, I was not just an alchemist but an apprentice mage—ever so enthusiastic, insatiably curious, yet admittedly clumsy. " +
                        "I remember the fateful moment when an ill-timed levitation spell sent me tumbling from a towering shelf in the castle library. " +
                        "Despite that embarrassing mishap, each spell, each failed incantation, taught me the value of perseverance and tempered my zeal with wisdom. " +
                        "Would you like to hear more about my enchanted beginnings, the lessons from my clumsy misadventures, or how those early failures shaped my present alchemical path?"
                    );
                    journeyModule.AddOption("Tell me about your enchanted beginnings.",
                        p => true,
                        p =>
                        {
                            DialogueModule beginningsModule = new DialogueModule(
                                "I was once a bright-eyed apprentice in a modest mage’s tower. Every day was a discovery—every spell a doorway to wonder. " +
                                "Yet, my eagerness often outpaced my skill: potions exploded, enchanted quills wrote wildly, and one rather memorable incantation turned my mentor’s robes a shocking shade of violet. " +
                                "But through these magical misfires, I learned that true power lies not in perfection, but in the relentless pursuit of knowledge."
                            );
                            // A nested branch on personal reflection.
                            beginningsModule.AddOption("How did these mishaps affect you?",
                                pp => true,
                                pp =>
                                {
                                    DialogueModule mishapReflectionModule = new DialogueModule(
                                        "Each mishap was a painful yet invaluable lesson. I learned humility, caution, and the importance of embracing the unexpected. " +
                                        "While my clumsiness sometimes brought ridicule from fellow apprentices, it ultimately forged a spirit of resilience and an unyielding curiosity that defines me to this day."
                                    );
                                    pp.SendGump(new DialogueGump(pp, mishapReflectionModule));
                                });
                            p.SendGump(new DialogueGump(p, beginningsModule));
                        });
                    journeyModule.AddOption("Tell me about the lessons from your clumsy misadventures.",
                        p => true,
                        p =>
                        {
                            DialogueModule clumsyModule = new DialogueModule(
                                "One incident comes vividly to mind: during a full-moon ritual, my eagerness led me to misread the ancient incantation. " +
                                "A misfired spell sent vials of volatile elixir cascading across the floor, dousing the ritual circle in a cascade of shimmering, yet hazardous, magic. " +
                                "I spent days mending the damage, both physically and to my reputation. That day, I learned that a measured approach is just as vital as passion."
                            );
                            // Another nested branch questioning how he overcame the crisis.
                            clumsyModule.AddOption("How did you overcome that fiasco?",
                                pp => true,
                                pp =>
                                {
                                    DialogueModule overcomeModule = new DialogueModule(
                                        "With the help of a patient mentor and a steadfast friend, I slowly pieced together a solution. " +
                                        "That trial taught me to blend caution with my natural enthusiasm, creating a balance that has come to define my approach to both magic and alchemy."
                                    );
                                    pp.SendGump(new DialogueGump(pp, overcomeModule));
                                });
                            p.SendGump(new DialogueGump(p, clumsyModule));
                        });
                    journeyModule.AddOption("How have these early failures shaped your work today?",
                        p => true,
                        p =>
                        {
                            DialogueModule shapedModule = new DialogueModule(
                                "Every failure, every humorous misadventure, has been a stepping stone toward the mastery I now pursue. " +
                                "They remind me to cherish every unexpected outcome, to learn from my mistakes, and to continuously refine my craft. " +
                                "My secret past as an enthusiastic, curious, yet clumsy apprentice mage underpins my resilience and my unyielding pursuit of alchemical perfection."
                            );
                            p.SendGump(new DialogueGump(p, shapedModule));
                        });
                    player.SendGump(new DialogueGump(player, journeyModule));
                });

            // Option 5: General Musings & Clumsy Anecdotes
            greeting.AddOption("Your enthusiasm seems boundless—but are you ever embarrassed by your clumsiness?",
                player => true,
                player =>
                {
                    DialogueModule clumsyAnecdoteModule = new DialogueModule(
                        "Oh, my dear friend, my clumsiness is as much a part of me as my love for alchemy! " +
                        "There was a time when a simple flick of my wrist during an experiment sent a cascade of potions tumbling, " +
                        "resulting in a colorful, chaotic explosion that left me covered in iridescent slime. " +
                        "Instead of despair, I laughed at myself and learned that every fall carries its own wisdom. " +
                        "Would you like to hear the full tale or perhaps know how I managed to save the day despite it all?"
                    );
                    clumsyAnecdoteModule.AddOption("Please, tell me the full tale!",
                        p => true,
                        p =>
                        {
                            DialogueModule fullTaleModule = new DialogueModule(
                                "It was during a particularly stormy night—perfect for radical experiments. " +
                                "I had concocted a potion intended to enhance cognitive abilities. But as fate would have it, " +
                                "a misjudged incantation and a toppled vial led to a mini explosion that sent scrolls and reagents flying. " +
                                "I ended up with a rather spectacular head-to-toe makeover of glowing, dancing runes! " +
                                "It took weeks to clean up, but the incident remains a cherished, humorous memory that reminds me to always keep a sense of humor in the face of chaos."
                            );
                            p.SendGump(new DialogueGump(p, fullTaleModule));
                        });
                    clumsyAnecdoteModule.AddOption("How did you manage to fix things afterward?",
                        p => true,
                        p =>
                        {
                            DialogueModule fixModule = new DialogueModule(
                                "With a combination of heartfelt apologies, hastily summoned cleaning sprites (yes, I once tried magic to help with the mess!), " +
                                "and the patient guidance of a wise old mentor, I managed to restore not only the laboratory but also my own pride. " +
                                "It was a lesson in humility, resourcefulness, and the art of embracing one's imperfections."
                            );
                            p.SendGump(new DialogueGump(p, fixModule));
                        });
                    player.SendGump(new DialogueGump(player, clumsyAnecdoteModule));
                });

            // Farewell Option
            greeting.AddOption("Farewell, I must be on my way.",
                player => true,
                player =>
                {
                    player.SendMessage("Mino smiles wryly, his eyes reflecting the myriad misadventures and secret wonders of his past, as you depart.");
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
