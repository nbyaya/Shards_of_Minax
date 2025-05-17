using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the silent remains of Thistle")]
    public class Thistle : BaseCreature
    {
        [Constructable]
        public Thistle() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Thistle";
            Body = 0x190; // Using a human male body as a base

            // Set basic stats: agile, clever, and full of inner ambition
            SetStr(90);
            SetDex(80);
            SetInt(100);
            SetHits(80);

            // Appearance: A shadowy figure with a weathered cloak and discreet attire – hints of nobility in his bearing
            AddItem(new Cloak() { Hue = 1175, Name = "Shadow Cloak" });
            AddItem(new Shirt() { Hue = 0x455 });
            AddItem(new Boots() { Hue = 0x455 });
			AddItem(new LongPants() { Hue = 1150 });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Thistle(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Eh, careful now — you’ve got that look in your eyes, the kind that whispers you’re not like the rest of the flock. I'm Thistle, the courier of whispers and secrets. I keep the words of the unseen alive. From Mino in East Montor, whose coded missives carry ancient lore, to Lilo on Pirate Isle with his uncanny knack for hidden routes, and Willow in West Montor, whose discretion ensures mysterious packages vanish into the night — that's my trade. But perhaps you suspect there’s more beneath the cloak?");
            
            // Option 1: Secret Network & Routes
            greeting.AddOption("Tell me about your secret network and the routes you use.", 
                player => true,
                player =>
                {
                    DialogueModule networkModule = new DialogueModule("My network is as sprawling and secret as the forest's undergrowth. Every whisper and coded scrap finds its way through hidden channels. Mino, the keeper of ancient lore, sends messages laden with mystic secrets from East Montor, while Lilo, that audacious rogue on Pirate Isle, navigates treacherous waters and dark coves. And Willow? She's the iron fist in West Montor, ensuring that even the most dangerous packages pass unnoticed.");
                    
                    networkModule.AddOption("Who exactly is Mino, and what secrets does he keep?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule minoModule = new DialogueModule("Mino is no mere scholar; he is a keeper of arcane codes. His messages hint at relics that might twist fate itself. Few understand the gravity hidden within his cryptic notes — and fewer yet are trusted to carry them.");
                            pl.SendGump(new DialogueGump(pl, minoModule));
                        });
                    networkModule.AddOption("What about Lilo? I need to know more about his tactics.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule liloModule = new DialogueModule("Lilo is a maverick from the coasts, a roguish soul who’s mastered every hidden cove on Pirate Isle. His methods border on the theatrical, but they’re as effective as they are daring. His loyalty is hard-won, yet his connections make him indispensable.");
                            pl.SendGump(new DialogueGump(pl, liloModule));
                        });
                    networkModule.AddOption("And Willow? What makes her so reliable?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule willowModule = new DialogueModule("Willow is the unyielding anchor of West Montor. With a keen eye for discretion and a myriad of contacts, she ensures our most sensitive dispatches fade into the mundane. Her reliability is legendary among those who dwell in the shadows.");
                            pl.SendGump(new DialogueGump(pl, willowModule));
                        });
                    networkModule.AddOption("Let's return to your other secrets.", 
                        pl => true,
                        pl => player.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, networkModule));
                });

            // Option 2: Handling Secret Messages
            greeting.AddOption("What do you do with all those secret messages you carry?", 
                player => true,
                player =>
                {
                    DialogueModule messagesModule = new DialogueModule("The messages are the lifeblood of our covert world. Each note is embedded with clues — ancient curses, forbidden lore, and warnings that ripple through the undercurrents of power. Every scrap is a puzzle, and I am the one who must ensure the pieces find their place in the grand design.");
                    
                    messagesModule.AddOption("Have any messages ever altered the course of events?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule eventModule = new DialogueModule("Indeed. Once, a simple coded warning about an impending raid spurred a cascade of events that saved a hidden stronghold from ruin. It’s astounding how a whisper in the dark can ignite change in the realm.");
                            pl.SendGump(new DialogueGump(pl, eventModule));
                        });
                    messagesModule.AddOption("Ever come dangerously close to having your messages intercepted?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule dangerModule = new DialogueModule("Danger is always a step behind — shadows that never leave you. Rival couriers and double agents lurk in every dark alley, but with nerves of steel and careful planning, I’ve always managed to outwit them.");
                            pl.SendGump(new DialogueGump(pl, dangerModule));
                        });
                    messagesModule.AddOption("Let's talk about something else.", 
                        pl => true,
                        pl => player.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, messagesModule));
                });

            // Option 3: Mysterious Package Deliveries
            greeting.AddOption("I heard you also handle mysterious package deliveries. What's the story there?", 
                player => true,
                player =>
                {
                    DialogueModule packagesModule = new DialogueModule("Ah, the packages... They are far more than mere parcels. Some contain rare alchemical ingredients, others hidden relics that defy explanation. Each item is wrapped in enchantments and secrecy, destined for those who understand that knowledge is power. The very act of delivery is a ritual — precise, dangerous, and exhilarating.");
                    
                    packagesModule.AddOption("Ever encountered close calls during a delivery?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule perilModule = new DialogueModule("Many times! I recall a delivery where a rival courier was hot on my heels — a nerve-wracking chase through forgotten tunnels and secret corridors. Each narrow escape is a reminder of the ever-present dangers of our clandestine trade.");
                            pl.SendGump(new DialogueGump(pl, perilModule));
                        });
                    packagesModule.AddOption("What kinds of items are we talking about here?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule itemsModule = new DialogueModule("The items are as varied as the rumors of Sosaria. From enchanted trinkets to precious alchemical reagents and even scrolls that murmur secrets of lost civilizations — every package carries its own mystery and weight.");
                            pl.SendGump(new DialogueGump(pl, itemsModule));
                        });
                    packagesModule.AddOption("Let's return to discussing your life in the shadows.", 
                        pl => true,
                        pl => player.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, packagesModule));
                });

            // Option 4: The Risks of the Trade
            greeting.AddOption("Your life seems full of risks. How do you manage to survive out there?", 
                player => true,
                player =>
                {
                    DialogueModule riskModule = new DialogueModule("Risk is the very pulse of our existence. Every step in this shadowed dance is measured and deliberate. Guided by the quiet wisdom of Mino, the daring routes of Lilo, and the steadfastness of Willow, I've learned that fear must be mastered — not banished. Every scar, every misstep, only sharpens my resolve.");
                    
                    riskModule.AddOption("Do you ever find the danger overwhelming?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule worryModule = new DialogueModule("Overwhelming? No. In our world, caution is more valuable than gold. I calculate every risk, and my experience in the dark arts of subterfuge ensures that even the gravest perils serve as stepping stones towards a greater destiny.");
                            pl.SendGump(new DialogueGump(pl, worryModule));
                        });
                    riskModule.AddOption("Back to your other secrets.", 
                        pl => true,
                        pl => player.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, riskModule));
                });

            // Option 5: Offering Assistance
            greeting.AddOption("Maybe I can lend a hand in your operations. How can I help?", 
                player => true,
                player =>
                {
                    DialogueModule questModule = new DialogueModule("Now you're speaking my language. The shadows always welcome an extra pair of skillful hands. I have a task: a simple yet vital delivery of an enchanted package bound for Willow’s safehouse in West Montor. The journey won't be easy — expect narrow escapes, secret detours, and constant vigilance. Succeed, and you'll earn not only my gratitude but a boon that could tip the scales in your favor.");
                    
                    questModule.AddOption("Tell me more about this delivery.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule planModule = new DialogueModule("The package is unremarkable in appearance — a modest wooden box that disguises its potent secrets. Its enchantments ward off any prying magic. Stick to the shadowed back roads of West Montor, and avoid the crowded main streets where rival eyes await. Are you prepared for such a challenge?");
                            
                            planModule.AddOption("I'm ready. Consider it done.", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule confirmationModule = new DialogueModule("Wise choice. Remember, in this world, silence and precision are the keys to survival. Deliver the package safely and our paths will cross again under brighter (albeit secret) stars.");
                                    plc.SendGump(new DialogueGump(plc, confirmationModule));
                                });
                            planModule.AddOption("I need more details before I commit.", 
                                plc => true,
                                plc =>
                                {
                                    DialogueModule detailsModule = new DialogueModule("Very well. The box is small — no larger than a loaf of bread — yet its contents could unravel power balances if mishandled. Do not tamper with its seal. Rival factions lurk in every shadow, so trust only your instincts. Make your way swiftly and discreetly.");
                                    plc.SendGump(new DialogueGump(plc, detailsModule));
                                });
                            
                            pl.SendGump(new DialogueGump(pl, planModule));
                        });
                    questModule.AddOption("I don't think I can handle that kind of risk.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule retreatModule = new DialogueModule("In this life, hesitation is as dangerous as bold moves. But I understand — sometimes the shadows are too deep. Just know that opportunities in our realm tend to resurface when you least expect them.");
                            pl.SendGump(new DialogueGump(pl, retreatModule));
                        });
                    questModule.AddOption("Let's talk about something else.", 
                        pl => true,
                        pl => player.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    
                    player.SendGump(new DialogueGump(player, questModule));
                });
            
            // NEW Option 6: Delving into His Secret Past & Ambition
            greeting.AddOption("You seem to hide more than just smuggled secrets. What's your true background?", 
                player => true,
                player =>
                {
                    DialogueModule secretModule = CreateSecretPastModule();
                    player.SendGump(new DialogueGump(player, secretModule));
                });

            return greeting;
        }

        private DialogueModule CreateSecretPastModule()
        {
            // This module exposes Thistle's hidden noble origins, his ruthless ambition, and his secret plot to reclaim a rightful throne.
            DialogueModule secretPast = new DialogueModule("So, you want to know the truth behind the shadow? Very few dare to pry. I was not born in the gutters, but into a lineage of nobility — bloodlines that once commanded respect and fear. My birthright was more than a title; it was a destiny. Even now, beneath this guise of a smuggler, the spark of royalty burns fiercely. I harbor ambitions not just to trade secrets, but to seize power and reclaim the throne I believe is rightfully mine. Ask what you will, but tread carefully — pride and ruthlessness are my closest allies.");
            
            // Nested Option 6.1: Inquiry into his Noble Birth
            secretPast.AddOption("Are you truly of noble blood?", 
                pl => true,
                pl =>
                {
                    DialogueModule nobleModule = new DialogueModule("Indeed, my lineage is ancient and illustrious. I was raised among the fineries of courtly life, where every whisper carried the weight of tradition and every glance was a measure of worth. My ancestors ruled with both wisdom and an iron resolve. Even as I ventured into the clandestine arts, those royal roots pulsed through my veins.");
                    
                    nobleModule.AddOption("Tell me about your ancestors.", 
                        p => true,
                        p =>
                        {
                            DialogueModule ancestorsModule = new DialogueModule("My forebears were masters of strategy and power. They forged alliances, waged silent wars, and ruled vast lands. Their legacy taught me that true power is not given — it is seized, piece by piece, through calculated ruthlessness.");
                            p.SendGump(new DialogueGump(p, ancestorsModule));
                        });
                    nobleModule.AddOption("I find that hard to believe.", 
                        p => true,
                        p =>
                        {
                            DialogueModule disbeliefModule = new DialogueModule("Skepticism is a luxury for those who do not know the taste of true heritage. My every step, every covert maneuver, is driven by a burning desire to reclaim what was once mine. I do not ask for belief — I demand acknowledgment of my destiny.");
                            p.SendGump(new DialogueGump(p, disbeliefModule));
                        });
                    nobleModule.AddOption("Back to your past.", 
                        p => true,
                        p => p.SendGump(new DialogueGump(p, secretPast)));
                    
                    pl.SendGump(new DialogueGump(pl, nobleModule));
                });

            // Nested Option 6.2: On Claiming the Throne
            secretPast.AddOption("What do you mean by reclaiming the throne?", 
                pl => true,
                pl =>
                {
                    DialogueModule throneModule = new DialogueModule("I speak of a destiny long suppressed. The current liege sits on a throne that, by the ancient rights of blood and honor, should have been mine. I have spent years in the shadows, gathering intelligence, forming alliances, and waiting for the moment when a single misstep by my rival would pave the way for my ascent.");
                    
                    throneModule.AddOption("How do you plan to seize it?", 
                        p => true,
                        p =>
                        {
                            DialogueModule coupModule = new DialogueModule("With precision and ruthlessness. I maneuver through the intricate web of court intrigues and clandestine operations. Each secret message, each whispered plot, is a step toward destabilizing the current order. When the time is right, a swift and decisive strike shall shatter their complacency.");
                            p.SendGump(new DialogueGump(p, coupModule));
                        });
                    throneModule.AddOption("Are you not afraid of the consequences?", 
                        p => true,
                        p =>
                        {
                            DialogueModule consequenceModule = new DialogueModule("Fear is for the weak. My ambition burns so fiercely that I have no time for hesitation. Every risk calculated, every betrayal met with decisive retribution — these are the tools of my trade. I embrace the chaos, for it is in the storm that my destiny will be forged.");
                            p.SendGump(new DialogueGump(p, consequenceModule));
                        });
                    throneModule.AddOption("Back to your origins.", 
                        p => true,
                        p => p.SendGump(new DialogueGump(p, secretPast)));
                    
                    pl.SendGump(new DialogueGump(pl, throneModule));
                });

            // Nested Option 6.3: On His Ambition and Ruthlessness
            secretPast.AddOption("Your ambition is immense. Are you truly ruthless?", 
                pl => true,
                pl =>
                {
                    DialogueModule ruthlessModule = new DialogueModule("Ruthlessness is not cruelty for its own sake — it is the necessary edge of ambition. In the game of power, mercy is a luxury that can cost you your birthright. I have never hesitated to make the hard choices. Betrayal is met with swift justice, and loyalty is enforced by fear and respect alike.");
                    
                    ruthlessModule.AddOption("Can you give me an example of your methods?", 
                        p => true,
                        p =>
                        {
                            DialogueModule methodModule = new DialogueModule("There was a time when a trusted advisor turned traitor. Instead of a long, drawn-out purge, I orchestrated a silent strike that left no witnesses and no room for dissent. Every setback only fuels my resolve — a lesson I impart to those who would dare question my path.");
                            p.SendGump(new DialogueGump(p, methodModule));
                        });
                    ruthlessModule.AddOption("Is there any limit to your ambition?", 
                        p => true,
                        p =>
                        {
                            DialogueModule limitModule = new DialogueModule("Limits are set by the current order, and I intend to shatter them all. My ambition is boundless, driven by the certainty that the throne is my rightful inheritance. None shall stand in my way.");
                            p.SendGump(new DialogueGump(p, limitModule));
                        });
                    ruthlessModule.AddOption("Back to your secret past.", 
                        p => true,
                        p => p.SendGump(new DialogueGump(p, secretPast)));
                    
                    pl.SendGump(new DialogueGump(pl, ruthlessModule));
                });

            // Nested Option 6.4: On What Holds Him Back (or Not)
            secretPast.AddOption("What prevents you from seizing the throne now?", 
                pl => true,
                pl =>
                {
                    DialogueModule hesitationModule = new DialogueModule("Every move must be perfectly timed. The liege's power is bolstered by loyal guardians and the current order's momentum. A premature strike would only lead to my ruin. But mark my words, the day will come — and when it does, the world will tremble before the rightful ruler.");
                    
                    hesitationModule.AddOption("How do you plan to wait for the right moment?", 
                        p => true,
                        p =>
                        {
                            DialogueModule timingModule = new DialogueModule("I watch, I wait, and I sow discontent from within. Subtle manipulations, strategic eliminations, and secret alliances pave the long road to my ascension. Every falling piece of their empire is a silent prelude to my takeover.");
                            p.SendGump(new DialogueGump(p, timingModule));
                        });
                    hesitationModule.AddOption("Back to your ambitions.", 
                        p => true,
                        p => p.SendGump(new DialogueGump(p, secretPast)));
                    
                    pl.SendGump(new DialogueGump(pl, hesitationModule));
                });

            // Nested Option 6.5: On His Methods and the Price of Power
            secretPast.AddOption("Tell me more about your methods and the cost of ambition.", 
                pl => true,
                pl =>
                {
                    DialogueModule methodsModule = new DialogueModule("My methods are as varied as they are secret. I employ spies, manipulate rival factions, and sometimes resort to measures that would chill the heart of a lesser man. Every alliance is temporary, every friendship a means to an end. The cost of power is steep, but it is a price I gladly pay.");
                    
                    methodsModule.AddOption("What of the allies you trust?", 
                        p => true,
                        p =>
                        {
                            DialogueModule alliesModule = new DialogueModule("I trust no one with blind loyalty. My allies are bound by mutual interest and the inevitability of change. They know that in our world, trust is a currency traded only in the shadows — and it must be guarded with ruthless resolve.");
                            p.SendGump(new DialogueGump(p, alliesModule));
                        });
                    methodsModule.AddOption("And what if you fail?", 
                        p => true,
                        p =>
                        {
                            DialogueModule failureModule = new DialogueModule("Failure is not an option when destiny calls. Every setback is merely a lesson, every defeat a stepping stone. I have built my life upon the unyielding belief in my own right — and I will not be deterred by the cynics or the cowardly.");
                            p.SendGump(new DialogueGump(p, failureModule));
                        });
                    methodsModule.AddOption("Back to your secret past.", 
                        p => true,
                        p => p.SendGump(new DialogueGump(p, secretPast)));
                    
                    pl.SendGump(new DialogueGump(pl, methodsModule));
                });

            // Final Option in this branch: Return to the General Dialogue
            secretPast.AddOption("I have heard enough... Let us return to our previous conversation.", 
                pl => true,
                pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
            
            return secretPast;
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
