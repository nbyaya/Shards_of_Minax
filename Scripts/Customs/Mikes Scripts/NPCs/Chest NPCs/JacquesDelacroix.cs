using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Jacques Delacroix")]
    public class JacquesDelacroix : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public JacquesDelacroix() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Jacques Delacroix";
            Body = 0x190; // Male body
            Title = "the Explorer";
            Hue = Utility.RandomSkinHue();

            AddItem(new TricorneHat() { Hue = Utility.RandomGreenHue() });
            AddItem(new Shirt(Utility.RandomBlueHue()));
            AddItem(new LongPants() { Hue = Utility.RandomBlueHue() });
            AddItem(new Boots() { Hue = Utility.RandomBrightHue() });

            SetSkill(SkillName.Anatomy, 60.0, 80.0);
            SetSkill(SkillName.Magery, 70.0, 90.0);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (from.InRange(this, 3))
            {
                string speech = e.Speech.ToLower();

                if (speech.Contains("name"))
                {
                    Say("Greetings, traveler! I am Jacques Delacroix, an explorer of the unknown.");
                }
                else if (speech.Contains("job"))
                {
                    Say("I traverse the lands in search of hidden treasures and forgotten lore.");
                }
                else if (speech.Contains("health"))
                {
                    Say("I am in good health, thanks to the invigorating spirit of exploration!");
                }
                else if (speech.Contains("exploration"))
                {
                    Say("Exploration is the key to uncovering the world's greatest secrets. Are you ready for a challenge?");
                }
                else if (speech.Contains("challenge"))
                {
                    Say("Very well. Prove your worth by solving this puzzle. Speak to me about 'treasure' to begin.");
                }
                else if (speech.Contains("treasure"))
                {
                    Say("Ah, you seek treasure! The path to it is filled with wisdom. Speak to me about 'secrets' to proceed.");
                }
                else if (speech.Contains("secrets"))
                {
                    Say("To uncover the greatest secrets, one must be both brave and wise. Speak of 'legend' to show your true quest.");
                }
                else if (speech.Contains("legend"))
                {
                    if (!m_RewardGiven)
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        Say("You have already proven yourself worthy. Return later for another challenge.");
                    }
                }
                else
                {
                    base.OnSpeech(e);
                }
            }
        }

        private void GiveReward(Mobile from)
        {
            ChamplainTreasureChest chest = new ChamplainTreasureChest();
            from.AddToBackpack(chest);
            from.SendMessage("Congratulations! You have solved the puzzle and proven your worth. Take this Champlain’s Treasure Chest as your reward.");
            m_RewardGiven = true;
        }

        public JacquesDelacroix(Serial serial) : base(serial)
        {
        }

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
