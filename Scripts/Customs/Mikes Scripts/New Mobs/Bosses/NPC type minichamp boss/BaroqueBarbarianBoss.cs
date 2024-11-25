using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the baroque barbarian overlord")]
    public class BaroqueBarbarianBoss : BaroqueBarbarian
    {
        [Constructable]
        public BaroqueBarbarianBoss() : base()
        {
            Name = "Baroque Barbarian Overlord";
            Title = "the Supreme Barbarian";

            // Enhance stats to match or exceed Barracoon-like strength
            SetStr(1200); // Upper limit of Barracoon's strength
            SetDex(255); // Upper limit of Barracoon's dexterity
            SetInt(250); // Upper limit of Barracoon's intelligence

            SetHits(10000); // Match or exceed Barracoon's hit points
            SetDamage(35, 50); // Enhance damage for a stronger boss

            SetResistance(ResistanceType.Physical, 80); // Stronger physical resistance
            SetResistance(ResistanceType.Fire, 80); // Stronger fire resistance
            SetResistance(ResistanceType.Cold, 60); // Stronger cold resistance
            SetResistance(ResistanceType.Poison, 100); // Strong poison resistance
            SetResistance(ResistanceType.Energy, 50); // Stronger energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 120.0); // Higher tactics skill
            SetSkill(SkillName.Wrestling, 120.0); // Higher wrestling skill

            Fame = 30000; // Higher fame for the boss
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 70; // Stronger virtual armor

            // Attach a random ability to this boss
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
            // Additional boss behavior can be added here
        }

        public BaroqueBarbarianBoss(Serial serial) : base(serial)
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
