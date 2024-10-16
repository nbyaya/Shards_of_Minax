using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zara the Flame Whisperer")]
    public class ZaraTheFlameWhisperer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ZaraTheFlameWhisperer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zara the Flame Whisperer";
            Body = 0x190; // Human female body

            // Stats
            Str = 80;
            Dex = 60;
            Int = 90;
            Hits = 85;

            // Appearance
            AddItem(new Robe() { Hue = 1172 });
            AddItem(new Sandals() { Hue = 1171 });
            AddItem(new FireballWand() { Name = "Zara's Ember" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler. I am Zara the Flame Whisperer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in tune with the flames, my health is always vibrant.");
            }
            else if (speech.Contains("job"))
            {
                Say("My calling is to harness the power of flames and guide others in their path.");
            }
            else if (speech.Contains("flames") || speech.Contains("warmth"))
            {
                Say("Fire is both a destructive and transformative force. Have you ever felt its warmth?");
            }
            else if (speech.Contains("secrets") || speech.Contains("dare"))
            {
                Say("Indeed, I have. Fire can both cleanse and consume. Its secrets are for those who dare to understand.");
            }
            else if (speech.Contains("zara"))
            {
                Say("Ah, you know my name. It was given to me by the elders after I whispered to my first flame.");
            }
            else if (speech.Contains("vibrant"))
            {
                Say("Yes, the fires rejuvenate my spirit and keep my essence alive. It is a bond that not many understand.");
            }
            else if (speech.Contains("guide"))
            {
                Say("Many come seeking guidance, and I help them discover the power of the flame within themselves. It's a path of enlightenment and self-discovery.");
            }
            else if (speech.Contains("destructive"))
            {
                Say("Many see fire as only a force of destruction, but I see its potential for rebirth and renewal. Like the phoenix, we too can rise from the ashes.");
            }
            else if (speech.Contains("understand"))
            {
                Say("It's a journey to truly grasp the essence of fire. Many have tried and faltered. But for those who persevere, the rewards are immeasurable.");
            }
            else if (speech.Contains("warmth"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Warmth is the embrace of the flame. It reminds us that even in the darkest times, there is a spark of hope. For those who seek, I can bestow a gift of this warmth upon you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ZaraTheFlameWhisperer(Serial serial) : base(serial) { }

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
