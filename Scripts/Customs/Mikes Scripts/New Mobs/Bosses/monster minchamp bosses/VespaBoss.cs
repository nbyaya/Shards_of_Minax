using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a hive mind corpse")]
    public class VespaBoss : Vespa
    {
        [Constructable]
        public VespaBoss() : base()
        {
            Name = "Vespa the Hive Overlord";
            Title = "the Supreme Hive Mind";
            
            // Updated Stats to Boss Level (Matching Barracoon's stats or higher)
            SetStr(1200); // Higher than Vespa's upper bound
            SetDex(255); // Max dexterity for fast combat
            SetInt(250); // High intelligence

            SetHits(12000); // Increase hit points for boss difficulty
            SetDamage(35, 45); // Increased damage range

            SetDamageType(ResistanceType.Physical, 60); // Increased physical damage
            SetDamageType(ResistanceType.Fire, 30); // Increased fire damage
            SetDamageType(ResistanceType.Energy, 30); // Increased energy damage

            SetResistance(ResistanceType.Physical, 75, 85); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 70, 85); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60, 70); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 80, 90); // Increased poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Increased energy resistance

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Increased anatomy skill
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Increased eval skill
            SetSkill(SkillName.Magery, 120.0, 140.0); // Increased magery skill
            SetSkill(SkillName.Meditation, 50.0, 75.0); // Increased meditation skill
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Increased magic resist skill
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Increased wrestling skill

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma (still evil)

            VirtualArmor = 100; // Higher armor

            // Attach the XmlRandomAbility for random abilities
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
            // Additional boss logic could be added here
        }

        public VespaBoss(Serial serial) : base(serial)
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
