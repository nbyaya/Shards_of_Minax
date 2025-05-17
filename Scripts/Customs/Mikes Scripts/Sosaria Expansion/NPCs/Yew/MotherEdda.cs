using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the lingering spirit of Mother Edda")]
    public class MotherEdda : BaseCreature
    {
        [Constructable]
		public MotherEdda() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mother Edda";
            Body = 0x190; // Human appearance

            // Stats
            SetStr(80);
            SetDex(60);
            SetInt(120);
            SetHits(100);

            // Appearance & Equipment
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });
            AddItem(new FloppyHat() { Hue = 1150 });
            // A mystical token of her visionary nature
            AddItem(new BodySash() { Name = "Crystal of Foresight", Hue = 1175 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public MotherEdda(Serial serial) : base(serial) { }

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
                "Ah, welcome, traveler. I am Mother Edda, seer of omens and keeper of ancient truths. " +
                "The winds of fate whisper secrets to me, and though I now share visions and premonitions, " +
                "there lies a secret past hidden in my heart—a past of playful daring and cunning mischief. " +
                "What wisdom do you seek on this day?");

            // Option 1: Prophetic Visions
            greeting.AddOption("Tell me about your prophetic visions.",
                player => true,
                player =>
                {
                    DialogueModule visionsModule = new DialogueModule(
                        "My visions are like shards of a broken mirror—each fragment revealing a possible future. " +
                        "I have seen the determined path of Jonas in West Montor and the burning resolve of Syla in East Montor. " +
                        "Would you like to hear more about their destinies, or perhaps even glimpse a vision of your own?");
                    
                    // Jonas branch
                    visionsModule.AddOption("What do you see for Jonas?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule jonasModule = new DialogueModule(
                                "Jonas, the young mage with untapped power, stands at a crossroads. " +
                                "I see him traversing a spectral labyrinth where every turn forces him to confront the ghosts of his past. " +
                                "Only by embracing both his light and shadow can he prevail.");
                            jonasModule.AddOption("Tell me more about his trials.",
                                p => true,
                                p =>
                                {
                                    DialogueModule jonasTrialsModule = new DialogueModule(
                                        "Within those winding corridors, echoes of lost time challenge his resolve. " +
                                        "The trials are not just physical battles but tests of the very core of his spirit.");
                                    p.SendGump(new DialogueGump(p, jonasTrialsModule));
                                });
                            jonasModule.AddOption("How might he overcome these challenges?",
                                p => true,
                                p =>
                                {
                                    DialogueModule jonasOvercomeModule = new DialogueModule(
                                        "The answer lies in balance. Jonas must seek forgotten wisdom and trust in his allies. " +
                                        "Only by harmonizing ambition with humility will his heart find peace.");
                                    p.SendGump(new DialogueGump(p, jonasOvercomeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, jonasModule));
                        });

                    // Syla branch
                    visionsModule.AddOption("And what of Syla's future?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule sylaModule = new DialogueModule(
                                "Syla of East Montor carries a fierce inner flame tempered by past sorrows. " +
                                "My visions reveal her standing on the brink of transformation, bridging ancient wisdom with a new dawn.");
                            sylaModule.AddOption("Reveal the secrets of her path.",
                                p => true,
                                p =>
                                {
                                    DialogueModule sylaSecretsModule = new DialogueModule(
                                        "Her journey is illuminated by relics and the silent teachings of nature. " +
                                        "The whispers of ancestral spirits guide her through turbulent times.");
                                    p.SendGump(new DialogueGump(p, sylaSecretsModule));
                                });
                            sylaModule.AddOption("What challenges will she face?",
                                p => true,
                                p =>
                                {
                                    DialogueModule sylaChallengesModule = new DialogueModule(
                                        "Dark omens and forgotten adversaries lurk at the edges of her destiny. " +
                                        "Only by embracing both hope and sorrow will she forge a future of renewal.");
                                    p.SendGump(new DialogueGump(p, sylaChallengesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, sylaModule));
                        });

                    // Personal vision branch
                    visionsModule.AddOption("Can I see a vision of my own?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule personalVisionModule = new DialogueModule(
                                "The tapestry of fate is woven uniquely for each soul. " +
                                "To glimpse your future, you must surrender to the mystery and trust the murmurs of the unseen. " +
                                "Are you prepared to see the path that awaits you?");
                            personalVisionModule.AddOption("Yes, I am ready to know my fate.",
                                p => true,
                                p =>
                                {
                                    DialogueModule yourVisionModule = new DialogueModule(
                                        "Close your eyes and listen to the quiet pulse of your heart. " +
                                        "I see a winding path, fraught with uncertainty yet bright with possibility. " +
                                        "Remember, destiny is ever-changing with each decision you make.");
                                    p.SendGump(new DialogueGump(p, yourVisionModule));
                                });
                            personalVisionModule.AddOption("No, I must gather my strength first.",
                                p => true,
                                p =>
                                {
                                    DialogueModule prepareModule = new DialogueModule(
                                        "Wisdom grows in the fertile soil of patience. " +
                                        "Return when the call of destiny grows louder in your heart, and the mists shall part.");
                                    p.SendGump(new DialogueGump(p, prepareModule));
                                });
                            pl.SendGump(new DialogueGump(pl, personalVisionModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, visionsModule));
                });

            // Option 2: Ominous Premonition at the Ice Cavern
            greeting.AddOption("Tell me about your ominous premonition at the Ice Cavern.",
                player => true,
                player =>
                {
                    DialogueModule premonitionModule = new DialogueModule(
                        "The Ice Cavern—a realm where frost and foreboding intertwine. " +
                        "In a vision that still chills my bones, I witnessed a darkness seeping from the cavern’s core. " +
                        "I once confided these grim omens to Cassian of Fawn. " +
                        "Would you like to know more about what I saw or how Cassian received my words?");
                    
                    premonitionModule.AddOption("What did you see in the cavern?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule visionDetailModule = new DialogueModule(
                                "Inside the cavern, shards of ice glowed with an eerie luminescence, " +
                                "and the very walls pulsed with the heartbeat of ancient sorrow. " +
                                "Whispers of lost souls foretold a time when Sosaria’s balance would be shattered by an unyielding cold.");
                            pl.SendGump(new DialogueGump(pl, visionDetailModule));
                        });
                    
                    premonitionModule.AddOption("How did Cassian react?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule cassianModule = new DialogueModule(
                                "Cassian, ever the seeker of forbidden lore in Fawn, listened with a mix of wonder and dread. " +
                                "His eyes shone with both skepticism and acceptance—a recognition that some premonitions weigh heavy on the soul.");
                            pl.SendGump(new DialogueGump(pl, cassianModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, premonitionModule));
                });

            // Option 3: Learning About Her Life and Secrets
            greeting.AddOption("Share with me more about your life and the secrets you guard.",
                player => true,
                player =>
                {
                    DialogueModule lifeModule = new DialogueModule(
                        "My life is a tapestry of journeys through enchanted groves, desolate ruins, and ancient sanctuaries. " +
                        "I have lent my visions to Jonas, Syla, and many others. " +
                        "Each encounter adds a bittersweet note to my eternal song. What would you like to know?");
                    
                    lifeModule.AddOption("Tell me about your journeys.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule journeyModule = new DialogueModule(
                                "Every step I have taken led me deeper into the secrets of Sosaria—its hidden lore, forgotten legends, " +
                                "and the melancholy beauty of a world in constant flux. My travels have carved wisdom into my very soul.");
                            pl.SendGump(new DialogueGump(pl, journeyModule));
                        });
                    
                    lifeModule.AddOption("What secrets do you keep?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule secretsModule = new DialogueModule(
                                "The secrets I guard are not mere stories but the living essence of fate—tales of triumph, tragedy, and the fragile balance between light and shadow. " +
                                "They are whispered only to those who dare to listen.");
                            pl.SendGump(new DialogueGump(pl, secretsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            // Option 4: Reveal Your Secret Past
            greeting.AddOption("Reveal your secret past.",
                player => true,
                player =>
                {
                    DialogueModule secretPastModule = new DialogueModule(
                        "Ah, so you wish to unmask the mysteries hidden beneath these prophetic eyes? " +
                        "I must confess—a long time ago, before the weight of destiny claimed me, I danced in the shadows " +
                        "as a playful, agile, and cunning thief. I stole not for wealth, but to challenge fate and test my skills " +
                        "against the impossible. What would you like to know of that bygone life?");
                    
                    secretPastModule.AddOption("Tell me of your daring escapades.",
                        p => true,
                        p =>
                        {
                            DialogueModule escapadesModule = new DialogueModule(
                                "In my youth, I navigated gilded halls and moonlit rooftops with the grace of a falling leaf. " +
                                "I relished outsmarting the most vigilant guards, slipping through traps set to deter the unworthy. " +
                                "One night, under a sliver of moon, I infiltrated a noble’s impregnable vault to steal a jeweled mirror " +
                                "rumored to reveal hidden truths. Would you like me to recount that heist in detail?");
                            
                            escapadesModule.AddOption("Yes, recount that daring heist.",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule heistModule = new DialogueModule(
                                        "The night was a canvas of shadows and starlight. I crept along silent corridors, " +
                                        "my footsteps as light as whispers. Every trap and sentinel was but a puzzle to be solved. " +
                                        "I reached the vault where the jeweled mirror hung like a silent oracle. " +
                                        "The thrill of the chase was intoxicating, and I emerged victorious with a glimmer of that secret truth.");
                                    
                                    heistModule.AddOption("What did that mirror reveal to you?",
                                        p3 => true,
                                        p3 =>
                                        {
                                            DialogueModule mirrorModule = new DialogueModule(
                                                "I learned that some treasures are not meant for mortal eyes. " +
                                                "The mirror reflected a future of intertwined destinies and untold mysteries—its lesson was that the greatest treasure " +
                                                "is the exhilaration of the challenge itself.");
                                            p3.SendGump(new DialogueGump(p3, mirrorModule));
                                        });
                                    
                                    heistModule.AddOption("Did that night change you?",
                                        p3 => true,
                                        p3 =>
                                        {
                                            DialogueModule changeModule = new DialogueModule(
                                                "Indeed, it did. That night taught me that every lock, every obstacle, is a test " +
                                                "of one's wit and agility. It ignited a spark within me—a playful defiance of fate that still burns, " +
                                                "even as I now share visions of destiny.");
                                            p3.SendGump(new DialogueGump(p3, changeModule));
                                        });
                                    
                                    p2.SendGump(new DialogueGump(p2, heistModule));
                                });
                            
                            escapadesModule.AddOption("What challenges did you face during those escapades?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule challengesModule = new DialogueModule(
                                        "Each heist was a dance with danger—dodging enchanted traps, outwitting cunning guardians, " +
                                        "and navigating labyrinthine corridors imbued with ancient magic. I recall one trial where I had to " +
                                        "outmaneuver a maze of shifting walls, a true test of agility and cunning.");
                                    
                                    challengesModule.AddOption("How did you overcome that maze?",
                                        p3 => true,
                                        p3 =>
                                        {
                                            DialogueModule mazeModule = new DialogueModule(
                                                "With nimble steps and a mind sharp as a blade, I treated the maze like a playful puzzle. " +
                                                "I left subtle clues of my passage—a signature of mischief that only those with a keen eye could follow.");
                                            p3.SendGump(new DialogueGump(p3, mazeModule));
                                        });
                                    
                                    p2.SendGump(new DialogueGump(p2, challengesModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, escapadesModule));
                        });
                    
                    secretPastModule.AddOption("What drove you to choose this life of thievery?",
                        p => true,
                        p =>
                        {
                            DialogueModule driveModule = new DialogueModule(
                                "It was never about wealth or greed. I was driven by the thrill—the challenge of outsmarting fate " +
                                "and testing my limits against impossibly secure treasures. Every stolen gem and pilfered relic " +
                                "was a riddle posed by destiny, waiting for an answer only a cunning soul could provide.");
                            
                            driveModule.AddOption("How did that life shape who you are now?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule shapeModule = new DialogueModule(
                                        "It taught me that agility of body and mind is the key to unlocking life’s mysteries. " +
                                        "The playful defiance of convention remains with me, even as I now embrace the role of seer. " +
                                        "I have learned to see the beauty in chaos and the order within the unpredictable.");
                                    p2.SendGump(new DialogueGump(p2, shapeModule));
                                });
                            
                            driveModule.AddOption("I prefer not to dwell on the past.",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule dismissModule = new DialogueModule(
                                        "As you wish, traveler. Some secrets are meant to be left in the shadows, " +
                                        "just as the night conceals the playful dance of the stars.");
                                    p2.SendGump(new DialogueGump(p2, dismissModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, driveModule));
                        });
                    
                    secretPastModule.AddOption("I do not wish to know your past.",
                        p => true,
                        p =>
                        {
                            DialogueModule refuseModule = new DialogueModule(
                                "Very well. The past remains locked away like a treasure meant only for the brave—or the foolish. " +
                                "May your path be free of burdens and your destiny ever bright.");
                            p.SendGump(new DialogueGump(p, refuseModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, secretPastModule));
                });

            // Option 5: Farewell
            greeting.AddOption("Farewell, Mother Edda.",
                player => true,
                player =>
                {
                    DialogueModule farewellModule = new DialogueModule(
                        "May the winds of fate guide your steps, traveler. " +
                        "Remember, every ending heralds a new beginning. Until our paths cross again, tread lightly upon the threads of destiny.");
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
