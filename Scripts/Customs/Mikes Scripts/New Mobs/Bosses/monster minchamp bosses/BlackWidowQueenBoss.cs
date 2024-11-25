using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a black widow queen corpse")]
    public class BlackWidowQueenBoss : BlackWidowQueen
    {
        [Constructable]
        public BlackWidowQueenBoss()
            : base()
        {
            Name = "Black Widow Queen";
            Title = "the Supreme Arachnid";
            Hue = 1153; // Unique boss hue for the queen

            // Enhance the stats to match boss tier
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Higher hit points to match a boss tier
            SetDamage(45, 60); // Increase damage for a harder fight

            SetResistance(ResistanceType.Physical, 80, 95); // Increase resistance for durability
            SetResistance(ResistanceType.Fire, 80, 90); 
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 80, 95);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resist for boss status
            SetSkill(SkillName.Tactics, 120.0); // Improved tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill

            Fame = 35000; // Increased fame for a boss-tier creature
            Karma = -35000; // Negative karma for the evil nature

            VirtualArmor = 120; // Stronger virtual armor for durability

            // Attach the random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());


        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            // Drop 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Optionally, boss-specific AI or behaviors can be added here
        }

        public BlackWidowQueenBoss(Serial serial)
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
