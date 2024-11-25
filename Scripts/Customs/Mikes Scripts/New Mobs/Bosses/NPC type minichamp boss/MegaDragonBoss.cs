using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using System.Collections.Generic;

namespace Server.Mobiles
{
    public class MegaDragonBoss : MegaDragon
    {
        [Constructable]
        public MegaDragonBoss() : base()
        {
            // Update name and title for the boss version
            Name = "MegaDragon Overlord";
            Title = "the Devastator";

            // Update stats to match or exceed Barracoon's values
            SetStr(1185); // Matching Barracoon's upper strength
            SetDex(175); // Matching Barracoon's upper dexterity
            SetInt(775); // Matching Barracoon's upper intelligence

            SetHits(12000); // Exceeds the original MegaDragon's health
            SetDamage(29, 38); // Matching Barracoon's damage range

            // Set Resistances to match or exceed Barracoon's
            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 70, 75);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Set Skill values to match or exceed Barracoon's
            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;  // Boss-level fame
            Karma = -22500; // Negative karma for a boss

            VirtualArmor = 70;  // Increased armor for a boss-level NPC

            // Attach a random ability for added uniqueness
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override GenerateLoot to add 5 MaxxiaScroll items to the loot
        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public MegaDragonBoss(Serial serial) : base(serial)
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
