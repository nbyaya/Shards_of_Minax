using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a whirlwind fiend boss corpse")]
    public class WhirlwindFiendBoss : WhirlwindFiend
    {
        [Constructable]
        public WhirlwindFiendBoss() : base()
        {
            Name = "Whirlwind Fiend Boss";
            Title = "the Stormbringer";
            Hue = 1155; // Darker hue for chaos, could change for further distinction
            BaseSoundID = 655;

            // Update stats to match or exceed Barracoon's values
            SetStr(1200); // Enhanced strength
            SetDex(255); // Maximum dexterity
            SetInt(250); // Maximum intelligence

            SetHits(12000); // Increased health to boss tier
            SetDamage(35, 50); // Increased damage range

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 170.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

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
            // Additional boss logic could be added here if desired
        }

        public WhirlwindFiendBoss(Serial serial) : base(serial)
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
