using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Master Zalazar")]
    public class MasterZalazar : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MasterZalazar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Master Zalazar";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 40;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 1153 });
            AddItem(new Sandals() { Hue = 1153 });
            AddItem(new WizardsHat() { Hue = 1153 });
            AddItem(new MortarPestle() { Name = "Zalazar's Mortar and Pestle" });

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
                Say("I am Master Zalazar, a seeker of wisdom!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, both in body and mind.");
            }
            else if (speech.Contains("job"))
            {
                Say("My purpose is to seek knowledge and understanding.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("True wisdom is found in the pursuit of virtues. Are you familiar with the virtues of Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility?");
            }
            else if (speech.Contains("yes"))
            {
                Say("The virtues are the foundation of a noble life. Seek to embody them in all your actions.");
            }
            else if (speech.Contains("mind"))
            {
                Say("The mind is a vast expanse that many neglect. Meditation has helped me unlock its potential. Do you meditate?");
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is a journey, not a destination. My latest pursuit involves the mysterious ruins in the East. Have you ventured there?");
            }
            else if (speech.Contains("honor"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Honor is a virtue not easily defined. It means standing true to oneself and to others, even in the face of adversity. I have a small token for those who show true honor. Would you like it?");
                    from.AddToBackpack(new BlacksmithyAugmentCrystal()); // Reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("sacrifice"))
            {
                Say("Sacrifice is the virtue of giving without expecting in return. Long ago, I sacrificed much to protect a sacred artifact. Have you heard tales of this artifact?");
            }
            else if (speech.Contains("realm"))
            {
                Say("The hidden realm is said to be a place where the physical and spiritual worlds merge. Few have been there and returned to tell the tale. If you seek it, be prepared for challenges.");
            }
            else if (speech.Contains("meditate"))
            {
                Say("Meditation is a practice that calms the soul and sharpens the mind. In my meditations, I have seen visions of a distant mountain. Have you climbed such peaks?");
            }
            else if (speech.Contains("ruins"))
            {
                Say("The ruins to the East are remnants of an ancient civilization. They hold secrets and treasures. But beware, for they are also guarded by creatures of old.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the light that pierces the darkness of ignorance. Through my studies, I've come across ancient texts that speak of a hidden realm. Have you heard of it?");
            }

            base.OnSpeech(e);
        }

        public MasterZalazar(Serial serial) : base(serial) { }

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
