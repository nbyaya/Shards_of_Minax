using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the grill overlord")]
    public class GrillMasterBoss : GrillMaster
    {
        [Constructable]
        public GrillMasterBoss() : base()
        {
            Name = "Grill Overlord";
            Title = "the Supreme Grill Master";

            // Update stats to match or exceed Barracoon for a tougher boss
            SetStr(1200); // Upper limit of strength
            SetDex(255); // Upper limit of dexterity
            SetInt(250); // Upper limit of intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(25, 40); // Increased damage range to make the fight more challenging

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            SetResistance(ResistanceType.Physical, 75, 85); // Boosted physical resistance
            SetResistance(ResistanceType.Fire, 75, 85); // Boosted fire resistance
            SetResistance(ResistanceType.Cold, 60, 75); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 100); // Maintained poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Boosted energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resistance
            SetSkill(SkillName.Tactics, 100.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 100.0); // Enhanced wrestling
            SetSkill(SkillName.Magery, 120.0); // Increased magery skill
            SetSkill(SkillName.EvalInt, 110.0); // Increased evalint

            Fame = 25000; // Increased fame for the boss
            Karma = -25000; // Negative karma for the boss

            VirtualArmor = 80; // Enhanced virtual armor

            // Attach a random ability to this boss
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

        public GrillMasterBoss(Serial serial) : base(serial)
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
