using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mr. Lancealot")]
    public class MrLancealot : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public MrLancealot() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mr. Lancealot";
            Body = 0x190; // Human male body

            // Stats
            Str = 60;
            Dex = 80;
            Int = 80;
            Hits = 30;

            // Appearance
            AddItem(new LeatherLegs() { Hue = 1153 });
            AddItem(new LeatherChest() { Hue = 1153 });
            AddItem(new LeatherGloves() { Hue = 1153 });
            AddItem(new LeatherCap() { Hue = 1153 });
            AddItem(new Boots() { Hue = 1153 });
            AddItem(new Lance() { Name = "Mr. Lancealot's Lance" });

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
                Say("Greetings, traveler. I am Mr. Lancealot, a scientist.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in perfect health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to unravel the mysteries of the cosmos.");
            }
            else if (speech.Contains("cosmos") || speech.Contains("mysteries") || speech.Contains("knowledge"))
            {
                Say("The secrets of the universe are hidden in plain sight. Do you seek knowledge?");
            }
            else if (speech.Contains("yes") || speech.Contains("greatest question"))
            {
                Say("Ah, a curious mind! Tell me, what is the greatest question you ponder?");
            }
            else if (speech.Contains("universe"))
            {
                Say("The universe is vast and infinite, with more galaxies than grains of sand on a beach. In my studies, I've come across a peculiar anomaly. Would you like to know more?");
            }
            else if (speech.Contains("anomaly"))
            {
                Say("Ah, indeed! The anomaly I've discovered is a rift in the fabric of space, leading to an alternate dimension. There, I believe hidden truths await. I've been working on a device to explore it.");
            }
            else if (speech.Contains("device"))
            {
                Say("I call it the 'Dimensional Key'. A tool that can open doors to other realms. But to complete it, I need a rare crystal found only in the treacherous caves of Gloomshade. If you retrieve it for me, I shall reward you handsomely.");
            }
            else if (speech.Contains("gloomshade"))
            {
                Say("Gloomshade is a cave system to the east, notorious for the dangers that lurk within. But the crystals inside are said to possess powerful energy. Tread with caution should you decide to venture there.");
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
                    Say("Ah, always an incentive, isn't it? Retrieve the crystal, and I shall grant you a piece of knowledge lost to time and perhaps something more tangible to aid you in your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("knowledge"))
            {
                Say("Knowledge is the greatest treasure. In my vast studies, I've uncovered tales of lost civilizations, forgotten magics, and ancient artifacts. Every answer leads to more questions, such is the beauty of the cosmos.");
            }
            else if (speech.Contains("civilizations"))
            {
                Say("Long ago, there were civilizations that harnessed the power of the stars, bending reality to their will. Their ruins can still be found, hidden in the corners of the world, waiting for someone to decipher their secrets.");
            }
            else if (speech.Contains("magic"))
            {
                Say("Magic is the essence that binds the universe. While many consider it a force of old, there are still places where it thrives, untouched by the ravages of time. Seek them out, and you might find power beyond imagination.");
            }
            else if (speech.Contains("artifacts"))
            {
                Say("Ancient artifacts, relics from bygone eras, are scattered throughout the land. Some bestow great power, while others carry curses. Be wary but know that possessing one could change your destiny. Take this friend.");
                from.AddToBackpack(new MaxxiaScroll()); // Reward for asking about artifacts
            }

            base.OnSpeech(e);
        }

        public MrLancealot(Serial serial) : base(serial) { }

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
