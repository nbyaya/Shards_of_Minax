using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss stone golem corpse")]
    public class BossStoneGolem : StoneGolem
    {
        [Constructable]
        public BossStoneGolem()
        {
            Name = "Stone Golem";
            Title = "the Colossus";
            Hue = 1924; // Unique hue for the boss Stone Golem
            Body = 752; // Standard Golem body

            SetStr(1500, 1800); // Enhanced strength
            SetDex(255, 300);  // Enhanced dexterity
            SetInt(350, 450);  // Enhanced intelligence

            SetHits(15000, 18000); // Boss-level health

            SetDamage(45, 60); // Increased damage

            SetDamageType(ResistanceType.Physical, 60); // Increased physical damage
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90); // Boss-level resistances
            SetResistance(ResistanceType.Fire, 75, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.1, 80.0);  // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.1, 150.0);
            SetSkill(SkillName.Magery, 100.5, 150.0);
            SetSkill(SkillName.Meditation, 50.1, 75.0);
            SetSkill(SkillName.MagicResist, 150.5, 180.0);
            SetSkill(SkillName.Tactics, 100.1, 150.0);
            SetSkill(SkillName.Wrestling, 100.1, 150.0);

            Fame = 30000; // Increased fame for the boss
            Karma = -30000; // Negative karma, as it is a boss

            VirtualArmor = 100; // Increased armor

            Tamable = false;
            ControlSlots = 1;

            // Attach the random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retain the base loot
            this.Say("You cannot defeat the might of the Stone Golem!");
            PackGold(2000, 2500); // Enhanced loot drop
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain the base behavior and abilities
        }

        public BossStoneGolem(Serial serial) : base(serial)
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
