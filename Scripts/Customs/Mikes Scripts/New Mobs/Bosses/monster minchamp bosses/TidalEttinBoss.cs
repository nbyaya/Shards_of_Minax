using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a tidal ettin corpse")]
    public class TidalEttinBoss : TidalEttin
    {
        [Constructable]
        public TidalEttinBoss() : base()
        {
            Name = "Tidal Ettin Boss";
            Title = "the Tsunami Wielder";
            Hue = 1556; // Retain the original hue

            // Update stats to match or exceed the original TidalEttin
            SetStr(1200, 1500); // Higher strength than before
            SetDex(255, 300); // Higher dexterity
            SetInt(250, 350); // Higher intelligence

            SetHits(15000); // Much higher health
            SetDamage(45, 60); // Higher damage range

            // Enhance resistance values to make it more resilient
            SetResistance(ResistanceType.Physical, 85, 100);
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 95);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resistance skill
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill
            SetSkill(SkillName.Magery, 120.0); // Enhanced magery for more potent spells

            Fame = 30000; // Increased fame for a boss
            Karma = -30000; // Negative karma for the boss
            VirtualArmor = 100; // Increased virtual armor for more durability

            Tamable = false; // Ensure the boss cannot be tamed
            ControlSlots = 3;
            MinTameSkill = 0;

            // Attach the random ability via XML
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

            // Additional boss logic can be added here if necessary
        }

        public TidalEttinBoss(Serial serial) : base(serial)
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
