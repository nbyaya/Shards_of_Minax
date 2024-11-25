using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the oracle overlord")]
    public class OracleBoss : Oracle
    {
        [Constructable]
        public OracleBoss() : base()
        {
            Name = "Oracle Overlord";
            Title = "the Visionary";

            // Update stats to match or exceed Barracoon's level
            SetStr(800); // Higher than original strength
            SetDex(150); // Higher than original dexterity
            SetInt(400); // Higher than original intelligence

            SetHits(7000); // Significantly higher than original health
            SetDamage(15, 30); // Higher damage range

            // Update resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.Magery, 120.0); // Stronger magery skill
            SetSkill(SkillName.MagicResist, 120.0); // Stronger magic resist skill
            SetSkill(SkillName.EvalInt, 100.0); // Stronger eval int skill
            SetSkill(SkillName.Tactics, 100.0); // Higher tactics skill
            SetSkill(SkillName.Wrestling, 80.0); // Higher wrestling skill

            Fame = 15000; // Higher fame
            Karma = 15000; // Higher karma

            VirtualArmor = 60; // Increased virtual armor

            // Attach a random ability
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

        public OracleBoss(Serial serial) : base(serial)
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
