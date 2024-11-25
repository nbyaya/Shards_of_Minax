using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the dark Sith overlord")]
    public class SithBoss : Sith
    {
        [Constructable]
        public SithBoss() : base()
        {
            Name = "Sith Overlord";
            Title = "the Dark Lord";

            // Update stats to match or exceed Barracoon (as a high boss tier)
            SetStr(1200); // Upper-bound strength
            SetDex(255); // Upper-bound dexterity
            SetInt(250); // Upper-bound intelligence

            SetHits(12000); // Boss-level health
            SetDamage(29, 38); // Increased damage range

            // Increased resistances
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 60);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 100.0); // Enhanced magery skill

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Increased virtual armor for higher defense

            // Attach the XmlRandomAbility to provide dynamic enhancements
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

            // Additional boss logic or behavior could go here
        }

        public SithBoss(Serial serial) : base(serial)
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
