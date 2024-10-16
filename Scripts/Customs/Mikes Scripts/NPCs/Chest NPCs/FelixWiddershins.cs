using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Felix Widdershins")]
    public class FelixWiddershins : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FelixWiddershins() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Felix Widdershins";
            Body = 0x190; // Human male body

            // Stats
            Str = 75;
            Dex = 85;
            Int = 90;
            Hits = 80;

            // Appearance
            AddItem(new LeatherChest() { Hue = 1109 }); // Dark color for stealth
            AddItem(new LeatherLegs() { Hue = 1109 });
            AddItem(new LeatherGloves() { Hue = 1109 });
            AddItem(new Boots() { Hue = 1109 });
            AddItem(new Bandana() { Hue = 1109 });
            AddItem(new Dagger() { Name = "Shadow Dagger", Hue = 0x455 }); // Signature weapon

            Hue = Utility.RandomSkinHue();
            HairItemID = 0x2040; // Short hair
            HairHue = Utility.RandomHairHue();

            // Speech Hue
            SpeechHue = 1152; // Mysterious green color

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
                Say("Ah, I see you have an eye for details. I am Felix Widdershins, a master of the art of stealth and discovery.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to remain hidden in the shadows while gathering the treasures and secrets others can only dream of.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as elusive as my presence. I’m always prepared for the unexpected.");
            }
            else if (speech.Contains("stealth"))
            {
                Say("Stealth is an art form, a blend of shadow and silence. It’s how I thrive.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, treasure! I have a penchant for rare finds. Seek and you shall find the hidden rewards I guard.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Secrets are like shadows; they are not always what they seem. But uncovering them is a thief’s greatest reward.");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Discovery is the key to understanding. The thrill of finding something hidden is unmatched.");
            }
            else if (speech.Contains("puzzle"))
            {
                Say("A puzzle, you say? Indeed, finding the right pieces is essential to uncovering the grander design.");
            }
            else if (speech.Contains("shadow"))
            {
                Say("Shadows are my domain. They conceal and reveal the world in mysterious ways.");
            }
            else if (speech.Contains("hidden"))
            {
                Say("Hidden treasures are the lifeblood of a thief’s existence. They are the ultimate reward for those who seek.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your persistence has led you through a labyrinth of words. As a token of your success, take this special chest, a treasure of the deepest hideaway.");
                    from.AddToBackpack(new ThiefsHideawayStash()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public FelixWiddershins(Serial serial) : base(serial) { }

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
