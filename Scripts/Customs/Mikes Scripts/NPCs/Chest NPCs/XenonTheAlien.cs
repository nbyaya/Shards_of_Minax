using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the remains of Xenon the Alien")]
    public class XenonTheAlien : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public XenonTheAlien() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Xenon the Alien";
            Body = 0x190; // Human male body (adjust as needed to fit an alien theme)

            // Stats
            Str = 70;
            Dex = 50;
            Int = 100;
            Hits = 60;

            // Appearance
            AddItem(new Boots() { Hue = 1153 }); // Example hue for a "space" theme
            AddItem(new FancyShirt() { Hue = 1153 });
            AddItem(new LongPants() { Hue = 1153 });
            AddItem(new WizardsHat() { Hue = 1153 }); // Adjust for an alien look

            Hue = Race.RandomSkinHue(); // Adjust or specify a hue for an alien appearance
            HairItemID = 0; // No hair
            HairHue = 0;
            
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
                Say("Greetings, Earthling! I am Xenon the Alien, keeper of cosmic treasures.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as resilient as the stars themselves! I am well.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to safeguard the rarest artifacts from across the galaxy.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("Ah, the Alien Artifact! It is a relic of immense power and mystery. Do you wish to know more?");
            }
            else if (speech.Contains("more"))
            {
                Say("To earn the Alien Artifact, you must first prove your worth. Tell me about your adventures.");
            }
            else if (speech.Contains("adventures"))
            {
                Say("Adventures are the lifeblood of any great hero! Speak to me of your greatest feats, and I may reward you.");
            }
            else if (speech.Contains("feats"))
            {
                Say("Impressive feats are those that challenge the cosmos. If you have tales of bravery, I shall grant you a reward.");
            }
            else if (speech.Contains("bravery"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have already given you a reward recently. Return later to prove your worth again.");
                }
                else
                {
                    Say("Your bravery shines as brightly as the stars. As promised, here is your reward.");
                    from.AddToBackpack(new AlienArtifaxChest()); // Give the Alien Artifact chest
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else
            {
                Say("I do not understand that. Speak of adventures, feats, or bravery, and I may reward you.");
            }

            base.OnSpeech(e);
        }

        public XenonTheAlien(Serial serial) : base(serial) { }

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
