using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the aquatic overlord")]
    public class AquaticTamerBoss : AquaticTamer
    {
        [Constructable]
        public AquaticTamerBoss() : base()
        {
            Name = "Aquatic Overlord";
            Title = "the Supreme Tamer";

            // Enhanced stats to match or exceed Barracoon's
            SetStr(800); // Upper strength
            SetDex(250); // Upper dexterity
            SetInt(500); // Upper intelligence

            SetHits(12000); // Matching Barracoon's health
            SetDamage(25, 40); // Stronger damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 50);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 120.0);
            SetSkill(SkillName.Meditation, 120.0, 150.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 90.0, 110.0);

            Fame = 22500;  // Similar to Barracoon's fame
            Karma = -22500;  // Negative karma as a boss

            VirtualArmor = 60;  // Enhanced armor

            // Attach the random ability
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
            // Additional boss-specific logic could go here
        }

        public AquaticTamerBoss(Serial serial) : base(serial)
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
