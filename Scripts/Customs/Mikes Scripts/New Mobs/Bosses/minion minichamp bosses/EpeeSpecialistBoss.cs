using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master epee specialist")]
    public class EpeeSpecialistBoss : EpeeSpecialist
    {
        [Constructable]
        public EpeeSpecialistBoss() : base()
        {
            Name = "Master Epee Specialist";
            Title = "the Supreme Blade";

            // Update stats to match or exceed Barracoon's stats for a boss-tier difficulty
            SetStr(700, 1000); // Enhanced strength
            SetDex(400, 500); // Enhanced dexterity
            SetInt(200, 300); // Enhanced intelligence

            SetHits(10000); // Significantly higher health
            SetDamage(25, 40); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 75, 85); // Increased resistances
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 85);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Fencing, 120.0); // Improved skills
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Anatomy, 100.0);

            Fame = 22500; // Increased fame for a boss-level creature
            Karma = -22500; // Negative karma

            VirtualArmor = 60; // Enhanced armor

            // Attach the XmlRandomAbility for random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new BalronSummoningMateria());
			PackItem(new SurgeonsInsightfulMask());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if needed
        }

        public EpeeSpecialistBoss(Serial serial) : base(serial)
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
