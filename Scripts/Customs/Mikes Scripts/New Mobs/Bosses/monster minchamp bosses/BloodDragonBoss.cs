using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a blood dragon corpse")]
    public class BloodDragonBoss : BloodDragon
    {
        [Constructable]
        public BloodDragonBoss() : base()
        {
            Name = "Blood Dragon Overlord";
            Title = "the Supreme Blood Dragon";
            
            // Update stats to match or exceed Barracoon
            SetStr(1200); // Matching or exceeding Barracoon's upper strength
            SetDex(255); // Matching Barracoon's upper dexterity
            SetInt(250); // Matching or exceeding Barracoon's upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma

            VirtualArmor = 100; // Increased armor

            Tamable = false; // Boss dragons are untamable
            ControlSlots = 0; // Not tamable
            MinTameSkill = 0;

            Hue = 1487; // Blood red hue for visual identity
            
            // Attach a random ability
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

        public BloodDragonBoss(Serial serial) : base(serial)
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
