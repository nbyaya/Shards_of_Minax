using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Zana")]
    public class Zana : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Zana() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Zana";
            Body = 0x191; // Human female body

            // Stats
            Str = 80;
            Dex = 80;
            Int = 100;
            Hits = 70;

            // Appearance
            AddItem(new Skirt() { Hue = 1440 });
            AddItem(new Boots() { Hue = 1440 });
            AddItem(new Cloak() { Hue = 1440 });
            AddItem(new MagicWand() { Name = "Zana's Wand" });
			
			Hue = Race.RandomSkinHue();
			HairItemID = Race.RandomHair(Female);
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
                Say("I am Zana, the exiled cartographer.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is none of your concern, exile.");
            }
            else if (speech.Contains("job"))
            {
                Say("I used to explore maps, but now I'm stuck in this wretched place.");
            }
            else if (speech.Contains("dangers"))
            {
                Say("Do you think you can handle the dangers of Wraeclast, exile?");
            }
            else if (speech.Contains("exile"))
            {
                Say("You think you can survive in this forsaken land? We'll see.");
            }
            else if (speech.Contains("zana"))
            {
                Say("I was once respected among my peers, creating intricate maps of uncharted territories. Now, I'm just a memory to them.");
            }
            else if (speech.Contains("concern"))
            {
                Say("Though my physical state may be stable, my spirit is battered by the isolation of this place.");
            }
            else if (speech.Contains("maps"))
            {
                Say("The vast lands I once roamed with excitement have turned into a prison for me. Yet, I still possess some maps of secret places. Help me with a task, and I might share one with you.");
            }
            else if (speech.Contains("territories"))
            {
                Say("The Lost Caves of Elara, the Whispering Forest, the Sunken City... all places I've mapped. But there's one place even I dare not go.");
            }
            else if (speech.Contains("isolation"))
            {
                Say("Being alone in this vast expanse is a curse. I often find solace in my maps, reminiscing about my adventures.");
            }
            else if (speech.Contains("task"))
            {
                Say("There's a particular artifact I've been searching for, known as the Cartographer's Compass. Find it for me, and I'll reward you generously.");
            }
            else if (speech.Contains("compass"))
            {
                Say("Ah, you've heard of it? It's said to reveal hidden paths on maps. Bring it to me, and you shall have your reward.");
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
                    Say("I have many treasures from my travels. Once you bring me the Compass, I promise to give you something of great value. A sample.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public Zana(Serial serial) : base(serial) { }

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
