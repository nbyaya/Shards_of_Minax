using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Vladimir Reddov")]
    public class VladimirReddov : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public VladimirReddov() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Vladimir Reddov";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Bolshevik";

            // Equip items
            AddItem(new LeatherChest() { Hue = Utility.RandomRedHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomRedHue() });
            AddItem(new LeatherArms() { Hue = Utility.RandomRedHue() });
            AddItem(new LeatherGloves() { Hue = Utility.RandomRedHue() });
            AddItem(new Boots() { Hue = Utility.RandomRedHue() });
            AddItem(new SkullCap() { Hue = Utility.RandomRedHue() });

            // Stats
            Str = 100;
            Dex = 75;
            Int = 85;
            Hits = 100;

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
                Say("Greetings, comrade! I am Vladimir Reddov.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in robust health, ready to defend the revolution!");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to uphold the ideals of the Bolshevik revolution and safeguard the people's treasures.");
            }
            else if (speech.Contains("revolution"))
            {
                Say("The revolution has reshaped our world. It is the very essence of change and progress.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Our treasure is not merely gold but the very spirit of our cause. To prove yourself, you must show true dedication.");
            }
            else if (speech.Contains("dedication"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You must return later for another reward.");
                }
                else
                {
                    Say("Your dedication to the cause is commendable. For your efforts, accept this chest filled with Bolshevik loot.");
                    from.AddToBackpack(new BolsheviksLoot()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public VladimirReddov(Serial serial) : base(serial) { }

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
