using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Kain")]
    public class Kain : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Kain() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Kain";
            Body = 0x190; // Human male body

            // Stats
            Str = 120;
            Dex = 120;
            Int = 40;
            Hits = 120;

            // Appearance
            AddItem(new PlateLegs() { Hue = 2335 });
            AddItem(new PlateChest() { Hue = 1745 });
            AddItem(new PlateHelm() { Hue = 1955 });
            AddItem(new PlateGloves() { Hue = 1843 });
            AddItem(new Boots() { Hue = 2322 });
            AddItem(new Halberd() { Name = "Kain's Halberd" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
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
                Say("I am Kain, the Dragoon from Baron Castle.");
            }
            else if (speech.Contains("health"))
            {
                Say("I'm in good shape, ready for battle!");
            }
            else if (speech.Contains("job"))
            {
                Say("My duty is to protect Baron Castle and its people.");
            }
            else if (speech.Contains("battle"))
            {
                Say("The path of a Dragoon is one of honor and valor. Are you familiar with our ways?");
            }
            else if (speech.Contains("yes") && speech.Contains("battle"))
            {
                Say("Then prove your valor in battle, and the Dragoons of Baron will welcome you.");
            }
            else if (speech.Contains("baron"))
            {
                Say("Baron Castle is an ancient stronghold, standing tall for centuries. Many secrets are hidden within its walls.");
            }
            else if (speech.Contains("dragoon"))
            {
                Say("A Dragoon is a noble warrior trained in the art of dragon combat. We leap into the skies and dive onto our foes.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor is not just in fighting bravely but in standing up for what's right, even if one stands alone.");
            }
            else if (speech.Contains("protect"))
            {
                Say("From power-hungry warlords to menacing beasts, the threats are endless. But I swear on my life to keep the castle safe.");
            }
            else if (speech.Contains("secrets"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastRewardTime < cooldown)
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Ah, many have tried to uncover the mysteries of Baron Castle. If you prove trustworthy, perhaps I could share one with you.");
                    from.AddToBackpack(new NeckSlotChangeDeed()); // Give the reward
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("trustworthy"))
            {
                Say("By helping the people of Baron and showing your dedication to the cause. Actions speak louder than words.");
            }
            else if (speech.Contains("help"))
            {
                Say("Many need assistance in these trying times. From fetching medicines to defending against foes, there's always a way to aid.");
            }

            base.OnSpeech(e);
        }

        public Kain(Serial serial) : base(serial) { }

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
