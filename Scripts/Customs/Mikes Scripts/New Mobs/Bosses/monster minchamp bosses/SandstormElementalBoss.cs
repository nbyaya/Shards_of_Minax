using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a sandstorm elemental boss corpse")]
    public class SandstormElementalBoss : SandstormElemental
    {
        [Constructable]
        public SandstormElementalBoss() : base()
        {
            Name = "Sandstorm Overlord";
            Title = "the Supreme Tempest";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Match Barracoon's upper strength
            SetDex(255); // Maximize dexterity for mobility
            SetInt(750); // Maximize intelligence for spell power

            SetHits(12000); // Set high health for a boss
            SetDamage(35, 45); // Slightly higher damage for the boss version

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = 30000; // Boss-tier fame
            Karma = -30000; // Negative karma for an evil boss

            VirtualArmor = 100; // Increase virtual armor for the boss

            // Attach random abilities for extra challenge
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

            // Enhanced behavior with additional boss-level challenge logic could be added here
        }

        public SandstormElementalBoss(Serial serial) : base(serial)
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
