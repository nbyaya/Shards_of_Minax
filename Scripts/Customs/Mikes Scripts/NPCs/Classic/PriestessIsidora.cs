using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Priestess Isidora")]
    public class PriestessIsidora : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PriestessIsidora() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Priestess Isidora";
            Body = 0x191; // Human female body

            // Stats
            SetStr(60);
            SetDex(50);
            SetInt(120);
            SetHits(50);

            // Appearance
            AddItem(new Robe() { Hue = 1150 });
            AddItem(new Sandals() { Hue = 1150 });

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
            DialogueModule greeting = new DialogueModule("Welcome, traveler. I am Priestess Isidora, a humble servant of the divine. The cycles of the moon hold great power and significance in our rituals. How may I assist you in your understanding of them?");

            greeting.AddOption("Tell me about lunar worship.",
                player => true,
                player => 
                {
                    DialogueModule lunarWorshipModule = new DialogueModule("Lunar worship is a sacred practice that honors the cycles of the moon. Each phase has its own meaning and significance. Would you like to learn about the different phases or perhaps the rituals associated with them?");
                    lunarWorshipModule.AddOption("What are the phases of the moon?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule phasesModule = new DialogueModule("The moon goes through various phases: New Moon, Waxing Crescent, First Quarter, Waxing Gibbous, Full Moon, Waning Gibbous, Last Quarter, and Waning Crescent. Each phase brings its own energies and opportunities for reflection.");
                            phasesModule.AddOption("Tell me about the New Moon.",
                                pla => true,
                                pla =>
                                {
                                    DialogueModule newMoonModule = new DialogueModule("The New Moon symbolizes new beginnings and fresh starts. It is a time for setting intentions and planning for the future. Rituals during this phase often involve meditation and writing down your goals.");
                                    newMoonModule.AddOption("What rituals can I perform?",
                                        p => true,
                                        p =>
                                        {
                                            DialogueModule newMoonRituals = new DialogueModule("You can perform a simple ritual by lighting a white candle, sitting in silence, and visualizing your goals. Consider writing them down on paper and keeping it somewhere special.");
                                            newMoonRituals.AddOption("I will try that.",
                                                plq => true,
                                                plq => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                            newMoonRituals.AddOption("Tell me about the Waxing Crescent.",
                                                plw => true,
                                                plw => pl.SendGump(new DialogueGump(pl, CreateWaxingCrescentModule())));
                                            p.SendGump(new DialogueGump(p, newMoonRituals));
                                        });
                                    pla.SendGump(new DialogueGump(pla, newMoonModule));
                                });
                            phasesModule.AddOption("What about the Full Moon?",
                                ple => true,
                                ple =>
                                {
                                    DialogueModule fullMoonModule = new DialogueModule("The Full Moon represents fulfillment, completion, and illumination. It is a powerful time for manifesting intentions and celebrating your accomplishments. Rituals often include gratitude and releasing what no longer serves you.");
                                    fullMoonModule.AddOption("What rituals can I perform?",
                                        p => true,
                                        p =>
                                        {
                                            DialogueModule fullMoonRituals = new DialogueModule("A Full Moon ritual can involve creating a circle of crystals around you, reflecting on your achievements, and offering thanks to the moon. You can also write down what you want to release and burn the paper safely.");
                                            fullMoonRituals.AddOption("I will perform this ritual.",
                                                plr => true,
                                                plr => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                            fullMoonRituals.AddOption("Tell me about the Waning Moon.",
                                                plt => true,
                                                plt => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                            p.SendGump(new DialogueGump(p, fullMoonRituals));
                                        });
                                    pl.SendGump(new DialogueGump(pl, fullMoonModule));
                                });
                            greeting.AddOption("What significance do the moon phases hold?",
                                playera => true,
                                playera =>
                                {
                                    DialogueModule significanceModule = new DialogueModule("Each moon phase influences our emotional and spiritual states. They guide us in timing our intentions and actions for the best outcomes. Would you like to explore how to align your energy with these phases?");
                                    significanceModule.AddOption("Yes, please.",
                                        pls => true,
                                        pls => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                    significanceModule.AddOption("Maybe later.",
                                        pld => true,
                                        pld => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                                    player.SendGump(new DialogueGump(player, significanceModule));
                                });
                            pl.SendGump(new DialogueGump(pl, phasesModule));
                        });
                    lunarWorshipModule.AddOption("What about the rituals?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule ritualsModule = new DialogueModule("Rituals can vary widely, but they often include offerings, meditations, and prayers. Each phase offers different rituals. Would you like to learn about specific rituals for each moon phase?");
                            ritualsModule.AddOption("Yes, tell me more.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                            ritualsModule.AddOption("I have other questions.",
                                pla => true,
                                pla => pla.SendGump(new DialogueGump(pla, CreateGreetingModule())));
                            player.SendGump(new DialogueGump(player, ritualsModule));
                        });
                    player.SendGump(new DialogueGump(player, lunarWorshipModule));
                });

            greeting.AddOption("Do you offer guidance?",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, CreateGuidanceModule()));
                });

            return greeting;
        }

        private DialogueModule CreateWaxingCrescentModule()
        {
            DialogueModule waxingCrescentModule = new DialogueModule("The Waxing Crescent is a time of hope and growth. It encourages you to nurture your intentions. Rituals often involve planting seeds—both literally and metaphorically.");
            waxingCrescentModule.AddOption("What can I do during this phase?",
                player => true,
                player =>
                {
                    DialogueModule waxingRituals = new DialogueModule("You can plant seeds in your garden or journal about your aspirations. Create a vision board to visualize what you want to manifest in your life.");
                    waxingRituals.AddOption("I will create a vision board.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    waxingRituals.AddOption("What comes after the Waxing Crescent?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateFirstQuarterModule())));
                    player.SendGump(new DialogueGump(player, waxingRituals));
                });
            return waxingCrescentModule;
        }

        private DialogueModule CreateFirstQuarterModule()
        {
            DialogueModule firstQuarterModule = new DialogueModule("The First Quarter is about overcoming challenges. It's a time to take action on your intentions and face any obstacles.");
            firstQuarterModule.AddOption("How can I embrace this energy?",
                player => true,
                player =>
                {
                    DialogueModule actionModule = new DialogueModule("You can write down the obstacles you face and strategize on how to overcome them. It is a good time to seek support from friends or mentors.");
                    actionModule.AddOption("I will write them down.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    actionModule.AddOption("What comes next?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateWaxingGibbousModule())));
                    player.SendGump(new DialogueGump(player, actionModule));
                });
            return firstQuarterModule;
        }

        private DialogueModule CreateWaxingGibbousModule()
        {
            DialogueModule waxingGibbousModule = new DialogueModule("The Waxing Gibbous is a time of refinement and preparation. Focus on refining your goals and intentions.");
            waxingGibbousModule.AddOption("What should I focus on?",
                player => true,
                player =>
                {
                    DialogueModule refinementModule = new DialogueModule("Consider what adjustments you can make to bring your goals to fruition. Reflect on your progress and tweak your plans if necessary.");
                    refinementModule.AddOption("I will reflect on my progress.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    refinementModule.AddOption("What about the Full Moon?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateFullMoonModule())));
                    player.SendGump(new DialogueGump(player, refinementModule));
                });
            return waxingGibbousModule;
        }

        private DialogueModule CreateFullMoonModule()
        {
            DialogueModule fullMoonModule = new DialogueModule("The Full Moon represents fulfillment and celebration. It is a powerful time for manifesting intentions and celebrating your accomplishments.");
            fullMoonModule.AddOption("What rituals can I perform?",
                player => true,
                player =>
                {
                    DialogueModule fullMoonRituals = new DialogueModule("A Full Moon ritual can involve creating a circle of crystals around you, reflecting on your achievements, and offering thanks to the moon. You can also write down what you want to release and burn the paper safely.");
                    fullMoonRituals.AddOption("I will perform this ritual.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    fullMoonRituals.AddOption("What comes after the Full Moon?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateWaningGibbousModule())));
                    player.SendGump(new DialogueGump(player, fullMoonRituals));
                });
            return fullMoonModule;
        }

        private DialogueModule CreateWaningGibbousModule()
        {
            DialogueModule waningGibbousModule = new DialogueModule("The Waning Gibbous is a time of reflection and gratitude. It's essential to take a step back and assess your journey.");
            waningGibbousModule.AddOption("How can I reflect?",
                player => true,
                player =>
                {
                    DialogueModule reflectionModule = new DialogueModule("Take some time to journal about your experiences during the full moon phase. Acknowledge your growth and the lessons you've learned.");
                    reflectionModule.AddOption("I will journal my experiences.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    reflectionModule.AddOption("What about the Last Quarter?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateLastQuarterModule())));
                    player.SendGump(new DialogueGump(player, reflectionModule));
                });
            return waningGibbousModule;
        }

        private DialogueModule CreateLastQuarterModule()
        {
            DialogueModule lastQuarterModule = new DialogueModule("The Last Quarter is a time for release and letting go. Focus on what no longer serves you.");
            lastQuarterModule.AddOption("What should I release?",
                player => true,
                player =>
                {
                    DialogueModule releaseModule = new DialogueModule("Consider negative thoughts, habits, or relationships that hinder your progress. Create a ritual to let them go, perhaps by writing them down and burning the paper as a symbol of release.");
                    releaseModule.AddOption("I will create a release ritual.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    releaseModule.AddOption("What about the Waning Crescent?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateWaningCrescentModule())));
                    player.SendGump(new DialogueGump(player, releaseModule));
                });
            return lastQuarterModule;
        }

        private DialogueModule CreateWaningCrescentModule()
        {
            DialogueModule waningCrescentModule = new DialogueModule("The Waning Crescent is a time of rest and preparation for the New Moon. It's essential to reflect and rejuvenate.");
            waningCrescentModule.AddOption("How can I rejuvenate?",
                player => true,
                player =>
                {
                    DialogueModule restModule = new DialogueModule("Consider practicing self-care, meditation, or spending time in nature. Allow yourself to recharge for the new intentions to come.");
                    restModule.AddOption("I will take time for self-care.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    restModule.AddOption("What comes after the Waning Crescent?",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateNewMoonModule())));
                    player.SendGump(new DialogueGump(player, restModule));
                });
            return waningCrescentModule;
        }

        private DialogueModule CreateNewMoonModule()
        {
            DialogueModule newMoonModule = new DialogueModule("The cycle begins anew with the New Moon, symbolizing new beginnings and fresh starts. Embrace the opportunity to set your intentions once again.");
            newMoonModule.AddOption("What should I do during this phase?",
                player => true,
                player =>
                {
                    DialogueModule newMoonRituals = new DialogueModule("You can perform a simple ritual by lighting a white candle, sitting in silence, and visualizing your goals. Consider writing them down on paper and keeping it somewhere special.");
                    newMoonRituals.AddOption("I will try that.",
                        pl => true,
                        pl => pl.SendGump(new DialogueGump(pl, CreateGreetingModule())));
                    player.SendGump(new DialogueGump(player, newMoonRituals));
                });
            return newMoonModule;
        }

        private DialogueModule CreateGuidanceModule()
        {
            DialogueModule guidanceModule = new DialogueModule("Many seek guidance to find their purpose or to make amends. The path to redemption is often paved with good intentions. Have you wronged someone and seek redemption?");
            guidanceModule.AddOption("Yes, I have.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            guidanceModule.AddOption("No, I am at peace.",
                player => true,
                player => player.SendGump(new DialogueGump(player, CreateGreetingModule())));
            return guidanceModule;
        }

        public PriestessIsidora(Serial serial) : base(serial) { }

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
