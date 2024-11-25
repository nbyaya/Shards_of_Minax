using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a nightshade bramble boss corpse")]
    public class NightshadeBrambleBoss : NightshadeBramble
    {
        [Constructable]
        public NightshadeBrambleBoss() : base("Nightshade Bramble, the Overlord")
        {
            // Enhanced stats to make this a boss-tier version
            SetStr(1200); // Upper strength
            SetDex(255);  // Upper dexterity
            SetInt(250);  // Upper intelligence

            SetHits(12000); // Increase health significantly

            SetDamage(35, 45); // Increase damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90);  // Better physical resistance
            SetResistance(ResistanceType.Fire, 80, 90);      // Better fire resistance
            SetResistance(ResistanceType.Cold, 60, 80);      // Better cold resistance
            SetResistance(ResistanceType.Poison, 100);       // Max poison resistance
            SetResistance(ResistanceType.Energy, 60, 80);    // Better energy resistance

            SetSkill(SkillName.Anatomy, 50.0, 100.0);  // Enhance skill ranges
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Enhance skill ranges
            SetSkill(SkillName.Magery, 100.0, 120.0);  // Enhance skill ranges
            SetSkill(SkillName.Meditation, 50.0, 100.0); // Enhance skill ranges
            SetSkill(SkillName.MagicResist, 150.0);     // Max skill for magic resist
            SetSkill(SkillName.Tactics, 120.0);         // Enhance skill ranges
            SetSkill(SkillName.Wrestling, 120.0);       // Enhance skill ranges

            Fame = 30000;  // Increase Fame for a boss-tier
            Karma = -30000; // Increase Karma (negative for the boss)

            VirtualArmor = 100;  // Increase virtual armor for higher defense

            Tamable = false; // Make sure it's untamable for the boss version
            ControlSlots = 0; // Not tamable by players

            // Attach a random ability for extra challenge
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss logic could be added here if needed
        }

        public NightshadeBrambleBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
