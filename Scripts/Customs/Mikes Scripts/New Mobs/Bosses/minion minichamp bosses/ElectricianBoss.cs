using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the electric overlord")]
    public class ElectricianBoss : Electrician
    {
        [Constructable]
        public ElectricianBoss() : base()
        {
            Name = "Electric Overlord";
            Title = "the Shockmaster";

            // Update stats to match or exceed Barracoon
            SetStr(800, 1200); // Increase Strength
            SetDex(250, 300); // Increase Dexterity
            SetInt(300, 400); // Increase Intelligence

            SetHits(12000); // Increased health to match a boss tier
            SetDamage(20, 40); // Increase damage

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Energy, 75);

            // Improve resistances to make it a stronger boss
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 50.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 20000;  // High fame for a boss
            Karma = -20000; // Negative karma, fitting for a boss

            VirtualArmor = 80; // Boss-level armor

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
			PackItem(new GoronsGauntlets());
			PackItem(new DrapedBlanket());            
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

        public ElectricianBoss(Serial serial) : base(serial)
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
