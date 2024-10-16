using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Groovy McVibe")]
    public class GroovyMcVibe : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public GroovyMcVibe() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Groovy McVibe";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Hue = Utility.RandomBrightHue() });
            AddItem(new LongPants() { Hue = Utility.RandomBrightHue() });
            AddItem(new Sandals() { Hue = Utility.RandomBrightHue() });
            AddItem(new JesterHat() { Hue = Utility.RandomBrightHue() });
            AddItem(new BodySash() { Hue = Utility.RandomBrightHue() });
            AddItem(new Cloak() { Hue = Utility.RandomBrightHue() });
            AddItem(new Bandana() { Hue = Utility.RandomBrightHue() });
            
            // Customizations
            Hue = 2337;
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            
            // Speech Hue
            SpeechHue = Utility.RandomNeutralHue(); // A neutral hue for speech
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Hey there, I'm Groovy McVibe, the spirit of fun and good vibes!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in great shape, feeling groovy and full of energy!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Just spreading good vibes and peace wherever I go, man.");
            }
            else if (speech.Contains("vibes"))
            {
                Say("It's all about the positive vibes, dude! Can you feel the groove?");
            }
            else if (speech.Contains("groove"))
            {
                Say("Groove is what makes the world go 'round! Stay groovy and keep the peace.");
            }
            else if (speech.Contains("peace"))
            {
                Say("Peace is the ultimate vibe. It's all about harmony and chill.");
            }
            else if (speech.Contains("chill"))
            {
                Say("Chill is the essence of relaxation. Let go of stress and embrace the calm.");
            }
            else if (speech.Contains("relaxation"))
            {
                Say("Relaxation is key to a peaceful mind. Embrace it and let go of worries.");
            }
            else if (speech.Contains("worries"))
            {
                Say("Worries can be heavy, but letting them go brings freedom and joy.");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom is like dancing without boundaries. It's the ultimate form of expression.");
            }
            else if (speech.Contains("dancing"))
            {
                Say("Dancing is a celebration of life. It’s how you express the groove within.");
            }
            else if (speech.Contains("celebration"))
            {
                Say("Celebration brings people together. It’s a collective experience of joy and unity.");
            }
            else if (speech.Contains("unity"))
            {
                Say("Unity is the essence of harmony. Together, we can achieve anything.");
            }
            else if (speech.Contains("harmony"))
            {
                Say("Harmony is a balance between all elements. It brings peace and contentment.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance in life is crucial. It helps in maintaining peace and avoiding chaos.");
            }
            else if (speech.Contains("chaos"))
            {
                Say("Chaos disrupts harmony. Finding order amidst chaos is a true skill.");
            }
            else if (speech.Contains("order"))
            {
                Say("Order brings structure. It's essential for a peaceful and organized life.");
            }
            else if (speech.Contains("structure"))
            {
                Say("Structure helps in creating stability. It’s the foundation of a well-organized life.");
            }
            else if (speech.Contains("stability"))
            {
                Say("Stability provides a sense of security. It’s essential for mental and emotional well-being.");
            }
            else if (speech.Contains("security"))
            {
                Say("Security is about feeling safe and protected. It’s the backbone of a peaceful existence.");
            }
            else if (speech.Contains("existence"))
            {
                Say("Existence is the state of being. Embrace it fully and live life to the fullest.");
            }
            else if (speech.Contains("live"))
            {
                Say("To live fully is to experience every moment with joy and appreciation.");
            }
            else if (speech.Contains("joy"))
            {
                Say("Joy is the essence of a fulfilled life. It’s found in the little things and the big moments.");
            }
            else if (speech.Contains("fulfilled"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("Hang loose, man! Come back later for your reward.");
                }
                else
                {
                    Say("You've got the right vibe, traveler. Here’s a Groovy Vibes Chest to keep the good times rolling!");
                    from.AddToBackpack(new GroovyVibesChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public GroovyMcVibe(Serial serial) : base(serial) { }

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
