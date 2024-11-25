using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("an inferno python corpse")]
    public class InfernoPythonBoss : InfernoPython
    {
        [Constructable]
        public InfernoPythonBoss() : base()
        {
            Name = "Inferno Python Overlord";
            Title = "the Supreme Serpent";

            // Enhance the stats to make it a boss
            SetStr(1200); // Increased Strength
            SetDex(255); // Maximum Dexterity
            SetInt(250); // Maximum Intelligence

            SetHits(12000); // Increased Health
            SetDamage(35, 45); // Increased Damage

            // Increase resistance values
            SetResistance(ResistanceType.Physical, 80, 95);
            SetResistance(ResistanceType.Fire, 85, 95);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 85, 95);
            SetResistance(ResistanceType.Energy, 60, 75);

            // Enhance skill levels
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 110; // Higher armor

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

        public InfernoPythonBoss(Serial serial) : base(serial)
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
