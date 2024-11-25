using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a nymph overlord's corpse")]
    public class NymphBoss : Nymph
    {
        [Constructable]
        public NymphBoss() : base()
        {
            // Enhanced name and title for the boss version
            Name = "Nymph Overlord";
            Title = "the Enchantress Supreme";
            Hue = 0x4F9; // Change to a more unique color

            // Increase stats to match or exceed the original Nymph's values
            SetStr(1200); // Upper limit for strength
            SetDex(255);  // Upper limit for dexterity
            SetInt(250);  // Upper limit for intelligence

            SetHits(12000); // Increased health
            SetDamage(35, 50); // Increased damage range

            // Enhanced resistance to match the original Nymph's stats, can be higher for the boss version
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            // Boosted skill levels
            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 110;

            // Attach the XmlRandomAbility for dynamic gameplay features
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
            // Additional boss logic could go here if necessary (e.g., more powerful abilities)
        }

        public NymphBoss(Serial serial) : base(serial)
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
