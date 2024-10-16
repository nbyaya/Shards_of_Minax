using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rydia")]
    public class Rydia : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Rydia() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rydia";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 60;
            Int = 120;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 233 });
            AddItem(new Cloak() { Hue = 572 });
            AddItem(new Sandals() { Hue = 2774 });
            AddItem(new FeatheredHat() { Hue = 2840 });
            AddItem(new Whip() { Name = "Rydia's Whip" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Rydia. How may I assist you today?");
            
            greeting.AddOption("Tell me about becoming a summoner.",
                player => true,
                player =>
                {
                    DialogueModule summonerModule = new DialogueModule("Becoming a summoner is no easy task. It requires immense dedication and an unwavering spirit. My journey began when I was a child, filled with dreams of calling upon powerful spirits.");
                    
                    summonerModule.AddOption("What challenges did you face?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule challengeModule = new DialogueModule("The path was fraught with trials. I faced skepticism from my peers and encountered many formidable creatures that tested my resolve. My training involved not just mastering spells but also understanding the essence of the spirits themselves.");
                            
                            challengeModule.AddOption("How did you overcome these challenges?",
                                plq => true,
                                plq =>
                                {
                                    DialogueModule overcomeModule = new DialogueModule("I sought guidance from ancient texts and mentors. Each encounter with a spirit taught me valuable lessons. Some spirits were kind and offered their wisdom, while others were hostile and required clever strategies to pacify.");
                                    
                                    overcomeModule.AddOption("Can you share a specific experience?",
                                        plw => true,
                                        plw =>
                                        {
                                            DialogueModule experienceModule = new DialogueModule("I remember one particular instance in the Whispering Woods, where I encountered a fierce spirit of the wild. It was angry at the disturbance caused by humans. I had to calm it using both my words and a ritual to show my respect for its domain.");
                                            
                                            experienceModule.AddOption("What was the outcome?",
                                                ple => true,
                                                ple =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("I managed to appease the spirit, and in return, it bestowed upon me a blessing that enhanced my abilities as a summoner. It taught me that respect and understanding can bridge the gap between our worlds.")));
                                                });
                                            experienceModule.AddOption("That sounds intense!",
                                                plr => true,
                                                plr =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! Each encounter was a lesson, and some were quite dangerous. But those moments shaped me into the summoner I am today.")));
                                                });
                                            pl.SendGump(new DialogueGump(pl, experienceModule));
                                        });
                                    overcomeModule.AddOption("What do you think is most important for a summoner?",
                                        plt => true,
                                        plt =>
                                        {
                                            DialogueModule importantModule = new DialogueModule("Patience and empathy. The spirits can sense your intentions. If your heart is true, they will respond positively. However, if you approach them with malice or greed, they can become hostile.");
                                            importantModule.AddOption("I see the value in that.",
                                                ply => true,
                                                ply => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                            pl.SendGump(new DialogueGump(pl, importantModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, overcomeModule));
                                });
                            challengeModule.AddOption("What did your mentors teach you?",
                                plu => true,
                                plu =>
                                {
                                    DialogueModule mentorModule = new DialogueModule("They taught me the importance of balance—between power and restraint, ambition and humility. They stressed that a true summoner is a protector of the realms, not just a wielder of magic.");
                                    mentorModule.AddOption("That's a wise lesson.",
                                        pli => true,
                                        pli => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                    pl.SendGump(new DialogueGump(pl, mentorModule));
                                });
                            pl.SendGump(new DialogueGump(pl, challengeModule));
                        });
                    
                    summonerModule.AddOption("What inspired you to become a summoner?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule inspireModule = new DialogueModule("I was captivated by the legends of summoners who could call forth powerful beings to aid them in battle. The stories spoke of deep connections and the harmony they created with the spirits. I yearned for that connection.");
                            inspireModule.AddOption("Do you feel connected to the spirits now?",
                                plo => true,
                                plo =>
                                {
                                    DialogueModule connectedModule = new DialogueModule("Absolutely! Each spirit I summon has its own personality and will. I've developed bonds with many of them over the years. It's a profound experience that goes beyond mere command.");
                                    connectedModule.AddOption("What are some of the spirits you summon?",
                                        plp => true,
                                        plp =>
                                        {
                                            DialogueModule spiritsModule = new DialogueModule("I often summon spirits of nature like the Sylphs and the Elementals. They have a gentle nature but can be fierce protectors when angered. There are also more exotic spirits, like the Phoenix, which requires great respect to summon.");
                                            spiritsModule.AddOption("The Phoenix sounds fascinating!",
                                                pla => true,
                                                pla =>
                                                {
                                                    DialogueModule phoenixModule = new DialogueModule("The Phoenix is a spirit of rebirth and fire. It teaches the importance of renewal and resilience. However, summoning it is incredibly demanding, requiring a rare offering to gain its favor.");
                                                    phoenixModule.AddOption("What offering do you need?",
                                                        pls => true,
                                                        pls =>
                                                        {
                                                            DialogueModule offeringModule = new DialogueModule("To summon the Phoenix, one must present a gem imbued with the essence of a star, alongside a heartfelt promise to protect the lands. It’s a heavy responsibility to bear.");
                                                            offeringModule.AddOption("Have you ever summoned it?",
                                                                pld => true,
                                                                pld =>
                                                                {
                                                                    DialogueModule summonModule = new DialogueModule("Once, I attempted to summon the Phoenix during a great crisis. My heart was pure, but I underestimated the energy required. The experience left me drained but taught me much about the balance of power.");
                                                                    summonModule.AddOption("That sounds incredible, yet daunting.",
                                                                        plf => true,
                                                                        plf => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                                    pl.SendGump(new DialogueGump(pl, summonModule));
                                                                });
                                                            offeringModule.AddOption("I'd love to learn more about spirits.",
                                                                plg => true,
                                                                plg => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                            pl.SendGump(new DialogueGump(pl, offeringModule));
                                                        });
                                                    phoenixModule.AddOption("Can you tell me about other spirits?",
                                                        plh => true,
                                                        plh =>
                                                        {
                                                            DialogueModule otherSpiritsModule = new DialogueModule("Certainly! There's the Sylph of the winds, which can aid in speed and travel, and the Elemental of earth, which embodies strength and resilience. Each spirit offers unique abilities.");
                                                            otherSpiritsModule.AddOption("I find that fascinating!",
                                                                plj => true,
                                                                plj => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                            pl.SendGump(new DialogueGump(pl, otherSpiritsModule));
                                                        });
                                                    pl.SendGump(new DialogueGump(pl, phoenixModule));
                                                });
                                            spiritsModule.AddOption("What happens if a spirit is angered?",
                                                plk => true,
                                                plk =>
                                                {
                                                    DialogueModule angerModule = new DialogueModule("Angered spirits can wreak havoc. They can disrupt the balance of nature and cause chaos in the realms. It is essential to always show them respect and understand their needs.");
                                                    angerModule.AddOption("I'll keep that in mind.",
                                                        pll => true,
                                                        pll => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                    pl.SendGump(new DialogueGump(pl, angerModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, spiritsModule));
                                        });
                                    connectedModule.AddOption("What is your favorite spirit?",
                                        plz => true,
                                        plz =>
                                        {
                                            DialogueModule favoriteModule = new DialogueModule("I hold a special fondness for the Sylphs. They are playful yet wise, and their connection to the winds grants me clarity in thought. Each encounter brings a new lesson.");
                                            favoriteModule.AddOption("That sounds lovely!",
                                                plx => true,
                                                plx => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                            pl.SendGump(new DialogueGump(pl, favoriteModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, connectedModule));
                                });
                            pl.SendGump(new DialogueGump(pl, inspireModule));
                        });
                    
                    player.SendGump(new DialogueGump(player, summonerModule));
                });

            greeting.AddOption("What was your time in the monster realms like?",
                player => true,
                player =>
                {
                    DialogueModule monsterRealmModule = new DialogueModule("The monster realms were both awe-inspiring and terrifying. Each realm had its own rules and inhabitants, making it a place of endless learning and peril.");
                    
                    monsterRealmModule.AddOption("What did you learn there?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule learningModule = new DialogueModule("I learned the importance of adaptability. Each encounter was unique, and I had to use all of my skills to navigate the dangers. The monsters taught me lessons in survival and strategy.");
                            
                            learningModule.AddOption("What kinds of monsters did you encounter?",
                                plc => true,
                                plc =>
                                {
                                    DialogueModule typesModule = new DialogueModule("I faced many creatures—some majestic, others terrifying. From the gentle spirits of the forest realms to the fierce beasts of the fire realms, each had its own nature and temperament.");
                                    typesModule.AddOption("Which was the most challenging?",
                                        plv => true,
                                        plv =>
                                        {
                                            DialogueModule challengingModule = new DialogueModule("The most challenging was a fearsome dragon that guarded a sacred grove. It was relentless and intelligent, and I had to devise a clever strategy to avoid a direct confrontation.");
                                            
                                            challengingModule.AddOption("How did you handle that?",
                                                plb => true,
                                                plb =>
                                                {
                                                    DialogueModule handleModule = new DialogueModule("I used the environment to my advantage. I summoned the winds to confuse it and used illusions to distract it while I gathered the sacred herbs I needed.");
                                                    handleModule.AddOption("That's impressive!",
                                                        pln => true,
                                                        pln => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                    pl.SendGump(new DialogueGump(pl, handleModule));
                                                });
                                            typesModule.AddOption("Did you ever befriend a monster?",
                                                plm => true,
                                                plm =>
                                                {
                                                    DialogueModule friendshipModule = new DialogueModule("Yes! I managed to befriend a small elemental creature. It became my guide in the realm and helped me navigate the dangers. Our bond was special, and it taught me the value of trust.");
                                                    friendshipModule.AddOption("That's amazing! What did it teach you?",
                                                        plqq => true,
                                                        plqq =>
                                                        {
                                                            DialogueModule teachModule = new DialogueModule("It taught me that even the smallest beings can hold great power and wisdom. Respecting all creatures, no matter how intimidating they may seem, is essential in this world.");
                                                            teachModule.AddOption("I will remember that.",
                                                                plww => true,
                                                                plww => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                                            pl.SendGump(new DialogueGump(pl, teachModule));
                                                        });
                                                    pl.SendGump(new DialogueGump(pl, friendshipModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, challengingModule));
                                        });
                                    monsterRealmModule.AddOption("Did any of the monsters help you?",
                                        plee => true,
                                        plee =>
                                        {
                                            DialogueModule helpModule = new DialogueModule("Indeed! Some monsters provided guidance or assistance when they sensed my intentions. A wise creature once shared the secret of a hidden passage that saved me from a perilous situation.");
                                            helpModule.AddOption("What a fortunate encounter!",
                                                plrr => true,
                                                plrr => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                            pl.SendGump(new DialogueGump(pl, helpModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, typesModule));
                                });
                            player.SendGump(new DialogueGump(player, learningModule));
                        });
                    
                    monsterRealmModule.AddOption("Did you ever feel afraid?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule fearModule = new DialogueModule("Of course. Fear is a natural reaction. But I learned to embrace it. Instead of allowing it to paralyze me, I used it to fuel my determination to overcome obstacles.");
                            
                            fearModule.AddOption("That's an inspiring perspective.",
                                pltt => true,
                                pltt => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            player.SendGump(new DialogueGump(player, fearModule));
                        });
                    player.SendGump(new DialogueGump(player, monsterRealmModule));
                });

            greeting.AddOption("What do you think about the future?",
                player => true,
                player =>
                {
                    DialogueModule futureModule = new DialogueModule("I hope for a future where summoners and spirits can work together to protect our realms. The balance between nature and civilization is delicate, and it requires our collective efforts.");
                    
                    futureModule.AddOption("That sounds noble.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    futureModule.AddOption("I would like to help with that!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule helpFutureModule = new DialogueModule("Every bit helps! By respecting nature and learning from it, we can forge a better world for all. Perhaps we can collaborate in the future.");
                            helpFutureModule.AddOption("I'd love that!",
                                plyy => true,
                                plyy => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                            player.SendGump(new DialogueGump(player, helpFutureModule));
                        });
                    player.SendGump(new DialogueGump(player, futureModule));
                });

            return greeting;
        }

        public Rydia(Serial serial) : base(serial) { }

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
