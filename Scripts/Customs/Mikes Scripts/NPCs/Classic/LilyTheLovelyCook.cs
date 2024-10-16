using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lily the Lovely Cook")]
    public class LilyTheLovelyCook : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilyTheLovelyCook() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lily the Lovely Cook";
            Body = 0x191; // Human female body

            // Stats
            Str = 70;
            Dex = 50;
            Int = 70;
            Hits = 45;

            // Appearance
            AddItem(new LongPants() { Hue = 2128 });
            AddItem(new FancyShirt() { Hue = 2128 });
            AddItem(new Sandals() { Hue = 2128 });
            AddItem(new HalfApron() { Hue = 38 });
            AddItem(new SkullCap() { Name = "Lily's Cooking Cap" });
            AddItem(new Spoon() { Name = "Lily's Stirring Spoon" });

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
            string speech = e.Speech.ToLower();

            if (!from.InRange(this, 3))
                return;

            if (speech.Contains("name"))
            {
                Say("Greetings, dear traveler. I am Lily the Lovely Cook.");
            }
            else if (speech.Contains("health"))
            {
                Say("Oh, my health? I'm as hearty as a stew simmering over an open flame.");
            }
            else if (speech.Contains("job"))
            {
                Say("My humble job? I am the cook of this fine establishment, serving up delectable dishes.");
            }
            else if (speech.Contains("battles"))
            {
                Say("But do you know, traveler, the true test of character isn't in the kitchen, but in the world beyond. Have you faced your battles?");
            }
            else if (speech.Contains("yes") && Insensitive.Contains(e.Speech, "courage"))
            {
                Say("Indeed, one must have courage to face life's challenges. Do you possess that courage?");
            }
            else if (speech.Contains("lily"))
            {
                Say("Ah, Lily, a name my dear mother gave me. She loved the lily flowers that grew by our old cottage.");
            }
            else if (speech.Contains("stew"))
            {
                Say("Stew! Ah, I've made many in my time. My favorite is the one with fresh herbs and meat from the northern valleys.");
            }
            else if (speech.Contains("dishes"))
            {
                Say("My dishes? They're inspired by my travels. I've been to distant lands and tasted their unique flavors.");
            }
            else if (speech.Contains("world"))
            {
                Say("The world is vast and mysterious. I've heard tales of far-off places with strange creatures and hidden treasures.");
            }
            else if (speech.Contains("treasures"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Treasures aren't just gold and jewels, dear traveler. Sometimes, the greatest treasures are recipes passed down through generations. Speaking of which, for your curiosity, here's a little reward from my collection.");
                    from.AddToBackpack(new GrandmastersRobe()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("recipes"))
            {
                Say("Oh, my recipes? They are stories in themselves. Each ingredient has its own tale, and every dish tells a story of its own.");
            }

            base.OnSpeech(e);
        }

        public LilyTheLovelyCook(Serial serial) : base(serial) { }

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
