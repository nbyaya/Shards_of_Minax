using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an ethereal dragon corpse")]
    public class EtherealDragonBoss : EtherealDragon
    {
        [Constructable]
        public EtherealDragonBoss() : base()
        {
            Name = "Ethereal Dragon Overlord";
            Title = "the Spectral Terror";

            // Update stats to match or exceed Barracoon's values for a boss-level challenge
            SetStr(1200); // Upper bound strength from Barracoon stats
            SetDex(255);  // Upper dexterity from Barracoon stats
            SetInt(250);  // Upper intelligence from Barracoon stats

            SetHits(12000);  // Set to a high boss health
            SetDamage(29, 45);  // Boss-level damage

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0);  // Boss-level magic resistance
            SetSkill(SkillName.Tactics, 120.0);     // Boss-level tactics
            SetSkill(SkillName.Wrestling, 120.0);   // Boss-level wrestling

            Fame = 30000;  // Higher fame for a stronger boss
            Karma = -30000;  // Negative karma for a villainous boss

            VirtualArmor = 100;  // Higher virtual armor for the boss

            Tamable = false;  // Bosses are typically untamable
            ControlSlots = 0;  // No control slots

            // Attach a random ability for added challenge
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScrolls in addition to the base loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // You can add more custom logic for the boss if needed
        }

        public EtherealDragonBoss(Serial serial) : base(serial)
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
