using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Liberty")]
    public class CaptainLiberty : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CaptainLiberty() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Liberty";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomNeutralHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomNeutralHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomNeutralHue() });

            // Add unique items
            AddItem(new FancyShirt() { Hue = Utility.RandomRedHue() });
            AddItem(new Boots() { Hue = Utility.RandomNeutralHue() });

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
                Say("I am Captain Liberty, a defender of freedom and justice.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in fine health, ready to fight for the cause.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to lead the charge against tyranny and oppression.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom is the most precious of all gifts. It is worth fighting for.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is the foundation of any free society. We must uphold it at all costs.");
            }
            else if (speech.Contains("fight"))
            {
                Say("To fight is to stand for what is right. Do you have the courage to join the cause?");
            }
            else if (speech.Contains("courage"))
            {
                Say("Courage is essential in the face of adversity. Do you have the fortitude to persist?");
            }
            else if (speech.Contains("fortitude"))
            {
                Say("Fortitude is the strength to endure hardships. It is what keeps us steadfast in our mission.");
            }
            else if (speech.Contains("mission"))
            {
                Say("Our mission is to bring freedom to the oppressed and to fight against tyranny.");
            }
            else if (speech.Contains("oppressed"))
            {
                Say("The oppressed are those who suffer under tyranny. We must be their champions.");
            }
            else if (speech.Contains("tyranny"))
            {
                Say("Tyranny can only be defeated through unity and determination. We must remain vigilant.");
            }
            else if (speech.Contains("unity"))
            {
                Say("Unity is our greatest strength. Together, we can overcome any obstacle.");
            }
            else if (speech.Contains("obstacle"))
            {
                Say("An obstacle is merely a challenge to overcome. With unity, nothing is insurmountable.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges test our resolve and determination. They are opportunities to prove our mettle.");
            }
            else if (speech.Contains("resolve"))
            {
                Say("Resolve is the commitment to our cause, despite difficulties. It is crucial for success.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success is the result of perseverance and dedication. Your determination has not gone unnoticed.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication to our cause is what drives us forward. It is the foundation of all our efforts.");
            }
            else if (speech.Contains("efforts"))
            {
                Say("Our efforts are what build the path to freedom. Every action counts in this grand endeavor.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom is the ultimate reward for our struggles. It is the light that guides us in the dark.");
            }
            else if (speech.Contains("light"))
            {
                Say("Light represents hope and clarity. In the midst of darkness, it shows us the way.");
            }
            else if (speech.Contains("way"))
            {
                Say("The way forward is through courage and unity. Follow this path, and you shall find the reward.");
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
                    Say("Your journey through the dialogue has proven your dedication. For your perseverance, accept this Revolutionary Relic Chest as a token of our gratitude.");
                    from.AddToBackpack(new RevolutionaryRelicChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("Speak the words of freedom, justice, and courage. Only then will you find the answers you seek.");
            }

            base.OnSpeech(e);
        }

        public CaptainLiberty(Serial serial) : base(serial) { }

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
