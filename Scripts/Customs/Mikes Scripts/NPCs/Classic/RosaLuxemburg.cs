using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rosa Luxemburg")]
    public class RosaLuxemburg : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RosaLuxemburg() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rosa Luxemburg";
            Body = 0x191; // Human female body

            // Stats
            Str = 85;
            Dex = 70;
            Int = 95;
            Hits = 74;

            // Appearance
            AddItem(new Skirt() { Hue = 1912 });
            AddItem(new PlainDress() { Hue = 1104 });
            AddItem(new Boots() { Hue = 1104 });
            AddItem(new WarFork() { Name = "Revolutionary's Spear" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Rosa Luxemburg, a champion of the proletariat!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is of no concern when there's a class struggle to fight!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to liberate the oppressed and challenge the bourgeoisie!");
            }
            else if (speech.Contains("class struggle"))
            {
                Say("The workers' struggle for justice and equality is the only battle that matters. Do you understand the plight of the proletariat?");
            }
            else if (speech.Contains("join"))
            {
                Say("Your words reveal your true intentions. Will you join the proletariat in the fight for a just society?");
            }
            else if (speech.Contains("proletariat"))
            {
                Say("The proletariat are the workers, the oppressed, those who labor but do not own the means of production. They are at the heart of the revolution.");
            }
            else if (speech.Contains("concern"))
            {
                Say("My only concern is for the working class. Their suffering under the oppressive system is what drives me each day.");
            }
            else if (speech.Contains("liberate"))
            {
                Say("To liberate means to set free. I aim to free the minds and hearts of the oppressed from the chains that bind them.");
            }
            else if (speech.Contains("justice"))
            {
                Say("Justice is more than just laws. It is about equal rights, opportunities, and a fair distribution of resources. It's a dream for many, but a goal for us.");
            }
            else if (speech.Contains("society"))
            {
                Say("A just society is one where every individual has an equal chance at happiness, success, and prosperity. But to achieve it, we need to challenge the existing power structures.");
            }
            else if (speech.Contains("revolution"))
            {
                Say("The revolution is not a one-time event. It is a continuous process of challenging the status quo and striving for a world where everyone can thrive.");
            }
            else if (speech.Contains("chains"))
            {
                Say("These chains are not just physical, but mental. Many are bound by the chains of ignorance, prejudice, and fear. It's our duty to break them.");
            }
            else if (speech.Contains("thrive"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Your understanding warms my heart. Here, take this as a token of gratitude for being an ally in our cause.");
                    from.AddToBackpack(new DetectingHiddenAugmentCrystal()); // Replace with the actual reward item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
        }

        public RosaLuxemburg(Serial serial) : base(serial) { }

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
