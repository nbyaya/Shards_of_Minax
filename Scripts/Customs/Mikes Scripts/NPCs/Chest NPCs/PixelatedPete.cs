using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Pixelated Pete")]
    public class PixelatedPete : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public PixelatedPete() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Pixelated Pete";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new FancyShirt() { Name = "Retro Arcade Shirt", Hue = Utility.RandomNeutralHue() });
            AddItem(new LongPants() { Hue = Utility.RandomMetalHue() });
            AddItem(new Sandals() { Hue = Utility.RandomBrightHue() });
            AddItem(new Cap() { Hue = Utility.RandomNondyedHue() });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = -1; // No facial hair

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
                Say("Hey there! I'm Pixelated Pete, the arcade aficionado. Have you ever played Pac-Man?");
            }
            else if (speech.Contains("pac-man"))
            {
                Say("Pac-Man is a classic! Navigating the maze and eating pellets. Speaking of classics, have you tried Space Invaders?");
            }
            else if (speech.Contains("space invaders"))
            {
                Say("Space Invaders is a timeless shooter. Did you know that the golden age of arcade games brought many such legends?");
            }
            else if (speech.Contains("golden age"))
            {
                Say("The golden age of arcade games includes titles like Pac-Man and Space Invaders. They're real treasures of gaming history.");
            }
            else if (speech.Contains("treasures"))
            {
                Say("Ah, the treasures of gaming! Every arcade cabinet is like a treasure chest full of memories. Speaking of treasures, have you ever found an arcade relic?");
            }
            else if (speech.Contains("arcade relic"))
            {
                Say("Arcade relics are fascinating. They remind us of the history and evolution of gaming. And now, a relic of my own: a Retro Arcade Chest!");
            }
            else if (speech.Contains("chest"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I've already given out my reward for now. Come back later for more retro fun!");
                }
                else
                {
                    Say("For your enthusiasm about arcade history and relics, here's a Retro Arcade Chest packed with classic surprises!");
                    from.AddToBackpack(new RetroArcadeChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("surprises"))
            {
                Say("The surprises in the chest will remind you of the golden days of arcades. Enjoy exploring the chest!");
            }
            else if (speech.Contains("explore"))
            {
                Say("Exploring the chest will lead you to treasures of the arcade world. Each item has a story. Have fun and may the pixels be with you!");
            }

            base.OnSpeech(e);
        }

        public PixelatedPete(Serial serial) : base(serial) { }

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
