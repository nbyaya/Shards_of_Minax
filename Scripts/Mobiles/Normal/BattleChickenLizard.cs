using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a chicken lizard corpse")]
    public class BattleChickenLizard : BaseCreature
    {
        [Constructable]
        public BattleChickenLizard()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.05, 0.1)
        {
            Name = "a battle chicken lizard";
            Body = 716;

            SetStr(94, 177);
            SetDex(78, 124);
            SetInt(6, 13);

            SetHits(94, 177);

            SetDamage(5, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Fire, 5, 15);

            SetSkill(SkillName.MagicResist, 30.0, 53.0);
            SetSkill(SkillName.Tactics, 50.0, 62.0);
            SetSkill(SkillName.Wrestling, 50.0, 62.0);

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 0.0;
        }

        public override int Meat { get { return 3; } }
        public override MeatType MeatType { get { return MeatType.Bird; } }
        public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }

        public override int GetIdleSound() { return 1511; }
        public override int GetAngerSound() { return 1508; }
        public override int GetHurtSound() { return 1510; }
        public override int GetDeathSound() { return 1509; }

        public override IDamageable Combatant
        {
            get { return base.Combatant; }
            set
            {
                base.Combatant = value;

                if (!Controlled)
                {
                    if (0.05 > Utility.RandomDouble())
                    {
                        StopFlee();
                    }
                    else if (!CheckFlee())
                    {
                        BeginFlee(TimeSpan.FromSeconds(30));
                    }
                }
            }
        }


        public override bool CheckFlee()
        {
            if (Controlled)
            {
                return base.CheckFlee();
            }

            return DateTime.UtcNow < EndFleeTime;
        }

        public override void OnAfterTame(Mobile tamer)
        {
            ActiveSpeed = 0.2;
            PassiveSpeed = 0.4;

            if (Frozen)
                Frozen = false;

            StopFlee();
        }
		
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
			
            if (Utility.RandomDouble() < 0.001) // 1 in 1000 chance
            {
                this.PackItem(new ChickenWranglerSash());
            }
		}

        public BattleChickenLizard(Serial serial)
            : base(serial)
        {
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