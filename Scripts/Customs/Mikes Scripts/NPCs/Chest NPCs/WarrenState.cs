using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Warren State")]
    public class WarrenState : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public WarrenState() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Warren State";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });

            // Random facial features
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Greetings, traveler. I am Warren State, keeper of the Warring States Chest.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in great health, fortified by years of battle and strategy.");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to guard the Warring States Chest and ensure only the worthy gain its rewards.");
            }
            else if (speech.Contains("warring states"))
            {
                Say("The Warring States Chest holds many secrets of the ancient battles and treasures.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Ah, the secrets! They are unlocked only through wisdom and perseverance.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the key to understanding and mastering the art of war. The more you seek, the more you shall find.");
            }
            else if (speech.Contains("perseverance"))
            {
                Say("Perseverance through trials and challenges is what will reveal the chest's true rewards.");
            }
            else if (speech.Contains("trials"))
            {
                Say("Many have tried and failed to unlock the chest. Only those who prove their worth shall succeed.");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, you must understand the trials, and perseverance, and embrace the wisdom hidden within.");
            }
            else if (speech.Contains("embrace"))
            {
                Say("Embrace the challenge, and remember, the journey is as important as the reward.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Indeed, the journey through understanding these concepts will guide you to the final reward.");
            }
            else if (speech.Contains("reward"))
            {
                Say("The final reward is reserved for those who fully grasp the importance of each keyword.");
            }
            else if (speech.Contains("importance"))
            {
                Say("Understanding the importance of each keyword will lead you to the heart of the chest's secrets.");
            }
            else if (speech.Contains("secrets of the chest"))
            {
                Say("Ah, you've grasped the essence of the chest. Now, only those who prove their dedication are worthy.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication is the final key. Show me your true commitment to the quest.");
            }
            else if (speech.Contains("commitment"))
            {
                Say("Your commitment is admirable. For your understanding and perseverance, I shall reward you.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Your understanding has brought you to this moment. Accept the Warring States Chest as your reward.");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have already been rewarded recently. Return later for another chance.");
                }
                else
                {
                    from.AddToBackpack(new WarringStatesChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public WarrenState(Serial serial) : base(serial) { }

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
