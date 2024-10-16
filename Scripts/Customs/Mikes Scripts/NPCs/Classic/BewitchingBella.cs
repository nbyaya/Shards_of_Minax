using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Bewitching Bella")]
    public class BewitchingBella : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BewitchingBella() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Bewitching Bella";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 90;
            Int = 60;
            Hits = 55;

            // Appearance
            AddItem(new FancyDress(2968)); // PlainDress with hue 2968
            AddItem(new Sandals(2128));    // Heels with hue 2128
            AddItem(new GoldBracelet() { Name = "Bella's Bracelet" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this); // Female hair
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
                Say("Greetings, darling. I'm Bewitching Bella, but you knew that, didn't you?");
            }
            else if (speech.Contains("health"))
            {
                Say("My health? Darling, I'm as healthy as a flower in full bloom.");
            }
            else if (speech.Contains("job"))
            {
                Say("My \"job,\" you ask? Well, darling, I specialize in the art of seduction and companionship.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you believe in true love, or are you just here to waste my time?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Oh, a charmer, are we? Well, darling, actions speak louder than words. Prove yourself.");
            }
            else if (speech.Contains("flower"))
            {
                Say("Like a rose, I'm delicate yet resilient. Have you ever seen the rose gardens of Yew? They are a source of much inspiration for me.");
            }
            else if (speech.Contains("seduction"))
            {
                Say("Seduction is an art, my dear. One that I've perfected over time. However, even in my line of work, one must uphold certain principles. The virtue of Honor, for instance. Speaking of which, did you know the first syllable of its mantra is SUMM?");
            }
            else if (speech.Contains("enchantments"))
            {
                Say("Enchantments are everywhere, in the twinkle of stars, the whisper of the winds, and the allure of forbidden spells. Are you curious about spells, darling?");
            }
            else if (speech.Contains("yew"))
            {
                Say("Ah, the town of Yew. It's surrounded by lush forests and has the most mesmerizing trees. Some say they hold secrets of their own. Have you ever wondered about tree secrets?");
            }
            else if (speech.Contains("bewitching"))
            {
                Say("Oh, I see my reputation precedes me. Many have been enchanted by my presence. Do you believe in enchantments, darling?");
            }

            base.OnSpeech(e);
        }

        public override void OnDoubleClick(Mobile from)
        {
            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                Say("I have no reward right now. Please return later.");
            }
            else
            {
                Say("For your thoughtful inquiry, please accept this reward.");
                from.AddToBackpack(new Gold(1000)); // Example reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }

            base.OnDoubleClick(from);
        }

        public BewitchingBella(Serial serial) : base(serial) { }

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
