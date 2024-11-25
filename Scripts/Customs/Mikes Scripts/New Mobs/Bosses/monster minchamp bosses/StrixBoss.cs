using System;
using Server.Items;
using Server.Engines.XmlSpawner2;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a strix overlord corpse")]
    public class StrixBoss : Strix
    {
        [Constructable]
        public StrixBoss() : base()
        {
            Name = "Strix the Winged Overlord";
            Title = "the Supreme Horror";

            // Update stats to match or exceed Barracoon's level or superior
            SetStr(1200); // Strength elevated from original
            SetDex(255); // Dexterity elevated from original
            SetInt(250); // Intelligence elevated from original

            SetHits(12000); // Set a high hit points similar to Barracoon's value
            SetDamage(35, 50); // Increased damage range for a boss-tier creature

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 80); // Improved resistances
            SetResistance(ResistanceType.Fire, 70, 85);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0); // High magic resist for a boss fight
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Increased Fame for a boss NPC
            Karma = -30000; // Negative Karma for a boss-tier villain

            VirtualArmor = 90; // Enhanced armor

            // Attach a random ability for additional gameplay variety
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
            // Additional boss-specific logic can be added here
        }

        public StrixBoss(Serial serial) : base(serial)
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
