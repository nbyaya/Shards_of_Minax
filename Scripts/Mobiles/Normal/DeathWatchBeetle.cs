using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a deathwatchbeetle corpse")]
    [TypeAlias("Server.Mobiles.DeathWatchBeetle")]
    public class DeathwatchBeetle : BaseCreature
    {
        [Constructable]
        public DeathwatchBeetle()
            : base(AIType.AI_Melee, Core.ML ? FightMode.Aggressor : FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a deathwatch beetle";
            Body = 242;
			

            SetStr(136, 160);
            SetDex(41, 52);
            SetInt(31, 40);

            SetHits(121, 145);
            SetMana(20);

            SetDamage(5, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 35, 40);
            SetResistance(ResistanceType.Fire, 15, 30);
            SetResistance(ResistanceType.Cold, 15, 30);
            SetResistance(ResistanceType.Poison, 50, 80);
            SetResistance(ResistanceType.Energy, 20, 35);

            SetSkill(SkillName.MagicResist, 50.1, 58.0);
            SetSkill(SkillName.Tactics, 67.1, 77.0);
            SetSkill(SkillName.Wrestling, 50.1, 60.0);
            SetSkill(SkillName.Anatomy, 30.1, 34.0);

            Fame = 1400;
            Karma = -1400;

            switch ( Utility.Random(12) )
            {
                case 0:
                    PackItem(new LeatherGorget());
                    break;
                case 1:
                    PackItem(new LeatherGloves());
                    break;
                case 2:
                    PackItem(new LeatherArms());
                    break;
                case 3:
                    PackItem(new LeatherLegs());
                    break;
                case 4:
                    PackItem(new LeatherCap());
                    break;
                case 5:
                    PackItem(new LeatherChest());
                    break;
            }

            if (Utility.RandomDouble() < .5)
                PackItem(Engines.Plants.Seed.RandomBonsaiSeed());

            Tamable = true;
            MinTameSkill = 41.1;
            ControlSlots = 1;

            SetWeaponAbility(WeaponAbility.CrushingBlow);
            SetSpecialAbility(SpecialAbility.PoisonSpit);
        }

        public DeathwatchBeetle(Serial serial)
            : base(serial)
        {
        }

        public override int Hides
        {
            get
            {
                return 8;
            }
        }

        public override int GetAngerSound()
        {
            return 0x4F3;
        }

        public override int GetIdleSound()
        {
            return 0x4F2;
        }

        public override int GetAttackSound()
        {
            return 0x4F1;
        }

        public override int GetHurtSound()
        {
            return 0x4F4;
        }

        public override int GetDeathSound()
        {
            return 0x4F0;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.LowScrolls, 1);
            AddLoot(LootPack.Potions, 1);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new DeathWatchCarapace());
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

            if (version == 0)
            {
                SetWeaponAbility(WeaponAbility.CrushingBlow);
            }
        }
    }
}
