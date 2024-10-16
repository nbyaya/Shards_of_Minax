using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Scientist Tesla")]
    public class ScientistTesla : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public ScientistTesla() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Scientist Tesla";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 65;
            Int = 115;
            Hits = 65;

            // Appearance
            AddItem(new LongPants() { Hue = 1157 });
            AddItem(new Doublet() { Hue = 1158 });
            AddItem(new Shoes() { Hue = 0 });
            AddItem(new FeatheredHat() { Hue = 1159 });
            AddItem(new FireballWand() { Name = "Tesla's Innovations" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I am Scientist Tesla, a brilliant mind wasted on this wretched society!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? What does it matter? I'm a mere pawn in this wretched game of life.");
            }
            else if (speech.Contains("job"))
            {
                Say("My so-called 'job' is to toil away in obscurity, conducting experiments that no one appreciates.");
            }
            else if (speech.Contains("discoveries"))
            {
                Say("Do you have any idea how many groundbreaking discoveries I've made, only to have them ignored by the masses?");
            }
            else if (speech.Contains("mock"))
            {
                Say("So, are you here to mock me too, or do you actually care about science?");
            }
            else if (speech.Contains("brilliant"))
            {
                Say("Ah, you recognize brilliance when you see it! In a world where ignorance prevails, a sharp mind is a rarity.");
            }
            else if (speech.Contains("pawn"))
            {
                Say("Yes, like a pawn on a chessboard, forever moved by the hands of fate. However, I've decided to carve my own path, regardless of the obstacles.");
            }
            else if (speech.Contains("experiments"))
            {
                Say("My experiments are more than mere scientific endeavors. They are a pursuit of truth in an age of deception. Once, I even managed to harness pure energy, but my peers ridiculed me.");
            }
            else if (speech.Contains("groundbreaking"))
            {
                Say("Groundbreaking, indeed! One of my proudest achievements was an apparatus that could communicate across vast distances. Yet, it was deemed 'impossible' by the naysayers.");
            }
            else if (speech.Contains("science"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, science! The beacon of hope in a world filled with darkness. If you truly care about it, I have something for you. As a token of appreciation for a fellow enthusiast, take this.");
                    from.AddToBackpack(new PoisoningAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public ScientistTesla(Serial serial) : base(serial) { }

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
