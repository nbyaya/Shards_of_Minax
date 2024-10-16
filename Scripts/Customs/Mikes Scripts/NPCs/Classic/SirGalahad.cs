using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Galahad")]
    public class SirGalahad : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirGalahad() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Galahad";
            Body = 0x190; // Human male body

            // Stats
            Str = 155;
            Dex = 65;
            Int = 25;
            Hits = 110;

            // Appearance
            AddItem(new ChainLegs() { Hue = 1150 });
            AddItem(new ChainChest() { Hue = 1150 });
            AddItem(new PlateHelm() { Hue = 1150 });
            AddItem(new PlateGloves() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1150 });

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
                Say("I am Sir Galahad, the once-great knight!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is but a shadow of what it used to be.");
            }
            else if (speech.Contains("job"))
            {
                Say("My current job is to reminisce about past glory.");
            }
            else if (speech.Contains("battles"))
            {
                Say("True valor is now but a distant memory, lost in the sands of time. Do you seek valor?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Valor, you say? In this day and age? Ha! It's as rare as a dragon's tear.");
            }
            else if (speech.Contains("great"))
            {
                Say("Ah, you've heard of my tales from before. There was a time when my name echoed across the land after the battle of Ravenwood.");
            }
            else if (speech.Contains("shadow"))
            {
                Say("Yes, a shadow cast by the curse of the Wraith's dagger. Ever since that fatal strike, my health has never been the same.");
            }
            else if (speech.Contains("reminisce"))
            {
                Say("Ah, reminiscing brings both joy and sorrow. I often find myself lost in the memory of the Maiden's Tower and the challenge that awaited me there.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is not just in battles and wars; it's in the small choices we make every day. Once, in the village of Silverbrook, I made such a choice that changed my destiny.");
            }
            else if (speech.Contains("dragon"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("You seem intrigued. Dragon's tears are symbols of utmost rarity and purity. If you ever find one, cherish it, for it could change your life. In fact, here's something from my collection for a seeker like you.");
                    from.AddToBackpack(new MeditationAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("cherish"))
            {
                Say("Your curiosity and persistence remind me of myself in my prime. Here, take this as a token of appreciation. May it serve you well in your adventures.");
                from.AddToBackpack(new MeditationAugmentCrystal()); // Give the reward
            }

            base.OnSpeech(e);
        }

        public SirGalahad(Serial serial) : base(serial) { }

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
