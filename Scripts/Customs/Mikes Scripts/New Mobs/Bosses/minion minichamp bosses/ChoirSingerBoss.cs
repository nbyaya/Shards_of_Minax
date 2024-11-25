using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the choir overlord")]
    public class ChoirSingerBoss : ChoirSinger
    {
        [Constructable]
        public ChoirSingerBoss() : base()
        {
            Name = "Choir Overlord";
            Title = "the Supreme Singer";

            // Enhance stats to match boss tier
            SetStr(500, 700);  // Enhanced strength
            SetDex(200, 250);  // Enhanced dexterity
            SetInt(300, 400);  // Enhanced intelligence

            SetHits(12000);    // Set high health
            SetDamage(15, 25); // Enhanced damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50); // Keeping original damage types

            SetResistance(ResistanceType.Physical, 60, 70); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Meditation, 100.0, 120.0); // Enhanced skill range
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 75.0, 100.0);

            Fame = 12000;  // Increased fame for a boss
            Karma = -12000;  // Negative karma for a boss

            VirtualArmor = 50; // Enhanced virtual armor

            // Attach random ability from XmlRandomAbility
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new LexxVase());
			PackItem(new StaffOfTheElements());
            // Drop 5 MaxxiaScrolls in addition to regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // You could add additional behavior to enhance the boss' complexity here
        }

        public ChoirSingerBoss(Serial serial) : base(serial)
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
