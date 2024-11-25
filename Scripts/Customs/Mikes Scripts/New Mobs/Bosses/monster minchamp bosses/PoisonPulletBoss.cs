using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a poison overlord's corpse")]
    public class PoisonPulletBoss : PoisonPullet
    {
        private DateTime m_NextPoisonEgg;

        [Constructable]
        public PoisonPulletBoss()
            : base()
        {
            Name = "Poison Overlord";
            Title = "the Supreme Pullet";

            // Update stats to match or exceed Barracoon-like power
            SetStr(1200); // Increase strength
            SetDex(255); // Max out dexterity
            SetInt(250); // Max out intelligence

            SetHits(15000); // Higher health
            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100); // Maxed out poison resistance
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Increased skill values
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000; // Increased fame
            Karma = -24000; // Evil karma

            VirtualArmor = 100; // Increased armor

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

        public override void OnThink()
        {
            base.OnThink();
            // Optionally, you could add more advanced logic here for the boss's AI
        }

        public PoisonPulletBoss(Serial serial)
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
