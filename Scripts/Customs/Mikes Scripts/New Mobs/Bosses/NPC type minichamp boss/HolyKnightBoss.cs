using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Holy Knight Overlord")]
    public class HolyKnightBoss : HolyKnight
    {
        [Constructable]
        public HolyKnightBoss() : base()
        {
            Name = "Holy Knight Overlord";
            Title = "the Supreme Guardian";
            Body = 400; // No change in appearance, unless you'd like to alter this

            // Update stats to match or exceed a boss-level challenge
            SetStr(1200); // Significantly higher strength than the base HolyKnight
            SetDex(250);  // Much higher dexterity for more agility
            SetInt(200);  // Increased intelligence for better skills

            SetHits(15000); // Boss-level health
            SetDamage(40, 60); // Increased damage output

            SetResistance(ResistanceType.Physical, 80);  // Higher resistances for a boss fight
            SetResistance(ResistanceType.Fire, 75);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 70);
            SetResistance(ResistanceType.Energy, 75);

            SetSkill(SkillName.MagicResist, 120.0); // Boss-level resistance
            SetSkill(SkillName.Tactics, 130.0);     // Higher tactics for better AI
            SetSkill(SkillName.Wrestling, 130.0);   // Improved wrestling skill

            Fame = 20000;   // Higher fame for a boss
            Karma = -20000; // Negative karma for a villain boss

            VirtualArmor = 80;  // Higher virtual armor to make the fight harder

            // Add a horse and make it a special mount if desired
            AddHorse();

            ActiveSpeed = 0.01;   // Boss speed is faster for more challenge
            PassiveSpeed = 0.02;

            // Attach a random ability for more unpredictability
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
            // Add more complex behavior if needed for the boss fight
        }

        public HolyKnightBoss(Serial serial) : base(serial)
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
