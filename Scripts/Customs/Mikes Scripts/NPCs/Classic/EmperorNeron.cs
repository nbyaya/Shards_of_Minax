using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Emperor Nero")]
    public class EmperorNeron : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmperorNeron() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Emperor Neron";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 65;
            Int = 85;
            Hits = 85;

            // Appearance
            AddItem(new Robe() { Hue = 1157 });
            AddItem(new GoldRing() { Name = "Emperor Nero's Ring" });
            AddItem(new Boots() { Hue = 1175 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            SpeechHue = 0; // Default speech hue
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
            DialogueModule greeting = new DialogueModule("I am Emperor Neron, ruler of this wretched land. What do you seek, traveler?");

            greeting.AddOption("Tell me about the Great Fire of Rome.",
                player => true,
                player => {
                    DialogueModule fireModule = new DialogueModule("Ah, the fire that consumed the city! Some call it tragedy; I see it as a necessary rebirth. My palace, the Domus Aurea, stands as a testament to my vision. Do you understand the need for sacrifice for greatness?");
                    
                    fireModule.AddOption("Were you responsible for the fire?",
                        pl => true,
                        pl => {
                            DialogueModule responsibilityModule = new DialogueModule("Responsibility is a heavy burden. While the flames danced through the night, I found solace in the idea that great art often arises from destruction. My ambition to build the finest palace cannot be understated.");
                            
                            responsibilityModule.AddOption("But what of the suffering?",
                                p => true,
                                p => {
                                    DialogueModule sufferingModule = new DialogueModule("Suffering is the price of ambition, my friend. The common folk lament their losses, but true greatness requires sacrifice. This empire demands more than mediocrity!");
                                    
                                    sufferingModule.AddOption("That sounds cruel.",
                                        plq => true,
                                        plq => {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    sufferingModule.AddOption("You seem to justify your actions.",
                                        plw => true,
                                        plw => {
                                            DialogueModule justificationModule = new DialogueModule("Justification is often a matter of perspective. I sought to elevate Rome to heights unseen! The whispers of dissent are nothing compared to the legacy I shall leave.");
                                            
                                            justificationModule.AddOption("What legacy do you wish to leave?",
                                                pe => true,
                                                pe => {
                                                    DialogueModule legacyModule = new DialogueModule("A legacy of grandeur! A city transformed into a magnificent tapestry of marble and gold, where art and culture thrive under my reign. Would you not desire such a vision?");
                                                    
                                                    legacyModule.AddOption("Indeed, I would.",
                                                        plr => true,
                                                        plr => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    legacyModule.AddOption("Such dreams can lead to ruin.",
                                                        plt => true,
                                                        plt => {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    p.SendGump(new DialogueGump(p, legacyModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, justificationModule));
                                        });
                                    p.SendGump(new DialogueGump(p, sufferingModule));
                                });
                            pl.SendGump(new DialogueGump(pl, responsibilityModule));
                        });

                    fireModule.AddOption("What did you feel during the fire?",
                        pl => true,
                        pl => {
                            DialogueModule feelingsModule = new DialogueModule("In the chaos, I felt exhilaration! The city was reborn before my very eyes. I stood atop my palace, observing the flames, envisioning the wonders that would rise from the ashes.");
                            
                            feelingsModule.AddOption("Is that not heartless?",
                                p => true,
                                p => {
                                    DialogueModule heartlessModule = new DialogueModule("Heartless? Perhaps. But one must possess a cold heart to ignite the flames of greatness. The strong shape their destiny, while the weak are left to weep.");
                                    
                                    heartlessModule.AddOption("You view things very differently.",
                                        ply => true,
                                        ply => {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    heartlessModule.AddOption("There must be balance in ambition.",
                                        plu => true,
                                        plu => {
                                            DialogueModule balanceModule = new DialogueModule("Balance is an illusion in this chaotic world. Only the relentless rise to greatness! Those who hesitate are doomed to mediocrity.");
                                            
                                            balanceModule.AddOption("I hope you find peace.",
                                                pi => true,
                                                pi => {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            balanceModule.AddOption("Perhaps I disagree.",
                                                po => true,
                                                po => {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            pl.SendGump(new DialogueGump(pl, balanceModule));
                                        });
                                    p.SendGump(new DialogueGump(p, heartlessModule));
                                });
                            pl.SendGump(new DialogueGump(pl, feelingsModule));
                        });

                    fireModule.AddOption("What of the rebuilding?",
                        pl => true,
                        pl => {
                            DialogueModule rebuildingModule = new DialogueModule("Rebuilding was a monumental task, yet I embraced it with fervor. The Domus Aurea became a sanctuary of beauty, adorned with lavish gardens and golden mosaics. My vision was grander than the flames that preceded it.");
                            
                            rebuildingModule.AddOption("And what of the people displaced?",
                                p => true,
                                p => {
                                    DialogueModule displacedModule = new DialogueModule("Displacement is regrettable but necessary for progress. They will rebuild and thrive, inspired by the glory of my reign. Rome shall rise stronger and more beautiful.");
                                    
                                    displacedModule.AddOption("What if they do not return?",
                                        plp => true,
                                        plp => {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    displacedModule.AddOption("Your arrogance is astounding.",
                                        pla => true,
                                        pla => {
                                            DialogueModule arroganceModule = new DialogueModule("Arrogance? Or confidence? I prefer to see it as the latter. Only those with true vision can steer the course of history.");
                                            
                                            arroganceModule.AddOption("A dangerous path, indeed.",
                                                ps => true,
                                                ps => {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            p.SendGump(new DialogueGump(p, arroganceModule));
                                        });
                                    p.SendGump(new DialogueGump(p, displacedModule));
                                });
                            pl.SendGump(new DialogueGump(pl, rebuildingModule));
                        });

                    player.SendGump(new DialogueGump(player, fireModule));
                });

            greeting.AddOption("Do you regret your actions?",
                player => true,
                player => {
                    DialogueModule regretModule = new DialogueModule("Regret is for the weak! I do not look back on my decisions with sorrow; instead, I embrace them as part of my grand design. Every action has its purpose.");
                    
                    regretModule.AddOption("Even if it caused harm?",
                        pl => true,
                        pl => {
                            DialogueModule harmModule = new DialogueModule("Harm is a consequence of ambition. A ruler must make difficult choices for the greater good. Have you not heard of the saying, 'The road to greatness is paved with sacrifice'?");
                            
                            harmModule.AddOption("That sounds like an excuse.",
                                p => true,
                                p => {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            harmModule.AddOption("I can see your point.",
                                p => true,
                                p => {
                                    DialogueModule understandingModule = new DialogueModule("Ah, understanding is a rare gift! If only more could grasp the complexity of leadership. It is a burden I bear with pride.");
                                    
                                    understandingModule.AddOption("You are a complex man.",
                                        plf => true,
                                        plf => {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    understandingModule.AddOption("I still cannot condone your actions.",
                                        plg => true,
                                        plg => {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    p.SendGump(new DialogueGump(p, understandingModule));
                                });
                            player.SendGump(new DialogueGump(player, harmModule));
                        });

                    player.SendGump(new DialogueGump(player, regretModule));
                });

            greeting.AddOption("What is your vision for Rome?",
                player => true,
                player => {
                    DialogueModule visionModule = new DialogueModule("Rome will become the jewel of the world! A city of unparalleled beauty and culture, where art flourishes and the spirit of innovation thrives. Under my rule, we will reshape the destiny of this empire!");
                    
                    visionModule.AddOption("How will you achieve this?",
                        pl => true,
                        pl => {
                            DialogueModule achievementModule = new DialogueModule("Through passion, artistry, and unwavering determination! I will gather the greatest minds of our time, building a renaissance that will echo through the ages. The world will marvel at our transformation!");
                            
                            achievementModule.AddOption("What if they resist?",
                                p => true,
                                p => {
                                    DialogueModule resistanceModule = new DialogueModule("Resistance is inevitable, but it is my duty to overcome it! Those who stand against progress will be swept aside like dust in the wind. Change requires strength.");
                                    
                                    resistanceModule.AddOption("Your words are bold.",
                                        plh => true,
                                        plh => {
                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                        });
                                    resistanceModule.AddOption("Such hubris may lead to your downfall.",
                                        plj => true,
                                        plj => {
                                            DialogueModule downfallModule = new DialogueModule("Downfall is for the faint-hearted. I fear nothing and will allow nothing to obstruct my path! Rome shall shine brighter than ever before.");
                                            
                                            downfallModule.AddOption("I hope you succeed.",
                                                pk => true,
                                                pk => {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            downfallModule.AddOption("Time will tell.",
                                                plv => true,
                                                plv => {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            pl.SendGump(new DialogueGump(pl, downfallModule));
                                        });
                                    p.SendGump(new DialogueGump(p, resistanceModule));
                                });
                            player.SendGump(new DialogueGump(player, achievementModule));
                        });
                    player.SendGump(new DialogueGump(player, visionModule));
                });

            return greeting;
        }

        public EmperorNeron(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastRewardTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastRewardTime = reader.ReadDateTime();
        }
    }
}
