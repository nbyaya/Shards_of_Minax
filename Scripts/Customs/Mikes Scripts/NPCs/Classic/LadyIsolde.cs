using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Lady Isolde")]
    public class LadyIsolde : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public LadyIsolde() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Lady Isolde";
            Body = 0x191; // Human female body

            // Stats
            Str = 155;
            Dex = 65;
            Int = 25;
            Hits = 110;

            // Appearance
            AddItem(new PlateChest() { Hue = 1122 });
            AddItem(new PlateArms() { Hue = 1122 });
            AddItem(new PlateHelm() { Hue = 1122 });
            AddItem(new PlateGloves() { Hue = 1122 });
            AddItem(new ThighBoots() { Hue = 1157 });

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
                Say("I am Lady Isolde, a knight of this wretched realm.");
            }
            else if (speech.Contains("health"))
            {
                Say("Health? Bah! What does it matter? We're all doomed in the end anyway.");
            }
            else if (speech.Contains("job"))
            {
                Say("My so-called 'job' is to uphold the honor of this crumbling kingdom. A futile task if there ever was one.");
            }
            else if (speech.Contains("battles"))
            {
                Say("Valor? Hah! Valor is a fairy tale told to keep us fools fighting in this eternal war. Are you valiant, or just another pawn?");
            }
            else if (speech.Contains("yes"))
            {
                Say("Valiant, you say? Well, if you believe in such nonsense, then never flee unless you're staring death in the face.");
            }
            else if (speech.Contains("realm"))
            {
                Say("This realm was once a beacon of hope, now overshadowed by darkness and despair. But perhaps not all is lost. Perhaps...");
            }
            else if (speech.Contains("doomed"))
            {
                Say("Why are we doomed? Legends speak of a cursed artifact that brought misfortune upon us. I seek it, hoping to reverse our fate.");
            }
            else if (speech.Contains("honor"))
            {
                Say("Honor? In this bleak world, it's all I have left. It's the code that keeps my sword sharp and my spirit unbroken.");
            }
            else if (speech.Contains("kingdom"))
            {
                Say("This kingdom was once glorious, filled with life and laughter. Now it's but a shadow, a haunting reminder of the past.");
            }
            else if (speech.Contains("war"))
            {
                Say("The war has raged for as long as I can remember. It's consumed the best of us, turning friends into foes.");
            }
            else if (speech.Contains("valor"))
            {
                Say("Valor is the spark in a warrior's heart, the fire that keeps us going when all hope seems lost. Do you possess such a spark?");
            }
            else if (speech.Contains("pawn"))
            {
                Say("Many have become pawns in this grand game of power and deceit. I fight to reclaim my agency, my freedom.");
            }
            else if (speech.Contains("artifact"))
            {
                Say("Ah, you've heard of it? If you assist me in finding this cursed artifact, I might just have a reward for you.");
            }
            else if (speech.Contains("assist"))
            {
                Say("Your offer is intriguing. If you truly wish to help, bring me the map of the lost catacombs. In gratitude, I shall bestow upon you a gift.");
                // Example action, give a key item
                from.AddToBackpack(new MaxxiaScroll());
            }
            else if (speech.Contains("map"))
            {
                Say("Legend has it that the map is held by the Oracle of the East. Seek her guidance, and you may find what I seek.");
            }

            base.OnSpeech(e);
        }

        public LadyIsolde(Serial serial) : base(serial) { }

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
