using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the trap master")]
    public class TrapEngineerBoss : TrapEngineer
    {
        [Constructable]
        public TrapEngineerBoss() : base()
        {
            Name = "Trap Master";
            Title = "the Supreme Engineer";

            // Enhance stats to be on par with a boss-tier NPC
            SetStr(1200); // Matching or surpassing original Str range
            SetDex(255); // Upper bound dexterity from original
            SetInt(250); // Keep intelligence at high range

            SetHits(10000); // High health to match boss-tier
            SetDamage(20, 40); // Increase damage to a higher range

            // Enhance resistances, particularly poison, to make it tougher
            SetResistance(ResistanceType.Physical, 75, 80);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 50.0, 80.0); // Increase anatomy for stronger combat
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Higher magic skill
            SetSkill(SkillName.Magery, 100.0, 120.0); // Stronger mage skills
            SetSkill(SkillName.Meditation, 50.0, 70.0); // Decent meditation for mana regen
            SetSkill(SkillName.MagicResist, 150.0, 170.0); // High magic resistance for tougher combat
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Stronger tactics for better combat behavior
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Better wrestling for physical combat
            SetSkill(SkillName.DetectHidden, 100.0, 120.0); // Enhanced ability to detect hidden traps
            SetSkill(SkillName.RemoveTrap, 100.0, 120.0); // Enhanced trap removal ability

            Fame = 15000; // Increased fame for a boss-tier creature
            Karma = -15000; // Karma to indicate it's an enemy boss

            VirtualArmor = 70; // Improved armor for tougher defense

            // Attach a random ability
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

        public TrapEngineerBoss(Serial serial) : base(serial)
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
