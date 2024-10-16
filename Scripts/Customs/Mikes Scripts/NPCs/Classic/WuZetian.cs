using System;
using Server;
using Server.Mobiles;
using Server.Gumps;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Wu Zetian")]
    public class WuZetian : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public WuZetian() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Wu Zetian";
            Body = 0x191; // Human female body

            // Stats
            SetStr(100);
            SetDex(90);
            SetInt(90);
            SetHits(80);

            // Appearance
            AddItem(new FancyDress() { Hue = 1102 }); // Clothing item with hue 1102
            AddItem(new OrcMask() { Hue = 1102 }); // Mask with hue 1102
            AddItem(new Sandals() { Hue = 1102 }); // Sandals with hue 1102

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public WuZetian(Serial serial) : base(serial) { }

        public override void OnDoubleClick(Mobile from)
        {
            if (!(from is PlayerMobile player))
                return;

            DialogueModule greetingModule = CreateGreetingModule();
            player.SendGump(new DialogueGump(player, greetingModule));
        }

        private DialogueModule CreateGreetingModule()
        {
            DialogueModule greeting = new DialogueModule("I am Wu Zetian, Empress of China! What brings you here?");

            greeting.AddOption("Tell me about your health.",
                player => true,
                player => { Say("My health is none of your business!"); });

            greeting.AddOption("What is your job?",
                player => true,
                player => { Say("My 'job' is to sit here and entertain ignorant wanderers like you!"); });

            greeting.AddOption("What challenges do you speak of?",
                player => true,
                player =>
                {
                    DialogueModule challengeModule = new DialogueModule("Do you think you can handle the challenges of this world, or are you just another fool?");
                    challengeModule.AddOption("I am ready for a challenge!",
                        pl => true,
                        pl =>
                        {
                            DialogueModule proveModule = new DialogueModule("Well, let's see if you can prove me wrong. Are you up for the challenge? Choose wisely!");
                            proveModule.AddOption("I accept any challenge!",
                                p => true,
                                p => { Say("Very well! Your first task is to bring me the rare Phoenix Feather."); });

                            proveModule.AddOption("Maybe later.",
                                p => true,
                                p => { Say("Remember, challenges await those who seek them!"); });

                            pl.SendGump(new DialogueGump(pl, proveModule));
                        });

                    challengeModule.AddOption("I prefer to avoid challenges.",
                        pl => true,
                        pl => { Say("Then perhaps you should leave these matters to the brave."); });

                    player.SendGump(new DialogueGump(player, challengeModule));
                });

            greeting.AddOption("Tell me about your reign.",
                player => true,
                player => 
                {
                    DialogueModule reignModule = new DialogueModule("The history of my reign is filled with both intrigue and innovation. I have faced many adversities.");
                    reignModule.AddOption("What adversities?",
                        pl => true,
                        pl => { Say("From internal strife to external threats, I have navigated the tides of fortune with wisdom."); });

                    reignModule.AddOption("What innovations?",
                        pl => true,
                        pl => { Say("I have introduced reforms in governance, promoting culture and science throughout my empire."); });

                    player.SendGump(new DialogueGump(player, reignModule));
                });

            greeting.AddOption("Can you give me a reward?",
                player => DateTime.UtcNow - lastRewardTime >= TimeSpan.FromMinutes(10),
                player =>
                {
                    Say("Ensuring prosperity is a challenge. Here, take this.");
                    player.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                });

            greeting.AddOption("What do you mean by entertainment?",
                player => true,
                player =>
                {
                    DialogueModule entertainmentModule = new DialogueModule("Entertainment is but a small facet of my duties. Ruling a vast empire is my true task.");
                    entertainmentModule.AddOption("What forms of entertainment do you prefer?",
                        pl => true,
                        pl => { Say("I enjoy poetry, music, and theater. Each art form reflects the spirit of my people."); });

                    entertainmentModule.AddOption("Can you tell me a story?",
                        pl => true,
                        pl =>
                        {
                            Say("Once, a humble farmer became a hero by saving a village from a dragon. His courage inspired many!");
                            pl.SendGump(new DialogueGump(pl, entertainmentModule)); // Return to the module
                        });

                    player.SendGump(new DialogueGump(player, entertainmentModule));
                });

            greeting.AddOption("What do you think of your subjects?",
                player => true,
                player =>
                {
                    DialogueModule subjectsModule = new DialogueModule("My subjects are my pride and joy. Each has a story to tell and a contribution to make.");
                    subjectsModule.AddOption("Tell me more about their stories.",
                        pl => true,
                        pl => { Say("Each villager, soldier, and merchant adds a unique thread to the tapestry of my empire."); });

                    subjectsModule.AddOption("Do they ever disappoint you?",
                        pl => true,
                        pl =>
                        {
                            Say("At times, yes. But I believe in their potential. Disappointment is a part of growth.");
                            pl.SendGump(new DialogueGump(pl, subjectsModule)); // Return to the module
                        });

                    player.SendGump(new DialogueGump(player, subjectsModule));
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
