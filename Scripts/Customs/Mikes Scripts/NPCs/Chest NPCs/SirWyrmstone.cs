using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of Sir Wyrmstone")]
    public class SirWyrmstone : BaseCreature
    {
        private bool m_RewardGiven;

        [Constructable]
        public SirWyrmstone() : base(AIType.AI_Vendor, FightMode.None, 10, 1, 0.2, 0.4)
        {
            Name = "Sir Wyrmstone";
            Title = "the Dragon Keeper";
            Body = 0x190; // Male body
            Hue = Utility.RandomSkinHue();
            
            // Appearance
            AddItem(new PlateChest() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateLegs() { Hue = Utility.RandomMetalHue() });
            AddItem(new PlateArms() { Hue = Utility.RandomMetalHue() });
            AddItem(new Helmet() { Hue = Utility.RandomMetalHue() });
            AddItem(new Boots() { Hue = Utility.RandomMetalHue() });

            // Skills
            SetSkill(SkillName.Swords, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 85.0, 100.0);
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
                string speech = e.Speech.ToLower();

                if (speech.Contains("name"))
                {
                    from.SendMessage("I am Sir Wyrmstone, the keeper of dragon lore.");
                }
                else if (speech.Contains("job"))
                {
                    from.SendMessage("My duty is to guard the ancient dragon's treasure and share its lore with worthy seekers.");
                }
                else if (speech.Contains("health"))
                {
                    from.SendMessage("I am in robust health, prepared for the trials of guarding the dragon's hoard.");
                }
                else if (speech.Contains("dragon"))
                {
                    from.SendMessage("Dragons are majestic and fearsome creatures. They guard their treasures fiercely.");
                }
                else if (speech.Contains("hoard"))
                {
                    from.SendMessage("The dragon's hoard is legendary, filled with precious artifacts and rare treasures.");
                }
                else if (speech.Contains("treasure"))
                {
                    from.SendMessage("Treasure is not just gold and jewels, but the wisdom and stories it holds.");
                }
                else if (speech.Contains("reward"))
                {
                    if (CheckRewardConditions(from))
                    {
                        GiveReward(from);
                    }
                    else
                    {
                        from.SendMessage("You must prove your worth to receive the treasure.");
                    }
                }
                else
                {
                    base.OnSpeech(e);
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
                DragonHoardChest chest = new DragonHoardChest();
                from.AddToBackpack(chest);
                from.SendMessage("You have proven yourself worthy. Take this Dragon's Hoard as your reward.");
                m_RewardGiven = true;
            }
        }

        public SirWyrmstone(Serial serial) : base(serial)
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
