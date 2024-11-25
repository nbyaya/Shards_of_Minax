using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a selkie boss corpse")]
    public class SelkieBoss : Selkie
    {
        [Constructable]
        public SelkieBoss() : base()
        {
            Name = "Selkie Overlord";
            Title = "the Mighty Selkie";

            // Enhance stats to boss-tier levels
            SetStr(1200); // Higher strength
            SetDex(255); // Max dexterity
            SetInt(250); // Higher intelligence

            SetHits(12000); // High health
            SetDamage(35, 45); // Increased damage

            SetResistance(ResistanceType.Physical, 75, 90); // Stronger resistances
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 50.0, 100.0); // Increased skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0); // Max Magic Resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 25000; // Increased fame
            Karma = -25000; // Increased karma (evil)

            VirtualArmor = 100; // High virtual armor

            Tamable = false; // Bosses cannot be tamed
            ControlSlots = 0; // Cannot be controlled or tamed

            // Attach the XmlRandomAbility for a special random ability
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
            // Additional boss logic could be added here, if necessary
        }

        public SelkieBoss(Serial serial) : base(serial)
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
