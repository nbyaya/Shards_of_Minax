using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the galactic overlord")]
    public class ChrisRobertsBoss : ChrisRoberts
    {
        [Constructable]
        public ChrisRobertsBoss() : base()
        {
            Name = "Galactic Overlord Chris Roberts";
            Title = "the Master of the Stars";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Enhanced strength
            SetDex(255);  // Enhanced dexterity
            SetInt(250);  // Enhanced intelligence

            SetHits(10000); // Increase health to make it a boss-tier creature
            SetDamage(40, 60); // Increase damage for a tougher challenge

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 75, 85); // Increased resistances for a boss-tier creature
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Magery, 110.0, 120.0); // Enhanced skill levels for magic-related abilities
            SetSkill(SkillName.MagicResist, 150.0); // Boss-level resistances
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics for a more dangerous boss
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling skill

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Higher armor for better protection

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
            // Additional boss-specific logic could go here, like casting powerful spells or summoning stronger creatures
        }

        public ChrisRobertsBoss(Serial serial) : base(serial)
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
