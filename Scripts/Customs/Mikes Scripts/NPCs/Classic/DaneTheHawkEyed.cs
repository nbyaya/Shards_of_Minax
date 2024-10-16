using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Dane the Hawk-Eyed")]
    public class DaneTheHawkEyed : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public DaneTheHawkEyed() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Dane the Hawk-Eyed";
            Body = 0x190; // Human male body

            // Stats
            Str = 105;
            Dex = 70;
            Int = 50;
            Hits = 135;

            // Appearance
            AddItem(new LongPants(1172));
            AddItem(new HalfApron(1132));
            AddItem(new Shoes(1155));
            AddItem(new RepeatingCrossbow() { Name = "Dane's Crossbow" });

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
                Say("I am Dane the Hawk-Eyed, a master archer!");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is in excellent condition!");
            }
            else if (speech.Contains("job"))
            {
                Say("My job is to protect this realm with my trusty bow and arrow.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the courage to stand against adversity. Do you possess valor?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Valor is a rare trait. Use it wisely, for it defines a hero.");
            }
            else if (speech.Contains("archer"))
            {
                Say("I've honed my skills from the eastern mountains, where the winds whisper tales of legends. Have you heard of the Whispering Winds?");
            }
            else if (speech.Contains("condition"))
            {
                Say("Indeed, my daily regimen involves training in the woods, observing the habits of eagles. Have you ever trained with the eagles?");
            }
            else if (speech.Contains("bow"))
            {
                Say("My bow and arrow are crafted by the renowned blacksmith, Goran. His works are unparalleled. Do you know of him?");
            }
            else if (speech.Contains("hero"))
            {
                Say("A hero's path is filled with trials. If you prove your valor by assisting me, I shall reward you. Will you help a fellow protector?");
            }
            else if (speech.Contains("task"))
            {
                Say("There's a den of wolves to the west, causing trouble for travelers. Clear them out, and your valor shall be proven.");
            }
            else if (speech.Contains("wolves"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("You have shown true valor! As promised, here is your reward. May it serve you well on your journey.");
                    from.AddToBackpack(new MaxxiaScroll()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public DaneTheHawkEyed(Serial serial) : base(serial) { }

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
