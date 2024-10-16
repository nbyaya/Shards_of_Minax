using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a starwolf corpse")]
    public class Starwolf : BaseCreature
    {
        private DateTime m_NextCelestialBoost;
        private DateTime m_CelestialBoostEnd;

        [Constructable]
        public Starwolf()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a starwolf";
            Body = 0xE1;
            BaseSoundID = 0xE5;
            Hue = 1153; // Blue hue

            SetStr(180);
            SetDex(120);
            SetInt(80);

            SetDamage(10, 18);

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Cold, 40);

            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 80.0);
            SetSkill(SkillName.Tactics, 90.0);
            SetSkill(SkillName.Wrestling, 85.0);

            VirtualArmor = 50;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = 90.0;

            m_NextCelestialBoost = DateTime.UtcNow;
        }

        public Starwolf(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 2; } }
        public override int Hides { get { return 10; } }
        public override FoodType FavoriteFood { get { return FoodType.Meat; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextCelestialBoost && DateTime.UtcNow >= m_CelestialBoostEnd)
                {
                    ActivateCelestialBoost();
                }
            }

            if (DateTime.UtcNow >= m_CelestialBoostEnd && m_CelestialBoostEnd != DateTime.MinValue)
            {
                DeactivateCelestialBoost();
            }
        }

        private void ActivateCelestialBoost()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Celestial Boost! *");
            PlaySound(0x1E9);
            FixedEffect(0x376A, 10, 16);

            SetDex(Dex + 30);
            SetDamage(15, 23);

            m_CelestialBoostEnd = DateTime.UtcNow + TimeSpan.FromSeconds(20);
            m_NextCelestialBoost = DateTime.UtcNow + TimeSpan.FromMinutes(3);
        }

        private void DeactivateCelestialBoost()
        {
            SetDex(Dex - 30);
            SetDamage(10, 18);

            m_CelestialBoostEnd = DateTime.MinValue;
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

            m_NextCelestialBoost = DateTime.UtcNow;
            m_CelestialBoostEnd = DateTime.MinValue;
        }
    }
}
