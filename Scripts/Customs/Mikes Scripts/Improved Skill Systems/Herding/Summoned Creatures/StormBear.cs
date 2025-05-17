using System;

namespace Server.Mobiles
{
    [CorpseName("a storm bear corpse")]
    public class StormBear : BaseCreature
    {
        private DateTime m_NextThunderStrike;

        [Constructable]
        public StormBear()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "a storm bear";
            this.Body = 213; // Using the same body type as PolarBear
            this.BaseSoundID = 0xA3;
            this.Hue = 1360; // Unique blue hue

            this.SetStr(120, 150);
            this.SetDex(85, 110);
            this.SetInt(30, 55);

            this.SetHits(75, 90);
            this.SetMana(0);

            this.SetDamage(8, 14);

            this.SetDamageType(ResistanceType.Physical, 50);
            this.SetDamageType(ResistanceType.Energy, 50);

            this.SetResistance(ResistanceType.Physical, 30, 40);
            this.SetResistance(ResistanceType.Cold, 60, 80);
            this.SetResistance(ResistanceType.Poison, 20, 30);
            this.SetResistance(ResistanceType.Energy, 50, 70);

            this.SetSkill(SkillName.MagicResist, 50.1, 65.0);
            this.SetSkill(SkillName.Tactics, 65.1, 95.0);
            this.SetSkill(SkillName.Wrestling, 50.1, 75.0);

            this.Fame = 2000;
            this.Karma = 0;

            this.VirtualArmor = 22;

            this.Tamable = true;
            this.ControlSlots = 1;
            this.MinTameSkill = -10;

            m_NextThunderStrike = DateTime.UtcNow;
        }

        public StormBear(Serial serial)
            : base(serial)
        {
        }

        public override int Meat
        {
            get
            {
                return 2;
            }
        }
        public override int Hides
        {
            get
            {
                return 16;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Bear;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null && DateTime.UtcNow >= m_NextThunderStrike)
            {
                ThunderStrike();
            }
        }

        private void ThunderStrike()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int damage = Utility.RandomMinMax(10, 20);
                target.Damage(damage, this);
                target.Paralyze(TimeSpan.FromSeconds(2));
                target.PlaySound(0x29);
                target.FixedEffect(0x376A, 10, 16);
                m_NextThunderStrike = DateTime.UtcNow + TimeSpan.FromSeconds(30);
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
