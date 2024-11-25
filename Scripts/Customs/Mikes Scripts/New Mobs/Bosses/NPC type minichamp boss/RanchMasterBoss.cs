using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ranch overlord")]
    public class RanchMasterBoss : RanchMaster
    {
        [Constructable]
        public RanchMasterBoss() : base()
        {
            Name = "Ranch Overlord";
            Title = "the Supreme Rancher";

            // Enhanced stats for the boss version
            SetStr(1200); // Higher strength than the original
            SetDex(255); // Higher dexterity than the original
            SetInt(250); // Higher intelligence than the original

            SetHits(12000); // Much higher health for a boss-tier version

            SetDamage(25, 40); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            // Skills updated to match boss-tier difficulty
            SetSkill(SkillName.Archery, 100.0);
            SetSkill(SkillName.Bushido, 100.0);
            SetSkill(SkillName.Chivalry, 100.0);
            SetSkill(SkillName.Fencing, 100.0);
            SetSkill(SkillName.Lumberjacking, 100.0);
            SetSkill(SkillName.Ninjitsu, 100.0);
            SetSkill(SkillName.Parry, 100.0);
            SetSkill(SkillName.Swords, 100.0);

            // Boss-tier fame and karma for high difficulty
            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80; // Increased armor

            // Attach a random ability for added complexity
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

        public RanchMasterBoss(Serial serial) : base(serial)
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
