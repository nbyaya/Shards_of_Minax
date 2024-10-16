using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Napoleon Bonaparte")]
    public class NapoleonBonaparte : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public NapoleonBonaparte() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Napoleon Bonaparte";
            Body = 0x190; // Human male body

            // Stats
            Str = 100;
            Dex = 100;
            Int = 80;
            Hits = 70;

            // Appearance
            AddItem(new PlateChest() { Hue = 1150 });
            AddItem(new PlateLegs() { Hue = 1150 });
            AddItem(new PlateGloves() { Hue = 1150 });
            AddItem(new PlateHelm() { Hue = 1150 });
            AddItem(new Boots() { Hue = 1150 });
            AddItem(new Halberd() { Name = "Napoleon's Halberd" });

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
                Say("I am Napoleon Bonaparte, the Emperor of France!");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Pah! I am invincible!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? Conqueror of nations, of course!");
            }
            else if (speech.Contains("battles"))
            {
                Say("Do you think you can measure up to my greatness? Answer me!");
            }
            else if (speech.Contains("yes"))
            {
                Say("Hmph, I thought as much. Go on, insignificant one, and try not to disappoint me further.");
            }
            else if (speech.Contains("no"))
            {
                Say("Hmph, I thought as much. Go on, insignificant one, and try not to disappoint me further.");
            }
            else if (speech.Contains("emperor"))
            {
                Say("Yes, as the Emperor of France, I've seen the rise and fall of many. My legacy is one for the history books!");
            }
            else if (speech.Contains("invincible"))
            {
                Say("Indeed, I believe that with the right strategy, determination, and willpower, one can overcome any challenge!");
            }
            else if (speech.Contains("conqueror"))
            {
                Say("My campaigns have taken me across Europe, from the Pyramids of Egypt to the heart of Russia. Every land has its own tale!");
            }
            else if (speech.Contains("history"))
            {
                Say("History is written by the victors, but even those who write it cannot deny the impact I've had on the world. For good or for bad, my name will forever be remembered.");
            }
            else if (speech.Contains("determination"))
            {
                Say("Without determination, even the most well-laid plans will crumble. It's the fire within that drives us to greatness!");
            }
            else if (speech.Contains("pyramids"))
            {
                Say("Ah, the Pyramids of Egypt! An ancient marvel that even I admired. It reminded me of the eternal nature of legacies.");
            }
            else if (speech.Contains("remembered"))
            {
                Say("Ah, to be remembered is the desire of many. But actions, not words, will be the true testament of one's legacy.");
            }
            else if (speech.Contains("greatness"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Greatness is not given, it is earned. And those who chase it with a burning passion are the ones who achieve it. Speaking of which, I have something for someone as inquisitive as you. Take this!");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("eternal"))
            {
                Say("Eternity is a concept few can grasp. But monuments like the Pyramids stand as a testament to the timeless nature of human achievement.");
            }
            else if (speech.Contains("testament"))
            {
                Say("A testament is the lasting proof of one's deeds. Actions speak louder than words, and their echoes are heard through time.");
            }

            base.OnSpeech(e);
        }

        public NapoleonBonaparte(Serial serial) : base(serial) { }

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
