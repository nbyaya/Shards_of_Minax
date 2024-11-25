using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a necro ettin boss corpse")]
    public class NecroEttinBoss : NecroEttin
    {
        [Constructable]
        public NecroEttinBoss() : base()
        {
            Name = "Necro Ettin Overlord";
            Title = "the Supreme Necromancer";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(1200); // Increased strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Increased health to match a boss
            SetDamage(40, 50); // Increased damage range for a tougher boss

            SetResistance(ResistanceType.Physical, 75, 85); // Increased physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Increased fire resistance
            SetResistance(ResistanceType.Cold, 60, 75); // Increased cold resistance
            SetResistance(ResistanceType.Poison, 75, 90); // Increased poison resistance
            SetResistance(ResistanceType.Energy, 60, 75); // Increased energy resistance

            SetSkill(SkillName.Magery, 120.0); // Increased magery skill
            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resist skill
            SetSkill(SkillName.Tactics, 110.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 110.0); // Increased wrestling skill

            Fame = 30000; // Increased fame to indicate it's a boss-tier NPC
            Karma = -30000; // Increased negative karma (boss-tier usually has higher karma)

            VirtualArmor = 100; // Increased virtual armor for higher defense

            Tamable = false; // Not tamable, as it is a boss
            ControlSlots = 0; // Not controllable

            // Attach the XmlRandomAbility to add dynamic behavior
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override loot generation to add 5 MaxxiaScroll items
        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScroll items to the loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }


        public NecroEttinBoss(Serial serial) : base(serial)
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
