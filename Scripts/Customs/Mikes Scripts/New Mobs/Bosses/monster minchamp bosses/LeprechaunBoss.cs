using System;
using System.Collections.Generic;
using Server.Items;
using Server.Network;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a leprechaun overlord's corpse")]
    public class LeprechaunBoss : Leprechaun
    {
        [Constructable]
        public LeprechaunBoss() : base()
        {
            Name = "Leprechaun Overlord";
            Title = "the Grand Trickster";

            // Enhance stats to match a boss-level challenge
            SetStr(1200); // Increased strength
            SetDex(255); // Max dexterity
            SetInt(250); // Max intelligence

            SetHits(12000); // Increased health
            SetDamage(40, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);
            SetSkill(SkillName.EvalInt, 120.0);

            Fame = 24000;
            Karma = -24000;

            VirtualArmor = 90;

            // Attach a random ability
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

        public LeprechaunBoss(Serial serial) : base(serial)
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
