using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Lancelot")]
    public class SirLancealot : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirLancealot() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Lancelot";
            Body = 0x190; // Human male body

            // Stats
            Str = 158;
            Dex = 66;
            Int = 26;
            Hits = 113;

            // Appearance
            AddItem(new ChainLegs() { Hue = 1400 });
            AddItem(new ChainChest() { Hue = 1400 });
            AddItem(new PlateHelm() { Hue = 1400 });
            AddItem(new PlateGloves() { Hue = 1400 });
            AddItem(new Boots() { Hue = 1400 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Sir Lancelot, the most unappreciated knight in the realm.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? What does it matter? No one cares about my well-being.");
            }
            else if (speech.Contains("job"))
            {
                Say("My so-called 'job' is to protect this wretched town from imaginary threats.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you have any idea how it feels to be stuck here, defending a town that doesn't appreciate my valor?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Well, if you think valor is so important, what would you do if you were me?");
            }
            else if (speech.Contains("unappreciated"))
            {
                Say("Indeed, despite my tireless service to the realm, few recognize my contributions. Everyone's too busy singing praises of the other knights.");
            }
            else if (speech.Contains("realm"))
            {
                Say("Ah, the realm. A land of beauty and mysteries. But lately, it's been plagued by dark forces. If only someone would join me in driving them away.");
            }
            else if (speech.Contains("imaginary"))
            {
                Say("They say the threats are imaginary. But I've seen shadows lurking in the forests. I've heard whispers of dark gatherings. They laugh now, but soon, they may not.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows... they're not just ordinary ones. They move, whisper, and seem to have a will of their own. Many nights, I stand watch, waiting for them to reveal themselves.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is not just in the heat of the battle but in the quiet moments, standing guard, even when others mock or ignore you. It's in the unwavering dedication to one's duty.");
            }
            else if (speech.Contains("duty"))
            {
                Say("My duty is to protect this town, no matter what. Even if they don't see the dangers that I do, I'll stand vigilant. But it does get lonely, being the only one who seems to care.");
            }
            else if (speech.Contains("lonely"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Being a knight isn't always about glory and accolades. There are times of solitude and self-doubt. But seeing someone like you, showing interest... It reminds me of why I do this. Here, take this as a token of my appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SirLancealot(Serial serial) : base(serial) { }

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
