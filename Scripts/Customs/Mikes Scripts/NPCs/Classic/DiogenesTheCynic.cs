using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Diogenes the Cynic")]
    public class DiogenesTheCynic : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DiogenesTheCynic() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Diogenes the Cynic";
            Body = 0x190; // Human male body

            // Stats
            Str = 75;
            Dex = 45;
            Int = 115;
            Hits = 63;

            // Appearance
            AddItem(new Robe(2210));
            AddItem(new Boots(1179));
            AddItem(new Lantern { Name = "Diogenes' Lamp" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
                Say("I am Diogenes the Cynic, a philosopher!");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? What does it matter in this wretched world?");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' is to contemplate the absurdity of existence. A noble pursuit in a world of fools.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Do you seek wisdom in this miserable existence?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Wisdom requires questioning everything, even your own existence. Are you prepared to do that?");
            }
            else if (speech.Contains("philosopher"))
            {
                Say("I seek to understand the human condition, its virtues, and its vices. Such is the way of the philosopher.");
            }
            else if (speech.Contains("wretched"))
            {
                Say("This world is full of suffering and desires. One can find peace only by relinquishing these desires.");
            }
            else if (speech.Contains("absurdity"))
            {
                Say("Life is full of contradictions and inexplicable phenomena. The more we understand its absurdity, the closer we are to true wisdom.");
            }
            else if (speech.Contains("miserable"))
            {
                Say("Ah, the misery of existence is but a perspective. One might see it as an opportunity to rise above, to find meaning in the chaos.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtues are what guide us in life. Honesty, compassion, valor, and justice. Do you uphold these in your life?");
            }
            else if (speech.Contains("peace"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To achieve true peace, one must meditate and reflect upon one's actions and thoughts. For your genuine interest, I bestow upon you this small token of appreciation. Use it well on your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with actual item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("contradictions"))
            {
                Say("Life is a series of opposing forces. Light and dark, love and hate. It is in understanding these contradictions that one finds balance.");
            }
            else if (speech.Contains("meaning"))
            {
                Say("Each person must find their own meaning in this vast universe. What is meaningless to one might be the entire world to another.");
            }
            else if (speech.Contains("honesty"))
            {
                Say("Honesty is the foundation of all virtues. Without it, one's character crumbles. It is more than just telling the truth; it is living it.");
            }

            base.OnSpeech(e);
        }

        public DiogenesTheCynic(Serial serial) : base(serial) { }

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
