using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a Persian Shade boss corpse")]
    public class PersianShadeBoss : PersianShade
    {
        [Constructable]
        public PersianShadeBoss() : base()
        {
            Name = "Persian Shade Boss";
            Title = "the Shadow Master";

            // Enhanced stats for the boss
            SetStr(1200); // Enhanced strength
            SetDex(255); // Max dexterity
            SetInt(250); // High intelligence

            SetHits(12000); // High health for the boss
            SetDamage(35, 45); // Increased damage

            // Enhance resistances
            SetResistance(ResistanceType.Physical, 75, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 75);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Improved magic resist
            SetSkill(SkillName.Tactics, 100.0); // High tactics skill
            SetSkill(SkillName.Wrestling, 100.0); // High wrestling skill

            Fame = 24000; // Keep the same fame
            Karma = -24000; // Negative karma for the boss

            VirtualArmor = 100; // Higher armor

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

        public override void OnThink()
        {
            base.OnThink();
            // You could add more boss-specific logic here if needed
        }

        public PersianShadeBoss(Serial serial) : base(serial)
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
