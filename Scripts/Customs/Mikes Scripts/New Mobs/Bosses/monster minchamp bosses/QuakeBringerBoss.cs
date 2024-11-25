using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a quake bringer corpse")]
    public class QuakeBringerBoss : QuakeBringer
    {
        [Constructable]
        public QuakeBringerBoss()
            : base()
        {
            // Set name and title for boss version
            Name = "Quake Bringer";
            Title = "the Earthshaker";

            // Enhance stats to make it a boss (matching or exceeding Barracoon's stats)
            SetStr(1200); // Enhanced strength
            SetDex(255); // Enhanced dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Enhanced health
            SetDamage(29, 38); // Enhanced damage

            SetResistance(ResistanceType.Physical, 75, 85); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced resist skills
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90; // Enhanced armor

            // Attach the XmlRandomAbility to add dynamic abilities
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

            // Boss-specific logic or enhancements can be added here if needed
        }

        public QuakeBringerBoss(Serial serial)
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
