using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Renn : BaseCreature
    {
        private DateTime lastDialogueTime;

        [Constructable]
        public Renn() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Renn the Goatherd";
            Body = 0x190; // Human male body

            // Stats and Appearance
            SetStr(80);
            SetDex(90);
            SetInt(100);
            SetHits(120);

            AddItem(new Shirt() { Hue = 0x455 });
            AddItem(new ShortPants() { Hue = 0x455 });
            AddItem(new QuarterStaff() { Name = "Renn's Caretaker Staff" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            lastDialogueTime = DateTime.MinValue;
        }

        public Renn(Serial serial) : base(serial) {}

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;
            
            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Renn—the gentle goatherd you see before you. At first glance, I am a humble caretaker of my flock, but the winds whisper of secrets in my past. " +
                "Between the bleating of my goats and the rustle of the fields, there lies a tale of charm, opportunism, and cautious survival. What would you like to discuss today?");

            greeting.AddOption("Tell me about your lost herd.", 
                player => true,
                player =>
                {
                    DialogueModule lostHerdModule = CreateLostHerdModule();
                    player.SendGump(new DialogueGump(player, lostHerdModule));
                });

            greeting.AddOption("What can you tell me about Kara Salt?", 
                player => true,
                player =>
                {
                    DialogueModule karaSaltModule = CreateKaraSaltModule();
                    player.SendGump(new DialogueGump(player, karaSaltModule));
                });

            greeting.AddOption("I heard about your friendly rivalry with Darvin.", 
                player => true,
                player =>
                {
                    DialogueModule rivalryModule = CreateRivalryModule();
                    player.SendGump(new DialogueGump(player, rivalryModule));
                });

            greeting.AddOption("Share some local lore with me—perhaps something about old soldier Edda?", 
                player => true,
                player =>
                {
                    DialogueModule eddaModule = CreateEddaModule();
                    player.SendGump(new DialogueGump(player, eddaModule));
                });

            greeting.AddOption("What is life like as a goatherd?", 
                player => true,
                player =>
                {
                    DialogueModule lifeModule = CreateLifeModule();
                    player.SendGump(new DialogueGump(player, lifeModule));
                });

            // NEW: Secret past branch
            greeting.AddOption("You seem to hide secrets behind those gentle eyes… Care to share more about your past?", 
                player => true,
                player =>
                {
                    DialogueModule secretModule = CreateHiddenPastModule();
                    player.SendGump(new DialogueGump(player, secretModule));
                });

            return greeting;
        }

        private DialogueModule CreateLostHerdModule()
        {
            DialogueModule lostHerd = new DialogueModule("My herd… they were more than mere animals. They were the vibrant pulse of my daily life until that fateful, furious storm shattered our harmony. " +
                "I still hear the soft echoes of their bells, a bittersweet lullaby of loss. What aspect troubles you?");
            
            lostHerd.AddOption("Have you any clues to find them?", 
                player => true, 
                player =>
                {
                    DialogueModule cluesModule = new DialogueModule("Whispers from the elders and stranded minstrels hint that near the crags of West Montor, in a secluded glen, the sound of bleating persists. " +
                        "Some say that the wind may still carry their secrets to those who dare to listen.");
                    cluesModule.AddOption("I'll follow this lead.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    cluesModule.AddOption("Sometimes the past is best left alone.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule sympathizeModule = new DialogueModule("I understand your caution; the past can be a relentless tide. May our paths meet under kinder skies, traveler.");
                            pl.SendGump(new DialogueGump(pl, sympathizeModule));
                        });
                    player.SendGump(new DialogueGump(player, cluesModule));
                });

            lostHerd.AddOption("Tell me more about that dreadful storm.", 
                player => true, 
                player =>
                {
                    DialogueModule stormModule = new DialogueModule("It was a night of ruthless chaos—thunder crashed like titans at war, and the sky wept torrents upon us. " +
                        "In that maelstrom, my cherished herd was swept away, leaving behind a void that still gnaws at my heart.");
                    stormModule.AddOption("How tragic...", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, stormModule));
                });
            return lostHerd;
        }

        private DialogueModule CreateKaraSaltModule()
        {
            DialogueModule karaSalt = new DialogueModule("Kara Salt—a name that sings of mystery and promise. I first encountered her on the bustling docks of Grey & Pirate Isle, where " +
                "the briny scent of the sea mingled with hope. Her compassion and wisdom lit up even the darkest nights of my wandering heart.");
            
            karaSalt.AddOption("How did you meet her?", 
                player => true,
                player =>
                {
                    DialogueModule meetModule = new DialogueModule("In the fog-laced twilight, as I roamed the harbor pondering my solitary fate, I found Kara cradling a wounded seagull. " +
                        "Her gentle words and kind eyes drew me in, and our conversation—full of subtle winks and quiet confidences—binds our destinies ever since.");
                    meetModule.AddOption("That is a heartwarming tale.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    meetModule.AddOption("Can she help you find your herd?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule hopeModule = new DialogueModule("I cling to hope that Kara, with her far-reaching networks and uncanny intuition, may have caught whispers of my herd. " +
                                "Sometimes, even the saltiest winds carry secrets of lost companions.");
                            pl.SendGump(new DialogueGump(pl, hopeModule));
                        });
                    player.SendGump(new DialogueGump(player, meetModule));
                });

            karaSalt.AddOption("What feelings do you harbor for her?", 
                player => true,
                player =>
                {
                    DialogueModule romanceModule = new DialogueModule("In her gaze, I glimpse both the mystery of tempestuous seas and a tender promise of a new dawn. " +
                        "There is a bittersweet intimacy in our unspoken bond—a connection carved through shared hardships and gentle smiles.");
                    romanceModule.AddOption("I sense a deep, unspoken connection.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, romanceModule));
                });

            return karaSalt;
        }

        private DialogueModule CreateRivalryModule()
        {
            DialogueModule rivalry = new DialogueModule("Darvin of West Montor and I share a rivalry as vibrant as the ripening fruits of summer. " +
                "Our contests over the juiciness of harvests and the veracity of local lore are playful yet charged with passion. " +
                "Despite the banter, there is mutual respect—a dance of wit and skill under the golden sun.");
            
            rivalry.AddOption("Tell me about your friendly competitions.", 
                player => true,
                player =>
                {
                    DialogueModule competitionModule = new DialogueModule("Season by season, Darvin and I engage in spirited challenges—whether debating the perfect orchard secret or having taste-offs over our produce. " +
                        "I believe that love for the land and an artist’s heart are the true measures of a farmer's worth. Our rivalry is as much about celebrating nature as it is about testing each other’s mettle.");
                    competitionModule.AddOption("That sounds invigorating!", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, competitionModule));
                });

            rivalry.AddOption("Is Darvin’s produce really that exceptional?", 
                player => true,
                player =>
                {
                    DialogueModule produceModule = new DialogueModule("Ah, Darvin’s fields yield apples as red as a sunset, and his squashes glisten like golden orbs in the light. " +
                        "Yet, I wager that while his technique is admirable, the secret ingredient is the love one pours into the soil—a quality no contest can truly measure.");
                    produceModule.AddOption("A taste-off would indeed be a delightful contest!", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, produceModule));
                });

            return rivalry;
        }

        private DialogueModule CreateEddaModule()
        {
            DialogueModule edda = new DialogueModule("Old soldier Edda—a weathered veteran whose tales of valor and sorrow echo through the ages. " +
                "His recollections of fierce battles and the whispered secrets of ancient springs have often lent me strength and guidance in my lonelier hours.");
            
            edda.AddOption("What tales has he shared with you?", 
                player => true,
                player =>
                {
                    DialogueModule talesModule = new DialogueModule("Edda speaks of epic clashes, secret pacts, and mystical springs that, it is said, bestow clarity upon the brave. " +
                        "Every tale is layered with honor, loss, and a whisper of magic—a tapestry woven of history and hope.");
                    talesModule.AddOption("How fascinating—a window into the past.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, talesModule));
                });

            edda.AddOption("Does he offer any advice for your trials?", 
                player => true,
                player =>
                {
                    DialogueModule adviceModule = new DialogueModule("Indeed he does. Edda’s counsel is simple yet profound: 'Let each loss carve wisdom into your soul, and let hope light your way.' " +
                        "His words remind me to hold the past as a guide, not a chain, even when shadows linger.");
                    adviceModule.AddOption("Wise words indeed.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, adviceModule));
                });
            return edda;
        }

        private DialogueModule CreateLifeModule()
        {
            DialogueModule life = new DialogueModule("A goatherd’s life is one of quiet routines and introspection. " +
                "At dawn, I lead my goats to verdant pastures, repairing fences and listening to the whispers of the wind. " +
                "Each day is a gentle meditation on nature’s cycles and the lessons learned from solitude. " +
                "Would you like to learn more about my routines or the wisdom harvested in these quiet moments?");
            
            life.AddOption("Tell me about your daily routines.", 
                player => true,
                player =>
                {
                    DialogueModule routineModule = new DialogueModule("When the first blush of sunrise colors the horizon, I begin my day by gathering my goats and tending the land. " +
                        "The cool dew on the grass and the tranquil murmur of nature set the stage for hours of honest labor, reflection, and remembrance of what was lost. " +
                        "Every chore is a ritual—a quiet homage to the enduring spirit of life.");
                    routineModule.AddOption("That sounds both humble and profound.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, routineModule));
                });

            life.AddOption("What wisdom have you gained from solitude?", 
                player => true,
                player =>
                {
                    DialogueModule wisdomModule = new DialogueModule("In the silence of dawn and the solitude of twilight, I have learned that every loss makes space for renewal. " +
                        "Even in my darkest hours, hope whispers from unexpected corners, urging us to seek new beginnings while cherishing old memories.");
                    wisdomModule.AddOption("Your words are very comforting.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, wisdomModule));
                });
            return life;
        }

        // NEW: A secret, heavily nested module revealing Renn's hidden past as a smuggler.
        private DialogueModule CreateHiddenPastModule()
        {
            DialogueModule hiddenPast = new DialogueModule("You have a keen eye, traveler. What you see as the humble guise of a goatherd is but a carefully crafted mask. " +
                "Long ago, I navigated treacherous trade routes as a cunning smuggler—charming my way through danger, taking opportunities when they arose, and ever so cautiously hiding my true self. " +
                "The seas of deceit and the dark corridors of illicit trade were my true realm. But I dare not speak too freely…");
            
            hiddenPast.AddOption("Tell me more about your smuggling days.", 
                player => true,
                player =>
                {
                    DialogueModule smugglerModule = new DialogueModule("Ah, the days of high stakes and hidden coves! I once carried contraband from secret ports—exotic spices, rare relics, and magical trinkets that others only dreamed of. " +
                        "It was a life of alluring danger and opportunistic deals. Every whispered secret and covert exchange was wrapped in charm and cloaked by caution. " +
                        "But these were times of survival more than greed—a necessary masquerade in a world where trust was as rare as true loyalty.");
                    
                    smugglerModule.AddOption("How did you become involved in that world?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule involvementModule = new DialogueModule("Necessity drove me into those shadowy realms. " +
                                "I discovered that beneath the veneer of respectable society lay a network of outlaws and adventurers. " +
                                "Charming strangers, spotting opportunities where others saw none, and always watching my back—that became my modus operandi. " +
                                "It was a life lived on the knife’s edge, where every decision could lead to fortune or ruin.");
                            
                            involvementModule.AddOption("It sounds like a dangerous game.", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule dangerModule = new DialogueModule("Indeed, danger lurks at every turn in the shadowed corridors of trade. " +
                                        "I learned that caution is a friend—never trust too openly, never reveal too much. " +
                                        "Yet, sometimes, a well-placed charm and a quick wit can open doors even in the darkest of alleys.");
                                    dangerModule.AddOption("Your caution is admirable.", 
                                        plx => true,
                                        plx => { plx.SendGump(new DialogueGump(plx, CreateHiddenPastModule())); });
                                    plc.SendGump(new DialogueGump(plc, dangerModule));
                                });
                            
                            involvementModule.AddOption("And what about the thrill of the trade?", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule thrillModule = new DialogueModule("Oh, the thrill! Every discreet meeting in a fogbound tavern, every hurried exchange under the cover of night—each moment was a rush like no other. " +
                                        "It was a chance to rewrite destiny, to seize opportunity when fortune smiled. Yet, behind every daring escapade lay the ever-present risk of exposure.");
                                    thrillModule.AddOption("I can almost feel the excitement!", 
                                        plx => true,
                                        plx => { plx.SendGump(new DialogueGump(plx, CreateHiddenPastModule())); });
                                    plc.SendGump(new DialogueGump(plc, thrillModule));
                                });
                            pl.SendGump(new DialogueGump(pl, involvementModule));
                        });
                    
                    smugglerModule.AddOption("Do you still walk that dark path?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule currentModule = new DialogueModule("I must admit, the call of that dangerous life still lingers like a half-remembered dream. " +
                                "But I now walk a narrower path, using my skills to help those in need while keeping my past shrouded. " +
                                "I return to those shadowed trade routes only when necessity demands, ever careful, ever cautious.");
                            
                            currentModule.AddOption("What makes you take those risks now?", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule riskModule = new DialogueModule("Opportunity and duty. Sometimes, the underbelly of the world requires a gentle hand to set things right. " +
                                        "I seize chances that come my way—not for profit alone, but to balance the scales of fate. However, every step is measured, every deal weighed with caution.");
                                    riskModule.AddOption("A delicate balancing act indeed.", 
                                        plx => true,
                                        plx => { plx.SendGump(new DialogueGump(plx, CreateHiddenPastModule())); });
                                    plc.SendGump(new DialogueGump(plc, riskModule));
                                });
                            
                            currentModule.AddOption("You must be very cautious with your secrets.", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule cautionModule = new DialogueModule("Caution has been my greatest ally. In a world where every smile can hide a dagger and every friendly word may be a trap, " +
                                        "I learned long ago to guard my secrets with all the care they deserve. Trust is rare—and once given, must be cherished beyond measure.");
                                    cautionModule.AddOption("Your vigilance is remarkable.", 
                                        plx => true,
                                        plx => { plx.SendGump(new DialogueGump(plx, CreateHiddenPastModule())); });
                                    plc.SendGump(new DialogueGump(plc, cautionModule));
                                });
                            pl.SendGump(new DialogueGump(pl, currentModule));
                        });
                    
                    smugglerModule.AddOption("Your charm must have opened many doors.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule charmModule = new DialogueModule("Indeed, charm was my currency in those dark corridors of trade. A disarming smile, a clever turn of phrase—they were as potent as gold when dealing with the unpredictable. " +
                                "Yet, every charm employed was tempered with a dose of wariness. It was never a free gift, but a measured tactic to gain trust and slip through guarded gates.");
                            charmModule.AddOption("Your tales are as captivating as they are mysterious.", 
                                plx => true,
                                plx => { plx.SendGump(new DialogueGump(plx, CreateHiddenPastModule())); });
                            pl.SendGump(new DialogueGump(pl, charmModule));
                        });
                    
                    smugglerModule.AddOption("I appreciate your openness, though your past remains shrouded in mystery.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule finalModule = new DialogueModule("Mystery is the very essence of survival. Some truths are better kept hidden in the folds of time, revealed only to those who earn them. " +
                                "Remember, traveler, not every secret is a burden—sometimes, it is a shield that protects us in a world rife with peril.");
                            finalModule.AddOption("I understand. Your life is an inspiration.", 
                                plx => true,
                                plx => { plx.SendGump(new DialogueGump(plx, CreateGreetingModule())); });
                            pl.SendGump(new DialogueGump(pl, finalModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, smugglerModule));
                });

            hiddenPast.AddOption("I must admit, your hidden past only deepens your allure.", 
                player => true,
                player =>
                {
                    DialogueModule allureModule = new DialogueModule("Flattery will get you far in shadowy circles, traveler. " +
                        "There is a certain allure in a life lived on the edge—a charm in the very act of survival. " +
                        "But know this: every compliment, like every secret, is a double-edged sword.");
                    allureModule.AddOption("I appreciate your candor and caution.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateHiddenPastModule())); });
                    hiddenPast.AddOption("Tell me more about the risks of that life.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule riskDetails = new DialogueModule("Risks were a constant companion on those perilous trade routes. " +
                                "One misstep could lead to betrayal, capture, or worse. It was a realm where alliances were fleeting and every decision had its price. " +
                                "I learned to weigh each opportunity with a careful measure—a lesson in both opportunism and caution.");
                            riskDetails.AddOption("I see, every secret comes at a cost.", 
                                plx => true,
                                plx => { plx.SendGump(new DialogueGump(plx, CreateHiddenPastModule())); });
                            pl.SendGump(new DialogueGump(pl, riskDetails));
                        });
                    player.SendGump(new DialogueGump(player, allureModule));
                });

            hiddenPast.AddOption("I should leave you to your thoughts for now.", 
                player => true,
                player =>
                {
                    DialogueModule exitModule = new DialogueModule("Very well, traveler. Some secrets are meant to be cherished in silence. " +
                        "Remember, the shadows hold as much wisdom as the light. May our paths cross again when fate deems it so.");
                    exitModule.AddOption("Farewell, Renn.", 
                        pl => true,
                        pl => { pl.SendGump(new DialogueGump(pl, CreateGreetingModule())); });
                    player.SendGump(new DialogueGump(player, exitModule));
                });
            
            return hiddenPast;
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
