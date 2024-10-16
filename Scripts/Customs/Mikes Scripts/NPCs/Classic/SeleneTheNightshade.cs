using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Selene the Nightshade")]
    public class SeleneTheNightshade : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SeleneTheNightshade() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Selene the Nightshade";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 130;
            Int = 60;
            Hits = 90;

            // Appearance
            AddItem(new ThighBoots() { Hue = 1171 });
            AddItem(new LeatherBustierArms() { Hue = 1172 });
            AddItem(new Kryss() { Name = "Selene's Dagger" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("I am Selene the Nightshade, a shadow elemental. What do you want?");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? What does it matter to the likes of you?");
            }
            else if (speech.Contains("job"))
            {
                Say("My 'job'? I exist in shadows, not to entertain mortals like you.");
            }
            else if (speech.Contains("battles"))
            {
                Say("If you seek wisdom, answer me this: What do you know of the shadows?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Ha! Ignorance befits a mortal. You have much to learn.");
            }
            else if (speech.Contains("shadows") && !speech.Contains("secrets"))
            {
                Say("Very well, mortal. Seek the secrets of the shadows and return to me when you are enlightened.");
            }
            else if (speech.Contains("selene"))
            {
                Say("Ah, you've heard of me? Not many recognize the name of a shadow elemental. Perhaps there is more to you than meets the eye.");
            }
            else if (speech.Contains("matter"))
            {
                Say("Everything matters in the shadows. Even the slightest flicker of light or momentary lapse of darkness. What do you truly seek?");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The secrets of the shadows are not for the faint-hearted. However, for one who proves worthy, I might offer a reward.");
            }
            else if (speech.Contains("reward"))
            {
                Say("To earn my reward, you must first prove your dedication to the shadows. Return to me with an artifact of pure darkness and I shall bestow upon you something of great value.");
            }
            else if (speech.Contains("elemental"))
            {
                Say("Yes, as a shadow elemental, I am born from the union of darkness and magic. We are rare beings, often misunderstood. Do you seek knowledge or power from me?");
            }
            else if (speech.Contains("flicker"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("In every flicker lies an opportunity. For in the dance between light and dark, fortunes are made and lost. Do you dare dance with the shadows? Take this.");
                    from.AddToBackpack(new ThrowingAugmentCrystal()); // Replace with the actual item to give
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public SeleneTheNightshade(Serial serial) : base(serial) { }

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
