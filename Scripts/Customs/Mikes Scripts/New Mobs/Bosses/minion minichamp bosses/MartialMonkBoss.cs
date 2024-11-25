using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the martial master")]
    public class MartialMonkBoss : MartialMonk
    {
        [Constructable]
        public MartialMonkBoss() : base()
        {
            Name = "Martial Master";
            Title = "the Supreme Monk";

            // Update stats to match or exceed Barracoon (or better if already superior)
            SetStr(900, 1200); // Enhanced strength
            SetDex(300, 400); // Enhanced dexterity
            SetInt(350, 500); // Enhanced intelligence

            SetHits(12000); // Enhanced health

            SetDamage(25, 40); // Enhanced damage

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 120.0);
            SetSkill(SkillName.Meditation, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 15000;  // Increased fame
            Karma = 15000; // Increased karma

            VirtualArmor = 80; // Enhanced armor

            // Attach a random ability to the boss
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

        public MartialMonkBoss(Serial serial) : base(serial)
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
