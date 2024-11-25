using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the ska skald overlord")]
    public class SkaSkaldBoss : SkaSkald
    {
        [Constructable]
        public SkaSkaldBoss() : base()
        {
            Name = "Ska Skald Overlord";
            Title = "the Legendary Ska Skald";

            // Update stats to match or exceed Barracoon-like values for a boss
            SetStr(1200); // Enhanced strength
            SetDex(255);  // Enhanced dexterity
            SetInt(250);  // Enhanced intelligence

            SetHits(12000); // Increased health
            SetDamage(29, 38); // Increased damage

            SetResistance(ResistanceType.Physical, 75, 85); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 75, 85);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0); // Enhanced wrestling
            SetSkill(SkillName.Magery, 100.0); // Magery skill (still a mage type)

            Fame = 22500; // Increased fame
            Karma = -22500; // Increased karma (boss-level negative karma)

            VirtualArmor = 80; // Enhanced virtual armor

            // Attach a random ability for extra flair
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

        public SkaSkaldBoss(Serial serial) : base(serial)
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
