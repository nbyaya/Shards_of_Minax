using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Siamese Illusionist corpse")]
    public class SiameseIllusionistBoss : SiameseIllusionist
    {
        [Constructable]
        public SiameseIllusionistBoss() : base()
        {
            Name = "Siamese Illusionist";
            Title = "the Master of Illusions";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Higher strength for boss
            SetDex(255); // Max dexterity
            SetInt(750); // Increased intelligence

            SetHits(12000); // High health
            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100); // Keep poison resistance as is
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 120.0); // Increased skill values
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 35000; // Increased fame for boss-tier creature
            Karma = -35000; // Increased karma penalty for boss

            VirtualArmor = 120; // Increased armor value

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
            // Additional boss logic can be added here if necessary
        }

        public SiameseIllusionistBoss(Serial serial) : base(serial)
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
