using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Orglok the Mystic")]
    public class OrglokTheMystic : BaseCreature
    {
        private DateTime lastInteractionTime;

        [Constructable]
        public OrglokTheMystic() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Orglok the Mystic";
            Body = 0x191; // Orc body

            // Stats
            Str = 90;
            Dex = 70;
            Int = 110;
            Hits = 80;

            // Appearance
            AddItem(new BoneArms() { Hue = 2212 });
            AddItem(new BoneLegs() { Hue = 2212 });
            AddItem(new BoneChest() { Hue = 2212 });
            AddItem(new BoneGloves() { Hue = 2212 });
            AddItem(new OrcMask() { Hue = 2213 });
            AddItem(new QuarterStaff() { Name = "Orglok's Staff" });

            Hue = Race.RandomSkinHue();
            HairItemID = Race.RandomHair(this);
            HairHue = Race.RandomHairHue();

            SpeechHue = 0; // Default speech hue

            // Initialize the lastInteractionTime to a past time
            lastInteractionTime = DateTime.MinValue;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("I am Orglok the Mystic, seeker of hidden truths.");
            }
            else if (speech.Contains("health"))
            {
                Say("I am in harmony with the energies of this world.");
            }
            else if (speech.Contains("job"))
            {
                Say("I commune with the spirits and seek the wisdom of the ancients.");
            }
            else if (speech.Contains("wisdom"))
            {
                Say("The path to enlightenment is paved with humility and self-reflection. Art thou humble?");
            }
            else if (speech.Contains("yes"))
            {
                TimeSpan cooldown = TimeSpan.FromMinutes(10);
                if (DateTime.UtcNow - lastInteractionTime < cooldown)
                {
                    Say("I have no further wisdom to share right now. Please return later.");
                }
                else
                {
                    Say("Then seek inner peace and balance, for the truth lies within.");
                    lastInteractionTime = DateTime.UtcNow; // Update the timestamp
                }
            }

            base.OnSpeech(e);
        }

        public OrglokTheMystic(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(lastInteractionTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            lastInteractionTime = reader.ReadDateTime();
        }
    }
}
