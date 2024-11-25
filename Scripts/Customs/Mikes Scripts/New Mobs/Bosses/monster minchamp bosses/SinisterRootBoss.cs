using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a sinister root corpse")]
    public class SinisterRootBoss : SinisterRoot
    {
        [Constructable]
        public SinisterRootBoss()
            : base()
        {
            Name = "Sinister Root Overlord";
            Title = "the Dreaded";

            // Enhance stats to boss level
            SetStr(1200); // Increased strength
            SetDex(255);  // Max dexterity
            SetInt(250);  // Max intelligence

            SetHits(12000); // Increased health
            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Stronger resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100); // Full resistance to poison
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced resist skill
            SetSkill(SkillName.Tactics, 120.0); // Maxed tactics
            SetSkill(SkillName.Wrestling, 120.0); // Maxed wrestling

            Fame = 24000; // Same as before
            Karma = -24000; // Same as before

            VirtualArmor = 90; // High virtual armor

            // Attach the XmlRandomAbility to this boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            Tamable = false; // Make sure this boss is untamable
            ControlSlots = 0; // No control slots for taming
            MinTameSkill = 0; // Not tamable at all
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
            // Any additional boss behavior could be added here
        }

        public SinisterRootBoss(Serial serial)
            : base(serial)
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
