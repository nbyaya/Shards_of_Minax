using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Luna the Loyal Tamer")]
    public class LunaTheLoyalTamer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LunaTheLoyalTamer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Luna the Loyal Tamer";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 80;
            Hits = 90;

            // Appearance
            AddItem(new Skirt() { Hue = 65 });
            AddItem(new BodySash() { Hue = 34 });
            AddItem(new Sandals() { Hue = 1175 });
            AddItem(new ShepherdsCrook() { Name = "Luna's Crook" });

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
                Say("Greetings, traveler! I am Luna the Loyal Tamer.");
            }
            else if (speech.Contains("health"))
            {
                Say("All my animals are in good health.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an animal tamer. My life revolves around caring for these creatures.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("The virtue of compassion guides my every action. Do you understand its importance?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Compassion is the foundation of empathy and kindness. Without it, we are lost.");
            }
            else if (speech.Contains("loyalty"))
            {
                Say("It is said that true loyalty lies in the bonds we forge with others, be they human or animal.");
            }
            else if (speech.Contains("luna"))
            {
                Say("Ah, my parents named me Luna because I was born during a full moon. The night holds many mysteries.");
            }
            else if (speech.Contains("animals"))
            {
                Say("I ensure that every animal under my care is treated with utmost respect and love. Have you encountered any exotic creatures in your travels?");
            }
            else if (speech.Contains("creatures"))
            {
                Say("Not just any creature. I've tamed everything from fiery dragons to swift-footed deer. Each one has its own story to tell. Have you heard about the silver stag?");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion can be shown in many ways. For instance, I once saved a wounded griffon from certain death. Would you like to hear that tale?");
            }
            else if (speech.Contains("kindness"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("True kindness is rare. In fact, I have a small token for someone as kind-hearted as you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("bonds"))
            {
                Say("I believe that the strength of a bond, whether with a person or an animal, is tested during challenging times. Have you faced any challenges with a loyal companion by your side?");
            }

            base.OnSpeech(e);
        }

        public LunaTheLoyalTamer(Serial serial) : base(serial) { }

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
