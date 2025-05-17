using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Kess, the Dairy Archivist")]
    public class Kess : BaseCreature
    {
        [Constructable]
		public Kess() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kess";
            Body = 0x191; // Human body

            // Stats
            SetStr(100);
            SetDex(80);
            SetInt(90);
            SetHits(100);

            // Appearance
            AddItem(new Robe() { Hue = 1450 });
            AddItem(new Cap() { Hue = 2750 });
            AddItem(new Sandals() { Hue = 850 });

            // Random skin and hair style
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Kess(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler! I am Kess—a humble dairy trader by trade, keeper of secret lore, and, if you must know, a former apprentice of the arcane arts. Whether you wish to discuss the creamy wonders of my dairy products, listen to the eerie tales of the deep, inquire about mysterious shipments, or even learn about a secret past hidden beneath my everyday guise, I have many stories to share. What intrigues you today?");

            greeting.AddOption("Tell me about your dairy trade.", 
                player => true,
                player =>
                {
                    DialogueModule dairyModule = CreateDairyTradeModule();
                    player.SendGump(new DialogueGump(player, dairyModule));
                });

            greeting.AddOption("I wish to hear about unsettling maritime mysteries.", 
                player => true,
                player =>
                {
                    DialogueModule mysteryModule = CreateMaritimeMysteryModule();
                    player.SendGump(new DialogueGump(player, mysteryModule));
                });

            greeting.AddOption("Any news on strange shipments or covert dealings?", 
                player => true,
                player =>
                {
                    DialogueModule shipmentModule = CreateShipmentNewsModule();
                    player.SendGump(new DialogueGump(player, shipmentModule));
                });

            greeting.AddOption("Share with me the general news of the region.", 
                player => true,
                player =>
                {
                    DialogueModule newsModule = new DialogueModule("The winds whisper many secrets these days. Marin of Renika praises our dairy delights; Navi recounts uncanny happenings on the high seas; and Lilo brings covert news of enigmatic shipments. Which story would you like to pursue further?");
                    
                    newsModule.AddOption("Tell me more about Marin's influence on your dairy trade.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule marinModule = CreateMarinModule();
                            pl.SendGump(new DialogueGump(pl, marinModule));
                        });
                    
                    newsModule.AddOption("What eerie tales has Navi shared with you?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule naviModule = CreateMaritimeMysteryModule();
                            pl.SendGump(new DialogueGump(pl, naviModule));
                        });
                    
                    newsModule.AddOption("I want to know about Lilo's strange shipments.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule liloModule = CreateShipmentNewsModule();
                            pl.SendGump(new DialogueGump(pl, liloModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, newsModule));
                });

            // New secret past branch detailing her hidden background as an apprentice mage.
            greeting.AddOption("I sense you have a secret story. Tell me about your past.", 
                player => true,
                player =>
                {
                    DialogueModule secretModule = CreateSecretPastModule();
                    player.SendGump(new DialogueGump(player, secretModule));
                });

            return greeting;
        }

        private DialogueModule CreateDairyTradeModule()
        {
            DialogueModule dairy = new DialogueModule("Ah, dairy trading is both an art and a passion—one that fills every sunrise with promise. I work hard to produce and refine dairy products that taste like home. My trade thrives thanks to a long-standing alliance with Marin from Renika, whose knowledge of coastal routes ensures my goods reach far and wide without losing their freshness.");
            
            dairy.AddOption("Who is Marin, and how did you begin working together?", 
                player => true,
                player =>
                {
                    DialogueModule marinInfo = new DialogueModule("Marin is far more than a mere logistics expert—he is a dear friend with a poetic soul. We first met on a foggy morning when a delayed shipment nearly left me stranded. His calm confidence and deep understanding of the sea sealed our fateful alliance. Our bond grew as we exchanged stories over crates of fine cheese.");
                    
                    marinInfo.AddOption("Tell me more about your early days in this trade.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule earlyDays = new DialogueModule("I remember the chill of early dawn, the echo of distant cowbells, and the constant race against time. Every delivery was a challenge, but with Marin's guidance, I learned that hardship could be transformed into opportunity. Those days taught me the value of diligence and tradition.");
                            pl.SendGump(new DialogueGump(pl, earlyDays));
                        });
                    
                    marinInfo.AddOption("What makes your partnership with Marin unique?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule marinBond = new DialogueModule("Marin has a way of blending pragmatism with a poetic appreciation for the sea. He often remarks that every shipment is like a living story connecting distant lands and souls. His insights have refined not only my products but also the spirit in which I approach my craft.");
                            pl.SendGump(new DialogueGump(pl, marinBond));
                        });
                    
                    player.SendGump(new DialogueGump(player, marinInfo));
                });

            dairy.AddOption("Can you detail some of your prized dairy products?", 
                player => true,
                player =>
                {
                    DialogueModule productModule = new DialogueModule("Certainly! I offer an array of treats—from rich, velvety butter churned at first light to artisan cheeses imbued with the essence of our land. My signature creation, 'Moonlit Brie', is especially dear: crafted under the glow of a full moon, it is smooth, tangy, and utterly unforgettable.");
                    
                    productModule.AddOption("Tell me more about 'Moonlit Brie'.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule brieModule = new DialogueModule("'Moonlit Brie' is a labor of love. I milk cows nurtured in dew-kissed meadows and carefully age the curds in cool, ancient cellars. The result is a cheese with a taste as mysterious and captivating as the night sky.");
                            pl.SendGump(new DialogueGump(pl, brieModule));
                        });
                    
                    productModule.AddOption("How do you keep your butter so fresh?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule butterModule = new DialogueModule("Each batch of butter is churned with precision and care. I source only the freshest milk, and a dash of secret tradition ensures it remains pure and delicious. It's a humble product, yet made with heart and history.");
                            pl.SendGump(new DialogueGump(pl, butterModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, productModule));
                });

            dairy.AddOption("Return to main conversation.", 
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
            return dairy;
        }

        private DialogueModule CreateMaritimeMysteryModule()
        {
            DialogueModule mystery = new DialogueModule("The sea is a realm of wonder and enigma—a vast tapestry of light, shadow, and ancient whispers. I have often spent quiet nights listening to eerie tales told by Navi of Renika. Her vivid recollections of ghostly ships, spectral crews, and otherworldly luminescence evoke both dread and fascination.");
            
            mystery.AddOption("What eerie tales has Navi shared with you?", 
                player => true,
                player =>
                {
                    DialogueModule naviTales = new DialogueModule("Navi speaks of mysterious fleets emerging from the mist, abandoned vessels glowing under moonlight, and supernatural happenings that blur the line between myth and reality. One tale tells of a night when luminous orbs danced above a wreck, hinting at a message from spirits long forgotten.");
                    
                    naviTales.AddOption("Could you recount one specific tale?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule specificTale = new DialogueModule("I recall a foggy night when Navi described a grand galleon rising from the depths—its tattered sails glowing with an ethereal light. The silent crew, shrouded in mystery, seemed driven by a purpose beyond mortal life. The very sight sent shivers down my spine.");
                            pl.SendGump(new DialogueGump(pl, specificTale));
                        });
                    
                    naviTales.AddOption("What do you think causes these strange events?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule causeModule = new DialogueModule("Navi believes the sea harbors echoes of ancient curses and unresolved conflicts—a place where the past and present intertwine. Perhaps these odd occurrences are the restless murmurs of souls long lost, or the manifestation of forgotten magic beneath the waves.");
                            pl.SendGump(new DialogueGump(pl, causeModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, naviTales));
                });

            mystery.AddOption("Have you ever experienced such mysteries yourself?", 
                player => true,
                player =>
                {
                    DialogueModule personalExperience = new DialogueModule("There are nights when the salt air and the distant roar of the tide evoke memories of uncanny omens. Although my feet remain firmly planted in the realm of dairy, I too have felt that inexplicable presence—a reminder of the vast mysteries that lie beyond our sight.");
                    player.SendGump(new DialogueGump(player, personalExperience));
                });

            mystery.AddOption("Return to main conversation.", 
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
            return mystery;
        }

        private DialogueModule CreateShipmentNewsModule()
        {
            DialogueModule shipment = new DialogueModule("The world of covert shipments is as secretive as it is enthralling. My confidant Lilo from Grey & Pirate Isle brings whispers of exotic cargo that defy explanation. Each delivery is shrouded in mystery—a fusion of mundane goods and inexplicable magic.");
            
            shipment.AddOption("What types of shipments does Lilo report?", 
                player => true,
                player =>
                {
                    DialogueModule shipmentDetails = new DialogueModule("Lilo’s reports range from crates filled with exotic spices to packages radiating an otherworldly energy. I once received word of rune-etched barrels arriving under a blood-red moon—a sight that made one ponder whether destiny itself was being delivered.");
                    
                    shipmentDetails.AddOption("Give me an example of such a shipment.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule exampleModule = new DialogueModule("There was a time when a shipment of sealed, rune-covered barrels arrived on a night when the tide whispered secrets. The barrels pulsed with eerie light, and Lilo insisted they were more than mere trade goods—portents of fate, perhaps.");
                            pl.SendGump(new DialogueGump(pl, exampleModule));
                        });
                    
                    shipmentDetails.AddOption("Why is Lilo drawn to these mysterious cargos?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule motiveModule = new DialogueModule("Lilo thrives in the shadows of secrecy. He believes that each enigmatic shipment is a fragment of a grand puzzle waiting to be solved—a puzzle that might one day unveil the hidden forces that govern our world.");
                            pl.SendGump(new DialogueGump(pl, motiveModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, shipmentDetails));
                });

            shipment.AddOption("How do these mysterious items influence your trade?", 
                player => true,
                player =>
                {
                    DialogueModule impactModule = new DialogueModule("Occasionally, these strange consignments bring rare ingredients or subtle enchantments that enhance my dairy products. It is as if the interplay between magic and the mundane breathes new life into my creations.");
                    player.SendGump(new DialogueGump(player, impactModule));
                });

            shipment.AddOption("Return to main conversation.", 
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
            return shipment;
        }

        private DialogueModule CreateMarinModule()
        {
            DialogueModule marin = new DialogueModule("Marin of Renika is a steadfast friend and a guiding force behind my thriving trade. A seafarer with a lyrical soul, his wisdom in charting the dangerous coastal routes and his poetic view on life have enriched my world immeasurably.");
            
            marin.AddOption("What lessons has Marin taught you about trade?", 
                player => true,
                player =>
                {
                    DialogueModule lessons = new DialogueModule("Marin once confided that trade is much like a dance—a careful balance of risk, trust, and intuition. Every successful journey is choreographed with nature’s rhythm, and his lessons have been invaluable to my craft.");
                    player.SendGump(new DialogueGump(player, lessons));
                });

            marin.AddOption("How has his guidance improved your dairy products?", 
                player => true,
                player =>
                {
                    DialogueModule improvement = new DialogueModule("Thanks to Marin, I've refined my techniques and discovered new methods to preserve and enhance the flavors of my products. His advice has transformed my humble dairy fare into small works of art that nourish body and spirit alike.");
                    player.SendGump(new DialogueGump(player, improvement));
                });

            marin.AddOption("Return to previous menu.", 
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
            return marin;
        }

        // New secret past branch: revealing Kess's hidden backstory as an enthusiastic, curious, and clumsy apprentice mage.
        private DialogueModule CreateSecretPastModule()
        {
            DialogueModule secret = new DialogueModule("Very well, I shall confide in you—a secret that I have guarded for many moons. Long ago, before I devoted myself entirely to the art of dairy, I was an apprentice mage. I was brimming with enthusiasm and boundless curiosity, though my clumsiness often led to magical mishaps that left trails of chaos in their wake. My youthful heart burned with the promise of great arcane power, yet fate had other plans.");

            secret.AddOption("What sort of magic did you study?", 
                player => true,
                player =>
                {
                    DialogueModule studyModule = new DialogueModule("I delved into the art of elemental conjuration—experimenting with fire, water, air, and earth. My ambition often drove me to attempt feats that exceeded my control. I even tried to harness lunar power to enchant dairy, though the results were more explosive than enchanting!");
                    
                    studyModule.AddOption("That sounds dangerous! How did it affect your trade?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule tradeImpactModule = new DialogueModule("The chaos of my early experiments taught me a vital lesson in caution and precision. After several mishaps that nearly cost me more than my pride, I learned to channel my enthusiasm into something stable. Thus, I found solace in dairy—a craft where magic is tempered by tradition and the flavor of home.");
                            pl.SendGump(new DialogueGump(pl, tradeImpactModule));
                        });
                    
                    studyModule.AddOption("What were your favorite spells back then?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule spellsModule = new DialogueModule("Oh, the spells of my youth! I once attempted a charm to summon enchanted cows for an endless milk supply. Instead, my clumsy incantation summoned a minor thunderstorm inside the barn—soaking everything in minutes! It was as much a spectacle as it was a disaster.");
                            pl.SendGump(new DialogueGump(pl, spellsModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, studyModule));
                });

            secret.AddOption("Tell me about one of your most memorable magical mishaps.", 
                player => true,
                player =>
                {
                    DialogueModule mishapModule = new DialogueModule("There is one mishap that still makes me blush to remember. During an over-ambitious experiment, I tried to infuse a batch of cheese with a preservation charm. My eager incantation faltered, and the cheese erupted into a frothy, bubbling explosion—a veritable dairy disaster!");
                    
                    mishapModule.AddOption("What went wrong exactly?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule wentWrongModule = new DialogueModule("In my haste, I mispronounced a crucial syllable. Instead of invoking the word for 'preservation', I muddled it into a chaotic utterance that summoned the force of wild magic. The result was an explosion of enchanted curds and spilled secrets—the kind of mishap that makes one rethink every word.");
                            pl.SendGump(new DialogueGump(pl, wentWrongModule));
                        });
                    
                    mishapModule.AddOption("How did you recover from that incident?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule recoveryModule = new DialogueModule("After the explosion, I spent weeks patching up the damage—both in my workshop and in my pride. I had to face the stern looks of my mentors and painstakingly master repair spells. That incident, though painful, taught me humility and the importance of a steady hand.");
                            pl.SendGump(new DialogueGump(pl, recoveryModule));
                        });
                    
                    mishapModule.AddOption("Did that experience change your view of magic?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule viewModule = new DialogueModule("Indeed it did. I learned that raw, unchecked enthusiasm without discipline could lead to calamity. It was a turning point—a moment that pushed me towards a quieter, more grounded craft, where I could still honor my magical passion without courting disaster.");
                            pl.SendGump(new DialogueGump(pl, viewModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, mishapModule));
                });

            secret.AddOption("How did you transition from being an apprentice mage to a dairy trader?", 
                player => true,
                player =>
                {
                    DialogueModule transitionModule = new DialogueModule("It was an unexpected twist of fate. After one particularly calamitous event—when a spell of mine turned the workshop into a miniature enchanted waterfall—I realized that my magical talents might be better channeled into a safer, steadier art. I set aside my wand and embraced the time-honored traditions of dairy, where passion is measured in flavor and care rather than combustible magic.");
                    
                    transitionModule.AddOption("Tell me more about that turning point.", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule turningPointModule = new DialogueModule("I vividly recall that day: a chaotic cascade of enchanted milk spilling everywhere, the smell of scorched butter in the air, and a moment of clarity amid the chaos. It was then that I decided to trade volatile spells for the art of crafting exquisite cheeses and butter—each a testament to hard-earned wisdom and a more controlled passion.");
                            pl.SendGump(new DialogueGump(pl, turningPointModule));
                        });
                    
                    transitionModule.AddOption("How did your former mentors react to your change?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule mentorModule = new DialogueModule("Some mentors were disappointed, others relieved. They recognized that perhaps my true calling lay not in the dangerous realm of magic, but in creating nourishment and comfort. Their mixed reactions only deepened my resolve to excel in a craft that brought warmth and stability to our community.");
                            pl.SendGump(new DialogueGump(pl, mentorModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, transitionModule));
                });

            secret.AddOption("Do you ever long to return to magic?", 
                player => true,
                player =>
                {
                    DialogueModule returnMagicModule = new DialogueModule("The allure of magic still beckons me. My enthusiasm for the arcane remains, even if I must now tread carefully. Occasionally, in the quiet moments of the night, I conduct small, discreet experiments—tiny sparks of what once was—to keep that part of me alive without inviting chaos.");
                    
                    returnMagicModule.AddOption("What kind of experiments do you conduct now?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule experimentsModule = new DialogueModule("I now dabble in minor enchantments—little charms to enhance the flavor of my cheeses or to improve the health of my dairy herd. They are modest, controlled wonders that allow me to indulge my curious nature while keeping the risk of disaster at bay.");
                            pl.SendGump(new DialogueGump(pl, experimentsModule));
                        });
                    
                    returnMagicModule.AddOption("Aren't you afraid of repeating past mistakes?", 
                        pl => true,
                        pl =>
                        {
                            DialogueModule fearModule = new DialogueModule("I carry a cautious heart tempered by experience. Yes, my clumsiness lingers, and my eagerness sometimes tempts fate. But every careful experiment is a reminder to balance curiosity with responsibility. I now measure every spark of magic with equal parts hope and caution.");
                            pl.SendGump(new DialogueGump(pl, fearModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, returnMagicModule));
                });

            secret.AddOption("Return to main conversation.", 
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            
            return secret;
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
