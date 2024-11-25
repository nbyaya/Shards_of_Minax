using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the karate master")]
    public class KarateExpertBoss : KarateExpert
    {
        [Constructable]
        public KarateExpertBoss() : base()
        {
            Name = "Karate Master";
            Title = "the Supreme Karate Expert";

            // Enhance stats to match or exceed the original values
            SetStr(1000); // Enhanced strength
            SetDex(300); // Enhanced dexterity
            SetInt(150); // Enhanced intelligence

            SetHits(12000); // Enhanced health
            SetDamage(25, 45); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 70, 80); // Increased resistances
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Enhanced skills
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);
            SetSkill(SkillName.MagicResist, 95.0, 115.0);

            Fame = 15000; // Increased fame
            Karma = -15000; // Increased karma loss for being a boss

            VirtualArmor = 75; // Increased virtual armor

            // Attach the random ability for extra power
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

            // Optionally, you can add custom loot or messages here if needed
        }

        public KarateExpertBoss(Serial serial) : base(serial)
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
