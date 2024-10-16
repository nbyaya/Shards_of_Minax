using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Melody the Siren")]
    public class MelodyTheSiren : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MelodyTheSiren() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Melody";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 40;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new Skirt() { Hue = 1159 });
            AddItem(new FemaleLeatherChest() { Hue = 1159 });
            AddItem(new LeatherGloves() { Hue = 1159 });
            AddItem(new Lute() { Name = "Melody's Lute" });
            
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
                Say("I'm Melody the Siren, but don't expect me to sing for you!");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Why do you care? It's none of your business!");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job\"? I lure sailors to their doom with my enchanting voice. Happy now?");
            }
            else if (speech.Contains("battles"))
            {
                Say("Oh, really? You're valiant? Prove it then, but don't expect any sympathy from me!");
            }
            else if (speech.Contains("sing"))
            {
                Say("Why does everyone always want me to sing? I'm more than just my voice, you know. But if you bring me a trinket from the depths, maybe I'll consider it.");
            }
            else if (speech.Contains("care"))
            {
                Say("Curious, aren't you? Maybe it's because not many have shown genuine concern for me. Everyone's just scared of the legend.");
            }
            else if (speech.Contains("sailors"))
            {
                Say("Yes, sailors. Men with dreams of adventure, riches, and the sea. But they never expect the dangers that lurk below. There's a particular tale I remember...");
            }
            else if (speech.Contains("trinket"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, you have something for me? Very well. As promised, I'll sing a short tune. But remember, be careful of what you wish for.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("legend"))
            {
                Say("They say sirens used to be kind, guiding sailors safely home. But a betrayal by one sailor led us to become vengeful spirits. But who knows, every tale has some truth in it.");
            }
            else if (speech.Contains("doom"))
            {
                Say("Not every sailor meets their doom, some manage to resist. It's a game really, one of wit and will. Are you playing?");
            }
            else if (speech.Contains("tale"))
            {
                Say("Long ago, there was a sailor named Elias who charmed even the most vengeful of sirens. Instead of leading him to doom, they fell in love. A rare story, don't you think?");
            }
            else if (speech.Contains("betrayal"))
            {
                Say("One sailor, smitten by a siren's song, promised her his heart. But he betrayed her for another. Since then, we've been cautious of men and their fleeting promises.");
            }

            base.OnSpeech(e);
        }

        public MelodyTheSiren(Serial serial) : base(serial) { }

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
