using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a cyclone overlord corpse")]
    public class CycloneDemonBoss : CycloneDemon
    {
        [Constructable]
        public CycloneDemonBoss() : base()
        {
            Name = "Cyclone Overlord";
            Title = "the Supreme Demon of the Storm";

            // Update stats to match or exceed Barracoon (or better where applicable)
            SetStr(1200); // Increased strength for boss-tier
            SetDex(255);  // Maximum dexterity for better speed
            SetInt(250);  // High intelligence for magic usage

            SetHits(12000); // Increased health for the boss-tier challenge
            SetDamage(40, 50); // Higher damage for greater challenge

            // Increased resistances for a tougher fight
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 90, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            // Higher skill levels for more effective attacks
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);

            Fame = 30000; // Higher fame for a tougher boss
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 100; // Stronger armor for the boss

            Tamable = false; // Not tamable as it's a boss
            ControlSlots = 0; // No taming control slots

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            // Drop 5 MaxxiaScrolls in addition to the normal loot
            base.GenerateLoot();
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic could be added here if desired
        }

        public CycloneDemonBoss(Serial serial) : base(serial)
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
