using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a storm daemon corpse")]
    public class StormDaemonBoss : StormDaemon
    {
        [Constructable]
        public StormDaemonBoss() : base()
        {
            Name = "Storm Overlord";
            Title = "the Tempest Lord";
            
            // Enhanced Stats to make the boss stronger
            SetStr(1200); // Higher strength for the boss
            SetDex(255); // Maximum dexterity
            SetInt(250); // Higher intelligence for magic prowess

            SetHits(12000); // Boss-level health
            SetDamage(40, 50); // Increased damage range

            // Resistance enhancements for tougher defense
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 70);

            // Improved skill levels for the boss
            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            // Attach the XmlRandomAbility for additional random powers
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

            // You can modify or add additional behavior here
            // The boss will continue using its original abilities, 
            // but its stats and drops have been enhanced
        }

        public StormDaemonBoss(Serial serial) : base(serial)
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
