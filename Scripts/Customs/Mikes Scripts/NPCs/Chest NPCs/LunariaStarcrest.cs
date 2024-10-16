using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lunaria Starcrest")]
    public class LunariaStarcrest : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LunariaStarcrest() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lunaria Starcrest";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 70;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new Robe() { Hue = 1152 });
            AddItem(new Sandals() { Hue = 1152 });
            AddItem(new WizardsHat() { Hue = 1152 });
            AddItem(new Spellbook() { Name = "Lunaria's Grimoire" });
            AddItem(new GnarledStaff() { Hue = 1152 });

            // Hair and facial features
            HairItemID = 0x203B; // Long hair
            HairHue = 1152;
            FacialHairItemID = 0; // No facial hair for females

            // Speech Hue
            SpeechHue = 1152; // Moon-themed color
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
                Say("Greetings, I am Lunaria Starcrest, keeper of moonlit secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, sustained by the gentle light of the moon.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to guard the mysteries of the moon and share its wisdom with those who seek.");
            }
            else if (speech.Contains("moon"))
            {
                Say("The moon holds many secrets, but only those who truly seek may uncover them.");
            }
            else if (speech.Contains("secrets") && speech.Contains("moon"))
            {
                Say("The secrets of the moon are not easily revealed. You must prove your curiosity and patience.");
            }
            else if (speech.Contains("curiosity") && speech.Contains("secrets"))
            {
                Say("Curiosity is the path to knowledge. What do you wish to know?");
            }
            else if (speech.Contains("knowledge") && speech.Contains("curiosity"))
            {
                Say("To gain knowledge, one must first show dedication. Only then will the moon's gifts be revealed.");
            }
            else if (speech.Contains("dedication") && speech.Contains("knowledge"))
            {
                Say("Your dedication must be matched by your perseverance. Seek further truths.");
            }
            else if (speech.Contains("truths") && speech.Contains("dedication"))
            {
                Say("The truths of the cosmos are vast. To grasp them, one must embrace the unknown.");
            }
            else if (speech.Contains("unknown") && speech.Contains("truths"))
            {
                Say("Embracing the unknown requires courage. Only the brave may uncover the moon's greatest secrets.");
            }
            else if (speech.Contains("courage") && speech.Contains("unknown"))
            {
                Say("Courage is tested through trials. Have you faced challenges with bravery?");
            }
            else if (speech.Contains("trials") && speech.Contains("courage"))
            {
                Say("Facing trials shows your true mettle. To prove your worth, show me your resolve.");
            }
            else if (speech.Contains("resolve") && speech.Contains("trials"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you at this moment. Return when the moon is high in the sky.");
                }
                else
                {
                    Say("Your resolve has been tested and proven. Accept this Mystic Moon Chest as a token of the moon's favor.");
                    from.AddToBackpack(new MysticMoonChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LunariaStarcrest(Serial serial) : base(serial) { }

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
