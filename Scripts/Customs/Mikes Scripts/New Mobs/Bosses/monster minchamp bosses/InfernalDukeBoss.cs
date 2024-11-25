using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an infernal duke corpse")]
    public class InfernalDukeBoss : InfernalDuke
    {
        [Constructable]
        public InfernalDukeBoss()
            : base()
        {
            // Enhanced stats, making it a boss-tier creature
            Name = "Infernal Duke";
            Title = "the Supreme Overlord";

            SetStr(1200); // Enhanced strength
            SetDex(255); // Enhanced dexterity
            SetInt(250); // Enhanced intelligence

            SetHits(12000); // Increased health
            SetDamage(35, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false;
            ControlSlots = 0;

            // Attach XmlRandomAbility for dynamic abilities
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
            // Additional boss-specific behavior can be added here if necessary
        }

        public InfernalDukeBoss(Serial serial) : base(serial)
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
