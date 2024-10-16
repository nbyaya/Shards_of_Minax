using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Lancelot")]
    public class SirLancelot : BaseCreature
    {
        private DateTime lastRewardTime;

        [Constructable]
        public SirLancelot() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Lancelot";
            Body = 0x190; // Human male body

            // Stats
            Str = 158;
            Dex = 66;
            Int = 26;
            Hits = 113;

            // Appearance
            AddItem(new ChainChest() { Hue = 1400 });
            AddItem(new ChainLegs() { Hue = 1400 });
            AddItem(new PlateHelm() { Hue = 1400 });
            AddItem(new PlateGloves() { Hue = 1400 });
            AddItem(new Boots() { Hue = 1400 });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

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
                Say("I am Sir Lancelot, a knight of noble lineage.");
            }
            else if (speech.Contains("health"))
            {
                Say("My health is robust, as befits a knight of the realm.");
            }
            else if (speech.Contains("job"))
            {
                Say("I serve the kingdom as a protector and defender.");
            }
            else if (speech.Contains("valor"))
            {
                Say("True valor lies not only in the strength of one's arm but in the purity of one's heart. Dost thou value valor?");
            }
            else if (speech.Contains("yes") && LastSpeechContains("valor"))
            {
                Say("Then thou art on the path of honor. Remember, a true knight never wavers in the face of adversity.");
            }
            else if (speech.Contains("lineage"))
            {
                Say("My lineage traces back to the ancient knights who served the realm with honor and courage.");
            }
            else if (speech.Contains("robust"))
            {
                Say("Being robust isn't solely about physical strength, but also mental resilience. I often meditate on the mantra of Sacrifice to help me maintain both.");
            }
            else if (speech.Contains("protector"))
            {
                Say("As a protector, I've faced many perils and vanquished numerous foes. It's essential for knights like me to always be prepared.");
            }
            else if (speech.Contains("purity"))
            {
                Say("Purity of heart comes from understanding one's self and others. I've learned this through countless quests and challenges.");
            }
            else if (speech.Contains("mantra"))
            {
                Say("Ah, the mantra of Sacrifice... I've whispered its syllables many times. The second syllable is ZEN, though it holds more power when combined with the others.");
            }
            else if (speech.Contains("perils"))
            {
                Say("Every peril I've faced has taught me a lesson. Even in the darkest moments, there's always a glimmer of hope and a lesson to be learned.");
            }
            else if (speech.Contains("quests"))
            {
                Say("My quests have taken me to the far reaches of the realm. In each journey, I've sought to uphold the virtues and bring justice where it's needed.");
            }
            else if (speech.Contains("knights"))
            {
                Say("The ancient knights were paragons of virtue and valor. Their tales inspire me to this day, reminding me of the noble legacy I uphold.");
            }

            base.OnSpeech(e);
        }

        private bool LastSpeechContains(string keyword)
        {
            // Implement a method to check if the last speech contains the keyword
            // This is a placeholder method, you'll need to create the actual implementation.
            return false;
        }

        public SirLancelot(Serial serial) : base(serial) { }

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
