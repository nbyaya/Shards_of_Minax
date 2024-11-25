using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a vortex overlord corpse")]
    public class VortexGuardianBoss : VortexGuardian
    {
        [Constructable]
        public VortexGuardianBoss()
            : base()
        {
            Name = "Vortex Overlord";
            Title = "the Master of the Winds";

            // Update stats to match or exceed VortexGuardian's values
            SetStr(1200); // Upper bound of the strength
            SetDex(255); // Upper bound of the dexterity
            SetInt(250); // Upper bound of the intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(35, 50); // Higher damage range for the boss

            // Adjust resistances
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            // Update skill levels for the boss
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);

            Fame = 35000; // Boss-level fame
            Karma = -35000; // Boss-level karma

            VirtualArmor = 100; // Increased armor for the boss

            // Attach the XmlRandomAbility for extra random ability
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

            // Additional boss logic can be added here if needed
        }

        public VortexGuardianBoss(Serial serial)
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
