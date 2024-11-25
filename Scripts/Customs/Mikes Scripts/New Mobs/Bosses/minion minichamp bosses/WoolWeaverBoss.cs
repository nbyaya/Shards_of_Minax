using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the wool overlord")]
    public class WoolWeaverBoss : WoolWeaver
    {
        [Constructable]
        public WoolWeaverBoss() : base()
        {
            Name = "Wool Overlord";
            Title = "the Supreme Weaver";

            // Enhance stats to match or exceed Barracoon's or higher
            SetStr(800); // Higher strength
            SetDex(150); // Upper dexterity
            SetInt(600); // Upper intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(25, 35); // Enhanced damage

            SetResistance(ResistanceType.Physical, 75, 85); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.Anatomy, 75.0, 100.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 75.0, 100.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 75.0, 100.0);

            Fame = 22500; // Higher fame
            Karma = -22500; // Higher karma (negative for a boss-tier creature)

            VirtualArmor = 75; // Higher virtual armor

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

        public WoolWeaverBoss(Serial serial) : base(serial)
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
