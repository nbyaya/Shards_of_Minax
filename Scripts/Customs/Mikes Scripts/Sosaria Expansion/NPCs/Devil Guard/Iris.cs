using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of Iris, Crystal Whisperer")]
    public class Iris : BaseCreature
    {
        private DateTime lastDialogueTime;

        [Constructable]
        public Iris() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Iris, the Crystal Whisperer";
            Body = 0x191; // Human body

            // Stats
            SetStr(80);
            SetDex(110); // Increased dexterity nodding to her agile past.
            SetInt(120);
            SetHits(100);

            // Appearance & Gear (a mix of scholarly attire and subtle hints of her daring past)
            AddItem(new Robe() { Hue = 0x3E1 }); // A robe with a mystical hue.
            AddItem(new Sandals() { Hue = 0x3E1 });
            AddItem(new GoldRing() { Name = "Ring of Crystalline Clarity", Hue = 0x455 });
            // A concealed leather sash hints at her nimble past.
            AddItem(new BodySash() { Hue = 0x6, Name = "Hidden Sash" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            lastDialogueTime = DateTime.MinValue;
        }

        public Iris(Serial serial) : base(serial) { }

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
                "Greetings, seeker of luminous truths. I am Iris, the Crystal Whisperer. " +
                "In my days I have studied the magic of crystals, penned long letters to Elena the Archivist " +
                "of Castle British, and collaborated with the visionary Toma in Fawn. Yet beneath this scholarly " +
                "exterior, I harbor secrets of a past quite unorthodox—a past that was playful, agile, and cunning. " +
                "What mystery calls to you today?"
            );

            // Option: Inquire about her crystalline studies.
            greeting.AddOption("Tell me about your crystalline studies.",
                player => true,
                player =>
                {
                    DialogueModule crystalModule = CreateCrystalModule();
                    player.SendGump(new DialogueGump(player, crystalModule));
                });

            // Option: Discuss her correspondence with Elena the Archivist.
            greeting.AddOption("What do you and Elena discuss?",
                player => true,
                player =>
                {
                    DialogueModule elenaModule = CreateElenaModule();
                    player.SendGump(new DialogueGump(player, elenaModule));
                });

            // Option: Ask about her creative collaborations with Toma.
            greeting.AddOption("How do you work with Toma of Fawn?",
                player => true,
                player =>
                {
                    DialogueModule tomaModule = CreateTomaModule();
                    player.SendGump(new DialogueGump(player, tomaModule));
                });

            // Option: Discuss the unpredictable risks of crystal magic.
            greeting.AddOption("What risks do crystals pose?",
                player => true,
                player =>
                {
                    DialogueModule riskModule = CreateRiskModule();
                    player.SendGump(new DialogueGump(player, riskModule));
                });

            // Option: Learn about Iris the person behind the crystals.
            greeting.AddOption("Who are you beyond your crystals?",
                player => true,
                player =>
                {
                    DialogueModule personalModule = CreatePersonalModule();
                    player.SendGump(new DialogueGump(player, personalModule));
                });

            return greeting;
        }

        private DialogueModule CreateCrystalModule()
        {
            DialogueModule module = new DialogueModule(
                "Crystals are living echoes of ancient energies. I spend long nights studying their resonances—each shard " +
                "vibrating with stories lost in time. From the delicate blue opal to the blazing radiant quartz, every gem " +
                "holds a secret. Would you like to learn about a particular crystal or perhaps their role in enchantments?"
            );

            module.AddOption("Speak of the rare blue opal.",
                player => true,
                player =>
                {
                    DialogueModule blueOpalModule = new DialogueModule(
                        "The rare blue opal shimmers like the calm of moonlit water. Legend tells of its origins in the tears " +
                        "of an ancient celestial being. Do you wish to explore its lore, its healing properties, or an unusual " +
                        "incident when I narrowly avoided a trap while retrieving one from a cursed ruin?"
                    );
                    blueOpalModule.AddOption("Tell me its lore.",
                        p => true,
                        p =>
                        {
                            DialogueModule loreModule = new DialogueModule(
                                "Ancient scrolls record that the blue opal was formed when a deity wept for a lost love—its glow " +
                                "an eternal lament. I have pieced together fragments of this tale from forbidden manuscripts."
                            );
                            p.SendGump(new DialogueGump(p, loreModule));
                        });
                    blueOpalModule.AddOption("Describe its healing properties.",
                        p => true,
                        p =>
                        {
                            DialogueModule healingModule = new DialogueModule(
                                "Used in enchanted amulets, the blue opal is said to align the heart's rhythm with the subtle pulse " +
                                "of the universe. It can soothe ailments of both body and spirit, as I have confirmed in my experiments."
                            );
                            p.SendGump(new DialogueGump(p, healingModule));
                        });
                    blueOpalModule.AddOption("That reminds me—wasn't retrieving one dangerous?",
                        p => true,
                        p =>
                        {
                            DialogueModule dangerModule = new DialogueModule(
                                "Indeed, on one daring retrieval, I had to leap over collapsing ruins and dodge cursed traps—a dance " +
                                "of agility and wit. That escapade remains one of my most cherished memories, though I say it with a playful smirk."
                            );
                            dangerModule.AddOption("Tell me more about that escapade.",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule escapadeModule = new DialogueModule(
                                        "In the midnight of a stormy night, I infiltrated a long-abandoned temple. Every shadow was a threat, " +
                                        "and every step demanded exquisite caution. It was not for wealth but for the thrill—the perfect test of " +
                                        "my nimble reflexes and cunning mind."
                                    );
                                    pl.SendGump(new DialogueGump(pl, escapadeModule));
                                });
                            p.SendGump(new DialogueGump(p, dangerModule));
                        });
                    player.SendGump(new DialogueGump(player, blueOpalModule));
                });

            module.AddOption("Speak of the radiant quartz.",
                player => true,
                player =>
                {
                    DialogueModule quartzModule = new DialogueModule(
                        "Radiant quartz is a conduit for pure, unbridled energy. I have harnessed its power in many experiments " +
                        "and artistic endeavors. Would you like to know about its energy properties, its role in my creative " +
                        "collaborations, or perhaps a curious anecdote from its volatile nature?"
                    );
                    quartzModule.AddOption("Detail its energy properties.",
                        p => true,
                        p =>
                        {
                            DialogueModule energyModule = new DialogueModule(
                                "Radiant quartz can amplify both light and dark forces alike. When attuned correctly, it channels " +
                                "energy that heals as quickly as it can harm. Its clarity reflects not only physical light but also " +
                                "the inner essence of its wielder."
                            );
                            p.SendGump(new DialogueGump(p, energyModule));
                        });
                    quartzModule.AddOption("How do you use it in your art?",
                        p => true,
                        p =>
                        {
                            DialogueModule artModule = new DialogueModule(
                                "Together with Toma, we fuse radiant quartz with molten glass to create sculptures that pulse with life. " +
                                "Every piece is crafted with precision and a dash of rogue ingenuity, recalling days when I relied on " +
                                "stealth and speed to procure rare minerals in perilous locales."
                            );
                            p.SendGump(new DialogueGump(p, artModule));
                        });
                    quartzModule.AddOption("What an intriguing material—does it ever misbehave?",
                        p => true,
                        p =>
                        {
                            DialogueModule misbehaveModule = new DialogueModule(
                                "Oh, many times! On one occasion, a sudden surge of energy nearly shattered my workshop. That incident " +
                                "required quick thinking and even quicker feet. It was a reminder that magic, much like life, is unpredictable."
                            );
                            misbehaveModule.AddOption("How did you handle that?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule handleModule = new DialogueModule(
                                        "With a graceful leap and a few incantations, I contained the burst before it caused irreparable harm. " +
                                        "Moments like these are both a test and a thrill—a secret nod to my more agile, less scholarly past."
                                    );
                                    pl.SendGump(new DialogueGump(pl, handleModule));
                                });
                            p.SendGump(new DialogueGump(p, misbehaveModule));
                        });
                    player.SendGump(new DialogueGump(player, quartzModule));
                });

            module.AddOption("Return to main greetings.",
                player => true,
                player =>
                {
                    DialogueModule greeting = CreateGreetingModule();
                    player.SendGump(new DialogueGump(player, greeting));
                });

            return module;
        }

        private DialogueModule CreateElenaModule()
        {
            DialogueModule module = new DialogueModule(
                "Elena the Archivist is my esteemed correspondent. Our letters are an intricate blend of scholarly debate and " +
                "whispered secrets. Through our exchange, we compare arcane inscriptions and the subtle interplay between crystal " +
                "energies and forgotten history."
            );

            module.AddOption("What secret knowledge do you share?",
                player => true,
                player =>
                {
                    DialogueModule secretKnowledgeModule = new DialogueModule(
                        "Each letter with Elena is a journey through time. We analyze cryptic glyphs and decipher codes that " +
                        "speak of lost civilizations. Sometimes, our discourse even hints at the dark arts, hidden beneath layers " +
                        "of benign scholarship."
                    );
                    secretKnowledgeModule.AddOption("How do you decode these inscriptions?",
                        p => true,
                        p =>
                        {
                            DialogueModule decodeModule = new DialogueModule(
                                "Elena's mastery of ancient languages, combined with my intuition for crystalline harmonics, allows " +
                                "us to see beyond the obvious. Our efforts often reveal truth obscured by time and a touch of deliberate mischief."
                            );
                            p.SendGump(new DialogueGump(p, decodeModule));
                        });
                    secretKnowledgeModule.AddOption("What effects do these revelations have on you?",
                        p => true,
                        p =>
                        {
                            DialogueModule effectModule = new DialogueModule(
                                "Her insights are both a comfort and a challenge. They remind me that knowledge is powerful—and that " +
                                "sometimes, the quest for truth comes with a cost, a cost I once paid during nights filled with adrenaline " +
                                "and daring escapes."
                            );
                            p.SendGump(new DialogueGump(p, effectModule));
                        });
                    player.SendGump(new DialogueGump(player, secretKnowledgeModule));
                });

            module.AddOption("Return to main greetings.",
                player => true,
                player =>
                {
                    DialogueModule greeting = CreateGreetingModule();
                    player.SendGump(new DialogueGump(player, greeting));
                });

            return module;
        }

        private DialogueModule CreateTomaModule()
        {
            DialogueModule module = new DialogueModule(
                "Toma of Fawn is a genius of the arts—a creative force who transforms raw magical energy into living masterpieces. " +
                "In our workshops, the boundary between art and magic is blurred as we play with elements, hues, and enchanted materials."
            );

            module.AddOption("Describe a recent collaborative project.",
                player => true,
                player =>
                {
                    DialogueModule projectModule = new DialogueModule(
                        "Our latest project was a series of luminescent sculptures where we fused radiant quartz with delicate " +
                        "stained glass. It was a high-wire act of balancing volatile magic with artistic vision—a project " +
                        "that tested our skills and left us both exhilarated and humbled."
                    );
                    projectModule.AddOption("What were the biggest challenges?",
                        p => true,
                        p =>
                        {
                            DialogueModule challengeModule = new DialogueModule(
                                "There were moments when a misaligned spell nearly shattered our work. Toma's quick, agile reflexes " +
                                "and my calculated adjustments kept disaster at bay. It was akin to a dance where every misstep " +
                                "could mean ruin, and yet each step brought us closer to perfection."
                            );
                            p.SendGump(new DialogueGump(p, challengeModule));
                        });
                    projectModule.AddOption("How was your work received?",
                        p => true,
                        p =>
                        {
                            DialogueModule feedbackModule = new DialogueModule(
                                "The sculptures have been hailed as a fusion of beauty and arcane power. Critics speak of them as " +
                                "living art—a testament to the synergy of creativity and magic. The experience has deepened our mutual " +
                                "respect and, dare I say, sparked a playful rivalry over who can outdo the other in their creative feats."
                            );
                            p.SendGump(new DialogueGump(p, feedbackModule));
                        });
                    player.SendGump(new DialogueGump(player, projectModule));
                });

            module.AddOption("How do your skills complement each other?",
                player => true,
                player =>
                {
                    DialogueModule complementModule = new DialogueModule(
                        "Toma’s artistry brings emotions to life while I maintain the delicate scientific balance of magical energies. " +
                        "Our partnership is a blend of rigorous study and unbridled creativity—a harmonious contrast between discipline " +
                        "and playful freedom."
                    );
                    player.SendGump(new DialogueGump(player, complementModule));
                });

            module.AddOption("Return to main greetings.",
                player => true,
                player =>
                {
                    DialogueModule greeting = CreateGreetingModule();
                    player.SendGump(new DialogueGump(player, greeting));
                });

            return module;
        }

        private DialogueModule CreateRiskModule()
        {
            DialogueModule module = new DialogueModule(
                "Crystal magic, though wondrous, is perilous. Mishandled energies can erupt, distort reality, or reveal darker forces. " +
                "I have witnessed sudden surges and erratic bursts that require both caution and quick, agile responses."
            );

            module.AddOption("Tell me about a magical surge you've encountered.",
                player => true,
                player =>
                {
                    DialogueModule surgeModule = new DialogueModule(
                        "I recall a time when a misaligned crystal nearly caused a catastrophic burst in my workshop—an event that " +
                        "tested both my reflexes and resolve. In that moment, I leapt aside with a mischievous grin, as if the " +
                        "thrill of danger was a secret reward."
                    );
                    surgeModule.AddOption("How did you manage to survive it?",
                        p => true,
                        p =>
                        {
                            DialogueModule surviveModule = new DialogueModule(
                                "With a combination of swift footwork and carefully recited incantations, I contained the surge. " +
                                "It was a reminder that handling such potent magic requires not just knowledge, but a playful agility " +
                                "and a hint of rebellious daring."
                            );
                            p.SendGump(new DialogueGump(p, surviveModule));
                        });
                    player.SendGump(new DialogueGump(player, surgeModule));
                });

            module.AddOption("Can crystals affect emotions?",
                player => true,
                player =>
                {
                    DialogueModule emotionModule = new DialogueModule(
                        "Absolutely. Many believe that crystals can mirror or even amplify one's emotional state—calming or " +
                        "igniting passions. The resonance between a crystal and its master can be as tender as a lullaby or as " +
                        "fiery as a daring escapade."
                    );
                    player.SendGump(new DialogueGump(player, emotionModule));
                });

            module.AddOption("Return to main greetings.",
                player => true,
                player =>
                {
                    DialogueModule greeting = CreateGreetingModule();
                    player.SendGump(new DialogueGump(player, greeting));
                });

            return module;
        }

        private DialogueModule CreatePersonalModule()
        {
            DialogueModule module = new DialogueModule(
                "Beyond my studies, there lies a facet of me few dare to glimpse. I am playful and cunning, " +
                "with a past that is as agile as a cat and as daring as the night itself. In days long past, I roamed " +
                "the shadows—not for wealth, but for the pure challenge of outsmarting fate and testing my skills against " +
                "impossible odds."
            );

            module.AddOption("What inspired you to choose this path?",
                player => true,
                player =>
                {
                    DialogueModule motiveModule = new DialogueModule(
                        "I remember a time when a stray, shimmering crystal caught my eye, not for its value but " +
                        "for the thrill of securing it from a heavily guarded vault. The sheer audacity of that act " +
                        "ignited my spark—a desire to prove my cunning and agility. Every heist since then has been a " +
                        "dance with danger, a playful challenge against the fates."
                    );
                    motiveModule.AddOption("That sounds exhilarating—how do you balance it with your studies?",
                        p => true,
                        p =>
                        {
                            DialogueModule balanceModule = new DialogueModule(
                                "I blend scholarly rigor with my stealthy escapades. While my colleagues pore over dusty tomes, " +
                                "I sometimes slip away under moonlight to test my mettle, all while gathering rare, forbidden " +
                                "crystals. It is an art—a balance between discipline and playful defiance."
                            );
                            p.SendGump(new DialogueGump(p, balanceModule));
                        });
                    player.SendGump(new DialogueGump(player, motiveModule));
                });

            module.AddOption("What challenges do you face in your work?",
                player => true,
                player =>
                {
                    DialogueModule challengeModule = new DialogueModule(
                        "Every challenge is a lesson—balancing the wild nature of magic, protecting its secrets, and " +
                        "navigating a world where power attracts both admiration and envy. There are days when my past " +
                        "shadows me, and moments when that playful, cunning spirit is the only thing that saves me."
                    );
                    challengeModule.AddOption("Has your secret past ever endangered you?",
                        p => true,
                        p =>
                        {
                            DialogueModule dangerModule = new DialogueModule(
                                "Ah, indeed. My daring escapades have left scars—both physical and emotional. But each risk " +
                                "was a test of wits, a game where I was both the thief and the strategist. Those days taught me " +
                                "to be agile, to think on my feet, and to always embrace the unexpected."
                            );
                            dangerModule.AddOption("Do you ever regret those days?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule regretModule = new DialogueModule(
                                        "Regret? Not in the least. Every twist and turn of that secret past is a part of who I am today—a " +
                                        "blend of wisdom, mischief, and an irrepressible desire to challenge the ordinary."
                                    );
                                    pl.SendGump(new DialogueGump(pl, regretModule));
                                });
                            p.SendGump(new DialogueGump(p, dangerModule));
                        });
                    player.SendGump(new DialogueGump(player, challengeModule));
                });

            // NEW: A secret branch specifically for her hidden past.
            module.AddOption("I sense mischief in your eyes... Were you ever a thief?",
                player => true,
                player =>
                {
                    DialogueModule secretPastModule = new DialogueModule(
                        "You have a keen eye! Yes, in whispers and shadows I was once known as a nimble thief—a playful spirit " +
                        "who stole not for riches but for the pure challenge. I reveled in outwitting guards, scaling impossible walls, " +
                        "and leaving behind a trail of laughter and mystery."
                    );
                    secretPastModule.AddOption("Tell me about one of your most daring heists.",
                        p => true,
                        p =>
                        {
                            DialogueModule heistModule = new DialogueModule(
                                "There was a night when I set my sights on a priceless crystalline artifact locked within a labyrinth " +
                                "of cursed corridors. With nothing but my wits and quicksilver feet, I navigated shifting traps " +
                                "and deceptive mirrors—each step a testament to my agility and cunning. The thrill of that heist " +
                                "still quickens my pulse."
                            );
                            heistModule.AddOption("That sounds exhilarating! How did you escape?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule escapeModule = new DialogueModule(
                                        "In a blur of movement and laughter, I slipped through a hidden passage as alarms began to sound. " +
                                        "It was a flawless exit—a dance of shadows that left onlookers baffled and my pursuers empty-handed."
                                    );
                                    pl.SendGump(new DialogueGump(pl, escapeModule));
                                });
                            heistModule.AddOption("Did that experience change you?",
                                pl => true,
                                pl =>
                                {
                                    DialogueModule changeModule = new DialogueModule(
                                        "Absolutely. Each escapade honed my sense of risk, deepened my passion for the unpredictable, " +
                                        "and ultimately taught me that even in darkness, there is a beauty to be found in challenge and freedom."
                                    );
                                    pl.SendGump(new DialogueGump(pl, changeModule));
                                });
                            p.SendGump(new DialogueGump(p, heistModule));
                        });
                    secretPastModule.AddOption("Do you ever miss those days?",
                        p => true,
                        p =>
                        {
                            DialogueModule missModule = new DialogueModule(
                                "There is a nostalgic warmth when I remember the thrill of the chase and the art of a well-planned heist. " +
                                "Yet, those days belong to a different era of my life—a secret chapter that fuels my current quest for " +
                                "knowledge and the perfect synthesis of magic and art."
                            );
                            p.SendGump(new DialogueGump(p, missModule));
                        });
                    secretPastModule.AddOption("Keep your secret, I mustn't pry.",
                        p => true,
                        p =>
                        {
                            DialogueModule hushModule = new DialogueModule(
                                "Ah, a wise choice, dear friend. Some secrets are best left to the twilight. But know this: " +
                                "every secret has its spark, and mine still dances in the light of every crystal I study."
                            );
                            p.SendGump(new DialogueGump(p, hushModule));
                        });
                    player.SendGump(new DialogueGump(player, secretPastModule));
                });

            module.AddOption("Return to main greetings.",
                player => true,
                player =>
                {
                    DialogueModule greeting = CreateGreetingModule();
                    player.SendGump(new DialogueGump(player, greeting));
                });

            return module;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastDialogueTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastDialogueTime = reader.ReadDateTime();
        }
    }
}
