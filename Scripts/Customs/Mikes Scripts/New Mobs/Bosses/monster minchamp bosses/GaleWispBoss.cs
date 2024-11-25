using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a gale wisp boss corpse")]
    public class GaleWispBoss : GaleWisp
    {
        [Constructable]
        public GaleWispBoss() : base()
        {
            Name = "Gale Wisp Overlord";
            Title = "the Tempest Bringer";

            // Update stats to match or exceed the original GaleWisp
            SetStr(1200); // Upper range strength
            SetDex(255); // Upper range dexterity
            SetInt(250); // Upper range intelligence

            SetHits(12000); // Increase health to make it more boss-like

            SetDamage(35, 50); // Increase damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Increase physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Increase fire resistance
            SetResistance(ResistanceType.Cold, 60, 80); // Increase cold resistance
            SetResistance(ResistanceType.Poison, 100); // Keep poison resistance at max
            SetResistance(ResistanceType.Energy, 50, 60); // Increase energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Increase magic resist skill
            SetSkill(SkillName.Tactics, 110.0); // Increase tactics skill
            SetSkill(SkillName.Wrestling, 110.0); // Increase wrestling skill

            Fame = 30000; // Increase fame for boss status
            Karma = -30000; // Increase karma to reflect boss-like behavior

            VirtualArmor = 100; // Higher virtual armor for tougher defense

            // Attach a random ability for more dynamic behavior
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
            // Additional boss logic or behavior can be added here if needed
        }

        public GaleWispBoss(Serial serial) : base(serial)
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
