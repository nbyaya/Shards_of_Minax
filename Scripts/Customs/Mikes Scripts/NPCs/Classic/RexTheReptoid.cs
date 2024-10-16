using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rex the Reptoid")]
    public class RexTheReptoid : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RexTheReptoid() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rex the Reptoid";
            Body = 0x1C; // Reptoid body

            // Stats
            Str = 120;
            Dex = 80;
            Int = 40;
            Hits = 90;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 2121 });
            AddItem(new LeatherChest() { Hue = 2121 });
            AddItem(new LeatherGloves() { Hue = 2121 });
            AddItem(new PlateHelm() { Hue = 2122 });
            AddItem(new Axe() { Name = "Rex's Fang" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Greetings, I am Rex the Reptoid!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as stable as the cosmic currents.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a cosmic observer, contemplating the virtues of your world.");
            }
            else if (speech.Contains("virtues") || speech.Contains("humility") || speech.Contains("wisdom"))
            {
                Say("In the cosmic dance of virtues, humility is the key to unlocking the mysteries of the universe. Do you seek wisdom?");
            }
            else if (speech.Contains("seek") || speech.Contains("embrace humility"))
            {
                Say("Indeed, wisdom is the path to enlightenment. How do you seek to embrace humility in your journey?");
            }
            else if (speech.Contains("embrace"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("To truly embrace humility, one must let go of ego, pride, and the thirst for recognition. It's a challenging path, but I have something that might help you on your journey. Here, take this as a token of encouragement.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("journey"))
            {
                Say("Ah, the journey. It is ever winding and full of unexpected turns. I've observed many on their quests. Some find what they seek, others lose their way. What drives your journey?");
            }
            else if (speech.Contains("seek"))
            {
                Say("Seeking wisdom and understanding is commendable. The universe is vast, and its mysteries are infinite. If you remain humble and open-minded, you'll surely find the answers you're looking for.");
            }
            
            base.OnSpeech(e);
        }

        public RexTheReptoid(Serial serial) : base(serial) { }

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
