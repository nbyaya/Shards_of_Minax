using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Marie Curie")]
    public class MarieCurie : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MarieCurie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Marie Curie";
            Body = 0x191; // Human female body

            // Stats
            Str = 60;
            Dex = 60;
            Int = 120;
            Hits = 50;

            // Appearance
            AddItem(new Robe() { Hue = 2301 }); // Robe with specific hue
            AddItem(new Sandals() { Hue = 2301 }); // Sandals with specific hue
            AddItem(new GnarledStaff() { Name = "Marie's Staff" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Oh, it's you again. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? What's it to you? I'm not your personal physician.");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job\"? I'm stuck here answering your silly questions. That's my job, I suppose.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valiant? Hah! Do I look valiant to you?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, you're valiant, huh? Prove it! Tell me, what's the most valuable thing in the world?");
            }
            else if (speech.Contains("curie"))
            {
                Say("Marie Curie, that's who I am. I hail from a place called France. Ever heard of it?");
            }
            else if (speech.Contains("personal"))
            {
                Say("Your concern is unexpected. While I may seem grumpy, it's probably from the lack of radium around here.");
            }
            else if (speech.Contains("answering"))
            {
                Say("Ah, I jest. In another life, I was a pioneering scientist. My work revolved around radioactivity.");
            }
            else if (speech.Contains("france"))
            {
                Say("France, the land of wine and wonders. If you ever visit, remember to bask in its rich history. You might just learn something.");
            }
            else if (speech.Contains("radium"))
            {
                Say("Radium, a fascinating element I once researched. It's elusive and dangerous. I could give you a sample, but only if you promise to handle it with care.");
            }
            else if (speech.Contains("scientist"))
            {
                Say("Yes, a scientist. My research has brought both admiration and challenges. Have you ever been passionate about something?");
            }
            else if (speech.Contains("promise"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Good. Here is a small sample of radium. Remember, it's not a toy. Handle it with extreme care.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("passionate"))
            {
                Say("Passion drives us to achieve great things. It's what propelled me in my studies. Do you have any aspirations?");
            }
            else if (speech.Contains("care"))
            {
                Say("Yes, care. Too many treat things lightly, without understanding their power or significance. Promise me you won't be one of them.");
            }
            else if (speech.Contains("aspirations"))
            {
                Say("Everyone should have something they aspire to. It's what gives life meaning and purpose. What's yours?");
            }

            base.OnSpeech(e);
        }

        public MarieCurie(Serial serial) : base(serial) { }

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
