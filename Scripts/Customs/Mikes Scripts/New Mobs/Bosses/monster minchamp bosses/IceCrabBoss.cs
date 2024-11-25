using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ice overlord")]
    public class IceCrabBoss : IceCrab
    {
        [Constructable]
        public IceCrabBoss()
            : base("Ice Overlord Crab")
        {
            // Enhanced stats to match boss-tier requirements
            SetStr(1200); // Higher strength than original
            SetDex(255);  // Higher dexterity than original
            SetInt(250);  // Keep intelligence same, could be enhanced if needed

            SetHits(12000); // High health to match boss-tier
            SetDamage(29, 45); // Increased damage range for difficulty

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            // Higher resistances for toughness
            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resistance
            SetSkill(SkillName.Tactics, 120.0);    // Higher tactics
            SetSkill(SkillName.Wrestling, 120.0);  // Higher wrestling
            SetSkill(SkillName.Magery, 100.0);     // Magery skills for casting
            SetSkill(SkillName.Meditation, 50.0);  // Meditation for extra mana regen

            Fame = 30000; // Higher fame
            Karma = -30000; // Negative karma, boss-tier

            VirtualArmor = 100; // Stronger armor

            // Tamable settings remain the same (boss is untamable)
            Tamable = false;
            ControlSlots = 0;
            
            // Attach the XmlRandomAbility for extra abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Add 5 MaxxiaScrolls in addition to standard loot
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

        public IceCrabBoss(Serial serial)
            : base(serial)
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
