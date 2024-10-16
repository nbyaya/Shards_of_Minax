using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Professor Orion")]
    public class ProfessorOrion : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfessorOrion() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Professor Orion";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 55;
            Int = 110;
            Hits = 65;

            // Appearance
            AddItem(new LongPants() { Hue = 1103 });
            AddItem(new Doublet() { Hue = 1109 });
            AddItem(new Cloak() { Hue = 1105 });
            AddItem(new Sandals() { Hue = 0 });

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
            DialogueModule greeting = new DialogueModule("Ah! A curious traveler! I am Professor Orion, a brilliant scientist, or so they say. What knowledge do you seek about the universe and consciousness?");
            
            greeting.AddOption("Tell me your name.",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, CreateGreetingModule()));
                });

            greeting.AddOption("What do you think about consciousness?",
                player => true,
                player => 
                {
                    DialogueModule consciousnessModule = new DialogueModule("Consciousness is a complex enigma. Is it merely the byproduct of neural processes, or is it something more profound—perhaps a connection to the quantum fabric of reality?");
                    consciousnessModule.AddOption("Is consciousness linked to quantum mechanics?",
                        p => true,
                        p => 
                        {
                            DialogueModule quantumConsciousnessModule = new DialogueModule("Indeed! Some theorists propose that consciousness collapses quantum wave functions, influencing the outcome of events at the subatomic level. What do you think?");
                            quantumConsciousnessModule.AddOption("That sounds intriguing. How does it work?",
                                pl => true,
                                pl => 
                                {
                                    DialogueModule mechanicsModule = new DialogueModule("At the quantum level, particles exist in a state of probability until observed. This act of observation may require consciousness itself to actualize reality. It's a fascinating interplay of observer and observed.");
                                    mechanicsModule.AddOption("Does this mean reality is subjective?",
                                        pla => true,
                                        pla => 
                                        {
                                            DialogueModule subjectiveRealityModule = new DialogueModule("In a sense, yes! If reality is influenced by our observations, then each individual's consciousness shapes their own version of reality. But that leads us to the question: Is there an objective reality at all?");
                                            subjectiveRealityModule.AddOption("What do you mean by objective reality?",
                                                pq => true,
                                                pq => 
                                                {
                                                    DialogueModule objectiveRealityModule = new DialogueModule("Objective reality would be a reality that exists independently of observation. However, the quantum perspective suggests our consciousness plays an active role in shaping it. The lines blur!");
                                                    p.SendGump(new DialogueGump(p, objectiveRealityModule));
                                                });
                                            pla.SendGump(new DialogueGump(pla, subjectiveRealityModule));
                                        });
                                    pl.SendGump(new DialogueGump(pl, mechanicsModule));
                                });
                            quantumConsciousnessModule.AddOption("That's a heavy concept. What about free will?",
                                pl => true,
                                pl => 
                                {
                                    DialogueModule freeWillModule = new DialogueModule("Ah, free will! If consciousness is intertwined with quantum mechanics, does that mean our choices are predetermined by quantum probabilities? Or do we have the power to influence them?");
                                    freeWillModule.AddOption("I believe we have free will.",
                                        pla => true,
                                        pla => 
                                        {
                                            DialogueModule freeWillBeliefModule = new DialogueModule("A noble belief! If we possess free will, then we are not just observers of reality but active participants. Each choice creates ripples through the quantum field.");
                                            pla.SendGump(new DialogueGump(pla, freeWillBeliefModule));
                                        });
                                    freeWillModule.AddOption("It feels like fate controls us.",
                                        pla => true,
                                        pla => 
                                        {
                                            DialogueModule fateModule = new DialogueModule("Fate is a compelling concept, suggesting a predetermined path. But perhaps fate itself is just a manifestation of collective consciousness shaping the universe?");
                                            pla.SendGump(new DialogueGump(pla, fateModule));
                                        });
                                    p.SendGump(new DialogueGump(p, freeWillModule));
                                });
                            p.SendGump(new DialogueGump(p, quantumConsciousnessModule));
                        });

                    consciousnessModule.AddOption("What role does perception play?",
                        p => true,
                        p => 
                        {
                            DialogueModule perceptionModule = new DialogueModule("Perception is crucial! It acts as a filter through which we experience reality. Each individual's perception can differ vastly, leading to unique interpretations of the same event.");
                            perceptionModule.AddOption("So, can perception alter reality?",
                                pl => true,
                                pl => 
                                {
                                    DialogueModule alterRealityModule = new DialogueModule("Absolutely! If perception influences how we interpret experiences, it can shape our reactions and emotional responses, thus altering our personal reality.");
                                    pl.SendGump(new DialogueGump(pl, alterRealityModule));
                                });
                            perceptionModule.AddOption("What about shared experiences?",
                                pl => true,
                                pl => 
                                {
                                    DialogueModule sharedExperienceModule = new DialogueModule("Shared experiences can create collective perceptions, which might help to forge a communal understanding of reality. This could strengthen or challenge individual beliefs.");
                                    pl.SendGump(new DialogueGump(pl, sharedExperienceModule));
                                });
                            p.SendGump(new DialogueGump(p, perceptionModule));
                        });

                    player.SendGump(new DialogueGump(player, consciousnessModule));
                });

            greeting.AddOption("What are your thoughts on quantum entanglement?",
                player => true,
                player => 
                {
                    DialogueModule entanglementModule = new DialogueModule("Quantum entanglement challenges our understanding of space and time. It suggests that particles can become interconnected, allowing them to influence each other instantaneously, regardless of distance. Could consciousness also be entangled?");
                    entanglementModule.AddOption("How might consciousness be entangled?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule entangledConsciousnessModule = new DialogueModule("Perhaps consciousness is not a solitary experience. If we are all interconnected at the quantum level, our thoughts and intentions could resonate across the cosmos, affecting others in ways we cannot comprehend.");
                            entangledConsciousnessModule.AddOption("That's a beautiful thought.",
                                pla => true,
                                pla => 
                                {
                                    DialogueModule beautifulThoughtModule = new DialogueModule("Indeed! It inspires a sense of unity among all beings. Imagine a world where our collective consciousness strives for harmony and understanding!");
                                    pla.SendGump(new DialogueGump(pla, beautifulThoughtModule));
                                });
                            entangledConsciousnessModule.AddOption("Is this just speculation?",
                                pla => true,
                                pla => 
                                {
                                    DialogueModule speculationModule = new DialogueModule("It is speculative, but science has often begun with speculation! The key is to explore these ideas further, to expand the boundaries of our understanding.");
                                    pla.SendGump(new DialogueGump(pla, speculationModule));
                                });
                            player.SendGump(new DialogueGump(player, entangledConsciousnessModule));
                        });

                    player.SendGump(new DialogueGump(player, entanglementModule));
                });

            greeting.AddOption("What about the nature of reality?",
                player => true,
                player => 
                {
                    DialogueModule natureRealityModule = new DialogueModule("The nature of reality is a profound question. Is it a construct of our minds, a simulation, or an objective universe? The debate is ongoing, but perhaps it's a combination of all these elements.");
                    natureRealityModule.AddOption("Can reality be a simulation?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule simulationModule = new DialogueModule("The simulation hypothesis posits that our reality could be a sophisticated simulation. If so, what does that imply about consciousness and existence? Are we mere players in a cosmic game?");
                            simulationModule.AddOption("That sounds unsettling.",
                                pla => true,
                                pla => 
                                {
                                    DialogueModule unsettlingModule = new DialogueModule("Indeed, it can be disconcerting. But if we are in a simulation, our experiences and consciousness still hold value. They contribute to the greater understanding of existence.");
                                    pla.SendGump(new DialogueGump(pla, unsettlingModule));
                                });
                            simulationModule.AddOption("If true, who is the creator?",
                                pla => true,
                                pla => 
                                {
                                    DialogueModule creatorModule = new DialogueModule("Ah, the creator! This leads to philosophical questions about the nature of the creator—are they benevolent, indifferent, or a complex force of nature? Such thoughts can be mind-boggling.");
                                    pla.SendGump(new DialogueGump(pla, creatorModule));
                                });
                            player.SendGump(new DialogueGump(player, simulationModule));
                        });

                    natureRealityModule.AddOption("Could consciousness define reality?",
                        pl => true,
                        pl => 
                        {
                            DialogueModule defineRealityModule = new DialogueModule("Perhaps! If consciousness shapes perceptions, then it might be the defining factor in how reality is constructed. Our understanding of existence could be as unique as each individual.");
                            defineRealityModule.AddOption("So, we are creators of our own reality?",
                                pla => true,
                                pla => 
                                {
                                    DialogueModule creatorsRealityModule = new DialogueModule("Yes! Each thought, intention, and action contributes to the tapestry of reality we experience. It's a powerful realization, is it not?");
                                    pla.SendGump(new DialogueGump(pla, creatorsRealityModule));
                                });
                            player.SendGump(new DialogueGump(player, defineRealityModule));
                        });

                    player.SendGump(new DialogueGump(player, natureRealityModule));
                });

            greeting.AddOption("What reward do I get?",
                player => true,
                player => 
                {
                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                    {
                        player.SendMessage("I have no reward right now. Please return later.");
                    }
                    else
                    {
                        player.SendMessage("Ah, the eureka moment! Take this!");
                        player.AddToBackpack(new FishingAugmentCrystal()); // Give the reward
                        lastRewardTime = DateTime.UtcNow; // Update the timestamp
                    }
                });

            return greeting;
        }

        public ProfessorOrion(Serial serial) : base(serial) { }

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
