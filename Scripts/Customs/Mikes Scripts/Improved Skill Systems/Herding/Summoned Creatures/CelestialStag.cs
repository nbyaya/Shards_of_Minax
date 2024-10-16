using System;
using Server.Items;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a celestial stag corpse")]
    public class CelestialStag : BaseCreature
    {
        private DateTime m_NextCelestialLight;
        private DateTime m_NextRadiantAura;
        private DateTime m_RadiantAuraEnd;

        [Constructable]
        public CelestialStag()
            : base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "a celestial stag";
            Body = 0xEA;
            Hue = 1153; // Light blue hue

            SetStr(150);
            SetDex(100);
            SetInt(120);

            SetHits(150);
            SetMana(100);

            SetDamage(10, 15);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 20, 30);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.EvalInt, 80.1, 90.0);
            SetSkill(SkillName.Meditation, 80.1, 90.0);
            SetSkill(SkillName.Magery, 80.1, 90.0);
            SetSkill(SkillName.MagicResist, 70.1, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 70.0);
            SetSkill(SkillName.Wrestling, 60.0, 70.0);

            Fame = 5000;
            Karma = 5000;

            VirtualArmor = 40;

            Tamable = true;
            ControlSlots = 1;
            MinTameSkill = -10;

            m_NextCelestialLight = DateTime.UtcNow;
            m_NextRadiantAura = DateTime.UtcNow;
        }

        public CelestialStag(Serial serial)
            : base(serial)
        {
        }

        public override int Meat { get { return 1; } }
        public override int Hides { get { return 12; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant != null)
            {
                if (DateTime.UtcNow >= m_NextCelestialLight)
                {
                    CastCelestialLight();
                }

                if (DateTime.UtcNow >= m_NextRadiantAura && DateTime.UtcNow >= m_RadiantAuraEnd)
                {
                    ActivateRadiantAura();
                }
            }

            if (DateTime.UtcNow >= m_RadiantAuraEnd && m_RadiantAuraEnd != DateTime.MinValue)
            {
                DeactivateRadiantAura();
            }
        }

        private void CastCelestialLight()
        {
            Mobile target = Combatant as Mobile;
            if (target != null && target.Alive)
            {
                int healAmount = Utility.RandomMinMax(15, 25);
                Hits = Math.Min(Hits + healAmount, HitsMax);
                target.PlaySound(0x1F2);
                target.FixedEffect(0x376A, 10, 16);
                m_NextCelestialLight = DateTime.UtcNow + TimeSpan.FromSeconds(30);
            }
        }

        private void ActivateRadiantAura()
        {
            PublicOverheadMessage(MessageType.Regular, 0x3B2, true, "* Radiant Aura! *");
            PlaySound(0x1ED);
            FixedEffect(0x376A, 10, 16);

            SetStr(Str + 20);
            SetDex(Dex + 20);
            SetInt(Int + 20);

            m_RadiantAuraEnd = DateTime.UtcNow + TimeSpan.FromSeconds(45);
            m_NextRadiantAura = DateTime.UtcNow + TimeSpan.FromMinutes(3);
        }

        private void DeactivateRadiantAura()
        {
            SetStr(Str - 20);
            SetDex(Dex - 20);
            SetInt(Int - 20);

            m_RadiantAuraEnd = DateTime.MinValue;
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

            m_NextCelestialLight = DateTime.UtcNow;
            m_NextRadiantAura = DateTime.UtcNow;
            m_RadiantAuraEnd = DateTime.MinValue;
        }
    }
}
