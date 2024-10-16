using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Morgana")]
    public class LadyMorgana : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyMorgana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Morgana";
            Body = 0x191; // Human female body

            // Stats
            Str = 158;
            Dex = 70;
            Int = 32;
            Hits = 115;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1160 });
            AddItem(new NorseHelm() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1160 });
            AddItem(new ThighBoots() { Hue = 1160 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public LadyMorgana(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lady Morgana, a knight of Valor!");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("My health is robust, thank you for asking! I train daily to maintain my strength.")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player =>
                {
                    DialogueModule jobModule = new DialogueModule("My duty is to uphold the virtue of Valor by defending the weak and standing against injustice.");
                    jobModule.AddOption("What kind of injustices do you fight?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule injusticeModule = new DialogueModule("Many injustices plague our lands: tyrants oppressing the innocent, bandits raiding peaceful villages, and even dark sorcery that corrupts the hearts of men.");
                            injusticeModule.AddOption("Have you encountered any specific tyrants?",
                                p => true,
                                p => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Indeed, I've faced many. One such tyrant was Lord Blackthorn, who sought to control all with fear and intimidation.")));
                                });
                            pl.SendGump(new DialogueGump(pl, injusticeModule));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What about battles?",
                player => true,
                player =>
                {
                    DialogueModule battlesModule = new DialogueModule("Valor is the strength to face danger with courage and resolve. Do you consider yourself valiant?");
                    battlesModule.AddOption("Yes, I am valiant!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule responseModule = new DialogueModule("Indeed, facing adversity with courage is the path to true valor. How do you exhibit your courage in your daily life?");
                            responseModule.AddOption("I stand up for those in need.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("That is truly noble! Every act of bravery, no matter how small, contributes to a better world."))));
                            responseModule.AddOption("I fight against evil.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("A true warrior's spirit! It is essential to confront evil wherever it arises."))));
                            responseModule.AddOption("I seek knowledge.",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("Knowledge is a powerful ally. With wisdom, we can find the best paths in life."))));
                            pl.SendGump(new DialogueGump(pl, responseModule));
                        });
                    player.SendGump(new DialogueGump(player, battlesModule));
                });

            greeting.AddOption("Are you a knight?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Yes, I am a knight trained in the arts of combat and chivalry. I've journeyed far and wide, facing numerous challenges to uphold my oath.")));
                });

            greeting.AddOption("What do you think about injustice?",
                player => true,
                player =>
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("Injustice is a bane on our lands. I strive to correct their wrongdoings and protect the innocent.")));
                });

            greeting.AddOption("What do you think about decisions?",
                player => true,
                player =>
                {
                    DialogueModule decisionModule = new DialogueModule("Every decision I make is guided by the virtues I uphold. Sometimes, it means making sacrifices for the greater good.");
                    decisionModule.AddOption("Can you give me an example?",
                        pl => true,
                        pl =>
                        {
                            pl.SendGump(new DialogueGump(pl, new DialogueModule("Once, I had to choose between saving a village from marauders or pursuing a dark sorcerer. I chose the village, knowing it would save many lives.")));
                        });
                    decisionModule.AddOption("What about rewards?",
                        pl => true,
                        pl =>
                        {
                            TimeSpan cooldown = TimeSpan.FromMinutes(10);
                            if (DateTime.UtcNow - lastRewardTime < cooldown)
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("I have no reward right now. Please return later.")));
                            }
                            else
                            {
                                pl.SendGump(new DialogueGump(pl, new DialogueModule("Ah, you are interested in the reward! Here, take this. It's a token of appreciation for those who seek knowledge.")));
                                pl.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                lastRewardTime = DateTime.UtcNow; // Update the timestamp
                            }
                        });
                    player.SendGump(new DialogueGump(player, decisionModule));
                });

            greeting.AddOption("Tell me about your training.",
                player => true,
                player =>
                {
                    DialogueModule trainingModule = new DialogueModule("Training is rigorous and requires dedication. I practice combat skills daily and study the code of chivalry.");
                    trainingModule.AddOption("What skills do you focus on?",
                        pl => true,
                        pl =>
                        {
                            DialogueModule skillsModule = new DialogueModule("I focus on swordsmanship, archery, and strategy. Each skill is essential for a knight.");
                            skillsModule.AddOption("Do you have a favorite weapon?",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("I favor the longsword; its balance and reach are perfect for both offense and defense."))));
                            skillsModule.AddOption("How do you improve your strategy?",
                                p => true,
                                p => p.SendGump(new DialogueGump(p, new DialogueModule("I study past battles and engage in mock duels to sharpen my tactical thinking."))));
                            pl.SendGump(new DialogueGump(pl, skillsModule));
                        });
                    player.SendGump(new DialogueGump(player, trainingModule));
                });

            return greeting;
        }

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
