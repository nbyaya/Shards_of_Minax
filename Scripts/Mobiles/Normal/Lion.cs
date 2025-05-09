using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a lion corpse")]
    public class Lion : BaseCreature
    {
        public override double HealChance { get { return .167; } }

        [Constructable]
        public Lion()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Lion";
            Body = 0x592;
            Female = true;
            BaseSoundID = 0x3EF;

            SetStr(710, 720);
            SetDex(200, 220);
            SetInt(120, 140);

            SetHits(350, 370);

            SetDamage(16, 22);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 35, 45);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 40);

            SetSkill(SkillName.Parry, 90.0, 100.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.DetectHidden, 80.0);

            Fame = 11000;
            Karma = -11000;
            
            Tamable = true;
            ControlSlots = 2;
            MinTameSkill = 96.0;

            SetMagicalAbility(MagicalAbility.Piercing);
        }

        public override int GetIdleSound() { return 0x673; }
        public override int GetAngerSound() { return 0x670; }
        public override int GetHurtSound() { return 0x672; }
        public override int GetDeathSound() { return 0x671; }

        public override double WeaponAbilityChance { get { return 0.5; } }
        
        public override int Hides { get { return 11; } }
        public override HideType HideType { get { return HideType.Regular; } }
        public override int Meat { get { return 5; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 1);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new LionheartMantle());
            }			
        }

        public override bool CanAngerOnTame { get { return true; } }
        public override bool StatLossAfterTame { get { return true; } }

        public Lion(Serial serial)
            : base(serial)
        {
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
            {
                SetWeaponAbility(WeaponAbility.ArmorIgnore);
                SetWeaponAbility(WeaponAbility.BleedAttack);
                SetWeaponAbility(WeaponAbility.ParalyzingBlow);
            }
        }
    }
}