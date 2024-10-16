using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lucius Fervidus")]
    public class LuciusFervidus : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LuciusFervidus() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lucius Fervidus";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = 1157 });
            AddItem(new PlateLegs() { Hue = 1157 });
            AddItem(new PlateArms() { Hue = 1157 });
            AddItem(new PlateGloves() { Hue = 1157 });
            AddItem(new PlateHelm() { Hue = 1157 });
            AddItem(new MetalShield() { Hue = 1157 });

            Hue = Race.RandomSkinHue(); // Random skin hue
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Salutations, I am Lucius Fervidus, once a guardian of Rome's hidden treasures. Ask me about my health, my job, or the treasures I guard.");
            }
            else if (speech.Contains("health") && speech.Contains("name"))
            {
                Say("My health is as robust as the Roman Empire at its peak. To learn more, inquire about my job or the treasures.");
            }
            else if (speech.Contains("job") && speech.Contains("health"))
            {
                Say("My task is to guard secrets and treasures from those unworthy. Ask me about Rome, treasures, or the hidden secrets.");
            }
            else if (speech.Contains("rome") && speech.Contains("job"))
            {
                Say("Rome, the eternal city. Ah, those were the days of glory and grandeur. If you seek deeper knowledge, speak of treasures or secrets.");
            }
            else if (speech.Contains("treasures") && speech.Contains("rome"))
            {
                Say("Treasures are not just gold and jewels but secrets and legends waiting to be discovered. To unlock more, speak of hidden secrets or seek wisdom.");
            }
            else if (speech.Contains("hidden") && speech.Contains("treasures"))
            {
                Say("Hidden, yes, but only to those who lack the wisdom to seek them. If you wish to know more, ask about wisdom or the secrets I guard.");
            }
            else if (speech.Contains("secrets") && speech.Contains("hidden"))
            {
                Say("Secrets are guarded by time and trials. Only those who are worthy shall uncover them. To prove your worth, inquire about wisdom or the path to reward.");
            }
            else if (speech.Contains("wisdom") && speech.Contains("secrets"))
            {
                Say("Wisdom is the key to unlocking the greatest secrets. If you seek to prove your worth, ask me about the path to reward or the trials you must face.");
            }
            else if (speech.Contains("path") && speech.Contains("wisdom"))
            {
                Say("The path to reward is fraught with challenges. Only those who endure shall receive the reward. Ask me about the final trials or the reward itself.");
            }
            else if (speech.Contains("trials") && speech.Contains("path"))
            {
                Say("The trials are many, but with perseverance, one can overcome them. If you are ready, ask me about the final step or the reward for your efforts.");
            }
            else if (speech.Contains("reward") && speech.Contains("trials"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Patience, seeker. The reward shall come in time.");
                }
                else
                {
                    Say("Your quest for wisdom and endurance has been proven. For your perseverance, accept this token of Rome’s hidden splendor.");
                    from.AddToBackpack(new NeroChest()); // Give the Nero's Chest as the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LuciusFervidus(Serial serial) : base(serial) { }

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
