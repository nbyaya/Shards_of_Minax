using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Kai the Wild")]
    public class KaiTheWild : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public KaiTheWild() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kai the Wild";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 70;
            Hits = 85;

            // Appearance
            AddItem(new TribalMask() { Name = "Kai's Tribal Mask" });
            AddItem(new BoneArms() { Name = "Kai's Bone Arms" });
            AddItem(new BoneLegs() { Name = "Kai's Bone Legs" });
            AddItem(new BoneChest() { Name = "Kai's Bone Chest" });
            AddItem(new ShortSpear() { Name = "Kai's Tribal Spear" });

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
                Say("I am Kai the Wild, a tribal warrior of these lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("My body is strong, and my spirit is fierce!");
            }
            else if (speech.Contains("job"))
            {
                Say("I live by the ways of the tribe, protecting our lands and traditions.");
            }
            else if (speech.Contains("virtues"))
            {
                if (speech.Contains("humility"))
                {
                    Say("The eight virtues are the foundation of our tribal life: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
                }
                else if (speech.Contains("ponder"))
                {
                    Say("True valor lies not just in strength, but in the unity of the tribe. Are you aware of the eight virtues?");
                }
                else if (speech.Contains("learn"))
                {
                    Say("Our tribe seeks to embody these virtues in all we do. Do you wish to learn more about them?");
                }
            }
            else if (speech.Contains("lands"))
            {
                Say("These lands have been the home of my ancestors for generations. They are sacred to us and hold many secrets.");
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit is nurtured by our connection to the land and the stories of old. It guides us in our decisions and actions.");
            }
            else if (speech.Contains("traditions"))
            {
                Say("Our traditions are passed down through the ages. They guide us in our daily lives and remind us of who we are. One such tradition is the Festival of the Moon.");
            }
            else if (speech.Contains("honor"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Honor is a core principle of our tribe. It's not just about battlefield valor, but also about being truthful and standing by one's commitments. For your curiosity and desire to learn, I would like to reward you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("ancestors"))
            {
                Say("Our ancestors were brave warriors and wise shamans. Their teachings and tales are still shared around campfires today.");
            }
            else if (speech.Contains("stories"))
            {
                Say("The stories of old teach us about courage, love, betrayal, and redemption. They shape our beliefs and morals.");
            }
            else if (speech.Contains("festival"))
            {
                Say("The Festival of the Moon is a time of celebration and reflection. We gather to dance, sing, and remember the past while looking forward to the future.");
            }
            else if (speech.Contains("reward"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward for you right now. Please return after some time.");
                }
                else
                {
                    Say("This reward is a symbol of our tribe's gratitude. May it serve you well on your journeys.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public KaiTheWild(Serial serial) : base(serial) { }

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
