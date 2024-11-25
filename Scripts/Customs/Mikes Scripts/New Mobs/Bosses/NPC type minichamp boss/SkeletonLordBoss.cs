using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the skeleton overlord")]
    public class SkeletonLordBoss : SkeletonLord
    {
        [Constructable]
        public SkeletonLordBoss() : base()
        {
            Name = "Skeleton Overlord";
            Title = "the Supreme Undead";

            // Update stats to match or exceed Barracoon or improve upon them
            SetStr(1200); // Increased strength
            SetDex(255);  // Increased dexterity
            SetInt(250);  // Increased intelligence

            SetHits(12000); // Boss-level health
            SetDamage(30, 50); // Increased damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 80);  // Higher physical resistance
            SetResistance(ResistanceType.Fire, 70, 80);     // Higher fire resistance
            SetResistance(ResistanceType.Cold, 60, 75);     // Higher cold resistance
            SetResistance(ResistanceType.Poison, 100);      // Maintains full poison resistance
            SetResistance(ResistanceType.Energy, 50, 60);   // Higher energy resistance

            SetSkill(SkillName.MagicResist, 150.0); // Maxing out Magic Resist for the boss
            SetSkill(SkillName.Tactics, 120.0);     // Increased tactics skill
            SetSkill(SkillName.Wrestling, 120.0);   // Increased wrestling skill
            SetSkill(SkillName.Magery, 120.0);      // Increased magery skill
            SetSkill(SkillName.Meditation, 100.0);  // Increased meditation for mana regen

            Fame = 22500; // Increased fame for a boss
            Karma = -22500; // Negative karma for the boss

            VirtualArmor = 75; // Higher virtual armor for the boss

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

            // Additional boss behavior could be added here, like using special attacks or summoning stronger minions.
        }

        public SkeletonLordBoss(Serial serial) : base(serial)
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
