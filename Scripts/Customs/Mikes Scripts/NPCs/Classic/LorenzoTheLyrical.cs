using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lorenzo the Lyrical")]
    public class LorenzoTheLyrical : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LorenzoTheLyrical() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lorenzo the Lyrical";
            Body = 0x190; // Human male body

            // Stats
            Str = 40;
            Dex = 80;
            Int = 80;
            Hits = 60;

            // Appearance
            AddItem(new FancyShirt() { Hue = 1153 });
            AddItem(new LongPants() { Hue = 1153 });
            AddItem(new Boots() { Hue = 1153 });
            AddItem(new FeatheredHat() { Hue = 1153 });
            AddItem(new Lute() { Name = "Lorenzo's Lute" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Initialize the lastRewardTime to a past time
            lastRewardTime = DateTime.MinValue;

            // Speech Hue
            SpeechHue = 0; // Default speech hue
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Lorenzo the Lyrical, a rogue with a silver tongue!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is as unpredictable as a gambler's luck, my friend.");
            }
            else if (speech.Contains("job"))
            {
                Say("My craft is in the shadows, dancing with the moonlight and whispering secrets in the dark. I am a rogue.");
            }
            else if (speech.Contains("virtue"))
            {
                Say("Life is a game of chance, my friend. What virtue do you think guides a rogue like me?");
            }
            else if (speech.Contains("balance"))
            {
                Say("Interesting choice. Remember, even in the shadows, there is room for virtue. Seek the balance, my friend.");
            }
            else if (speech.Contains("silver"))
            {
                Say("Ah, my silver tongue! It's been my savior in more than one tight spot. Words can be mightier than the sword, if used correctly. Have you ever been swayed by a tale or song?");
            }
            else if (speech.Contains("gambler"))
            {
                Say("Gambling is a game of chance and fate. I've had my fair share of losses, but the thrill of the game always brings me back. Do you feel fate has a hand in your life?");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows are where I thrive. Hidden from prying eyes, it's where I find my peace and conduct my business. Ever felt the allure of the unseen, the mysterious?");
            }
            else if (speech.Contains("moonlight"))
            {
                Say("Moonlight has always been my guide, casting the world in a gentle, silvery glow. It's the time when the world is at its most honest, don't you think?");
            }
            else if (speech.Contains("dark"))
            {
                Say("In the dark, one can find both danger and opportunity. It's a time when the world sleeps, but rogues like me are wide awake. Tell me, do you fear the dark or embrace it?");
            }
            else if (speech.Contains("virtue") && speech.Contains("balance"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, the eternal struggle to find balance in one's life. Too much of anything can lead to ruin. For those who can maintain balance, I've been known to offer a token of appreciation.");
                    from.AddToBackpack(new SwordsmanshipAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public LorenzoTheLyrical(Serial serial) : base(serial) { }

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
