using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a powerful living grimoire corpse")]
    public class GrimorieBoss : Grimorie
    {
        [Constructable]
        public GrimorieBoss() : base()
        {
            Name = "Grimorie the Supreme Grimoire";
            Title = "the Ancient One";

            // Enhance stats to match boss-level power
            SetStr(1200); // Increased strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Higher health than normal Grimorie
            SetDamage(35, 50); // Increased damage output

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 70.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0);
            SetSkill(SkillName.Tactics, 120.0, 140.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame value
            Karma = -30000; // Higher karma penalty

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false; // This boss should not be tamable
            ControlSlots = 0;

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
            // Additional boss logic could be added here
        }

        public GrimorieBoss(Serial serial) : base(serial)
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
