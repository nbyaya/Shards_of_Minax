using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Bartholomew Gearwright")]
    public class BartholomewGearwright : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public BartholomewGearwright() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Bartholomew Gearwright";
            Body = 0x190; // Human male body

            // Stats
            Str = 85;
            Dex = 60;
            Int = 90;
            Hits = 65;

            // Appearance
            AddItem(new Cap() { Hue = Utility.RandomMetalHue() });
            AddItem(new Doublet() { Hue = Utility.RandomMetalHue() });
            AddItem(new LongPants() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new GnarledStaff() { Hue = Utility.RandomMetalHue() });

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
                Say("Greetings! I am Bartholomew Gearwright, the master of tinkering and all things mechanical. Ask me about my work or the marvelous creations I've made.");
            }
            else if (speech.Contains("work"))
            {
                Say("My work revolves around creating intricate gadgets and devices. Every gear and spring has its purpose. What interests you about tinkering?");
            }
            else if (speech.Contains("gadgets"))
            {
                Say("Gadgets are fantastic creations that simplify and enhance our lives. I take great pride in my ingenious designs. Do you have a favorite gadget?");
            }
            else if (speech.Contains("designs"))
            {
                Say("Each design is a blend of artistry and precision. From gears to springs, it's all about making something functional and beautiful. Have you ever designed anything yourself?");
            }
            else if (speech.Contains("function"))
            {
                Say("The function of a gadget is its primary purpose. Each piece must work flawlessly to achieve its goal. Are you curious about how specific gadgets function?");
            }
            else if (speech.Contains("goal"))
            {
                Say("The goal of a well-designed gadget is to make life easier or more enjoyable. Speaking of which, have you ever been interested in tinkering yourself?");
            }
            else if (speech.Contains("tinkering"))
            {
                Say("Tinkering is both an art and a science. It requires creativity, patience, and skill. If you’re interested in tinkering, I might have something special for you.");
            }
            else if (speech.Contains("special"))
            {
                Say("Ah, special indeed! For those who show a genuine interest in tinkering, there are rewards. Have you heard of the Chest of the Master Tinkerer?");
            }
            else if (speech.Contains("chest"))
            {
                Say("The Chest of the Master Tinkerer is filled with marvelous items and treasures, all crafted with care. If you're truly interested, you might just earn one yourself.");
            }
            else if (speech.Contains("earn"))
            {
                Say("Earning the chest requires not just interest but understanding. Show me your curiosity and commitment to tinkering, and I may reward you.");
            }
            else if (speech.Contains("curiosity"))
            {
                Say("Curiosity is the spark that drives discovery and innovation. It's essential for any tinkerer. Are you ready to prove your dedication?");
            }
            else if (speech.Contains("dedication"))
            {
                Say("Dedication to the craft of tinkering is what separates the amateurs from the masters. If you're committed, I'll reward you with something truly special.");
            }
            else if (speech.Contains("masters"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward for you right now. Please return later.");
                }
                else
                {
                    Say("For your dedication and interest in tinkering, I present to you the Chest of the Master Tinkerer. Enjoy the treasures within!");
                    from.AddToBackpack(new TinkeringTreasureChest()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public BartholomewGearwright(Serial serial) : base(serial) { }

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
