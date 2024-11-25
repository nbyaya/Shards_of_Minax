using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the javelin overlord")]
    public class JavelinAthleteBoss : JavelinAthlete
    {
        [Constructable]
        public JavelinAthleteBoss() : base()
        {
            Name = "Javelin Overlord";
            Title = "the Supreme Javelin Athlete";

            // Update stats to match or exceed Barracoon-like values
            SetStr(700); // Enhanced strength
            SetDex(250); // Enhanced dexterity
            SetInt(150); // Maximum intelligence

            SetHits(12000); // Higher health than original
            SetDamage(29, 38); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.Anatomy, 80.0, 100.0);
            SetSkill(SkillName.Archery, 110.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 100.0);

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased karma

            VirtualArmor = 50; // Enhanced virtual armor

            // Attach the XmlRandomAbility for additional random enhancements
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
            // Additional boss logic could be added here
        }

        public JavelinAthleteBoss(Serial serial) : base(serial)
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
