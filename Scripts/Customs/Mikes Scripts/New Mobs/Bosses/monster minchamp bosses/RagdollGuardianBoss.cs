using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a ragdoll guardian boss corpse")]
    public class RagdollGuardianBoss : RagdollGuardian
    {
        [Constructable]
        public RagdollGuardianBoss()
            : base()
        {
            Name = "Ragdoll Guardian Boss";
            Title = "the Supreme Guardian";
            Hue = 1298; // Unique hue (same as the original)

            // Enhance stats to match a boss tier creature
            SetStr(1200, 1500); // Increased strength
            SetDex(255, 300); // Increased dexterity
            SetInt(250, 350); // Increased intelligence

            SetHits(15000); // Boss-level health
            SetDamage(45, 55); // Increased damage

            // Resistance values also boosted
            SetResistance(ResistanceType.Physical, 85, 95);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 120);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Magery, 115.0, 130.0); // Higher magic skill
            SetSkill(SkillName.MagicResist, 150.0, 160.0); // Better magic resistance
            SetSkill(SkillName.Tactics, 110.0, 130.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Increased wrestling skill

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma (boss-level)

            VirtualArmor = 120; // Increased virtual armor

            // Attach random abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override the loot generation to include 5 MaxxiaScrolls
        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
            // Drop 5 MaxxiaScrolls in addition to the regular loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Boss-level AI and behavior can be added here if desired
        }

        public RagdollGuardianBoss(Serial serial)
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
