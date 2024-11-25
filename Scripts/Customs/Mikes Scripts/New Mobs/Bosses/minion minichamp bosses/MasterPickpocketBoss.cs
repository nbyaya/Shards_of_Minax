using System;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master pickpocket overlord")]
    public class MasterPickpocketBoss : MasterPickpocket
    {
        [Constructable]
        public MasterPickpocketBoss() : base()
        {
            Name = "Master Pickpocket Overlord";
            Title = "the Supreme Thief";

            // Update stats to match or exceed the original
            SetStr(500, 600); // Higher strength
            SetDex(800, 900); // Higher dexterity
            SetInt(300, 400); // Higher intelligence

            SetHits(12000); // Boss-level health
            SetDamage(15, 25); // Increased damage

            SetResistance(ResistanceType.Physical, 65, 80); // Higher resistances
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            SetSkill(SkillName.Fencing, 120.0); // Increased skills
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Stealing, 120.0);
            SetSkill(SkillName.Snooping, 120.0);

            Fame = 10000; // Boss-level fame
            Karma = -10000; // Boss-level karma

            VirtualArmor = 80; // Higher virtual armor

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
            // Additional boss logic could be added here, if needed
        }

        public MasterPickpocketBoss(Serial serial) : base(serial)
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
