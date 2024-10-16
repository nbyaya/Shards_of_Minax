using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Pennyless Pete")]
    public class PennylessPete : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PennylessPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pennyless Pete";
            Body = 0x190; // Human male body

            // Stats
            Str = 35;
            Dex = 35;
            Int = 25;
            Hits = 42;

            // Appearance
            AddItem(new ShortPants() { Hue = 33 });
            AddItem(new Doublet() { Hue = 44 });
            AddItem(new Shoes() { Hue = 1109 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("Ye lookin' at Pennyless Pete, the wretched beggar.");
            }
            else if (speech.Contains("health"))
            {
                Say("Me health? What health? I'm a beggar, lad. Health's for the rich and fortunate.");
            }
            else if (speech.Contains("job"))
            {
                Say("Me job? Ha! Me job is askin' folks like ye for a bit o' coin to keep me belly from singin' the song of starvation.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Think ye're better off than me, eh? Answer me this: What's the point of wealth if ye can't help those in need?");
            }
            else if (speech.Contains("yes"))
            {
                Say("If ye say so, but remember, even the mightiest fall eventually. Help a beggar out, and maybe ye'll earn a bit of good fortune yourself.");
            }
            else if (speech.Contains("wretched"))
            {
                Say("Aye, I be wretched and cursed by the winds of fate. But once, long ago, I held a secret that now is lost to me.");
            }
            else if (speech.Contains("fortunate"))
            {
                Say("Fortunate? Ha! I once knew a mage who claimed that true fortune lay in the words of mantras. Spoke of 'em all the time, he did.");
            }
            else if (speech.Contains("coin"))
            {
                Say("Sometimes when folks give me coin, they whisper secrets. Once, a hooded figure told me the third syllable of the mantra of Justice is LOR.");
            }
            else if (speech.Contains("secret"))
            {
                Say("My secret? Well, I once held a piece of a map that led to untold riches. Alas, it was stolen from me, and I've been on this cursed path ever since.");
            }
            else if (speech.Contains("mage"))
            {
                Say("That mage, always muttering about the balance of the world. Said he'd found the meaning of life in the echo of spells.");
            }
            else if (speech.Contains("hooded"))
            {
                Say("That hooded figure, always lurking in the shadows. I've seen him at the old ruins to the east, always up to some shady business.");
            }
            else if (speech.Contains("map"))
            {
                Say("That map, it's said to lead to the lair of a mighty dragon, guarded by creatures from the abyss. But who knows? It's lost to me now.");
            }
            else if (speech.Contains("balance"))
            {
                Say("Balance is like a dance, always shifting, always changing. That mage taught me that the balance of the world is fragile and should be treated with care.");
            }

            base.OnSpeech(e);
        }

        public override void OnDoubleClick(Mobile from)
        {
            base.OnDoubleClick(from);

            TimeSpan cooldown = TimeSpan.FromMinutes(10);
            if (DateTime.UtcNow - lastRewardTime < cooldown)
            {
                Say("I have no reward right now. Please return later.");
            }
            else
            {
                Say("Deep reflection on virtues is essential for one's personal growth. In recognizing them, we shape our destiny. For your thoughtful inquiry, please accept this reward.");
                from.AddToBackpack(new Gold(1)); // Example reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }
        }

        public PennylessPete(Serial serial) : base(serial) { }

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
