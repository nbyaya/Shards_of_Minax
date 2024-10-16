using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Emperor Claudius")]
    public class EmperorClaudius : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EmperorClaudius() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Emperor Claudius";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 60;
            Int = 100;
            Hits = 80;

            // Appearance
            AddItem(new Robe() { Hue = 1308 }); // Robe with specific hue
            AddItem(new PlateHelm() { Name = "Emperor Claudius' Crown" });
            AddItem(new Mace() { Name = "Emperor Claudius' Scepter" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this); // Assuming this method for hair is available
            HairHue = Race.RandomHairHue();

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
                Say("I am Emperor Claudius, ruler of this land!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is irrelevant, for I am the embodiment of this realm's strength.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to maintain the balance of power in this realm.");
            }
            else if (speech.Contains("balance"))
            {
                Say("True power is not in the crown, but in the hearts of the people. Do you understand this?");
            }
            else if (speech.Contains("yes") && Insensitive.Contains(e.Speech, "balance"))
            {
                Say("Indeed, your words reflect wisdom. Perhaps there is hope for this realm yet.");
            }
            else if (speech.Contains("land"))
            {
                Say("This land has been my domain for many generations. It has seen prosperity and despair, and through it all, I've tried to guide it to greatness.");
            }
            else if (speech.Contains("realm"))
            {
                Say("This realm derives its strength from the unity and dedication of its people. As they prosper, so do I.");
            }
            else if (speech.Contains("power"))
            {
                Say("Power is a responsibility, not a privilege. It requires one to lead with both wisdom and compassion. It's not just about ruling, but understanding those you rule.");
            }
            else if (speech.Contains("prosperity"))
            {
                Say("Prosperity is not just about wealth or comfort, but the happiness and contentment of the people. It's a sign of a successful reign.");
            }
            else if (speech.Contains("unity"))
            {
                Say("In unity, we find our greatest strength. When the people of this realm stand together, no adversity is too great.");
            }
            else if (speech.Contains("responsibility"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("With great power comes great responsibility. It is my duty to ensure the safety and well-being of every citizen in this realm. For your dedication to our land, please accept this token of gratitude.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversities test our resolve, but overcoming them brings growth and enlightenment. We must always face challenges head-on.");
            }
            else if (speech.Contains("bonds"))
            {
                Say("The bonds of friendship and kinship are the foundation of any great society. Nurture them, and they will support you in return.");
            }
            else if (speech.Contains("challenges"))
            {
                Say("Every challenge we overcome makes us stronger. It is the way of life, and it shapes our destiny.");
            }

            base.OnSpeech(e);
        }

        public EmperorClaudius(Serial serial) : base(serial) { }

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
