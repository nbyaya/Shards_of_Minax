using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a bone golem corpse")]
    public class BossBoneGolem : BoneGolem
    {
        [Constructable]
        public BossBoneGolem() : base()
        {
            Name = "Bone Golem";
            Title = "the Immense";
            Hue = 2120; // Bone-like hue for consistency
            Body = 752; // Golem body

            SetStr(850); // Enhanced strength
            SetDex(150); // Enhanced dexterity
            SetInt(200); // Enhanced intelligence

            SetHits(12000); // Boss-level health
            SetDamage(40, 60); // Increased damage

            SetResistance(ResistanceType.Physical, 70, 90); // Increased resistances
            SetResistance(ResistanceType.Fire, 50, 70);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 70, 90);
            SetResistance(ResistanceType.Energy, 60, 80);

            SetSkill(SkillName.MagicResist, 100.0); // Enhanced resist
            SetSkill(SkillName.Tactics, 120.0, 130.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 120.0, 130.0); // Enhanced wrestling

            Fame = 25000; // Increased fame
            Karma = -25000; // Increased karma for a boss

            VirtualArmor = 75; // Boss-level armor

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original abilities
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            PackGold(2000, 3000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More iron ingots for a boss
        }

        public BossBoneGolem(Serial serial) : base(serial)
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
