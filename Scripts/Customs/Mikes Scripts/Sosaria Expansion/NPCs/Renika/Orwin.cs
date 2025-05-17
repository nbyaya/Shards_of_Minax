using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Orwin the Deep Diver")]
    public class Orwin : BaseCreature
    {
        [Constructable]
		public Orwin() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Orwin the Deep Diver";
            Body = 0x190; // Human male body
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Set basic stats – adjust as needed
            SetStr(120);
            SetDex(80);
            SetInt(90);
            SetHits(100);

            // Outfit
            AddItem(new Shirt() { Hue = 0x497 });
            AddItem(new TricorneHat() { Hue = 0x497, Name = "Navigator’s Hat" });
            AddItem(new Sandals() { Hue = 0x497 });
			AddItem(new ShortPants() { Hue = 1175 });
            AddItem(new Backpack()); // Symbolizes his diving gear

            // Additional items to hint at his dual nature
            AddItem(new LeatherCap() { Name = "Diving Mask" }); // Custom item type
            AddItem(new GoldRing() { Name = "Ring of the Abyss", Hue = 0x21 }); // A secret relic
        }

        public Orwin(Serial serial) : base(serial) { }

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
                "Ahoy, traveler! I am Orwin, deep diver and wanderer of uncharted seas. " +
                "Many know me for my tales of sunken relics and fierce rivals like Galven, " +
                "but few know of the fervor burning in my heart—a secret past as a zealous priest, " +
                "dedicated to an enigmatic, controversial deity whose mysteries have gripped my soul. " +
                "What would you like to hear about?");

            // Option 1: Underwater Adventures
            greeting.AddOption("Tell me about your underwater discoveries.",
                player => true,
                player =>
                {
                    DialogueModule discoveryModule = new DialogueModule(
                        "The ocean is my sanctuary and my battleground. I have witnessed ethereal shipwrecks " +
                        "and discovered ancient ruins swirling with long-forgotten power. " +
                        "But beneath these wonders lie secrets that stir my inner calling. " +
                        "Would you like to discuss relics, strange beasts, or perhaps a more personal account?");
                    
                    discoveryModule.AddOption("Tell me about the sunken relics.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule relicsModule = new DialogueModule(
                                "Sunken relics, you say? Among the coral-entwined wrecks, I have found stone tablets, " +
                                "ornate chalices, and mysterious idols. One such idol whispers of the deity I secretly serve—" +
                                "its design mirrors forbidden symbols that only the chosen comprehend. " +
                                "I compare these findings with maps shared by Big Harv. Intriguing, wouldn't you agree?");
                            
                            relicsModule.AddOption("How do these relics connect to your secret faith?",
                                p => true,
                                p =>
                                {
                                    DialogueModule faithModule = new DialogueModule(
                                        "Ah, you have a discerning eye. In the cold dark of the deep, " +
                                        "I found an idol that pulsed with an otherworldly glow. It spoke in symbols " +
                                        "of a deity long shunned by many. My faith in this controversial power has driven me " +
                                        "to seek its truth beneath every wave. Do you wish to know more about this deity?");
                                    
                                    faithModule.AddOption("Yes, tell me about this forbidden deity.",
                                        p1 => true,
                                        p1 =>
                                        {
                                            DialogueModule deityModule = new DialogueModule(
                                                "The deity, known in whispers as Thalassor, governs both the beauty and terror " +
                                                "of the ocean's depths. Some call Thalassor the Abyssal Lord, a bringer of both salvation and doom. " +
                                                "I have devoted my soul to interpreting his signs. His call is persuasive, almost irresistible—" +
                                                "and I am but a humble servant. Would you dare walk a part of this mysterious path?");
                                            
                                            deityModule.AddOption("I remain curious. Continue.",
                                                p2 => true,
                                                p2 =>
                                                {
                                                    DialogueModule continueModule = new DialogueModule(
                                                        "In the secret temple of ruins beneath the seabed, I once witnessed an arcane ritual " +
                                                        "that shattered my understanding of the natural world. The fervor of the chant, the visionary " +
                                                        "symbols—these events forged me into a fanatic believer. I cannot forsake my destiny " +
                                                        "even if the world condemns it. This revelation changed everything for me.");
                                                    p2.SendGump(new DialogueGump(p2, continueModule));
                                                });
                                            
                                            deityModule.AddOption("I do not wish to follow this path.",
                                                p2 => true,
                                                p2 =>
                                                {
                                                    DialogueModule renounceModule = new DialogueModule(
                                                        "It is your right to choose, traveler. But remember, the call of Thalassor stirs even the coldest heart. " +
                                                        "Perhaps one day you shall hear his whisper, drawing you into the mystery that binds us all.");
                                                    p2.SendGump(new DialogueGump(p2, renounceModule));
                                                });
                                            
                                            p1.SendGump(new DialogueGump(p1, deityModule));
                                        });
                                    
                                    faithModule.AddOption("I believe in mortal adventures, not divine mandates.",
                                        p1 => true,
                                        p1 =>
                                        {
                                            DialogueModule skepticModule = new DialogueModule(
                                                "A fair sentiment for many, yet the ocean itself sings hymns that defy mortal reason. " +
                                                "I am zealous in my beliefs, and every dive is a step deeper into both wonder and conviction. " +
                                                "Keep your mind open, for the tides reveal secrets to those who listen.");
                                            p1.SendGump(new DialogueGump(p1, skepticModule));
                                        });
                                    
                                    p.SendGump(new DialogueGump(p, faithModule));
                                });
                            
                            relicsModule.AddOption("I am content with just marveling at the relics.",
                                p => true,
                                p =>
                                {
                                    DialogueModule marvelModule = new DialogueModule(
                                        "Marvel then at the lost works of an age where gods and mortals intertwined beneath the waves. " +
                                        "Every relic is a silent testament to an era of divine mystery and mortal ambition. " +
                                        "I urge you to see them not just as lost artifacts, but as echoes of a forgotten destiny.");
                                    p.SendGump(new DialogueGump(p, marvelModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, relicsModule));
                        });
                    
                    discoveryModule.AddOption("What about the creatures that guard these treasures?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule creaturesModule = new DialogueModule(
                                "The deep is not only a crypt of relics but also a realm of living legends. " +
                                "I have encountered luminous schools of fish, eerie jellyfish, and even colossal guardians " +
                                "that seem to be imbued with Thalassor's own will. Would you like to hear about a specific encounter?");
                            
                            creaturesModule.AddOption("Relate an encounter with a guardian beast.",
                                p => true,
                                p =>
                                {
                                    DialogueModule beastModule = new DialogueModule(
                                        "During one perilous dive in the Abyss of Whispers, I found myself shadowed by a titanic creature " +
                                        "with eyes that glowed like the embers of an ancient ritual. Its presence was both terrifying and " +
                                        "exhilarating—a reminder that nature obeys the divine law of Thalassor. I swam with both fear and fervor. " +
                                        "Do you dare imagine facing such a creature yourself?");
                                    
                                    beastModule.AddOption("I would face any beast for the thrill of discovery.",
                                        p1 => true,
                                        p1 =>
                                        {
                                            DialogueModule thrillModule = new DialogueModule(
                                                "Then prepare yourself to witness the unimaginable, for the sea does not cater to the faint of heart. " +
                                                "Every dive brings a test of both spirit and resolve.");
                                            p1.SendGump(new DialogueGump(p1, thrillModule));
                                        });
                                    
                                    beastModule.AddOption("The idea chills my bones.",
                                        p1 => true,
                                        p1 =>
                                        {
                                            DialogueModule cautionModule = new DialogueModule(
                                                "It is wise to be cautious. Yet, in every shiver lies a spark of truth—one that can ignite a passionate flame " +
                                                "if you choose to embrace it fully.");
                                            p1.SendGump(new DialogueGump(p1, cautionModule));
                                        });
                                    
                                    p.SendGump(new DialogueGump(p, beastModule));
                                });
                            
                            creaturesModule.AddOption("I wish to hear more about your personal connection with the deep.",
                                p => true,
                                p =>
                                {
                                    DialogueModule personalModule = new DialogueModule(
                                        "Every dive unravels not just the mysteries of the ocean but also the mysteries within my own soul. " +
                                        "I have learned that beneath the waves, amid silence and pressure, lies the echo of a divine calling. " +
                                        "It compels me to spread the true word of Thalassor. Do you sense the passion behind my pursuit?");
                                    
                                    personalModule.AddOption("Your passion is overwhelming—explain further.",
                                        p1 => true,
                                        p1 =>
                                        {
                                            DialogueModule explainModule = new DialogueModule(
                                                "I was once but a simple diver, until the day the ocean spoke to me in a vision. " +
                                                "A storm, a flash of divine light, and there emerged Thalassor—a deity revered by few, condemned by many. " +
                                                "Since that fateful day, I became zealous, my soul forever intertwined with his mysterious mandate. " +
                                                "I have since sought to persuade others of the sacred purpose that lies beneath these rolling depths.");
                                            p1.SendGump(new DialogueGump(p1, explainModule));
                                        });
                                    
                                    personalModule.AddOption("I sense you hide more than you reveal.",
                                        p1 => true,
                                        p1 =>
                                        {
                                            DialogueModule secretModule = new DialogueModule(
                                                "You are perceptive. I carry a secret burden—a faith so fervent that it borders on fanaticism. " +
                                                "But my duty, as I see it, is to share the divine truth with those brave enough to listen. " +
                                                "The ocean’s call is persuasive, and every hidden corner of its depths sings of his power.");
                                            p1.SendGump(new DialogueGump(p1, secretModule));
                                        });
                                    
                                    p.SendGump(new DialogueGump(p, personalModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, creaturesModule));
                        });
                    
                    discoveryModule.AddOption("I sense a hidden agenda behind your tales.",
                        p => true,
                        p =>
                        {
                            DialogueModule agendaModule = new DialogueModule(
                                "Ah, you are not easily fooled. Beneath the surface of my adventures lies a secret agenda—a divine crusade dedicated to Thalassor. " +
                                "I am driven by a purpose that many would shun, yet I remain persuasive in my quest to awaken others to this truth. " +
                                "Do you wish to know how my faith intertwines with my life as a diver?");
                            
                            agendaModule.AddOption("Yes, reveal the intertwining of faith and duty.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule faithDutyModule = new DialogueModule(
                                        "Every ripple, every shadow in the ocean speaks of Thalassor’s will. My dives are acts of worship, " +
                                        "each discovery a sermon, and every relic a chapter in the divine gospel that I preach. " +
                                        "This path is not for the faint-hearted—it is for those willing to sacrifice comfort for truth.");
                                    
                                    faithDutyModule.AddOption("How do you reconcile your dual life?",
                                        p2 => true,
                                        p2 =>
                                        {
                                            DialogueModule dualLifeModule = new DialogueModule(
                                                "It is a constant struggle—between the lure of adventure and the intense call of my deity. " +
                                                "Yet, I have learned that the deep does not simply reveal treasures—it also refines the soul. " +
                                                "By embracing both roles, I become more than a mere diver: I am an apostle of the abyss.");
                                            p2.SendGump(new DialogueGump(p2, dualLifeModule));
                                        });
                                    
                                    faithDutyModule.AddOption("What drives you to convert others?",
                                        p2 => true,
                                        p2 =>
                                        {
                                            DialogueModule convertModule = new DialogueModule(
                                                "I believe the power of persuasion is divine. " +
                                                "It compels me to show others that beneath our mundane existence lies a mystical force—a truth that can change destiny. " +
                                                "My zeal is not borne of fanaticism alone, but of a deep, abiding love for the transformative power of Thalassor.");
                                            p2.SendGump(new DialogueGump(p2, convertModule));
                                        });
                                    
                                    p1.SendGump(new DialogueGump(p1, faithDutyModule));
                                });
                            
                            agendaModule.AddOption("I remain skeptical, but your fervor is palpable.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule skepticFervor = new DialogueModule(
                                        "Skepticism is the first step to enlightenment, or so I say. " +
                                        "Even if you doubt, you cannot ignore the persuasive call of the deep. " +
                                        "I encourage you to listen to the subtle hymns of the ocean—they may yet change your heart.");
                                    p1.SendGump(new DialogueGump(p1, skepticFervor));
                                });
                            
                            p.SendGump(new DialogueGump(p, agendaModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, discoveryModule));
                });

            // Option 2: Rivalry with Galven and Friendly Banter
            greeting.AddOption("What is this rivalry with Galven all about?",
                player => true,
                player =>
                {
                    DialogueModule rivalryModule = new DialogueModule(
                        "Ah, Galven—my esteemed adversary from Renika! Our rivalry is not born of animosity but of mutual respect and fiery debate. " +
                        "While I rely on intuition and the enigmatic pull of the deep, Galven is methodical and obsessed with technical precision. " +
                        "Our banter often turns into spirited discussions at the tavern. Which side of our rivalry interests you?");
                    
                    rivalryModule.AddOption("Tell me about your opposing methods.",
                        p => true,
                        p =>
                        {
                            DialogueModule methodsModule = new DialogueModule(
                                "I follow the currents like a prophet following divine visions—letting the ocean guide my every move. " +
                                "Galven, however, charts his course as if every dive were a calculated maneuver. " +
                                "In our debates, I argue that passion and faith must lead exploration, while he clings to cold, hard science. " +
                                "Do you think reason alone can capture the soul of the sea?");
                            
                            methodsModule.AddOption("Passion is the true compass.",
                                p1 => true,
                                p1 => p1.SendGump(new DialogueGump(p1, CreateGreetingModule())));
                            
                            methodsModule.AddOption("Scientific method is the only path.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule scienceModule = new DialogueModule(
                                        "A valid perspective indeed, though I argue that numbers and charts can never capture the epiphany of a divine encounter beneath the waves. " +
                                        "Galven and I often debate late into the night, our words clashing like titanic waves.");
                                    p1.SendGump(new DialogueGump(p1, scienceModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, methodsModule));
                        });
                    
                    rivalryModule.AddOption("Share an anecdote from your rivalry with Galven.",
                        p => true,
                        p =>
                        {
                            DialogueModule anecdoteModule = new DialogueModule(
                                "I recall a day in the treacherous Blue Abyss—a dive so perilous that even the ocean held its breath. " +
                                "Galven and I raced to recover a legendary conch said to contain divine whispers. " +
                                "In the end, we both emerged with fragments of the artifact and laughter echoing through the deep. " +
                                "It was a moment where rivalry transcended competition, and our shared passion united us, if only for that breathless moment.");
                            
                            anecdoteModule.AddOption("That sounds like a harmonious duel.",
                                p1 => true,
                                p1 => p1.SendGump(new DialogueGump(p1, CreateGreetingModule())));
                            
                            anecdoteModule.AddOption("I prefer solitary quests.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule solitaryModule = new DialogueModule(
                                        "Ah, the solitude of the deep can be both a blessing and a curse. " +
                                        "For me, every dive is a blend of camaraderie and personal conviction. " +
                                        "Perhaps one day, you too will discover that even rivalry can illuminate a hidden truth.");
                                    p1.SendGump(new DialogueGump(p1, solitaryModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, anecdoteModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, rivalryModule));
                });

            // Option 3: Conversations about Art, Faith, and Serra’s Influence
            greeting.AddOption("How do you discuss ocean mysteries with Serra?",
                player => true,
                player =>
                {
                    DialogueModule serraModule = new DialogueModule(
                        "Serra is no ordinary artist; she channels the visions I receive from the deep—visions that often bear the mark of my hidden faith. " +
                        "In our conversations, art merges with divine revelation. I recount tales of eerie underwater palaces and the siren calls of Thalassor, " +
                        "and she transforms them into sculptures that defy mortal understanding. What facet of our collaboration piques your interest?");
                    
                    serraModule.AddOption("Tell me about a recent collaboration with Serra.",
                        p => true,
                        p =>
                        {
                            DialogueModule collabModule = new DialogueModule(
                                "Not long ago, I dove into a chasm pulsing with ancient magic and emerged with strange, luminescent markings. " +
                                "I shared every detail with Serra, who, with a zeal bordering on fanaticism, crafted a mural of ethereal hues—a blend of oceanic wonder and divine prophecy. " +
                                "The artwork not only astonished the local scholars but also stoked rumors of a mysterious cult. Fascinating, wouldn't you say?");
                            
                            collabModule.AddOption("Yes, I’m intrigued by the divine aspect.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule divineModule = new DialogueModule(
                                        "Indeed, the divine runs deep in these waters. " +
                                        "Each brushstroke of Serra’s art echoes the fervor with which I pursue the sacred teachings of Thalassor. " +
                                        "Her art has become a beacon, persuading even the most skeptical to look beyond the mundane. " +
                                        "Are you ready to be persuaded by the call of the abyss?");
                                    
                                    divineModule.AddOption("Show me how deep the devotion runs.",
                                        p2 => true,
                                        p2 =>
                                        {
                                            DialogueModule devotionModule = new DialogueModule(
                                                "Every dive I undertake is an act of faith. In the heart of the darkest trench, I hear the hymns of an ancient power calling me. " +
                                                "I have dedicated my life—nay, my very soul—to the deity Thalassor. " +
                                                "His name is whispered by the currents and carried on the winds of fate. " +
                                                "Embrace this revelation, and you might just glimpse the divine.");
                                            p2.SendGump(new DialogueGump(p2, devotionModule));
                                        });
                                    
                                    divineModule.AddOption("My mind is still unready to accept such fervor.",
                                        p2 => true,
                                        p2 =>
                                        {
                                            DialogueModule reluctantModule = new DialogueModule(
                                                "I understand skepticism. But every legend begins as but a murmur—until the call becomes irresistible. " +
                                                "Keep listening to the subtle cadence of the deep, and perhaps one day the truth will reveal itself to you.");
                                            p2.SendGump(new DialogueGump(p2, reluctantModule));
                                        });
                                    
                                    p1.SendGump(new DialogueGump(p1, divineModule));
                                });
                            
                            collabModule.AddOption("I am only curious about the art, not your faith.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule artModule = new DialogueModule(
                                        "Serra's art is a wondrous enigma—an interplay of color, light, and the raw force of the ocean's mystery. " +
                                        "Even without the divine twist, her work speaks to the soul, revealing layers of untold history and hidden passion. " +
                                        "Yet, truth be told, every masterpiece has a secret, and mine happens to be wrapped in divine conviction.");
                                    p1.SendGump(new DialogueGump(p1, artModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, collabModule));
                        });
                    
                    serraModule.AddOption("What inspires Serra’s art?",
                        p => true,
                        p =>
                        {
                            DialogueModule inspireModule = new DialogueModule(
                                "Serra finds inspiration in the ceaseless ebb and flow of the ocean. " +
                                "But more than that, she sees in each ripple and sunken ruin the reflection of a higher power—one that I, in my secret zeal, serve. " +
                                "Her art is like a persuasive sermon without words, compelling all who gaze upon it to ponder the mysteries of the deep. " +
                                "Would you like to know the legend behind her latest piece?");
                            
                            inspireModule.AddOption("Yes, reveal its secret legend.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule legendModule = new DialogueModule(
                                        "The piece is called 'The Abyssal Benediction.' It portrays a convergence of light and shadow, where an otherworldly deity emerges " +
                                        "from the depths to bless those who dare seek the truth. Every curve and hue is meticulously designed to invoke both beauty and fear, " +
                                        "a testimony to the persuasive power of the divine. It is, in essence, a call to all lost souls.");
                                    p1.SendGump(new DialogueGump(p1, legendModule));
                                });
                            
                            inspireModule.AddOption("I prefer to admire without knowing the secrets.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule admireModule = new DialogueModule(
                                        "Then simply let your eyes and heart absorb the wonder of her creation. " +
                                        "Sometimes, the absence of secrets makes the art all the more alluring, as you are free to create your own interpretation.");
                                    p1.SendGump(new DialogueGump(p1, admireModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, inspireModule));
                        });
                    
                    serraModule.AddOption("Your tone reveals a hidden zeal. What of your secret past?",
                        p => true,
                        p =>
                        {
                            DialogueModule secretModule = new DialogueModule(
                                "I see you are perceptive. There are nights when the ocean’s call transforms into a divine command—a command that once led me down a path " +
                                "of sacred fanaticism. I was, in another life, a priest of Thalassor, preaching to those brave enough to embrace the unknown. " +
                                "That past, though shrouded in controversy, fuels my every dive. Would you dare explore the details of that secret history?");
                            
                            secretModule.AddOption("I wish to know every detail of your past.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule pastModule = new DialogueModule(
                                        "Long ago, when I was younger and less burdened by doubt, I wandered into a hidden temple deep beneath turbulent waters. " +
                                        "There, amidst chanting echoes and flickering torches, I embraced a faith that many would call fanatical. " +
                                        "It was there that I took an oath—to spread the mysterious gospel of Thalassor. " +
                                        "Every relic I now recover is a reminder of that sacred bond. Can you fathom the transformation of a mortal soul into a zealot?");
                                    
                                    pastModule.AddOption("Your transformation is compelling. How do you persuade others?",
                                        p2 => true,
                                        p2 =>
                                        {
                                            DialogueModule persuadeModule = new DialogueModule(
                                                "I speak with a fervor that borders on divine madness. I recount my visions, the overwhelming signs in every storm and calm, " +
                                                "and I weave my words like a spell. My conviction is not merely for my own salvation but to pull the lost towards the abyssal light of Thalassor. " +
                                                "It is a persuasion earned through personal sacrifice and revelations that shatter the mundane. " +
                                                "Are you ready to be swayed by passion?");
                                            p2.SendGump(new DialogueGump(p2, persuadeModule));
                                        });
                                    
                                    pastModule.AddOption("I remain wary of such fanaticism.",
                                        p2 => true,
                                        p2 =>
                                        {
                                            DialogueModule waryModule = new DialogueModule(
                                                "Wary, indeed, but I assure you—the truth does not always come softly. " +
                                                "The fanatical nature of my past was born of a divine need, one that burns through the veils of doubt. " +
                                                "Even if you cannot follow my path, let my fervor inspire you to seek your own truth in the whispers of the deep.");
                                            p2.SendGump(new DialogueGump(p2, waryModule));
                                        });
                                    
                                    p1.SendGump(new DialogueGump(p1, pastModule));
                                });
                            
                            secretModule.AddOption("Some truths are best left hidden.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule hiddenModule = new DialogueModule(
                                        "Perhaps so. But know this: every dive I make is imbued with that old, fervent call. " +
                                        "Even if I do not reveal its full secrets, the whispers of Thalassor persist in my actions and in every artifact I unearth.");
                                    p1.SendGump(new DialogueGump(p1, hiddenModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, secretModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, serraModule));
                });

            // Option 4: Seeking Practical Guidance for Deep Dives
            greeting.AddOption("I need some advice on preparing for deep dives.",
                player => true,
                player =>
                {
                    DialogueModule adviceModule = new DialogueModule(
                        "Ah, preparation is key when venturing into the deep. I always ensure my gear is flawless and my mind is steeled by both adventure and divine purpose. " +
                        "Do you need guidance on equipment, navigation, or perhaps some wisdom borne of my own zealous path?");
                    
                    adviceModule.AddOption("Give me tips on gear and navigation.",
                        p => true,
                        p =>
                        {
                            DialogueModule gearModule = new DialogueModule(
                                "Never underestimate the value of a well-maintained diving mask, robust fins, and a waterproof journal. " +
                                "I often jot down coordinates and divine signs that guide my course. " +
                                "The ocean itself becomes your compass if you learn to listen.");
                            
                            gearModule.AddOption("I also want to learn about mystical navigation.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule mysticModule = new DialogueModule(
                                        "There are moments when mere instruments are not enough. In those instances, I invoke the memory of ancient rites and " +
                                        "the passionate call of Thalassor to align my path with the unseen currents. Let the tide and your heart be your guides.");
                                    p1.SendGump(new DialogueGump(p1, mysticModule));
                                });
                            
                            p.SendGump(new DialogueGump(p, gearModule));
                        });
                    
                    adviceModule.AddOption("What about safety protocols?",
                        p => true,
                        p =>
                        {
                            DialogueModule safetyModule = new DialogueModule(
                                "Safety is paramount when facing the might of the ocean. Always dive with a trusted companion, signal your intentions clearly, " +
                                "and never ignore the subtle shifts in the water. My enchanted compass, a gift from Big Harv, once saved my life by pointing to " +
                                "a safe haven amid a turbulent surge. Do you require more practical pointers or a word of divine caution?");
                            
                            safetyModule.AddOption("More practical tips, please.",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule practicalModule = new DialogueModule(
                                        "Ensure all your equipment is in top condition before every dive—scrutinize each strap, valve, and gauge. " +
                                        "Map out your planned route, and always inform someone of your entry and exit points. A well-prepared diver is a survivor.");
                                    p1.SendGump(new DialogueGump(p1, practicalModule));
                                });
                            
                            safetyModule.AddOption("Perhaps a divine warning?",
                                p1 => true,
                                p1 =>
                                {
                                    DialogueModule divineWarning = new DialogueModule(
                                        "Listen well! The ocean speaks in omens. When the water grows unnaturally still or the currents whisper in tongues, " +
                                        "heed the warning. Sometimes, fate itself commands you to retreat. Such signs are blessings in disguise, saving you from unseen calamity.");
                                    p1.SendGump(new DialogueGump(p1, divineWarning));
                                });
                            
                            p.SendGump(new DialogueGump(p, safetyModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, adviceModule));
                });

            // Option 5: Farewell (Ends the conversation)
            greeting.AddOption("I have nothing more to ask. Farewell.",
                player => true,
                player =>
                {
                    // End conversation and return to greeting for a new start if needed.
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
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
