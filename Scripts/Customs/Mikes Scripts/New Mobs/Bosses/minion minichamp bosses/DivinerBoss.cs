using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the divination master")]
    public class DivinerBoss : Diviner
    {
        [Constructable]
        public DivinerBoss() : base()
        {
            Name = "Divination Master";
            Title = "the Grand Diviner";

            // Update stats to match or exceed Barracoon (or appropriate boss stats)
            SetStr(700); // Stronger strength for the boss
            SetDex(150); // Upper dexterity from Barracoon
            SetInt(400); // Intelligence increased for the boss

            SetHits(12000); // Increased health to match boss tier
            SetDamage(29, 38); // Higher damage for the boss

            SetDamageType(ResistanceType.Physical, 50); // Balanced damage types
            SetDamageType(ResistanceType.Energy, 50); // Include energy damage

            // Improved resistances to match boss tier
            SetResistance(ResistanceType.Physical, 70, 75);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Boss-level skill
            SetSkill(SkillName.Magery, 100.0, 120.0); // Magery for powerful magic abilities
            SetSkill(SkillName.Meditation, 60.0, 80.0); // Higher meditation skill
            SetSkill(SkillName.MagicResist, 95.0, 120.0); // Improved magic resist
            SetSkill(SkillName.Tactics, 80.0, 100.0); // Boss tactics
            SetSkill(SkillName.Wrestling, 60.0, 80.0); // Some wrestling skill for defense

            Fame = 22500; // High fame for a boss
            Karma = -22500; // Negative karma for a villainous boss

            VirtualArmor = 60; // Higher armor for the boss

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
			PackItem(new RandomMagicWeapon());
			PackItem(new RandomMagicArmor());
			PackItem(new RandomMagicClothing());
			PackItem(new RandomMagicJewelry());
			PackItem(new DeerBreathMateria());
			PackItem(new HarpySummoningMateria());            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            base.OnThink();
            // Additional boss logic can be implemented here (e.g., specific abilities)
        }

        public DivinerBoss(Serial serial) : base(serial)
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
