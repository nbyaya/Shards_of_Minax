using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the star overlord")]
    public class StarCitizenBoss : StarCitizen
    {
        [Constructable]
        public StarCitizenBoss() : base()
        {
            Name = "Star Overlord";
            Title = "the Cosmic Ruler";

            // Enhanced stats to make the boss tougher
            SetStr(1200); // Enhanced strength for the boss
            SetDex(255);  // Maximum dexterity
            SetInt(250);  // Maximum intelligence

            SetHits(12000); // High health to match a boss
            SetDamage(30, 50); // Enhanced damage

            // Improved resistances to make it a more challenging opponent
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 70, 90);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Boss-level fame
            Karma = -30000; // Evil alignment

            VirtualArmor = 90; // High virtual armor to mitigate damage

            // Attach a random ability for added unpredictability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Drop 5 MaxxiaScrolls in addition to normal loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss behavior can be added here if desired
        }

        public StarCitizenBoss(Serial serial) : base(serial)
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
