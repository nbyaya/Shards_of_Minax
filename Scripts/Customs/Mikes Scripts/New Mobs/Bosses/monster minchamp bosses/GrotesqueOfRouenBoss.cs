using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a grotesque of Rouen corpse")]
    public class GrotesqueOfRouenBoss : GrotesqueOfRouen
    {
        [Constructable]
        public GrotesqueOfRouenBoss()
            : base()
        {
            Name = "Grotesque of Rouen";
            Title = "the Awakened Horror";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Improved strength for the boss
            SetDex(255); // Maxed out dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(40, 55); // Increased damage

            SetDamageType(ResistanceType.Physical, 60); // Higher physical damage
            SetDamageType(ResistanceType.Fire, 30); // Higher fire damage
            SetDamageType(ResistanceType.Energy, 30); // Higher energy damage

            SetResistance(ResistanceType.Physical, 80, 100); // Maxed physical resistance
            SetResistance(ResistanceType.Fire, 80, 100); // Maxed fire resistance
            SetResistance(ResistanceType.Cold, 60, 80); // Enhanced cold resistance
            SetResistance(ResistanceType.Poison, 80, 100); // Maxed poison resistance
            SetResistance(ResistanceType.Energy, 50, 70); // Higher energy resistance

            SetSkill(SkillName.Anatomy, 50.0, 80.0); // Increased Anatomy skill
            SetSkill(SkillName.EvalInt, 110.0, 120.0); // Higher EvalInt skill
            SetSkill(SkillName.Magery, 120.0, 130.0); // Enhanced Magery skill
            SetSkill(SkillName.Meditation, 50.0, 70.0); // Increased Meditation skill
            SetSkill(SkillName.MagicResist, 150.0, 180.0); // High Magic Resist skill
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Increased Tactics skill
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Improved Wrestling skill

            Fame = 30000; // Higher fame
            Karma = -30000; // Boss-level karma

            VirtualArmor = 120; // Increased armor

            PackItem(new BossTreasureBox());
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
            // Additional boss logic could be added here if necessary
        }

        public GrotesqueOfRouenBoss(Serial serial)
            : base(serial)
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
