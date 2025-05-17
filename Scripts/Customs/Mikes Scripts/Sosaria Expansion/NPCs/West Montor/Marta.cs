using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the restless corpse of Marta")]
    public class Marta : BaseCreature
    {
        [Constructable]
        public Marta() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marta, the Dreamer";
            Body = 0x191; // Human body

            // Set basic stats
            SetStr(90);
            SetDex(70);
            SetInt(110);
            SetHits(100);

            // Appearance: Elegant and evocative
            AddItem(new FancyDress() { Hue = 1750, Name = "The Midnight Gown" });
            AddItem(new Cloak() { Hue = 1850, Name = "The Veil of Night" });
            AddItem(new Sandals() { Hue = 1650 });
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Marta(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        /// <summary>
        /// Sets up the main dialogue entry point.
        /// </summary>
        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Hello, traveler. I am Marta. Night after night, my dreams are invaded by a terrible vision of Catastrophe—ruined landscapes and spectral figures whispering secrets of doom. Alongside these visions, a secret torment haunts my every step. Would you care to learn of my dreams, my relationships with Mother Edda, Darvin, and Syla, or something more hidden in my past?");
            
            // Branch: Recurring Dreams
            greeting.AddOption("Tell me about your recurring dreams.", 
                player => true,
                player =>
                {
                    DialogueModule dreamsModule = new DialogueModule("Every night the nightmares come: I see a desolate future with crumbling ruins and ghostly laments echoing through the darkness. Sometimes a cloaked figure leads me along paths shrouded in mystery. My dreams are a mixture of foreboding warnings and sorrowful memories.");
                    
                    dreamsModule.AddOption("How do you cope with these visions?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule copeModule = new DialogueModule("I cope by recording every detail in my journal—hoping that in those frenzied scribbles lie clues to a greater truth. Darvin, with his unwavering pragmatism, often helps me sort through the chaos. It is a delicate dance between despair and determination.");
                            copeModule.AddOption("Your commitment is both moving and tragic.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, copeModule));
                        });
                    
                    dreamsModule.AddOption("What exactly do you see?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule detailsModule = new DialogueModule("Some nights, I see endless barren fields bathed in a blood-red twilight, with statues of broken heroes and crumbling fortresses. Other nights, the vision is of a solitary figure—its elegance masked by a veil of sorrow, beckoning me with outstretched arms.");
                            
                            detailsModule.AddOption("Could these be warnings of an impending doom?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule warningModule = new DialogueModule("I fear they are. The visions seem not merely dreams, but omens woven into the tapestry of fate. Even Mother Edda's cryptic counsel hints that these dreams are connected to the deeper, darker currents that flow through our land.");
                                    warningModule.AddOption("Fate is often inscrutable.",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, warningModule));
                                });
                            
                            detailsModule.AddOption("Or might they simply mirror your inner sorrow?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule innerModule = new DialogueModule("Perhaps. The pain I bear is more than simple dread—it is entwined with memories of a past life, a performance of beauty and torment that continues to haunt me.");
                                    innerModule.AddOption("That sounds very personal.",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, innerModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, detailsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, dreamsModule));
                });
            
            // Branch: Relationships with NPCs
            greeting.AddOption("I'm curious about your relationships. Tell me about Mother Edda, Darvin, and Syla.",
                player => true,
                player =>
                {
                    DialogueModule relationshipsModule = new DialogueModule("My bonds with these kind souls provide a fragile pillar of hope. Mother Edda of Yew, with her ancient prophecies, sees beyond the visible. Darvin of West Montor offers logical counsel tempered by compassion, and gentle Syla of East Montor soothes my spirit with quiet empathy.");
                    
                    relationshipsModule.AddOption("Tell me more about Mother Edda.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule eddaModule = new DialogueModule("Mother Edda speaks in murmurs and riddles. In the deep woods of Yew, her words mix with the rustling leaves, guiding me like a beacon through my darkest dreams. Her wisdom is as old as the trees, though never without pain.");
                            eddaModule.AddOption("Her guidance seems both mysterious and comforting.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, eddaModule));
                        });
                    
                    relationshipsModule.AddOption("What of Darvin?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule darvinModule = new DialogueModule("Darvin is my anchor in the storm. He listens without judgment, offering practical advice drawn from his own life of labor and soil in West Montor. His steady presence reminds me that even when visions shake my soul, there is stability in this mortal realm.");
                            darvinModule.AddOption("He sounds like a true friend.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, darvinModule));
                        });
                    
                    relationshipsModule.AddOption("And Syla? How does she help ease your pain?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule sylaModule = new DialogueModule("Syla, with her soft-spoken words and innate magic, helps me to find moments of calm. In East Montor, her gentle insight pierces through the melancholy, reminding me that hope may yet bloom in barren soil.");
                            sylaModule.AddOption("It’s beautiful that you have her support.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, sylaModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, relationshipsModule));
                });
            
            // Branch: Impact on Daily Life
            greeting.AddOption("Do your dreams affect your daily life?",
                player => true,
                player =>
                {
                    DialogueModule impactModule = new DialogueModule("Every waking moment carries an echo of those nightmarish visions. I often find small omens—a fallen leaf, an unexpected chill—that mirror the shadows of my dreams. These events force me to always walk a fine line between art and agony.");
                    
                    impactModule.AddOption("That must weigh heavily on you.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    impactModule.AddOption("Do you take measures to guard against these omens?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule actionsModule = new DialogueModule("I do. I meticulously record every detail in my leather-bound journal, seeking patterns that might unravel the curse. Sometimes, guided by Mother Edda, I perform small rituals—tracing ancient symbols, whispering forgotten verses—to stave off the creeping despair.");
                            actionsModule.AddOption("Your dedication is truly inspiring.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, actionsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, impactModule));
                });
            
            // NEW Branch: The Secret Past
            greeting.AddOption("Tell me about your secret past.", 
                player => true,
                player =>
                {
                    DialogueModule pastModule = new DialogueModule("There is a part of me I rarely share—a hidden chapter of my life. Before these tormenting dreams and cursed nights, I was a dancer. I moved with the grace of a swan and the passion of a wildfire upon the grand stages of life. Yet a tragic curse was cast upon me, condemning me to an existence of immortal performance, an endless waltz with sorrow.");
                    
                    pastModule.AddOption("How did you become cursed?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule curseModule = new DialogueModule("The curse is a murmur of legends. Some say it was the act of a spurned lover, others speak of a vengeful spirit of an ancient deity. All I recall is a night of dazzling lights and a forbidden dance performed under a lunar eclipse—a moment when my passion and talent invoked the wrath of a force far beyond mortal ken.");
                            
                            curseModule.AddOption("Tell me more about that night.", 
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule nightModule = new DialogueModule("That fateful night, I danced with a fervor I had never known. The audience was captivated, but as I spun upon the stage, an eerie chill swept through the hall. A luminous figure appeared in the darkness—a messenger of fate—cursing me to eternal performance. I was granted immortality, but at the cost of endless longing and sorrow.");
                                    
                                    nightModule.AddOption("It sounds unbearably tragic.",
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule tragicModule = new DialogueModule("It is a tragedy wrapped in beauty—a constant reminder that even art can demand a heavy price. Each graceful movement is a lament for the life I once had, and each performance is a plea for release from this relentless curse.");
                                            tragicModule.AddOption("Is there any hope for your freedom?",
                                                pl4 => true,
                                                pl4 =>
                                                {
                                                    DialogueModule hopeModule = new DialogueModule("I dare to dream of liberation. I wander this realm searching for a kindred soul, one whose touch or spirit might finally break the cycle of eternal torment. Until that day, I am bound to the stage, dancing in shadow and sorrow.");
                                                    hopeModule.AddOption("I hope you find what you seek.",
                                                        pl5 => true,
                                                        pl5 => pl5.SendGump(new DialogueGump(pl5, CreateGreetingModule())));
                                                    pl4.SendGump(new DialogueGump(pl4, hopeModule));
                                                });
                                            pl3.SendGump(new DialogueGump(pl3, tragicModule));
                                        });
                                    
                                    nightModule.AddOption("What did your life look like before the curse?",
                                        pl3 => true,
                                        pl3 =>
                                        {
                                            DialogueModule beforeModule = new DialogueModule("Before this curse, I was celebrated in the courts and the streets alike. Every performance was an expression of pure joy—a moment where the music and movement set my soul ablaze. I reveled in the applause, the adoration, and the ephemeral beauty of life. That joy, however, has been shadowed by an unquenchable sadness since the curse befell me.");
                                            beforeModule.AddOption("It must be hard to remember such a bittersweet past.",
                                                pl4 => true,
                                                pl4 => pl4.SendGump(new DialogueGump(pl4, CreateGreetingModule())));
                                            pl3.SendGump(new DialogueGump(pl3, beforeModule));
                                        });
                                    pl2.SendGump(new DialogueGump(pl2, nightModule));
                                });
                            
                            curseModule.AddOption("Do you regret your past?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule regretModule = new DialogueModule("There is a deep ache in my heart—a longing for the freedom and the fleeting beauty of mortal life. Regret and acceptance intertwine. My dance is both a celebration of what was and a lament for what will never be again.");
                                    regretModule.AddOption("Your sorrow is palpable.",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, regretModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, curseModule));
                        });
                    
                    pastModule.AddOption("Do you ever wish to stop dancing?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule stopDanceModule = new DialogueModule("Every step I take, every graceful pirouette, is a reminder of my unending torment. There are moments when the weight of immortality feels unbearable, and I long for the sweet release of oblivion. Yet, even in my sadness, I clothe my pain in beauty as if to honor the fleeting nature of life.");
                            
                            stopDanceModule.AddOption("Is there a way to break the curse?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule breakCurseModule = new DialogueModule("Legends hint at a ritual—an ancient rite performed beneath a rare celestial alignment—that could finally sever the bonds of this curse. I have sought wisdom from Mother Edda and Darvin, but the answer remains elusive. Only a soul of great compassion and strength might hold the key to ending my eternal performance.");
                                    breakCurseModule.AddOption("I wish you success in finding that key.",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, breakCurseModule));
                                });
                            
                            stopDanceModule.AddOption("Must it always be so heartbreaking?",
                                pl2 => true,
                                pl2 =>
                                {
                                    DialogueModule heartbreakingModule = new DialogueModule("Heartbreak and beauty are my constant companions. In every graceful movement, there lies both a memory of joyful existence and the echo of my endless suffering. Perhaps, in time, this sorrow will transform into a quiet acceptance.");
                                    heartbreakingModule.AddOption("Your resilience is moving.",
                                        pl3 => true,
                                        pl3 => pl3.SendGump(new DialogueGump(pl3, CreateGreetingModule())));
                                    pl2.SendGump(new DialogueGump(pl2, heartbreakingModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, stopDanceModule));
                        });
                    
                    pastModule.AddOption("What do you do now, in your endless dance?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule performModule = new DialogueModule("I perform in quiet corners of the realm—at twilight, upon ancient stone bridges, in secret clearings where only the moon bears witness. Each performance is an elegy to the life I once knew and a prayer that one day, someone may understand and help break this curse.");
                            performModule.AddOption("Your performances must be incredibly moving.",
                                pl2 => true,
                                pl2 => pl2.SendGump(new DialogueGump(pl2, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, performModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, pastModule));
                });
            
            // Return option to allow the conversation to keep cycling back
            greeting.AddOption("Thank you for sharing your story, Marta.", 
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
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
