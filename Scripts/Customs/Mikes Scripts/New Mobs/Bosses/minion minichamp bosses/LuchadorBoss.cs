using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the luchador overlord")]
    public class LuchadorBoss : Luchador
    {
        [Constructable]
        public LuchadorBoss() : base()
        {
            Name = "Luchador Overlord";
            Title = "the Supreme Luchador";

            // Enhance stats to match or exceed Barracoon-like values
            SetStr(1000); // Maximum strength for a boss
            SetDex(300); // Maximum dexterity for a boss
            SetInt(150); // Keep intelligence moderate

            SetHits(12000); // Higher health like a boss-tier creature
            SetDamage(30, 45); // Increased damage

            // Enhance resistances for a boss-tier creature
            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 65, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Enhance skills for a boss-tier creature
            SetSkill(SkillName.Anatomy, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 120.0, 140.0);
            SetSkill(SkillName.Ninjitsu, 90.0, 110.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 70; // Higher armor for a boss-tier creature

            // Attach a random ability to this boss
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
            // Additional boss-specific logic could be added here
        }

        public LuchadorBoss(Serial serial) : base(serial)
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
