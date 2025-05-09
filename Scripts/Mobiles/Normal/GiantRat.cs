using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a giant rat corpse")]
    [TypeAlias("Server.Mobiles.Giantrat")]
    public class GiantRat : BaseCreature
    {
        [Constructable]
        public GiantRat()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a giant rat";
            this.Body = 0xD7;
            this.BaseSoundID = 0x188;

            this.SetStr(32, 74);
            this.SetDex(46, 65);
            this.SetInt(16, 30);

            this.SetHits(26, 39);
            this.SetMana(0);

            this.SetDamage(4, 8);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 15, 20);
            this.SetResistance(ResistanceType.Fire, 5, 10);
            this.SetResistance(ResistanceType.Poison, 25, 35);

            this.SetSkill(SkillName.MagicResist, 25.1, 30.0);
            this.SetSkill(SkillName.Tactics, 29.3, 44.0);
            this.SetSkill(SkillName.Wrestling, 29.3, 44.0);

            this.Fame = 300;
            this.Karma = -300;

            this.VirtualArmor = 18;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = 29.1;
        }

        public GiantRat(Serial serial)
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
        public override int Hides
        {
            get
            {
                return 6;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Fish | FoodType.Meat | FoodType.FruitsAndVegies | FoodType.Eggs;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Poor);
			
            if (Utility.RandomDouble() < 0.01) // 1 in 1000 chance
            {
                this.PackItem(new RatcatchersSash());
            }			
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}