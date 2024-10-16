using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Noble Kage")]
    public class NobleKage : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NobleKage() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Noble Kage";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 65;
            Int = 100;
            Hits = 75;

            // Appearance
            AddItem(new PlateChest() { Hue = 1152 });
            AddItem(new PlateLegs() { Hue = 1152 });
            AddItem(new DragonHelm() { Hue = 1152 });
            AddItem(new BoneGloves() { Hue = 1152 });
            AddItem(new ThighBoots() { Hue = 1152 });

            // Hair and Facial Hair
            HairItemID = 0x203B; // Dark hair
            HairHue = 0x00; // Black

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
                Say("Greetings, I am Noble Kage, guardian of treasures untold.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as sturdy as the finest armor, for I am protected by the strength of my treasures.");
            }
            else if (speech.Contains("job"))
            {
                Say("I guard the secrets and treasures of this realm. Only the worthy may unlock their secrets.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, the treasure! Prove your worth, and I shall reward you with a chest of wonders.");
            }
            else if (speech.Contains("prove"))
            {
                Say("Show me your determination and wisdom, and you shall be rewarded.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Wisdom is the key to unlocking many doors. It guides one to true rewards.");
            }
            else if (speech.Contains("determination"))
            {
                Say("Determination is a trait of the steadfast. It will guide you through trials.");
            }
            else if (speech.Contains("trials"))
            {
                Say("The trials of life are tests of character. Only those who persist shall find their way.");
            }
            else if (speech.Contains("character"))
            {
                Say("Character is shaped by our actions and choices. It's the essence of our being.");
            }
            else if (speech.Contains("actions"))
            {
                Say("Our actions define us more than our words. They are the true measure of our worth.");
            }
            else if (speech.Contains("words"))
            {
                Say("Words can inspire or deceive. It is our actions that truly speak volumes.");
            }
            else if (speech.Contains("inspire"))
            {
                Say("To inspire is to lift others and guide them towards their goals.");
            }
            else if (speech.Contains("deceive"))
            {
                Say("Deception clouds the truth. Seek honesty and integrity in all your dealings.");
            }
            else if (speech.Contains("truth"))
            {
                Say("The truth is often hidden, but it is essential for true understanding.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding is the foundation of wisdom. Seek to understand before acting.");
            }
            else if (speech.Contains("foundation"))
            {
                Say("The foundation of a strong character is built upon integrity and truth.");
            }
            else if (speech.Contains("integrity"))
            {
                Say("Integrity is the bedrock of trust. Without it, nothing else can be built.");
            }
            else if (speech.Contains("trust"))
            {
                Say("Trust is a fragile thing, once broken it is hard to repair. Cherish it.");
            }
            else if (speech.Contains("cherish"))
            {
                Say("Cherish the moments and relationships that bring you joy. They are the true treasures of life.");
            }
            else if (speech.Contains("treasures"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("You have yet to prove your worth. Come back later.");
                }
                else
                {
                    Say("Your journey has been one of insight and growth. For your perseverance and wisdom, accept this treasure chest as a token of my esteem.");
                    from.AddToBackpack(new KagesTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public NobleKage(Serial serial) : base(serial) { }

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
