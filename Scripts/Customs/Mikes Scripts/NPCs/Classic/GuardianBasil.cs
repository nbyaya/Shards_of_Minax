using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Guardian Basil")]
    public class GuardianBasil : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GuardianBasil() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Guardian Basil";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 90;
            Int = 60;
            Hits = 90;

            // Appearance
            AddItem(new ChainChest() { Hue = 1953 });
            AddItem(new ChainLegs() { Hue = 1953 });
            AddItem(new ChainCoif() { Hue = 1953 });
            AddItem(new LeatherGloves() { Hue = 1953 });
            AddItem(new Boots() { Hue = 1953 });
            AddItem(new Halberd() { Name = "Basil's Halberd" });

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
                Say("I am Guardian Basil, the keeper of ancient secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My essence remains untouched.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard the gateway to hidden realms.");
            }
            else if (speech.Contains("secrets") || speech.Contains("knowledge"))
            {
                Say("Hidden truths require the key of curiosity. Dost thou seek knowledge?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then heed my words, and the path to enlightenment shall be revealed.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets I protect are ancient and powerful. Some are too dangerous for mortals to know. Do you seek power or wisdom?");
            }
            else if (speech.Contains("essence"))
            {
                Say("My essence is bound by the magic of the ancients. They have bestowed upon me longevity and purpose. Do you believe in ancient magic?");
            }
            else if (speech.Contains("gateway"))
            {
                Say("The gateway I guard is not for the faint of heart. Only those who prove their worth may pass. Are you prepared to prove yourself?");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("True curiosity is a gift, a key that can unlock many doors. But remember, not all doors should be opened. Have you ever regretted opening a door?");
            }
            else if (speech.Contains("wisdom"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Wisdom is more valuable than any treasure. To those who truly seek it, I shall grant a small token of knowledge. Hold out your hand.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("power"))
            {
                Say("Power can be a double-edged sword. It can protect or destroy. Use it wisely, for with great power comes great responsibility. Do you understand the weight of this responsibility?");
            }
            else if (speech.Contains("ancients"))
            {
                Say("The ancients were beings of immense knowledge and magic. Their legacy is scattered throughout the land in hidden places. Would you like a clue to one such place?");
            }
            else if (speech.Contains("worth"))
            {
                Say("To prove your worth, you must face challenges and tests. But fear not, for I sense a strong spirit within you. Are you ready to embark on this quest? Take this.");
                from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }

            base.OnSpeech(e);
        }

        public GuardianBasil(Serial serial) : base(serial) { }

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
