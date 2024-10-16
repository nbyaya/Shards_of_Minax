using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dr. Newton")]
    public class DrNewton : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DrNewton() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dr. Newton";
            Body = 0x190; // Human male body

            // Stats
            Str = 70;
            Dex = 50;
            Int = 120;
            Hits = 50;

            // Appearance
            AddItem(new LongPants() { Hue = 1104 });
            AddItem(new FancyShirt() { Hue = 1152 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new Cap() { Hue = 1104 });
            AddItem(new Spellbook() { Name = "Dr. Newton's Notes" });

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
                Say("Greetings, I am Dr. Newton, the scientist.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job as a scientist is to unravel the mysteries of the world.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Science, like all pursuits, requires valor in the face of the unknown. Are you a courageous individual?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Indeed, courage is a virtue worth cherishing. It's essential to persevere in the pursuit of knowledge.");
            }
            else if (speech.Contains("scientist"))
            {
                Say("As a scientist, I've delved into various realms of discovery. Lately, I've been particularly engrossed in the study of ancient artifacts.");
            }
            else if (speech.Contains("perfect"))
            {
                Say("While I am in perfect health now, there was a time I was gravely ill due to an experiment gone awry. But thanks to a rare herb, I made a miraculous recovery.");
            }
            else if (speech.Contains("mysteries"))
            {
                Say("The mysteries I unravel often lead to new questions. Recently, I've been studying a peculiar stone that emits a strange energy. I'm yet to decipher its purpose.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Ancient artifacts often hold secrets from the past. If you ever come across any curious items, bring them to me. I might reward you for your effort.");
            }
            else if (speech.Contains("experiment"))
            {
                Say("Experiments can be risky, but they pave the way for new discoveries. While some of my tests have been failures, they've taught me invaluable lessons.");
            }
            else if (speech.Contains("stone"))
            {
                Say("This peculiar stone is unlike any I've seen. Its energy is both enchanting and perplexing. Some believe it might be of extraterrestrial origin.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Your efforts in aiding my research won't go unnoticed. Here, take this as a token of my appreciation. I hope it serves you well.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("lessons"))
            {
                Say("One of the most important lessons I've learned is to never give up, even when faced with seemingly insurmountable challenges. Persistence is key in science.");
            }
            else if (speech.Contains("extraterrestrial"))
            {
                Say("The idea of life beyond our world has always intrigued me. The universe is vast, and we're only beginning to scratch the surface of its secrets.");
            }

            base.OnSpeech(e);
        }

        public DrNewton(Serial serial) : base(serial) { }

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
