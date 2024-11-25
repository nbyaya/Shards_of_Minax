using System;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an inferno overlord corpse")]
    public class InfernoWardenBoss : InfernoWarden
    {
        [Constructable]
        public InfernoWardenBoss()
            : base()
        {
            // Boss name and title
            Name = "Inferno Overlord";
            Title = "the Eternal Flame";

            // Enhanced stats
            SetStr(1200); // Enhanced strength
            SetDex(255);  // Max dexterity
            SetInt(250);  // High intelligence

            SetHits(12000); // High health
            SetDamage(35, 45); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Higher Magic Resist skill
            SetSkill(SkillName.Tactics, 100.0);    // Enhanced tactics
            SetSkill(SkillName.Wrestling, 100.0);  // Higher wrestling skill

            Fame = 30000; // Higher fame for a boss-tier creature
            Karma = -30000; // Negative karma to make it a villainous boss

            VirtualArmor = 100; // Enhanced virtual armor

            Tamable = false; // It is a boss and cannot be tamed
            MinTameSkill = 100.0; // For clarity, but taming is disabled

            // Attach the XmlRandomAbility for random special abilities
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            // Inherit the original loot generation and add 5 MaxxiaScrolls
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
            // Additional boss behavior logic can be added here
        }

        public InfernoWardenBoss(Serial serial)
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
