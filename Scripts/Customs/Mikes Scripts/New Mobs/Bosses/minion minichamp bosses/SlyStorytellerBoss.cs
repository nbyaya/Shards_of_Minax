using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the sly storyteller overlord")]
    public class SlyStorytellerBoss : SlyStoryteller
    {
        [Constructable]
        public SlyStorytellerBoss() : base()
        {
            Name = "Sly Storyteller Overlord";
            Title = "the Master of Tales";

            // Update stats to match or exceed Barracoon for a boss-level difficulty
            SetStr(700, 900);  // Enhanced strength
            SetDex(150, 200);  // Enhanced dexterity
            SetInt(700, 900);  // Enhanced intelligence

            SetHits(12000);    // Enhanced health to make it a challenging boss
            SetDamage(15, 25); // Enhanced damage for a more threatening boss fight

            // Adjust resistances to be higher for boss-level challenge
            SetResistance(ResistanceType.Physical, 60, 75);
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 60, 80);
            SetResistance(ResistanceType.Energy, 50, 70);

            // Skills also enhanced for a boss encounter
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 115.0, 125.0);
            SetSkill(SkillName.Meditation, 110.0, 120.0);
            SetSkill(SkillName.MagicResist, 130.0, 150.0);
            SetSkill(SkillName.Wrestling, 90.0, 120.0);

            Fame = 15000;  // Increased fame for a high-tier boss
            Karma = -15000; // Negative karma for an evil character

            VirtualArmor = 75;  // Increased virtual armor for higher defense

            // Attach a random ability to the boss using XmlRandomAbility
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

        public SlyStorytellerBoss(Serial serial) : base(serial)
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
