using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of Cassian, the Spirit Seeker")]
    public class Cassian : BaseCreature
    {
        [Constructable]
		public Cassian() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cassian";
            Body = 0x190; // Human male body

            // Stats
            SetStr(90);
            SetDex(80);
            SetInt(120);
            SetHits(100);

            // Appearance
            AddItem(new Robe() { Hue = 1175 });
            AddItem(new WideBrimHat() { Hue = 1175, Name = "Hat of Veiled Whispers" });
            AddItem(new Sandals() { Hue = 1175 });
            AddItem(new Spellbook() { Name = "Tomes of the Forgotten" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Cassian(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Cassian, a seeker of spectral truths and forgotten lore. My life is intertwined with the restless spirits I commune with, the eerie visions shared by Mother Edda, and the arcane research I pursue alongside Mino. What would you like to learn of today?");
            
            greeting.AddOption("Tell me about the spirits you commune with.", 
                player => true, 
                player =>
                {
                    DialogueModule spiritModule = CreateSpiritModule();
                    player.SendGump(new DialogueGump(player, spiritModule));
                });

            greeting.AddOption("I’d like to hear about your eerie encounters with Mother Edda.", 
                player => true, 
                player =>
                {
                    DialogueModule eerieModule = CreateEerieModule();
                    player.SendGump(new DialogueGump(player, eerieModule));
                });

            greeting.AddOption("What can you share about your research with Mino?", 
                player => true, 
                player =>
                {
                    DialogueModule researchModule = CreateResearchModule();
                    player.SendGump(new DialogueGump(player, researchModule));
                });

            greeting.AddOption("Tell me about your personal journey and past.", 
                player => true, 
                player =>
                {
                    DialogueModule journeyModule = CreateJourneyModule();
                    player.SendGump(new DialogueGump(player, journeyModule));
                });

            greeting.AddOption("Farewell.", 
                player => true, 
                player =>
                {
                    // Ends the dialogue tree
                    player.SendMessage("Cassian nods slowly as you step away, the weight of his secrets lingering in the air.");
                });

            return greeting;
        }

        private DialogueModule CreateSpiritModule()
        {
            DialogueModule spirit = new DialogueModule("The spirits I commune with are as fickle as the autumn wind. I have long collaborated with Jonas from West Montor—a young mage with a sharp mind for interpreting these ghostly whispers. Together, we decipher ominous messages and cryptic portents. One evening, under a waning crescent, Jonas and I heard a lament that foretold a great upheaval in the realms of men and magic.");
            
            spirit.AddOption("What did that omen reveal?", 
                player => true, 
                player =>
                {
                    DialogueModule omenModule = new DialogueModule("The omen spoke of a time when the barrier between our world and the realm of spirits would falter. Jonas interpreted the wails as a prelude to a tempest of lost souls, urging us to prepare for a night when the dead might walk among us. Do you believe in such portents?");
                    
                    omenModule.AddOption("I find such mysteries fascinating.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    omenModule.AddOption("I remain skeptical of such whispers.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, omenModule));
                });

            spirit.AddOption("Tell me more about Jonas and your secret meetings.", 
                player => true, 
                player =>
                {
                    DialogueModule jonasModule = new DialogueModule("Ah, Jonas! A kindred spirit in the art of spectral interpretation. In the secluded libraries of West Montor, we pore over ancient scrolls and cryptic runes. He once confided that every spirit has its own sorrow—a tale etched in the winds of time. Would you care to hear one of his most haunting accounts?");
                    
                    jonasModule.AddOption("Yes, please share his tale.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule taleModule = new DialogueModule("One moonless night, Jonas spoke of a forlorn wraith whose sorrow was so deep that it echoed across the valleys. Its mournful song was said to forewarn of impending disaster, a secret only the brave dare to decipher. Our shared encounters have forged a bond, one of trust in a world where silence often hides the truth.");
                            pl.SendGump(new DialogueGump(pl, taleModule));
                        });
                    
                    jonasModule.AddOption("I’d rather not delve too deeply.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, jonasModule));
                });

            spirit.AddOption("Back to main topics.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return spirit;
        }

        private DialogueModule CreateEerieModule()
        {
            DialogueModule eerie = new DialogueModule("Mother Edda of Yew is a mystery wrapped in ancient prophecy. Her eyes see beyond the veil of this world, and her visions have haunted my nights. I recall a twilight meeting beneath the gnarled boughs of an old oak, where she described a convergence of fate and doom—a premonition that still chills me to the bone.");
            
            eerie.AddOption("What exactly did she see that night?", 
                player => true, 
                player =>
                {
                    DialogueModule premonitionModule = new DialogueModule("She spoke of a spectral procession, where the echoes of forgotten souls danced in the fog. Her words painted a grim picture of a coming darkness, one that might shatter the delicate balance between life and death. Each detail was etched with sorrow and urgency. Do you find such prophecies compelling?");
                    
                    premonitionModule.AddOption("They are deeply moving.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    premonitionModule.AddOption("I remain cautious about such visions.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, premonitionModule));
                });

            eerie.AddOption("How have her visions affected your path?", 
                player => true, 
                player =>
                {
                    DialogueModule impactModule = new DialogueModule("Mother Edda’s insights have often steered me away from peril—or towards it, as fate might demand. Her cryptic warnings have led me to ancient ruins and long-forgotten crypts where the past lingers in every shadow. It is a burden and a blessing, to know the threads of destiny. Does your heart not quiver at the thought of fate’s unseen hand?");
                    
                    impactModule.AddOption("I am intrigued by fate’s design.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    impactModule.AddOption("I prefer to shape my own destiny.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, impactModule));
                });

            eerie.AddOption("Back to main topics.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return eerie;
        }

        private DialogueModule CreateResearchModule()
        {
            DialogueModule research = new DialogueModule("My research with Mino of East Montor delves into the forbidden texts and relics of lost civilizations. Together, we labor over ancient manuscripts and alchemical symbols that speak of powers beyond mortal ken. Recently, we unearthed a relic whose inscriptions hint at a secret long buried by time.");
            
            research.AddOption("What is this relic you speak of?", 
                player => true, 
                player =>
                {
                    DialogueModule relicModule = new DialogueModule("The relic is a weathered tablet etched with runes that pulse faintly under moonlight. Mino believes it contains the key to unlocking a dormant power—a force that may either protect or doom us. Our discussions have been fraught with both wonder and dread. Would you like to learn more about its origins?");
                    
                    relicModule.AddOption("Yes, tell me its history.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule historyModule = new DialogueModule("Legend has it that the tablet was forged in an age when magic ruled the earth. Its inscriptions are a palimpsest of ancient lore, merging the language of the old gods with cryptic symbols of nature’s wrath. Mino and I continue to debate its true purpose to this very day.");
                            pl.SendGump(new DialogueGump(pl, historyModule));
                        });
                    
                    relicModule.AddOption("I think I'll let some mysteries lie.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, relicModule));
                });

            research.AddOption("How do you and Mino work together?", 
                player => true, 
                player =>
                {
                    DialogueModule partnershipModule = new DialogueModule("Our partnership is built on mutual respect and a shared thirst for forbidden knowledge. While I provide insight into the spectral and ethereal, Mino brings the rigor of alchemy and historical scholarship. We often debate late into the night, our discussions lit only by the flicker of candlelight and the glow of arcane symbols.");
                    
                    partnershipModule.AddOption("That sounds like a powerful alliance.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    partnershipModule.AddOption("Do you ever disagree?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule debateModule = new DialogueModule("Oh, indeed! Our debates are as fierce as they are enlightening. Mino insists on logical precision while I lean into the realm of intuition and mystery. Yet, it is precisely this friction that sparks breakthroughs in our research. In the end, our differences forge a path to deeper understanding.");
                            pl.SendGump(new DialogueGump(pl, debateModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, partnershipModule));
                });

            research.AddOption("Back to main topics.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return research;
        }

        private DialogueModule CreateJourneyModule()
        {
            DialogueModule journey = new DialogueModule("My journey is etched in both joy and sorrow. I was once a scholar of conventional lore, until tragedy struck—an inexplicable loss that drove me into the arms of the unknown. Wandering haunted ruins and forsaken graveyards, I sought answers in every whisper of the past. Every encounter, every spectral voice, has taught me a lesson about loss, hope, and the elusive nature of truth.");
            
            journey.AddOption("What tragedy drove you to this path?", 
                player => true, 
                player =>
                {
                    DialogueModule tragedyModule = new DialogueModule("I lost those I held dear to an inexplicable malady—one that seemed to be born of the very spirits I now seek to understand. In my grief, I turned to the occult and the arcane, desperate to find meaning. This path, though perilous, has revealed secrets that most would not dare imagine.");
                    
                    tragedyModule.AddOption("I am sorry for your loss.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    tragedyModule.AddOption("It seems fate has a cruel sense of irony.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, tragedyModule));
                });

            journey.AddOption("Have your experiences changed you?", 
                player => true, 
                player =>
                {
                    DialogueModule changeModule = new DialogueModule("Every encounter with the beyond leaves its mark. I have learned humility in the face of forces I scarcely comprehend, and I bear the scars of both loss and revelation. My heart has grown both heavier and wiser, forever intertwined with the enigmatic rhythms of life and death.");
                    
                    changeModule.AddOption("Your wisdom is evident.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    changeModule.AddOption("I admire your strength.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, changeModule));
                });

            journey.AddOption("Back to main topics.", 
                player => true, 
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));

            return journey;
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
