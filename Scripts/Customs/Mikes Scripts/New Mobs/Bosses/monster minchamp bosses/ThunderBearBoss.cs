using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a thunder overlord corpse")]
    public class ThunderBearBoss : ThunderBear
    {
        [Constructable]
        public ThunderBearBoss()
            : base()
        {
            Name = "Thunder Overlord";
            Hue = 1179; // Electric blue hue for a unique look

            // Update stats to match or exceed Barracoon-style boss stats
            SetStr(1200); // Increased strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Enhanced health for boss-tier
            SetDamage(40, 50); // Increased damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80); // Increased resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.Anatomy, 75.0, 100.0); // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);

            Fame = 35000; // Increased fame
            Karma = -35000; // Increased karma for a tougher opponent

            VirtualArmor = 120; // Increased virtual armor for boss-level defense

            // Attach random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Initialize special abilities
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
            // Additional boss logic could be added here
        }

        public ThunderBearBoss(Serial serial)
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
