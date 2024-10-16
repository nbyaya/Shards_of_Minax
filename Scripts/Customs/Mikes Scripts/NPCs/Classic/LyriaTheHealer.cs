using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lyria the Healer")]
    public class LyriaTheHealer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LyriaTheHealer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lyria the Healer";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 80;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 2501 });
            AddItem(new Sandals() { Hue = 1153 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(true); // true for female
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
                Say("I am Lyria the Healer!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good health, thank you!");
            }
            else if (speech.Contains("job"))
            {
                Say("I heal and mend the wounded. I also provide wisdom about herbs and their uses.");
            }
            else if (speech.Contains("healing"))
            {
                Say("Healing is not just mending bones, it's also soothing the soul. Would you like to learn about herbs?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, each herb has a unique property. For example, Ginseng can help restore one's stamina.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("True wisdom is understanding the mysteries of the heart and soul. Seek knowledge and you shall find peace.");
            }
            else if (speech.Contains("lyria"))
            {
                Say("Lyria is an ancient name, passed down in my family. It means 'whisper' in the old tongue.");
            }
            else if (speech.Contains("good"))
            {
                Say("Maintaining one's health is a balance of mind, body, and spirit. Daily meditation and the right herbs help me greatly.");
            }
            else if (speech.Contains("herbs"))
            {
                Say("There are countless herbs in the realm, each with its purpose. Mandrake root, for example, is powerful and used in many rituals. Do you wish to know more about it?");
            }
            else if (speech.Contains("soul"))
            {
                Say("The soul is a delicate entity, often affected by the wounds of the world. It's essential to nurture it with positivity and love.");
            }
            else if (speech.Contains("knowledge"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Knowledge is a treasure, but applying it is the key to wisdom. For your thirst for understanding, here's a small reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("stamina"))
            {
                Say("Stamina is vital for adventurers. If you're ever in need, come to me, and I might have just the right concoction for you.");
            }
            else if (speech.Contains("whisper"))
            {
                Say("Whispers of the past often carry secrets. My ancestors were known to communicate with spirits. Would you like to know a secret?");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance in life comes from understanding oneself and one's surroundings. Nature has a lot to teach us.");
            }
            else if (speech.Contains("rituals"))
            {
                Say("Rituals are ceremonies that harness the energy of the universe. They can be used for healing, protection, and even divination.");
            }
            else if (speech.Contains("love"))
            {
                Say("Love is the most potent healing force. It binds us, drives us, and fills our world with light.");
            }

            base.OnSpeech(e);
        }

        public LyriaTheHealer(Serial serial) : base(serial) { }

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
