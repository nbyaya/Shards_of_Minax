using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jester Jangle")]
    public class JesterJangle : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public JesterJangle() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jester Jangle";
            Body = 0x191; // Human male body

            // Stats
            Str = 130;
            Hits = 75;

            // Appearance
            AddItem(new JesterHat() { Hue = 2150 }); // Jester hat with hue 2150
            AddItem(new JesterSuit() { Hue = 1287 }); // Jester suit with hue 1287
            AddItem(new Boots() { Hue = 2105 }); // Boots with hue 2105

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Welcome, traveler! I am Jester Jangle, the mirthful one.");
            }
            else if (speech.Contains("job"))
            {
                Say("I bring joy to these lands, but I also ponder the eight virtues.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
            }
            else if (speech.Contains("interactions"))
            {
                Say("The virtues intertwine, much like a dance. Can you decipher their interactions?");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Let me pose a riddle. In a world without justice, can there be true honor?");
            }
            else if (speech.Contains("balance"))
            {
                Say("Consider this and seek the balance of the virtues, noble traveler.");
            }
            else if (speech.Contains("mirthful"))
            {
                Say("Ah, being mirthful is my essence. In these dark times, a laugh or two can light up the world.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am well, thank you. The energy of the virtues keeps me spry and lively!");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy is what I spread, and joy is what I feel when I see the virtues being practiced.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is not just in battles, but in everyday decisions. It's choosing the right path even when no one is watching.");
            }
            else if (speech.Contains("dance"))
            {
                Say("Aye, the dance of the virtues. Like steps in a dance, they depend on each other to create harmony. Do you dance, traveler?");
            }
            else if (speech.Contains("world"))
            {
                Say("The world we live in is a reflection of the choices we make. Some choose the path of virtue, others the opposite. But remember, it's never too late to turn around.");
            }
            else if (speech.Contains("seek"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To truly seek the balance is to accept the challenge of walking the tightrope of the virtues. Do so, and I'll grant you a small token of appreciation.");
                    from.AddToBackpack(new FatigueAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("dark"))
            {
                Say("The dark times I speak of are filled with challenges. But they also offer opportunities for heroes to rise.");
            }
            else if (speech.Contains("energy"))
            {
                Say("The energy I speak of is not just physical but spiritual. The virtues fuel this spirit.");
            }
            else if (speech.Contains("practiced"))
            {
                Say("To practice the virtues is a lifelong commitment. But the journey, oh, it's worth every step!");
            }

            base.OnSpeech(e);
        }

        public JesterJangle(Serial serial) : base(serial) { }

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
