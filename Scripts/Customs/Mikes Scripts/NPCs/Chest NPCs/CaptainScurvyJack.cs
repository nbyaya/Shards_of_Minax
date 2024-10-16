using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Captain Scurvy Jack")]
    public class CaptainScurvyJack : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public CaptainScurvyJack() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Captain Scurvy Jack";
            Title = "the Pirate";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();

            // Stats
            Str = 100;
            Dex = 75;
            Int = 60;
            Hits = 80;

            // Appearance
            AddItem(new TricorneHat() { Hue = Utility.RandomMetalHue() });
            AddItem(new Doublet() { Hue = Utility.RandomRedHue() });
            AddItem(new LongPants() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });
            AddItem(new Scimitar() { Name = "Captain's Cutlass" });
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!from.InRange(this, 3))
                return;

            string speech = e.Speech.ToLower();

            if (speech.Contains("name"))
            {
                Say("Ahoy! I be Captain Scurvy Jack, the scourge of the seven seas!");
            }
            else if (speech.Contains("job"))
            {
                Say("I be plunderin' treasure and navigatin' the treacherous waters of this here world!");
            }
            else if (speech.Contains("health"))
            {
                Say("Arrr, me health be as hearty as a ship's keel, though a bit worn from the waves!");
            }
            else if (speech.Contains("treasure"))
            {
                Say("Ah, ye seek treasure, do ye? The seas hold many secrets and many dangers.");
            }
            else if (speech.Contains("seas"))
            {
                Say("The seas be a wild and unforgiving place. Ye best be prepared for what lies beyond the horizon.");
            }
            else if (speech.Contains("reward"))
            {
                if (!m_RewardGiven)
                {
                    GiveReward(from);
                }
                else
                {
                    Say("I’ve already given ye the treasure, savvy? Best be off before I change me mind!");
                }
            }
            else
            {
                Say("Arrr, I don’t be knowin’ that one. Speak plainly or be gone!");
            }
        }

        private void GiveReward(Mobile from)
        {
            CaptainCooksTreasure chest = new CaptainCooksTreasure();
            from.AddToBackpack(chest);
            from.SendMessage("Ye have proven yerself worthy! Take this Captain Cook’s Treasure as yer reward!");
            m_RewardGiven = true;
        }

        public CaptainScurvyJack(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0); // version
            writer.Write(m_RewardGiven);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_RewardGiven = reader.ReadBool();
        }
    }
}
