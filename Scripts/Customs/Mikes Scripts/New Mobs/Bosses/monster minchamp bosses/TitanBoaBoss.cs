using System;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a titan boa boss corpse")]
    public class TitanBoaBoss : TitanBoa
    {
        [Constructable]
        public TitanBoaBoss() : base()
        {
            // Updated to be a boss-tier version
            Name = "Titan Boa Overlord";
            Body = 0x15; // Same body as Titan Boa
            Hue = 1771; // Unique hue
            BaseSoundID = 219;

            // Enhanced Stats (matching or better than Barracoon's stats)
            SetStr(1200, 1500); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(12000); // Enhanced health
            SetDamage(40, 50); // Increased damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Enhanced anatomy skill
            SetSkill(SkillName.EvalInt, 100.0, 150.0); // Enhanced evaluation skill
            SetSkill(SkillName.Magery, 100.0, 120.0); // Enhanced magery skill
            SetSkill(SkillName.Meditation, 50.0, 75.0); // Enhanced meditation skill
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Enhanced magic resist skill
            SetSkill(SkillName.Tactics, 100.0, 150.0); // Enhanced tactics skill
            SetSkill(SkillName.Wrestling, 100.0, 150.0); // Enhanced wrestling skill

            Fame = 30000; // Increased fame for the boss
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 100; // Enhanced armor

            Tamable = false; // Not tamable
            ControlSlots = 3; // No control slots required

            // Attach a random ability to the boss
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
            // Additional boss-specific logic could be added here
        }

        public TitanBoaBoss(Serial serial) : base(serial)
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
