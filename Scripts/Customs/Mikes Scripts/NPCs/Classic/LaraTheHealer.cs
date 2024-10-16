using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lara the Healer")]
    public class LaraTheHealer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LaraTheHealer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lara the Healer";
            Body = 0x191; // Human female body

            // Stats
            Str = 100;
            Dex = 70;
            Int = 80;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1165 }); // Robe with hue 1165
            AddItem(new Sandals() { Hue = 0 }); // Sandals with hue 0
            AddItem(new QuarterStaff() { Name = "Lara's Staff" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Lara the Healer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I heal those in need and guide souls seeking wisdom.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("The body can be healed with potions and magic, but the soul requires understanding and empathy. Tell me, do you seek wisdom?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Then listen closely: The strongest warriors are those who fight not only with their weapons but with their hearts.");
            }
            else if (speech.Contains("journey"))
            {
                Say("The journey for wisdom is endless. Continue to seek, and you shall find.");
            }
            else if (speech.Contains("lara"))
            {
                Say("Yes, my name is rooted in ancient tales. It means 'protection'. In my role, I strive to protect both body and spirit.");
            }
            else if (speech.Contains("good"))
            {
                Say("Being in good health is not just about the body, but also the mind and spirit. I meditate daily to keep them all in balance.");
            }
            else if (speech.Contains("heal"))
            {
                Say("Healing is more than mending wounds. It's about understanding the root of the pain, whether it be physical or emotional. Often, I use herbs in my practice.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("True understanding comes from listening, not just hearing. It's about opening one's heart to the experiences of others. Have you ever helped someone in dire need?");
            }
            else if (speech.Contains("seek"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Those who seek with a pure heart often find more than they expect. For your dedication to understanding, here is a token of my appreciation.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("weapons"))
            {
                Say("Weapons can defend or harm, but it's the intention behind them that matters. Have you ever used your strength for a cause?");
            }

            base.OnSpeech(e);
        }

        public LaraTheHealer(Serial serial) : base(serial) { }

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
