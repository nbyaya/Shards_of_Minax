using System;
using Server;
using Server.Items;
using Server.Spells;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of Minax the Time Overlord")]
    public class MinaxSorceressBoss : MinaxSorceress
    {
        [Constructable]
        public MinaxSorceressBoss() : base()
        {
            Name = "Minax the Time Overlord";
            Title = "the Supreme Sorceress";

            // Update stats to match or exceed Barracoon (or better)
            SetStr(1200); // Increased strength for a boss
            SetDex(255); // Maxed dexterity
            SetInt(250); // Increased intelligence

            SetHits(12000); // Boss-level health

            SetDamage(40, 50); // Boss-level damage

            SetResistance(ResistanceType.Physical, 80, 90); // Higher resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 70, 80);

            SetSkill(SkillName.MagicResist, 150.0); // Increased magic resist
            SetSkill(SkillName.Tactics, 120.0); // Maxed tactics
            SetSkill(SkillName.Wrestling, 120.0); // Maxed wrestling

            Fame = 22500;
            Karma = -22500;

            VirtualArmor = 80; // Increased armor

            // Attach a random ability for extra variety in fights
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

        public MinaxSorceressBoss(Serial serial) : base(serial)
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
