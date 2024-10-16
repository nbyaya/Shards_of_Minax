using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Mulan")]
    public class Mulan : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public Mulan() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Mulan";
            Body = 0x190; // Human female body

            // Stats
            Str = 90;
            Dex = 90;
            Int = 60;
            Hits = 70;

            // Appearance
            AddItem(new ChainChest() { Hue = 1197 });
            AddItem(new ChainLegs() { Hue = 1197 });
            AddItem(new ChainCoif() { Hue = 1197 });
            AddItem(new PlateGloves() { Hue = 1197 });
            AddItem(new Boots() { Hue = 1197 });
            AddItem(new Bow() { Name = "Mulan's Bow" });

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
                Say("I am Mulan, a traveler from distant lands.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in good health, thank you for asking.");
            }
            else if (speech.Contains("job"))
            {
                Say("I am a traveler and a storyteller.");
            }
            else if (speech.Contains("wisdom"))
            {
                if (DateTime.UtcNow - lastRewardTime < TimeSpan.FromMinutes(10))
                {
                    Say("I have no reward right now. Please return later.");
                }
                else
                {
                    Say("Then may your path be guided by the stars, and may you find what you seek.");
                    from.AddToBackpack(new Gold(1000)); // Example reward, adjust as needed
                    lastRewardTime = DateTime.UtcNow; // Update the timestamp
                }
            }
            else if (speech.Contains("adventure"))
            {
                Say("May your journey be filled with wonder, kind traveler.");
            }

            base.OnSpeech(e);
        }

        public Mulan(Serial serial) : base(serial) { }

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
