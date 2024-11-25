using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the armor protector")]
    public class ArmorCurerBoss : ArmorCurer
    {
        [Constructable]
        public ArmorCurerBoss() : base()
        {
            Name = "Armor Protector";
            Title = "the Supreme Curer";

            // Enhanced stats to match or exceed those of Barracoon
            SetStr(700);  // Enhanced Strength
            SetDex(150);  // Enhanced Dexterity
            SetInt(250);  // Enhanced Intelligence

            SetHits(6000);  // Increased health
            SetDamage(10, 20);  // Increased damage range

            // Enhanced resistances to make the boss more challenging
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 60, 70);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // Enhanced skills for higher difficulty
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 120.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 60.0, 80.0);

            Fame = 10000;  // Increased fame
            Karma = -10000;  // Increased karma (negative since it's a boss)

            VirtualArmor = 60;  // Increased virtual armor for more defense

            // Attach random ability
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

        public ArmorCurerBoss(Serial serial) : base(serial)
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
