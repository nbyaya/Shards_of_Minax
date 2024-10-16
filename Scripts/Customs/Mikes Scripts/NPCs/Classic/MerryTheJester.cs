using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Merry the Jester")]
    public class MerryTheJester : BaseCreature
    {
        private DateTime lastGiftTime;

        [Constructable]
        public MerryTheJester() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Merry the Jester";
            Body = 0x190; // Human male body

            // Stats
            Str = 50;
            Dex = 50;
            Int = 50;
            Hits = 50;

            // Appearance
            AddItem(new FancyShirt() { Hue = 2213 });
            AddItem(new FloppyHat() { Hue = 2213 });
            
            // Hair and skin
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            // Speech Hue
            SpeechHue = 0; // Default speech hue

            // Initialize the lastGiftTime to a past time
            lastGiftTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Welcome, dear traveler! I am Merry the Jester, here to brighten your day with humor and riddles!");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good spirits, as always!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to make people laugh and think! I'm a jester, after all.");
            }
            else if (speech.Contains("riddle"))
            {
                Say("Why did the chicken cross the road? To find Lord British!");
            }
            else if (speech.Contains("humor"))
            {
                Say("Great choice! Here's a jest for you: Why don't scientists trust atoms? Because they make up everything!");
            }
            else if (speech.Contains("merry"))
            {
                Say("Ah, my name carries stories from towns near and far. Ever heard of the 'Great Jest of Luna'?");
            }
            else if (speech.Contains("spirits"))
            {
                Say("Spirits remind me of a potion I once drank. It made me see everything upside down for a whole day! Have you ever tried any potions?");
            }
            else if (speech.Contains("jester"))
            {
                Say("Ah, being a jester is not just about fun and games. Sometimes, we keep important secrets. In fact, I know a secret about the 'Ancient Medallion'.");
            }
            else if (speech.Contains("british"))
            {
                Say("Lord British? Ah, he once challenged me to a duel of wits! We both shared riddles and jests. Have you met the good lord?");
            }
            else if (speech.Contains("atoms"))
            {
                Say("Atoms are fascinating! Speaking of which, have you ever heard of the 'Mystical Element'? It's said to have magical properties!");
            }
            else if (speech.Contains("luna"))
            {
                Say("Luna, a beautiful city with walls that shimmer in moonlight. I performed there and received a gift that I no longer need. Would you like it?");
            }
            else if (speech.Contains("potions"))
            {
                Say("Potions can be tricky. Once, a witch gave me a potion that was supposed to make me invisible, but it only made my nose disappear! Ever dealt with witches?");
            }
            else if (speech.Contains("medallion"))
            {
                Say("The Ancient Medallion is said to hold immense power. Legend says it's hidden in the 'Enchanted Forest'. But, tread with caution, brave adventurer.");
            }
            else if (speech.Contains("duel"))
            {
                Say("A duel of wits with Lord British is no easy task. He's as sharp as they come! But in the end, he awarded me with a 'Golden Feather' for my cleverness.");
            }
            else if (speech.Contains("element"))
            {
                Say("The Mystical Element is rumored to be the core of all magic. Few have seen it, and fewer have harnessed its power. Would you be brave enough to seek it?");
            }
            else if (speech.Contains("gift"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastGiftTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, for someone as curious as you, here it is! [Merry hands you a small package]. May it serve you well in your adventures.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastGiftTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("witch"))
            {
                Say("Witches can be unpredictable. There's one named 'Elara' who is known for her peculiar brews. If you ever cross paths, be wary of her concoctions.");
            }

            base.OnSpeech(e);
        }

        public MerryTheJester(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastGiftTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastGiftTime = reader.ReadDateTime();
        }
    }
}
