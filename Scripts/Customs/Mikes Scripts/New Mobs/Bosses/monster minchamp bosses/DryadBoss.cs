using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a dryad boss corpse")]
    public class DryadBoss : Dryad
    {
        [Constructable]
        public DryadBoss() : base()
        {
            Name = "Dryad Queen";
            Title = "the Forest's Wrath";
            Body = 0x191; // Keep the original body

            // Update stats to match or exceed Barracoon's boss tier
            SetStr(1200); // Higher strength
            SetDex(255);  // Maximum dexterity
            SetInt(250);  // Increased intelligence

            SetHits(12000); // Boss-level health
            SetDamage(35, 45); // Stronger damage output

            // Set resistance values to be stronger than the original
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.EvalInt, 100.0);
            SetSkill(SkillName.Magery, 100.0);

            Fame = 35000; // Boss-level fame
            Karma = -35000; // Boss-level karma

            VirtualArmor = 120; // Increased virtual armor for toughness

            Tamable = false; // Boss creatures are untamable
            ControlSlots = 0; // Can't be controlled by players
            MinTameSkill = 0;

            // Attach a random ability to this boss
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override loot generation to include 5 MaxxiaScrolls
        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        // Override to handle any additional boss behaviors (optional)
        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if needed
        }

        public DryadBoss(Serial serial) : base(serial)
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
