using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Virelia the Swamp Whisperer")]
    public class VireliaTheSwampWhisperer : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public VireliaTheSwampWhisperer() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Virelia the Swamp Whisperer";
            Body = 0x191; // Human female body

            // Stats
            Str = 50;
            Dex = 60;
            Int = 140;
            Hits = 80;

            // Appearance
            AddItem(new Skirt() { Hue = 1162 });
            AddItem(new BodySash() { Hue = 1161 });
            AddItem(new GnarledStaff() { Name = "Virelia's Marsh Staff" });
			
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
                Say("I am Virelia the Swamp Whisperer, child of the waters.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in harmony with the swamp's vitality.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am the Whisperer of the Swamp, attuned to its secrets.");
            }
            else if (speech.Contains("swamp") || speech.Contains("essence") || speech.Contains("waters"))
            {
                Say("Within the murk and mire, one finds the true essence of oneself. Have you sought your inner waters, traveler?");
            }
            else if (speech.Contains("journey") || speech.Contains("seek") || speech.Contains("reveal"))
            {
                Say("Your response speaks of your journey's intent. The swamp reveals much to those who listen. What is it you seek?");
            }
            else if (speech.Contains("virelia"))
            {
                Say("Ah, you speak my name with curiosity. My lineage is deeply connected with the swamp's ancient spirits.");
            }
            else if (speech.Contains("vitality"))
            {
                Say("The vitality of the swamp is unlike any other. It has an energy that heals and sustains. Many have sought its power, but only few truly understand.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("The swamp holds many secrets, some dark and others enlightening. In my whispers, I try to unveil them to the worthy.");
            }
            else if (speech.Contains("essence"))
            {
                Say("The essence of the swamp is a mirror of our souls, reflecting both the murky doubts and the clear truths. Few dare to gaze upon it.");
            }
            else if (speech.Contains("inner"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("If it's guidance you seek, then listen to the symphony of the swamp. It might just guide your way. Here, take this charm, it may aid you.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("reveal"))
            {
                Say("The swamp reveals the history of the land, the stories of lost souls, and the mysteries of nature. But one must be patient to decipher its tales.");
            }

            base.OnSpeech(e);
        }

        public VireliaTheSwampWhisperer(Serial serial) : base(serial) { }

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
