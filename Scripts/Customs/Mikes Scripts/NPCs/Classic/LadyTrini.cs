using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Trini")]
    public class LadyTrini : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyTrini() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Trini";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 120;
            Int = 60;
            Hits = 80;

            // Appearance
            AddItem(new RingmailLegs() { Hue = 53 });
            AddItem(new RingmailChest() { Hue = 53 });
            AddItem(new RingmailGloves() { Hue = 53 });
            AddItem(new ChainCoif() { Hue = 53 });
            AddItem(new Boots() { Hue = 53 });
            AddItem(new Dagger() { Name = "Lady Trini's Dagger" });

            // Random Hair and Hue
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
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
                Say("Greetings, traveler. I am Lady Trini.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My calling is that of a scholar and a seeker of knowledge.");
            }
            else if (speech.Contains("virtues"))
            {
                Say("True wisdom lies in understanding the eight virtues: Honesty, Compassion, Valor, Justice, Sacrifice, Honor, Spirituality, and Humility.");
            }
            else if (speech.Contains("yes"))
            {
                Say("Do you seek to better understand the virtues, traveler?");
            }
            else if (speech.Contains("library"))
            {
                Say("The ancient library is a vast repository of wisdom. Within its walls, there lies a hidden chamber.");
            }
            else if (speech.Contains("tome"))
            {
                Say("That tome contained the secrets of alchemy. If you find it, I'd be grateful enough to offer you a reward.");
            }
            else if (speech.Contains("caves"))
            {
                Say("The Caves of Despair are said to be cursed. I went there searching for a crystal of immense power.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Demonstrating valor often means facing one's fears. There's an artifact known as the Shield of Courage that embodies this virtue.");
            }
            else if (speech.Contains("chamber"))
            {
                Say("The hidden chamber in the library is said to be protected by an ancient spell. Only those with pure intent can access it.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("For your thoughtful inquiry, please accept this token of my gratitude.");
                    from.AddToBackpack(new LegsSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("crystal"))
            {
                Say("This crystal is said to have the ability to amplify magical energies. But be warned, many have sought it and never returned.");
            }
            else if (speech.Contains("shield"))
            {
                Say("The Shield of Courage was lost in the Catacombs of Regret. It's said to protect its bearer from all fears.");
            }
            else if (speech.Contains("spell"))
            {
                Say("The spell on the chamber is said to be a test. Only those who have studied the virtues thoroughly can break it.");
            }

            base.OnSpeech(e);
        }

        public LadyTrini(Serial serial) : base(serial) { }

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
