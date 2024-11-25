using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ultimate retro robot romancer")]
    public class RetroRobotRomancerBoss : RetroRobotRomancer
    {
        [Constructable]
        public RetroRobotRomancerBoss() : base()
        {
            Name = "Ultimate Retro Robot Romancer";
            Title = "the Heartless Mechanic";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Increased strength to make it more powerful
            SetDex(255); // Maximized dexterity for agility
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Increased damage

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 100.0);
            SetSkill(SkillName.EvalInt, 110.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 110.0);
            SetSkill(SkillName.Wrestling, 110.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 70;

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

            // Optionally, add unique loot for the boss
            PackItem(new Gold(500));
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be added here, such as special attacks
        }

        public RetroRobotRomancerBoss(Serial serial) : base(serial)
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
