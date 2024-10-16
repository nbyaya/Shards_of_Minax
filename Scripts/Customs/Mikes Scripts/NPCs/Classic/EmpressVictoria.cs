using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Empress Victoria")]
    public class EmpressVictoria : BaseCreature
    {
        [Constructable]
        public EmpressVictoria() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Empress Victoria";
            Body = 0x191; // Human female body
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            SetStr(80);
            SetDex(80);
            SetInt(80);
            SetHits(80);

            // Appearance
            AddItem(new FancyDress() { Hue = 2118 });
            AddItem(new Boots() { Hue = 2118 });
            AddItem(new GoldNecklace() { Name = "Victoria's Necklace" });
            AddItem(new Cloak() { Name = "Victoria's Cloak" });
            AddItem(new Mace() { Name = "Victoria's Fan" });

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public EmpressVictoria(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Empress Victoria, proud ruler of this empire. How may I assist you today?");

            greeting.AddOption("Tell me about your empire.",
                player => true,
                player =>
                {
                    DialogueModule empireModule = new DialogueModule("Our empire stands as a beacon of civilization in a world often shadowed by chaos. We strive to bring culture, knowledge, and prosperity to all corners of the land. Would you like to know more about our achievements or our challenges?");
                    empireModule.AddOption("Tell me about your achievements.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule achievementsModule = new DialogueModule("We have built great cities with magnificent architecture, established schools for the arts and sciences, and created a justice system that serves to protect all citizens. The roads we lay connect towns, fostering trade and unity among our people. Which achievement interests you most?");
                            achievementsModule.AddOption("The cities you have built.",
                                p => true,
                                p =>
                                {
                                    DialogueModule citiesModule = new DialogueModule("Our cities are marvels of design, featuring parks, libraries, and marketplaces. Each is a hub of culture where artists and scholars flourish. For example, in Eldoria, the Grand Library is said to house tomes of knowledge from across the realm.");
                                    citiesModule.AddOption("What else is in Eldoria?",
                                        plq => true,
                                        plq =>
                                        {
                                            DialogueModule moreCitiesModule = new DialogueModule("Eldoria also hosts the Festival of Lights, a celebration that draws visitors from afar to witness art, music, and performances that highlight our rich cultural heritage.");
                                            moreCitiesModule.AddOption("That sounds enchanting!",
                                                pw => true,
                                                pw =>
                                                {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            pl.SendGump(new DialogueGump(pl, moreCitiesModule));
                                        });
                                    citiesModule.AddOption("What challenges do you face?",
                                        ple => true,
                                        ple =>
                                        {
                                            DialogueModule challengesModule = new DialogueModule("As we build, we encounter resistance from those who fear change. Some prefer the old ways and resist the prosperity we bring. Yet, we strive to demonstrate that progress benefits all.");
                                            challengesModule.AddOption("How do you handle resistance?",
                                                pr => true,
                                                pr =>
                                                {
                                                    DialogueModule handlingResistanceModule = new DialogueModule("We engage in dialogue, listening to their concerns, and find common ground. Many have come to understand that our intentions are for the greater good.");
                                                    handlingResistanceModule.AddOption("That seems wise.",
                                                        plt => true,
                                                        plt =>
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    p.SendGump(new DialogueGump(p, handlingResistanceModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, challengesModule));
                                        });
                                    player.SendGump(new DialogueGump(player, citiesModule));
                                });

                            achievementsModule.AddOption("The schools you have established.",
                                p => true,
                                p =>
                                {
                                    DialogueModule schoolsModule = new DialogueModule("Our schools have become centers of knowledge, promoting literacy and the arts. We have trained many gifted scholars who have contributed to advancements in magic, technology, and philosophy.");
                                    schoolsModule.AddOption("What subjects are taught?",
                                        ply => true,
                                        ply =>
                                        {
                                            DialogueModule subjectsModule = new DialogueModule("Students learn a variety of subjects, including mathematics, alchemy, history, and the magical arts. Each discipline fosters creativity and critical thinking.");
                                            subjectsModule.AddOption("What about alchemy?",
                                                pu => true,
                                                pu =>
                                                {
                                                    DialogueModule alchemyModule = new DialogueModule("Alchemy is highly regarded in our curriculum. It combines science and art, teaching students to harness the properties of nature to create potions and remedies.");
                                                    alchemyModule.AddOption("What are the most important ingredients?",
                                                        pli => true,
                                                        pli =>
                                                        {
                                                            DialogueModule ingredientsModule = new DialogueModule("Some of the most sought-after ingredients include Moonlit Petals and Dragon's Breath. Their rarity makes them highly valuable and integral to advanced alchemical practices.");
                                                            ingredientsModule.AddOption("Fascinating! Do you have any recommendations for aspiring alchemists?",
                                                                po => true,
                                                                po =>
                                                                {
                                                                    DialogueModule adviceModule = new DialogueModule("Aspiring alchemists should practice patience and keep detailed notes on their experiments. Mistakes can lead to valuable discoveries!");
                                                                    adviceModule.AddOption("Thank you for the advice!",
                                                                        plp => true,
                                                                        plp =>
                                                                        {
                                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                                        });
                                                                    p.SendGump(new DialogueGump(p, adviceModule));
                                                                });
                                                            pl.SendGump(new DialogueGump(pl, ingredientsModule));
                                                        });
                                                    p.SendGump(new DialogueGump(p, alchemyModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, subjectsModule));
                                        });
                                    player.SendGump(new DialogueGump(player, schoolsModule));
                                });

                            achievementsModule.AddOption("The justice system you have created.",
                                p => true,
                                p =>
                                {
                                    DialogueModule justiceModule = new DialogueModule("Our justice system is built on principles of fairness and equality. It ensures that every citizen has a voice, and it strives to resolve disputes amicably.");
                                    justiceModule.AddOption("How do you maintain fairness?",
                                        pla => true,
                                        pla =>
                                        {
                                            DialogueModule fairnessModule = new DialogueModule("Judges are trained to remain impartial, and we conduct regular reviews of our laws to ensure they serve the needs of the community.");
                                            fairnessModule.AddOption("What happens if someone is wrongly accused?",
                                                ps => true,
                                                ps =>
                                                {
                                                    DialogueModule wrongAccusationModule = new DialogueModule("If someone is wrongfully accused, we have a process for appeals and investigations. Justice must be upheld, and we must correct any mistakes.");
                                                    wrongAccusationModule.AddOption("That is a comforting system.",
                                                        pld => true,
                                                        pld =>
                                                        {
                                                            pl.SendGump(new DialogueGump(pl, CreateGreetingModule()));
                                                        });
                                                    p.SendGump(new DialogueGump(p, wrongAccusationModule));
                                                });
                                            pl.SendGump(new DialogueGump(pl, fairnessModule));
                                        });
                                    player.SendGump(new DialogueGump(player, justiceModule));
                                });

                            player.SendGump(new DialogueGump(player, achievementsModule));
                        });

                    empireModule.AddOption("Tell me about your challenges.",
                        pl => true,
                        pl =>
                        {
                            DialogueModule challengesModule = new DialogueModule("Every empire faces challenges. We encounter resistance from those who fear the changes we bring. Yet, we stand firm in our mission to spread civilization and knowledge.");
                            challengesModule.AddOption("What kind of resistance?",
                                p => true,
                                p =>
                                {
                                    DialogueModule resistanceModule = new DialogueModule("Some factions prefer the chaos of the past, rejecting the order we establish. They spread rumors and incite fear among the populace.");
                                    resistanceModule.AddOption("How do you counter these factions?",
                                        plf => true,
                                        plf =>
                                        {
                                            DialogueModule counterModule = new DialogueModule("We counter them through education and diplomacy. We invite them to our discussions and showcase the benefits of our governance.");
                                            counterModule.AddOption("It sounds like a difficult task.",
                                                pg => true,
                                                pg =>
                                                {
                                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                                });
                                            pl.SendGump(new DialogueGump(pl, counterModule));
                                        });
                                    p.SendGump(new DialogueGump(p, resistanceModule));
                                });
                            player.SendGump(new DialogueGump(player, challengesModule));
                        });

                    player.SendGump(new DialogueGump(player, empireModule));
                });

            greeting.AddOption("How is your health?",
                player => true,
                player =>
                {
                    player.SendMessage("I am in good health, thank you for asking. My duties keep me invigorated and engaged with my people.");
                });

            greeting.AddOption("What do you value most about your rule?",
                player => true,
                player =>
                {
                    DialogueModule valuesModule = new DialogueModule("I value integrity, compassion, and progress. It is crucial that my reign brings about positive change and ensures that my people live in harmony.");
                    valuesModule.AddOption("How do you ensure harmony?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule harmonyModule = new DialogueModule("Harmony comes from understanding and respect. I encourage open dialogue among my advisors and the citizens to address their needs and concerns.");
                            harmonyModule.AddOption("That sounds effective.",
                                p => true,
                                p =>
                                {
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, harmonyModule));
                        });
                    player.SendGump(new DialogueGump(player, valuesModule));
                });

            greeting.AddOption("What are your hopes for the future of the empire?",
                player => true,
                player =>
                {
                    DialogueModule futureModule = new DialogueModule("My hopes are to expand our influence, spread our values of civilization, and ensure that every citizen has access to education and resources. Together, we can forge a brighter future.");
                    futureModule.AddOption("How can I help?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule helpModule = new DialogueModule("Your desire to help is commendable. You can assist by sharing our message, supporting your community, and standing against those who oppose progress.");
                            helpModule.AddOption("I will do my best!",
                                p => true,
                                p =>
                                {
                                    p.SendMessage("Empress Victoria nods appreciatively at your commitment.");
                                    p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                                });
                            pl.SendGump(new DialogueGump(pl, helpModule));
                        });
                    player.SendGump(new DialogueGump(player, futureModule));
                });

            greeting.AddOption("Goodbye.",
                player => true,
                player =>
                {
                    player.SendMessage("Farewell, traveler. May your path be safe, and may you carry the light of civilization wherever you go.");
                });

            return greeting;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
