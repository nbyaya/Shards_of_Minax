using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Aria the Luminal Dancer")]
    public class AriaTheLuminalDancer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public AriaTheLuminalDancer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Aria the Luminal Dancer";
            Body = 0x191; // Human female body

            // Stats
            Str = 50;
            Dex = 130;
            Int = 120;
            Hits = 80;

            // Appearance
            AddItem(new FancyDress(1187)); // PlainDress
            AddItem(new TribalMask(1186));  // DearMask
            AddItem(new Longsword() { Name = "Aria's Whisper" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Aria the Luminal Dancer, a visitor from distant realms!");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect harmony!");
            }
            else if (speech.Contains("job"))
            {
                Say("I dance among the stars!");
            }
            else if (speech.Contains("dance"))
            {
                Say("True luminescence is found in the hearts of those who seek the light. Dost thou seek enlightenment?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then continue to dance along the path of light!");
            }
            else if (speech.Contains("realms"))
            {
                Say("Ah, the realms I hail from are filled with shimmering lights and undulating energies, where dance and song are the very essence of existence.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is the balance between the mind, body, and spirit. I have learned the ways of inner peace from the ancient sages of my realm. They hold ancient secrets. Would you like to know more?");
            }
            else if (speech.Contains("stars"))
            {
                Say("The stars are not just mere balls of fire in the sky; they hold stories and mysteries that few dare to explore. I have danced on many a star, absorbing their tales and wisdom. One such tale speaks of a hidden treasure.");
            }
            else if (speech.Contains("enlightenment"))
            {
                Say("Enlightenment is the pure understanding of oneself and the universe. It is a state few achieve but many strive for. In my travels, I've found a sacred dance that aids in attaining this state. Would you like to learn it?");
            }
            else if (speech.Contains("lights"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Lights are manifestations of energy, each color and hue representing a different emotion and vibration. When I dance, I channel these vibrations, bringing warmth and joy to all who witness. As thanks for your interest, take this token of appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("secrets"))
            {
                Say("The ancient sages held the power to merge with the universe, listening to its whispers and understanding its desires. They left behind clues for the worthy. Seek the monolith of echoes if you wish to uncover their teachings.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("The treasure is not of gold or gems, but of knowledge and memories of ages past. Many have searched for it, but only those pure of heart have found it. Will you embark on this quest?");
            }
            else if (speech.Contains("sacred"))
            {
                Say("The sacred dance is a sequence of movements that align the dancer with the universe's rhythm. When performed under the crescent moon, it is said to bestow visions. Tread carefully, for not all visions are pleasant.");
            }

            base.OnSpeech(e);
        }

        public AriaTheLuminalDancer(Serial serial) : base(serial) { }

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
