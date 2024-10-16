using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Ragged Ronny")]
    public class RaggedRonny : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RaggedRonny() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Ragged Ronny";
            Body = 0x190; // Human male body

            // Stats
            Str = 38;
            Dex = 29;
            Int = 20;
            Hits = 43;

            // Appearance
            AddItem(new Kilt() { Hue = 52 });
            AddItem(new Shirt() { Hue = 1102 });
            AddItem(new ThighBoots() { Hue = 1153 });

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
                Say("I am Ragged Ronny, a beggar by trade.");
            }
            else if (speech.Contains("job"))
            {
                Say("Life on the streets, friend, it's a constant battle.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Compassion, friend, that's the virtue I hold dear.");
            }
            else if (speech.Contains("virtue compassion"))
            {
                Say("Compassion is the light that guides us through the darkest alleys.");
            }
            else if (speech.Contains("virtue compassion yes") || speech.Contains("virtue compassion no"))
            {
                Say("Do you possess the virtue of compassion, my friend?");
            }
            else if (speech.Contains("health"))
            {
                Say("My body may be frail, but my spirit remains strong. The streets have hardened me.");
            }
            else if (speech.Contains("streets"))
            {
                Say("The streets have taught me more about life than any book. Every cobblestone has a story if you're willing to listen.");
            }
            else if (speech.Contains("compassion"))
            {
                Say("Compassion is not just a virtue but a way of life. It's the reason I share what little I have with others on the streets. Did you know the mantra of Sacrifice starts with 'CAH'?");
            }
            else if (speech.Contains("spirit"))
            {
                Say("My spirit has been my guiding force. Even in the harshest winters, it has kept me warm and given me hope.");
            }
            else if (speech.Contains("cobblestone"))
            {
                Say("Each cobblestone has felt the weight of both the pauper and the king. They've witnessed acts of kindness and moments of cruelty.");
            }
            else if (speech.Contains("mantra"))
            {
                Say("Ah, the mantras. Each one tied to a virtue. The mantra of Sacrifice, as I mentioned, starts with 'CAH'. But to truly understand it, one must live it.");
            }
            else if (speech.Contains("winters"))
            {
                Say("The cold winds and snow have tested my resolve, but I've always found shelter, thanks to the kindness of strangers.");
            }
            else if (speech.Contains("kindness"))
            {
                Say("Kindness, even a simple act, can change someone's day. A coin, a loaf of bread, or just a listening ear can make a world of difference.");
            }
            else if (speech.Contains("live"))
            {
                Say("To live is not just to exist. It's to experience, to share, and to give. Even in my situation, I find moments of joy and purpose.");
            }
            else if (speech.Contains("ragged"))
            {
                Say("Many call me a mere beggar, but I've seen more of the world from the streets than most in castles.");
            }
            else if (speech.Contains("virtues ponder"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                    from.AddToBackpack(new Gold(100)); // Example reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public RaggedRonny(Serial serial) : base(serial) { }

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
