using System;
using Server.Network;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a war horse corpse")]
    public class IronWarHorse : BaseMount
    {
        [Constructable]
        public IronWarHorse()
            : this("a War Horse")
        {
        }

        [Constructable]
        public IronWarHorse(string name)
            : base(name, 0x11C, 284, AIType.AI_Mage, FightMode.Evil, 10, 1, 0.2, 0.4)
        {
            this.BaseSoundID = 0x3C5;
            this.Hue = 2401;

            this.SetStr(296, 325);
            this.SetDex(86, 105);
            this.SetInt(186, 225);

            this.SetHits(191, 210);

            this.SetDamage(16, 22);

            this.SetDamageType(ResistanceType.Physical, 70);
            this.SetDamageType(ResistanceType.Fire, 10);
            this.SetDamageType(ResistanceType.Cold, 10);
            this.SetDamageType(ResistanceType.Energy, 10);

            this.SetResistance(ResistanceType.Physical, 55, 65);
            this.SetResistance(ResistanceType.Fire, 35, 45);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 25, 35);
            this.SetResistance(ResistanceType.Energy, 25, 35);

            this.SetSkill(SkillName.MagicResist, 85.3, 100.0);
            this.SetSkill(SkillName.Tactics, 20.1, 22.5);
            this.SetSkill(SkillName.Wrestling, 80.5, 92.5);

            this.Fame = 9000;
            this.Karma = 9000;

            this.Tamable = true;
            this.ControlSlots = 2;
            this.MinTameSkill = 95.1;
        }

        public IronWarHorse(Serial serial)
            : base(serial)
        {
        }

        public override bool AllowFemaleRider
        {
            get
            {
                return false;
            }
        }
        public override bool AllowFemaleTamer
        {
            get
            {
                return false;
            }
        }
        public override bool InitialInnocent
        {
            get
            {
                return true;
            }
        }

        public override int Meat
        {
            get
            {
                return 3;
            }
        }
        public override int Hides
        {
            get
            {
                return 10;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Horned;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.FruitsAndVegies | FoodType.GrainsAndHay;
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Potions);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)1); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (version == 0)
                this.AI = AIType.AI_Mage;
        }
    }
}
