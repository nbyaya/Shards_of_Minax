using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Morgar")]
    public class SirMorgar : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirMorgar() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Morgar";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 90;
            Int = 40;
            Hits = 85;

            // Appearance
            AddItem(new ChainChest() { Hue = 2417 });
            AddItem(new ChainLegs() { Hue = 2417 });
            AddItem(new ChainCoif() { Hue = 2417 });
            AddItem(new LeatherGloves() { Hue = 2417 });
            AddItem(new Boots() { Hue = 2417 });
            AddItem(new WarMace() { Name = "Sir Morgar's Mace" });

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
                Say("I am Sir Morgar, the shadow in the night.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is of no concern to thee.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am an assassin, a master of shadows and death.");
            }
            else if (speech.Contains("assassin") || speech.Contains("path"))
            {
                Say("The path of an assassin is one of secrecy and darkness. Dost thou seek such a path?");
            }
            else if (speech.Contains("yes") || speech.Contains("shadows") || speech.Contains("trust") || speech.Contains("trace"))
            {
                Say("Very well. In the world of shadows, trust no one, and leave no trace behind.");
            }
            else if (speech.Contains("shadow"))
            {
                Say("The shadow is not just a title but a way of life. I blend in and strike from where least expected.");
            }
            else if (speech.Contains("concern"))
            {
                Say("Perhaps it isn't your concern, but in this line of work, health is paramount. One must always be ready.");
            }
            else if (speech.Contains("death"))
            {
                Say("Death is not the end, but a new beginning. I bring justice to those who evade it.");
            }
            else if (speech.Contains("secrecy"))
            {
                Say("Secrets are the weapon of my trade. Knowledge is power, and silence is golden.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is a rare commodity. Yet, for one who proves themselves, I might offer a token of appreciation.");
            }
            else if (speech.Contains("token"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("You have intrigued me. Here, take this as a sign of my trust. Use it wisely.");
                    from.AddToBackpack(new MaxxiaScroll()); // Example reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is subjective. What may be just for one, may not be for another. Remember that in your journey.");
            }
            else if (speech.Contains("journey"))
            {
                Say("Every journey has its challenges. In the world of assassination, challenges mean life or death.");
            }

            base.OnSpeech(e);
        }

        public SirMorgar(Serial serial) : base(serial) { }

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
