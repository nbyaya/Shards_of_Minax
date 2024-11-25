using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a mystic fowl overlord corpse")]
    public class MysticFowlBoss : MysticFowl
    {
        private DateTime m_NextMysticEgg;

        [Constructable]
        public MysticFowlBoss() : base()
        {
            Name = "Mystic Fowl Overlord";
            Title = "the Supreme Mystic Fowl";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(1200); // Higher than the original MysticFowl
            SetDex(255);  // Higher dexterity
            SetInt(250);  // Higher intelligence

            SetHits(12000); // Increased health to match boss-tier
            SetDamage(35, 45); // Increased damage for a stronger boss

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            Tamable = false; // Boss should not be tamable
            ControlSlots = 0; // No control slots

            // Attach a random ability to enhance the boss's capabilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextMysticEgg = DateTime.UtcNow;
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
            // Add additional boss behavior here if necessary
        }

        public MysticFowlBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_NextMysticEgg = DateTime.UtcNow;
        }
    }
}
