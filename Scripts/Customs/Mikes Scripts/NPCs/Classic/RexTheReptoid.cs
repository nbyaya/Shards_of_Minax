using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rex the Reptoid")]
    public class RexTheReptoid : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RexTheReptoid() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rex the Reptoid";
            Body = 0x1C; // Reptoid body

            // Stats
            Str = 120;
            Dex = 80;
            Int = 40;
            Hits = 90;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 2121 });
            AddItem(new LeatherChest() { Hue = 2121 });
            AddItem(new LeatherGloves() { Hue = 2121 });
            AddItem(new PlateHelm() { Hue = 2122 });
            AddItem(new Axe() { Name = "Rex's Fang" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
            DialogueModule greeting = new DialogueModule("Greetings, I am Rex the Reptoid, a scout for the Reptoid Space Empire! My mission is to survey this planet for future conquest. What brings you to my side?");

            greeting.AddOption("Tell me about the Reptoid Space Empire.",
                player => true,
                player => 
                {
                    DialogueModule empireModule = new DialogueModule("The Reptoid Space Empire is a vast interstellar civilization known for its advanced technology and strategic prowess. We travel the cosmos, expanding our territory and knowledge. What aspects of our Empire intrigue you?");
                    
                    empireModule.AddOption("What is your mission here?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule missionModule = new DialogueModule("I am here to gather intelligence on the inhabitants and resources of this planet. My goal is to determine its strategic value for conquest. Are you aware of any threats that may impede our progress?");
                            
                            missionModule.AddOption("I can help with your mission.",
                                p => true,
                                p =>
                                {
                                    DialogueModule helpModule = new DialogueModule("Your offer is appreciated! I need local intel on the creatures that roam this land. Do you have any information on their strengths or weaknesses?");
                                    
                                    helpModule.AddOption("I know of a fierce creature in the woods.",
                                        plq => true,
                                        plq =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Tell me more about this creature!")));
                                        });

                                    helpModule.AddOption("I have seen nothing of concern.",
                                        plw => true,
                                        plw =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Very well, I will continue my observations.")));
                                        });

                                    p.SendGump(new DialogueGump(p, helpModule));
                                });

                            missionModule.AddOption("What are your views on conquest?",
                                p => true,
                                p =>
                                {
                                    DialogueModule conquestModule = new DialogueModule("Conquest is the natural order of the universe. The strong thrive, and the weak must adapt or perish. It is essential for the growth of the Reptoid Empire. Do you believe in the strength of our ideals?");
                                    
                                    conquestModule.AddOption("Yes, strength is vital.",
                                        ple => true,
                                        ple =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Indeed! You understand the cosmic balance.")));
                                        });

                                    conquestModule.AddOption("No, we should seek peace.",
                                        plr => true,
                                        plr =>
                                        {
                                            DialogueModule peaceModule = new DialogueModule("Peace is but a fleeting illusion. History shows that peace often precedes turmoil. However, I respect your perspective. What would you propose instead?");
                                            
                                            peaceModule.AddOption("We should negotiate.",
                                                plt => true,
                                                plt =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Negotiation can yield valuable alliances. But trust must be earned, not freely given.")));
                                                });

                                            peaceModule.AddOption("Perhaps coexistence is possible.",
                                                ply => true,
                                                ply =>
                                                {
                                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Coexistence is a delicate dance. Each party must recognize their place. How do you suggest we achieve this balance?")));
                                                });

                                            p.SendGump(new DialogueGump(p, peaceModule));
                                        });

                                    p.SendGump(new DialogueGump(p, conquestModule));
                                });

                            player.SendGump(new DialogueGump(player, missionModule));
                        });

                    empireModule.AddOption("What technologies does your Empire possess?",
                        playeru => true,
                        playeru =>
                        {
                            DialogueModule techModule = new DialogueModule("Our Empire boasts advanced technologies, including interstellar travel, energy manipulation, and biological enhancements. We utilize these to maintain our dominance across galaxies. Would you like to learn about a specific technology?");
                            
                            techModule.AddOption("Tell me about interstellar travel.",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Interstellar travel allows us to traverse vast distances in the blink of an eye, utilizing wormholes and advanced propulsion systems.")));
                                });

                            techModule.AddOption("What about energy manipulation?",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("We harness and manipulate energy sources from stars and planetary bodies, using them to power our technology and sustain our colonies.")));
                                });

                            techModule.AddOption("What are biological enhancements?",
                                pl => true,
                                pl =>
                                {
                                    pl.SendGump(new DialogueGump(pl, new DialogueModule("Our scientists have developed methods to enhance our physical and mental capabilities, ensuring our supremacy in any environment.")));
                                });

                            player.SendGump(new DialogueGump(player, techModule));
                        });

                    empireModule.AddOption("Goodbye.",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                        });

                    player.SendGump(new DialogueGump(player, empireModule));
                });

            greeting.AddOption("What can you tell me about your observations?",
                player => true,
                player =>
                {
                    DialogueModule observationsModule = new DialogueModule("As a scout, I observe the inhabitants and their behaviors. They display unique characteristics that vary by region. What aspect of my observations interests you?");
                    
                    observationsModule.AddOption("Tell me about the local fauna.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule faunaModule = new DialogueModule("The fauna here is diverse, from small critters to large beasts. Some are hostile, while others show curiosity. Each species plays a role in the ecosystem. Are you familiar with any specific creatures?");
                            
                            faunaModule.AddOption("I've encountered a fierce beast in the mountains.",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Describe this beast! I need all the intel I can gather.")));
                                });

                            faunaModule.AddOption("The creatures seem harmless.",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Interesting... I will keep a close watch.")));
                                });

                            player.SendGump(new DialogueGump(player, faunaModule));
                        });

                    observationsModule.AddOption("What about the inhabitants?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule inhabitantModule = new DialogueModule("The inhabitants exhibit a range of cultures and societies. They possess varying degrees of intelligence and technology. What specific details do you wish to know?");
                            
                            inhabitantModule.AddOption("Do they pose a threat?",
                                p => true,
                                p =>
                                {
                                    DialogueModule threatModule = new DialogueModule("Many are wary of outsiders and will defend their territory. However, others may be open to alliances. Understanding their motives is crucial. What are your thoughts on them?");
                                    
                                    threatModule.AddOption("They could be valuable allies.",
                                        pli => true,
                                        pli =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Alliances can strengthen our position, but trust is a fragile thing.")));
                                        });

                                    threatModule.AddOption("They are a danger to us.",
                                        plo => true,
                                        plo =>
                                        {
                                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Dangerous indeed! Precaution and surveillance are essential.")));
                                        });

                                    p.SendGump(new DialogueGump(p, threatModule));
                                });

                            inhabitantModule.AddOption("Goodbye.",
                                pla => true,
                                pla =>
                                {
                                    pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                });

                            player.SendGump(new DialogueGump(player, inhabitantModule));
                        });

                    player.SendGump(new DialogueGump(player, observationsModule));
                });

            greeting.AddOption("What is your health like?",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My health is as stable as the cosmic currents.")));
                });

            greeting.AddOption("What do you know about journeys?",
                player => true,
                player => 
                {
                    DialogueModule journeyModule = new DialogueModule("Ah, the journey. It is ever winding and full of unexpected turns. What drives your journey?");
                    player.SendGump(new DialogueGump(player, journeyModule));
                });

            return greeting;
        }

        private void SayReward(PlayerMobile player)
        {
            Say("To truly embrace humility, one must let go of ego, pride, and the thirst for recognition. Here, take this as a token of encouragement.");
            player.AddToBackpack(new MaxxiaScroll()); // Give the reward
            lastRewardTime = DateTime.UtcNow; // Update the timestamp
        }

        public RexTheReptoid(Serial serial) : base(serial) { }

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
