using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Slamson the Bold")]
    public class SlamsonTheBold : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SlamsonTheBold() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Slamson the Bold";
            Body = 0x190; // Human male body

            // Stats
            Str = 105;
            Dex = 70;
            Int = 70;
            Hits = 105;

            // Appearance
            AddItem(new BodySash() { Hue = 1154 });
            AddItem(new Skirt() { Hue = 1154 });
            AddItem(new Boots() { Hue = 1175 });
            AddItem(new LeatherGloves() { Name = "Slamson's Gloves" });

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
                Say("I am Slamson the Bold, the champion of the wrestling ring!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am as fit as a fiddle!");
            }
            else if (speech.Contains("job"))
            {
                Say("I challenge opponents in the wrestling ring, testing both strength and honor!");
            }
            else if (speech.Contains("matches"))
            {
                Say("Honor is what separates brutes from true warriors in the ring. Do you understand the meaning of honor in combat?");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is not just about respecting your opponent but understanding oneself. It's a beacon that guides one's spirit. Have you ever faced a challenge where your honor was tested?");
            }
            else if (speech.Contains("training"))
            {
                Say("Training for the ring is more than just physical. It's a spiritual journey, where one learns to sacrifice and to uphold the virtues. Every scar is a lesson in humility.");
            }
            else if (speech.Contains("slamson"))
            {
                Say("Ah, you've heard of me, have you? Many have watched me in awe as I toppled champions in the ring. Do you wish to become a wrestler too?");
            }
            else if (speech.Contains("fiddle"))
            {
                Say("It's not just about being fit. It's about daily rituals and a diet that fuels the body and mind. Have you ever tried the warrior's brew?");
            }
            else if (speech.Contains("strength"))
            {
                Say("True strength comes not just from the body but from the mind. It's the willpower to keep going even when you're on the edge. Have you experienced such moments?");
            }
            else if (speech.Contains("warriors"))
            {
                Say("True warriors don't just fight; they inspire, lead, and set examples. They are revered not for their might but for their principles. Have you met such revered individuals on your journey?");
            }
            else if (speech.Contains("spiritual"))
            {
                Say("Spirituality is about understanding one's place in the universe. It guides us, especially in the direst of times. I often meditate to connect with my inner self. Have you tried meditation?");
            }
            else if (speech.Contains("wrestler"))
            {
                Say("Becoming a wrestler is not just about physical prowess. It's about the heart, discipline, and the desire to be the best. If you ever consider it, come find me. I might have something for you.");
            }
            else if (speech.Contains("brew"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the warrior's brew! It's a secret concoction that I drink before my matches. It invigorates the soul and body. If you ever want a sip, just ask. It's my treat! Here have some!");
                    from.AddToBackpack(new PetSlotDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("willpower"))
            {
                Say("Pushing past our limits, that's where true growth happens. In my early days, I had mentors who taught me to harness my inner strength. Do you have such mentors in your life?");
            }
            else if (speech.Contains("revered"))
            {
                Say("I've met many individuals who have left a lasting impact on me. Their stories and wisdom shape my path. I carry a token from one such revered mentor; perhaps you'd like to see it?");
            }
            else if (speech.Contains("meditate"))
            {
                Say("Meditation is my sanctuary. It helps me find balance and clarity. I have a special spot where I go to meditate, surrounded by nature's embrace. Would you like to know more?");
            }
            else if (speech.Contains("best"))
            {
                Say("Ah, the spirit of competition and excellence! For your desire to be the best, I want to reward you with something special. Take this; it's served me well in my journey.");
                from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }

            base.OnSpeech(e);
        }

        public SlamsonTheBold(Serial serial) : base(serial) { }

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
