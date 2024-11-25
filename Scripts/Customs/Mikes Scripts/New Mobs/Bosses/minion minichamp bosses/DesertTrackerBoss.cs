using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the desert overlord")]
    public class DesertTrackerBoss : DesertTracker
    {
        [Constructable]
        public DesertTrackerBoss() : base()
        {
            Name = "Desert Overlord";
            Title = "the Supreme Tracker";

            // Update stats to match or exceed Barracoon's or the original NPC's better stats
            SetStr(900); // Higher Strength for a boss
            SetDex(250); // Higher Dexterity for more speed
            SetInt(300); // Higher Intelligence for more spellcasting potential

            SetHits(12000); // Boss-tier health
            SetDamage(18, 30); // Higher damage range for a boss

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 85); // Increased resistances
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 80.0, 100.0); // Improved skills for a boss
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 40.0, 60.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 20000; // High fame for a boss NPC
            Karma = -20000; // High karma loss for a boss NPC

            VirtualArmor = 75; // Increased virtual armor for tankiness

            // Attach a random ability for extra dynamics
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new ToxicologistsTrove());
			PackItem(new AerobicsInstructorsLegwarmers());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public DesertTrackerBoss(Serial serial) : base(serial)
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
