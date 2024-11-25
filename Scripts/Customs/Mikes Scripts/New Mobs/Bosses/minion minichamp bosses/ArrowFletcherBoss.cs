using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master arrow fletcher")]
    public class ArrowFletcherBoss : ArrowFletcher
    {
        [Constructable]
        public ArrowFletcherBoss() : base()
        {
            Name = "Master Arrow Fletcher";
            Title = "the Grand Archer";

            // Enhanced stats to make this a boss version
            SetStr(425); // Increased strength compared to the original
            SetDex(350); // Increased dexterity for improved archery and speed
            SetInt(200); // Increased intelligence

            SetHits(12000); // Increased health compared to the original
            SetDamage(20, 40); // Higher damage range for a boss

            SetResistance(ResistanceType.Physical, 75, 85); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 50, 70);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Archery, 120.0); // Max skill in Archery
            SetSkill(SkillName.Tactics, 100.0); // High skill in Tactics
            SetSkill(SkillName.Anatomy, 90.0); // High skill in Anatomy
            SetSkill(SkillName.MagicResist, 80.0); // Increased Magic Resist

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased karma (more negative for a boss)

            VirtualArmor = 70; // Higher virtual armor

            // Attach a random ability for extra power
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

        public ArrowFletcherBoss(Serial serial) : base(serial)
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
