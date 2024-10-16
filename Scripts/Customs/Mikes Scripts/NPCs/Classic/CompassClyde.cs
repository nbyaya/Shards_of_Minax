using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Compass Clyde")]
    public class CompassClyde : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CompassClyde() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Compass Clyde";
            Body = 0x190; // Human male body

            // Stats
            Str = 108;
            Dex = 53;
            Int = 117;
            Hits = 73;

            // Appearance
            AddItem(new ShortPants() { Hue = 1152 });
            AddItem(new Doublet() { Hue = 44 });
            AddItem(new Shoes() { Hue = 1155 });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

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
                Say("I'm Compass Clyde, the world's greatest cartographer!");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Who cares about health when you're charting the unknown!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I draw maps, of course. But who appreciates the hard work of a cartographer?");
            }
            else if (speech.Contains("maps"))
            {
                Say("Do you have any idea how many times I've been asked to map the same forest? Art thou daft?");
            }
            else if (speech.Contains("challenge"))
            {
                Say("So, you think you could be a cartographer, eh? Tell me, can you even find your way out of this room?");
            }
            else if (speech.Contains("unknown"))
            {
                Say("The unknown is what drives me! There's a cave to the east that no one has ever charted. I've been meaning to go there, but dangers lurk!");
            }
            else if (speech.Contains("cave"))
            {
                Say("Ah, the cave! It's said to be a labyrinth, and legend tells of a hidden treasure within. If you find it and map the cave, bring the map to me.");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Legends whisper of an artifact in that cave - The Compass of Truth. Many have sought it, none returned. Perhaps you're brave enough to seek it out?");
            }
            else if (speech.Contains("compass"))
            {
                Say("An ancient compass said to always point towards one's true desire. It's more than just a tool; it's a symbol of a cartographer's passion!");
            }
            else if (speech.Contains("passion"))
            {
                Say("Being a cartographer isn't just a job; it's a calling! Charting the unknown, facing dangers... all for the thrill of discovery! Do you share this passion?");
            }
            else if (speech.Contains("discovery"))
            {
                Say("Ah, the joy of discovery! The first time you set foot where no one else has, seeing landscapes untouched by civilization. There's no feeling like it.");
            }
            else if (speech.Contains("landscapes"))
            {
                Say("Mountains, forests, rivers... Every landscape has its own charm. But the most mysterious of all? The vastness of the deep blue sea.");
            }
            else if (speech.Contains("sea"))
            {
                Say("The ocean is the last great uncharted territory. I once tried to map its depths, but it's a task too great for one man. Perhaps with help, we could uncover its secrets?");
            }
            else if (speech.Contains("help"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Would you help me map the depths? Bring me a rare coral from the ocean's floor, and I shall reward you handsomely!");
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("coral"))
            {
                Say("The Radiant Coral, it glows with an ethereal light. Only found in the darkest parts of the ocean. If you manage to get one, your reward awaits. A token as a sample!");
                from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                lastRewardTime = DateTime.UtcNow; // Update the timestamp
            }

            base.OnSpeech(e);
        }

        public CompassClyde(Serial serial) : base(serial) { }

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
