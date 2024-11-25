using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a milking demon boss corpse")]
    public class MilkingDemonBoss : MilkingDemon
    {
        [Constructable]
        public MilkingDemonBoss() : base()
        {
            Name = "The Milking Demon Overlord";
            Title = "the Supreme Milker";

            // Update stats to match or exceed Barracoon
            SetStr(1200); // Higher strength than the original
            SetDex(255); // Higher dexterity than the original
            SetInt(250); // Keep intelligence as is or higher

            SetHits(12000); // Make it a true boss with much more health
            SetDamage(40, 50); // Boost damage for a higher challenge

            // Enhance resistances to make it a tough encounter
            SetResistance(ResistanceType.Physical, 80, 100);
            SetResistance(ResistanceType.Fire, 80, 100);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60, 80);

            // Increase skills to make it more formidable
            SetSkill(SkillName.Anatomy, 50.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 100;

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

        public MilkingDemonBoss(Serial serial) : base(serial)
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
