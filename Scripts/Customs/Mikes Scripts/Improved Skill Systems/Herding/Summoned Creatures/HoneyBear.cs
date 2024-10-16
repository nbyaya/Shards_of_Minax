using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a honey bear corpse")]
    public class HoneyBear : BaseCreature
    {
        private DateTime m_NextHoneyHeal;

        [Constructable]
        public HoneyBear()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a honey bear";
            Body = 167;
            BaseSoundID = 0xA3;
            Hue = 1161; // Honey hue

            SetStr(150, 200);
            SetDex(50, 75);
            SetInt(50, 75);

            SetHits(100, 150);
            SetMana(0);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 15, 20);

            SetSkill(SkillName.MagicResist, 35.1, 50.0);
            SetSkill(SkillName.Tactics, 50.1, 70.0);
            SetSkill(SkillName.Wrestling, 50.1, 70.0);

            Fame = 600;
            Karma = 0;

            VirtualArmor = 35;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -18.9;

            m_NextHoneyHeal = DateTime.UtcNow;
        }

        public HoneyBear(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.FruitsAndVegies | FoodType.Meat; } }
        public override PackInstinct PackInstinct { get { return PackInstinct.Bear; } }

        public override void OnThink()
        {
            base.OnThink();

            if (DateTime.UtcNow >= m_NextHoneyHeal)
            {
                HoneyHeal();
            }
        }

        private void HoneyHeal()
        {
            if (Hits < HitsMax)
            {
                Hits += Utility.RandomMinMax(20, 30); // Heal a random amount between 20 and 30
                if (Hits > HitsMax)
                    Hits = HitsMax;

                PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Honey Heal *");
                PlaySound(0x20E); // Sound of eating honey
                FixedEffect(0x375A, 10, 15); // Honey effect
                m_NextHoneyHeal = DateTime.UtcNow + TimeSpan.FromSeconds(30); // Cooldown of 30 seconds
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextHoneyHeal);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextHoneyHeal = reader.ReadDateTime();
        }
    }
}
