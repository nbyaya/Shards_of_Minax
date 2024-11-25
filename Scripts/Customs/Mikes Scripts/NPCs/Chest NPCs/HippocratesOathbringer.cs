using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Hippocrates Oathbringer")]
    public class HippocratesOathbringer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public HippocratesOathbringer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Hippocrates Oathbringer";
            Body = 0x190; // Human male body
            Hue = Utility.RandomSkinHue();
            Title = "the Healer";

            // Appearance
            AddItem(new Robe(Utility.RandomBlueHue()));
            AddItem(new Sandals());
            AddItem(new WizardsHat(Utility.RandomBlueHue()));

            SetSkill(SkillName.Healing, 80.0, 100.0);
            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.Magery, 60.0, 80.0);

            // Stats
            Str = 90;
            Dex = 70;
            Int = 100;
            Hits = 80;

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
                Say("Greetings, traveler. I am Hippocrates Oathbringer.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a healer, devoted to the well-being of those who seek my aid.");
            }
            else if (speech.Contains("healing"))
            {
                Say("Healing is both an art and a science. It requires knowledge, skill, and compassion.");
            }
            else if (speech.Contains("oath"))
            {
                Say("The Hippocratic Oath binds me to the ethical practice of medicine. 'First, do no harm.'");
            }
            else if (speech.Contains("doctor"))
            {
                Say("As a doctor, my duty is to heal and to care. Every life is precious.");
            }
            else if (speech.Contains("aid"))
            {
                Say("If you need aid, speak to me of your ailments, and I shall see what I can do.");
            }
            else if (speech.Contains("ailments"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your earnest pursuit of knowledge and virtue, I grant you this Doctor's Bag as a reward.");
                    from.AddToBackpack(new DoctorsBag());
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                base.OnSpeech(e);
            }
        }

        public HippocratesOathbringer(Serial serial) : base(serial) { }

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
