using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master carver")]
    public class CarverBoss : Carver
    {
        [Constructable]
        public CarverBoss() : base()
        {
            Name = "Master Carver";
            Title = "the Supreme Sculptor";

            // Update stats to match or exceed Barracoon
            SetStr(700, 900); // Enhanced strength
            SetDex(200, 250); // Enhanced dexterity
            SetInt(150, 200); // Enhanced intelligence

            SetHits(12000); // Increased health to boss-tier level

            SetDamage(20, 30); // Increased damage for boss-tier combat

            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 60, 75);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 90.0, 120.0); // Increased skill values
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);
            SetSkill(SkillName.Carpentry, 120.0, 140.0);

            Fame = 22500; // Increased fame for boss
            Karma = -22500; // Increased karma for boss

            VirtualArmor = 80; // Increased virtual armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new SherlocksSleuthingCap());
			PackItem(new CartographyDesk());
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here if needed
        }

        public CarverBoss(Serial serial) : base(serial)
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
