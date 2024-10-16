using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class SirReginaldBastille : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public SirReginaldBastille() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Reginald Bastille";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            Title = "the Custodian";

            // Equip the NPC with some medieval-themed gear
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateHelm() { Hue = Utility.RandomMetalHue() });
            AddItem(new Longsword() { Hue = Utility.RandomMetalHue() });
            AddItem(new OrderShield(Utility.RandomMetalHue()));
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            return true;
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!m_RewardGiven && from.InRange(this, 3))
            {
                switch (e.Speech.ToLower())
                {
                    case "name":
                        from.SendMessage("Greetings, noble adventurer. I am Sir Reginald Bastille, the Custodian of European Relics.");
                        break;
                    case "job":
                        from.SendMessage("I am tasked with guarding and preserving the relics of our great European heritage.");
                        break;
                    case "health":
                        from.SendMessage("I am in splendid health, thanks to the blessings of valor and honor.");
                        break;
                    case "relics":
                        from.SendMessage("Ah, the relics of Europe are steeped in history and mystery. To earn one, you must prove your worth.");
                        break;
                    case "worthy":
                        from.SendMessage("Proving one's worth is not an easy task. Speak to me of valor and you may earn the treasure I guard.");
                        break;
                    case "valor":
                        from.SendMessage("Valor is the heart of a true hero. It is through bravery and noble deeds that one can claim true rewards.");
                        break;
                    case "treasure":
                        if (CheckRewardConditions(from))
                        {
                            GiveReward(from);
                        }
                        else
                        {
                            from.SendMessage("You must display true valor before receiving such a treasure.");
                        }
                        break;
                    default:
                        base.OnSpeech(e);
                        break;
                }
            }
        }

        private bool CheckRewardConditions(Mobile from)
        {
            // Placeholder for reward conditions
            return true;
        }

        private void GiveReward(Mobile from)
        {
            if (!m_RewardGiven)
            {
                EuropeanRelicsChest chest = new EuropeanRelicsChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven your valor and worth. Take this European Relics Chest as a token of your bravery.");
                m_RewardGiven = true;
            }
        }

        public SirReginaldBastille(Serial serial) : base(serial)
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
