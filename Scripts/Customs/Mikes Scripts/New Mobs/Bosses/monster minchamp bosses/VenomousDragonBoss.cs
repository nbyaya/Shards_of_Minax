using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a venomous dragon corpse")]
    public class VenomousDragonBoss : VenomousDragon
    {
        [Constructable]
        public VenomousDragonBoss()
            : base()
        {
            Name = "Venomous Dragon";
            Title = "the Supreme Venom";

            // Enhanced stats, matching or exceeding the original Venomous Dragon
            SetStr(1200); // Enhanced strength
            SetDex(255); // Maxed out dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // High health for the boss tier
            SetDamage(35, 45); // Increased damage for a boss-tier NPC

            // Resistance adjustments to make the dragon tougher
            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 80, 95);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 80, 95);
            SetResistance(ResistanceType.Energy, 50, 65);

            // Enhanced skills
            SetSkill(SkillName.Anatomy, 50.0, 80.0);
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.Meditation, 50.0, 80.0);
            SetSkill(SkillName.MagicResist, 150.0, 160.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // High fame for boss
            Karma = -30000; // Negative karma for a boss villain

            VirtualArmor = 120; // Strong virtual armor

            // Attach the random ability XML
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // The venomous dragon has been made a boss-tier creature
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
            // You can add extra abilities or logic for the boss here if needed
        }

        public VenomousDragonBoss(Serial serial)
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
