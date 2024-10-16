using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sun Tzu")]
    public class SunTzu : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SunTzu() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sun Tzu";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 80;
            Int = 140;
            Hits = 75;

            // Appearance
            AddItem(new Robe() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1175 });
            AddItem(new Spellbook() { Name = "Art of War" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Sun Tzu, the master of strategy.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is irrelevant, but my spirit endures.");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job' is to impart wisdom to those who seek it.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you think yourself a warrior, or merely a pawn?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then prove your worth in the grand scheme of life.");
            }
            else if (speech.Contains("strategy"))
            {
                Say("Strategy is about understanding the deeper intricacies of conflict. Would you like to know more about its principles?");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit is the essence of our being. It remains long after our physical form has faded. Do you believe in the strength of spirit?");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is not just knowing, but understanding. Seek not just knowledge but the understanding that comes with experience. Would you embark on a quest for true understanding?");
            }
            else if (speech.Contains("principles"))
            {
                Say("The principles of strategy are many, but their core revolves around deception, environment, and direct conflict. Of these, which intrigues you the most?");
            }
            else if (speech.Contains("strength"))
            {
                Say("True strength is not measured by physical might, but by the resolve of one's spirit. Show me your resolve and I shall reward your spirit.");
            }
            else if (speech.Contains("quest"))
            {
                Say("Very well. I have a task that will test not only your wits but also your determination. Succeed, and you shall receive a token of my appreciation.");
            }
            else if (speech.Contains("resolve"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Your determination is commendable. Here, take this as a sign of my respect for your spirit's strength.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SunTzu(Serial serial) : base(serial) { }

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
