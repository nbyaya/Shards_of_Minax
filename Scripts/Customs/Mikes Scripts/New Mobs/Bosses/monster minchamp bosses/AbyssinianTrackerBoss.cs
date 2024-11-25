using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an Abyssinian Tracker Boss corpse")]
    public class AbyssinianTrackerBoss : AbyssinianTracker
    {
        [Constructable]
        public AbyssinianTrackerBoss() : base()
        {
            Name = "Abyssinian Tracker, the Alpha";
            Title = "the Supreme Hunter";
            Hue = 1357; // Unique red hue for distinction

            // Enhanced stats based on boss requirements
            SetStr(1200); // Upper bound of original strength
            SetDex(255); // Upper bound of original dexterity
            SetInt(250); // Upper bound of original intelligence

            SetHits(12000); // Boss-level health
            SetDamage(35, 45); // Increased damage for the boss-tier encounter

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Increased fame for the boss
            Karma = -30000; // Increased negative karma

            VirtualArmor = 100;

            // Attach the random ability (XmlRandomAbility)
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

            // Additional behavior or abilities can be added here for the boss
        }

        public AbyssinianTrackerBoss(Serial serial) : base(serial)
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
