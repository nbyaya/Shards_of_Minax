using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    public class Jonas : BaseCreature
    {
        [Constructable]
        public Jonas() : base(AIType.AI_Mage, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jonas, the Budding Mage";
            Body = 0x190; // Human male body

            // Stats
            SetStr(90);
            SetDex(80);
            SetInt(120);
            SetHits(100);

            // Appearance: a mage in training with a well-worn robe and arcane trinkets
            AddItem(new Robe() { Hue = 1152 });
            AddItem(new Sandals() { Hue = 1152 });
            // A simple wand as his arcane focus
            AddItem(new FireballWand() { Name = "Wand of Novice Mysteries" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
        }

        public Jonas(Serial serial) : base(serial) { }

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
                "Greetings, fellow seeker of arcane wisdom! I am Jonas, a budding mage with a complex past. " +
                "I learn much from my secret correspondence with Cassian of Fawn, the prophetic guidance of Mother Edda in Yew, " +
                "and collaborative research with Mino of East Montor. Yet beneath my pursuit of magic lies a history " +
                "shrouded in regret and cautious rebellion—a history where I served as a reluctant advisor to tyrannical rulers " +
                "while secretly aiding resistance groups to soothe my conscience. What mysteries shall we unravel together today?"
            );

            // Branch 1: Secret Correspondence with Cassian (Fawn)
            greeting.AddOption("Tell me about your secret correspondence with Cassian.", 
                player => true, 
                player =>
                {
                    DialogueModule cassianModule = new DialogueModule(
                        "Ah, Cassian—his art and cryptic messages are like fragments of forgotten incantations. " +
                        "His sketches resonate with hidden forces and whisper secrets of the ley lines that interlace our realm. " +
                        "Would you care to hear how his visions guide my research, or the nature of his enigmatic riddles?"
                    );
                    cassianModule.AddOption("How do his visions guide you?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule visionsModule = new DialogueModule(
                                "Each missive from Cassian arrives like a shard of mystical moonlight. " +
                                "I recall one such vision: a night when the moon fractured into silver shards, " +
                                "revealing ancient glyphs on the living bark of enchanted trees. That revelation spurred me " +
                                "to map the concealed ley lines beneath our very feet."
                            );
                            visionsModule.AddOption("Fascinating! Tell me more about this discovery.", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule discoveryModule = new DialogueModule(
                                        "The glyphs were like cryptic keys, unlocking a dialogue between nature and the arcane. " +
                                        "Now, I pore over ancient tomes and listen closely to the whispers of the winds, hoping to decipher " +
                                        "further secrets embedded in the natural world."
                                    );
                                    plc.SendGump(new DialogueGump(plc, discoveryModule));
                                });
                            visionsModule.AddOption("I understand. Let's move on.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, visionsModule));
                        });
                    cassianModule.AddOption("Tell me more about his enigmatic messages.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule messageModule = new DialogueModule(
                                "Cassian's messages are like delicate riddles—each phrase laced with arcane significance. " +
                                "I spend long hours comparing his imagery to forgotten lore, finding that every carefully chosen word " +
                                "mirrors the mysteries of celestial alignments and elemental forces."
                            );
                            messageModule.AddOption("How do you decipher these riddles?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule decodeModule = new DialogueModule(
                                        "I treat his missives as parts of an ancient incantation. By cross-referencing his imagery with " +
                                        "venerable spellbooks and my own experiments, the language of magic gradually unfolds before me. " +
                                        "It is both art and science—a delicate balance of intuition and discipline."
                                    );
                                    plc.SendGump(new DialogueGump(plc, decodeModule));
                                });
                            messageModule.AddOption("Let's discuss another matter.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, messageModule));
                        });
                    player.SendGump(new DialogueGump(player, cassianModule));
                }
            );

            // Branch 2: Prophetic Guidance from Mother Edda (Yew)
            greeting.AddOption("What guidance have you received from Mother Edda?", 
                player => true, 
                player =>
                {
                    DialogueModule eddaModule = new DialogueModule(
                        "Mother Edda, the wise seer of Yew, has been a steadfast guide on my mystic journey. " +
                        "Her visions—uncertain as the wind rustling through ancient groves—warn of both imminent peril and hidden promise. " +
                        "Do you wish to hear of her latest foresight, or learn more about her storied past that shapes her wisdom?"
                    );
                    eddaModule.AddOption("Share with me her latest vision.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule recentVision = new DialogueModule(
                                "In her most recent vision, she spoke of a time when the boundaries between our realm and the spirit world " +
                                "blur—a moment when the rustle of leaves carried omens of transformation. She saw the ancient oaks of Yew " +
                                "awaken, their bark inscribed with runes foretelling a surge of elemental magic that could alter the balance forever."
                            );
                            recentVision.AddOption("What do those runes portend?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule runesModule = new DialogueModule(
                                        "The runes speak of a primal power—one that could either heal the scars of our land or unleash chaos upon it. " +
                                        "Her warning is both a call for reverence and a hint to prepare for a transformative reckoning."
                                    );
                                    plc.SendGump(new DialogueGump(plc, runesModule));
                                });
                            recentVision.AddOption("Intriguing. I must ponder this further.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, recentVision));
                        });
                    eddaModule.AddOption("Tell me more about her storied past.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule storiedPast = new DialogueModule(
                                "Mother Edda’s life spans eras, her memories steeped in the lost tongues of druids and the echoes of ruined temples. " +
                                "She is a silent witness to the ebb and flow of magic across the ages—a repository of lore that few dare to question."
                            );
                            storiedPast.AddOption("What lessons has she imparted to you?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule lessonsModule = new DialogueModule(
                                        "Her greatest lesson is that magic demands respect. Every incantation comes with a price, " +
                                        "and every spark of arcane power must be nurtured with humility. It is a discipline that forces one to " +
                                        "weigh duty against desire, often at great personal cost."
                                    );
                                    plc.SendGump(new DialogueGump(plc, lessonsModule));
                                });
                            storiedPast.AddOption("Thank you. Let's continue our discourse later.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, storiedPast));
                        });
                    player.SendGump(new DialogueGump(player, eddaModule));
                }
            );

            // Branch 3: Collaborative Research with Mino (East Montor)
            greeting.AddOption("I hear you collaborate with Mino on magical research. Can you elaborate?", 
                player => true, 
                player =>
                {
                    DialogueModule minoModule = new DialogueModule(
                        "Indeed, Mino from East Montor is a kindred spirit in the realm of arcane scholarship. " +
                        "Together, we pore over lost manuscripts and experiment with rare alchemical ingredients in our quest " +
                        "to harness raw magical energies. Would you like to hear about our recent experiments or the philosophy underpinning our work?"
                    );
                    minoModule.AddOption("Describe your recent experiments.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule experimentsModule = new DialogueModule(
                                "Our latest experiments have involved the fusion of elemental crystals with enchanted flora. " +
                                "The resulting energy pulses shimmer like the aurora, teetering on the brink of creation and chaos. " +
                                "Though fraught with peril, these tests may unlock new methods to channel and control raw magic."
                            );
                            experimentsModule.AddOption("What insights did you gain?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule insightsModule = new DialogueModule(
                                        "We observed that when the crystals resonate in unison with the vitality of magical plants, " +
                                        "a unique energy signature emerges—one that may bolster defensive wards and augment offensive spells. " +
                                        "It is an exciting breakthrough in our research."
                                    );
                                    plc.SendGump(new DialogueGump(plc, insightsModule));
                                });
                            experimentsModule.AddOption("How do you mitigate the risks?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule riskModule = new DialogueModule(
                                        "Risk is ever-present in the pursuit of forbidden knowledge. Mino and I rely on enchanted wards and " +
                                        "protective runes, all woven together with our most cautious incantations. Our shared vigilance is our greatest safeguard."
                                    );
                                    plc.SendGump(new DialogueGump(plc, riskModule));
                                });
                            pl.SendGump(new DialogueGump(pl, experimentsModule));
                        });
                    minoModule.AddOption("What is the philosophy behind your research?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule researchPhilosophy = new DialogueModule(
                                "We perceive magic as a language—a dialogue with the cosmos itself. Every experiment " +
                                "is an inquiry into nature’s secrets, blending scientific precision with the poetry of the unknown. " +
                                "Our work is a testament to both rigorous discipline and the creative passion that fuels discovery."
                            );
                            researchPhilosophy.AddOption("How does this philosophy shape your work?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule shapingModule = new DialogueModule(
                                        "It compels us to proceed with humility and care. Each success and failure instructs us, " +
                                        "reminding us that power is best wielded when tempered by wisdom and respect for the forces we command."
                                    );
                                    plc.SendGump(new DialogueGump(plc, shapingModule));
                                });
                            researchPhilosophy.AddOption("I appreciate that perspective.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, researchPhilosophy));
                        });
                    player.SendGump(new DialogueGump(player, minoModule));
                }
            );

            // Branch 4: Personal Studies in Magic
            greeting.AddOption("I am curious about your personal studies. How do you pursue your magical mastery?", 
                player => true, 
                player =>
                {
                    DialogueModule studiesModule = new DialogueModule(
                        "My path is an endless voyage into the arcane—a blend of intense study, " +
                        "keen observation of the natural and mystical world, and relentless experimentation. " +
                        "I immerse myself in ancient tomes, study celestial patterns, and carefully craft incantations " +
                        "that push the boundaries of known magic. What aspect would you like to explore further?"
                    );
                    studiesModule.AddOption("Tell me about your spellcasting experiments.", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule spellcastingModule = new DialogueModule(
                                "Spellcasting is an art of balance and precision. I attempt daring incantations; some " +
                                "unfold flawlessly while others remind me of magic’s volatile nature. Each experiment " +
                                "teaches me valuable lessons in focus, intent, and the eternal interplay between power and restraint."
                            );
                            spellcastingModule.AddOption("Have you learned anything from failed spells?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule failuresModule = new DialogueModule(
                                        "Every misfired incantation carries its own lesson—a lesson in humility and caution. " +
                                        "The failures sharpen my resolve to master the delicate dance of magic, ensuring that each spell is " +
                                        "crafted with the utmost care."
                                    );
                                    plc.SendGump(new DialogueGump(plc, failuresModule));
                                });
                            spellcastingModule.AddOption("What successes have affirmed your path?", 
                                plc => true, 
                                plc =>
                                {
                                    DialogueModule successesModule = new DialogueModule(
                                        "There are moments when the magic sings perfectly, when the air itself vibrates with power. " +
                                        "These successes, though fleeting, are the beacons that guide me along a path fraught with uncertainty."
                                    );
                                    plc.SendGump(new DialogueGump(plc, successesModule));
                                });
                            pl.SendGump(new DialogueGump(pl, spellcastingModule));
                        });
                    studiesModule.AddOption("What fuels your passion for magic?", 
                        pl => true, 
                        pl =>
                        {
                            DialogueModule passionModule = new DialogueModule(
                                "It is the allure of the unknown, the promise of discovery, and the hope of redemption " +
                                "that fuel my pursuit of magic. Every breakthrough, every carefully cast spell, is a step " +
                                "toward transforming the dark legacy of my past into a brighter future."
                            );
                            passionModule.AddOption("That is truly inspiring.", 
                                plc => true, 
                                plc => plc.SendGump(new DialogueGump(plc, CreateGreetingModule())));
                            pl.SendGump(new DialogueGump(pl, passionModule));
                        });
                    studiesModule.AddOption("I have no further questions for now.", 
                        pl => true, 
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, studiesModule));
                }
            );

            // Branch 5: Jonas's Secret Past as a Reluctant Advisor
            greeting.AddOption("I have heard whispers of a shadowed past in your heart. Tell me of your secret history.", 
                player => true, 
                player =>
                {
                    DialogueModule secretModule = new DialogueModule(
                        "My... my past is a labyrinth of painful memories and secret burdens. " +
                        "Before I fully embraced the call of magic, I served as an advisor to rulers whose tyranny " +
                        "left deep scars upon our land. Reluctantly, I lent my intelligence to their schemes—each word " +
                        "crafted with trembling caution—even as I nurtured a covert hope to aid those oppressed."
                    );
                    
                    secretModule.AddOption("How did you become an advisor to such tyrants?", 
                        player2 => true, 
                        player2 =>
                        {
                            DialogueModule advisorModule = new DialogueModule(
                                "It began when I was but a youth, lured by the promise of knowledge. " +
                                "I was recruited for my keen mind and sharp insight, thrust into the inner circles of power. " +
                                "Though my hands would shake at the weight of every decision, I found myself forced to offer counsel " +
                                "while my heart ached with disquiet."
                            );
                            advisorModule.AddOption("What was it like serving them?", 
                                player3 => true, 
                                player3 =>
                                {
                                    DialogueModule servingModule = new DialogueModule(
                                        "Serving those tyrants was a torment draped in false honor. " +
                                        "Every word I uttered felt like a betrayal to my very soul. In secret, I crafted subtle plans " +
                                        "to ease their cruelty, planting whispers of resistance amidst orders of oppression—a dangerous " +
                                        "dance of deceit and hope."
                                    );
                                    servingModule.AddOption("Tell me about the resistance you aided secretly.", 
                                        player4 => true, 
                                        player4 =>
                                        {
                                            DialogueModule resistanceModule = new DialogueModule(
                                                "In the dead of night, when the weight of my actions became unbearable, " +
                                                "I reached out to those daring enough to defy tyranny. I sent coded messages and invaluable advice " +
                                                "to underground resistance groups. Every clandestine act, though fraught with peril, was my way " +
                                                "of redeeming the choices I was forced to make."
                                            );
                                            resistanceModule.AddOption("Were you conflicted about your dual role?", 
                                                player5 => true, 
                                                player5 =>
                                                {
                                                    DialogueModule conflictModule = new DialogueModule(
                                                        "Oh, endlessly so. Regret and fear warred within me. " +
                                                        "Every moment was a battle between duty and conscience—a trembling uncertainty that haunted my nights. " +
                                                        "I learned to cloak my true feelings behind a mask of cautious wisdom, even as I reeled from the cost."
                                                    );
                                                    conflictModule.AddOption("How did you find the strength to continue?", 
                                                        player6 => true, 
                                                        player6 =>
                                                        {
                                                            DialogueModule strengthModule = new DialogueModule(
                                                                "Strength came from the belief that even the smallest act of defiance " +
                                                                "could kindle a beacon of hope. I studied ancient lore and practiced my spells with a fervor " +
                                                                "driven by the desire to one day overturn the darkness that had shadowed my path."
                                                            );
                                                            strengthModule.AddOption("Your resolve is admirable.", 
                                                                player7 => true, 
                                                                player7 =>
                                                                {
                                                                    strengthModule.AddOption("Let us return to our discussion of magic and mysteries.", 
                                                                        player8 => true, 
                                                                        player8 => player8.SendGump(new DialogueGump(player8, CreateGreetingModule()))
                                                                    );
                                                                    player7.SendGump(new DialogueGump(player7, strengthModule));
                                                                });
                                                            player6.SendGump(new DialogueGump(player6, strengthModule));
                                                        });
                                                    conflictModule.AddOption("I sense the pain in your words, Jonas.", 
                                                        player6 => true, 
                                                        player6 =>
                                                        {
                                                            DialogueModule empathyModule = new DialogueModule(
                                                                "Yes, the scars of my past are indelible and serve as constant reminders " +
                                                                "of the cost of compliance. Yet, each painful memory fuels my resolve to act with care " +
                                                                "and to embrace the path of subtle rebellion."
                                                            );
                                                            empathyModule.AddOption("Thank you for sharing your truth.", 
                                                                player7 => true, 
                                                                player7 => player7.SendGump(new DialogueGump(player7, CreateGreetingModule()))
                                                            );
                                                            player6.SendGump(new DialogueGump(player6, empathyModule));
                                                        });
                                                    player5.SendGump(new DialogueGump(player5, conflictModule));
                                                });
                                            resistanceModule.AddOption("Have your past deeds made it hard to trust others?", 
                                                player5 => true, 
                                                player5 =>
                                                {
                                                    DialogueModule trustModule = new DialogueModule(
                                                        "Trust is a fragile currency in a life scarred by duplicity. " +
                                                        "My experiences taught me to be ever vigilant—my intelligence now shadows my every word, " +
                                                        "and my caution ensures that I reveal only what must be revealed."
                                                    );
                                                    trustModule.AddOption("I understand. Caution is wise.", 
                                                        player6 => true, 
                                                        player6 => player6.SendGump(new DialogueGump(player6, CreateGreetingModule()))
                                                    );
                                                    player5.SendGump(new DialogueGump(player5, trustModule));
                                                });
                                            player4.SendGump(new DialogueGump(player4, resistanceModule));
                                        });
                                    servingModule.AddOption("Did your service leave a lasting mark on you?", 
                                        player4 => true, 
                                        player4 =>
                                        {
                                            DialogueModule lastingMarkModule = new DialogueModule(
                                                "Every decision, every half-spoken word—each counseled decree—etched itself into my soul. " +
                                                "I emerged wiser, albeit burdened with a perpetual caution that guides my every step today."
                                            );
                                            lastingMarkModule.AddOption("Your journey inspires hope for redemption.", 
                                                player5 => true, 
                                                player5 => player5.SendGump(new DialogueGump(player5, CreateGreetingModule()))
                                            );
                                            player4.SendGump(new DialogueGump(player4, lastingMarkModule));
                                        });
                                    player3.SendGump(new DialogueGump(player3, servingModule));
                                });
                            advisorModule.AddOption("Were there moments of regret that haunted you?", 
                                player3 => true, 
                                player3 =>
                                {
                                    DialogueModule regretModule = new DialogueModule(
                                        "Regret was my constant companion. Each command I gave—each word of counsel—was heavy with the knowledge " +
                                        "that I might be steering events toward further tragedy. I learned to mask my remorse behind calculated caution."
                                    );
                                    regretModule.AddOption("Did those regrets push you toward aiding the resistance?", 
                                        player4 => true, 
                                        player4 =>
                                        {
                                            DialogueModule pushModule = new DialogueModule(
                                                "Indeed, in the quiet hours of desperation, I would secretly liaise with dissenters, " +
                                                "offering advice that undermined the tyrants’ cruel designs. My intelligence became my weapon—both " +
                                                "in serving those in power and in sowing the seeds of rebellion."
                                            );
                                            pushModule.AddOption("Remarkable. You wield your knowledge like a double-edged sword.", 
                                                player5 => true, 
                                                player5 => player5.SendGump(new DialogueGump(player5, CreateGreetingModule()))
                                            );
                                            player4.SendGump(new DialogueGump(player4, pushModule));
                                        });
                                    regretModule.AddOption("I cannot fathom the burden you carried.", 
                                        player4 => true, 
                                        player4 => player4.SendGump(new DialogueGump(player4, CreateGreetingModule()))
                                    );
                                    player3.SendGump(new DialogueGump(player3, regretModule));
                                });
                            player2.SendGump(new DialogueGump(player2, advisorModule));
                        });
                    secretModule.AddOption("How do you reconcile that dark past with your pursuit of magic?", 
                        player2 => true, 
                        player2 =>
                        {
                            DialogueModule reconcileModule = new DialogueModule(
                                "Reconciliation is a slow, painful process—a blend of atonement and relentless study. " +
                                "I now pursue magic as a healing art, each spell a deliberate step toward mending the past " +
                                "and illuminating a future free from tyranny. My cautious nature ensures that I never repeat those mistakes."
                            );
                            reconcileModule.AddOption("That transformation is truly inspiring.", 
                                player3 => true, 
                                player3 => player3.SendGump(new DialogueGump(player3, CreateGreetingModule()))
                            );
                            reconcileModule.AddOption("Do you fear your past might still shadow your future?", 
                                player3 => true, 
                                player3 =>
                                {
                                    DialogueModule fearModule = new DialogueModule(
                                        "There is always a lingering dread—a cautious whisper that my past may stain all that I strive for. " +
                                        "Yet, I channel that fear into rigorous study and careful incantation, determined to forge a destiny " +
                                        "that upholds hope over despair."
                                    );
                                    fearModule.AddOption("Your vigilance is commendable.", 
                                        player4 => true, 
                                        player4 => player4.SendGump(new DialogueGump(player4, CreateGreetingModule()))
                                    );
                                    player3.SendGump(new DialogueGump(player3, fearModule));
                                });
                            player2.SendGump(new DialogueGump(player2, reconcileModule));
                        });
                    secretModule.AddOption("I appreciate your honesty, Jonas. Let us return to other mysteries of magic.", 
                        player2 => true, 
                        player2 => player2.SendGump(new DialogueGump(player2, CreateGreetingModule()))
                    );
                    player.SendGump(new DialogueGump(player, secretModule));
                }
            );

            // Branch 6: End Dialogue
            greeting.AddOption("Farewell for now.", 
                player => true, 
                player =>
                {
                    player.SendMessage("Jonas nods thoughtfully as you take your leave, his eyes dancing with both arcane fire and hidden sorrow.");
                }
            );

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
