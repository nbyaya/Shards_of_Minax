using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mistress Viper")]
    public class MistressViper : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MistressViper() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mistress Viper";
            Body = 0x191; // Human female body

            // Stats
            Str = 90;
            Dex = 130;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new StuddedLegs() { Hue = 1272 });
            AddItem(new StuddedChest() { Hue = 1272 });
            AddItem(new StuddedArms() { Hue = 1272 });
            AddItem(new StuddedGloves() { Hue = 1272 });
            AddItem(new Boots() { Hue = 1272 });
            AddItem(new Dagger() { Name = "Mistress Viper's Dagger" });

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
                Say("I am Mistress Viper, the shadow in the night.");
            }
            else if (speech.Contains("health"))
            {
                Say("My wounds are concealed beneath the cloak of shadows.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to eliminate those who dare oppose the darkness.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor is found in the shadows, where the heart's darkness is tested. Are you prepared for this path?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then embrace the shadows, for they shall be your closest ally.");
            }
            else if (speech.Contains("night"))
            {
                Say("The night conceals many secrets and tales, but not all of them are meant to be told.");
            }
            else if (speech.Contains("wounds"))
            {
                Say("These wounds, both visible and hidden, are tokens from battles against those who challenge the abyss.");
            }
            else if (speech.Contains("darkness"))
            {
                Say("The darkness is an ancient force. Those who understand it can wield it, but those who oppose it shall fall beneath its weight.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("One secret I can share, is that sometimes, rewards come to those who tread the path of shadows.");
            }
            else if (speech.Contains("abyss"))
            {
                Say("The abyss is a realm of endless void. But it's also where the truest forms of power reside. Would you risk yourself for such power?");
            }
            else if (speech.Contains("force"))
            {
                Say("True force is not about brute strength, but understanding the balance between darkness and light.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Since you seek knowledge and show courage, take this as a token of my appreciation. May it aid you in your journey.");
                    from.AddToBackpack(new DiscordanceAugmentCrystal()); // Replace with actual reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public MistressViper(Serial serial) : base(serial) { }

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
