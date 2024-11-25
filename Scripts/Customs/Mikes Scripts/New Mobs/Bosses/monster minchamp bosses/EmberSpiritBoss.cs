using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an ember overlord corpse")]
    public class EmberSpiritBoss : EmberSpirit
    {
        [Constructable]
        public EmberSpiritBoss() : base()
        {
            Name = "Ember Overlord";
            Title = "the Infernal Spirit";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(1200); // Maximum strength
            SetDex(255);  // Maximum dexterity
            SetInt(250);  // Maximum intelligence

            SetHits(12000); // Increased health for the boss tier
            SetDamage(35, 50); // Increased damage range

            // Set resistance values to make it tougher
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            // Skills are enhanced for the boss version
            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000;  // Increased fame for the boss
            Karma = -30000; // Negative karma (as it is an evil boss)

            VirtualArmor = 100; // Increased virtual armor to make it tougher

            // Attach the XmlRandomAbility for random enhancements
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Drop 5 MaxxiaScrolls in addition to normal loot
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

        public EmberSpiritBoss(Serial serial) : base(serial)
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
