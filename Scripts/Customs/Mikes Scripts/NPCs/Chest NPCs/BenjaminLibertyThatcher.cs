using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Benjamin 'Liberty' Thatcher")]
    public class BenjaminLibertyThatcher : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BenjaminLibertyThatcher() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Benjamin 'Liberty' Thatcher";
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
            AddItem(new TricorneHat() { Hue = 1157 });

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
                Say("Greetings, I am Benjamin 'Liberty' Thatcher, a defender of freedom.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in excellent health, as is fitting for a champion of liberty.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role is to spread the ideals of freedom and ensure our revolutionary cause prospers.");
            }
            else if (speech.Contains("revolution"))
            {
                Say("The revolution is the fight for freedom, a noble cause indeed. Do you have the spirit to aid in this quest?");
            }
            else if (speech.Contains("spirit"))
            {
                Say("Spirit is what drives us forward. It is the fire in our hearts that fuels our quest for freedom.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom is the right of all men. It is a flame that burns bright in the hearts of those who dare to fight for it.");
            }
            else if (speech.Contains("flame"))
            {
                Say("A flame that burns in the dark, guiding us through the shadows of tyranny and oppression.");
            }
            else if (speech.Contains("tyranny"))
            {
                Say("Tyranny is the absence of liberty. It is the heavy hand that stifles our voices and seeks to oppress the people.");
            }
            else if (speech.Contains("oppression"))
            {
                Say("Oppression breeds dissent and strife. It is the very thing we fight against with our revolution.");
            }
            else if (speech.Contains("dissent"))
            {
                Say("Dissent is the voice of the oppressed. It is the challenge to the status quo that demands change.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("To challenge is to confront the old order and strive for a new one, one based on justice and equality.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is the cornerstone of our new nation. It ensures that all men are treated with fairness and respect.");
            }
            else if (speech.Contains("nation"))
            {
                Say("Our nation is built on the ideals of freedom and equality. It is a land where liberty prevails.");
            }
            else if (speech.Contains("liberty"))
            {
                Say("Liberty is the cornerstone of our new nation. It represents our struggle and our triumph.");
            }
            else if (speech.Contains("chest"))
            {
                Say("Ah, you seek the chest of revolution! To earn it, you must first prove your resolve.");
            }
            else if (speech.Contains("resolve"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("Your resolve is commendable. For your dedication to the cause, accept this Revolutionary Chest as a token of our gratitude.");
                    from.AddToBackpack(new RevolutionaryChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication to the cause is what wins the fight for freedom. We must stay true to our principles.");
            }
            else if (speech.Contains("principles"))
            {
                Say("Principles are the foundation of our beliefs. They guide our actions and decisions in the pursuit of liberty.");
            }
            else if (speech.Contains("beliefs"))
            {
                Say("Beliefs are deeply held convictions that shape our understanding of the world and our role in it.");
            }
            else if (speech.Contains("understanding"))
            {
                Say("Understanding is key to empathy and connection. It allows us to see beyond our own perspectives and appreciate others' experiences.");
            }
            else if (speech.Contains("perspectives"))
            {
                Say("Perspectives are the lenses through which we view the world. They shape our thoughts and influence our actions.");
            }
            else if (speech.Contains("thoughts"))
            {
                Say("Thoughts are the seeds of our actions. What we think influences what we do and how we interact with others.");
            }
            else if (speech.Contains("actions"))
            {
                Say("Actions speak louder than words. It is through our deeds that we make a real impact on the world.");
            }
            else if (speech.Contains("impact"))
            {
                Say("The impact we make is a reflection of our values and efforts. It can inspire others and drive change.");
            }
            else if (speech.Contains("inspire"))
            {
                Say("To inspire is to ignite the spark of motivation and passion in others. It is a powerful force for change.");
            }
            else if (speech.Contains("motivation"))
            {
                Say("Motivation fuels our endeavors and drives us to achieve our goals, even in the face of adversity.");
            }
            else if (speech.Contains("adversity"))
            {
                Say("Adversity tests our resolve and strength. It is through overcoming challenges that we grow and succeed.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success is the result of perseverance and hard work. It is a reward for our efforts and a testament to our commitment.");
            }

            base.OnSpeech(e);
        }

        public BenjaminLibertyThatcher(Serial serial) : base(serial) { }

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
