using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Guinevere")]
    public class LadyGuinevere : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyGuinevere() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Guinevere";
            Body = 0x191; // Human female body

            // Stats
            SetStr(100);
            SetDex(100);
            SetInt(60);
            SetHits(70);

            // Appearance
            AddItem(new ChainChest() { Hue = 1908 });
            AddItem(new ChainLegs() { Hue = 1908 });
            AddItem(new ChainCoif() { Hue = 1908 });
            AddItem(new PlateGloves() { Hue = 1908 });
            AddItem(new Boots() { Hue = 1908 });
            AddItem(new Broadsword() { Name = "Lady Guinevere's Sword" });

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
            DialogueModule greeting = new DialogueModule("Greetings, traveler. I am Lady Guinevere. How may I assist you today?");
            
            greeting.AddOption("Tell me about your health.",
                player => true,
                player => 
                {
                    player.SendGump(new DialogueGump(player, new DialogueModule("I am in good health, thank you for asking. The beauty of this land sustains my spirit.")));
                });

            greeting.AddOption("What is your job?",
                player => true,
                player => 
                {
                    DialogueModule jobModule = new DialogueModule("My duty is to uphold the virtue of Justice in these lands. I strive to ensure fairness and righteousness.");
                    jobModule.AddOption("What does justice mean to you?",
                        playerq => true,
                        playerq => 
                        {
                            DialogueModule justiceMeaning = new DialogueModule("Justice is the balance between punishment and mercy. It requires understanding the circumstances behind each action.");
                            justiceMeaning.AddOption("Can you give an example?",
                                playerw => true,
                                playerw => 
                                {
                                    player.SendGump(new DialogueGump(player, new DialogueModule("Consider two thieves: one stole to survive, while the other did it out of greed. Should they face the same punishment?")));
                                });
                            player.SendGump(new DialogueGump(player, justiceMeaning));
                        });
                    player.SendGump(new DialogueGump(player, jobModule));
                });

            greeting.AddOption("What is virtue?",
                player => true,
                player => 
                {
                    DialogueModule virtueModule = new DialogueModule("Virtue is the moral excellence that guides our actions. It is the light that leads us through darkness.");
                    virtueModule.AddOption("How can one cultivate virtue?",
                        playere => true,
                        playere => 
                        {
                            DialogueModule cultivationModule = new DialogueModule("To cultivate virtue, one must practice kindness, honesty, and humility. Every small act of goodness contributes to a virtuous life.");
                            player.SendGump(new DialogueGump(player, cultivationModule));
                        });
                    player.SendGump(new DialogueGump(player, virtueModule));
                });

            greeting.AddOption("Do you believe in justice?",
                player => true,
                player => 
                {
                    DialogueModule justiceModule = new DialogueModule("Justice is more than just a concept; it's a way of life. For those who understand its depth, I sometimes bestow a token of appreciation. Would you like to be tested on your understanding of Justice?");
                    
                    justiceModule.AddOption("Yes, please test me.",
                        p => true,
                        p => 
                        {
                            DialogueModule testModule = new DialogueModule("Very well. Answer this: In a village, two men committed a crime. One did it out of greed, the other out of desperation to feed his starving family. Should their punishments be the same or different?");
                            
                            testModule.AddOption("Different.",
                                pr => 
                                {
                                    TimeSpan cooldown = TimeSpan.FromMinutes(10);
                                    if (DateTime.UtcNow - lastRewardTime < cooldown)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        lastRewardTime = DateTime.UtcNow;
                                        return true;
                                    }
                                },
                                pt =>
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Wise answer. Recognizing the nuance is crucial for true Justice. For your wisdom, I bestow upon you a reward. May it serve you well in your journey.") { }));
                                    p.AddToBackpack(new MaxxiaScroll()); // Give the reward
                                });

                            testModule.AddOption("Same.",
                                py => true,
                                py => 
                                {
                                    p.SendGump(new DialogueGump(p, new DialogueModule("Justice requires understanding the nuances of each situation. Think carefully about your answer next time.")));
                                });

                            p.SendGump(new DialogueGump(p, testModule));
                        });

                    justiceModule.AddOption("Maybe later.",
                        p => true,
                        p => 
                        {
                            p.SendGump(new DialogueGump(p, CreateGreetingModule()));
                        });

                    player.SendGump(new DialogueGump(player, justiceModule));
                });

            greeting.AddOption("Tell me about your lineage.",
                player => true,
                player => 
                {
                    DialogueModule lineageModule = new DialogueModule("I hail from the ancient lineage of Avalon, and my name carries the weight of my ancestors' legacy.");
                    lineageModule.AddOption("What is Avalon?",
                        playeru => true,
                        playeru => 
                        {
                            player.SendGump(new DialogueGump(player, new DialogueModule("Avalon is a mythical island, a place of peace and wisdom, where the noblest virtues are revered. Legends say it is hidden from the eyes of the unworthy.")));
                        });
                    player.SendGump(new DialogueGump(player, lineageModule));
                });

            greeting.AddOption("What are your thoughts on the world?",
                player => true,
                player => 
                {
                    DialogueModule worldThoughts = new DialogueModule("The world is a tapestry of light and shadow, woven together by the actions of its inhabitants. It can be both beautiful and cruel.");
                    worldThoughts.AddOption("How can we improve the world?",
                        playeri => true,
                        playeri => 
                        {
                            DialogueModule improvementModule = new DialogueModule("We can improve the world through acts of kindness, justice, and understanding. Each person has the power to make a difference.");
                            player.SendGump(new DialogueGump(player, improvementModule));
                        });
                    player.SendGump(new DialogueGump(player, worldThoughts));
                });

            return greeting;
        }

        public LadyGuinevere(Serial serial) : base(serial) { }

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
