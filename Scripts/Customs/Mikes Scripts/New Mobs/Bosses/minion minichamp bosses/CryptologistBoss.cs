using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master cryptologist")]
    public class CryptologistBoss : Cryptologist
    {
        [Constructable]
        public CryptologistBoss() : base()
        {
            Name = "Master Cryptologist";
            Title = "the Grand Master of Codes";

            // Update stats to make it more powerful
            SetStr(800); // Higher strength
            SetDex(200); // Higher dexterity
            SetInt(300); // Higher intelligence

            SetHits(12000); // High health to match a boss
            SetDamage(20, 40); // Increased damage

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Increase skill values to reflect the boss-tier
            SetSkill(SkillName.Anatomy, 90.0, 110.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 70.0, 90.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 22500; // Higher fame for a boss
            Karma = -22500; // Negative karma to reflect a villain

            VirtualArmor = 60; // Higher armor for boss

            // Attach random ability to the boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new ConquistadorsHoard());
			PackItem(new GuillotineCircleMateria());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic (e.g., special abilities or dialogue) could be added here
        }

        public CryptologistBoss(Serial serial) : base(serial)
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
