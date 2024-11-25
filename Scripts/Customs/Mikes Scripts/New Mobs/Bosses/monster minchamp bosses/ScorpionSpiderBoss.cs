using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a scorpion spider boss corpse")]
    public class ScorpionSpiderBoss : ScorpionSpider
    {
        [Constructable]
        public ScorpionSpiderBoss()
            : base()
        {
            Name = "Scorpion Spider Overlord";
            Title = "the Venomous Terror";

            // Enhanced Stats (Boss-tier)
            SetStr(1200); // Upper bound of Barracoon-like strength
            SetDex(255); // Max dexterity from ScorpionSpider
            SetInt(250); // Upper bound intelligence from ScorpionSpider

            SetHits(12000); // Boss-tier health
            SetDamage(35, 50); // Higher damage range

            // Damage Types (keeping same for consistency)
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            // Resistance Adjustments (Making it tougher)
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Energy, 60);

            // Skills (Scaling up to a boss level)
            SetSkill(SkillName.Anatomy, 50.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000; // Boss Fame
            Karma = -24000; // Negative Karma (Boss tier)

            VirtualArmor = 90; // Keeping virtual armor from original

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
            // Additional boss logic could be added here if necessary
        }

        public ScorpionSpiderBoss(Serial serial)
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
