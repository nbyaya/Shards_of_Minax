using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("corpse of the supreme field commander")]
    public class FieldCommanderBoss : FieldCommander
    {
        [Constructable]
        public FieldCommanderBoss() : base()
        {
            Name = "Supreme Field Commander";
            Title = "the Overlord of the Battlefield";
            
            // Enhanced Stats (matching or exceeding Barracoon)
            SetStr(1100); // Stronger than original FieldCommander
            SetDex(200);  // Max out dexterity range
            SetInt(200);  // Keep intelligence the same

            SetHits(12000); // Boss health much higher than original
            SetDamage(29, 38); // Enhanced damage range to match or exceed Barracoon

            // Improved resistances
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Skills to match boss tier (similar to Barracoon)
            SetSkill(SkillName.Swords, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);

            Fame = 25000; // Higher Fame
            Karma = -25000; // More negative Karma (as expected for a boss)

            VirtualArmor = 70; // Higher armor value for boss

            // Attach the XmlRandomAbility for extra dynamic capabilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScrolls in addition to normal loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss logic could be added here if necessary
        }

        public FieldCommanderBoss(Serial serial) : base(serial)
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
