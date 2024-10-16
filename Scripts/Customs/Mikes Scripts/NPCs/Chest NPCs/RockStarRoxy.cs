using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Rock Star Roxy")]
    public class RockStarRoxy : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public RockStarRoxy() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Rock Star Roxy";
            Body = 0x190; // Human female body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new LeatherChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new LeatherLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new Bandana() { Name = "Rock Star's Bandana", Hue = Utility.RandomBrightHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new SkullCap() { Hue = Utility.RandomMetalHue() });
            
            Hue = Race.RandomSkinHue(); // Skin hue
            HairItemID = Race.RandomHair(this); // Random hair
            HairHue = Race.RandomHairHue(); // Random hair hue

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
                Say("Hey there, I'm Rock Star Roxy! Ready to rock and roll?");
            }
            else if (speech.Contains("rock"))
            {
                Say("Rock 'n' roll is a way of life! It's about breaking the mold and living loud. What do you know about the genre?");
            }
            else if (speech.Contains("genre"))
            {
                Say("The genre of rock includes many styles, but grunge is my favorite. Have you heard of it?");
            }
            else if (speech.Contains("grunge"))
            {
                Say("Grunge is the spirit of rebellion and raw emotion. It’s a powerful genre with a lot to say. What's your favorite band?");
            }
            else if (speech.Contains("band"))
            {
                Say("Ah, bands! Nirvana and Pearl Jam are legendary. Do you have a favorite song?");
            }
            else if (speech.Contains("song"))
            {
                Say("Great songs have the power to move us. Do you know the lyrics to any grunge hits?");
            }
            else if (speech.Contains("lyrics"))
            {
                Say("Lyrics are the heart of music. Can you recite any famous lyrics?");
            }
            else if (speech.Contains("famous"))
            {
                Say("Famous lyrics often capture the essence of a time. What about the grunge era speaks to you?");
            }
            else if (speech.Contains("era"))
            {
                Say("The grunge era was marked by a raw and powerful sound. It's a symbol of a time when music spoke louder than words. Would you like to learn more?");
            }
            else if (speech.Contains("learn"))
            {
                Say("To learn more, you need to explore the depths of rock. But before that, do you know what a cache is?");
            }
            else if (speech.Contains("cache"))
            {
                Say("A cache is a hidden store of treasures. Speaking of which, I have a special reward for those who understand the spirit of rock. Are you interested?");
            }
            else if (speech.Contains("interested"))
            {
                Say("Fantastic! For your dedication to rock and roll, I present to you the Grunge Rocker's Cache! It’s filled with legendary items!");
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I’ve already given out the reward recently. Check back later for another chance!");
                }
                else
                {
                    from.AddToBackpack(new GrungeRockersCache()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("spirit"))
            {
                Say("The spirit of rock is about freedom and expression. It's a powerful force. Can you feel it?");
            }
            else if (speech.Contains("freedom"))
            {
                Say("Freedom in rock means breaking the rules and making your own way. What rules would you break for a rock star moment?");
            }
            else if (speech.Contains("rules"))
            {
                Say("Rules are meant to be challenged. Sometimes breaking them leads to the most memorable moments. What memorable moments have you had?");
            }
            else if (speech.Contains("memorable"))
            {
                Say("Memorable moments are the ones we cherish. Rock 'n' roll has given us countless moments to remember. What's your most cherished memory?");
            }
            else if (speech.Contains("cherished"))
            {
                Say("Cherished memories are like treasured songs—timeless and unforgettable. Do you have any timeless favorites?");
            }

            base.OnSpeech(e);
        }

        public RockStarRoxy(Serial serial) : base(serial) { }

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
