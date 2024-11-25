using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the jungle naturalist overlord")]
    public class JungleNaturalistBoss : JungleNaturalist
    {
        [Constructable]
        public JungleNaturalistBoss() : base()
        {
            Name = "Jungle Naturalist Overlord";
            Title = "the Master of the Jungle";

            // Enhanced stats to match or exceed a boss level
            SetStr(1200); // Boss-level strength
            SetDex(255); // Boss-level dexterity
            SetInt(350); // High intelligence

            SetHits(12000); // Increased health to match a boss
            SetDamage(30, 40); // Increased damage for a boss fight

            // Resisting enhancements for a boss creature
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            // Enhanced skills for a boss creature
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);

            Fame = 25000; // Boss-level fame
            Karma = -25000; // Negative karma for an evil boss

            VirtualArmor = 80; // Higher virtual armor for more defense

            // Attach the XmlRandomAbility to introduce randomness in the abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override loot generation to include 5 MaxxiaScrolls
        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        // Override OnThink to maintain original functionality (summoning a creature)
        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here
        }

        public JungleNaturalistBoss(Serial serial) : base(serial)
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
