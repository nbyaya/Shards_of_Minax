using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Edgar Roni Figaro")]
    public class EdgarRoniFigaro : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public EdgarRoniFigaro() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Edgar Roni Figaro";
            Body = 0x190; // Human male body

            // Stats
            Str = 110;
            Dex = 90;
            Int = 100;
            Hits = 85;

            // Appearance
            AddItem(new PlateLegs() { Hue = 1900 });
            AddItem(new PlateChest() { Hue = 1900 });
            AddItem(new PlateGloves() { Hue = 1900 });
            AddItem(new Cloak() { Hue = 1175 });
            AddItem(new BattleAxe() { Name = "Edgar's Axe" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

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
                Say("Ah, greetings, my friend. I am Edgar Roni Figaro, a man of many talents.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is a secret known only to the wind. Worry not about me.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job, you ask? Let's just say I'm a master of disguise and a lover of the finer things in life.");
            }
            else if (speech.Contains("truth"))
            {
                Say("Life is a masquerade, my friend, and we all wear masks. Do you see the truth behind the facade?");
            }
            else if (speech.Contains("observe"))
            {
                Say("Ah, a keen observer, aren't you? Remember, my friend, sometimes it's what's unsaid that matters most.");
            }
            else if (speech.Contains("talents"))
            {
                Say("Ah, my talents? I've been known to play the lute, dance under the moonlight, and even craft trinkets for the ones I hold dear.");
            }
            else if (speech.Contains("wind"))
            {
                Say("The wind has been my confidant, carrying my secrets far and wide. But not everything is for everyone's ears, some tales are kept close to the heart.");
            }
            else if (speech.Contains("disguise"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Disguises have allowed me to enter places and hear stories unknown to most. It's a risky endeavor, but the thrill is unmatched. Speaking of risks, would you like a small token of gratitude for your interest in my tales?");
                    from.AddToBackpack(new ProvocationAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("facade"))
            {
                Say("The facade is but a veil, my friend. Behind each mask lies a story, a passion, a dream. It's up to us to discern the truth and the lies.");
            }
            else if (speech.Contains("unsaid"))
            {
                Say("The unsaid, the pauses between words, the lingering glances, they all have a tale to tell. It's the silent moments that sometimes scream the loudest truths.");
            }

            base.OnSpeech(e);
        }

        public EdgarRoniFigaro(Serial serial) : base(serial) { }

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
