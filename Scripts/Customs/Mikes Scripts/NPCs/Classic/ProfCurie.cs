using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Prof. Curie")]
    public class ProfCurie : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ProfCurie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Prof. Curie";
            Body = 0x191; // Human female body
            Str = 65;
            Dex = 60;
            Int = 130;
            Hits = 55;

            // Appearance
            AddItem(new Skirt() { Hue = 1905 });
            AddItem(new Shirt() { Hue = 1904 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new Bonnet() { Hue = 1905 });
            AddItem(new MortarPestle() { Name = "Prof. Curie's Experiments" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            SpeechHue = 0; // Default speech hue

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
            DialogueModule greeting = new DialogueModule("I am Prof. Curie, the brilliant scientist, forced to toil in this wretched place! My research delves into the wonders and dangers of radioactive substances. How can I enlighten you today?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player =>
                {
                    DialogueModule healthModule = new DialogueModule("My health is irrelevant, but this place has left me broken. Though, it's fascinating how radioactive materials can have both healing and harmful properties.");
                    player.SendGump(new DialogueGump(player, healthModule));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("I'm a brilliant scientist, cursed to waste away in obscurity. I study radioactive substances and their myriad uses in our world.");
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What can you tell me about radioactive substances?",
                player => true,
                player =>
                {
                    DialogueModule radioModule = new DialogueModule("Ah! Radioactive substances! They are not merely sources of danger, but hold remarkable potential in various fields. What would you like to know about them?");
                    radioModule.AddOption("How are they used in medicine?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule medicineModule = new DialogueModule("In medicine, radioactive isotopes are used in diagnostics and treatment. For instance, radioactive iodine is crucial for thyroid disorders. The precise application of these materials can save lives.");
                            medicineModule.AddOption("Can you give an example?",
                                p => true,
                                p =>
                                {
                                    DialogueModule exampleModule = new DialogueModule("Certainly! For example, doctors can use radioactive tracers to visualize processes in the body, aiding in the early detection of cancers.");
                                    p.SendGump(new DialogueGump(p, exampleModule));
                                });
                            medicineModule.AddOption("What are the risks?",
                                p => true,
                                p =>
                                {
                                    DialogueModule risksModule = new DialogueModule("Ah, the risks! Exposure must be controlled to avoid damaging healthy cells. It's a delicate balance, and improper use can lead to severe health issues.");
                                    p.SendGump(new DialogueGump(p, risksModule));
                                });
                            pl.SendGump(new DialogueGump(pl, medicineModule));
                        });

                    radioModule.AddOption("What about agriculture?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule agricultureModule = new DialogueModule("In agriculture, radioactive isotopes can help improve crop yields and study soil properties. They allow scientists to track nutrient uptake and optimize conditions for growth.");
                            agricultureModule.AddOption("How does that work?",
                                p => true,
                                p =>
                                {
                                    DialogueModule trackModule = new DialogueModule("By using isotopes as tracers, researchers can determine how quickly plants absorb nutrients, leading to better fertilization techniques. It's quite fascinating!");
                                    p.SendGump(new DialogueGump(p, trackModule));
                                });
                            agricultureModule.AddOption("Are there any side effects?",
                                p => true,
                                p =>
                                {
                                    DialogueModule sideEffectsModule = new DialogueModule("Yes, there are concerns. If mismanaged, radioactive substances can contaminate soil and water sources, posing risks to ecosystems and human health. Proper regulations are essential.");
                                    p.SendGump(new DialogueGump(p, sideEffectsModule));
                                });
                            pl.SendGump(new DialogueGump(pl, agricultureModule));
                        });

                    radioModule.AddOption("Can you explain their use in energy?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule energyModule = new DialogueModule("Absolutely! Radioactive materials, particularly uranium and thorium, are pivotal in nuclear power generation. They provide significant amounts of energy with a small environmental footprint.");
                            energyModule.AddOption("What are the benefits of nuclear energy?",
                                p => true,
                                p =>
                                {
                                    DialogueModule benefitsModule = new DialogueModule("Nuclear energy is efficient and can reduce dependence on fossil fuels, lowering greenhouse gas emissions. However, it requires stringent safety protocols.");
                                    p.SendGump(new DialogueGump(p, benefitsModule));
                                });
                            energyModule.AddOption("What about the risks?",
                                p => true,
                                p =>
                                {
                                    DialogueModule risksEnergyModule = new DialogueModule("The risks include potential nuclear accidents and the challenge of radioactive waste disposal. It's a field that demands responsibility and foresight.");
                                    p.SendGump(new DialogueGump(p, risksEnergyModule));
                                });
                            pl.SendGump(new DialogueGump(pl, energyModule));
                        });

                    radioModule.AddOption("What research are you currently conducting?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule researchModule = new DialogueModule("I'm currently investigating the potential of radioisotopes in extending shelf life for food preservation. There’s a promising intersection between science and everyday life!");
                            researchModule.AddOption("How would that work?",
                                p => true,
                                p =>
                                {
                                    DialogueModule shelfLifeModule = new DialogueModule("By applying controlled doses of radiation, we can eliminate bacteria and pests without altering the food's quality. It's a game changer for food safety.");
                                    p.SendGump(new DialogueGump(p, shelfLifeModule));
                                });
                            pl.SendGump(new DialogueGump(pl, researchModule));
                        });

                    player.SendGump(new DialogueGump(player, radioModule));
                });

            greeting.AddOption("How can I help you escape?",
                player => true,
                player =>
                {
                    DialogueModule escapeModule = new DialogueModule("If you can bring me the rare ingredients I need for my experiment, I might reward you for freeing me from this mundane existence.");
                    escapeModule.AddOption("What ingredients do you need?",
                        p => true,
                        p =>
                        {
                            if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                            {
                                DialogueModule noRewardModule = new DialogueModule("I have no reward right now. Please return later.");
                                p.SendGump(new DialogueGump(p, noRewardModule));
                            }
                            else
                            {
                                DialogueModule ingredientsModule = new DialogueModule("I need the Feather of a Phoenix, the Heart of a Gorgon, and the Tear of a Siren. Bring them to me, and you will be handsomely rewarded.");
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                                p.SendGump(new DialogueGump(p, ingredientsModule));
                                p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                            }
                        });
                    player.SendGump(new DialogueGump(player, escapeModule));
                });

            greeting.AddOption("What are your discoveries?",
                player => true,
                player =>
                {
                    DialogueModule discoveriesModule = new DialogueModule("My discoveries range from the elixir of life to the power of manipulating time. But alas, they are stored in my lost journals.");
                    player.SendGump(new DialogueGump(player, discoveriesModule));
                });

            greeting.AddOption("What experiments have you conducted?",
                player => true,
                player =>
                {
                    DialogueModule experimentsModule = new DialogueModule("I once experimented with merging magic and science. The results were... unpredictable. Yet, I believe I was close to perfection.");
                    player.SendGump(new DialogueGump(player, experimentsModule));
                });

            greeting.AddOption("What is your ultimate experiment?",
                player => true,
                player =>
                {
                    DialogueModule ultimateExperimentModule = new DialogueModule("Ah! My ultimate experiment was to create a portal to another dimension. But, the components are scattered across the land.");
                    player.SendGump(new DialogueGump(player, ultimateExperimentModule));
                });

            greeting.AddOption("What tools do you need?",
                player => true,
                player =>
                {
                    DialogueModule toolsModule = new DialogueModule("My most important tool was the Crystal of Foresight. It was stolen from me. Retrieve it, and I might bestow a reward upon you.");
                    player.SendGump(new DialogueGump(player, toolsModule));
                });

            return greeting;
        }

        public ProfCurie(Serial serial) : base(serial) { }

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
