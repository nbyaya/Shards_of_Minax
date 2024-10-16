using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Faye the Pixie")]
    public class FayeThePixie : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public FayeThePixie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Faye the Pixie";
            Body = 0x191; // Human female body
            
            // Stats
            Str = 50;
            Dex = 90;
            Int = 80;
            Hits = 40;

            // Appearance
            AddItem(new Kilt() { Hue = 1163 });
            AddItem(new FemaleLeatherChest() { Hue = 1164 });
            AddItem(new PlateGloves() { Hue = 1165 });
            AddItem(new FireballWand() { Name = "Faye's Wand" });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(Female);
            HairHue = Race.RandomHairHue();

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
                Say("Greetings, traveler! I am Faye the Pixie, a creature of the woods.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am but a fleeting spirit of the forest, untouched by the passage of time.");
            }
            else if (speech.Contains("job"))
            {
                Say("My role in this ancient forest is to observe and protect its secrets.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("Do you seek the wisdom of the forest, traveler? If so, answer me this: What is the whisper of leaves?");
            }
            else if (speech.Contains("whisper of leaves"))
            {
                Say("Your answer is poetic, traveler. You may seek the forest's wisdom when you need it most.");
            }
            else if (speech.Contains("woods"))
            {
                Say("The woods are ancient, filled with memories of past eras and beings who once tread its paths. Some say they're enchanted, and if you listen closely, you might hear tales whispered by the wind.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Many tales lie hidden within the trees, shadows, and streams of this forest. Secrets of forgotten magic, lost travelers, and hidden realms. I am here to ensure they remain undisturbed.");
            }
            else if (speech.Contains("observe"))
            {
                Say("I've witnessed countless seasons change, creatures being born and passing on, and the dance of nature in its most primal form. But of late, there have been disturbances, dark shadows that shouldn't be.");
            }
            else if (speech.Contains("ancient"))
            {
                Say("The forest is as old as time itself, having witnessed the dawn of the world. Its roots go deep, and its branches have touched the sky for eons.");
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
                    Say("Ah, wise traveler! For those who show true understanding and respect for the forest, there is indeed a reward. Here, take this. It's a small token, but it might aid you on your journey.");
                    from.AddToBackpack(new LightningAugmentCrystal()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public FayeThePixie(Serial serial) : base(serial) { }

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
