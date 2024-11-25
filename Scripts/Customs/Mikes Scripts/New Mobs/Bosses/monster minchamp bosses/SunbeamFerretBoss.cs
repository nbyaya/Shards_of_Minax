using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a sunbeam ferret boss corpse")]
    public class SunbeamFerretBoss : SunbeamFerret
    {
        [Constructable]
        public SunbeamFerretBoss()
            : base()
        {
            // Update name and title for the boss version
            Name = "Sunbeam Ferret";
            Title = "the Radiant Overlord";

            // Update stats to match or exceed the original values
            SetStr(1200, 1500); // Higher strength for the boss
            SetDex(255, 300);   // Higher dexterity for the boss
            SetInt(250, 350);   // Higher intelligence for the boss

            SetHits(15000);     // Boss-level health
            SetDamage(40, 50);  // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 30);

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.EvalInt, 120.0, 140.0);
            SetSkill(SkillName.Magery, 120.0, 140.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000;  // Higher fame for boss
            Karma = -30000; // Higher karma for boss

            VirtualArmor = 120;  // Increased virtual armor for the boss

            Tamable = false;  // Boss cannot be tamed
            ControlSlots = 0; // No control slots

            // Attach a random ability for added challenge
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        // Override the loot generation method to include 5 MaxxiaScrolls
        public override void GenerateLoot()
        {
            base.GenerateLoot();
            PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        // Override OnThink method if needed for special logic
        public override void OnThink()
        {
            base.OnThink();
            // Boss-specific behaviors or timing tweaks can be added here if necessary
        }

        public SunbeamFerretBoss(Serial serial)
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
