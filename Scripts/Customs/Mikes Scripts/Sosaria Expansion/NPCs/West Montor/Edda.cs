using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;
using Server.Network; // For NetState and RelayInfo

namespace Server.Mobiles
{
    [CorpseName("the remains of Edda, the battle-worn veteran")]
    public class Edda : BaseCreature
    {
        private DateTime lastStoryTime;

        [Constructable]
        public Edda() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Edda";
            Body = 0x190; // Human male body

            // Set Stats
            SetStr(120);
            SetDex(80);
            SetInt(90);
            SetHits(100);

            // Appearance and Equipment
            AddItem(new Doublet() { Hue = 1150 });
            AddItem(new Shoes() { Hue = 1150 });
            AddItem(new Helmet() { Hue = 1150 });
			AddItem(new LongPants() { Hue = 1432 });
            // Natural variation in skin and hair
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            lastStoryTime = DateTime.MinValue;
        }

        public Edda(Serial serial) : base(serial)
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
                "Greetings, traveler. I am Edda—a proud veteran, a fierce warrior, and an honorable soul " +
                "bound by duty. My life is a tapestry of glorious battle, bitter regrets, and a secret past: " +
                "I lead a dwindling warrior clan whose honor has been tarnished by time and treachery. " +
                "I fight each day to restore our family’s legacy through valor. What would you like to discuss?"
            );

            greeting.AddOption("Tell me about your war experiences.", 
                player => true, 
                player =>
                {
                    DialogueModule warModule = CreateWarStoriesModule();
                    player.SendGump(new DialogueGump(player, warModule));
                });

            greeting.AddOption("I sense a deep grudge against Pirate Isle. Explain your feud with Captain Waylon.", 
                player => true, 
                player =>
                {
                    DialogueModule pirateModule = CreatePirateGrudgeModule();
                    player.SendGump(new DialogueGump(player, pirateModule));
                });

            greeting.AddOption("I heard you mentor the youth—particularly Renn. What wisdom do you share?", 
                player => true, 
                player =>
                {
                    DialogueModule mentorModule = CreateMentorAdviceModule();
                    player.SendGump(new DialogueGump(player, mentorModule));
                });

            greeting.AddOption("Reveal your personal regrets and sorrows.", 
                player => true, 
                player =>
                {
                    DialogueModule regretsModule = CreatePersonalRegretsModule();
                    player.SendGump(new DialogueGump(player, regretsModule));
                });

            greeting.AddOption("Tell me about your warrior clan and its legacy.", 
                player => true, 
                player =>
                {
                    DialogueModule clanModule = CreateClanBackgroundModule();
                    player.SendGump(new DialogueGump(player, clanModule));
                });

            greeting.AddOption("Thank you for sharing your tales. Farewell.", 
                player => true, 
                player =>
                {
                    player.SendMessage("Farewell, traveler. May your path be lit by honor and valor.");
                });

            return greeting;
        }

        private DialogueModule CreateWarStoriesModule()
        {
            DialogueModule warStories = new DialogueModule(
                "The fields of battle are etched in my soul. I recall the Battle of the Broken Shore—a time " +
                "when strategy and sacrifice intermingled in a dance of death. Would you like to hear about " +
                "our cunning tactics, the unbreakable bonds of camaraderie, or the brutal lessons learned from loss?"
            );

            warStories.AddOption("Speak of the cunning tactics and daring maneuvers.", 
                player => true, 
                player =>
                {
                    DialogueModule tacticsModule = new DialogueModule(
                        "When darkness fell over the battlefield, our minds became our weapons. I led covert flanking " +
                        "movements, executing ambushes beneath the starry veil. In one fateful engagement, when our line " +
                        "wavered, I ordered a fierce counter-assault—a move that turned despair into glory. " +
                        "Do you wish to know the details of that decisive moment?"
                    );
                    tacticsModule.AddOption("Yes, describe that decisive moment in detail.",
                        p => true,
                        p =>
                        {
                            DialogueModule decisiveMoment = new DialogueModule(
                                "Under a moonless sky, with enemy forces closing in, I rallied my comrades with a battle cry " +
                                "that still echoes in my ears. We surged forward as one—each strike, each parry, a testament " +
                                "to our determination. That night, victory was carved out of valor and sacrifice."
                            );
                            decisiveMoment.AddOption("That is awe-inspiring. What did you learn from it?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule lessonModule = new DialogueModule(
                                        "I learned that honor in battle is earned through relentless courage and unwavering unity. " +
                                        "Every wound sustained, every loss endured, teaches us the price of freedom—and the cost of glory."
                                    );
                                    p2.SendGump(new DialogueGump(p2, lessonModule));
                                });
                            p.SendGump(new DialogueGump(p, decisiveMoment));
                        });
                    player.SendGump(new DialogueGump(player, tacticsModule));
                });

            warStories.AddOption("Tell me about the bonds forged in the heat of combat.", 
                player => true, 
                player =>
                {
                    DialogueModule bondsModule = new DialogueModule(
                        "In the midst of carnage, a brotherhood was born. With sweat and blood, we bound ourselves " +
                        "together in unspoken oaths—oaths that still guide us. I remember how our camaraderie was " +
                        "tested during long nights in enemy territory. Would you like a tale of kinship and sacrifice?"
                    );
                    bondsModule.AddOption("Yes, recount the story of your closest comrades.",
                        p => true,
                        p =>
                        {
                            DialogueModule comradeModule = new DialogueModule(
                                "I recall a night when we found ourselves surrounded—hopeless odds closing in. With no " +
                                "chance of retreat, we fought as one entity. Amidst the chaos, the face of a dear friend " +
                                "became my beacon, reminding me that the honor of our clan depends on our mutual strength. " +
                                "It is that spirit which I now pass on to young Renn."
                            );
                            comradeModule.AddOption("How did that experience shape your leadership?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule leadershipModule = new DialogueModule(
                                        "It forged my resolve to lead by example—never shying from the direst circumstances. " +
                                        "Every battle, every sacrifice, has steeled me for the responsibility of restoring our clan's honor."
                                    );
                                    p2.SendGump(new DialogueGump(p2, leadershipModule));
                                });
                            p.SendGump(new DialogueGump(p, comradeModule));
                        });
                    player.SendGump(new DialogueGump(player, bondsModule));
                });

            warStories.AddOption("Share the painful lessons learned through bitter loss.", 
                player => true, 
                player =>
                {
                    DialogueModule lossModule = new DialogueModule(
                        "Loss cuts deeper than any blade. Each fallen comrade taught me that glory comes with a heavy " +
                        "price. The bitterness of defeat and the grief of farewell have become both my burden and my guide. " +
                        "Would you like to know how these experiences have molded my spirit?"
                    );
                    lossModule.AddOption("Yes, tell me how loss transformed you.",
                        p => true,
                        p =>
                        {
                            DialogueModule transformedModule = new DialogueModule(
                                "Every tear shed for a fallen brother forged the iron of my resolve. I learned that true honor " +
                                "is reflected in the willingness to endure pain for the sake of those you protect. This sorrow " +
                                "fuels my fierce determination to see our clan restored to its rightful glory."
                            );
                            transformedModule.AddOption("What do you do with that sorrow every day?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule copeModule = new DialogueModule(
                                        "I channel it into training, in every swing of my blade and every lesson I pass to the youth. " +
                                        "I honor the memory of our lost with each new dawn—a constant reminder that our legacy must not be forgotten."
                                    );
                                    p2.SendGump(new DialogueGump(p2, copeModule));
                                });
                            p.SendGump(new DialogueGump(p, transformedModule));
                        });
                    player.SendGump(new DialogueGump(player, lossModule));
                });

            return warStories;
        }

        private DialogueModule CreatePirateGrudgeModule()
        {
            DialogueModule pirateGrudge = new DialogueModule(
                "The mere name of Pirate Isle kindles a fierce anger within me. Captain Waylon—once a man of promise— " +
                "turned his back on honor, embracing treachery and pillaging our sacred lands. His actions not only " +
                "ravaged our homelands but also brought ignominy upon our clan. Would you like to hear the details of his " +
                "reprehensible deeds, or learn of the personal toll his betrayal has inflicted on me?"
            );

            pirateGrudge.AddOption("Describe in detail the ruthless deeds of Captain Waylon and his band.", 
                player => true, 
                player =>
                {
                    DialogueModule detailModule = new DialogueModule(
                        "Captain Waylon was a stain upon the honor of warriors. His raids were savage—villages burned, " +
                        "ancient sanctuaries defiled, and our people's grief left raw and unhealed. I witnessed firsthand " +
                        "the devastation of his final assault, when even the skies seemed to weep for the fallen."
                    );
                    detailModule.AddOption("What happened during that final, devastating raid?",
                        p => true,
                        p =>
                        {
                            DialogueModule raidModule = new DialogueModule(
                                "It was a night of unholy chaos. The clamor of battle mixed with the cries of the innocent " +
                                "as Waylon's men ransacked our coastal defenses. I fought with every ounce of my being, " +
                                "but the enormity of the horror left a mark on my soul. That night, the legacy of our clan was " +
                                "stained with betrayal."
                            );
                            raidModule.AddOption("How do you still bear such a wound?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule woundModule = new DialogueModule(
                                        "I bear it as a scar—a constant reminder of what must never be allowed to happen again. " +
                                        "My every breath is a pledge to restore honor by ensuring that treachery like his never goes unpunished."
                                    );
                                    p2.SendGump(new DialogueGump(p2, woundModule));
                                });
                            p.SendGump(new DialogueGump(p, raidModule));
                        });
                    player.SendGump(new DialogueGump(player, detailModule));
                });

            pirateGrudge.AddOption("How have these treacheries affected you personally?",
                player => true,
                player =>
                {
                    DialogueModule personalImpact = new DialogueModule(
                        "The betrayal cut deeper than any wound inflicted by steel. It shattered my once unyielding faith " +
                        "in the righteousness of our cause. Every act of piracy felt like a dagger thrust into my honor—" +
                        "a call to arms that still burns within me."
                    );
                    personalImpact.AddOption("Do you seek redemption or revenge?",
                        p => true,
                        p =>
                        {
                            DialogueModule redemptionModule = new DialogueModule(
                                "I seek both. Redemption for the sins of the past and revenge upon those who would sully our legacy. " +
                                "My blood boils with the desire to see justice served—and to restore the honor of my people."
                            );
                            redemptionModule.AddOption("How do you plan to achieve that?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule planModule = new DialogueModule(
                                        "Through glorious battle, relentless training, and by passing on the sacred traditions of our clan " +
                                        "to a new generation of warriors. Each victory, each hard-fought victory, chips away at the disgrace " +
                                        "that has long marred our name."
                                    );
                                    p2.SendGump(new DialogueGump(p2, planModule));
                                });
                            p.SendGump(new DialogueGump(p, redemptionModule));
                        });
                    player.SendGump(new DialogueGump(player, personalImpact));
                });

            return pirateGrudge;
        }

        private DialogueModule CreateMentorAdviceModule()
        {
            DialogueModule mentorAdvice = new DialogueModule(
                "Guidance for the young is the honor bestowed upon me by fate. I have mentored many, including dear Renn, " +
                "instilling in them the values of valor, honor, and integrity. Would you like to hear my words on the nature " +
                "of true strength or a specific memory of mentoring a promising soul?"
            );

            mentorAdvice.AddOption("Share with me the greatest lesson you learned from battle.", 
                player => true, 
                player =>
                {
                    DialogueModule lessonModule = new DialogueModule(
                        "Battle has taught me that honor is forged in the crucible of sacrifice. A warrior's true worth " +
                        "is not measured by victories alone but by the moral fiber that endures even in the face of defeat. " +
                        "Do you wish to know how such lessons inform my every decision?"
                    );
                    lessonModule.AddOption("Indeed, how do you embody that honor?",
                        p => true,
                        p =>
                        {
                            DialogueModule embodyModule = new DialogueModule(
                                "I stand tall against adversity—proud and unyielding. Each scar is a badge of honor, each wound a reminder " +
                                "that the price of peace is paid in blood. That is why I strive to guide my kin and the youth, " +
                                "to never falter in the face of overwhelming odds."
                            );
                            embodyModule.AddOption("What advice do you give to those about to step into battle?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule adviceModule = new DialogueModule(
                                        "I tell them: 'Remember who you are—a descendant of warriors whose legacy is written in valor. " +
                                        "Embrace your fear, but let it fuel your determination. Stand fast, for in unity, there is unyielding strength.'"
                                    );
                                    p2.SendGump(new DialogueGump(p2, adviceModule));
                                });
                            p.SendGump(new DialogueGump(p, embodyModule));
                        });
                    player.SendGump(new DialogueGump(player, lessonModule));
                });

            mentorAdvice.AddOption("Recount a memory of mentoring Renn and the lessons shared.", 
                player => true, 
                player =>
                {
                    DialogueModule rennMemory = new DialogueModule(
                        "I remember a cold dusk when Renn, eyes alight with both hope and uncertainty, approached me. " +
                        "I spoke of our clan’s glorious past and the daunting challenge of reclaiming our honor. " +
                        "I urged him to fight not for conquest alone but for the dignity of our ancestry. " +
                        "Would you like to hear the exact words that shaped his destiny?"
                    );
                    rennMemory.AddOption("Yes, please share those words.", 
                        p => true,
                        p =>
                        {
                            DialogueModule wordsModule = new DialogueModule(
                                "I said, 'Renn, our blood flows with the valor of countless warriors. " +
                                "Let no shame tarnish the legacy we bear. Every hardship you face is but a stepping stone " +
                                "to a future where honor reigns supreme. Fight with all your heart, and let the echoes " +
                                "of our ancestors guide you to victory.'"
                            );
                            wordsModule.AddOption("That is powerful. How did Renn respond?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule responseModule = new DialogueModule(
                                        "With a solemn nod and a fire in his eyes, he vowed to carry our clan's honor " +
                                        "into every battle. His determination became a beacon of hope in these dark times."
                                    );
                                    p2.SendGump(new DialogueGump(p2, responseModule));
                                });
                            p.SendGump(new DialogueGump(p, wordsModule));
                        });
                    player.SendGump(new DialogueGump(player, rennMemory));
                });

            return mentorAdvice;
        }

        private DialogueModule CreatePersonalRegretsModule()
        {
            DialogueModule regrets = new DialogueModule(
                "Regret is the shadow that follows a warrior—ever-present and unyielding. I lament those moments " +
                "when duty forced my hand to act against my own mercy. Would you like to know what haunts me " +
                "the most, or how these sorrows have shaped my unbreakable resolve?"
            );

            regrets.AddOption("What is the deepest regret that weighs on your soul?", 
                player => true, 
                player =>
                {
                    DialogueModule regretMost = new DialogueModule(
                        "My deepest regret is the day I had to choose between the lives of my kin and the success of a perilous mission. " +
                        "That choice split my soul in two—one part clinging to life, the other lost in the mists of duty. " +
                        "Do you dare ask if I still seek forgiveness for that day?"
                    );
                    regretMost.AddOption("Is redemption possible after such a betrayal of your heart?", 
                        p => true, 
                        p =>
                        {
                            DialogueModule forgivenessModule = new DialogueModule(
                                "Redemption is a long, arduous journey—a path walked one painful step at a time. " +
                                "I seek solace by teaching the next generation, hoping that they may carve a nobler path " +
                                "than the one I was forced to tread."
                            );
                            p.SendGump(new DialogueGump(p, forgivenessModule));
                        });
                    player.SendGump(new DialogueGump(player, regretMost));
                });

            regrets.AddOption("Do you find any peace amidst the memories of battle?", 
                player => true, 
                player =>
                {
                    DialogueModule peaceModule = new DialogueModule(
                        "Peace, like the calm after a fierce storm, is transient. I find moments of serenity " +
                        "in the quiet lessons passed to young warriors and in the memories of our once-proud clan. " +
                        "Yet, the fierce battles of my past remind me that true peace is only earned on the field of honor."
                    );
                    peaceModule.AddOption("Your words carry the weight of wisdom. Thank you.",
                        p => true,
                        p =>
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });
                    player.SendGump(new DialogueGump(player, peaceModule));
                });

            return regrets;
        }

        private DialogueModule CreateClanBackgroundModule()
        {
            DialogueModule clanBackground = new DialogueModule(
                "There is a part of me that few dare to reveal—a legacy written in blood and honor. " +
                "I come from a proud warrior clan whose glory has faded with time, a once-formidable name " +
                "now diminished by the ravages of betrayal and neglect. My duty is to restore our honor " +
                "through glorious battle and unwavering resolve. What aspect of our heritage intrigues you?"
            );

            clanBackground.AddOption("Tell me about the origins of your clan.", 
                player => true, 
                player =>
                {
                    DialogueModule originsModule = new DialogueModule(
                        "Our clan was founded centuries ago by fierce warriors whose bravery lit the darkness " +
                        "of oppressive times. We were known as the Blade of Dawn—a beacon of hope and the embodiment " +
                        "of honor. Over time, internal strife and external enemies eroded our legacy. Would you like " +
                        "to know more about the legendary founders or the early glories of our clan?"
                    );
                    originsModule.AddOption("I would like to hear about the legendary founders.",
                        p => true,
                        p =>
                        {
                            DialogueModule foundersModule = new DialogueModule(
                                "The founders were men and women of unmatched valor, each renowned for feats " +
                                "that border on myth. Their names are spoken in hushed reverence and their deeds " +
                                "serve as the foundation of everything we hold dear. Their banners still inspire us, " +
                                "even as our numbers dwindle."
                            );
                            foundersModule.AddOption("How do you keep their memory alive?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule memoryModule = new DialogueModule(
                                        "I dedicate every battle, every lesson, to their memory. I wear our clan's crest " +
                                        "as a constant reminder that I must redeem our name, not just for myself, but for all " +
                                        "who came before me."
                                    );
                                    p2.SendGump(new DialogueGump(p2, memoryModule));
                                });
                            p.SendGump(new DialogueGump(p, foundersModule));
                        });
                    originsModule.AddOption("What were the early glories of the Blade of Dawn?",
                        p => true,
                        p =>
                        {
                            DialogueModule gloriesModule = new DialogueModule(
                                "In our prime, our warriors led epic campaigns that carved the very face of history. " +
                                "Legends speak of battles where the enemy faltered at the sight of our unwavering advance. " +
                                "That splendor now seems like a distant dream, but it fuels my every endeavor to reclaim it."
                            );
                            p.SendGump(new DialogueGump(p, gloriesModule));
                        });
                    player.SendGump(new DialogueGump(player, originsModule));
                });

            clanBackground.AddOption("How did your clan fall into decline?", 
                player => true, 
                player =>
                {
                    DialogueModule declineModule = new DialogueModule(
                        "Tragic betrayal and ceaseless internal strife led us astray. A once-united band of warriors " +
                        "was torn apart by greed and the lure of easy victory. The scars of those dark times are etched " +
                        "in my heart. Would you like to hear of that fateful betrayal, or how it transformed me?"
                    );
                    declineModule.AddOption("Tell me of the fateful betrayal.",
                        p => true,
                        p =>
                        {
                            DialogueModule betrayalModule = new DialogueModule(
                                "It was a night shrouded in deceit when trusted kin turned their blades against us. " +
                                "That act of treachery fractured our unity and cast our name into dishonor. I vowed then " +
                                "that I would one day restore what was lost, even if it meant wading through the blood of traitors."
                            );
                            betrayalModule.AddOption("And how did that shape your destiny?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule destinyModule = new DialogueModule(
                                        "It ignited a fire within me—a fierce determination to reclaim our legacy through every " +
                                        "ruthless battle and every honor-bound act. I would lead our surviving kin, even if the cost " +
                                        "was my soul."
                                    );
                                    p2.SendGump(new DialogueGump(p2, destinyModule));
                                });
                            p.SendGump(new DialogueGump(p, betrayalModule));
                        });
                    declineModule.AddOption("How has that betrayal changed you?",
                        p => true,
                        p =>
                        {
                            DialogueModule changeModule = new DialogueModule(
                                "It hardened me, instilling a pride and ferocity that are both my strength and my burden. " +
                                "The memory of that night fuels my every action in the present—I strive to be both a beacon " +
                                "of honor and a relentless avenger of those who wronged my kin."
                            );
                            p.SendGump(new DialogueGump(p, changeModule));
                        });
                    player.SendGump(new DialogueGump(player, declineModule));
                });

            clanBackground.AddOption("What is your ultimate goal for restoring your clan's honor?", 
                player => true, 
                player =>
                {
                    DialogueModule goalModule = new DialogueModule(
                        "My ultimate goal is simple yet profound: to lead my remaining kin into a new age of glory " +
                        "through heroic deeds and righteous battle. I seek to reclaim our family’s revered legacy, " +
                        "to prove that honor, though tarnished, can be reborn. Would you like to learn of the strategies " +
                        "and trials that lie ahead in this quest?"
                    );
                    goalModule.AddOption("Yes, tell me about your strategies and the challenges you face.",
                        p => true,
                        p =>
                        {
                            DialogueModule strategyModule = new DialogueModule(
                                "Our strategies are as relentless as our spirit. We train in hidden groves, " +
                                "study the ancient scrolls of our forefathers, and forge alliances with like-minded warriors. " +
                                "Each challenge—from vicious skirmishes with rogue mercenaries to epic clashes with pirate brigands— " +
                                "is a stepping stone to our rebirth."
                            );
                            strategyModule.AddOption("What alliances have you formed?",
                                p2 => true,
                                p2 =>
                                {
                                    DialogueModule alliancesModule = new DialogueModule(
                                        "Though our numbers dwindle, there are those who still believe in our cause—scholars, " +
                                        "mages, and even other clans scarred by similar betrayals. Their support strengthens our resolve, " +
                                        "and together we aim to turn the tide of history."
                                    );
                                    p2.SendGump(new DialogueGump(p2, alliancesModule));
                                });
                            p.SendGump(new DialogueGump(p, strategyModule));
                        });
                    goalModule.AddOption("Do you foresee personal sacrifice in this quest?",
                        p => true,
                        p =>
                        {
                            DialogueModule sacrificeModule = new DialogueModule(
                                "Every step towards redemption demands sacrifice. I am ready to face whatever the fates decree— " +
                                "for the honor of my clan is worth every hardship, every drop of blood spilled on the altar of glory."
                            );
                            p.SendGump(new DialogueGump(p, sacrificeModule));
                        });
                    player.SendGump(new DialogueGump(player, goalModule));
                });

            return clanBackground;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastStoryTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastStoryTime = reader.ReadDateTime();
        }
    }
}
