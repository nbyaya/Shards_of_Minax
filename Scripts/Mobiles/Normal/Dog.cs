using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a dog corpse")]
    public class Dog : BaseCreature
    {
        [Constructable]
        public Dog()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a dog";
            Body = 0xD9;
            Hue = Utility.RandomAnimalHue();
            BaseSoundID = 0x85;

            SetStr(27, 37);
            SetDex(28, 43);
            SetInt(29, 37);

            SetHits(17, 22);
            SetMana(0);

            SetDamage(4, 7);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 10, 15);

            SetSkill(SkillName.MagicResist, 22.1, 47.0);
            SetSkill(SkillName.Tactics, 19.2, 31.0);
            SetSkill(SkillName.Wrestling, 19.2, 31.0);

            Fame = 0;
            Karma = 300;

            VirtualArmor = 12;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -21.3;
        }

        public Dog(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 1;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Meat;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Canine;
            }
        }
		
        public override void GenerateLoot()
        {		
            if (Utility.RandomDouble() < 0.01) // 1 in 1000 chance
            {
                this.PackItem(new PackmastersCloak());
            }
		}			
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}