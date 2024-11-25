using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a mystic wisp boss corpse")]
    public class MysticWispBoss : MysticWisp
    {
        [Constructable]
        public MysticWispBoss() : base()
        {
            Name = "Mystic Wisp Overlord";
            Title = "the Supreme Wisp";

            // Enhance stats to be more powerful, like a boss
            SetStr(1200); // Enhanced Strength
            SetDex(255); // Max Dexterity
            SetInt(250); // Enhanced Intelligence

            SetHits(12000); // Enhanced Health
            SetDamage(35, 50); // Enhanced Damage

            // Improved resistances to make the boss tougher
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma for a boss
            VirtualArmor = 100;

            Tamable = false; // No longer tamable for boss version
            ControlSlots = 0; // Ensure no control slots

            // Attach the random ability XML
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
            // Additional boss logic could be added here, such as enhanced ability usage or special effects
        }

        public MysticWispBoss(Serial serial) : base(serial)
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
