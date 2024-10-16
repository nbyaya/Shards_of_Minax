using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Brock Steelbridge")]
    public class BrockSteelbridge : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BrockSteelbridge() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Brock Steelbridge";
            Body = 0x190; // Human male body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 85;
            Hits = 70;

            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateGloves() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new MetalShield() { Hue = Utility.RandomMetalHue() });

            Hue = Utility.RandomSkinHue();
            HairItemID = Utility.RandomList(0x203B, 0x2041); // Short hair
            HairHue = Utility.RandomHairHue();
            FacialHairItemID = Utility.RandomList(0x203C, 0x2045); // Beard or mustache

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
                Say("Ah, greetings! I am Brock Steelbridge, the finest railway worker you'll ever meet.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in top shape, thanks to a lifetime of lifting and hauling!");
            }
            else if (speech.Contains("job"))
            {
                Say("I’m the chief engineer for the railway project. My job is to ensure that every track is laid perfectly.");
            }
            else if (speech.Contains("tracks"))
            {
                Say("Ah, the tracks! They are the veins of our railway system. Without them, the trains couldn’t move.");
            }
            else if (speech.Contains("trains"))
            {
                Say("Trains are the backbone of our transportation. I’ve seen them carry everything from people to precious cargo.");
            }
            else if (speech.Contains("cargo"))
            {
                Say("Precious cargo is what makes this railway worthwhile. It's not just iron and wood; it's the lifeblood of our economy.");
            }
            else if (speech.Contains("economy"))
            {
                Say("Indeed! This railway will drive prosperity across the land. It connects cities and fuels commerce.");
            }
            else if (speech.Contains("prosperity"))
            {
                Say("Prosperity is the goal! Hard work and determination will lead us there.");
            }
            else if (speech.Contains("hard work"))
            {
                Say("Hard work is the key to success. It’s not just about laying tracks; it’s about dedication to the cause.");
            }
            else if (speech.Contains("success"))
            {
                Say("Success comes from perseverance and teamwork. Every rail laid is a step towards a brighter future.");
            }
            else if (speech.Contains("teamwork"))
            {
                Say("Teamwork is essential for any large project. It’s what keeps us on track and moving forward.");
            }
            else if (speech.Contains("project"))
            {
                Say("Our railway project is more than just construction; it's a symbol of progress and unity.");
            }
            else if (speech.Contains("progress"))
            {
                Say("Progress is achieved through collective effort and vision. Our railway is a testament to that.");
            }
            else if (speech.Contains("vision"))
            {
                Say("Vision is what guides us. It’s the dream of a connected land that keeps us working hard.");
            }
            else if (speech.Contains("dream"))
            {
                Say("The dream of a unified land drives us forward. It’s what makes every challenge worth overcoming.");
            }
            else if (speech.Contains("challenge"))
            {
                Say("Challenges are just opportunities in disguise. Overcoming them is what makes our achievements truly valuable.");
            }
            else if (speech.Contains("achievements"))
            {
                Say("Our achievements are not just about building a railway; they’re about creating a legacy for future generations.");
            }
            else if (speech.Contains("legacy"))
            {
                Say("A legacy is built on hard work and dedication. It’s something that lasts long after we’re gone.");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication to a cause ensures that even the hardest tasks are completed with pride.");
            }
            else if (speech.Contains("pride"))
            {
                Say("We take pride in our work because we know it will benefit many for years to come.");
            }
            else if (speech.Contains("benefit"))
            {
                Say("The benefits of our railway will be felt across the land, connecting people and fostering trade.");
            }
            else if (speech.Contains("trade"))
            {
                Say("Trade will flourish with the new railway, bringing prosperity to every corner of the land.");
            }
            else if (speech.Contains("prosperity"))
            {
                Say("Indeed! Prosperity is the ultimate reward for our efforts. It’s what we strive for each day.");
            }
            else if (speech.Contains("reward"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please come back later.");
                }
                else
                {
                    Say("Your understanding of our mission and dedication is commendable. For your efforts, please accept this Railway Worker’s Chest as a token of our gratitude.");
                    from.AddToBackpack(new RailwayWorkersChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BrockSteelbridge(Serial serial) : base(serial) { }

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
