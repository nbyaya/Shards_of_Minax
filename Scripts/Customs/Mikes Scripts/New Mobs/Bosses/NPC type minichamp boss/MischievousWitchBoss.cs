using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the supreme witch")]
    public class MischievousWitchBoss : MischievousWitch
    {
        [Constructable]
        public MischievousWitchBoss() : base()
        {
            Name = "Supreme Witch";
            Title = "the Dark Enchantress";

            // Update stats to match or exceed Barracoon
            SetStr(1200);  // Increased strength
            SetDex(255);   // Max dexterity
            SetInt(250);   // Max intelligence

            SetHits(12000); // Increased health to match Barracoon's tier
            SetDamage(20, 40); // Increased damage range

            // Enhanced resistances
            SetResistance(ResistanceType.Physical, 80);
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 110.0);
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 100.0);
            SetSkill(SkillName.Wrestling, 100.0);

            Fame = 22500; // Boss-tier fame
            Karma = -22500; // Negative karma for the boss

            VirtualArmor = 70; // Increased armor value

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

        public MischievousWitchBoss(Serial serial) : base(serial)
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
