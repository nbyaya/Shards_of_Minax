using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lila the Explorer")]
    public class LilaTheExplorer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LilaTheExplorer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lila the Explorer";
            Body = 0x191; // Human female body

            // Stats
            Str = 40;
            Dex = 40;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new Skirt() { Hue = 38 });
            AddItem(new FancyShirt() { Hue = 64 });
            AddItem(new Boots() { Hue = 64 });
            AddItem(new FeatheredHat() { Hue = 38 });
            AddItem(new Cloak() { Name = "Lila's Map" });

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
                Say("I'm Lila the Explorer, cartographer extraordinaire.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Ha, who cares about health when you're mapping the unknown?");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job'? I'm a cartographer, in case you haven't been listening. You know, one of those people who draw maps for the ungrateful masses.");
            }
            else if (speech.Contains("exploring"))
            {
                Say("Interested in exploring the wilds, are you? Well then, answer me this: Do you have the guts to face the unknown?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, you're one of those brave souls, huh? We'll see if you're still so fearless after you've explored a few treacherous places.");
            }
            else if (speech.Contains("explorer"))
            {
                Say("Ah, you've noticed my title? Being an explorer isn't just about venturing into uncharted territories, but also about documenting them. My maps have helped many adventurers find their way.");
            }
            else if (speech.Contains("cartographer"))
            {
                Say("As a cartographer, I've journeyed through perilous mountains, dense forests, and crossed stormy seas. There's a certain thrill in bringing the unseen to light on a piece of parchment.");
            }
            else if (speech.Contains("unknown"))
            {
                Say("The unknown is both a curse and a blessing. It's daunting, yes, but the allure of discovery, of being the first to set foot somewhere, is too strong to resist.");
            }
            else if (speech.Contains("maps"))
            {
                // No specific response defined for this keyword in XML
            }
            else if (speech.Contains("trinsic"))
            {
                Say("Ah, Trinsic. A coastal city with a sturdy wall and brave knights. But what lies beyond the city's boundaries is where the true adventure begins.");
            }
            else if (speech.Contains("yew"))
            {
                Say("Yew's forests are filled with mysteries. Some say spirits of ancient guardians still roam there. Always take care and trust in your map.");
            }
            else if (speech.Contains("dangerous"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("I've faced many perils in my travels. Lost in deserts, stranded on islands, and even chased by dragons! But with each challenge, my maps became more detailed and valuable. For your courage, take this - may it serve you well on your adventures.");
                    from.AddToBackpack(new CartographyAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LilaTheExplorer(Serial serial) : base(serial) { }

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
