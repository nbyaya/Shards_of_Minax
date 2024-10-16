using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Tidal Sage")]
    public class TidalSage : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public TidalSage() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Tidal Sage";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 50;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 2159 }); // Deep blue hue to match the abyssal theme
            AddItem(new Sandals() { Hue = 2159 });
            AddItem(new WizardsHat() { Hue = 2159 });
            AddItem(new Cloak() { Name = "Abyssal Cloak", Hue = 2159 });

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
                Say("I am the Tidal Sage, keeper of secrets from the depths of the abyss.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is but a reflection of the ocean's vastness, always ebbing and flowing.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard ancient treasures and share the wisdom of the abyss with those who seek it.");
            }
            else if (speech.Contains("depths"))
            {
                Say("The depths of the ocean hide many secrets. Only those who truly seek will uncover them.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, you speak of treasure. The true reward lies in understanding the mysteries of the deep.");
            }
            else if (speech.Contains("reveal"))
            {
                Say("To reveal the secrets of the abyss, one must prove their worth. Only then shall the chest be yours.");
            }
            else if (speech.Contains("worth"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have yet to prove your worth. Return when you have demonstrated your dedication.");
                }
                else
                {
                    Say("You have shown perseverance and wisdom. As a token of the abyssal secrets, take this chest.");
                    from.AddToBackpack(new AbyssalPlaneChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets of the abyss are revealed to those who have patience and curiosity. Keep exploring, traveler.");
            }

            base.OnSpeech(e);
        }

        public TidalSage(Serial serial) : base(serial) { }

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
