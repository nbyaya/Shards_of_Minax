using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the nerve overlord")]
    public class NerveAgentBoss : NerveAgent
    {
        [Constructable]
        public NerveAgentBoss() : base()
        {
            Name = "Nerve Overlord";
            Title = "the Supreme Nerve Agent";

            // Enhance stats to match or exceed the original
            SetStr(1200); // Increased strength
            SetDex(200); // Increased dexterity
            SetInt(300); // Enhanced intelligence

            SetHits(12000); // Increased health for a boss-tier entity

            SetDamage(29, 45); // Increased damage for boss tier

            // Keep energy damage type as the main damage type
            SetDamageType(ResistanceType.Energy, 100);

            // Increased resistances for a more challenging encounter
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 70, 85);

            // Enhance skills to match a boss-level challenge
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 95.0, 110.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 22500; // Increased fame for boss-tier
            Karma = -22500; // Negative karma to reflect evil nature of the boss

            VirtualArmor = 70; // Increased virtual armor

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

            // Add additional boss logic or unique behavior if needed here
        }

        public NerveAgentBoss(Serial serial) : base(serial)
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
