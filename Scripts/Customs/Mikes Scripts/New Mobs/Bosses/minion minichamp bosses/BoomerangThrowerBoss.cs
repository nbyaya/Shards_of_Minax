using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the boomerang overlord")]
    public class BoomerangThrowerBoss : BoomerangThrower
    {
        [Constructable]
        public BoomerangThrowerBoss() : base()
        {
            Name = "Boomerang Overlord";
            Title = "the Supreme Thrower";

            // Update stats to match or exceed Barracoon-like boss stats
            SetStr(1200); // Boss-tier strength
            SetDex(300);  // Max dexterity for agility and attack speed
            SetInt(250);  // Intelligence increased slightly

            SetHits(12000); // Boss-tier health
            SetDamage(29, 38); // Matching Barracoon's damage range

            SetDamageType(ResistanceType.Physical, 80); // More physical damage
            SetDamageType(ResistanceType.Cold, 20); // Cold damage as per the original

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 75.0, 100.0); // Increased anatomy skill
            SetSkill(SkillName.Archery, 110.0, 120.0); // Increased archery skill for more boomerang damage
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Increased tactics skill
            SetSkill(SkillName.MagicResist, 90.0, 100.0); // Enhanced magic resistance

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 70; // Better virtual armor to withstand attacks

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
			PackItem(new UndeadCrown());
			PackItem(new SilksOfTheVictor());            
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

        public BoomerangThrowerBoss(Serial serial) : base(serial)
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
