using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lilith the Deathweaver")]
    public class LilithTheDeathweaver : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilithTheDeathweaver() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lilith the Deathweaver";
            Body = 0x191; // Human female body

            // Stats
            Str = 125;
            Dex = 55;
            Int = 155;
            Hits = 105;

            // Appearance
            AddItem(new Robe() { Hue = 1175 });
            AddItem(new Sandals() { Hue = 1109 });
            AddItem(new BoneHelm());

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
                Say("I am Lilith the Deathweaver, a master of dark arts!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is sustained by the power of the dark forces.");
            }
            else if (speech.Contains("job"))
            {
                Say("I delve into the secrets of necromancy and control the undead.");
            }
            else if (speech.Contains("dark arts"))
            {
                Say("Dark arts are not for the faint of heart. Are you brave enough to learn?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then prove your dedication by completing this dark task.");
            }
            else if (speech.Contains("forces"))
            {
                Say("The dark forces are ancient energies, flowing through the world, waiting to be harnessed by those with the knowledge and will.");
            }
            else if (speech.Contains("necromancy"))
            {
                Say("Necromancy, the art of communing with and controlling the dead. I have studied its mysteries for years. There is power beyond imagination for those who dare.");
            }
            else if (speech.Contains("undead"))
            {
                Say("The undead are misunderstood beings, merely tools to be wielded. With the right incantations, they can be made to serve any purpose. Take this friend.");
                from.AddToBackpack(new SpiritSpeakAugmentCrystal()); // Give the reward
            }
            else if (speech.Contains("power"))
            {
                Say("Power comes at a price. If you're willing, I can offer you a task. Complete it, and you shall be rewarded.");
            }
            else if (speech.Contains("task"))
            {
                Say("Deep in the Whispering Woods, there lies a crypt. Retrieve the Black Orb resting there, and bring it to me. Do this, and you shall receive what you seek.");
            }
            else if (speech.Contains("reward"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, curious about your reward? Complete the task, and you'll find out. Rest assured, it will be worth your effort.");
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LilithTheDeathweaver(Serial serial) : base(serial) { }

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
