using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Cloak Charlie")]
    public class CloakCharlie : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public CloakCharlie() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Cloak Charlie";
            Body = 0x190; // Human male body

            // Stats
            Str = 80;
            Dex = 70;
            Int = 85;
            Hits = 80;

            // Appearance
            AddItem(new BoneLegs() { Hue = 1285 });
            AddItem(new Kryss());
            AddItem(new Cloak() { Hue = 1285 });
            
            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();
            FacialHairItemID = Race.RandomFacialHair(this);

            SpeechHue = 0; // Default speech hue

            lastRewardTime = DateTime.MinValue; // Initialize reward timer
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Cloak Charlie, a rogue skilled in shadows and secrets.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is but a shadow of what it once was, hidden from prying eyes.");
            }
            else if (speech.Contains("job"))
            {
                Say("My job? I dance in the moon's shadow and unravel the threads of fate.");
            }
            else if (speech.Contains("secrets"))
            {
                Say("Are you a seeker of hidden truths and secrets, or merely a wanderer in the night?");
            }
            else if (speech.Contains("ally"))
            {
                Say("If you seek the path of shadows, remember that silence is your ally.");
            }
            else if (speech.Contains("shadows"))
            {
                Say("The shadows have been my solace. They have whispered tales of distant lands and ancient secrets. Do you wish to learn the art of the shadow?");
            }
            else if (speech.Contains("shadow"))
            {
                Say("The shadow, while a place of refuge, can also be a place of peril. Many have sought its depth, but few return. Have you faced the dangers of the dark?");
            }
            else if (speech.Contains("moon"))
            {
                Say("The moon's shadow holds many mysteries. One of them is a treasure I once hid. But be warned, retrieving it requires great skill. Are you up for the challenge?");
            }
            else if (speech.Contains("hidden"))
            {
                Say("Hidden truths are often the most powerful. I've discovered a few in my time, and for a price, I might share one with you. Do you have what it takes to trade?");
            }
            else if (speech.Contains("silence"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("In silence, one can hear the faintest of whispers, the softest of secrets. In return for your discretion, I offer you this reward.");
                    from.AddToBackpack(new MaxxiaScroll()); // Replace with your actual item
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public CloakCharlie(Serial serial) : base(serial) { }

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
